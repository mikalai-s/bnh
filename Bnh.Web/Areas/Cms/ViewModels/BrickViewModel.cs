using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Models;
using Bnh.Cms.Repositories;
using Newtonsoft.Json;

namespace Bnh.Cms.ViewModels
{
    public interface IBrickViewModel<out T> where T : BrickContent
    {
        string Title { get; }

        float Width { get; }

        string BrickContentId { get; }

        T Content { get; }

        string WidthString { get; }

        string ToJson();
    }

    public class BrickViewModel<T> : IBrickViewModel<T> where T : BrickContent
    {
        protected CmsRepos Repos { get; private set; }

        protected ViewModelContext Context { get; private set; }

        public string Title { get; private set; }

        public float Width { get; private set; }

        public string BrickContentId { get; private set; }

        public T Content { get; private set; }

        public string WidthString
        {
            get { return this.Width.ToString("F") + "%"; }
        }

        public BrickViewModel(ViewModelContext context, string title, float width, string brickContentId, T content, CmsRepos repos)
        {
            this.Title = title;
            this.Width = width;
            this.BrickContentId = brickContentId;
            this.Content = content;
            this.Repos = repos;
        }


        public string ToJson()
        {
            var properties = new Dictionary<string, object>();
            properties["title"] = this.Title;
            properties["width"] = this.Width;
            properties["brickcontentid"] = this.BrickContentId;
            return JsonConvert.SerializeObject(properties);
        }

        internal static IBrickViewModel<BrickContent> Create(ViewModelContext context, Brick brick, CmsRepos db)
        {
            var content = brick.GetContent(db);
            if (content is ReviewsContent)
            {
                return (IBrickViewModel<BrickContent>)new ReviewsBrickViewModel(context, brick.Title, brick.Width, brick.BrickContentId, (ReviewsContent)brick.GetContent(db), db);
            }
            else
            {
                var viewModelType = typeof(BrickViewModel<>).MakeGenericType(content.GetType());

                return (IBrickViewModel<BrickContent>)Activator.CreateInstance(viewModelType, context, brick.Title, brick.Width, brick.BrickContentId, brick.GetContent(db), db);
            }
        }

        public static IBrickViewModel<BrickContent> Prototype
        {
            get
            {
                return new BrickViewModel<BrickContent>(null, string.Empty, 100.0F, string.Empty, null, null);
            }
        }
    }
}