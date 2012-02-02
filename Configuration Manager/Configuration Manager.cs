using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Views;

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Resources res = Resources.getInstance();
        private Model model = Model.getInstance();
        private ControlFactory cf = ControlFactory.getInstance();

        public MainForm()
        {
            InitializeComponent();
            InitViews();
        }

        private void InitViews()
        {
            TabControlView tabControlView = new TabControlView(tabControl, model);
            ToolStripView toolStripView = new ToolStripView(toolStrip, tabControl, model);

            if(model.ObjectDefinitionExists) toolStripView.ReadObjectDefinition();
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
                contextMenuStrip1.Show();
            }
        }
    }
}
