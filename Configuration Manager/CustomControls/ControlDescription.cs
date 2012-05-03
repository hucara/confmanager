﻿using System;
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

        public Boolean Changed { get; set; }

        private String name = "";
        private String hint = "";
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

        public String DisplayRight { get; set; }
        public String ModificationRight { get; set; }

        private Control control;
        private Control parent;

        private bool currentVisibility;
        public bool userVisibility;

        public bool userModification;

        public int TypeId { get; set; }
        public int Id { get; set; }

        public String DestinationType { get; set; }
        public String MainDestination { get; set; }

        public String SubDestination { get; set; }          // SubDestination path already translated
        public String RealSubDestination { get; set; }      // SubDestination path without being translated

        private List<String> SubDestinationNodes;

        public List<ICustomControl> RelatedWrite { get; set; }
        public List<ICustomControl> RelatedRead { get; set; }
        public List<ICustomControl> RelatedVisibility { get; set; }
        public List<ICustomControl> CoupledControls { get; set; }

        public List<String> comboBoxItems;                 // Items containing the showing and translated values
        public List<String> comboBoxRealItems;             // Items containing the - still to translate - values
        public List<String> comboBoxConfigItems;           // Items containing the alias inside config files

        private Section parentSection;

        public ControlDescription(Control c)
        {
            this.Changed = false;
            this.control = c;
            this.parent = c.Parent;
            this.Type = c.GetType().Name;

            this.text = c.Text;
            this.Name = c.Name;
            this.currentFont = c.Font;
            this.backColor = c.BackColor;
            this.foreColor = c.ForeColor;
            this.currentVisibility = c.Visible;

            this.top = c.Top;
            this.left = c.Left;
            this.width = c.Width;
            this.height = c.Height;

            this.Id = count;

            this.RelatedRead = new List<ICustomControl>();
            this.RelatedWrite = new List<ICustomControl>();
            this.RelatedVisibility = new List<ICustomControl>();
            this.CoupledControls = new List<ICustomControl>();

            this.SubDestinationNodes = new List<String>();

			if (this.Type == "CComboBox")
			{
                this.comboBoxItems = new List<String>();
                this.comboBoxRealItems = new List<String>();
                this.comboBoxConfigItems = new List<String>();
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
            }
        }

		public String Hint
		{
			get { return this.hint; }
			set
			{
				this.hint = value;
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

        public Boolean Visible
        {
            get { return this.currentVisibility; }
            set
            {
                this.control.Visible = value;
                this.currentVisibility = value;
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

        public bool Enabled {
            get { return this.control.Enabled; }
            set
            {
                this.control.Enabled = value;
            }
        }
    }
}