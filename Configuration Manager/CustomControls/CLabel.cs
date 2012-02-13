using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CLabel : Label, ICustomControl
    {
        static int count = 0;

        public int typeId { get; private set; }
        public int id { get; private set; }

        //private Label c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CLabel()
        {
            this.typeId = count;
            this.Name = "CLabel" + count;

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
