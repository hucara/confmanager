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
        public int TypeId;
        public int RelatedButtonIndex;

        //public List<ICustomControl> RelatedControls { get; private set; }

        public CTabPage()
        {
            this.TypeId = count;
            this.Name = "CTabPage" + count;
            this.Text = this.Name;
            count++;
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
