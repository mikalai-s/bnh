using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Ms.Cms.ViewModels
{
    public class WallViewModel
    {
        public string Title { get; set; }

        public float Width { get; set; }

        public IEnumerable<BrickViewModel> Bricks { get; set; }

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