using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CGroupBox : GroupBox, ICustomControl
    {
        static int count = 0;
        ControlDescription cd;

        public CGroupBox()
        {
            this.Name = "CGroupBox" + count;
            this.Text = this.Name;
            count++;
        }

        public void SetControlDescription()
        {
            cd = new ControlDescription(this);
        }

        ControlDescription ICustomControl.cd
        {
            get
            {
                return cd;
            }
            set
            {
                cd = value;
            }
        }
    }
}
