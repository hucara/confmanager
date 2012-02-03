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
        public int typeId;

        private TextBox c;
        public List<ICustomControl> RelatedControls { get; private set; }

        public CTextBox(TextBox textBox)
        {
            this.c = textBox;

            typeId = count;
            this.Name = "TextBox" + count;
            count++;
        }

        public CTextBox()
        {
            typeId = count;
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
