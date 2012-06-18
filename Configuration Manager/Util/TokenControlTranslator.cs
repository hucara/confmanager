using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager.Util
{
    static class TokenControlTranslator
    {
        private const String DEFAULT_TOKEN = "##";

        private static Boolean active = true;

        private static String tokenKey;
        private static String textToTranslate;
        private static String translatedText;

        private static List<String> valuesToTranslate = new List<String>();
        private static List<String> translatedValues = new List<String>();

        //public static TokenControlTranslator GetInstance()
        //{
        //    if (tokenControlTranslator == null)
        //    {
        //        tokenControlTranslator = new TokenControlTranslator();
        //    }
        //    return tokenControlTranslator;
        //}

        //private TokenControlTranslator()
        //{
        //    this.active = true;

        //    this.valuesToTranslate = new List<String>();
        //    this.translatedValues = new List<String>();
        //}

        //private static TokenControlTranslator(String tokenKey)
        //{
        //    SetTokenKey(tokenKey);
        //}

        public static void SetTokenKey(String k)
        {
            if (k == "" || k == null) tokenKey = DEFAULT_TOKEN;
            else tokenKey = k;
        }

        public static String TranslateFromControl(String text)
        {
            if (text == null) return text;

            translatedText = text;

            if (active && text.Contains(Model.getInstance().controlToken))
            {
                valuesToTranslate.Clear();
                translatedValues.Clear();

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

        private static bool TextHasEvenTokens()
        {
            if (textToTranslate != null && textToTranslate != "" && tokenKey != null)
            {
                int count = System.Text.RegularExpressions.Regex.Matches(textToTranslate, tokenKey).Count;
                if (count % 2 == 0) return true;
            }
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

        private static void GetTranslatedValues()
        {
            String text = "";
            foreach (String s in valuesToTranslate)
            {
                String name = s;
                if (name.Contains("_CONF"))
                    name = name.Remove(name.IndexOf("_CONF"));
                ICustomControl control = Model.getInstance().AllControls.Find(c => c.cd.Name == name);

                text = "";
                if (control != null)
                {
                    //if (control is CComboBox && (control as CComboBox).SelectedItem != null)
                    //{
                    //    int index = (control as CComboBox).SelectedIndex;
                    //    if(control.cd.comboBoxConfigItems[index] != String.Empty)
                    //        text = control.cd.comboBoxConfigItems[index];
                    //    else
                    //        text = (control as CComboBox).SelectedItem.ToString();
                    //}
                    if (control is CComboBox)
                    {
                        text = GetComboBoxValue(s, control);
                    }
                    else if (control is CCheckBox)
                    {
                        if ((control as CCheckBox).Checked)
                            text = control.cd.checkBoxCheckedValue;
                        else
                            text = control.cd.checkBoxUncheckedValue;
                    }
                    else text = control.cd.Text;
                }
                else text = tokenKey + s + tokenKey;
                translatedValues.Add(text);
            }
        }

        private static String GetComboBoxValue(String key, ICustomControl control)
        {
            String text = "";
            int index = (control as CComboBox).SelectedIndex;
            if ((key.Contains("_CONF") && control.cd.comboBoxConfigItems.Count > 0))
            {
                if (index != -1)
                    text = control.cd.comboBoxConfigItems[index];
            }
            else
                if (index != -1)
                    text = (control as CComboBox).SelectedItem.ToString();

            return text;
        }

        private static String ReplaceValueToTranslation(String textToTranslate)
        {
            String translated = textToTranslate;
            for (int i = 0; i < valuesToTranslate.Count; i++)
            {
                String replace = tokenKey + valuesToTranslate[i].ToString() + tokenKey;

                if (translatedValues.Count > i)
                {
                    String with = translatedValues[i];
                    translated = translated.Replace(replace, with);
                }
            }
            return translated;
        }
    }
}
