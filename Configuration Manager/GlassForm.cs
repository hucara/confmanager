using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager
{
    public partial class GlassForm : Form
    {
        private Size Size;
        private Point Location;
        private Form ToCover;
        public Control DragControl;

        public GlassForm(Form toCover, Size size, Point location)
        {
            this.BackColor = Color.DarkGray;
            this.Opacity = 0.30;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            //this.Location = this.Location;

            this.ToCover = toCover;
            this.Size = size;
            this.Location = location;

            InitializeComponent();
        }

        public GlassForm(Size size, Point location)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.BackColor = Color.DarkGray;
            this.Opacity = 0.50;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;

            this.Height = size.Height;
            this.Width = size.Width;
            this.Top = location.Y + 62;
            this.Left = location.X - 181;
        }

        public void Show(Control dragControl, Point location)
        {
            this.DragControl = dragControl;
            this.DragControl.Location = location;
            this.Controls.Add(dragControl);
        }

        new public Control Close()
        {
            this.Close();
            return this.DragControl;
        }
    }
}
