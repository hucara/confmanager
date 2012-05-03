using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Util;

namespace Configuration_Manager.CustomControls
{
    class ReadRelationManager
    {
        TokenControlTranslator tct = TokenControlTranslator.GetInstance();
        TokenTextTranslator ttt = TokenTextTranslator.GetInstance();

        public void TextBoxRead(object sender, EventArgs e)
        {
        }

        public void CommonRead(object sender, EventArgs e)
        {
        }

        public void ComboBoxRead(object sender, EventArgs e)
        {
            CComboBox control = sender as CComboBox;

            foreach (ICustomControl related in control.cd.RelatedRead)
            {
                String text = tct.TranslateFromControl(related.cd.RealText);
                text = ttt.TranslateFromTextFile(text);

                related.cd.Text = text;
            }
        }

        private void TranslateHint(ICustomControl r)
        {
            // Isn't it done on the fly?
        }

        public void ReadRelationUpdate(object sender, EventArgs e)
        {
            ICustomControl c = sender as ICustomControl;

            foreach (ICustomControl r in c.cd.RelatedRead)
            {
                // Updates the new SubDestination
                if (r.cd.RealSubDestination != "" && r.cd.RealSubDestination != null)
                    TranslateSubDestination(r);

                // Updates the Text field or Items from the ComboBox
                if (r.cd.Type == "CComboBox") TranslateComboBoxItems(r);
                else TranslateText(r);

                // Re-reads the value from the source file
                if (r.cd.MainDestination != null && r.cd.MainDestination != "")
                {
                    String fileType = r.cd.MainDestination.Substring(r.cd.MainDestination.Length - 4, 4);
                    if (fileType == ".ini")
                    {
                        ReReadINIFile(r);
                    }
                }
            }
        }

        private void TranslateSubDestination(ICustomControl r)
        {
            String text = tct.TranslateFromControl(r.cd.RealSubDestination);
            text = ttt.TranslateFromTextFile(text);

            r.cd.SubDestination = text;
        }

        private void TranslateText(ICustomControl r)
        {
            String text = tct.TranslateFromControl(r.cd.RealText);
            text = ttt.TranslateFromTextFile(text);

            r.cd.Text = text;
        }

        private void TranslateComboBoxItems(ICustomControl r)
        {
            int index = (r as ComboBox).SelectedIndex;

            r.cd.comboBoxItems.Clear();
            (r as ComboBox).Items.Clear();

            for (int i = 0; i < r.cd.comboBoxRealItems.Count; i++)
            {
                String text = tct.TranslateFromControl(r.cd.comboBoxRealItems[i]);
                text = ttt.TranslateFromTextFile(text);

                (r as ComboBox).Items.Add(text);
            }

            (r as ComboBox).SelectedIndex = index;
        }

        private void ReReadINIFile(ICustomControl r)
        {
            try
            {
                IniFile file = new IniFile(r.cd.MainDestination);
                List<String> nodes = r.cd.SubDestination.TrimStart('\\').Split('\\').ToList();

                if (r.cd.Type == "CComboBox")
                {
                    String value = file.IniReadValue(nodes[0], nodes[1]);
                    int index = r.cd.comboBoxConfigItems.IndexOf(value);

                    if ((r as ComboBox).Items.Count >= index)
                        (r as ComboBox).SelectedIndex = index;
                }
                else
                {
                    String value = file.IniReadValue(nodes[0], nodes[1]);
                    r.cd.Text = value;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading INI file for: " +r.cd.Name);
            }
        }
    }
}
