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

        private int CELL_WIDTH = 100;
        private int CELL_HEIGHT = 40;

        private Control oldParent;
        private Point oldLocation;
        private ControlEditor editor;

        private bool mouseOverForm = false;

        public GlassForm()
        {
            this.model = Model.getInstance();
            this.Opacity = .00;

            InitializeComponent();

            CELL_HEIGHT = model.cellHeight;
            CELL_WIDTH = model.cellWidth;

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

        public void ActivateDragDrop(ControlEditor editor)
        {
            this.Visible = true;
            this.Enabled = false;
            this.mouseOverForm = true;
            this.editor = editor;
            this.Activate();

            dragControl = model.currentClickedControl as ICustomControl;
            if (dragControl != null)
            {
                oldLocation = model.currentClickedControl.Location;
                oldParent = model.currentClickedControl.Parent;
                model.currentClickedControl.BringToFront();

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
            if (mouseOverForm)
            {
                Point cord = System.Windows.Forms.Cursor.Position;
                cord = this.PointToClient(cord);
                dragControl.cd.Top = cord.Y - model.lastClickedY;
                dragControl.cd.Left = cord.X - model.lastClickedX;
                //CalculateSnapToGrid();
            }
            else
            {
                e.Action = DragAction.Cancel;
                this.Controls.Remove(dragControl as Control);
                RestoreControlState();

                for (double i = .30; i > 0.00; i -= 0.05)
                {
                    this.Opacity = i;
                    System.Threading.Thread.Sleep(30);
                }
            }
        }

        private void CalculateSnapToGrid()
        {
            Point location = this.PointToScreen((dragControl as Control).Location);
            Point cursor = System.Windows.Forms.Cursor.Position;

            // Get the distance between the control location and the closest -x-y grid lines.
            int difY = location.Y % CELL_HEIGHT;
            int difX = location.X % CELL_WIDTH;

            int newY = cursor.Y;
            int newX = cursor.X;

            if (difY < 5 && difY > 0) newY = cursor.Y + difY;
            if (difX < 5 && difX > 0) newX = cursor.X + difX;

            if (newY != cursor.Y || newX != cursor.X)
            {
                //System.Diagnostics.Debug.WriteLine("-- SNAPPING --");
                //System.Diagnostics.Debug.WriteLine("DifXY: " + difX + "," + difY);
                //System.Diagnostics.Debug.WriteLine("LocXY: " + location.X + "," + location.Y);
                //System.Diagnostics.Debug.WriteLine("CurXY: " + cursor.X + "," + cursor.Y);
                //System.Diagnostics.Debug.WriteLine("NewXY: " + newX + "," + newY);
                //System.Diagnostics.Debug.WriteLine("--------------");
                System.Windows.Forms.Cursor.Position = new Point(newX, newY);
            }
        }

        private void GlassForm_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point cord = System.Windows.Forms.Cursor.Position;

            //this.Enabled = false;
            Control parent = GetTopChildOnCoordinates(model.currentSection.Tab, cord);
            this.Enabled = true;

            if (parent != dragControl && dragControl != null)
            {
                if (parent is CGroupBox || parent is CPanel || parent is CTabPage || parent is TabPage)
                {
                    cord = parent.PointToClient(cord);
                    dragControl.cd.Top = cord.Y - model.lastClickedY;
                    dragControl.cd.Left = cord.X - model.lastClickedX;

                    CheckSnapOnDrop(dragControl as Control, parent);

                    if(!(parent is TabPage))
                    {
                        (dragControl as Control).Enabled = true;
                        (dragControl as Control).BringToFront();
                        this.Controls.Remove(model.currentClickedControl);
                        parent.Controls.Add(model.currentClickedControl);
                        (model.currentClickedControl as ICustomControl).cd.Parent = parent;
                    }
                    else
                    {
                        (dragControl as Control).Enabled = true;
                        (dragControl as Control).BringToFront();
                        int tabPageindex = (parent.Parent as TabControl).SelectedIndex;
                        parent = (parent.Parent as TabControl).TabPages[tabPageindex];
                        this.Controls.Remove(model.currentClickedControl);
                        parent.Controls.Add(model.currentClickedControl);
                        (model.currentClickedControl as ICustomControl).cd.Parent = parent;
                    }

                    model.uiChanged = true;
                    System.Diagnostics.Debug.WriteLine("Dropped on: " + (dragControl as Control).Location);
                    System.Diagnostics.Debug.WriteLine("New parent: " + model.currentClickedControl.Parent.Name);
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    System.Diagnostics.Debug.WriteLine("That guy does not accept drops... ");
                    RestoreControlState();
                }

                (dragControl as Control).BringToFront();
                parent.Invalidate();
                if(editor!= null) editor.ReadFromControl();
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
            this.Focus();
            (dragControl as Control).Update();
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

        private void GlassForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null) e.Effect = DragDropEffects.Move;
        }


        private Control GetTopChildOnCoordinates(Control sectionTab, Point screenCoord)
        {
            Point relCoord = sectionTab.PointToClient(screenCoord);
            Control p = sectionTab.GetChildAtPoint(relCoord, GetChildAtPointSkip.Disabled);
            
            if (p != null && p.Parent is CTabControl) 
                p = (p.Parent as TabControl).SelectedTab;
            
            Control newParent = p;

            while (p != null)
            {
                relCoord = p.PointToClient(screenCoord);
                p = p.GetChildAtPoint(relCoord, GetChildAtPointSkip.Disabled);
                if (p != null)
                {
                    if (p is CTabPage) p = (p.Parent as CTabControl).SelectedTab; 
                    System.Diagnostics.Debug.WriteLine(p.Name +":"+ p.PointToClient(screenCoord));
                    if (p.GetType().Equals(sectionTab)) return p;
                    newParent = p;
                }
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
            mouseOverForm = false;
        }

        // Override of the OnPaint event to draw the grid lines.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (int x = 0; x <= this.Width; x += CELL_WIDTH)
            {
                e.Graphics.DrawLine(System.Drawing.Pens.White, new Point(x, 0), new Point(x, this.Height));
                //e.Graphics.FillRectangle(System.Drawing.Brushes.DarkRed, new Rectangle(new Point(x, 0), new Size(this.Width, 5)));
            }

            for (int y = 0; y <= this.Height; y += CELL_HEIGHT)
            {
                e.Graphics.DrawLine(System.Drawing.Pens.White, new Point(0, y), new Point(this.Width, y));
                //e.Graphics.FillRectangle(System.Drawing.Brushes.DarkRed, new Rectangle(new Point(0, y), new Size(this.Width, 5)));
            }
        }
    }
}