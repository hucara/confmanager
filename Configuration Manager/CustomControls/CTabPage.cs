using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CTabPage : TabPage, ICustomControl
    {
        static int count = 0;
        ControlDescription cd = null;

        public CTabPage()
        {
            this.Name = "CTabPage" + count;
            this.Text = this.Name;
            count++;
        }

        internal void SetNavBarDescription(Section s)
        {
            throw new NotImplementedException();
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

        void SetControlDescription(ControlDescription cd)
        {
            throw new NotImplementedException();
        }
    }
}