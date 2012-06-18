using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.Cms
{
    /// <summary>
    /// Tells what destination folder to use for given resource namespace
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public class WebExtractAttribute : Attribute
    {
        /// <summary>
        /// Source namespace
        /// </summary>
        public string SourceNamespace { get; set; }

        /// <summary>
        /// Target folder
        /// </summary>
        public string TargeFolder { get; set; }

        /// <summary>
        /// Don't extract resource with given namespace
        /// </summary>
        public bool Skip { get; set; }
    }
}
