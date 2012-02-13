using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    /// Description class with the needed parameters to create a Control.
    /// If the Control needs / uses all of just some of the properties,
    /// it is defined in the build method of each Custom Control.

    class ControlDescription
    {
        public String Name { get; set; }
        public String Hint { get; set; }
        public String Text { get; set; }

        public Font CurrentFont { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Control Parent { get; set; }
        public bool Visible { get; set; }

        public int TypeId { get; set; }
        public int Id { get; set; }

        public byte VisibilityRgiht { get; set; }
        public byte ModificationRight { get; set; }

        public String DestinationType { get; set; }
        public String MainDestination { get; set; }
        public String SubDestination { get; set; }

        public List<ICustomControl> RelatedWrite { get; private set; }
        public List<ICustomControl> RelatedRead { get; private set; }
        public List<ICustomControl> RelatedVisibility { get; private set; }
        public List<ICustomControl> CoupledControls { get; private set; }

        public ControlDescription()
        {
        }

        public ControlDescription(String name, String text)
        {
            this.Name = name;
            this.Text = text;
        }

        public void SetFontProperties(Font font, Color BackgroundColor)
        {
            this.CurrentFont = font;
            this.BackColor = BackgroundColor;
        }

        public void SetSize(int Height, int Width)
        {
            this.Height = Height;
            this.Width = Width;
        }

        public void SetPosition(int Top, int Left)
        {
            this.Top = Top;
            this.Left = Left;
        }
    }
}
