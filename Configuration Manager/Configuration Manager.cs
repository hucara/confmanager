﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Views;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;
using System.Xml.Linq;
using Configuration_Manager.RelationManagers;

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Model model = Model.getInstance();
        //private ControlFactory cf = ControlFactory.getInstance();
        private ObjectDefinitionManager odm = ObjectDefinitionManager.getInstance();

        SectionTabsView sectionTabsView;
        SectionMenuView sectionMenuView;
        CustomHandler ch;

        public MainForm()
        {
            this.DoubleBuffered = true;

            InitializeComponent();

            model.ReadConfigurationFile();
            SetUpMainForm();

            PrintWellcomeLogMessage();

            InitCustomHandler();
            InitViews();
            InitHandlers();
        }

        private void PrintWellcomeLogMessage()
        {
            model.logCreator.Append(" ");
            model.logCreator.AppendDividingLine();
            model.logCreator.AppendCenteredWithFrame("");
            model.logCreator.AppendCenteredWithFrame(" Configuration Manager ");
            model.logCreator.AppendCenteredWithFrame("");
            model.logCreator.AppendDividingLine();
            model.logCreator.Append(" ");
        }

        private void SetUpMainForm()
        {
            this.TopMost = model.stayOnTop;
            this.Width = model.width;
            this.Height = model.height;
            this.Top = model.top;
            this.Left = model.left;

            if (model.resizable)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            else
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            if (!model.movable)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

        }

        private void InitCustomHandler()
        {
            ch = new CustomHandler(contextMenu);
            ControlFactory.SetCustomHandler(ch);
        }

        private void InitHandlers()
        {
            this.labelToolStripMenuItem.Click += ch.labelToolStripMenuItem_Click;
            this.textBoxToolStripMenuItem.Click += ch.textBoxToolStripMenuItem_Click;
            this.checkBoxToolStripMenuItem.Click += ch.checkBoxToolStripMenuItem_Click;
            this.comboBoxToolStripMenuItem.Click += ch.comboBoxToolStripMenuItem_Click;

            this.groupBoxToolStripMenuItem.Click += ch.groupBoxToolStripMenuItem_Click;
            this.shapeToolStripMenuItem.Click += ch.shapeToolStripMenuItem_Click;

            this.tabControlMenuItem.Click += ch.tabControlToolStripMenuItem_Click;
            this.tabPageMenuItem.Click += ch.tabPageToolStripMenuItem_Click;

            this.editToolStripMenuItem.Click += ch.editToolStripMenuItem_Click;
            this.deleteToolStripMenuItem.Click += ch.deleteToolStripMenuItem_Click;
        }

        private void InitViews()
        {
            sectionTabsView = new SectionTabsView(tabControl, ch);
            sectionMenuView = new SectionMenuView(sectionBar, tabControl, contextMenu);

            if (model.ObjectDefinitionExists)
            {
                odm.SetDocument(model.ConfigObjects);
                odm.RestoreOldUI();
                sectionMenuView.readAndShow();
                sectionTabsView.readAndShow();
            }
            else
            {
                model.logCreator.AppendCenteredWithFrame(" ! Object Definition file not found ! ");
                model.logCreator.AppendCenteredWithFrame(" Creating new empty UI ");
                model.logCreator.Append(" ");
            }
        }

        private void SwitchProgMode()
        {
            model.SwitchProgrammingMode();

            if (model.progMode)
            {
                sectionBar.BackColor = System.Drawing.Color.LightPink;
            }
            else
            {
                sectionBar.BackColor = System.Windows.Forms.ProfessionalColors.ToolStripBorder;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Control && e.KeyCode == Keys.P)
            {
                SwitchProgMode();
                this.Refresh();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.S)
            {
                odm.SerializeObjectDefinition();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.G)
            {
                WriteConfigurationManager.SaveChanges();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.D)
            {
                System.Diagnostics.Debug.WriteLine("\n###################################");
                System.Diagnostics.Debug.WriteLine("! Printing Sections:");

                foreach (Section s in model.Sections)
                {
                    System.Diagnostics.Debug.WriteLine(s.Name + " " + s.Tab.GetType().Name + ":" + s.Tab.Name + " " + s.Button.GetType().Name + ":" + s.Button.Text);
                }

                System.Diagnostics.Debug.WriteLine("-----------------------------------");
                System.Diagnostics.Debug.WriteLine("! Printing List of Controls: ");
                System.Diagnostics.Debug.WriteLine("\tControl        Parent");
                System.Diagnostics.Debug.WriteLine("___________________________________");
                foreach (ICustomControl c in model.AllControls)
                {
                    String line = "\t";
                    if (c.cd.Name != null) line += c.cd.Name + " ..... ";
                    if (c.cd.Parent != null) line += c.cd.Parent.Name;
                    System.Diagnostics.Debug.WriteLine(line);
                }
                System.Diagnostics.Debug.WriteLine("#####################################");
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.R)
            {
                System.Diagnostics.Debug.WriteLine("\n###################################");
                foreach (ICustomControl c in model.AllControls)
                {
                    string line = "\t";
                    line += "- "+ c.cd.Name + "\t\t Display: " + c.cd.operatorVisibility + "\t Modify: " + c.cd.operatorModification;
                    System.Diagnostics.Debug.WriteLine(line);
                }
            }
        }

        private void newSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SectionForm sf = new SectionForm();
            if (DialogResult.OK == sf.ShowDialog())
            {
                sectionMenuView.AddNewSection(sf.SectionName);
                sectionTabsView.readAndShow();
            }
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String msg = "This will delete the section and all of its children.";
            DialogResult dr = MessageBox.Show(msg, " Deleting Section", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                sectionMenuView.RemoveSection();
                sectionTabsView.readAndShow();
            }
        }

        private void toolStrip_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                if (sectionBar.GetItemAt(me.X, me.Y) is CToolStripButton)
                {
                    sectionBar.GetItemAt(me.X, me.Y).PerformClick();
                    deleteSectionToolStripMenuItem.Enabled = true;
                    editSectionNameToolStripMenuItem.Enabled = true;
                    sectionMenuView.SetSelectedButton(sectionBar.GetItemAt(me.X, me.Y) as CToolStripButton);
                }
                else
                {
                    deleteSectionToolStripMenuItem.Enabled = false;
                    editSectionNameToolStripMenuItem.Enabled = false;
                }

                if (model.Sections.Count >= Model.getInstance().maxSections)
                    newSectionToolStripMenuItem.Enabled = false;
                else
                    newSectionToolStripMenuItem.Enabled = true;

                contextNavMenu.Show(c, me.X, me.Y);
            }
        }

        private void toolStrip_SizeChanged(object sender, EventArgs e)
        {
            model.sectionMenuWidth = (sender as ToolStrip).Size.Width;
        }

        private void ReplaceLabels()
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                this.Text = texts.Single(x => (int?)x.Attribute("id") == 26).Value;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an error reading text for main form labels.");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Model.getInstance().uiChanged)
            {
                String msg = "Some changes were made to the User Interface. Do you want to save before closing?";
                DialogResult dr = MessageBox.Show(msg, "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    odm.SerializeObjectDefinition();
                    e.Cancel = false;
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                    e.Cancel = false;
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                    e.Cancel = true;
            }

            foreach (ICustomControl c in Model.getInstance().AllControls)
            {
                if (c.cd.Changed && c.cd.MainDestination != "")
                {
                    String msg = "Some changes were made to the configuration. Do you want to save before closing?";
                    DialogResult dr = MessageBox.Show(msg, "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        WriteConfigurationManager.SaveChanges();
                        e.Cancel = false;
                    }
                    else if (dr == System.Windows.Forms.DialogResult.No)
                        e.Cancel = false;
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        e.Cancel = true;

                    break;
                }
            }

            SaveCurrentLocation();
        }

        private void SaveCurrentLocation()
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(model.ConfigFilePath);
                XElement settings = xdoc.Element("ConfigurationManager").Element("Settings");

                settings.Element("Top").Value = this.Top.ToString();
                settings.Element("Left").Value = this.Left.ToString();

                xdoc.Save(model.ConfigFilePath);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an error writing location in config file.");
            }
        }

        private void editSectionNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            CToolStripButton tb = null;


            foreach (Section s in model.Sections)
            {
                if (s.Selected) tb = s.Button;
            }

            if (tb != null)
            {
                SectionForm sf = new SectionForm(tb.Text);

                if (DialogResult.OK == sf.ShowDialog())
                {
                    sectionMenuView.RenameSection(tb.Text, sf.Controls.Find("NameTextBox", false)[0].Text);
                    sectionTabsView.readAndShow();
                }
            }
        }
    }
}
