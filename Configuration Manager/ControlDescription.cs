using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager
{
    class ControlDescription
    {
        int Id;
        int Top, Left, Width, Height;
        String Name, Text, Hint, Type;

        System.Drawing.Color BackColor, ForeColor;

        System.Drawing.Font Font;

        List<Control> RelatedControls;
        Control parent;

        public ControlDescription()
        {
        }

        public void getDescriptionFromClick(object sender, EventArgs e)
        {
            // takes coords, etc from the mouse possition when clicked
            this.Top = 0;
            this.Left = 0;
            this.Width = 0;
            this.Height = 0;
        }

        public void getDescriptionFromObjectDescriber()
        {
            // takes the parameters introduced in the edit form
            this.Text = "Text";
            this.Name = "Name";
            this.Hint = "Hint";
            this.Type = "Type";

            //this.BackColor = null;
            //this.ForeColor = null;

            this.Font = null;
        }
    }
}
