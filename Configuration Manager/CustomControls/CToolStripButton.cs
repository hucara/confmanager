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
        public int RelatedTabPageIndex;
        public int TypeId { get; private set; }
        public int gId { get; private set; }

        //public List<ICustomControl> RelatedControls { get; private set; }

        public CToolStripButton()
        {
            this.TypeId = count;
            this.Name = "CToolStripButton" + count;
            this.Text = this.Name;
           
            count++;
        }

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
