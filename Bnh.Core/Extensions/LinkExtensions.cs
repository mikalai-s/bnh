using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return (source == null) || !source.Any();
        }
    }
}
