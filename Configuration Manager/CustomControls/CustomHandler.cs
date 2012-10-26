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

        // Tool tip properties.
        ToolTip controlTip = new ToolTip();
        String mainDestination = Model.GetTranslationFromID(15);
        String subDestination = Model.GetTranslationFromID(16);
        String relatedRead = Model.GetTranslationFromID(22);
        String relatedVisibility = Model.GetTranslationFromID(24);

        Bitmap previewImage = System.Drawing.SystemIcons.Error.ToBitmap();
        Rectangle previewRectangle = Rectangle.Empty;
        Rectangle snapRectangle = Rectangle.Empty;

		public CustomHandler(ContextMenuStrip cms)
		{
			this.contextMenu = cms;
			this.model = Model.getInstance();
			this.t.Interval = 100;
			this.t.Tick += TimerTick;

            this.controlTip.Active = true;
            this.controlTip.ShowAlways = true;
            this.controlTip.InitialDelay = 3000;
            this.controlTip.AutoPopDelay = 5000;
            this.controlTip.IsBalloon = true;
            this.controlTip.UseFading = true; 
		}

		public void CTextBox_RightClick(object sender, EventArgs e)
		{
			MouseEventArgs me = e as MouseEventArgs;
			Control c = sender as Control;
			String type = sender.GetType().Name;

			if (model.progMode && me.Button == MouseButtons.Right)
			{
				c.ContextMenuStrip = contextMenu;

				model.currentClickedControl = c;
				model.lastClickedX = me.X;
				model.lastClickedY = me.Y;

				SetContextMenuStrip(type);
				contextMenu.Show(c, me.X, me.Y);
			}
			else if (!model.progMode && me.Button == MouseButtons.Right)
			{
				c.ContextMenuStrip = null;

				model.currentClickedControl = c;
				model.lastClickedX = me.X;
				model.lastClickedY = me.Y;

				SetContextMenuStrip(type);
			}
		}

		private void SetContextMenuStrip(string type)
		{
			enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, -1, true);

			if (type == "CGroupBox" || type == "CPanel")
			{
				// Editable Containers
				contextMenu.Items[0].Enabled = true;    // New
                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

				contextMenu.Items[1].Enabled = true;    // Edit
                contextMenu.Items[3].Enabled = true;    // Copy
                contextMenu.Items[4].Enabled = true;    // Cut
				contextMenu.Items[7].Enabled = true;    // Delete
			}
			else if (type == "TabPage")
			{
				// Section Tabs
				contextMenu.Items[0].Enabled = true;
				enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

                contextMenu.Items[1].Enabled = false;
                contextMenu.Items[3].Enabled = false;
                contextMenu.Items[4].Enabled = false;
				contextMenu.Items[7].Enabled = false;
			}
			else if (type == "CTabPage")
			{
				// Custom Tabs
				contextMenu.Items[0].Enabled = true;
				enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);
				
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[3].Enabled = false;
                contextMenu.Items[4].Enabled = false;
				contextMenu.Items[7].Enabled = true;

				// Check if it is the only and last tab inside the CTabcontrol
				CTabPage p = model.currentClickedControl as CTabPage;
				if ((p.Parent as CTabControl).TabCount <= 1)
					contextMenu.Items[7].Enabled = false;
			}
            else if (type == "CTabControl")
            {
                contextMenu.Items[0].Enabled = true;
                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, true);

                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[3].Enabled = false;
                contextMenu.Items[4].Enabled = true;
                contextMenu.Items[7].Enabled = true;
            }
            else
            {
                // Not a container
                contextMenu.Items[0].Enabled = false;
                enableDropDownItems(contextMenu.Items[0] as ToolStripMenuItem, 11, false);

                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[3].Enabled = true;
                contextMenu.Items[4].Enabled = true;
                contextMenu.Items[7].Enabled = true;
            }

            // Set the PASTE option
            if (model.copiedControl || model.cutControl)
                contextMenu.Items[5].Enabled = true;
            else
                contextMenu.Items[5].Enabled = false;
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
			CLabel label = ControlFactory.BuildCLabel(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(label);
		}

		public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTextBox textBox = ControlFactory.BuildCTextBox(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(textBox);
		}

        public void bitmapToolSTripMenuItem_Click(object sender, EventArgs e)
        {
            CBitmap bitmap = ControlFactory.BuildCBitmap(model.currentClickedControl);

            editor = new ControlEditor();
            editor.Show(bitmap);
        }

		public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CComboBox comboBox = ControlFactory.BuildCComboBox(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(comboBox);
		}

		public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CCheckBox checkBox = ControlFactory.BuildCCheckBox(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(checkBox);
		}

        public void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CButton button = ControlFactory.BuildCButton(model.currentClickedControl);

            editor = new ControlEditor();
            editor.Show(button);
        }

		public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CGroupBox groupBox = ControlFactory.BuildCGroupBox(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(groupBox);
		}

		public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CPanel panel = ControlFactory.BuildCPanel(model.currentClickedControl);

			editor = new ControlEditor();
			editor.Show(panel);
		}

		public void tabControlToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabControl tabControl = ControlFactory.BuildCTabControl(model.currentClickedControl);
            CTabPage ctab = ControlFactory.BuildCTabPage(tabControl);

			tabControl.MouseDown += Control_Click;

			editor = new ControlEditor();
			editor.Show(tabControl);
		}

		public void tabPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CTabPage tabPage = ControlFactory.BuildCTabPage(model.currentClickedControl);
			tabPage.MouseDown += Control_Click;

			editor = new ControlEditor();
			editor.Show(tabPage);
		}

		public void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (model.currentClickedControl == null) return; 
			editor = new ControlEditor();
			model.lastClickedX = model.currentClickedControl.Location.X;
			model.lastClickedY = model.currentClickedControl.Location.Y;
			model.logCreator.Append("! Editing: " + model.currentClickedControl.Name);

            foreach (ControlEditor ed in Application.OpenForms.OfType<ControlEditor>())
            {
                if (ed.control == model.currentClickedControl)
                {
                    ed.Focus();
                    return;
                }
            }
			editor.Show(model.currentClickedControl);
		}

		public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
            // Close editor if oppened
            ControlEditor closing = null;
            foreach (ControlEditor ed in Application.OpenForms.OfType<ControlEditor>())
                if (ed.control == model.currentClickedControl) closing = ed;
            if (closing != null) closing.Close();

            // Delete control
			model.DeleteControl(model.currentClickedControl, false);
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

				model.currentSection.Tab.Refresh();

				rect = c.Bounds;
				rect.Inflate(1, 1);
				g.DrawRectangle(p, rect);
			}

			model.currentClickedControl = c;
			model.lastClickedX = me.X;
			model.lastClickedY = me.Y;

			if (model.progMode && me.Button == MouseButtons.Right)
			{
				SetContextMenuStrip(type);
				contextMenu.Show(c, me.X, me.Y);
			}
            else if (model.progMode && me.Button == MouseButtons.Left && DRAGDROP_ACTIVE)
			{
                if (type != "TabControl" && type != "TabPage" && type != "CTabPage")
                {
					t.Start();
                    CreatePreviewRectangle(model.currentClickedControl as ICustomControl);
					Debug.WriteLine("! Timer Started");
				}
			}

			Debug.WriteLine("! Clicked: " + model.currentClickedControl.Name + " in X: " + model.lastClickedX + " - Y: " + model.lastClickedY);
		}

        // This is the exact moment where the control is selected to begin the drag and drop process
		private void TimerTick(object sender, EventArgs e)
		{
			(sender as Timer).Stop();
			MouseEventArgs me = e as MouseEventArgs;

            if (model.currentClickedControl != null)
                model.glassScreen.ActivateDragDrop(editor);
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
                if (ed.control == model.currentClickedControl)
                    ed.ReadFromControl();
            }
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
                //System.Threading.Thread.Sleep(350); // Fixes the drag and drop bug.
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void copyStripMenuItem_Click(object sender, EventArgs e)
        {
            model.copiedControlData = model.currentClickedControl as ICustomControl;
            model.copiedControl = true;
            model.cutControl = false;
        }

        public void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (model.copiedControl)
            {
                CreateControlCopy(model.copiedControlData as ICustomControl);
            }
            else if (model.cutControl)
                MoveControl();

            Model.getInstance().uiChanged = true;
        }

        public void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(model.currentClickedControl != null)
            {
                model.cutControlData = model.currentClickedControl as ICustomControl;
                model.currentClickedControl.Parent.Controls.Remove(model.currentClickedControl);
                model.copiedControl = false;
                model.cutControl = true;
            }
        }

        private void MoveControl()
        {
            model.cutControlData.cd.Top = model.lastClickedY;
            model.cutControlData.cd.Left = model.lastClickedX;
            model.currentClickedControl.Controls.Add(model.cutControlData as Control);
            model.cutControlData.cd.Parent = model.currentClickedControl;
            model.cutControlData.cd.ParentSection = model.currentSection;

            model.copiedControl = false;
            model.cutControl = false;
        }

        private Control CreateControlCopy(ICustomControl source)
        {
            ICustomControl newControl = null;
            newControl = SetCopyType(source.cd.Type);

            if (newControl is CTabControl) 
                SetCopyTabControl(newControl);

            if (newControl != null)
            {
                CopyControlProperties(newControl, source);
                Debug.WriteLine("Copied control: " +newControl.cd.Name);

                if ((source as Control).HasChildren)
                {
                    Debug.WriteLine("Has children!"); 
                    foreach (Control child in (source as Control).Controls)
                    {
                        Control childCopy = CreateControlCopy(child as ICustomControl);
                        (newControl as Control).Controls.Add(childCopy);
                        (childCopy as ICustomControl).cd.Parent = newControl as Control;
                        //(newControl as Control).Controls.Add(CreateControlCopy(child as ICustomControl));
                        Debug.WriteLine("|_> Copied child: " + child.Name); 
                    }
                }

                if (newControl.cd.RealText == model.copiedControlData.cd.RealText)
                {
                    newControl.cd.ParentSection = model.currentSection;
                    newControl.cd.Parent = model.currentClickedControl;

                    newControl.cd.Top = model.lastClickedY;
                    newControl.cd.Left = model.lastClickedX;
                }

                CheckSnapOnDrop(newControl as Control, newControl.cd.Parent);

                return newControl as Control;
            }
            return null;
        }

        private void SetCopyTabControl(ICustomControl newControl)
        {
            if (newControl is CTabControl)
            {
                TabPage[] tpc = new TabPage[(model.copiedControlData as CTabControl).TabCount];
                int numPages = (model.copiedControlData as CTabControl).TabCount;

                for (int i = 0; i < numPages; i++)
                    tpc[i] = (model.copiedControlData as CTabControl).TabPages[i];

                (newControl as CTabControl).TabPages.AddRange(tpc);
            }
        }

        private ICustomControl SetCopyType(string sourceType)
        {
            ICustomControl newControl = null;
            switch (sourceType)
            {
                case "CLabel":
                    newControl = ControlFactory.BuildCLabel(model.currentClickedControl);
                    break;

                case "CTextBox":
                    newControl = ControlFactory.BuildCTextBox(model.currentClickedControl);
                    break;

                case "CButton":
                    newControl = ControlFactory.BuildCButton(model.currentClickedControl);
                    break;

                case "CBitmap":
                    newControl = ControlFactory.BuildCBitmap(model.currentClickedControl);
                    break;

                case "CCheckBox":
                    newControl = ControlFactory.BuildCCheckBox(model.currentClickedControl);
                    break;

                case "CComboBox":
                    newControl = ControlFactory.BuildCComboBox(model.currentClickedControl);
                    break;

                case "CPanel":
                    newControl = ControlFactory.BuildCPanel(model.currentClickedControl);
                    break;

                case "CTabControl":
                    newControl = ControlFactory.BuildCTabControl(model.currentClickedControl);
                    break;

                case "CGroupBox":
                    newControl = ControlFactory.BuildCGroupBox(model.currentClickedControl);
                    break;
            }

            return newControl;
        }

        private Control CopyControlProperties(ICustomControl dest, ICustomControl source)
        {
            dest.cd.Text = source.cd.Text;
            dest.cd.Changed = source.cd.Changed;
            dest.cd.RealText = source.cd.RealText;
            dest.cd.Type = source.cd.Type;
            dest.cd.Format = source.cd.Format;
            dest.cd.TextAlign = source.cd.TextAlign;

            dest.cd.Width = source.cd.Width;
            dest.cd.Height = source.cd.Height;

            dest.cd.CurrentFont = source.cd.CurrentFont;
            dest.cd.BackColor = source.cd.BackColor;
            dest.cd.ThisControl = dest as Control;

            dest.cd.DisplayRight = source.cd.DisplayRight;
            dest.cd.ModificationRight = source.cd.ModificationRight;

            //c.cd.operatorModification = source.cd.operatorModification;
            //c.cd.operatorVisibility = source.cd.operatorVisibility;

            dest.cd.checkBoxCheckedValue = source.cd.checkBoxCheckedValue;
            dest.cd.checkBoxUncheckedValue = source.cd.checkBoxUncheckedValue;

            dest.cd.DestinationType = source.cd.DestinationType;
            dest.cd.MainDestination = source.cd.MainDestination;
            dest.cd.RealMainDestination = source.cd.RealMainDestination;
            dest.cd.SubDestination = source.cd.SubDestination;
            dest.cd.RealSubDestination = source.cd.RealSubDestination;

            dest.cd.RealPath = source.cd.RealPath;
            dest.cd.Parameters = source.cd.Parameters;

            //dest.cd.RelatedRead.AddRange(source.cd.RelatedRead);
            //dest.cd.RelatedVisibility.AddRange(source.cd.RelatedVisibility);
            //dest.cd.CoupledControls.AddRange(source.cd.CoupledControls);

            dest.cd.ParentSection = model.currentSection;
            //dest.cd.Parent = model.CurrentClickedControl;

            dest.cd.Top = source.cd.Top;
            dest.cd.Left = source.cd.Left;

            if (source.cd.comboBoxItems != null && source.cd.comboBoxItems.Count > 0)
            {
                dest.cd.comboBoxItems.AddRange(source.cd.comboBoxItems);
                dest.cd.comboBoxRealItems.AddRange(source.cd.comboBoxRealItems);
                dest.cd.comboBoxConfigItems.AddRange(source.cd.comboBoxConfigItems);
                (dest as ComboBox).Items.AddRange(dest.cd.comboBoxItems.ToArray());
                (dest as ComboBox).SelectedItem = dest.cd.comboBoxItems.First();
                (dest as ComboBox).Update();
            }

            dest.cd.Visible = source.cd.Visible;
            return dest as Control;
        }

        public void MultiplyControl()
        {
            if (model.currentClickedControl is CLabel)
            {
                CLabel lbl = ControlFactory.BuildCLabel(model.currentClickedControl.Parent);
                lbl.cd.Top = model.currentClickedControl.Top + 25;
                lbl.cd.Left = model.currentClickedControl.Left;
                lbl.cd.Width = model.currentClickedControl.Width;
                lbl.cd.RealSubDestination = (model.currentClickedControl as ICustomControl).cd.RealSubDestination;
                lbl.cd.RealMainDestination = (model.currentClickedControl as ICustomControl).cd.RealMainDestination;
                lbl.cd.DestinationType = (model.currentClickedControl as ICustomControl).cd.DestinationType;

                model.currentClickedControl = lbl;
            }
            else if (model.currentClickedControl is CTextBox)
            {
                CTextBox txtb = ControlFactory.BuildCTextBox(model.currentClickedControl.Parent);
                txtb.cd.Top = model.currentClickedControl.Top + 25;
                txtb.cd.Left = model.currentClickedControl.Left;
                txtb.cd.Width = model.currentClickedControl.Width;
                txtb.cd.RealSubDestination = (model.currentClickedControl as ICustomControl).cd.RealSubDestination;
                txtb.cd.RealMainDestination = (model.currentClickedControl as ICustomControl).cd.RealMainDestination;
                txtb.cd.DestinationType = (model.currentClickedControl as ICustomControl).cd.DestinationType;

                model.currentClickedControl = txtb;
            }

            Model.getInstance().uiChanged = true;
        }

        public void TransportControl(KeyEventArgs e)
        {
            if(model.currentClickedControl is CTextBox || model.currentClickedControl is CLabel
                || model.currentClickedControl is CCheckBox)
            {
                if(e.KeyCode == Keys.Down) (model.currentClickedControl as ICustomControl).cd.Top += 25;
                else if (e.KeyCode == Keys.Up) (model.currentClickedControl as ICustomControl).cd.Top -= 25;
            }

            Model.getInstance().uiChanged = true;
        }

        public void ShowControlInfoHint(object sender, EventArgs e)
        {
            if (model.progMode && sender != null)
            {
                Control s = (sender as Control);
                ICustomControl cc = (sender as ICustomControl);
                string text = " ";

                this.controlTip.ToolTipTitle = cc.cd.Name;
                this.controlTip.ToolTipTitle += " " + cc.cd.RealText;

                if (!String.IsNullOrEmpty(cc.cd.MainDestination)) text += "\n" + this.mainDestination + ":  " + cc.cd.MainDestination;
                if (!String.IsNullOrEmpty(cc.cd.RealSubDestination)) text += "\n" + this.subDestination + ":  " + cc.cd.RealSubDestination;

                //if(cc.cd.RelatedRead.Count > 0) 
                //    text += "\n" +this.relatedRead +":  " + String.Join(", ", cc.cd.RelatedRead.ToArray());

                //if(cc.cd.RelatedVisibility.Count > 0)
                //    text += "\n" +this.relatedVisibility +":  " +cc.cd.RelatedVisibility.ToString();

                this.controlTip.SetToolTip(s, text);
            }
        }
    }
}
