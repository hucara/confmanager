﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.Util;
using Configuration_Manager.CustomControls;
using System.Xml;

namespace Configuration_Manager.RelationManagers
{
    // *** Write Configuration Class *** //
    /*
     * The Write Configuration class is not a real relation.
     * It is in charge of saving the changes that the user
     * made to the configuration files using the controls.
     * 
     * For example, changing the value of a comboBox that should
     * be stored in a configuration.ini file. This class will 
     * store it in a convenient way.
     * 
     */
    static class WriteConfigurationManager
    {
        static TokenControlTranslator tct = TokenControlTranslator.GetInstance();
        static TokenTextTranslator ttt = TokenTextTranslator.GetInstance();

        // Only interested on controls that:
        // - Have configured a destination file and,
        // - Have the "Changed" property as true.
        public static void SaveChanges()
        {
            foreach (ICustomControl c in Model.getInstance().AllControls.Where(p => p.cd.Changed))
            {
                if (c.cd.MainDestination != null && c.cd.MainDestination != "")
                {
                    SaveControlChanges(c);
                    c.cd.Changed = false;
                }
            }
        }

        private static void SaveControlChanges(ICustomControl c)
        {
            String fileType = c.cd.MainDestination.Substring(c.cd.MainDestination.Length - 4, 4);

            String path = ttt.TranslateFromTextFile(c.cd.RealSubDestination);
            path = tct.TranslateFromControl(path).TrimStart('\\');

            String value = GetValueToSave(c);

            if (fileType == ".ini" && c.cd.DestinationType == ".INI") SaveINIFile(c, path, value);
            else if (fileType == ".xml" && c.cd.DestinationType == ".XML") SaveXMLFile(c, path, value);
            else if (c.cd.DestinationType == "REG") SaveRegistryKey(c, path, value);
            else
            {
                Model.getInstance().logCreator.Append("[ERROR] Destination file and type not matching for " + c.cd.Name);
                System.Diagnostics.Debug.WriteLine("!ERROR : Destination file and type not matching for " + c.cd.Name);
            }
        }

        private static string GetValueToSave(ICustomControl c)
        {
            String value = "";
            if (c.cd.Type == "CComboBox")
            {
                if (c.cd.comboBoxConfigItems.Count != 0)
                    value = c.cd.comboBoxConfigItems[(c as CComboBox).SelectedIndex];
                else
                    value = c.cd.comboBoxItems[(c as CComboBox).SelectedIndex];
            }
            else if (c.cd.Type == "CTextBox")
            {
                value = c.cd.Text;
            }
            else if (c.cd.Type == "CCheckBox")
            {
                value = c.cd.checkBoxUncheckedValue;
                if ((c as CCheckBox).CheckState == System.Windows.Forms.CheckState.Checked)
                    value = c.cd.checkBoxCheckedValue;
                else
                    value = c.cd.checkBoxUncheckedValue;
            }
            return value;
        }

        private static void SaveXMLFile(ICustomControl c, String path, String value)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(c.cd.MainDestination);

                xdoc.SelectSingleNode(path).InnerText = value;

                xdoc.Save(c.cd.MainDestination);
                System.Diagnostics.Debug.WriteLine(">>.Xml Saved value from: " + c.cd.Name);

                ReReadControl(c);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an exception while saving changes in file for: " + c.cd.Name);
                System.Diagnostics.Debug.WriteLine(e.Message);
                Model.getInstance().logCreator.Append("[ERROR] Writing file for " + c.cd.Name);
            }      
        }

        private static void ReReadControl(ICustomControl c)
        {
            foreach (ICustomControl r in c.cd.RelatedRead)
            {
                //ReadRelationManager rm = new ReadRelationManager();
                ReadRelationManager.ReadConfigOnStartup(r);
            }
        }

        private static void SaveINIFile(ICustomControl c, String path, String value)
        {
            try
            {
                Util.IniFile file = new Util.IniFile(c.cd.MainDestination);
                List<String> nodes = path.Split('\\').ToList();

                file.IniWriteValue(nodes[0], nodes[1], value);
                System.Diagnostics.Debug.WriteLine(">>.Ini Saved value from: " + c.cd.Name);

                ReReadControl(c);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an exception while saving changes in file for: " + c.cd.Name);
                System.Diagnostics.Debug.WriteLine(e.Message);
                Model.getInstance().logCreator.Append("[ERROR] Writing file for " + c.cd.Name);
            }
        }

        private static void SaveRegistryKey(ICustomControl c, String path, String value)
        {
            try
            {
                RegistryManager regMan = new RegistryManager(c.cd.MainDestination);
                regMan.SetValue(path, value);
                System.Diagnostics.Debug.WriteLine(">>Registry Saved value from: " + c.cd.Name);

                ReReadControl(c);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an exception while saving changes in file for: " + c.cd.Name);
                System.Diagnostics.Debug.WriteLine(e.Message);
                Model.getInstance().logCreator.Append("[ERROR] Writing file for " + c.cd.Name);
            }
        }
    }
}
