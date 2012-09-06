using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration_Manager.Util
{
	static class TokenTextTranslator
	{
		private static String DEFAULT_TOKEN = "@@";

        private static Boolean active = true;
        //private static Boolean defaultMode = false;		// Shall we use a sidekick language file for not found tokens? 

        //private static String defaultLang = "";				// In case the token is not found in the current language, search it here
        //private static String defaultSubPath;				// Search 

        private static String currentLang;					// Language file from where we take the values out
        //private static String currentSubPath;

        private static String tokenKey;					// The defining characters that delimite a token

        private static String textToTranslate;				// Text with the values to translate
        private static String translatedText;				// Text with all the tokens translated

        private static List<String> valuesToTranslate = new List<String>();		// List of values to translate inside the textToTranslate
        private static List<String> translatedValues = new List<String>();		// List of the values translated in the same order as the valuesToTranslate
        private static List<String> subPathElements = new List<String>();		// List representing the node tree to find the desired value to translate

		public static void SetTokenTextTranslator(String tokenKey)
		{
			SetTokenKey(tokenKey);
		}

		public static void SetTokenTextTranslator(String tokenKey, String language)
		{
			SetTokenKey(tokenKey);
			currentLang = language;
		}

		private static void SetTokenKey(String k)
		{
			if (k == "" || k == null) tokenKey = k;
			else tokenKey = k;
		}

		public static Boolean SetSubPath(String defaultSubPath)
		{
			if (defaultSubPath != "" && defaultSubPath != null)
			{
				defaultSubPath = defaultSubPath.TrimStart("\\".ToCharArray());
				defaultSubPath = defaultSubPath.TrimEnd("\\".ToCharArray());

				subPathElements.AddRange(defaultSubPath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
				return true;
			}
			return false;
		}

		public static String TranslateFromTextFile(String text)
		{
            if (text == null) return text;

			translatedText = text;

			if (active && text.Contains(DEFAULT_TOKEN))
			{
				valuesToTranslate.Clear();
				translatedValues.Clear();
				subPathElements.Clear();

				textToTranslate = text;

				if (textToTranslate != null && textToTranslate != "")
				{
					if (TextHasEvenTokens())
					{
						GetValuesToTranslate();
						GetTranslatedValues();
						translatedText = ReplaceValueToTranslation(textToTranslate);
					}
				}
			}

			return translatedText;
		}

		public static String Translate(String text, String subPath)
		{
			textToTranslate = text;
			translatedText = "";

			if (textToTranslate != null && textToTranslate != "")
			{
				if (TextHasEvenTokens())
				{
					GetValuesToTranslate();
					ReplaceValueToTranslation(textToTranslate);
				}
				else
					return "*ERROR*";
			}
			return "";
		}

		private static bool TextHasEvenTokens()
		{
			if (textToTranslate != null && textToTranslate != "" && tokenKey != null)
			{
				int count = System.Text.RegularExpressions.Regex.Matches(textToTranslate, tokenKey).Count;
				if (count % 2 == 0) return true;
			}
			return false;
		}

		private static bool ValidateText(System.Text.RegularExpressions.MatchCollection mc)
		{
			System.Diagnostics.Debug.WriteLine("! Count: " + mc.Count);
			if (mc.Count > 0) return true;
			return false;
		}

		private static void GetValuesToTranslate()
		{
			System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(textToTranslate, tokenKey + "\\w*" + tokenKey);

			foreach (System.Text.RegularExpressions.Match token in mc)
			{
				if (ValidateToken(token.ToString()))
				{
					String value = "";
					value = token.ToString().TrimStart(tokenKey.ToCharArray());
					value = value.TrimEnd(tokenKey.ToCharArray());
					valuesToTranslate.Add(value);
				}
			}
		}

		private static bool ValidateToken(string t)
		{
			if (t != null)
			{
				if (t.StartsWith(tokenKey) && t.EndsWith(tokenKey) && t.Length > tokenKey.Length * 2)
					return true;
			}
			return false;
		}

		private static String ReplaceValueToTranslation(String textToTranslate)
		{
			String translated = textToTranslate;
			for (int i = 0; i < valuesToTranslate.Count; i++)
			{
                String replace = tokenKey + valuesToTranslate[i].ToString() + tokenKey;
				
				if (translatedValues.Count > i)
				{
					String with = translatedValues[i].ToString();
					translated = translated.Replace(replace, with);
				}
			}
			return translated;
		}

		//private void GetTranslatedValues()
		//{
		//    try
		//    {
		//        XDocument xdoc = XDocument.Load(currentLang);

		//        if (subPathElements.Count > 0)
		//        {
		//            // Search from the subpath
		//            XElement e = xdoc.Element(subPathElements[0]);
		//            foreach (String s in subPathElements)
		//            {
		//                e = xdoc.Element(s);
		//                System.Diagnostics.Debug.WriteLine("! Getting into: "+s);
		//            }

		//            translatedValues.Add(e.Value);
		//        }
		//        else
		//        {
		//            // Search directly from the root
		//            foreach (String s in valuesToTranslate)
		//            {
		//                translatedValues.Add(xdoc.Element(s).Value);
		//            }
		//        }
		//    }
		//    catch (System.IO.FileNotFoundException e)
		//    {
		//        System.Diagnostics.Debug.WriteLine("! Problem reading the language file:");
		//        System.Diagnostics.Debug.WriteLine("/!\\Exception:" + e.ToString());
		//    }
		//}


		private static void GetTranslatedValues()
		{
			try
			{
				if (System.IO.File.Exists(Model.getInstance().TextsFilePath))
				{
					XDocument xdoc = XDocument.Load(Model.getInstance().TextsFilePath);
					foreach (String s in valuesToTranslate)
					{
						var q = from c in xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text")
								where c.Attribute("id").Value.ToString() == s
								select (string)c.Value;

						translatedValues.AddRange(q);
					}
				}
			}
			catch (System.IO.FileNotFoundException e)
			{
                System.Diagnostics.Debug.WriteLine("! Problem found while translating text to value.");
			}
		}
	}
}
