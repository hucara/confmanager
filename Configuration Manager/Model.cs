using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Configuration_Manager
{
    class Model
    {
        private static Model model;

		public bool stayOnTop = false;
		public bool movable = false;
		public bool resizable = false;

		public int top = 0;
		public int left = 0;
		public int width = 800;
		public int height = 600;
        public int maxSections = 6;
        public int maxControls = 30;
		public int controlMarging = 10;
		public int containerMargin = 10;

		public uint ModificatioRights = 0x00000000;
		public uint DisplayRights = 0x00000000;

#if DEBUG
        public bool progMode = true;
#else
        public bool progMode = false;
#endif



        public bool editingUI = false;
        public bool creatingNewComponent = false;
        public bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }
		public bool ConfigFileExists { get; set; }

        public List<String> DestinationFileTypes;
        public List<String> RelationTypes;
       
        public List<ICustomControl> AllControls { get; private set; }

        public Control CurrentClickedControl { get; set; }
        public ControlDescription CurrentControlDescription { get; set; }
        public int LastClickedX { get; set; }
        public int LastClickedY { get; set; }

        public Section CurrentSection;
        public List<Section> Sections { get; set; }
        
        // Private constructor
        private Model()
        {
            AllControls = new List<ICustomControl>();
            Sections = new List<Section>();
            DestinationFileTypes = new List<String>();
            RelationTypes = new List<String>();

            ObjectDefinitionExists = false;

            FillDestinationFileTypes();
            FillOutRelationTypes();
        }

        // Singleton approach
        public static Model getInstance()
        {
            if (model == null)
            {
                model = new Model();
            }
            return model;
        }

        private void FillDestinationFileTypes()
        {
            DestinationFileTypes.Add(".INI");
            DestinationFileTypes.Add(".XML");
            DestinationFileTypes.Add(".TXT");
        }

        private void FillOutRelationTypes()
        {
            //RelationTypes.Add("Write related");
            //RelationTypes.Add("Read related");
            //RelationTypes.Add("Visibility related");
            //RelationTypes.Add("Coupled controls");
        }

        public String DeleteControlReferences(Control c)
        {
            ICustomControl d = c as ICustomControl;

            foreach (ICustomControl s in AllControls)
            {
                s.cd.RelatedRead.Remove(d);
                s.cd.RelatedVisibility.Remove(d);
                s.cd.RelatedWrite.Remove(d);
                s.cd.CoupledControls.Remove(d);
            }

            return "";
        }

		public void ReadConfigurationFile()
		{
			if (ConfigFileExists)
			{
				XDocument xdoc = Resources.getInstance().ConfigFile;

				ReadSettingsSection(xdoc);
				ReadLanguagesSection(xdoc);
				ReadRightsSection(xdoc);

				System.Diagnostics.Debug.WriteLine("** INFO ** Config file readed. Printing info.");
				System.Diagnostics.Debug.WriteLine(" - Top: " + this.top);
				System.Diagnostics.Debug.WriteLine(" - Left: " + this.left);
				System.Diagnostics.Debug.WriteLine(" - Height: " + this.height);
				System.Diagnostics.Debug.WriteLine(" - Width: " + this.width);
				System.Diagnostics.Debug.WriteLine(" - StayOnTop: " + this.stayOnTop);
				System.Diagnostics.Debug.WriteLine(" - Movable: " + this.movable);
				System.Diagnostics.Debug.WriteLine(" - Resizable: " + this.resizable);
				System.Diagnostics.Debug.WriteLine(" - Lang: " + Resources.getInstance().CurrentLangPath);
				System.Diagnostics.Debug.WriteLine(" - DefLang: " + Resources.getInstance().DefaultLangPath);
				System.Diagnostics.Debug.WriteLine(" - ProgMode: " + this.progMode);
				System.Diagnostics.Debug.WriteLine(" - Modification: " + this.ModificatioRights);
				System.Diagnostics.Debug.WriteLine(" - Display: " + this.DisplayRights);
				System.Diagnostics.Debug.WriteLine("** INFO ** Config file end.");
			}
		}

		private void ReadSettingsSection(XDocument xdoc)
		{
			XElement settings = xdoc.Element("ConfigurationManager").Element("Settings");

			Boolean.TryParse(settings.Element("StayOnTop").Value, out this.stayOnTop);

			if (settings.Element("StayOnTop").Value == "yes") this.stayOnTop = true;
			if (settings.Element("Movable").Value == "yes") this.movable = true;
			if (settings.Element("Resizable").Value == "yes") this.resizable = true;

			Int32.TryParse(settings.Element("Top").Value.ToString(), out this.top);
			Int32.TryParse(settings.Element("Left").Value.ToString(), out this.left);
			Int32.TryParse(settings.Element("Width").Value.ToString(), out this.width);
			Int32.TryParse(settings.Element("Height").Value.ToString(), out this.height);

			Int32.TryParse(settings.Element("MaxSections").Value.ToString(), out this.maxSections);
			Int32.TryParse(settings.Element("MaxControls").Value.ToString(), out this.maxControls);

			Int32.TryParse(settings.Element("ControlMargin").Value.ToString(), out this.controlMarging);
			Int32.TryParse(settings.Element("ContainerMargin").Value.ToString(), out this.containerMargin);
		}

		private void ReadLanguagesSection(XDocument xdoc)
		{
			XElement languages = xdoc.Element("ConfigurationManager").Element("Languages");

			Resources.getInstance().CurrentLangPath = languages.Element("Current").Value.ToString();
			Resources.getInstance().DefaultLangPath = languages.Element("Default").Value.ToString();

			if (Resources.getInstance().CurrentLangPath == "" || Resources.getInstance().CurrentLangPath == null)
			{
				System.Diagnostics.Debug.WriteLine("** INFO ** Language file not found. Selecting default language.");
				
				if (Resources.getInstance().DefaultLangPath == "" || Resources.getInstance().DefaultLangPath == null)
				{
					System.Diagnostics.Debug.WriteLine("** ERROR ** DEFAULT language file not found. This gonna look weird.");
				}
			}
		}

		private void ReadRightsSection(XDocument xdoc)
		{
			XElement rights = xdoc.Element("ConfigurationManager").Element("Rights");

			if (rights.Element("ProgrammerMode").Value == "yes") this.progMode = true;
			uint.TryParse(rights.Element("Modification").Value, out this.ModificatioRights);
			uint.TryParse(rights.Element("Display").Value, out this.DisplayRights);
		}
    }
}