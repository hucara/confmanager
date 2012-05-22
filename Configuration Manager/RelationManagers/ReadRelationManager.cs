using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Util;
using System.Xml;

namespace Configuration_Manager.CustomControls
{
    // *** Read Relation Manager *** //
    /* 
     * This class takes care of the Read Relations, which means that when
     * a control changes its state, other controls depending on the value
     * from the first one, should be updated or should re-read the source.
     * 
     */
    static class ReadRelationManager
    {
        static TokenControlTranslator tct = TokenControlTranslator.GetInstance();
        static TokenTextTranslator ttt = TokenTextTranslator.GetInstance();

        // Handler that updates all the control fields that should be updated
        // for the read related controls.
        public static void ReadRelationUpdate(object sender, EventArgs e)
        {
            ICustomControl c = sender as ICustomControl;

            foreach (ICustomControl r in c.cd.RelatedRead)
            {
                // Updates the new SubDestination
                if (r.cd.RealSubDestination != "" && r.cd.RealSubDestination != null)
                    TranslateSubDestination(r);

                // Updates the Text field or Items from the ComboBox
                if (r.cd.Type == "CComboBox") TranslateComboBoxItems(r);
                //if (sender is CCheckBox) TranslateCheckBoxState(sender as CCheckBox, r);
                else TranslateText(r);

                // Re-reads the value from the source file
                if (r.cd.MainDestination != null && r.cd.MainDestination != "")
                {
                    String fileType = r.cd.MainDestination.Substring(r.cd.MainDestination.Length - 4, 4);
                    if (fileType == ".ini" && r.cd.DestinationType == ".INI") ReReadINIFile(r);
                    else if (fileType == ".xml" && r.cd.DestinationType == ".XML") ReReadXMLFile(r);
                    else if (r.cd.DestinationType == "REG") ReReadRegistry(r);
                }
            }
        }

        private static void TranslateCheckBoxState(CCheckBox cb, ICustomControl r)
        {
            Boolean s = cb.Checked;

            if (s)
                r.cd.Text = cb.cd.checkBoxCheckedValue;
            else
                r.cd.Text = cb.cd.checkBoxUncheckedValue;
        }

        public static void ReadConfigOnStartup(ICustomControl r)
        {
            if (r.cd.RealSubDestination != "" && r.cd.RealSubDestination != null)
                TranslateSubDestination(r);

            // Updates the Text field or Items from the ComboBox
            if (r.cd.Type == "CComboBox") TranslateComboBoxItems(r);
            //if (sender is CCheckBox) TranslateCheckBoxState(sender as CCheckBox, r);
            else TranslateText(r);

            // Re-reads the value from the source file
            if (r.cd.MainDestination != null && r.cd.MainDestination != "")
            {
                String fileType = r.cd.MainDestination.Substring(r.cd.MainDestination.Length - 4, 4);
                if (fileType == ".ini" && r.cd.DestinationType == ".INI") ReReadINIFile(r);
                else if (fileType == ".xml" && r.cd.DestinationType == ".XML") ReReadXMLFile(r);
                else if (r.cd.DestinationType == "REG") ReReadRegistry(r);
            }
        }

        private static void TranslateSubDestination(ICustomControl r)
        {
            String text = tct.TranslateFromControl(r.cd.RealSubDestination);
            text = ttt.TranslateFromTextFile(text);

            r.cd.SubDestination = text;        
        }

        private static void TranslateText(ICustomControl r)
        {
            String text = tct.TranslateFromControl(r.cd.RealText);
            text = ttt.TranslateFromTextFile(text);

            r.cd.Text = text;
        }

        private static void TranslateComboBoxItems(ICustomControl r)
        {
            int index = (r as ComboBox).SelectedIndex;

            r.cd.comboBoxItems.Clear();
            (r as ComboBox).Items.Clear();

            for (int i = 0; i < r.cd.comboBoxRealItems.Count; i++)
            {
                String text = tct.TranslateFromControl(r.cd.comboBoxRealItems[i]);
                text = ttt.TranslateFromTextFile(text);

                (r as ComboBox).Items.Add(text);
                r.cd.comboBoxItems.Add(text);
            }

            (r as ComboBox).SelectedIndex = index;
        }

        private static void ReReadINIFile(ICustomControl r)
        {
            try
            {
                IniFile file = new IniFile(r.cd.MainDestination);
                List<String> nodes = r.cd.SubDestination.TrimStart('\\').Split('\\').ToList();
                String value = file.IniReadValue(nodes[0], nodes[1]);

                SetReadValue(r, value);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading INI file for: " +r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading INI file for " + r.cd.Name);
            }
        }

        private static void ReReadXMLFile(ICustomControl r)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(r.cd.MainDestination);

                String value = xdoc.SelectSingleNode(r.cd.SubDestination).InnerText;
                SetReadValue(r, value);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading XML file for: " +r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading XML file for " + r.cd.Name);
            }
        }

        private static void ReReadRegistry(ICustomControl r)
        {
            try
            {
                RegistryManager regMan = new RegistryManager(r.cd.MainDestination);

                String value = regMan.GetValue(r.cd.SubDestination);
                SetReadValue(r, value);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading REGISTRY for: " + r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading registry for " + r.cd.Name);
            }
        }

        private static void SetReadValue(ICustomControl r, String value)
        {
            if (r.cd.Type == "CComboBox")
            {
                int index = r.cd.comboBoxConfigItems.IndexOf(value);

                if ((r as ComboBox).Items.Count >= index)
                    (r as ComboBox).SelectedIndex = index;
            }
            else if (r.cd.Type == "CCheckBox")
            {
                if (value == r.cd.checkBoxCheckedValue) (r as CCheckBox).CheckState = CheckState.Checked;
                else (r as CCheckBox).CheckState = CheckState.Unchecked;
            }
            else
            {
                r.cd.Text = value;
            }
        }
    }
}
