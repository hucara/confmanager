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

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Resources res = Resources.getInstance();
        private Model model = Model.getInstance();
        private ControlFactory cf = ControlFactory.getInstance();
        private ObjectDefinitionManager odr = ObjectDefinitionManager.getInstance();

        TabControlView tabControlView;
        ToolStripView toolStripView;

        public MainForm()
        {
            InitializeComponent();
            InitViews();
            InitHandlers();
        }

        private void InitHandlers()
        {
            this.labelToolStripMenuItem.Click += tabControlView.labelToolStripMenuItem_Click;
            this.textBoxToolStripMenuItem.Click += tabControlView.textBoxToolStripMenuItem_Click;
            this.checkBoxToolStripMenuItem.Click += tabControlView.checkBoxToolStripMenuItem_Click;
            this.comboBoxToolStripMenuItem.Click += tabControlView.comboBoxToolStripMenuItem_Click;

            this.groupBoxToolStripMenuItem.Click += tabControlView.groupBoxToolStripMenuItem_Click;
            this.shapeToolStripMenuItem.Click += tabControlView.shapeToolStripMenuItem_Click;

            this.tabControlMenuItem.Click += tabControlView.tabControlToolStripMenuItem_Click;
            this.tabPageMenuItem.Click += tabControlView.tabPageToolStripMenuItem_Click;

            //this.contextEditMenu.Opening += tabControlView.contextEditMenu_Opening;
            this.editToolStripMenuItem.Click += tabControlView.editToolStripMenuItem_Click;
            this.deleteToolStripMenuItem.Click += tabControlView.deleteToolStripMenuItem_Click;
        }

        private void InitViews()
        {
            tabControlView = new TabControlView(tabControl, contextEditMenu);
            toolStripView = new ToolStripView(toolStrip, tabControl, contextEditMenu, model);

            if (model.ObjectDefinitionExists)
            {
                odr.BuildDefinedSectionList(res.ConfigObjects);
                toolStripView.readAndShow();
                tabControlView.readAndShow();
                //toolStripView.SetDefinedSections(model.Sections);
            }
        }

        private void SetProgMode()
        {
            if (model.progMode == false)
            {
                model.progMode = true;
                System.Diagnostics.Debug.WriteLine("** INFO ** Programmer mode ACTIVE.");
            }
            else
            {
                model.progMode = false;
                System.Diagnostics.Debug.WriteLine("** INFO ** Programmer mode INACTIVE.");
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Control && e.KeyCode == Keys.P)
            {
                SetProgMode();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.S)
            {
                odr.SerializeObjectDefinition();
            }
        }

        private void newSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SectionForm sf = new SectionForm();
            sf.ShowDialog();
            toolStripView.AddNewSection(sf.SectionName);
            tabControlView.readAndShow();
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripView.RemoveSection();
            tabControlView.readAndShow();
        }

        private void toolStrip_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                if (toolStrip.GetItemAt(me.X, me.Y) is CToolStripButton)
                {
                    toolStrip.GetItemAt(me.X, me.Y).PerformClick();
                    deleteSectionToolStripMenuItem.Enabled = true;
                    toolStripView.SetSelectedButton(toolStrip.GetItemAt(me.X, me.Y) as CToolStripButton);
                }
                else
                {
                    deleteSectionToolStripMenuItem.Enabled = false;
                }

                if (model.Sections.Count >= Model.MAX_SECTIONS)
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
    }
}
