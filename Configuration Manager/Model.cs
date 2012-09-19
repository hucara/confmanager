using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

using Util;
using Configuration_Manager.Util;

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

        public bool uiChanged { get; set; }

        public int top = 0;
        public int left = 0;
        public int width = 800;
        public int height = 600;
        public int maxSections = 9;
        public int maxControls = 30;
        public int controlMarging = 10;
        public int containerMargin = 10;

        public byte[] MainModificationRights = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public byte[] MainDisplayRights = new byte[4] { 0x00, 0x00, 0x00, 0x00 };

        public bool createLogs = false;
        public int maxAgeOfLogs = -1;

        public int sectionMenuWidth = 131;

        public bool editingUI = false;
        public bool creatingNewComponent = false;
        public bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }
        public bool ConfigFileExists { get; set; }

        public String Headline { get; private set; }

        public String ExePath { get; private set; }
        public String AppFolderPath { get; private set; }

        public String MainFolderPath { get; private set; }
        public String ConfigFolderPath { get; private set; }
        public String LogFolderPath { get; private set; }
        public String ConfigFilePath { get; private set; }

        public string LangID { get; set; }
        public String TextsFilesFolderPath { get; private set; }
        public String TextsFilePath { get; private set; }
        public String TranslationLangPath { get; private set; }
        private static String translationPath;
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
        public static XDocument translationFile { get; private set; }
        public static XDocument TextFile { get; private set; }

        public LogCreation logCreator { get; private set; }
        public LogDeletion logDeleter { get; private set; }

        public ToolStripTextBox InfoLabel { get; set; }

        public String textToken { get; set; }
        public String controlToken { get; set; }

        public System.Drawing.Font lastSelectedFont { get; set; }

        // Private constructor
        private Model()
        {
            ObjectDefinitionExists = false;
            uiChanged = false;

            AllControls = new List<ICustomControl>();
            Sections = new List<Section>();
            DestinationFileTypes = new List<String>();
            RelationTypes = new List<String>();

            SetPaths();
            LoadFiles();

            FillDestinationFileTypes();
        }

        // Singleton
        public static Model getInstance()
        {
            if (model == null)
                model = new Model();
            return model;
        }

        private void LoadFiles()
        {
            System.Diagnostics.Debug.WriteLine(" ");
            System.Diagnostics.Debug.WriteLine("*** - Starting Configuration Manager - ***");

            LoadConfigurationFile();
            LoadObjectDefinitionFile();
        }

        private void LoadConfigurationFile()
        {
            try
            {
                ConfigFile = XDocument.Load(ConfigFilePath);
                ConfigFileExists = true;
                System.Diagnostics.Debug.WriteLine("** OK ** " + ConfigFilePath + " - File found");
            }
            catch (FileNotFoundException)
            {
                ConfigFileExists = false;
                System.Diagnostics.Debug.WriteLine("** ERROR ** " + ConfigFilePath + " - File not found");

                String caption = GetTranslationFromID(37);
                String msg = GetTranslationFromID(41) +" "+ GetTranslationFromID(43);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.Environment.Exit(1);
            }
        }

        private void LoadObjectDefinitionFile()
        {
            try
            {
                ConfigObjects = XDocument.Load(ObjectDefinitionsPath);
                ObjectDefinitionExists = true;
                System.Diagnostics.Debug.WriteLine("** OK ** " + ObjectDefinitionsPath + " - File found");
            }
            catch (Exception)
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
            this.TextsFilesFolderPath = this.MainFolderPath + "\\texts";
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

                this.Headline = (String)xdoc.Element("ConfigurationManager").Element("Headline") ?? "Configuration Manager - DEFAULT HEADLINE";

                ReadSettingsSection(xdoc);
                ReadLanguagesSection(xdoc);
                ReadRightsSection(xdoc);
                ReadLogsSection(xdoc);

                this.args = Environment.GetCommandLineArgs();

                if (this.args != null)
                    SetArguments();
                
                if (this.createLogs)
                {
                    logCreator = new LogCreation("CM", 70, '#', this.createLogs);
                    logDeleter = new LogDeletion("ConfigurationManager", "CM", this.maxAgeOfLogs);
                }

                if (this.textToken == "" || this.textToken == null) TokenTextTranslator.SetTokenTextTranslator(null, this.TextsFilePath);
                else TokenTextTranslator.SetTokenTextTranslator(textToken, this.TextsFilePath);

                if (this.controlToken == "" || this.controlToken == null) TokenControlTranslator.SetTokenKey("##");
                else TokenControlTranslator.SetTokenKey(controlToken);

                System.Diagnostics.Debug.WriteLine(" ");
                System.Diagnostics.Debug.WriteLine("** INFO ** Config file and arguments read. Printing info.");
                System.Diagnostics.Debug.WriteLine(" - Top: " + this.top);
                System.Diagnostics.Debug.WriteLine(" - Left: " + this.left);
                System.Diagnostics.Debug.WriteLine(" - Height: " + this.height);
                System.Diagnostics.Debug.WriteLine(" - Width: " + this.width);
                System.Diagnostics.Debug.WriteLine(" - StayOnTop: " + this.stayOnTop);
                System.Diagnostics.Debug.WriteLine(" - Movable: " + this.movable);
                System.Diagnostics.Debug.WriteLine(" - Resizable: " + this.resizable);
                System.Diagnostics.Debug.WriteLine(" - Lang: " + this.TextsFilePath);
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
            try
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

                Int32.TryParse(settings.Element("ControlMargin").Value.ToString(), out this.controlMarging);
                Int32.TryParse(settings.Element("ContainerMargin").Value.ToString(), out this.containerMargin);

                this.textToken = settings.Element("TextToken").Value.ToString();
                this.controlToken = settings.Element("ControlToken").Value.ToString();
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** INFO *** There was a problem reading the Settings section in the configuration file.");
            }
        }

        private void ReadLanguagesSection(XDocument xdoc)
        {
            try
            {
                XElement languages = xdoc.Element("ConfigurationManager").Element("Languages");
                this.TextsFilePath = languages.Element("Text").Value.ToString();
                this.TranslationLangPath = languages.Element("Translation").Value.ToString();
                this.DefaultLangPath = languages.Element("Translation").Value.ToString();

                if (this.TextsFilePath != "" && this.TextsFilePath != null)
                {
                    if (!System.IO.File.Exists(this.TextsFilePath))
                    {
                        System.Diagnostics.Debug.WriteLine("** INFO ** Language file not found.");

                        String caption = GetTranslationFromID(37);
                        String msg = GetTranslationFromID(439) +" "+ GetTranslationFromID(43);
                        MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Environment.Exit(0);
                    }
                    else
                        Model.translationPath = this.TranslationLangPath;

                    translationFile = XDocument.Load(this.TranslationLangPath);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** INFO *** There was a problem reading the Languages section in the configuration file.");
            }
        }

        private void ReadRightsSection(XDocument xdoc)
        {
            try
            {
                XElement rights = xdoc.Element("ConfigurationManager").Element("Rights");

                string mode = ((string)rights.Element("ProgrammerMode")) ?? "";
                if (mode.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    this.progModeAllowed = true;
                else
                    this.progModeAllowed = false;

                this.MainModificationRights = HexToData(rights.Element("Modification").Value.Substring(2));
                this.MainDisplayRights = HexToData(rights.Element("Display").Value.Substring(2));
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** INFO *** There was a problem reading Rights in the configuration file.");
            }
        }


        private void ReadLogsSection(XDocument xdoc)
        {
            try
            {
                XElement logs = xdoc.Element("ConfigurationManager").Element("Logs");
                if (logs.Element("CreateLogs").Value == "yes")
                {
                    this.createLogs = true;
                    Int32.TryParse(logs.Element("MaxAge").Value, out this.maxAgeOfLogs);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** INFO *** There was a problem reading the Logs section in the configuration file.");
            }
        }

        public void DeleteControl(Control c, bool deletingSection)
        {
            DialogResult delChildren = DialogResult.OK;
            DialogResult deleteControl = DialogResult.OK;
            Boolean hasRelations = false;
            String relMessage = "";

            if (c == null) throw new ArgumentNullException();

            if (!deletingSection)
            {
                if (c.Controls.Count > 0)
                {
                    String msg = GetTranslationFromID(33) + c.Name + "\n" + GetTranslationFromID(36);
                    String caption = GetTranslationFromID(32);
                    delChildren = MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }

                if (!(c is TabPage)) hasRelations = HasRelations(c, out relMessage);

                if (hasRelations)
                    deleteControl = MessageBox.Show(relMessage, "Remove control", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                else
                    deleteControl = MessageBox.Show("Are you sure you want to remove " + c.Name + "?", "Remove control", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);


                if (delChildren == DialogResult.OK && deleteControl == DialogResult.OK)
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
            //String question = "\nAre you sure you want to remove " + control.Name + "?";
            String references = "";
            String referencedBy = "";

            ICustomControl c = control as ICustomControl;

            // Check if this control has anything else inside the related list
            if (ControlReferencesOthers(c, out references))
            {
                msg += c.cd.Name + GetTranslationFromID(34) + "\n";
                msg += references + "\n";
            }

            // Check if this control is inside the related lists of other controls
            if (ControlIsReferenced(c, out referencedBy))
            {
                msg += GetTranslationFromID(35) + " " + c.cd.Name + ":\n";
                msg += referencedBy;
            }

            Message = msg;

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
                references += "- " + GetTranslationFromID(22) + ": ";
                foreach (Control co in c.cd.RelatedRead)
                    references += (co as ICustomControl).cd.Name + " ";
                references += "\n";
            }

            if (c.cd.RelatedWrite.Count > 0)
            {
                references += "- " + GetTranslationFromID(23) + ": ";
                foreach (Control co in c.cd.RelatedWrite)
                    references += (co as ICustomControl).cd.Name + " ";
                references += "\n";
            }

            if (c.cd.RelatedVisibility.Count > 0)
            {
                references += "- " + GetTranslationFromID(24) + ": ";
                foreach (Control co in c.cd.RelatedVisibility)
                    references += (co as ICustomControl).cd.Name + " ";
                references += "\n";
            }

            if (c.cd.CoupledControls.Count > 0)
            {
                references += "- " + GetTranslationFromID(25) + ": ";
                foreach (Control co in c.cd.CoupledControls)
                    references += (co as ICustomControl).cd.Name + " ";
                references += "\n";
            }

            if (references != "") return true;
            return false;
        }

        public void GetArguments(string[] args)
        {
            if (args != null)
                this.args = args;
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
            catch (ArgumentOutOfRangeException)
            {
                string msg = GetTranslationFromID(58);
                string caption = GetTranslationFromID(37);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Diagnostics.Debug.WriteLine("! Error when parsing the arguments. Out of range?");

                System.Environment.Exit(0);
            }
        }

        public void UpdateInfoLabel(object sender, EventArgs e)
        {
            if (sender is CToolStripButton)
            {
                CToolStripButton ct = sender as CToolStripButton;
                if(CurrentSection.Button == ct)
                    SetInfoLabelText((sender as CToolStripButton).ToolTipText);
            }
            else
                if (!String.IsNullOrEmpty((sender as ICustomControl).cd.Hint))
                    SetInfoLabelText((sender as ICustomControl).cd.Hint);
        }

        private void SetInfoLabelText(String hint)
        {
            this.InfoLabel.Text = "";
            this.InfoLabel.Text = hint;
            if (this.InfoLabel.Text != "")
            {
                this.InfoLabel.Text = TokenControlTranslator.TranslateFromControl(this.InfoLabel.Text);
                this.InfoLabel.Text = TokenTextTranslator.TranslateFromTextFile(this.InfoLabel.Text);
                this.InfoLabel.BackColor = System.Drawing.Color.Lavender;
            }
        }

        public void EraseInfoLabel(object sender, EventArgs e)
        {
            this.InfoLabel.Text = "";
            this.InfoLabel.BackColor = System.Drawing.SystemColors.Control;
        }

        // Converts the string of characters to array of bytes.
        // The correct format is "00000000"
        public static byte[] HexToData(string hexString)
        {
            if (hexString == null)
                return null;

            if (hexString.Length % 2 == 1)
                hexString = '0' + hexString;

            byte[] data = new byte[hexString.Length / 2];

            try
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                //byte.TryParse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier, out data[i]);
            }
            catch (Exception)
            {
                model.logCreator.Append("[ERROR] Problem converting rights to the correct format: " + hexString);
            }

            return data;
        }

        public static bool ObtainRights(byte[] ControlRight, byte[] MainRight)
        {
            bool res = true;
            for (int i = 0; i < ControlRight.Length; i++)
                if ((ControlRight[i] & MainRight[i]) != ControlRight[i])
                    res = false;

            System.Diagnostics.Debug.WriteLine("R&R: " + BitConverter.ToString(ControlRight) + " Result: " + res.ToString());
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

                    // Close open editors
                    List<ControlEditor> editors = Application.OpenForms.OfType<ControlEditor>().ToList();
                    for (int i = 0; i < editors.Count; i++)
                        editors[i].Close();
                }
            }

            ApplyRightsToControls();
            ApplyRightsToSections();
        }

        public void ApplyRightsToControls()
        {
            if (progModeAllowed)
            {
                foreach (ICustomControl c in AllControls)
                {
                    if (progMode)
                    {
                        if (!(c is CTabPage))
                        {
                            c.cd.Enabled = true;
                            c.cd.Visible = true;
                        }
                    }
                    else
                    {
                        if (!(c is CTabPage))
                        {
                            // This is fucked
                            ApplyRelations(c);
                            if(!c.cd.operatorVisibility) c.cd.Visible = c.cd.operatorVisibility;
                            if(!c.cd.operatorModification) c.cd.Enabled = c.cd.operatorModification;
                        }
                    }
                }
            }
        }

        public void ApplyRightsToSections()
        {
            if (progModeAllowed)
            {
                if (progMode)
                {
                    foreach (Section s in this.Sections)
                    {
                        s.Button.Visible = true;
                        s.Button.Enabled = true;

                        foreach (ICustomControl c in AllControls.Where(p => p.cd.ParentSection == s))
                            (c as Control).Enabled = true;
                    }
                }
                else
                {
                    foreach (Section s in this.Sections)
                    {
                        bool visibility = ObtainRights(s.DisplayBytes, model.MainDisplayRights);
                        if (!visibility)
                        {
                            foreach (ICustomControl c in AllControls.Where(p => p.cd.ParentSection == s))
                                (c as Control).Visible = false;
                        }

                        s.Button.Enabled = ObtainRights(s.ModificationBytes, model.MainModificationRights);
                        if (!s.Button.Enabled)
                        {
                            foreach (ICustomControl c in AllControls.Where(p => p.cd.ParentSection == s))
                                (c as Control).Enabled = false;
                        }
                    }
                }
            }
        }

        public void ApplyRelations(ICustomControl c)
        {
            if (c.cd.CoupledControls.Count > 0 || c.cd.RelatedVisibility.Count > 0 || c.cd.RelatedRead.Count > 0)
            {
                if (c is CCheckBox)
                {
                    CheckState state = (c as CheckBox).CheckState;
                    if (state == CheckState.Checked) (c as CheckBox).CheckState = CheckState.Unchecked;
                    else (c as CheckBox).CheckState = CheckState.Checked;

                    (c as CheckBox).CheckState = state;
                }

                if (c is CComboBox)
                {
                    int index = (c as ComboBox).SelectedIndex;
                    (c as ComboBox).SelectedIndex = -1;
                    (c as ComboBox).SelectedIndex = index;
                }
            }
        }

        public static String GetTranslationFromID(int id)
        {
            String text = "TEXT NOT FOUND";
            try
            {
                text = translationFile.Descendants("TextFile").Descendants("Texts").Descendants("Text").Single(x => (int?)x.Attribute("id") == id).Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Error found in the translation file. The application will now close.", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                System.Environment.Exit(0);
            }
            return text;
        }
    }
}