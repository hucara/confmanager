using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CLabel : Label, ICustomControl
    {
        static int count = 0;
        public int typeId;

        //private Label c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CLabel(Label label)
        {
            //this.c = label;

            this.Name = "CLabel" + count;
            count++;
        }

        public CLabel()
        {
            this.typeId = count;
            this.Name = "CLabel" + count;
            this.Text = this.Name;
            count++;
        }

        //public Label GetLabel()
        //{
        //    return c;
        //}

        public void SetControlDescription(ControlDescription cd)
        {
            this.Text = " HO LA ";
            this.Top = 30;
            this.Left = 40;
        }
    }
}
