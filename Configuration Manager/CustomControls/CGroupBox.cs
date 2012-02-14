using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CGroupBox : GroupBox, ICustomControl
    {
        static int count = 0;

        public int TypeId { get; private set; }
        public int Id { get; private set; }

        public List<ICustomControl> RelatedControls { get; private set; }

        public CGroupBox()
        {
            this.TypeId = count;
            this.Name = "CGroupBox" + count;

            this.Text = this.Name;
            this.Size = this.DefaultSize;
            this.Click += CGroupBox_Click;

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

        public void CGroupBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            CGroupBox c = sender as CGroupBox;

            if (Model.getInstance().progMode && me.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // Call the edit menu
                
                System.Diagnostics.Debug.WriteLine("! Clicked: " +c.Name+ ": in X: " +me.X+ " - Y: "+me.Y);
            }
        }
    }
}
