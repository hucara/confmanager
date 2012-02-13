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

        public List<ICustomControl> RelatedControls { get; private set; }

        public CComboBox()
        {
            this.Name = "ComboBox" + count;
            this.Size = this.DefaultSize;
            count++;
        }

        public void SetControlDescription(ControlDescription cd)
        {
            this.Text = cd.Text;
            this.Top = cd.Top;
            this.Left = cd.Left;
            this.Height = cd.Height;
            this.Width = cd.Width;

            this.Font = cd.CurrentFont;
            this.BackColor = cd.BackColor;
        }
    }
}
