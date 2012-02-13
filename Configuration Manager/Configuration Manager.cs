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
        private ObjectDefinitionReader odr = ObjectDefinitionReader.getInstance();

        TabControlView tabControlView;
        ToolStripView toolStripView;

        public MainForm()
        {
            InitializeComponent();
            InitViews();
        }

        private void InitViews()
        {
            tabControlView = new TabControlView(tabControl, contextEditMenu, model);
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
        }

        private void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLabel label = cf.BuildCLabel(null);
            tabControl.SelectedTab.Controls.Add(label);

            Editor editor = new Editor(label);
            //Editor editor = new Editor("CLabel", tabControl.SelectedTab);
            editor.Show();
        }

        private void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CButton cb = ControlFactory.getInstance().BuildCButton(null);
            tabControl.SelectedTab.Controls.Add(cb);
            tabControl.Refresh();
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

        private void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CComboBox ccb = cf.BuildCComboBox(null);
            tabControl.SelectedTab.Controls.Add(ccb);

            Editor editor = new Editor(ccb);
            //Editor editor = new Editor("CLabel", tabControl.SelectedTab);
            editor.Show();
        }

        private void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTextBox ctb = cf.BuildCTextBox(null);
            tabControl.SelectedTab.Controls.Add(ctb);

            Editor editor = new Editor(ctb);
            editor.Show();
        }
    }
}
