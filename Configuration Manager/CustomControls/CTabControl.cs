using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CTabControl : System.Windows.Forms.TabControl, ICustomControl
    {
        ControlDescription cd;
        static int count = 0;
        
        public CTabControl()
        {
        }

        public void SetControlDescription(ControlDescription cd)
        {
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
