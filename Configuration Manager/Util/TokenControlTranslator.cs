using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager.Util
{
    class TokenControlTranslator
    {
        private static TokenControlTranslator tokenControlTranslator;

        private static String DEFAULT_TOKEN = "##";

        private Boolean active = true;

        private String defaultLang;
        private String currentLang;

        private String tokenKey;
        private String textToTranslate;
        private String translatedText;

        private List<String> valuesToTranslate = new List<String>();
        private List<String> translatedValues = new List<String>();
        //private List<String> subPathElements = new List<String>();

        private TokenControlTranslator() 
        {
            this.defaultLang = "";
            this.active = true;

            this.valuesToTranslate = new List<String>();
            this.translatedValues = new List<String>();
        }

        private TokenControlTranslator(String tokenKey)
        {
            SetTokenKey(tokenKey);
        }

        public static TokenControlTranslator GetInstance()
        {
            if (tokenControlTranslator == null)
            {
                tokenControlTranslator = new TokenControlTranslator();
            }
            return tokenControlTranslator;
        }

        public void SetTokenKey(String tokenKey)
        {
            if (tokenKey == "" || tokenKey == null) this.tokenKey = DEFAULT_TOKEN;
            else this.tokenKey = tokenKey;
        }

        public String TranslateFromControl(String textToTranslate)
        {
            this.translatedText = textToTranslate;

            if (active && textToTranslate.Contains(Model.getInstance().controlToken))
            {
                this.valuesToTranslate.Clear();
                this.translatedValues.Clear();

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

        private bool TextHasEvenTokens()
        {
            if (this.textToTranslate != null && this.textToTranslate != "" && this.tokenKey != null)
            {
                int count = System.Text.RegularExpressions.Regex.Matches(textToTranslate, tokenKey).Count;
                if (count % 2 == 0) return true;
            }
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

        private void GetTranslatedValues()
        {
            String text = "";
            foreach (String s in valuesToTranslate)
            {
                ICustomControl control = Model.getInstance().AllControls.Find(c => c.cd.Name == s);

                text = "";
                if (control != null)
                {
                    if (control is CComboBox && (control as CComboBox).SelectedItem != null)
                    {
                        int index = (control as CComboBox).SelectedIndex;
                        if(control.cd.comboBoxConfigItems[index] != String.Empty)
                        {
                            text = control.cd.comboBoxConfigItems[index];
                        }
                        else
                        {
                            text = (control as CComboBox).SelectedItem.ToString();
                        }
                    }
                    else text = control.cd.Text;
                }
                else text = tokenKey + s + tokenKey;
                translatedValues.Add(text);
            }
        }

        private String ReplaceValueToTranslation(String textToTranslate)
        {
            String translated = textToTranslate;
            for (int i = 0; i < valuesToTranslate.Count; i++)
            {
                String replace = tokenKey + valuesToTranslate[i].ToString() + tokenKey;

                if (translatedValues.Count > i)
                {
                    String with = translatedValues[i];
                    //String with = translatedValues[i].ToString();
                    translated = translated.Replace(replace, with);
                }
            }
            return translated;
        }

        // TODO Create a function that splits up the tokens, translates them, and then returns the list of tokens and a list with its translation in the same order.
        public Dictionary<string, string> GetValueTranslatedPairs(String textToTranslate)
        {
            Dictionary<string, string> valueTranslatedPairs = new Dictionary<string, string>();

            this.translatedText = textToTranslate;

            if (active)
            {
                this.valuesToTranslate.Clear();
                this.translatedValues.Clear();

                this.textToTranslate = textToTranslate;

                if (this.textToTranslate != null && this.textToTranslate != "")
                {
                    if (TextHasEvenTokens())
                    {
                        GetValuesToTranslate();
                        GetTranslatedValues();
                    }
                }

                for (int i = 0; i < valuesToTranslate.Count; i++)
                {
                    if(valuesToTranslate[i] != null && translatedValues[i] != null)
                    {
                        valueTranslatedPairs.Add(valuesToTranslate[i], translatedValues[i]);
                    }
                }
            }

            return valueTranslatedPairs;
        }
    }
}
