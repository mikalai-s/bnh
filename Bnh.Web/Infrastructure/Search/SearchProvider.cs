using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Bnh.Cms.Models;
using Bnh.Core;
using HtmlAgilityPack;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Store;
using MongoDB.Driver.Linq;

namespace Bnh.Web.Infrastructure.Search
{
    public class SearchProvider : ISearchProvider
    {
        IEntityRepositories Entities { get; set; }
        CmsEntities Cms { get; set; }
        Config Config { get; set; }
        IPathMapper PathMapper { get; set; }

        public SearchProvider(IEntityRepositories entites, CmsEntities cms, Config config, IPathMapper pathMapper)
        {
            this.Entities = entites;
            this.Cms = cms;
            this.Config = config;
            this.PathMapper = pathMapper;
        }

        public void RebuildIndex()
        {
            // all content IDs
            var contentWithScenes = from scene in this.Cms.Scenes.ToList()
                                    from wall in scene.Walls
                                    from brick in wall.Bricks
                                    select new
                                    {
                                        SceneId = scene.SceneId,
                                        ContentId = brick.BrickContentId
                                    };
            var contentIds = contentWithScenes.Select(c => c.ContentId);

            // all HTML brick contents to index
            var contents = this.Cms.BrickContents
                .OfType<HtmlContent>()
                .AsQueryable()
                .Where(b => b.BrickContentId.In(contentIds))
                .ToList();

            // group them by their scenes
            var contentGroups = from contentWithScene in contentWithScenes
                                from content in contents
                                where contentWithScene.ContentId == content.BrickContentId
                                group content by contentWithScene.SceneId;
            // var dictionary = contentGroups.ToDictionary(c => c.Key, c => c.ToList());

            // NOTE: we indexing only communities now

            // state the file location of the index
            var path = this.PathMapper.Map(this.Config.SearchIndexFolder);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var dir = FSDirectory.Open(path);

            // create an analyzer to process the text
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            // create the index writer with the directory and analyzer defined.
            using (var indexWriter = new IndexWriter(dir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var groups in contentGroups)
                {
                    foreach (var content in groups)
                    {
                        // create a document, add in a single field
                        var doc = new Document();

                        doc.Add(new Field("community-id", groups.Key, Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("content-id", content.BrickContentId, Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("type", "community", Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("content", EscapeHtml(content.Html), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));

                        // write the document to the index
                        indexWriter.AddDocument(doc);
                    }
                }

                //optimize and close the writer
                indexWriter.Optimize();
            }
        }

        private string EscapeHtml(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            return doc.DocumentNode.InnerText;
        }

        public IEnumerable<ISearchResult> Search(string query)
        {
            // create an analyzer to process the text
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            //create an index searcher that will perform the search
            var dir = FSDirectory.Open(this.PathMapper.Map(this.Config.SearchIndexFolder));

            IndexSearcher searcher = null;
            try
            {
                searcher = new IndexSearcher(dir);
            }
            catch (FileNotFoundException)
            {
                // Index wasn't bulid yet
                this.RebuildIndex();

                searcher = new IndexSearcher(dir);
            }

            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "content", analyzer);

            var que = parser.Parse(query);
            var results = searcher.Search(que, 10).ScoreDocs;

            var formatter = new SimpleHTMLFormatter("<strong>", "</strong>");
            var highlighter = new Highlighter(formatter, new QueryScorer(searcher.Rewrite(que)));

            foreach (var scoreDoc in results)
            {
                var doc = searcher.Doc(scoreDoc.Doc);

                yield return new SearchResult
                {
                    CommunityId = doc.Get("community-id"),
                    ContentId = doc.Get("content-id"),
                    Content = highlighter.GetBestFragment(analyzer, "content", doc.Get("content")) 
                };
            }
        }
    }
}