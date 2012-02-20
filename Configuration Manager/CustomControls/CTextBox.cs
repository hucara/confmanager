﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CTextBox : TextBox, ICustomControl
    {
        static int count = 0;
        ControlDescription cd;

        public CTextBox()
        {
            this.Name = "TextBox" + count;
            count++;
        }

        public void SetControlDescription(ControlDescription cd)
        {}

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
