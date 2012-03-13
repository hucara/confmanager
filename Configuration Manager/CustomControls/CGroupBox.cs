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
        public ControlDescription cd;

        public CGroupBox()
        {
            this.Name = "CGroupBox" + CGroupBox.count;
            this.Text = this.Name;
			this.DoubleBuffered = true;
			this.AllowDrop = true;

            CGroupBox.count++;

            System.Diagnostics.Debug.WriteLine("+ Created: "+this.Name);
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
