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
        // Handler that updates all the control fields that should be updated
        // for the read related controls.
        public static void ReadRelationUpdate(object sender, EventArgs e)
        {
            ICustomControl c = sender as ICustomControl;

            foreach (ICustomControl r in c.cd.RelatedRead)
                ReadConfiguration(r);

            foreach (ICustomControl r in c.cd.RelatedRead)
                r.cd.Changed = false;
        }

        public static void ReadConfiguration(ICustomControl r)
        {
            if (r.cd.RealSubDestination != "" && r.cd.RealSubDestination != null)
                TranslateSubDestination(r);

            // Updates the Text field or Items from the ComboBox
            if (r.cd.Type == "CComboBox") TranslateComboBoxItems(r);
            else TranslateText(r);

            // Re-reads the value from the source file
            if (r.cd.MainDestination != null && r.cd.MainDestination != "")
            {
                String value = "";

                System.Diagnostics.Debug.WriteLine("<< Reading key: " + r.cd.SubDestination);
                String fileType = r.cd.MainDestination.Substring(r.cd.MainDestination.Length - 4, 4);
                if (fileType == ".ini" && r.cd.DestinationType == ".INI") value = ReReadINIFile(r);
                else if (fileType == "conf" && r.cd.DestinationType == ".INI") value = ReReadINIFile(r);
                else if (fileType == ".xml" && r.cd.DestinationType == ".XML") value = ReReadXMLFile(r);
                else if (r.cd.DestinationType == "REG") value = ReReadRegistry(r);

                SetReadValue(r, value);
                //r.cd.Changed = false;
            }

            if (r.cd.Type == "CBitmap")
                r.cd.RealPath = r.cd.RealPath;

            //r.cd.Changed = false;
        }

        private static void TranslateSubDestination(ICustomControl r)
        {
            String text = TokenControlTranslator.TranslateFromControl(r.cd.RealSubDestination);
            text = TokenTextTranslator.TranslateFromTextFile(text);

            r.cd.SubDestination = text;
        }

        private static void TranslateText(ICustomControl r)
        {
            String text = TokenControlTranslator.TranslateFromControl(r.cd.RealText);
            text = TokenTextTranslator.TranslateFromTextFile(text);

            r.cd.Text = text;
        }

        private static void TranslateComboBoxItems(ICustomControl r)
        {
            int index = (r as ComboBox).SelectedIndex;
            r.cd.comboBoxItems.Clear();
            (r as ComboBox).Items.Clear();
            (r as ComboBox).BeginUpdate();

            for (int i = 0; i < r.cd.comboBoxRealItems.Count; i++)
            {
                String text = TokenControlTranslator.TranslateFromControl(r.cd.comboBoxRealItems[i]);
                text = TokenTextTranslator.TranslateFromTextFile(text);

                (r as ComboBox).Items.Add(text);
                r.cd.comboBoxItems.Add(text);
                System.Diagnostics.Debug.WriteLine(">> TRANSLATED item: " + text + " for " + r.cd.Name);
            }

            (r as ComboBox).EndUpdate();
            (r as ComboBox).SelectedIndex = index;
            System.Diagnostics.Debug.WriteLine(">> SELECTED item: " + index + " for " + r.cd.Name);

            //if(!HasLoopRelation(r)) (r as ComboBox).SelectedIndex = index;
        }

        private static String ReReadINIFile(ICustomControl r)
        {
            try
            {
                IniFile file = new IniFile(r.cd.MainDestination);
                List<String> nodes = r.cd.SubDestination.TrimStart('\\').Split('\\').ToList();
                String value = file.IniReadValue(nodes[0], nodes[1]);

                return value;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading INI file for: " + r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading INI file for " + r.cd.Name);
            }
            return "";
        }

        private static String ReReadXMLFile(ICustomControl r)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(r.cd.MainDestination);

                String value = xdoc.SelectSingleNode(r.cd.SubDestination).InnerText;
                return value;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading XML file for: " + r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading XML file for " + r.cd.Name);
            }
            return "";
        }

        private static String ReReadRegistry(ICustomControl r)
        {
            try
            {
                RegistryManager regMan = new RegistryManager(r.cd.MainDestination);

                String value = regMan.GetValue(r.cd.SubDestination);
                return value;
                //SetReadValue(r, value);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** Re-Reading REGISTRY for: " + r.cd.Name);
                Model.getInstance().logCreator.Append("[ERROR] Re-Reading registry for " + r.cd.Name);
            }
            return "";
        }

        private static void SetReadValue(ICustomControl r, String value)
        {
            if (r.cd.Type == "CComboBox")
            {
                if (r.cd.Format != "" || r.cd.Format != null)
                {
                    string formattedValue = StringFormatter.GetFormattedText(value, r.cd.Format);
                    formattedValue = TokenControlTranslator.TranslateFromControl(formattedValue);
                    formattedValue = TokenTextTranslator.TranslateFromTextFile(formattedValue);

                    int index = r.cd.comboBoxConfigItems.IndexOf(formattedValue);
                    if ((r as ComboBox).Items.Count >= index)
                        (r as ComboBox).SelectedIndex = index;
                }
                else
                {
                    int index = r.cd.comboBoxConfigItems.IndexOf(value);
                    if ((r as ComboBox).Items.Count >= index)
                        (r as ComboBox).SelectedIndex = index;
                }
            }
            else if (r.cd.Type == "CCheckBox")
            {
                if (!String.IsNullOrEmpty(r.cd.Format))
                    value = StringFormatter.GetFormattedText(value, r.cd.Format);

                if (value.Equals(r.cd.checkBoxCheckedValue, StringComparison.OrdinalIgnoreCase)) (r as CCheckBox).CheckState = CheckState.Checked;
                else (r as CCheckBox).CheckState = CheckState.Unchecked;

            }
            else
                if (!String.IsNullOrEmpty(r.cd.Format))
                    r.cd.Text = StringFormatter.GetFormattedText(value, r.cd.Format);
                else
                    r.cd.Text = value;
        }

        private static bool HasLoopRelation(ICustomControl c)
        {
            List<ICustomControl> rels = new List<ICustomControl>();

            foreach (ICustomControl r in c.cd.RelatedRead)
            {
                rels = r.cd.RelatedRead;
                if (rels.Contains(c)) return true;
            }

            return false;
        }

        public static string GetUnformattedValue(ICustomControl r)
        {
            string value = "";

            if (r.cd.MainDestination != null && r.cd.MainDestination != "")
            {
                System.Diagnostics.Debug.WriteLine("<< Getting Unformatted value: " + r.cd.SubDestination);
                String fileType = r.cd.MainDestination.Substring(r.cd.MainDestination.Length - 4, 4);
                if (fileType == ".ini" && r.cd.DestinationType == ".INI") value = ReReadINIFile(r);
                else if (fileType == "conf" && r.cd.DestinationType == ".INI") value = ReReadINIFile(r);
                else if (fileType == ".xml" && r.cd.DestinationType == ".XML") value = ReReadXMLFile(r);
                else if (r.cd.DestinationType == "REG") value = ReReadRegistry(r);
            }

            return value;
        }
    }
}
