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

        static int valueStart;
        static int tokenStart;
        static int tokenEnd;
        static int valueEnd;

        static String text;
        static String format;
        static String result;

        static String tokenStartDelimiter;
        static String tokenEndDelimiter;

        static String formatHead;
        static String formatTail;
        static String textHead;
        static String textTail;

        /// <summary>
        /// Returns a string of the stext parametter selecting it with the sformat string parametter.
        /// To select the desired value, specify it with the token '##this##'.
        /// The use of wildcards '?' and '*' is allowed before and after the selecting token,
        /// but the combination of both in one side will return the original text.
        /// </summary>
        /// <param name="stext">Text to be formatted.</param>
        /// <param name="sformat">Selection format string.</param>
        /// <returns>Selected text.</returns>
        public static string GetFormattedText(string stext, string sformat)
        {
            result = stext;
            text = stext;
            format = sformat;

            if (text == String.Empty || text == "") return text;
            if (format == String.Empty || format == "") return text;

            GetLimits();
            PrintInfo();

            if (!String.IsNullOrEmpty(textTail))
                text = text.Remove(valueEnd);

            if (!String.IsNullOrEmpty(textHead))
                text = text.Remove(0, valueStart);


            //if (!String.IsNullOrEmpty(textHead))
            //    text = text.Remove(0, valueStart);

            //if (!String.IsNullOrEmpty(textTail))
            //{
            //    int index = text.IndexOf(textTail);
            //    text = text.Remove(index);
            //}


            //if (!String.IsNullOrEmpty(textHead))
            //    text = text.Replace(textHead, "");

            //if (!String.IsNullOrEmpty(textTail))
            //    text = text.Replace(textTail, "");


            System.Diagnostics.Debug.WriteLine("Text: " + stext);
            System.Diagnostics.Debug.WriteLine("Format: " + format);
            System.Diagnostics.Debug.WriteLine("Result: " + text);
            System.Diagnostics.Debug.WriteLine("**                 **");

            return text;

        }

        /// <summary>
        /// Allows to replace a string or a value inside a string.
        /// To select the desired value, specify it with the token '##this##'.
        /// The use of wildcards '?' and '*' is allowed before and after the selecting token,
        /// but the combination of both in one side will return the original text.
        /// </summary>
        /// <param name="newValue">New value for replacement.</param>
        /// <param name="oldText">Old string of text.</param>
        /// <param name="sformat">Format of the string to replace.</param>
        /// <returns>The new string.</returns>
        public static string GetUnFormattedText(string newValue, string oldText, string sformat)
        {
            if (String.IsNullOrEmpty(newValue)) return oldText;
            if (String.IsNullOrEmpty(oldText)) return oldText;
            if (String.IsNullOrEmpty(sformat)) return oldText;

            text = oldText;
            format = sformat;

            GetLimits();
            PrintInfo();

            //string oldValue = oldText.Substring(valueStart, valueEnd - valueStart);

            string oldValue = oldText;

            if (!String.IsNullOrEmpty(textHead))
                oldValue = oldValue.Replace(textHead, "");

            if (!String.IsNullOrEmpty(textTail))
                oldValue = oldValue.Replace(textTail, "");

            //string newText = oldText.Replace(oldValue, newValue);
            string newText = textHead + newValue + textTail;

            System.Diagnostics.Debug.WriteLine("Text: " + text);
            System.Diagnostics.Debug.WriteLine("Format: " + format);
            System.Diagnostics.Debug.WriteLine("Result: " + newText);
            System.Diagnostics.Debug.WriteLine("**                 **");

            return newText;
        }

        private static bool CheckHeadMatches()
        {
            if (String.IsNullOrEmpty(textHead))
                return false;
            else return true;
        }

        private static bool CheckTailMatches()
        {
            if (String.IsNullOrEmpty(textTail))
                return false;
            else return true;
        }

        private static void GetLimits()
        {
            tokenStart = GetTokenStartIndex();
            tokenEnd = GetTokenEndIndex();

            textHead = GetTextHead(text, format);
            textTail = GetTextTail(text, format);

            valueStart = textHead.Length;
            valueEnd = text.Length - textTail.Length;

            formatHead = GetFormatHead();
            formatTail = GetFormatTail();
        }

        private static void SetValueEnd()
        {
            tokenEndDelimiter = GetTokenEndDelimiter();

            if (tokenEndDelimiter == "")
            {
                int count = 0;
                for (int i = 0; i < formatTail.Length; i++)
                    if (formatTail[i] != '*') count++;

                valueEnd = text.Length - count;
            }
            else
                valueEnd = GetValueEndIndex();
        }

        private static void SetValueStart()
        {
            tokenStartDelimiter = GetTokenStartDelimiter();

            if (tokenStartDelimiter == "")
                valueStart = formatHead.Length;
            else
                valueStart = GetValueStartIndex();
        }

        private static string GetFormatTail()
        {
            return format.Substring(tokenEnd + 1); //+1
        }

        private static string GetFormatHead()
        {
            return format.Substring(0, tokenStart);
        }

        private static void PrintInfo()
        {
            System.Diagnostics.Debug.WriteLine("**                 **");
            System.Diagnostics.Debug.WriteLine("-- Formatting info --");
            System.Diagnostics.Debug.WriteLine("tokenStart : " + tokenStart);
            System.Diagnostics.Debug.WriteLine("tokenEnd : " + tokenEnd);
            System.Diagnostics.Debug.WriteLine("tokenStartDelimiter [" + tokenStartDelimiter + "]");
            System.Diagnostics.Debug.WriteLine("tokenEndDelimiter [" + tokenEndDelimiter + "]");
            System.Diagnostics.Debug.WriteLine("valueStart : " + valueStart);
            System.Diagnostics.Debug.WriteLine("valueEnd : " + valueEnd);
            System.Diagnostics.Debug.WriteLine("formatHead [" + formatHead + "]");
            System.Diagnostics.Debug.WriteLine("formatTail [" + formatTail + "]");
            System.Diagnostics.Debug.WriteLine("textHead [" + textHead + "]");
            System.Diagnostics.Debug.WriteLine("textTail [" + textTail + "]");
            System.Diagnostics.Debug.WriteLine("-- -- -- -- -- -- --");
        }

        private static int GetTokenStartIndex()
        {
            return format.IndexOf(TOKEN, StringComparison.OrdinalIgnoreCase);
        }

        private static int GetTokenEndIndex()
        {
            return tokenStart + TOKEN.Length - 1;
        }

        private static string GetTokenStartDelimiter()
        {
            String startText = format.Substring(0, tokenStart);
            String delimiter = "";

            if (startText != "")
            {
                for (int i = startText.Length - 1; i > -1; i--)
                {
                    if (startText[i] != '?' && startText[i] != '*')
                        delimiter = startText[i] + delimiter;
                    else
                        break;
                }
            }
            return delimiter;
        }

        private static string GetTokenEndDelimiter()
        {
            String endText = format.Substring(tokenEnd);
            String delimiter = "";

            if (endText != "")
            {
                for (int i = 1; i < endText.Length; i++)
                {
                    if (endText[i] != '?' && endText[i] != '*')
                        delimiter = delimiter + endText[i];
                    else
                        break;
                }
            }
            return delimiter;
        }

        private static int GetValueStartIndex()
        {
            if (tokenStartDelimiter == "")
                return 0;
            else
                return textHead.Length;
        }

        private static int GetValueEndIndex()
        {
            if (tokenEndDelimiter == "")
                return text.Length;
            else
                return text.IndexOf(textTail);

        }

        private static String WildcardsToRegex(string pattern)
        {
            //string s = "^" + Regex.Escape(pattern) + "$";
            string s = Regex.Escape(pattern);
            s = Regex.Replace(s, @"(?<!\\)\\\*", @".*"); // Negative Lookbehind
            s = Regex.Replace(s, @"\\\\\\\*", @"\*");
            s = Regex.Replace(s, @"(?<!\\)\\\?", @".");  // Negative Lookbehind
            s = Regex.Replace(s, @"\\\\\\\?", @"\?");
            return Regex.Replace(s, @"\\\\\\\\", @"\\");
        }

        public static void SearchMatches(string text, string format)
        {
            string fHead, fTail;
            int index = format.IndexOf(TOKEN, StringComparison.OrdinalIgnoreCase);

            fHead = format.Substring(0, index);
            fTail = format.Substring(index + 8);

            if (fHead.Contains('?') || fHead.Contains('*'))
                fHead = WildcardsToRegex(fHead);

            if (fTail.Contains('?') || fTail.Contains('*'))
                fTail = WildcardsToRegex(fTail);

            Match mHead = Regex.Match(text, fHead);
            Match mTail = Regex.Match(text.Substring(mHead.Length), fTail);

            System.Diagnostics.Debug.WriteLine(" ");
            System.Diagnostics.Debug.WriteLine("#############################");
            System.Diagnostics.Debug.WriteLine("mHead: [" + mHead.ToString() + "]");
            System.Diagnostics.Debug.WriteLine("mTail: [" + mTail.ToString() + "]");
            System.Diagnostics.Debug.WriteLine("#############################");
            System.Diagnostics.Debug.WriteLine(" ");
        }

        private static String GetTextHead(string text, string format)
        {
            if (String.IsNullOrEmpty(text)) return "";
            if (String.IsNullOrEmpty(format)) return "";

            string fHead;
            int index = format.IndexOf(TOKEN, StringComparison.OrdinalIgnoreCase);

            fHead = format.Substring(0, index);
            if (fHead.Contains('?') || fHead.Contains('*'))
            {
                fHead = "^" + WildcardsToRegex(fHead);
                //The next replace is to avoid regex greedyness.
                //This way, regex just matches until the first ocurrence.
                fHead = fHead.Replace("*", "*?");
            }

            Match mHead = Regex.Match(text, fHead, RegexOptions.Compiled);
            return mHead.ToString();
        }

        private static String GetTextTail(string text, string format)
        {
            if (String.IsNullOrEmpty(text)) return "";
            if (String.IsNullOrEmpty(format)) return "";

            string fTail;
            int index = format.IndexOf(TOKEN, StringComparison.OrdinalIgnoreCase) + 8;

            fTail = format.Substring(index);
            if (fTail.Contains('?') || fTail.Contains('*'))
                fTail = WildcardsToRegex(fTail) + "$"; //End of expression.

            string endingText = text.Substring(textHead.Length);

            Match mTail = Regex.Match(endingText, fTail, RegexOptions.Compiled);
            return mTail.ToString();
        }
    }
}
