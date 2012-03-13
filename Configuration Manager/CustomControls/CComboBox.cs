using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CComboBox : ComboBox , ICustomControl
    {
        static int count = 0;
        public ControlDescription cd;

        public CComboBox()
        {
            this.Name = "CComboBox" + count;
            this.Size = this.DefaultSize;
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
