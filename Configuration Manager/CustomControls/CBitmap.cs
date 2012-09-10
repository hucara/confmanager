using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CBitmap : System.Windows.Forms.PictureBox, ICustomControl
    {
        public static int count = 0;
        public ControlDescription cd;

        public CBitmap()
        {
            this.Name = "CBitmap" + count;
            this.DoubleBuffered = true;
            count++;
        }

        public void SetControlDescription()
        {
            cd = new ControlDescription(this);
        }

        ControlDescription ICustomControl.cd
        {
            get { return cd; }
            set { cd = value; }
        }
    }
}