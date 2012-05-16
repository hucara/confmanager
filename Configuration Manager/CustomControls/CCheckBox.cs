using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CCheckBox : CheckBox, ICustomControl
    {
        public ControlDescription cd = null;
        public static int count = 0;
        
        public CCheckBox()
        {
            cd = new ControlDescription(this);

            cd.TypeId = count;
            this.Name = "CCheckBox" + count;
            this.Text = this.Name;
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
