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

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Resources res = Resources.getInstance();
        private Model model = Model.getInstance();
        private ControlFactory cf = ControlFactory.getInstance();
        private ObjectDefinitionManager odm = ObjectDefinitionManager.getInstance();

        private Editor editor;

        TabControlView tabControlView;
        ToolStripView toolStripView;
        CustomHandler ch;


        public MainForm()
        {
            InitializeComponent();
            InitCustomHandler();
            InitViews();
            InitHandlers();
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
            tabControlView = new TabControlView(tabControl, ch);
            toolStripView = new ToolStripView(toolStrip, tabControl, contextMenu, model);

            if (model.ObjectDefinitionExists)
            {
                odm.SetDocument(res.ConfigObjects);
                odm.RestoreOldUI();
                toolStripView.readAndShow();
                tabControlView.readAndShow();
            }
        }

        private void SetProgMode()
        {
            if (model.progMode == false)
            {
                model.progMode = true;
                Debug.WriteLine("** INFO ** Programmer mode ACTIVE.");
            }
            else
            {
                model.progMode = false;
                Debug.WriteLine("** INFO ** Programmer mode INACTIVE.");
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
                odm.SerializeObjectDefinition();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.P)
            {
                System.Diagnostics.Debug.WriteLine("\n! PRINTING LIST OF CONTROLS !");
                foreach (ICustomControl c in model.AllControls)
                {
                    String line = "- ";
                    if (c.cd.Name != null) line += c.cd.Name + " -- ";
                    if (c.cd.Parent != null) line += c.cd.Parent.Name;
                    System.Diagnostics.Debug.WriteLine(line);
                }
                System.Diagnostics.Debug.WriteLine("\n! ######################### !");
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
