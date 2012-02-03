using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CButton : Button, ICustomControl
    {
        static int count = 0;
        public int typeId;

        public List<ICustomControl> RelatedControls { get; private set; }

        public CButton()
        {
            this.typeId = count;
            this.Name = "CButton" + count;
            this.Text = this.Name;
            count++;

            printInfo();
        }

        private void printInfo()
        {
            if(Model.getInstance().progMode)
            {
                System.Diagnostics.Debug.WriteLine("+ Added: " +this.Name+ " - Parent: ");
            }
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
