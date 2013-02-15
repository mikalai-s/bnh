using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.Models;
using Newtonsoft.Json;

namespace Cms.ViewModels
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

        protected ViewModelContext Context { get; private set; }

        public string Title { get; private set; }

        public float Width { get; private set; }

        public string BrickContentId { get; private set; }

        public T Content { get; private set; }

        public string WidthString
        {
            get { return this.Width.ToString("F") + "%"; }
        }

        static BrickViewModel()
        {
            // TODO: make discionary of brick content and brick view model assignments
            // to simplify Create() method below
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().OrderBy(t => t.Name).ToList();
            var contents = types.Where(t => t.IsAssignableFrom(typeof(BrickContent))).ToList();
            var viewModels = types.Where(t => t.IsAssignableFrom(typeof(BrickViewModel<>))).ToList();
        }

        public BrickViewModel(ViewModelContext context, string title, float width, string brickContentId, T content)
        {
            this.Title = title;
            this.Width = width;
            this.BrickContentId = brickContentId;
            this.Content = content;
        }


        public string ToJson()
        {
            var properties = new Dictionary<string, object>();
            properties["title"] = this.Title;
            properties["width"] = this.Width;
            properties["brickcontentid"] = this.BrickContentId;
            return JsonConvert.SerializeObject(properties);
        }

        internal static IBrickViewModel<BrickContent> Create(ViewModelContext context, Brick brick, BrickContent content)
        {
            if (content is ReviewsContent)
            {
                return (IBrickViewModel<BrickContent>)new ReviewsBrickViewModel(context, brick.Title, brick.Width, brick.BrickContentId, (ReviewsContent)content);
            }
            else if (content is RatingContent)
            {
                return (IBrickViewModel<BrickContent>)new RatingViewModel(context, brick.Title, brick.Width, brick.BrickContentId, (RatingContent)content);
            }
            else
            {
                var viewModelType = typeof(BrickViewModel<>).MakeGenericType(content.GetType());

                return (IBrickViewModel<BrickContent>)Activator.CreateInstance(viewModelType, context, brick.Title, brick.Width, brick.BrickContentId, content);
            }
        }

        public static IBrickViewModel<BrickContent> Prototype
        {
            get
            {
                return new BrickViewModel<BrickContent>(null, string.Empty, 100.0F, string.Empty, null);
            }
        }
    }
}