using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.Helpers;
using Cms.Models;
using Newtonsoft.Json;

namespace Cms.ViewModels
{
    public interface IBrickViewModel<out T> where T : Brick, new()
    {
        string Title { get; set; }

        bool ShowTitle { get; set; }

        float Width { get; set; }

        string BrickId { get; set; }

        T Content { get; }

        string WidthString { get; }

        string ToJson();

        string GetHtmlId();

        string GetBrickView(HttpServerUtilityBase server);

        string GetBrickEditView(HttpServerUtilityBase server);
    }

    public class BrickViewModel<T> : IBrickViewModel<T> where T : Brick, new()
    {

        protected ViewModelContext Context { get; private set; }

        public string Title { get; set; }

        public bool ShowTitle { get; set; }

        public float Width { get; set; }

        public string BrickId { get; set; }

        public T Content { get; private set; }

        public string WidthString
        {
            get { return this.Width.ToString("F") + "%"; }
        }


        static Dictionary<Type, Type> ViewModelMap = new Dictionary<Type, Type>();

        static BrickViewModel()
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().OrderBy(t => t.Name).ToList();
            var contents = types.Where(t => typeof(Brick).IsAssignableFrom(t)).ToList();

            foreach (var contentType in contents)
            {
                ViewModelMap[contentType] = 
                    types.SingleOrDefault(t => t.Name == contentType.Name.TrimEnd("Content".ToCharArray()) + "ViewModel") ??
                    typeof(BrickViewModel<>).MakeGenericType(contentType);
            }
        }

        public static Type Map(Type brickType)
        {
            return ViewModelMap[brickType];

        }

        public BrickViewModel(ViewModelContext context, T brick)
        {
            this.Content = brick ?? new T();

            this.Title = this.Content.Title;
            this.ShowTitle = this.Content.IsTitleVisible;
            this.Width = this.Content.Width;
            this.BrickId = this.Content.BrickId;
        }


        public string ToJson()
        {
            var properties = new Dictionary<string, object>();
            properties["title"] = this.Title;
            properties["width"] = this.Width;
            properties["brickId"] = this.BrickId;
            properties["brickType"] = typeof(T).Name;
            return JsonConvert.SerializeObject(properties);
        }

        /// <summary>
        /// Gets brick title converted to HTML id string
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public string GetHtmlId()
        {
            return this.Title.IsEmpty() ? this.BrickId.ToHtmlId() : this.Title.ToHtmlId();
        }



        /// <summary>
        /// Returns view for current brick content
        /// </summary>
        /// <param name="Brick"></param>
        /// <returns></returns>
        public string GetBrickView(HttpServerUtilityBase server)
        {
            var brickView = ContentUrl.Views.Brick.Partial.GetView(typeof(T));
            return System.IO.File.Exists(server.MapPath(brickView))
                ? brickView
                : ContentUrl.Views.Brick.Partial.GetView(typeof(Brick));
        }

        /// <summary>
        /// Returns view for current brick content
        /// </summary>
        /// <param name="Brick"></param>
        /// <returns></returns>
        public string GetBrickEditView(HttpServerUtilityBase server)
        {
            var brickView = ContentUrl.Views.Brick.Partial.GetEdit(typeof(T));
            return System.IO.File.Exists(server.MapPath(brickView))
                ? brickView
                : ContentUrl.Views.Brick.Partial.GetView(typeof(Brick));
        }

        internal static IBrickViewModel<Brick> Create(ViewModelContext context, Brick brick)
        {
            return (IBrickViewModel<Brick>)Activator.CreateInstance(Map(brick.GetType()), context, brick);
        }

        internal static IBrickViewModel<Brick> Create(Type brickType)
        {
            return (IBrickViewModel<Brick>)Activator.CreateInstance(Map(brickType), (object)null, (object)null);
        }

        public static IBrickViewModel<Brick> Prototype
        {
            get
            {
                return new BrickViewModel<Brick>(null, new Brick
                {
                    Title = string.Empty,
                    Width = 100.0F,
                    BrickId = string.Empty
                });
            }
        }
    }
}