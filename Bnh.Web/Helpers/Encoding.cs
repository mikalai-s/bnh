using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh
{
    public static class Encoding
    {
        public static string ToBase64(this string input)
        {
            return Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(input));
        }

        public static string FromBase64(this string input)
        {
            return ASCIIEncoding.Default.GetString(Convert.FromBase64String(input));
        }
    }
}
