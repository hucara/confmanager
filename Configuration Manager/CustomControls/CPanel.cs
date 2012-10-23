using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CPanel: System.Windows.Forms.Panel, ICustomControl
    {
        public static int count = 0;
        public ControlDescription cd;

        public CPanel()
        {
            this.Name = "CPanel" + count;
			this.AllowDrop = true;
            this.DoubleBuffered = true;

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
