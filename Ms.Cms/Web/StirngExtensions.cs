using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StirngExtensions
    {
        /// <summary>
        /// Formats string.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Convert string to HTML id by leaving only a-z, A-Z, -, _ characters.
        /// http://www.w3schools.com/tags/att_standard_id.asp
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns></returns>
        public static string ToHtmlId(this string str)
        {
            return Regex.Replace(str, @"[^a-zA-Z0-9_\-]", "");
        }
    }
}
