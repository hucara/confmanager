using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Debug = System.Diagnostics.Debug;
using System.Drawing;

namespace Configuration_Manager.CustomControls
{
    class CustomHandler
    {
		const int RGBMAX = 255;

        public ContextMenuStrip contextMenu;
        Model model;
        Editor editor;
        ControlFactory cf;

		Timer t = new Timer();

        public CustomHandler(ContextMenuStrip cms)
        {
            this.contextMenu = cms;
            this.model = Model.getInstance();
            this.cf = ControlFactory.getInstance();
			
			this.t.Interval = 800;
			this.t.Tick += TimerElapsed;
        }

        public void Control_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;
            String type = sender.GetType().Name;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                model.CurrentClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                SetContextMenuStrip(type);

                contextMenu.Show(c, me.X, me.Y);

                Debug.WriteLine("! Clicked: " + c.Name);
                Debug.WriteLine("! Clicked: " + model.CurrentClickedControl + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
            }
        }

        public void CTextBox_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;
            String type = sender.GetType().Name;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                c.ContextMenuStrip = contextMenu;

                model.CurrentClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                SetContextMenuStrip(type);

                contextMenu.Show(c, me.X, me.Y);
            }
            else if(!model.progMode && me.Button == MouseButtons.Right)
            {
                c.ContextMenuStrip = null;

                model.CurrentClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                SetContextMenuStrip(type);
            }
			else if (model.progMode && me.Button == MouseButtons.Left)
			{

			}
        }

        private void SetContextMenuStrip(string type)
        {
            enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, -1, true);

            if (type == "CGroupBox" || type == "CPanel")
            {
                // Editable Containers
                contextMenu.Items[0].Enabled = true;  // Disable the New> option
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;
            }
            else if (type == "TabPage")
            {
                // Section Tabs
                contextMenu.Items[0].Enabled = true;

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 9, false);

                contextMenu.Items[1].Enabled = false;
                contextMenu.Items[2].Enabled = false;
            }
            else if (type == "CTabPage")
            {
                // Custom Tabs
                contextMenu.Items[0].Enabled = true;

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 9, false);

                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;

                // Check if it is the only and last tab inside the CTabcontrol
                CTabPage p = model.CurrentClickedControl as CTabPage;
                if ((p.Parent as CTabControl).TabCount <= 1)
                {
                    contextMenu.Items[2].Enabled = false;
                }
            }
            else if (type == "CTabControl")
            {
                contextMenu.Items[0].Enabled = true;

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 9, true);

                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;
            }
            else
            {
                // Not a container
                contextMenu.Items[0].Enabled = false;
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;
            }
        }

        //////////////////////////////////////////////
        // Enables or disables the items inside a ToolStripMenu
        // if index > -1, applies the status to that item[index] and the !status to the rest.
        // if index = -1, applies the status to all the items inside the ToolStripMenu.
        //////////////////////////////////////////////
        private void enableDropDownItems(ToolStripMenuItem it, int index, bool status)
        {
            if (index > -1)
            {
                it.DropDownItems[index].Enabled = status;

                for (int i = 0; i < it.DropDownItems.Count; i++)
                {
                    if (i != index)
                    {
                        it.DropDownItems[i].Enabled = !status;
                    }
                }
            }
            else
            {
                for (int i = 0; i < it.DropDownItems.Count; i++)
                {
                    it.DropDownItems[i].Enabled = status;
                }
            }
        }

        public void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLabel label = cf.BuildCLabel(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(label);
        }

        public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTextBox textBox = cf.BuildCTextBox(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(textBox);
        }

        public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CComboBox comboBox = cf.BuildCComboBox(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(comboBox);
        }

        public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCheckBox checkBox = cf.BuildCCheckBox(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(checkBox);
        }

        public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CGroupBox groupBox = cf.BuildCGroupBox(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(groupBox);
        }

        public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CPanel panel = cf.BuildCPanel(model.CurrentClickedControl);

            editor = new Editor();
            editor.Show(panel);
        }

        public void tabControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTabControl tabControl = cf.BuildCTabControl(model.CurrentClickedControl);

            CTabPage tab = tabControl.TabPages[0] as CTabPage;
            tab.cd.Parent = tabControl;

            tabControl.MouseDown += Control_RightClick;
            tabControl.TabPages[0].Click += Control_RightClick;

            editor = new Editor();
            editor.Show(tabControl);
        }

        public void tabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTabPage tabPage = cf.BuildCTabPage(model.CurrentClickedControl);
            tabPage.MouseDown += Control_RightClick;

            editor = new Editor();
            editor.Show(tabPage);
        }

        public void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor = new Editor();

            model.LastClickedX = model.CurrentClickedControl.Location.X;
            model.LastClickedY = model.CurrentClickedControl.Location.Y;

            editor.Show(model.CurrentClickedControl);
        }

        public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control p = model.CurrentClickedControl.Parent;

			// In this function, the children controls will be deleted. 
			// There should be a recursive loop that goes inside each children control,
			// removing also those. From deepest level to highest level.
			if (model.CurrentClickedControl.Controls.Count > 0)
			{
				Control c = model.CurrentClickedControl;
				MessageBox.Show("This will kill all of my children.");

				DeleteChildren(model.CurrentClickedControl);
			}

			if (CheckRelations(model.CurrentClickedControl) == DialogResult.OK)
			{
				model.AllControls.Remove(model.CurrentClickedControl as ICustomControl);
				model.DeleteControlReferences(model.CurrentClickedControl);
				p.Controls.Remove(model.CurrentClickedControl);

				p.Refresh();
			}
        }

		private void DeleteChildren(Control c)
		{
			String ls = "";
			System.Diagnostics.Debug.WriteLine("! Now in: " + c.Name);
			foreach (Control child in c.Controls)
			{
				ls += ls + " " + child.Name;
				DeleteChildren(child);

				model.AllControls.Remove(child as ICustomControl);
				model.DeleteControlReferences(child);
			}
			System.Diagnostics.Debug.WriteLine(ls);
		}

		private DialogResult CheckRelations(Control control)
		{
			String msg = "";
			String references = "";
			String referencedBy = "";

			ICustomControl c = control as ICustomControl;
			
			// Check if this control has anything else inside the related list
			if (ControlReferencesOthers(c, out references))
			{
				msg += c.cd.Name + " is related to some other controls:\n";
				msg += references + "\n\n";
			}

			// Check if this control is inside the related lists of other controls
			if (ControlIsReferenced(c, out referencedBy))
			{
				msg += c.cd.Name + " is being related by some other controls:\n";
				msg += referencedBy;
			}

			return MessageBox.Show(msg, " Deleting control", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
		}

		private bool ControlIsReferenced(ICustomControl c, out String referencedBy)
		{
			referencedBy = "";

			foreach (ICustomControl co in model.AllControls)
			{
				if (co.cd.RelatedRead.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.RelatedWrite.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.RelatedVisibility.Contains(c)) referencedBy += co.cd.Name + " ";
				else if (co.cd.CoupledControls.Contains(c)) referencedBy += co.cd.Name + " ";
			}

			if (referencedBy != "") return true;
			return false;
		}

		private bool ControlReferencesOthers(ICustomControl c, out String references)
		{
			references = "";
			if (c.cd.RelatedRead.Count > 0)
			{
				references += "- Related read: ";
				foreach (Control co in c.cd.RelatedRead)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.RelatedWrite.Count > 0)
			{
				references += "- Related write: ";
				foreach (Control co in c.cd.RelatedWrite)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.RelatedVisibility.Count > 0)
			{
				references += "- Related visibility: ";
				foreach (Control co in c.cd.RelatedVisibility)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (c.cd.CoupledControls.Count > 0)
			{
				references += "- Coupled controls: ";
				foreach (Control co in c.cd.CoupledControls)
				{
					references += (co as ICustomControl).cd.Name + " ";
				}
				references += "\n";
			}

			if (references != "") return true;
			return false;
		}

		public void TextChanged(object sender, EventArgs e)
		{
			ICustomControl c = sender as ICustomControl;

			if ((sender as Control).Text == "") c.cd.Text = "0";
			else c.cd.Text = (sender as Control).Text;
		}

		public void Control_LeftClick(object sender, EventArgs e)
		{
			MouseEventArgs me = e as MouseEventArgs;
			Control c = sender as Control;
			String type = sender.GetType().Name;

			Rectangle rect = default(Rectangle);
			Pen p = new Pen(SystemColors.Highlight, 1);
			Graphics g = c.Parent.CreateGraphics();

			if (model.progMode && me.Button == MouseButtons.Left)
			{
				t.Start();
				Debug.WriteLine("! Timer Started");

				model.CurrentSection.Tab.Refresh();

				model.CurrentClickedControl = c;
				model.LastClickedX = me.X;
				model.LastClickedY = me.Y;

				rect = c.Bounds;
				rect.Inflate(1, 1);
				g.DrawRectangle(p, rect);

				Debug.WriteLine("! Clicked: " + model.CurrentClickedControl.Name + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
			}
		}

		private void TimerElapsed(object sender, EventArgs e)
		{
			(sender as Timer).Stop();
			MouseEventArgs me = e as MouseEventArgs;

			if (model.CurrentClickedControl != null)
			{
				String name = (model.CurrentClickedControl as ICustomControl).cd.Name;
				Debug.WriteLine("! Got the control: " + name);
				model.CurrentClickedControl.DoDragDrop(name, DragDropEffects.Move);
			}
		}

		public void OnDragDrop(object sender, DragEventArgs dea)
		{
			//dea.Effect = DragDropEffects.Move;
			String name;
			ICustomControl c = null;
			Control parent = sender as Control;
			
			if ((sender as ICustomControl).cd.Type == "CGroupBox")
			{
				name = (string)dea.Data.GetData(typeof(System.String));
				c = model.AllControls.Find(control => control.cd.Name == name);
			}

			c.cd.Parent = parent;
			parent.Refresh();

			Point cord = new Point(dea.X, dea.Y);
			cord = parent.PointToClient(cord);
			c.cd.Top = cord.Y;
			c.cd.Left = cord.X;

			Debug.WriteLine("! Dropped the control: " + c.cd.Name+ " Parent: " +c.cd.Parent.Name+" X: "+cord.X+" Y: "+cord.Y);
		}

		public void OnDragEnter(object sender, DragEventArgs dea)
		{
			dea.Effect = DragDropEffects.Move;
		}

		public void CancelDragDropTimer(object sender, EventArgs me)
		{
			if ((me as MouseEventArgs).Button == MouseButtons.Left)
			{
				t.Stop();
				Debug.WriteLine("! Timer Stopped");
			}
		}
	}
}
