using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CTabControl : System.Windows.Forms.TabControl, ICustomControl
    {
        static int count = 0;

        public int TypeId;

        public CTabControl()
        {
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
