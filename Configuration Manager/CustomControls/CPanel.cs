using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CPanel: System.Windows.Forms.Panel, ICustomControl
    {
        static int count = 0;
        public ControlDescription cd;

        public CPanel()
        {
            this.Name = "CPanel" + count;
			this.AllowDrop = true;

            this.Text = this.Name;
            count++;
        }

        public void SetControlDescription()
        {
            cd = new ControlDescription(this);
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
