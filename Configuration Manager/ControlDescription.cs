using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Configuration_Manager.CustomControls;
using System.Xml.Linq;

namespace Configuration_Manager
{
    public class ControlDescription
    {
        static int count = 0;

		private String name;
        public String Hint { get; set; }
        private String text;
		public String RealText { get; set; }
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

		private ComboBox.ObjectCollection comboBoxItems;
		private List<String> comboBoxRealItems;

        private Section parentSection;

        public ControlDescription(Control c)
        {
            this.control = c;
            this.parent = c.Parent;
            this.Type = c.GetType().Name;

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

			if (this.Type == "CComboBox")
			{
				this.comboBoxItems = new ComboBox.ObjectCollection(control as ComboBox);
				this.comboBoxRealItems = new List<string>();
			}
			
            this.parentSection = Model.getInstance().CurrentSection;

            this.control.BringToFront();

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

		public String Name
		{
			get { return this.name; }
			set
			{
				this.control.Name = value;
				this.name = value;
			}
		}

        public String Text
        {
            get { return this.text; }
            set 
            {
                this.control.Text = value;
                this.text = value;
				if (this.Type == "CTextBox") this.RealText = value;
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

        public Section ParentSection
        {
            get { return this.parentSection;}
            set { this.parentSection = value; }
        }

		public ComboBox.ObjectCollection ComboBoxItems
		{
			get { return this.comboBoxItems; }
			set
			{
				this.comboBoxItems = value;
				(this.control as ComboBox).Items.Clear();
				foreach (ComboBox.ObjectCollection i in comboBoxItems)
				{
					(this.control as ComboBox).Items.Add(i);
				}
			}
		}

		public List<String> ComboBoxRealItems
		{
			get { return this.comboBoxRealItems; }
			set
			{
				this.comboBoxRealItems = value;
			}
		}
    }
}
