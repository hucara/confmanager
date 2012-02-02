using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CToolStripButton : ToolStripButton, ICustomControl
    {
        static int count = 0;
        public int Id;

        //private ToolStripButton c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CToolStripButton(ToolStripButton tsButton)
        {
            //this.c = tsButton;
            this.Id = count;
            this.Name = "CToolStripButton" + count;
            this.Text = this.Name;
            count++;
        }

        public CToolStripButton()
        {
            this.Id = count;
            this.Name = "CToolStripButton" + count;
            this.Text = this.Name;
            count++;
        }

        //public ToolStripButton GetToolStripButton()
        //{
        //    //return this.c;
        //}

        public void SetControlDescription(ControlDescription cd)
        {
        }

        public void SetHandler()
        {
            this.Click += new EventHandler(ToolStripButton_Click);
        }

        public void ToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
