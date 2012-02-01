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

        private ComboBox c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CComboBox(ComboBox comboBox)
        {
            this.c = comboBox;

            this.Name = "ComboBox" + count;
            count++;
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
