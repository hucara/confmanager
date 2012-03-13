using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    public class CTabPage : TabPage, ICustomControl
    {
        static int count = 0;
        public ControlDescription cd = null;

        public CTabPage()
        {
            this.Name = "CTabPage" + count;
            this.Text = this.Name;
			this.AllowDrop = true;

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

        public void SetControlDescription()
        {
            cd = new ControlDescription(this);
        }
    }
}