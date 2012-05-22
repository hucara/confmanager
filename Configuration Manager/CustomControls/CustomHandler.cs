using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;


using Debug = System.Diagnostics.Debug;
using System.Drawing;

namespace Configuration_Manager.CustomControls
{
	class CustomHandler
	{
		const int RGBMAX = 255;
        const bool DRAGDROP_ACTIVE = false;

		public ContextMenuStrip contextMenu;
		Model model;
		Editor editor;
        //ControlFactory cf;
		Rectangle previewRect = new Rectangle(0, 0, 0, 0);

		Timer t = new Timer(); // Drag and drop timer.

		public CustomHandler(ContextMenuStrip cms)
		{
			this.contextMenu = cms;
			this.model = Model.getInstance();
            //this.cf = ControlFactory.getInstance();

			this.t.Interval = 200;
			this.t.Tick += TimerTick;
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
                SetContextMenuModificationRights(c);

				contextMenu.Show(c, me.X, me.Y);
			}
			else if (!model.progMode && me.Button == MouseButtons.Right)
			{
				c.ContextMenuStrip = null;

				model.CurrentClickedControl = c;
				model.LastClickedX = me.X;
				model.LastClickedY = me.Y;

				SetContextMenuStrip(type);
                SetContextMenuModificationRights(c);
			}
			else if (model.progMode && me.Button == MouseButtons.Left)
			{

			}
		}

        private void SetContextMenuModificationRights(Control c)
        {
            ICustomControl co = c as ICustomControl;

            if (!co.cd.operatorModification)
            {
                //c.Enabled = true;
                //contextMenu.Items[1].Enabled = true;
            }
            else
            {
                //c.Enabled = false;
                //contextMenu.Items[1].Enabled = false;  // Disable the Edit option
            }
        }

		private void SetContextMenuStrip(string type)
		{
			enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, -1, true);

			if (type == "CGroupBox" || type == "CPanel")
			{
				// Editable Containers
				contextMenu.Items[0].Enabled = true;
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
			CLabel label = ControlFactory.BuildCLabel(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(label);
		}

		public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTextBox textBox = ControlFactory.BuildCTextBox(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(textBox);
		}

		public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CComboBox comboBox = ControlFactory.BuildCComboBox(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(comboBox);
		}

		public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CCheckBox checkBox = ControlFactory.BuildCCheckBox(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(checkBox);
		}

		public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CGroupBox groupBox = ControlFactory.BuildCGroupBox(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(groupBox);
		}

		public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CPanel panel = ControlFactory.BuildCPanel(model.CurrentClickedControl);

			editor = new Editor();
			editor.Show(panel);
		}

		public void tabControlToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabControl tabControl = ControlFactory.BuildCTabControl(model.CurrentClickedControl);
            CTabPage ctab = ControlFactory.BuildCTabPage(tabControl);

			tabControl.MouseDown += Control_Click;

			editor = new Editor();
			editor.Show(tabControl);
		}

		public void tabPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabPage tabPage = ControlFactory.BuildCTabPage(model.CurrentClickedControl);
			tabPage.MouseDown += Control_Click;

			editor = new Editor();
			editor.Show(tabPage);
		}

		public void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			editor = new Editor();

			model.LastClickedX = model.CurrentClickedControl.Location.X;
			model.LastClickedY = model.CurrentClickedControl.Location.Y;

			model.logCreator.Append("! Editing: " + model.CurrentClickedControl.Name);

			editor.Show(model.CurrentClickedControl);
		}

		public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
            // Close editor if oppened
            Editor closing = null;
            foreach (Editor ed in Application.OpenForms.OfType<Editor>())
            {
                if (ed.control == model.CurrentClickedControl) closing = ed;
            }
            if (closing != null) closing.Close();

            // Delete control
			model.DeleteControl(model.CurrentClickedControl, false);
        }

		public void TextChanged(object sender, EventArgs e)
		{
			ICustomControl c = sender as ICustomControl;

			c.cd.Text = (sender as Control).Text;
			c.cd.RealText = (sender as Control).Text;
		}

		public void Control_Click(object sender, EventArgs e)
		{
			MouseEventArgs me = e as MouseEventArgs;
			Control c = sender as Control;
			String type = sender.GetType().Name;

			if (model.progMode)
			{
				Rectangle rect = default(Rectangle);
				Pen p = new Pen(SystemColors.Highlight, 1);
				Graphics g = c.Parent.CreateGraphics();

				model.CurrentSection.Tab.Refresh();

				rect = c.Bounds;
				rect.Inflate(1, 1);
				g.DrawRectangle(p, rect);
			}

			model.CurrentClickedControl = c;
			model.LastClickedX = me.X;
			model.LastClickedY = me.Y;

			if (model.progMode && me.Button == MouseButtons.Right)
			{
				SetContextMenuStrip(type);
				contextMenu.Show(c, me.X, me.Y);
			}
            else if (model.progMode && me.Button == MouseButtons.Left && DRAGDROP_ACTIVE)
			{
				if (type != "TabControl" && type != "TabPage" && type != "CTabControl" && type != "CTabPage")
				{
					t.Start();
					Debug.WriteLine("! Timer Started");
				}
			}

			Debug.WriteLine("! Clicked: " + model.CurrentClickedControl.Name + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
		}

		private void TimerTick(object sender, EventArgs e)
		{
			(sender as Timer).Stop();
			MouseEventArgs me = e as MouseEventArgs;

			if (model.CurrentClickedControl != null)
			{
				String name = (model.CurrentClickedControl as ICustomControl).cd.Name;
				String parent = (model.CurrentClickedControl as ICustomControl).cd.Parent.Name;
				Debug.WriteLine("! Got the control: " + name + " with Parent: " + parent);
				model.CurrentClickedControl.DoDragDrop(name, DragDropEffects.Move);
			}
		}

		public void OnDragDrop(object sender, DragEventArgs dea)
		{
			String name = "";
			ICustomControl c = null;
			Control parent = sender as Control;
			dea.Effect = DragDropEffects.Move;

			if (parent != model.CurrentClickedControl)
			{
				name = (string)dea.Data.GetData(typeof(System.String));
				c = model.AllControls.Find(control => control.cd.Name == name);

				if (c != null)
				{
					c.cd.Parent = parent;
					Point cord = new Point(dea.X, dea.Y);
					cord = parent.PointToClient(cord);
					c.cd.Top = cord.Y - model.LastClickedY;
					c.cd.Left = cord.X - model.LastClickedX;

					Debug.WriteLine("! Dropped the control: " + c.cd.Name + " Parent: " + c.cd.Parent.Name + " X: " + cord.X + " Y: " + cord.Y);
					model.CurrentSection.Tab.Refresh();
				}
			}
			else
			{
				Debug.WriteLine("! But you tried to drop it into itself...");
			}
		}

		public void OnDragEnter(object sender, DragEventArgs dea)
		{
			dea.Effect = DragDropEffects.Move;
		}

		public void CancelDragDropTimer(object sender, EventArgs e)
		{
			if ((e as MouseEventArgs).Button == MouseButtons.Left)
			{
				t.Stop();
				Debug.WriteLine("! Timer Stopped");
			}
		}

        public void Changed(object sender, EventArgs e)
        {
            (sender as ICustomControl).cd.Changed = true;
            if((sender as ICustomControl).cd.MainDestination != "")
                System.Diagnostics.Debug.WriteLine("! " +(sender as ICustomControl).cd.Name + " content has changed its value.");
        }
	}
}
