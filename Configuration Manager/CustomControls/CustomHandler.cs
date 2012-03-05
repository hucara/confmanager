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
        public ContextMenuStrip contextMenu;
        Model model;
        Editor editor;
        ControlFactory cf;

        public CustomHandler(ContextMenuStrip cms)
        {
            this.contextMenu = cms;
            this.model = Model.getInstance();
            this.cf = ControlFactory.getInstance();
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
				//c.Parent.Refresh();

				//model.CurrentClickedControl = c;
				//model.LastClickedX = me.X;
				//model.LastClickedY = me.Y;

				//Rectangle rect = default(Rectangle);

				//Pen p = new Pen(Color.Red, 1);
				//Graphics g = c.Parent.CreateGraphics();

				//rect = c.Bounds;
				//rect.Inflate(1, 1);
				//g.DrawRectangle(p, rect);
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
			//bool referencesOthers = ReferencesOthers(model.CurrentClickedControl as ICustomControl);
			//bool isReferenced = IsReferenced(model.CurrentClickedControl as ICustomControl);

			//String msg = "This control is related by other controls.\nAre you sure you want to delete it? ";
			//MessageBox.Show(msg, "Control is referenced", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            Control p = model.CurrentClickedControl.Parent;

			CheckRelations(model.CurrentClickedControl);

            model.AllControls.Remove(model.CurrentClickedControl as ICustomControl);
            model.DeleteControlReferences(model.CurrentClickedControl);
            p.Controls.Remove(model.CurrentClickedControl);

            p.Refresh();
        }

		private void CheckRelations(Control control)
		{
			String msg;
			bool references = false;
			ICustomControl c = control as ICustomControl;
			// Check if this control has anything else inside the related list
			msg = IsReferenced(c);

			if (msg != "" || msg != null)
			{

			}

			// Check if this control is inside the related lists of other controls
			references = ReferencesOthers(c);
		}

		private String IsReferenced(ICustomControl c)
		{
			String read = "";
			String write = "";
			String visib = "";
			String coupled = "";

			foreach (ICustomControl co in model.AllControls)
			{
				if (co.cd.RelatedRead.Contains(c)) read += " " + co.cd.Name;
				if (co.cd.RelatedWrite.Contains(c)) write += " " + co.cd.Name;
				if (co.cd.RelatedVisibility.Contains(c)) visib += " " + co.cd.Name;
				if (co.cd.CoupledControls.Contains(c)) coupled += " " + co.cd.Name;
			}
			return "Related read: "+ read + "\nRelated write: " + write + "\nRelated visibility: " + visib + "\nCoupled controls: " + coupled;
		}

		private bool ReferencesOthers(ICustomControl c)
		{
			if(c.cd.RelatedRead.Count > 0 || c.cd.RelatedVisibility.Count > 0 
				|| c.cd.RelatedWrite.Count > 0 || c.cd.CoupledControls.Count > 0)
			{
				return true;
			}
			return false;
		}
    }
}
