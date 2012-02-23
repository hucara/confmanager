using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;

namespace Configuration_Manager.Views
{
    class TabControlView : IView
    {
        TabControl tabControl;
        ContextMenuStrip contextMenu;
        ControlFactory cf;
        Model model;
        Editor editor = new Editor();

        public TabControlView(TabControl tc, ContextMenuStrip cms)
        {
            this.tabControl = tc;
            this.contextMenu = cms;

            this.model = Model.getInstance();
            this.cf = ControlFactory.getInstance();
        }

        // Takes the info from the UI, gets the changes made by the 
        // user and stores them inside the model  / views.
        public void saveToModel()
        {
        }

        // Reads the info from the model / views and fills-out the
        // UI with that info, refreshing the components.
        public void readAndShow()
        {
            foreach (Section s in model.Sections)
            {
                s.Tab.Click -= Control_RightClick;
                s.Tab.Click += Control_RightClick;
            }

            tabControl.Refresh();
        }

        private void CleanUpView()
        {
        }

        private void Control_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;
            String type = sender.GetType().Name;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                SetContextMenuStrip(type);

                model.LastClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                contextMenu.Show(c, me.X, me.Y);

                Debug.WriteLine("! Clicked: " + c.Name);
                Debug.WriteLine("! Clicked: " + model.LastClickedControl + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
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
                contextMenu.Items[1].Enabled = false;
                contextMenu.Items[2].Enabled = false;
            }
            else if (type == "CTabPage")
            {
                // Custom Tabs
                contextMenu.Items[0].Enabled = true;
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;
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
            CLabel label = cf.BuildCLabel(model.LastClickedControl);
            label.Click += Control_RightClick; 

            editor = new Editor();
            editor.Show(label);
        }

        public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTextBox textBox = cf.BuildCTextBox(model.LastClickedControl);
            textBox.ContextMenuStrip = contextMenu;
            textBox.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(textBox);
        }

        public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CComboBox comboBox = cf.BuildCComboBox(model.LastClickedControl);
            comboBox.ContextMenuStrip = contextMenu;
            comboBox.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(comboBox);
        }

        public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCheckBox checkBox = cf.BuildCCheckBox(model.LastClickedControl);
            checkBox.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(checkBox);
        }

        public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CGroupBox groupBox = cf.BuildCGroupBox(model.LastClickedControl);
            groupBox.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(groupBox);
        }

        public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CPanel panel = cf.BuildCPanel(model.LastClickedControl);
            panel.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(panel);
        }

        public void tabControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTabControl tabControl = cf.BuildCTabControl(model.LastClickedControl);

            CTabPage tab = tabControl.TabPages[0] as CTabPage;
            tab.cd.Parent = tabControl;

            tabControl.Click += Control_RightClick;
            tabControl.TabPages[0].Click += Control_RightClick;

            editor = new Editor();
            editor.Show(tabControl);
        }

        public void tabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTabPage tabPage = cf.BuildCTabPage(model.LastClickedControl);
            //tabPage.ContextMenuStrip = contextMenu;
            tabPage.Click += Control_RightClick;

            editor = new Editor();
            editor.Show(tabPage);
        }

        public void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor = new Editor();

            model.LastClickedX = model.LastClickedControl.Location.X;
            model.LastClickedY = model.LastClickedControl.Location.Y;

            editor.Show(model.LastClickedControl);
        }

        public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control p = model.LastClickedControl.Parent;

            p.Controls.Remove(model.LastClickedControl);
            model.AllControls.Remove(model.LastClickedControl as ICustomControl);

            p.Refresh();
        }
    }
}