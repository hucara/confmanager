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
            toolStripView = new ToolStripView(toolStrip, tabControl, model);

            if(model.ObjectDefinitionExists) toolStripView.ReadObjectDefinitionFile();
            tabControlView.SetProgModeHandlers();
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

        private void tabControl_Click(object sender, EventArgs e)
        {
            if (model.progMode)
            {
                System.Diagnostics.Debug.WriteLine("** INFO ** Clicked in tab.");
                contextEditMenu.Show();
            }
        }

        private void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CButton cb = ControlFactory.getInstance().BuildCButton(null);
            tabControl.SelectedTab.Controls.Add(cb);
            tabControl.Refresh();
        }

        private void newSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripView.AddNewToolStripButton();
        }

        private void toolStrip_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;

            if(model.progMode && me.Button == MouseButtons.Right)
            {
                toolStripView.SelectCToolStripButton(me.X, me.Y);
                contextNavMenu.Show(c, me.X, me.Y);
            }
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripView.RemoveToolStripButton();
        }
    }
}
