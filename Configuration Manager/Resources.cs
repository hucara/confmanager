using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration_Manager
{
    public class Resources
    {
        private static Resources res;

        public String ExePath { get; private set; }
        public String AppFolderPath { get; private set; }

        public String MainFolderPath { get; private set; }
        public String ConfigFolderPath { get; private set; }
        public String LogFolderPath { get; private set; }
        public String ConfigFilePath { get; private set; }

        public String LangFolderPath { get; private set; }
        public String CurrentLangPath { get; private set; }
        public List<String> LangsPath { get; private set; }

        public String ObjectDefinitionsPath { get; private set; }
        public bool ObjectDefinitionExists { get; private set; }

        public XDocument ConfigObjects { get; private set; }

        // Singleton approach
        private Resources()
        {
            this.ExePath = getExePath();
            this.MainFolderPath = getMainFolderPath();
            this.ConfigFolderPath = getConfigFolderPath();
            this.LogFolderPath = getLogFolderPath();
            this.LangFolderPath = getLangFolderPath();
            this.ObjectDefinitionsPath = getConfigObjectsPath();
            this.ConfigFilePath = getConfigFilePath();

            loadObjectDefinition();
        }

        // Singleton approach
        public static Resources getInstance()
        {
            if (res == null)
            {
                res = new Resources();
            }
            return res;
        }

        private String getExePath()
        {
            return System.Windows.Forms.Application.ExecutablePath;
        }

        private String getMainFolderPath()
        {
            String p = Path.GetFullPath(System.Windows.Forms.Application.ExecutablePath);
            p = p.Substring(0, p.LastIndexOf("\\"));
            return p.Substring(0, p.LastIndexOf("\\"));
            //return Path.GetFileName(System.Windows.Forms.Application.StartupPath);
        }

        private String getConfigFolderPath()
        {
            return this.MainFolderPath + "\\config";
        }

        private String getLogFolderPath()
        {
            return this.MainFolderPath + "\\log";
        }

        private string getLangFolderPath()
        {
            return this.MainFolderPath + "\\texts";
        }

        private string getConfigObjectsPath()
        {
            return this.ConfigFolderPath + "\\testing.xml";
        }

        private string getConfigFilePath()
        {
            return this.ConfigFolderPath + "\\ConfigurationManager.xml";
        }

        private void loadObjectDefinition()
        {
            try
            {
                ConfigObjects = XDocument.Load(ObjectDefinitionsPath);
                Model.getInstance().ObjectDefinitionExists = true;
                System.Diagnostics.Debug.WriteLine("** OK ** " + ObjectDefinitionsPath + " - File found");
                System.Diagnostics.Debug.WriteLine("** Setting up last UI ** ");
            }
            catch (FileNotFoundException)
            {
                Model.getInstance().ObjectDefinitionExists = false;
                System.Diagnostics.Debug.WriteLine("** INFO ** " + ObjectDefinitionsPath + " - File not found");
                System.Diagnostics.Debug.WriteLine("** Creating a new empty UI ** ");
            }
        }
    }
}
