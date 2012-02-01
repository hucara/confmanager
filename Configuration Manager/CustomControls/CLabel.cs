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

        private Label c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CLabel(Label label)
        {
            this.c = label;

            this.Name = "CLabel" + count;
            count++;
        }

        public Label GetLabel()
        {
            return c;
        }

        public void SetControlDescription(ControlDescription cd)
        {
            this.c.Text = " HO LA ";
            this.c.Top = 30;
            this.c.Left = 40;
        }
    }
}
