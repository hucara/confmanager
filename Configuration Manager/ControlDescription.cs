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

    public class ControlDescription
    {
        static int count = 0;

        public String Name { get; set; }
        public String Hint { get; set; }
        private String text;
        public String Type { get; set; }

        private Font currentFont;
        private Color backColor;
        private Color foreColor;

        private int top;
        private int left;
        private int width;
        private int height;

        private Control control;
        private Control parent;
        public bool Visible { get; set; }

        public int TypeId { get; set; }
        public int Id { get; set; }

        public byte VisibilityRight { get; set; }
        public byte ModificationRight { get; set; }

        public String DestinationType { get; set; }
        public String MainDestination { get; set; }
        public String SubDestination { get; set; }

        public List<ICustomControl> RelatedWrite { get; set; }
        public List<ICustomControl> RelatedRead { get; set; }
        public List<ICustomControl> RelatedVisibility { get; set; }
        public List<ICustomControl> CoupledControls { get; set; }

        public ControlDescription(Control c)
        {
            this.control = c;
            this.parent = c.Parent;

            this.text = c.Text;
            this.Name = c.Name;
            this.currentFont = c.Font;
            this.backColor = c.BackColor;
            this.foreColor = c.ForeColor;
            this.Visible = c.Visible;

            this.top = c.Top;
            this.left = c.Left;
            this.width = c.Width;
            this.height = c.Height;

            this.Id = count;

            this.RelatedRead = new List<ICustomControl>();
            this.RelatedWrite = new List<ICustomControl>();
            this.RelatedVisibility = new List<ICustomControl>();
            this.CoupledControls = new List<ICustomControl>();

            count++;
        }

        public ControlDescription(String name, String text, String type)
        {
            this.Name = name;
            this.control.Name = name;
            this.text = text;
            this.control.Text = text;
            this.Type = type;
        }

        // Properties
        public Control Parent
        {
            get { return this.parent; }
            set
            {
                this.control.Parent = value;
                this.parent = value;
            }
        }

        public String Text
        {
            get { return this.text; }
            set 
            {
                this.control.Text = value;
                this.text = value;
            }
        }

        public Font CurrentFont
        {
            get { return this.currentFont; }
            set
            {
                this.control.Font = value;
                this.currentFont = value;
            }
        }

        public Color BackColor
        {
            get { return this.backColor; }
            set
            {
                this.control.BackColor = value;
                this.backColor = value;
            }
        }

        public Color ForeColor
        {
            get { return this.foreColor; }
            set
            {
                this.control.ForeColor = value;
                this.foreColor = value;
            }
        }

        public int Top {
            get { return this.top; }
            set
            {
                this.control.Top = value;
                this.top = value;
            }
        }

        public int Left
        {
            get { return this.left; }
            set
            {
                this.control.Left = value;
                this.left = value;
            }
        }

        public int Width
        {
            get { return this.width; }
            set
            {
                this.control.Width = value;
                this.width = value;
            }
        }

        public int Height
        {
            get { return this.height; }
            set
            {
                this.control.Height = value;
                this.height = value;
            }
        }
    }
}
