using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

using Util;

namespace Configuration_Manager
{
    class Model
    {
        private static Model model;

		public string[] args { get; set; }

		public bool progModeAllowed = false;
		public bool progMode = false;
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

		public byte[] MainModificationRights = new byte [4] {0x00, 0x00,0x00, 0x00};
        public byte[] MainDisplayRights = new byte [4] {0x00, 0x00,0x00, 0x00};

		public bool createLogs = false;
		public int maxAgeOfLogs = -1;

		public int sectionMenuWidth = 131;

        public bool editingUI = false;
        public bool creatingNewComponent = false;
        public bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }
		public bool ConfigFileExists { get; set; }

		public String ExePath { get; private set; }
		public String AppFolderPath { get; private set; }

		public String MainFolderPath { get; private set; }
		public String ConfigFolderPath { get; private set; }
		public String LogFolderPath { get; private set; }
		public String ConfigFilePath { get; private set; }

		public string LangID { get; set; }
		public String LangFolderPath { get; private set; }
		public String CurrentLangPath { get; private set; }
		public String DefaultLangPath { get; private set; }
		public String ObjectDefinitionsPath { get; private set; }

		public List<String> LangsPath { get; private set; }
        public List<String> DestinationFileTypes;
        public List<String> RelationTypes;
       
        public List<ICustomControl> AllControls { get; private set; }

        public Control CurrentClickedControl { get; set; }
        public int LastClickedX { get; set; }
        public int LastClickedY { get; set; }

        public Section CurrentSection;
        public List<Section> Sections { get; set; }

		public XDocument ConfigObjects { get; private set; }
		public XDocument ConfigFile { get; private set; }
		public XDocument TextFile { get; private set; }

		public LogCreation logCreator { get; private set; }
		public LogDeletion logDeleter { get; private set; }

		public ToolStripTextBox InfoLabel { get; set; }

		private Util.TokenTextTranslator ttt = Util.TokenTextTranslator.GetInstance();
        private Util.TokenControlTranslator tct = Util.TokenControlTranslator.GetInstance();

		public String textToken { get; set; }
        public String controlToken { get; set; }
        
        // Private constructor
        private Model()
        {
			ObjectDefinitionExists = false;

			AllControls = new List<ICustomControl>();
            Sections = new List<Section>();
            DestinationFileTypes = new List<String>();
            RelationTypes = new List<String>();

			SetPaths();
			LoadFiles();

            FillDestinationFileTypes();
            FillOutRelationTypes();
		}

        // Singleton
        public static Model getInstance()
        {
            if (model == null)
            {
                model = new Model();
            }
            return model;
        }

		private void LoadFiles()
		{
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine("*** - Starting Configuration Manager - ***");
			
			LoadConfigFile();
			LoadObjectDefinitionFile();
		}

