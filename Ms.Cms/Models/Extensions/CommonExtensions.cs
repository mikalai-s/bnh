using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Ms.Cms.Models
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Tries to resolve object type defined in current assembly.
        /// It is used to avoid EF proxy types .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static Type GetTypeNonProxy(this object obj)
        {
            // Avoid EF proxies and get object type defined in current assembly
            var executingAssembly = Assembly.GetExecutingAssembly();
            var baseType = obj.GetType();
            while (baseType != null && baseType.Assembly != executingAssembly)
            {
                baseType = baseType.BaseType;
            }
            return baseType;
        }
    }
}