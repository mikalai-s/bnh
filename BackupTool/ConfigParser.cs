using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace BackupTool
{
    internal partial class Config
    {
        public static Config Parse(string path)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));

            config.CurrentDateTime = DateTime.UtcNow.ToString("u").Replace(":", "-");

            // setup dependencies
            var deps = new Dictionary<string, string[]>();
            var type = typeof (Config);
            foreach (var field in type.GetMembers(BindingFlags.Instance | BindingFlags.Public).OfType<FieldInfo>())
            {
                if (field.FieldType == typeof(string))
                {
                    var parameters = GetParameterNames((string) field.GetValue(config)).ToArray();
                    deps[field.Name] = parameters;
                }
                else
                {
                    deps[field.Name] = new string[] {};
                }
            }

            // sort dependencies so eveluate lest dependent first
            var sorted = TopologicalSort(deps.Keys, s => deps[s]).ToArray();

            // resolve config values
            foreach (var fieldName in sorted)
            {
                var field = type.GetField(fieldName);
                var value = field.GetValue(config);
                if (field.FieldType == typeof (string))
                {
                    var svalue = (string) value;
                    foreach (var dependency in deps[fieldName])
                    {
                        svalue = svalue.Replace("{" + dependency + "}", type.GetField(dependency).GetValue(config).ToString());
                    }
                    field.SetValue(config, svalue);
                }
            }
            
            return config;
        }

        internal static IEnumerable<string> GetParameterNames(string field)
        {
            StringBuilder parameter = null;
            foreach (var c in field)
            {
                if (parameter != null)
                {
                    if (c == '}')
                    {
                        yield return parameter.ToString();
                        parameter = null;
                    }
                    else
                    {
                        parameter.Append(c);
                    }
                }

                if (c == '{')
                {
                    parameter = new StringBuilder();
                }
            }

        }


        internal static IEnumerable<T> TopologicalSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies);

                sorted.Add(item);
            }
        }
    }
}
