using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Models;
using Newtonsoft.Json;

namespace Cms.ViewModels
{
    public class WallViewModel
    {
        public string Title { get; set; }

        public float Width { get; set; }

        public IEnumerable<IBrickViewModel<BrickContent>> Bricks { get; set; }

        public string WidthString
        {
            get { return this.Width.ToString("F") + "%"; }
        }

        public string ToJson()
        {
            var properties = new Dictionary<string, object>();
            properties["title"] = this.Title;
            properties["width"] = this.Width;
            return JsonConvert.SerializeObject(properties);
        }
    }
}