using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Util;
using System.Xml.Linq;

namespace Configuration_Manager
{
    public partial class SectionEditor : Form
    {
        public String SectionName { get; set; }
        private String SectionRealName { get; set; }
        private String ErrorMsg { get; set; }

        public Section section { get; private set; }

        public SectionEditor()
        {
            InitializeComponent();
            section = ControlFactory.BuildSection("", "", true);

            SetLabels();
        }

        public SectionEditor(String oldName)
        {
            InitializeComponent();
            SetLabels();
            this.NameTextBox.Text = oldName;
        }

        public SectionEditor(Section section)
        {
            this.section = section;
            InitializeComponent();

            SetLabels();
            LoadSection();
        }

        private void LoadSection()
        {
            if (this.section == null) return;

            this.NameTextBox.Text = this.section.RealText;
            this.displayRightTextBox.Text = this.section.DisplayRight;
            this.modificationRightTextBox.Text = this.section.ModificationRight;
            this.hintTextBox.Text = this.section.Hint;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ErrorMsg = "";
            ErrorMsg = CheckAttributes();

            if (ErrorMsg == "")
            {
                SaveSection();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Model.getInstance().uiChanged = true;
                this.Close();
            }
            else
            {
                ErrorMsg = "Errors found when editing section: \n" + ErrorMsg;
                MessageBox.Show(ErrorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSection()
        {
            this.section.RealText = this.NameTextBox.Text;
            this.section.Text = TokenTextTranslator.TranslateFromTextFile(this.NameTextBox.Text);

            this.section.DisplayRight = this.displayRightTextBox.Text.Substring(2);
            this.section.ModificationRight = this.modificationRightTextBox.Text.Substring(2);

            this.section.Hint = this.hintTextBox.Text;
        }

        private String CheckAttributes()
        {
            String msg = "";
            if (!CheckText()) msg += "\n- Text field is not valid.";
            if (!CheckDuplicatedText()) msg += "\n- A section with the same name already exists.";
            if (!CheckDisplayRight()) msg += "\n- Display Right field is not valid.";
            if (!CheckModificationRight()) msg += "\n- Modification Right field is not valid.";
            return msg;
        }

        private bool CheckDuplicatedText()
        {
            foreach (Section s in Model.getInstance().Sections)
                if (s.RealText == section.RealText) return true;
            return false;
        }

        private bool CheckText()
        {
            String text = TokenTextTranslator.TranslateFromTextFile(NameTextBox.Text);
            text.Trim();

            if (text != "" && text != null)
            {
                this.SectionName = text;
                this.SectionRealName = NameTextBox.Text;
                return true;
            }
            return false;
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                OkButton.PerformClick();
        }

        private bool CheckDisplayRight()
        {
            char[] text = displayRightTextBox.Text.ToArray();
            for (int i = 2; i < text.Length; i++)
                if (text[i] > 'F') return false;
            return true;
        }

        private bool CheckModificationRight()
        {
            char[] text = modificationRightTextBox.Text.ToArray();
            for (int i = 2; i < text.Length; i++)
                if (text[i] > 'F') return false;
            return true;
        }

        private void SetLabels()
        {
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                this.sectionLabel.Text = "Section : " + this.section.text;
                this.NameLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 4).Value;
                this.displayRightLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 17).Value;
                this.modificationRightLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 18).Value;
                this.HintLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 7).Value;
                this.OkButton.Text = texts.Single(x => (int?)x.Attribute("id") == 21).Value;
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading texts for the Section Editor Form.");
                Model.getInstance().logCreator.Append("[ERROR] There was a problem reading texts for the Section Editor form.");
            }
        }
    }
}
