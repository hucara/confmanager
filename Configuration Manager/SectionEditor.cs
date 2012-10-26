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
            this.NameTextBox.Focus();
        }

        public SectionEditor(Section section)
        {
            this.section = section;
            InitializeComponent();

            SetLabels();
            LoadSection();
            this.NameTextBox.Focus();
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
                String msg = Model.GetTranslationFromID(57) + ":\n" + ErrorMsg;
                String caption = Model.GetTranslationFromID(37);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSection()
        {
            this.section.RealText = this.NameTextBox.Text;
            this.section.Text = TokenTextTranslator.TranslateFromTextFile(this.NameTextBox.Text);

            this.section.DisplayRight = this.displayRightTextBox.Text.Substring(2);
            this.section.ModificationRight = this.modificationRightTextBox.Text.Substring(2);

            this.section.Hint = TokenTextTranslator.TranslateFromTextFile(this.hintTextBox.Text);
        }

        private String CheckAttributes()
        {
            String msg = "";
            if (!CheckText()) msg += "\n- " + Model.GetTranslationFromID(140);
            if (!CheckDuplicatedText()) msg += "\n- "+ Model.GetTranslationFromID(141);
            if (!CheckDisplayRight()) msg += "\n- " + Model.GetTranslationFromID(142);
            if (!CheckModificationRight()) msg += "\n- " + Model.GetTranslationFromID(143);
            return msg;
        }

        private bool CheckDuplicatedText()
        {
            foreach (Section s in Model.getInstance().sections)
                if(s != section && 
                    (s.RealText == this.NameTextBox.Text || s.Text == this.NameTextBox.Text)) return false;
            return true;
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
                xdoc = XDocument.Load(Model.getInstance().textsFilePath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                if(section.Text == "" || section.Text == null)
                    this.sectionLabel.Text = Model.GetTranslationFromID(120) + ": " + section.Name;
                else
                    this.sectionLabel.Text = Model.GetTranslationFromID(120) + ": " + section.Text;

                this.groupBox1.Text = " " +Model.GetTranslationFromID(12) +" ";
                this.NameLabel.Text = Model.GetTranslationFromID(4);
                this.displayRightLabel.Text = Model.GetTranslationFromID(17);
                this.modificationRightLabel.Text = Model.GetTranslationFromID(18);
                this.HintLabel.Text = Model.GetTranslationFromID(7);
                this.OkButton.Text = Model.GetTranslationFromID(21);
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading texts for the Section Editor Form.");
                Model.getInstance().logCreator.Append("[ERROR] There was a problem reading texts for the Section Editor form.");
            }
        }
    }
}
