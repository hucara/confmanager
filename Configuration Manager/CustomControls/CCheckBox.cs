using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CCheckBox : CheckBox, ICustomControl
    {
        static int count = 0;

        public int TypeId { get; private set; }
        public int Id { get; private set; }

        public List<ICustomControl> RelatedControls { get; private set; }

        public CCheckBox()
        {
            this.TypeId = count;
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
    }
}
