using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CLabel : Label, ICustomControl
    {
        public static int count = 0;
        public ControlDescription cd;

        public CLabel()
        {
            SetName();
			this.DoubleBuffered = true;
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

        private void SetName()
        {
            this.Name = "CLabel" + count;
            this.Text = this.Name;
        }


    }
}
