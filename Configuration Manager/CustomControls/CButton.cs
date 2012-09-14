using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CButton : Button, ICustomControl
    {
        public static int count = 0;
        public ControlDescription cd;

        public CButton()
        {
            this.Name = "CButton" + count;
            this.DoubleBuffered = true;
            count++;
        }

        public void SetControlDescription()
        {
            cd = new ControlDescription(this);
        }

        ControlDescription ICustomControl.cd
        {
            get { return cd; }
            set { cd = value; }
        }
    }
}
