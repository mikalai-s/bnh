using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Ms.Cms
{
    /// <summary>
    /// Extracts embedded web resources
    /// </summary>
    public class WebExtractor
    {
        public static void Extract(HttpApplication application)
        {
            var root = application.Server.MapPath("/");
          
            var assembly = Assembly.GetExecutingAssembly();
            var webExtractAttributes = assembly.GetCustomAttributes(typeof(WebExtractAttribute), false)
                .Cast<WebExtractAttribute>()
                .OrderBy(a => -a.SourceNamespace.Length)
                .ToList();

            var resourcesToExtract = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(r => webExtractAttributes.Any(a => r.StartsWith(a.SourceNamespace)))
                .ToList();

            while (resourcesToExtract.Count > 0)
            {
                var resource = resourcesToExtract[0];
                resourcesToExtract.RemoveAt(0);

                var webExtractAttribute = webExtractAttributes.First(a => resource.StartsWith(a.SourceNamespace));

                if (webExtractAttribute.Skip) { continue; }
                
                using (var stream = assembly.GetManifestResourceStream(resource))
                {
                    string folder = Path.Combine(root, webExtractAttribute.TargeFolder);
                    string filePath = Path.Combine(folder, resource.Substring(webExtractAttribute.SourceNamespace.Length + 1));

                    if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
                    if (File.Exists(filePath)) { File.Delete(filePath); }

                    using (var file = File.OpenWrite(filePath))
                    {
                        CopyStream(stream, file);
                    }
                }                
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;

            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}