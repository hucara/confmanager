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
    public partial class Form1 : Form
    {
        private Resources res = Resources.getInstance();
        private Model model = Model.getInstance();
        private ControlFactory cf = ControlFactory.getInstance();

        public Form1()
        {
            InitializeComponent();

            InitViews();


            //mainPanel.Controls.Add(cf.BuildCLabel(null));
            //mainPanel.Refresh();
        }

        private void InitViews()
        {
            ToolStripView toolStripView = new ToolStripView(toolStrip, tabControl, model);
            if(model.ObjectDefinitionExists) toolStripView.ReadObjectDefinition();
        }
    }
}
