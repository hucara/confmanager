using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    public partial class GlassForm : Form
    {
        public ICustomControl dragControl;
        private Model model;

        private Control oldParent;
        private Point oldLocation;

        private bool mouseOverForm = false;

        public GlassForm()
        {
            this.model = Model.getInstance();
            this.Opacity = .00;

            InitializeComponent();

            this.AllowDrop = true;
            this.DoubleBuffered = true;
            this.BackColor = Color.DarkBlue;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;

            this.DragDrop += new DragEventHandler(GlassForm_DragDrop);
            this.DragEnter += new DragEventHandler(GlassForm_DragEnter);
        }

        public void ActivateDragDrop()
        {
            this.Visible = true;
            this.Enabled = false;
            this.mouseOverForm = true;
            this.Activate();

            dragControl = model.CurrentClickedControl as ICustomControl;
            if (dragControl != null)
            {
                oldLocation = model.CurrentClickedControl.Location;
                oldParent = model.CurrentClickedControl.Parent;

                model.CurrentClickedControl.BringToFront();

                dragControl.cd.Parent.Controls.Remove(dragControl as Control);
                this.Controls.Add(dragControl as Control);

                this.Activate();
                for (double i = .00; i < 0.30; i += 0.05)
                {
                    this.Opacity = i;
                    System.Threading.Thread.Sleep(10);
                }

                System.Diagnostics.Debug.WriteLine("Starting Drag&Drop with: " + dragControl.cd.Name);
                dragControl.cd.Enabled = false;

                this.DoDragDrop(dragControl, DragDropEffects.Move);
            }
        }

        private void GlassForm_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
                Point cord = System.Windows.Forms.Cursor.Position;
                cord = this.PointToClient(cord);
                dragControl.cd.Top = cord.Y - model.LastClickedY;
                dragControl.cd.Left = cord.X - model.LastClickedX;
        }

        private void GlassForm_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point cord = System.Windows.Forms.Cursor.Position;

            this.Enabled = false;
            Control parent = GetTopChildOnCoordinates(model.CurrentSection.Tab, cord);
            this.Enabled = true;

            if (parent != dragControl && dragControl != null)
            {
                if (parent is CGroupBox || parent is CPanel || parent is CTabPage || parent is TabPage)
                {
                    cord = parent.PointToClient(cord);
                    dragControl.cd.Top = cord.Y - model.LastClickedY;
                    dragControl.cd.Left = cord.X - model.LastClickedX;

                    CheckSnapOnDrop(dragControl as Control, parent);

                    if(!(parent is CTabPage))
                    {
                        (dragControl as Control).Enabled = true;
                        this.Controls.Remove(model.CurrentClickedControl);
                        parent.Controls.Add(model.CurrentClickedControl);
                        (model.CurrentClickedControl as ICustomControl).cd.Parent = parent;
                    }
                    else
                    {
                        (dragControl as Control).Enabled = true;
                        int tpi = (parent.Parent as TabControl).SelectedIndex;
                        parent = (parent.Parent as TabControl).TabPages[tpi];
                        this.Controls.Remove(model.CurrentClickedControl);
                        parent.Controls.Add(model.CurrentClickedControl);
                        (model.CurrentClickedControl as ICustomControl).cd.Parent = parent;
                    }

                    System.Diagnostics.Debug.WriteLine("Dropped on: " + (dragControl as Control).Location);
                    System.Diagnostics.Debug.WriteLine("New parent: " + model.CurrentClickedControl.Parent.Name);
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    System.Diagnostics.Debug.WriteLine("That guy does not accept drops... ");
                    RestoreControlState();
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("You tried to drop it over itself... ");

            for (double i = .30; i > 0.00; i -= 0.05)
            {
                this.Opacity = i;
                System.Threading.Thread.Sleep(10);
            }

            this.Enabled = true;
            this.Visible = false;
        }

        private void RestoreControlState()
        {
            dragControl.cd.Parent = oldParent;
            dragControl.cd.Top = oldLocation.Y;
            dragControl.cd.Left = oldLocation.X;
            oldParent.Controls.Add(dragControl as Control);
            this.Controls.Remove(dragControl as Control);
            (dragControl as Control).Enabled = true;
        }

        void GlassForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null) e.Effect = DragDropEffects.Move;
        }


        private Control GetTopChildOnCoordinates(Control sectionTab, Point screenCoord)
        {
            Point relCoord = sectionTab.PointToClient(screenCoord);
            Control p = sectionTab.GetChildAtPoint(relCoord, GetChildAtPointSkip.Disabled);
            Control newParent = p;

            while (p != null)
            {
                relCoord = p.PointToClient(screenCoord);
                p = p.GetChildAtPoint(relCoord, GetChildAtPointSkip.Disabled);
                if (p != null) newParent = p;
            }

            if (newParent != null) return newParent;
            else return sectionTab;
        }

        public void CheckSnapOnDrop(Control control, Control parent)
        {
            ICustomControl c = control as ICustomControl;
            int margin = 10;
            int marginTop = 10;
            if (parent is TabPage) margin = 4;
            if (parent is CGroupBox) marginTop = 20;

            // Check Snaps to parent
            if (c.cd.Top < marginTop)
                c.cd.Top = marginTop;
            if (c.cd.Left < margin)
                c.cd.Left = margin;
            if (c.cd.Top + c.cd.Height > parent.Height - margin)
                c.cd.Top = parent.Height - c.cd.Height - margin;
            if (c.cd.Left + c.cd.Width > parent.Width - margin)
                c.cd.Left = parent.Width - c.cd.Width - margin;
        }

        private void GlassForm_DragLeave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Do you think you can leave with that?");
        }
    }
}
