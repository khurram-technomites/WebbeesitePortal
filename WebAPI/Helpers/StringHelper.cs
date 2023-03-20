using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public static class StringHelper
    {
        public static string RemoveSpecialCharacters(this string input)
        {
            string pattern = $"[@&'(\\s)<>#?;,.*+=~`$_]";
            return Regex.Replace(input, pattern, "");
        }
    }
}
