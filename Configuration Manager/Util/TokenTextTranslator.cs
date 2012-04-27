using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration_Manager.Util
{
	class TokenTextTranslator
	{
		private static TokenTextTranslator tokenTextTranslator;

		private static String DEFAULT_TOKEN = "@@";

		private Boolean active = true;
		private Boolean defaultMode = false;		// Shall we use a sidekick language file for not found tokens? 
		
		private String defaultLang;					// In case the token is not found in the current language, search it here
		private String defaultSubPath;				// Search 

		private String currentLang;					// Language file from where we take the values out
		private String currentSubPath;

		private String tokenKey;					// The defining characters that delimite a token

		private String textToTranslate;				// Text with the values to translate
		private String translatedText;				// Text with all the tokens translated

		private List<String> valuesToTranslate = new List<String>();		// List of values to translate inside the textToTranslate
		private List<String> translatedValues = new List<String>();		// List of the values translated in the same order as the valuesToTranslate
		private List<String> subPathElements = new List<String>();		// List representing the node tree to find the desired value to translate

        private TokenTextTranslator()
        {
            this.defaultMode = false;
            this.defaultLang = "";
            this.active = true;

            this.valuesToTranslate = new List<String>();
            this.translatedValues = new List<String>();
            this.subPathElements = new List<String>();
        }

		private TokenTextTranslator(String tokenKey)
		{
			SetTokenKey(tokenKey);

			this.defaultMode = false;
			this.defaultLang = "";
			this.active = true;

			this.valuesToTranslate = new List<String>();
			this.translatedValues = new List<String>();
			this.subPathElements = new List<String>();
		}

		private TokenTextTranslator(String tokenKey, String language)
		{
			SetTokenKey(tokenKey);

			this.currentLang = language;
			this.defaultMode = false;

			if (System.IO.File.Exists(language)) this.active = true;

			this.valuesToTranslate = new List<String>();
			this.translatedValues = new List<String>();
			this.subPathElements = new List<String>();
		}

		private TokenTextTranslator(String tokenKey, String defaultLang, String subPath)
		{
			SetTokenKey(tokenKey);

			this.defaultMode = true;
			this.defaultLang = defaultLang;

			this.valuesToTranslate = new List<String>();
			this.translatedValues = new List<String>();
			this.subPathElements = new List<String>();

			SetSubPath(subPath);
		}

		public static TokenTextTranslator GetInstance()
		{
			if (tokenTextTranslator == null)
			{
				tokenTextTranslator = new TokenTextTranslator();
			}
			return tokenTextTranslator;
		}

		public void SetTokenTextTranslator(String tokenKey)
		{
			SetTokenKey(tokenKey);
		}

		public void SetTokenTextTranslator(String tokenKey, String language)
		{
			SetTokenKey(tokenKey);
			this.currentLang = language;
		}

		private void SetTokenKey(String tokenKey)
		{
			if (tokenKey == "" || tokenKey == null) tokenKey = DEFAULT_TOKEN;
			else this.tokenKey = tokenKey;
		}

		public Boolean SetSubPath(String defaultSubPath)
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

		public String TranslateFromTextFile(String textToTranslate)
		{
			this.translatedText = textToTranslate;

			if (active)
			{
				this.valuesToTranslate.Clear();
				this.translatedValues.Clear();
				this.subPathElements.Clear();

				this.textToTranslate = textToTranslate;

				if (this.textToTranslate != null && this.textToTranslate != "")
				{
					if (TextHasEvenTokens())
					{
						GetValuesToTranslate();
						GetTranslatedValues();
						this.translatedText = ReplaceValueToTranslation(this.textToTranslate);
					}
				}
			}

			return translatedText;
		}

		public String Translate(String textToTranslate, String subPath)
		{
			this.textToTranslate = textToTranslate;
			this.translatedText = "";

			if (this.textToTranslate != null && this.textToTranslate != "")
			{
				if (TextHasEvenTokens())
				{
					GetValuesToTranslate();
					ReplaceValueToTranslation(this.textToTranslate);
				}
				else
				{
					return "*ERROR*";
				}
			}
			return "";
		}

		private bool TextHasEvenTokens()
		{
			if (this.textToTranslate != null && this.textToTranslate != "" && this.tokenKey != null)
			{
				int count = System.Text.RegularExpressions.Regex.Matches(textToTranslate, tokenKey).Count;
				if (count % 2 == 0) return true;
			}
			return false;
		}

		private bool ValidateText(System.Text.RegularExpressions.MatchCollection mc)
		{
			System.Diagnostics.Debug.WriteLine("! Count: " + mc.Count);
			if (mc.Count > 0) return true;
			return false;
		}

		private void GetValuesToTranslate()
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

		private bool ValidateToken(string t)
		{
			if (t != null)
			{
				if (t.StartsWith(tokenKey) && t.EndsWith(tokenKey) && t.Length > tokenKey.Length * 2)
				{
					return true;
				}
			}
			return false;
		}

		private String ReplaceValueToTranslation(String textToTranslate)
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


		private void GetTranslatedValues()
		{
			try
			{
				if (System.IO.File.Exists(Model.getInstance().CurrentLangPath))
				{
					XDocument xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
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
			}
		}
	}
}
