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

        private TabPage c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CTabPage(TabPage tabPage)
        {
            this.c = tabPage;

            this.Name = "CTabPage" + count;
            this.Text = this.Name;
            count++;
        }

        public TabPage GetTabPage()
        {
            return c;
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
