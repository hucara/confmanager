using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Configuration_Manager.Util
{
    static class StringFormatter
    {
        const string TOKEN = "##This##";

        public static string FormatText(string text, string format)
        {
            if (text == null || text == "") return text;
            if (format == null || format == "") return text;

            int startValue = 0;
            int tokenEnd = 0;
            int valueEnd = 0;
            String translated = text;
            String value = "";

            int tokenStart = format.IndexOf(TOKEN);
            if (tokenStart == -1) return text;

            if (CheckBeginning(text, format, tokenStart))
                valueEnd = GetValueEnd(text, format, tokenStart);
            else valueEnd = -1;

            if (valueEnd - tokenStart > 0 && CheckEnd(text, format, tokenStart, valueEnd))
                return text.Substring(tokenStart, valueEnd - tokenStart);
            else if (valueEnd - tokenStart <= 0 && CheckEnd(text, format, tokenStart, valueEnd))
                return text.Substring(tokenStart);
            else
                return text;

            return "";
        }

        private static bool CheckBeginning(string text, string format, int tokenStart)
        {
            bool res = true;
            String textBegin = text.Substring(0, tokenStart);
            String formatBegin = format.Substring(0, format.IndexOf(TOKEN));

            for (int i = 0; i < textBegin.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine("Index: " + i + " #:" + textBegin[i] + " #:" + formatBegin[i]);
                if (textBegin[i] != formatBegin[i] && formatBegin[i] != '?')
                {
                    res = false; 
                    break;
                }
            }
            System.Diagnostics.Debug.WriteLine("Beginning: " + res);
            return res;
        }

        private static int GetValueEnd(string text, string format, int tokenStart)
        {
            int valueEnd = -1;
            string restText = text.Substring(tokenStart);
            string restFormat = format.Substring(tokenStart + 8);

            if (restFormat != "" && restText != "")
            {
                for (int i = tokenStart; i < restText.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine("# " + restText.Substring(i));
                    if (restText.Substring(i).Length == restFormat.Length)
                    {
                        valueEnd = restText.Substring(i).Length;
                        break;
                    }
                }
            }
            return valueEnd + tokenStart;
        }

        private static bool CheckEnd(string text, string format, int tokenStart, int valueEnd)
        {
            bool result = true;
            int shorterLenght = (text.Length < format.Length) ? text.Length : format.Length;
            String textEnding = text.Substring(valueEnd);
            String formatEnding = format.Substring(tokenStart + 8);

            System.Diagnostics.Debug.WriteLine("Ending: " + result);
            return result;
        }

        //private static int getvalueend(string text, string format, int tokenstart)
        //{
        //    string rest = format.substring(tokenstart + 8);
        //    int index = -1;

        //    if (rest != "")
        //    {
        //        for (int i = tokenstart; i < text.length; i++)
        //        {
        //            if (text.substring(i) == rest)
        //            {
        //                system.diagnostics.debug.writeline(text.substring(i));
        //                index = i;
        //                break;
        //            }
        //        }
        //    }
        //    else if (tokenstart + 8 >= format.length) return text.length;
        //    return index;
        //}

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
