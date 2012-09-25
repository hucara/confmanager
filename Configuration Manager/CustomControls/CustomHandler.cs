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
        const bool DRAGDROP_ACTIVE = true;

		public ContextMenuStrip contextMenu;
		Model model;
		ControlEditor editor;
		Timer t = new Timer(); // Drag and drop timer.

        Bitmap previewImage = System.Drawing.SystemIcons.Error.ToBitmap();
        Rectangle previewRectangle = Rectangle.Empty;
        Rectangle snapRectangle = Rectangle.Empty;
        Graphics g;

		public CustomHandler(ContextMenuStrip cms)
		{
			this.contextMenu = cms;
			this.model = Model.getInstance();
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
				contextMenu.Show(c, me.X, me.Y);
			}
			else if (!model.progMode && me.Button == MouseButtons.Right)
			{
				c.ContextMenuStrip = null;

				model.CurrentClickedControl = c;
				model.LastClickedX = me.X;
				model.LastClickedY = me.Y;

				SetContextMenuStrip(type);
			}
		}

		private void SetContextMenuStrip(string type)
		{
			enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, -1, true);

			if (type == "CGroupBox" || type == "CPanel")
			{
				// Editable Containers
				contextMenu.Items[0].Enabled = true;

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

				contextMenu.Items[1].Enabled = true;
				contextMenu.Items[2].Enabled = true;
			}
			else if (type == "TabPage")
			{
				// Section Tabs
				contextMenu.Items[0].Enabled = true;

				enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

				contextMenu.Items[1].Enabled = false;
				contextMenu.Items[2].Enabled = false;
			}
			else if (type == "CTabPage")
			{
				// Custom Tabs
				contextMenu.Items[0].Enabled = true;

				enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);
				
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

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, true);

                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;
            }
            else
            {
                contextMenu.Items[0].Enabled = false;

                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

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
					if (i != index)
						it.DropDownItems[i].Enabled = !status;
			}
			else
			{
				for (int i = 0; i < it.DropDownItems.Count; i++)
					it.DropDownItems[i].Enabled = status;
			}
		}

		public void labelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CLabel label = ControlFactory.BuildCLabel(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(label);
		}

		public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTextBox textBox = ControlFactory.BuildCTextBox(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(textBox);
		}

        public void bitmapToolSTripMenuItem_Click(object sender, EventArgs e)
        {
            CBitmap bitmap = ControlFactory.BuildCBitmap(model.CurrentClickedControl);

            editor = new ControlEditor();
            editor.Show(bitmap);
        }

		public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CComboBox comboBox = ControlFactory.BuildCComboBox(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(comboBox);
		}

		public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CCheckBox checkBox = ControlFactory.BuildCCheckBox(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(checkBox);
		}

        public void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CButton button = ControlFactory.BuildCButton(model.CurrentClickedControl);

            editor = new ControlEditor();
            editor.Show(button);
        }

		public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CGroupBox groupBox = ControlFactory.BuildCGroupBox(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(groupBox);
		}

		public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CPanel panel = ControlFactory.BuildCPanel(model.CurrentClickedControl);

			editor = new ControlEditor();
			editor.Show(panel);
		}

		public void tabControlToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabControl tabControl = ControlFactory.BuildCTabControl(model.CurrentClickedControl);
            CTabPage ctab = ControlFactory.BuildCTabPage(tabControl);

			tabControl.MouseDown += Control_Click;

			editor = new ControlEditor();
			editor.Show(tabControl);
		}

		public void tabPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabPage tabPage = ControlFactory.BuildCTabPage(model.CurrentClickedControl);
			tabPage.MouseDown += Control_Click;

			editor = new ControlEditor();
			editor.Show(tabPage);
		}

		public void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			editor = new ControlEditor();
			model.LastClickedX = model.CurrentClickedControl.Location.X;
			model.LastClickedY = model.CurrentClickedControl.Location.Y;
			model.logCreator.Append("! Editing: " + model.CurrentClickedControl.Name);

            foreach (ControlEditor ed in Application.OpenForms.OfType<ControlEditor>())
            {
                if (ed.control == model.CurrentClickedControl)
                {
                    ed.Focus();
                    return;
                }
            }
			editor.Show(model.CurrentClickedControl);
		}

		public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
            // Close editor if oppened
            ControlEditor closing = null;
            foreach (ControlEditor ed in Application.OpenForms.OfType<ControlEditor>())
                if (ed.control == model.CurrentClickedControl) closing = ed;
            if (closing != null) closing.Close();

            // Delete control
			model.DeleteControl(model.CurrentClickedControl, false);
            Model.getInstance().uiChanged = true;
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
                //if (type != "TabControl" && type != "TabPage" && type != "CTabControl" && type != "CTabPage")
                if (type != "TabControl" && type != "TabPage" && type != "CTabPage")
                {
					t.Start();
                    CreatePreviewRectangle(model.CurrentClickedControl as ICustomControl);
					Debug.WriteLine("! Timer Started");
				}
			}

			Debug.WriteLine("! Clicked: " + model.CurrentClickedControl.Name + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
		}

        // This is the exact moment where the control is selected to begin the drag and drop process
		private void TimerTick(object sender, EventArgs e)
		{
			(sender as Timer).Stop();
			MouseEventArgs me = e as MouseEventArgs;

            if (model.CurrentClickedControl != null)
                model.glassScreen.ActivateDragDrop();
		}

        private Rectangle CreateSnapRectangle(Control control)
        {
            Rectangle rect = control.ClientRectangle;
            rect.X -= 10;
            rect.Width += 20;
            rect.Y -= 10;
            rect.Height += 20;
            return rect;
        }

        private void DrawRectangle(Control parent)
        {
            Graphics g = parent.CreateGraphics();
            g.DrawRectangle(System.Drawing.Pens.Aqua, snapRectangle);
            parent.Update();
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

        private void CreatePreviewRectangle(ICustomControl c)
        {
            snapRectangle = new Rectangle(Cursor.Position, (c as Control).Size);
        }

        private void RefreshEditorWindow(ICustomControl c)
        {
            foreach (ControlEditor ed in Application.OpenForms.OfType<ControlEditor>())
            {
                if (ed.control == model.CurrentClickedControl)
                    ed.ReadFromControl();
            }
        }

        //This event happens when the user gets into a dropable area
		public void OnDragEnter(object sender, DragEventArgs dea)
		{
            dea.Effect = DragDropEffects.Move;
            Control c = sender as Control;
            //c.DragOver -= moveControlWhileDragging_DragOver;
            //c.DragOver += moveControlWhileDragging_DragOver;
		}

		public void CancelDragDropTimer(object sender, EventArgs e)
		{
			if ((e as MouseEventArgs).Button == MouseButtons.Left)
			{
				t.Stop();
                snapRectangle = Rectangle.Empty;
				Debug.WriteLine("! Timer Stopped");
			}
		}

        public void ValueChanged(object sender, EventArgs e)
        {
            //(sender as ICustomControl).cd.Changed = true;
            if ((sender as ICustomControl).cd.MainDestination != "")
            {
                (sender as ICustomControl).cd.Changed = true;
                System.Diagnostics.Debug.WriteLine("! " + (sender as ICustomControl).cd.Name + " content has changed its value.");
            }
        }

        public void CButton_Click(object sender, EventArgs e)
        {
            String exe = (sender as CButton).cd.MainPath;
            String arg = (sender as CButton).cd.Parameters;
            if (!String.IsNullOrEmpty(exe) && System.IO.File.Exists(exe))
            {
                if (!String.IsNullOrEmpty(arg))
                    System.Diagnostics.Process.Start(exe, arg);
                else
                    System.Diagnostics.Process.Start(exe);
            }
            else
            {
                String msg = Model.GetTranslationFromID(67) +" "+ Model.GetTranslationFromID(52);
                String caption = Model.GetTranslationFromID(37);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnDragLeave(object sender, EventArgs e)
        {
            //CheckSnapOnDrop(model.CurrentClickedControl, model.CurrentClickedControl.Parent);
            //model.CurrentClickedControl.Enabled = true;
        }
    }
}
