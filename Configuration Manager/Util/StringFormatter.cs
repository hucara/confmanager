using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Configuration_Manager.Util
{
    static class StringFormatter
    {
        // The main token for format selection is 
        // ##This##

        public static string FormatText(string text, string format)
        {
            if (text == null || text == "") return text;
            if (format == null || format == "") return text;

            int startToken = 0;
            int startValue = 0;
            int endToken = 0;
            int endValue = 0;
            String t = text;
            String value = "";
            String token;

            token = GetToken(format);
            if (token == "") return text;

            startToken = format.IndexOf(token);
            endToken = startToken + 8;

            // The token and the value start at the same index.
            // But we do not know when the value ends.
            String rest = format.Substring(endToken);

            if (rest != "")
            {
                for (int i = startToken; i < t.Length; i++)
                {
                    if (t.Substring(i) == rest)
                    {
                        System.Diagnostics.Debug.WriteLine(t.Substring(i));
                        endValue = i;
                        break;
                    }
                }
                value = t.Substring(startToken, endValue - startToken);
            }
            else
                value = t.Substring(startToken);

            return value;
        }

        //public static String FormatText(String text, String format)
        //{
        //    if (text == null && text == "") return text;
        //    if (format == null && format == "") return text;

        //    int startToken = 0;
        //    int endToken = 0;
        //    int endValue = 0;
        //    String regexText = text;
        //    String regFormat = format;
        //    String value = "";
        //    String token;

        //    regexText = WildcardsToRegex(text);
        //    regFormat = WildcardsToRegex(format);
        //    System.Diagnostics.Debug.WriteLine("Regexized text: " + regexText);

        //    Regex rg = new Regex(regexText);

        //    MatchCollection mc = rg.Matches(format);
        //    return value;
        //}

        private static String GetToken(string format)
        {
            int start = format.IndexOf("##This##");

            if (start < 0)
                return "";
            else
                return format.Substring(start, 8);
        }

        private static int GetStartingStaticIndex(String text, String format)
        {
            int index = 0;
            char wildcardTailChar = 't';

            index = format.IndexOf('*');
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '*')
                {
                    wildcardTailChar = format[i + 1];
                    break;
                }
            }

            return index;
        }

        private static String WildcardsToRegex(string pattern)
        {
            string s = "^" + Regex.Escape(pattern) + "$";
            s = Regex.Replace(s, @"(?<!\\)\\\*", @".*"); // Negative Lookbehind
            s = Regex.Replace(s, @"\\\\\\\*", @"\*");
            s = Regex.Replace(s, @"(?<!\\)\\\?", @".");  // Negative Lookbehind
            s = Regex.Replace(s, @"\\\\\\\?", @"\?");
            return Regex.Replace(s, @"\\\\\\\\", @"\\"); 
        }
    }
}
