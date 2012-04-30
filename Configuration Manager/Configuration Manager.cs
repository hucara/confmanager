using System;
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

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Model model = Model.getInstance();
        private ControlFactory cf = ControlFactory.getInstance();
        private ObjectDefinitionManager odm = ObjectDefinitionManager.getInstance();

        private WriteRelationManager wrm = new WriteRelationManager();

        private Editor editor;

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

            if (model.resizable) this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        }

        private void InitCustomHandler()
        {
            ch = new CustomHandler(contextMenu);
            cf.SetCustomHandler(ch);
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
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.S)
            {
                odm.SerializeObjectDefinition();
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

                wrm.TranslateSubDestiantions();
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
            sectionMenuView.RemoveSection();
            sectionTabsView.readAndShow();
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
                    sectionMenuView.SetSelectedButton(sectionBar.GetItemAt(me.X, me.Y) as CToolStripButton);
                }
                else
                {
                    deleteSectionToolStripMenuItem.Enabled = false;
                }

                if (model.Sections.Count >= Model.getInstance().maxSections)
                {
                    newSectionToolStripMenuItem.Enabled = false;
                }
                else
                {
                    newSectionToolStripMenuItem.Enabled = true;
                }

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
    }
}