		private void LoadConfigFile()
		{
			try
			{
				ConfigFile = XDocument.Load(ConfigFilePath);
				ConfigFileExists = true;
				System.Diagnostics.Debug.WriteLine("** OK ** " + ConfigFilePath + " - File found");
			}
			catch (FileNotFoundException e)
			{
				ConfigFileExists = false;
				System.Diagnostics.Debug.WriteLine("** ERROR ** " + ConfigFilePath + " - File not found");

				String msg = "File " + ConfigFilePath + " not found.\nThe application will now close.";
				MessageBox.Show(msg, "Configuration file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

				System.Environment.Exit(0);
			}
		}

		private void LoadObjectDefinitionFile()
		{
			try
			{
				ConfigObjects = XDocument.Load(ObjectDefinitionsPath);
				ObjectDefinitionExists = true;
				System.Diagnostics.Debug.WriteLine("** OK ** " +ObjectDefinitionsPath + " - File found");
			}
			catch (FileNotFoundException e)
			{
				ObjectDefinitionExists = false;
				System.Diagnostics.Debug.WriteLine("** INFO ** " + ObjectDefinitionsPath + " - File not found");
				System.Diagnostics.Debug.WriteLine("** Creating new empty UI **");
			}
		}

		private void SetPaths()
		{
			this.ExePath = GetExePath();
			this.MainFolderPath = GetMainFolderPath();
			this.ConfigFolderPath = this.MainFolderPath + "\\config";
			this.LogFolderPath = this.MainFolderPath + "\\log";
			this.LangFolderPath = this.MainFolderPath + "\\texts";
			this.ObjectDefinitionsPath = this.ConfigFolderPath + "\\ObjectDefinition.xml";
			this.ConfigFilePath = this.ConfigFolderPath + "\\ConfigurationManager.xml";
		}

		private string GetExePath()
		{
			return System.Windows.Forms.Application.ExecutablePath;
		}

		private string GetMainFolderPath()
		{
			String p = System.IO.Path.GetFullPath(System.Windows.Forms.Application.ExecutablePath);
			p = p.Substring(0, p.LastIndexOf("\\"));
			return p.Substring(0, p.LastIndexOf("\\"));
		}

        private void FillDestinationFileTypes()
        {
            DestinationFileTypes.Add(".INI");
            DestinationFileTypes.Add(".XML");
            DestinationFileTypes.Add("REG");
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
				XDocument xdoc = this.ConfigFile;

				ReadSettingsSection(xdoc);
				ReadLanguagesSection(xdoc);
				ReadRightsSection(xdoc);
				ReadLogsSection(xdoc);

                string[] a = { "-p", "-r", "-t", "0", "-l", "1376", "-dr", "0xFF", "-mr", "0xFF" };
                this.args = a;

				if (this.args != null)
				{
					SetArguments();
				}
				if (this.createLogs)
				{
					logCreator = new LogCreation("CM", 70, '#', this.createLogs);
					logDeleter = new LogDeletion("ConfigurationManager", "CM", this.maxAgeOfLogs);
				}

				if (this.textToken == "" || this.textToken == null) ttt.SetTokenTextTranslator(null, this.CurrentLangPath);
				else ttt.SetTokenTextTranslator(textToken, this.CurrentLangPath);

                if (this.controlToken == "" || this.controlToken == null) tct.SetTokenKey("##");
                else tct.SetTokenKey(controlToken);

				System.Diagnostics.Debug.WriteLine(" ");
				System.Diagnostics.Debug.WriteLine("** INFO ** Config file and arguments read. Printing info.");
				System.Diagnostics.Debug.WriteLine(" - Top: " + this.top);
				System.Diagnostics.Debug.WriteLine(" - Left: " + this.left);
				System.Diagnostics.Debug.WriteLine(" - Height: " + this.height);
				System.Diagnostics.Debug.WriteLine(" - Width: " + this.width);
				System.Diagnostics.Debug.WriteLine(" - StayOnTop: " + this.stayOnTop);
				System.Diagnostics.Debug.WriteLine(" - Movable: " + this.movable);
				System.Diagnostics.Debug.WriteLine(" - Resizable: " + this.resizable);
				System.Diagnostics.Debug.WriteLine(" - Lang: " + this.CurrentLangPath);
				System.Diagnostics.Debug.WriteLine(" - DefLang: " + this.DefaultLangPath);
				System.Diagnostics.Debug.WriteLine(" - Createlogs: " + this.createLogs);
				System.Diagnostics.Debug.WriteLine(" - MaxAgeOfLogs: " + this.maxAgeOfLogs);

				System.Diagnostics.Debug.WriteLine(" - ProgModeAllowed: " + this.progModeAllowed);
				System.Diagnostics.Debug.WriteLine(" - ProgMode: " + this.progMode);
				System.Diagnostics.Debug.WriteLine(" - Modification: " + this.MainModificationRights);
				System.Diagnostics.Debug.WriteLine(" - Display: " + this.MainDisplayRights);
				System.Diagnostics.Debug.WriteLine("** INFO ** Config file end.");
				System.Diagnostics.Debug.WriteLine(" ");
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

			this.textToken = settings.Element("TextToken").Value.ToString();
            this.controlToken = settings.Element("ControlToken").Value.ToString();
		}

		private void ReadLanguagesSection(XDocument xdoc)
		{
			XElement languages = xdoc.Element("ConfigurationManager").Element("Languages");

			this.CurrentLangPath = languages.Element("Current").Value.ToString();
			this.DefaultLangPath = languages.Element("Default").Value.ToString();

			if (this.CurrentLangPath != "" && this.CurrentLangPath != null)
			{
				if (!System.IO.File.Exists(this.CurrentLangPath))
				{
					System.Diagnostics.Debug.WriteLine("** INFO ** Language file not found. Selecting default language.");

					String msg = "The file " + this.CurrentLangPath + " was not found.\nClosing the application.";
					MessageBox.Show(msg, "Translation file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

					System.Environment.Exit(0);
				}
			}
		}

		private void ReadRightsSection(XDocument xdoc)
		{
			XElement rights = xdoc.Element("ConfigurationManager").Element("Rights");

			if (rights.Element("ProgrammerMode").Value == "yes") this.progModeAllowed = true;
			this.MainModificationRights = HexToData(rights.Element("Modification").Value.Substring(2));
            this.MainDisplayRights = HexToData(rights.Element("Display").Value.Substring(2));
		}

		private void ReadLogsSection(XDocument xdoc)
		{
			XElement logs = xdoc.Element("ConfigurationManager").Element("Logs");

			if (logs.Element("CreateLogs").Value == "yes")
			{
				this.createLogs = true;
				Int32.TryParse(logs.Element("MaxAge").Value, out this.maxAgeOfLogs);
			}
		}

		public void DeleteControl(Control c, bool deletingSection)
		{
			DialogResult delChildren = DialogResult.OK;
			DialogResult delRelations = DialogResult.OK;
			Boolean hasRelations = false;
			String relMessage = "";

			if(c == null) throw new ArgumentNullException();

            if (!deletingSection)
            {
                if (c.Controls.Count > 0)
                {
                    String msg = "This will remove the control and all its children.";
                    delChildren = MessageBox.Show(msg, "Remove control", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                
                if (c.GetType().Name != "TabPage") hasRelations = HasRelations(c, out relMessage);
               
                if (hasRelations)
                    delRelations = MessageBox.Show(relMessage, "Remove control", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                
                if (delChildren == DialogResult.OK && delRelations == DialogResult.OK)
                {
                    DeleteChildren(c);
                    logCreator.Append("- Deleted: " + c.Name);

                    AllControls.Remove(c as ICustomControl);
                    DeleteControlReferences(c);

                    Control p = c.Parent;
                    c.Parent.Controls.Remove(c);
                    p.Refresh();
                }
            }
            else
            {
                DeleteChildren(c);
                logCreator.Append("- Deleted: " + c.Name);

                AllControls.Remove(c as ICustomControl);
                DeleteControlReferences(c);

                Control p = c.Parent;
                c.Parent.Controls.Remove(c);
                p.Refresh();
            }
		}

		private void DeleteChildren(Control c)
		{
			String ls = "";
			model.logCreator.Append("- Deleted: " + c.Name);
			System.Diagnostics.Debug.WriteLine("! Now in: " + c.Name);
			
			foreach (Control child in c.Controls)
			{
				DeleteChildren(child);

				model.AllControls.Remove(child as ICustomControl);
				model.DeleteControlReferences(child);
			}

			System.Diagnostics.Debug.WriteLine(ls);
		}

		private Boolean HasRelations(Control control, out String Message)
		{
			String msg = "";
			String question = "\nAre you sure you want to delete "+control.Name+"?";
			String references = "";
			String referencedBy = "";

			ICustomControl c = control as ICustomControl;

			// Check if this control has anything else inside the related list
			if (ControlReferencesOthers(c, out references))
			{
				msg += c.cd.Name + " is related to some other controls:\n";
				msg += references +"\n";
			}

			// Check if this control is inside the related lists of other controls
			if (ControlIsReferenced(c, out referencedBy))
			{
				msg += c.cd.Name + " is being related by some other controls:\n";
				msg += referencedBy;
			}

			Message = msg + question;

			if (msg != "") return true;
			else return false;
		}

		private bool ControlIsReferenced(ICustomControl c, out String referencedBy)
		{
			referencedBy = "";

			foreach (ICustomControl co in model.AllControls)
			{
				if (co.cd.RelatedRead.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.RelatedWrite.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.RelatedVisibility.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.CoupledControls.Contains(c)) referencedBy += co.cd.Name + " ";
			}

			if (referencedBy != "") return true;
			return false;
		}

		private bool ControlReferencesOthers(ICustomControl c, out String references)
		{
			references = "";
			if (c.cd.RelatedRead.Count > 0)
			{
				references += "- Related read: ";
				foreach (Control co in c.cd.RelatedRead)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.RelatedWrite.Count > 0)
			{
				references += "- Related write: ";
				foreach (Control co in c.cd.RelatedWrite)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.RelatedVisibility.Count > 0)
			{
				references += "- Related visibility: ";
				foreach (Control co in c.cd.RelatedVisibility)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.CoupledControls.Count > 0)
			{
				references += "- Coupled controls: ";
				foreach (Control co in c.cd.CoupledControls)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (references != "") return true;
			return false;
		}

		public void GetArguments(string[] args)
		{
			if (args != null)
			{
				this.args = args;
			}
		}

		private void SetArguments()
		{
			List<String> ag = this.args.ToList();

			if (ag.Contains("-st")) Model.getInstance().stayOnTop = true;
			if (ag.Contains("-m")) Model.getInstance().movable = true;
			if (ag.Contains("-p")) Model.getInstance().progModeAllowed = true;
			if (ag.Contains("-r")) Model.getInstance().resizable = true;

			try
			{
				if (ag.Contains("-mr"))
				{
					int i = ag.IndexOf("-mr");
                    String r = ag[i + 1].TrimStart("0x".ToArray());
                    if (r.Length <= 8)
                    {
                        while (r.Length < 8) r = "0" + r;
                        this.MainModificationRights = Model.HexToData(r);
                    }
                    else throw new ArgumentOutOfRangeException();
				}

				if (ag.Contains("-dr"))
				{
					int i = ag.IndexOf("-dr");
                    String r = ag[i + 1].TrimStart("0x".ToArray());
                    if (r.Length <= 8)
                    {
                        while (r.Length < 8) r = "0" + r;
                        this.MainDisplayRights = Model.HexToData(r);
                    }
                    else throw new ArgumentOutOfRangeException();
				}

				if (ag.Contains("-l"))
				{
					int i = ag.IndexOf("-l");
					int left;
					if (int.TryParse(ag[i + 1], out left))
						this.left = left;
				}

				if (ag.Contains("-t"))
				{
					int i = ag.IndexOf("-t");
					int top;
					if (int.TryParse(ag[i + 1], out top))
						this.top = top;
				}

				if (ag.Contains("-w"))
				{
					int i = ag.IndexOf("-w");
					int width;
					if (int.TryParse(ag[i + 1], out width))
						this.width = width;
				}

				if (ag.Contains("-h"))
				{
					int i = ag.IndexOf("-h");
					int height;
					if (int.TryParse(ag[i + 1], out height))
						this.height = height;
				}

				if (ag.Contains("-l"))
				{
					int i = ag.IndexOf("-l");
					this.LangID = ag[i + 1];
				}
			}
			catch (ArgumentOutOfRangeException e)
			{
                string msg = "There was an error while parsing the arguments.\nPlease, check them and try again.";
                string caption = "Error while parsing arguments";

                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				System.Diagnostics.Debug.WriteLine("! Error when parsing the arguments. Out of range?");

                System.Environment.Exit(0);
			}
		}

		public void UpdateInfoLabel(object sender, EventArgs e)
		{
			this.InfoLabel.Text = "";
			this.InfoLabel.Text = ttt.TranslateFromTextFile((sender as ICustomControl).cd.Hint);
            this.InfoLabel.Text = tct.TranslateFromControl(this.InfoLabel.Text);
		}

		public void EraseInfoLabel(object sender, EventArgs e)
		{
			this.InfoLabel.Text = "";
		}

        // Converts the string of characters to array of bytes.
        // The format needs to be "00000000"
        public static byte[] HexToData(string hexString)
        {
            if (hexString == null)
                return null;
            //hexString = hexString.Substring(2);

            if (hexString.Length % 2 == 1)
                hexString = '0' + hexString;

            byte[] data = new byte[hexString.Length / 2];

            try
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                //byte.TryParse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier, out data[i]);
            }
            catch (Exception e)
            {
                model.logCreator.Append("[ERROR] Problem converting rights to the correct format: " + hexString);
            }

            return data;
        }

        public bool ObtainRights(byte[] ControlRight, byte[] MainRight)
        {
            bool res = true;
            for (int i = 0; i < ControlRight.Length; i++)
            {
                if ((ControlRight[i] & MainRight[i]) != ControlRight[i]) res = false;
            }

            return res;            
        }

        public void SwitchProgrammingMode()
        {
            if (model.progModeAllowed)
            {
                progMode = !progMode;

                if (progMode)
                {
                    System.Diagnostics.Debug.WriteLine("** INFO ** Programmer mode ACTIVE.");
                    logCreator.Append("");
                    logCreator.Append("[INFO] Programmer mode ACTIVE");
                    logCreator.Append("");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("** INFO ** Programmer mode INACTIVE.");
                    logCreator.Append("");
                    logCreator.Append("[INFO] Programmer mode INACTIVE");
                    logCreator.Append("");

                    // Close opened editors
                    List<Editor> eds = Application.OpenForms.OfType<Editor>().ToList();
                    for (int i = 0; i < eds.Count; i++)
                    {
                        eds[i].Close();
                    }
                }
            }

            ApplyRightsToControls();
        }

        private void ApplyRightsToControls()
        {
            if (progModeAllowed)
            {
                foreach (ICustomControl c in AllControls)
                {
                    if (progMode)
                    {
                        if (c.cd.Type != "CTabPage")
                        {
                            if(!c.cd.inRelatedVisibility) c.cd.Visible = true;
                            c.cd.Enabled = true;
                        }
                    }
                    else
                    {
                        if (c.cd.Type != "CTabPage")
                        {
                            if (!c.cd.inRelatedVisibility) c.cd.Visible = c.cd.operatorVisibility;
                        }
                        c.cd.Enabled = c.cd.operatorModification;
                    }
                }
            }
        }
	}
}