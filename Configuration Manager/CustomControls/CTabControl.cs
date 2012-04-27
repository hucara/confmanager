using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CTabControl : System.Windows.Forms.TabControl, ICustomControl
    {
        public ControlDescription cd;
        public static int count = 0;
        
        public CTabControl(CTabPage tp)
        {
            this.Name = "CTabControl" + count;
            this.TabPages.Add(tp);
            count++;
        }

        public CTabControl()
        {
            this.Name = "CTabControl" + count;

            CTabPage ctp = ControlFactory.getInstance().BuildCTabPage(this);
            ctp.SetControlDescription();
            ctp.cd.Parent = this;

            this.TabPages.Add(ctp);
            count++;
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
