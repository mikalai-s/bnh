using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Cms.Models;
using Bnh.Core;
using HtmlAgilityPack;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Search.Vectorhighlight;
using Lucene.Net.Store;
using MongoDB.Driver.Linq;
using Cms.Core;

namespace Bnh.Infrastructure.Search
{
    public class SearchProvider : ISearchProvider
    {
        IBnhRepositories repos { get; set; }
        IConfig config { get; set; }
        IPathMapper pathMapper { get; set; }

        private const Lucene.Net.Util.Version LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        SnowballAnalyzer analyzer = new SnowballAnalyzer(LuceneVersion, "English");

        public SearchProvider(IBnhRepositories repos, IConfig config, IPathMapper pathMapper)
        {
            this.repos = repos;
            this.config = config;
            this.pathMapper = pathMapper;
        }

        public void RebuildIndex()
        {
            // state the file location of the index
            var path = this.pathMapper.Map(this.config.SearchIndexFolder);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            // remove all files in the folder
            var dir = FSDirectory.Open(path);
            System.IO.Directory.GetFiles(path).ToList().ForEach(dir.DeleteFile);

            // create the index writer with the directory and analyzer defined.
            using (var indexWriter = new IndexWriter(dir, this.analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                IndexCommunities(indexWriter);
                IndexReviews(indexWriter);

                //optimize and close the writer
                indexWriter.Optimize();
            }
        }

        private void IndexCommunities(IndexWriter indexWriter)
        {
            throw new NotImplementedException();
            // all content IDs
            //var sceneWithBricks = this.repos.Scenes
            //    .ToList()
            //    .ToDictionary(
            //        s => s.SceneId,
            //        s => s.Walls
            //            .SelectMany(w => w.Bricks)
            //            .OfType<HtmlBrick>());

            //foreach (var group in sceneWithBricks)
            //{
            //    foreach (var brick in group.Value)
            //    {
            //        if (brick.Html.IsEmpty()) { continue; }

            //        // create a document, add in a single field
            //        var doc = new Document();

            //        doc.Add(new Field("community-id", group.Key, Field.Store.YES, Field.Index.NO));
            //        doc.Add(new Field("content-id", brick.BrickId, Field.Store.YES, Field.Index.NO));
            //        doc.Add(new Field("type", "community", Field.Store.YES, Field.Index.NO));
            //        doc.Add(new Field("content", EscapeHtml(brick.Html), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

            //        // write the document to the index
            //        indexWriter.AddDocument(doc);
            //    }
            //}
        }

        private void IndexReviews(IndexWriter indexWriter)
        {
            // TODO: implement when there is need
        }

        private string EscapeHtml(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            return doc.DocumentNode.InnerText;
        }

        public IEnumerable<ISearchResult> Search(string query)
        {
            //create an index searcher that will perform the search
            var dir = FSDirectory.Open(this.pathMapper.Map(this.config.SearchIndexFolder));

            var reader = default(IndexReader);
            var searcher = default(IndexSearcher);
            try
            {
                reader = IndexReader.Open(dir, true);
                searcher = new IndexSearcher(reader);
            }
            catch (FileNotFoundException)
            {
                // Index wasn't build yet
                this.RebuildIndex();

                reader = IndexReader.Open(dir, true);
                searcher = new IndexSearcher(reader);
            }

            using (searcher)
            {
                var parser = new QueryParser(LuceneVersion, "content", this.analyzer);

                var que = parser.Parse(query);
                var results = searcher.Search(que, 10).ScoreDocs;

                var highlighter = new FastVectorHighlighter(true, true, new SimpleFragListBuilder(), new MyFRagmentBuilder());
                var fieldQuery = highlighter.GetFieldQuery(que);
                //var formatter = new SimpleHTMLFormatter("<strong>", "</strong>");
                //var highlighter = new Highlighter(formatter, new QueryScorer(searcher.Rewrite(que)));
               

                foreach (var scoreDoc in results)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);

                    yield return new CommunitySearchResult
                    {
                        CommunityId = doc.Get("community-id"),
                        ContentId = doc.Get("content-id"),
                        Fragments = highlighter.GetBestFragments(fieldQuery, reader, scoreDoc.Doc, "content", 20, 5)
                        //Fragments = highlighter.GetBestFragments(analyzer, "content", doc.Get("content"), 5)
                    };
                }
            }
        }
    }

 
    // TODO: fix large sentences
    public class MyFRagmentBuilder : BaseFragmentsBuilder
    {
        protected override string MakeFragment(System.Text.StringBuilder buffer, int[] index, Field[] values, FieldFragList.WeightedFragInfo fragInfo)
        {
            var builder = new FragmentBuilder(values[0].StringValue, GetField<int>(fragInfo, "startOffset"), GetField<int>(fragInfo, "endOffset"));

            var subInfos = GetField<IEnumerable>(fragInfo, "subInfos");
            foreach(var subInfoObject in subInfos)
            {
                var subInfoseqnum = GetField<int>(subInfoObject, "seqnum");

                var phrases = GetField<IEnumerable<FieldPhraseList.WeightedPhraseInfo.Toffs>>(subInfoObject, "termsOffsets")
                    .Select(to => Tuple.Create(GetField<int>(to, "startOffset"), GetField<int>(to, "endOffset")));

                builder.BuildPhrases(phrases, this.GetPreTag(subInfoseqnum), this.GetPostTag(subInfoseqnum));
            }

            return builder.ToString();
        }

        private T GetField<T>(object o, string name)
        {
            return (T)o.GetType().GetField(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(o);
        }

        public override List<FieldFragList.WeightedFragInfo> GetWeightedFragInfoList(List<FieldFragList.WeightedFragInfo> src)
        {
            return src;
        }
    }

    public class FragmentBuilder
    {
        private int startOffset;
        private int endOffset;
        private StringBuilder fragment;
        private int srcIndex;
        private string text;

        public FragmentBuilder(string text, int start, int end)
        {
            var stopChars = new[] { '.', '!', '?' };

            this.startOffset = start;
            this.endOffset = end;

            while (startOffset > 0 && !stopChars.Any(c => c == text[startOffset]))
            {
                startOffset--;
            }

            if (startOffset > 0)
            {
                startOffset++;
            }

            while (endOffset < text.Length && !stopChars.Any(c => c == text[endOffset]))
            {
                endOffset++;
            }

            if (endOffset < text.Length - 1)
            {
                endOffset++;
            }
            else if (endOffset > text.Length - 1)
            {
                endOffset = text.Length - 1;
            }


            var le = endOffset - startOffset + 1;
            this.text = text.Substring(startOffset, (startOffset + le) <= text.Length ? le : text.Length - startOffset);


            this.fragment = new StringBuilder();
            this.srcIndex = 0;
        }
    
        internal void BuildPhrases(IEnumerable<Tuple<int,int>> phrases,string preTag,string postTag)
        {
            var s = this.startOffset;

            foreach(var t in phrases)
            {
                var tostartOffset = t.Item1;
                var toendOffset = t.Item2;

                fragment
                    .Append(text.Substring(srcIndex, (tostartOffset - s) - srcIndex))
                    .Append(preTag)
                    .Append(text.Substring(tostartOffset - s, (toendOffset - s) - (tostartOffset - s)))
                    .Append(postTag);
                srcIndex = toendOffset - s;
            }
        }

        public override string ToString()
        {
            fragment.Append(text.Substring(srcIndex));
            return fragment.ToString();
        }
    }

    public class MyWeightedFragInfo
    {
        // Fields
        public int endOffset;
        public int startOffset;
        public List<SubInfo> subInfos;
        public float totalBoost;           

        // Nested Types
        public class SubInfo
        {
            // Fields
            public int seqnum;
            public List<FieldPhraseList.WeightedPhraseInfo.Toffs> termsOffsets;
            public string text;
        }
    }
}