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

        private TextBox c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CTextBox(TextBox textBox)
        {
            this.c = textBox;

            this.Name = "TextBox" + count;
            count++;
        }

        public TextBox getTextBox()
        {
            return c;
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }
    }
}
