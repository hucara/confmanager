using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CPanel: System.Windows.Forms.Panel, ICustomControl
    {
        static int count = 0;

        public int TypeId { get; private set; }
        public int Id { get; private set; }

        public List<ICustomControl> RelatedControls { get; private set; }

        public CPanel()
        {
            this.TypeId = count;
            this.Name = "CPanel" + count;

            this.Text = this.Name;
            this.Size = this.DefaultSize;
            this.BackColor = System.Drawing.Color.DarkOrange;
            this.Click += CPanel_Click;

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

        private void CPanel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs me = e as System.Windows.Forms.MouseEventArgs;
            CPanel c = sender as CPanel;

            if (Model.getInstance().progMode && me.Button == System.Windows.Forms.MouseButtons.Right)
            {
                System.Diagnostics.Debug.WriteLine("! Clicked: " + c.Name + ": in X: " + me.X + " - Y: " + me.Y);
            }
        }
    }
}
