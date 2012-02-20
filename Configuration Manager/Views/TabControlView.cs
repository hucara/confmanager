﻿using System;
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
                s.Tab.Click -= TabControl_RightClick;
                s.Tab.Click += TabControl_RightClick;
            }

            tabControl.Refresh();
        }

        private void CleanUpView()
        {
        }

        private void TabControl_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            TabPage tc = sender as TabPage;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                contextMenu.Items[0].Enabled = true;
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = false;

                if (tc is CTabPage) contextMenu.Items[1].Enabled = false;

                contextMenu.Show(tc, me.X, me.Y);

                model.LastClickedControl = tc;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                Debug.WriteLine("! Clicked: " + tc.Name);
                Debug.WriteLine("! Clicked: " + model.LastClickedControl + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
            }
        }

        public void labelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLabel label = cf.BuildCLabel(null);
            model.LastClickedControl.Controls.Add(label);

            label.Click += Control_RightClick;
            model.AllControls.Add(label);
            label.SetControlDescription();

            editor = new Editor();
            editor.Show(label);
        }

        public void textBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CTextBox textBox = cf.BuildCTextBox(null);
            model.LastClickedControl.Controls.Add(textBox);
            //tabControl.SelectedTab.Controls.Add(textBox);

            textBox.Click += Control_RightClick;
            model.AllControls.Add(textBox);
            textBox.SetControlDescription();

            editor = new Editor();
            editor.Show(textBox);
        }

        public void comboBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CComboBox comboBox = cf.BuildCComboBox(null);
            model.LastClickedControl.Controls.Add(comboBox);
            //tabControl.SelectedTab.Controls.Add(comboBox);

            comboBox.Click += Control_RightClick;
            model.AllControls.Add(comboBox);
            comboBox.SetControlDescription();

            editor = new Editor();
            editor.Show(comboBox);
        }

        public void checkBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCheckBox checkBox = cf.BuildCCheckBox(null);
            model.LastClickedControl.Controls.Add(checkBox);
            //tabControl.SelectedTab.Controls.Add(checkBox);

            checkBox.Click += Control_RightClick;
            model.AllControls.Add(checkBox);
            checkBox.SetControlDescription();

            editor = new Editor();
            editor.Show(checkBox);
        }

        public void groupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CGroupBox groupBox = cf.BuildCGroupBox(null);
            model.LastClickedControl.Controls.Add(groupBox);
            //tabControl.SelectedTab.Controls.Add(groupBox);

            groupBox.Click += ContainerControl_RightClick;
            model.AllControls.Add(groupBox);
            groupBox.SetControlDescription();

            editor = new Editor();
            editor.Show(groupBox);
        }

        public void shapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CPanel panel = cf.BuildCPanel(null);
            model.LastClickedControl.Controls.Add(panel);
            //tabControl.SelectedTab.Controls.Add(panel);

            panel.Click += ContainerControl_RightClick;
            model.AllControls.Add(panel);
            panel.SetControlDescription();

            editor = new Editor();
            editor.Show(panel);
        }

        public void ContainerControl_RightClick(object sender, EventArgs e)
        {
            Control c = sender as Control;

            MouseEventArgs me = e as MouseEventArgs;
            if (model.progMode && me.Button == MouseButtons.Right)
            {
                contextMenu.Items[0].Enabled = true;
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;

                contextMenu.Show(c, me.X, me.Y);

                model.LastClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                Debug.WriteLine("! Clicked: " + c.Name);
                Debug.WriteLine("! Clicked: " + model.LastClickedControl + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
            }
        }

        public void Control_RightClick(object sender, EventArgs e)
        {
            Control c = sender as Control;

            MouseEventArgs me = e as MouseEventArgs;
            if (model.progMode && me.Button == MouseButtons.Right)
            {
                contextMenu.Items[0].Enabled = false;  // Disable the New> option
                contextMenu.Items[1].Enabled = true;
                contextMenu.Items[2].Enabled = true;

                contextMenu.Show(c, me.X, me.Y);

                model.LastClickedControl = c;
                model.LastClickedX = me.X;
                model.LastClickedY = me.Y;

                Debug.WriteLine("! Clicked: " + c.Name);
                Debug.WriteLine("! Clicked: " + model.LastClickedControl + " in X: " + model.LastClickedX + " - Y: " + model.LastClickedY);
            }
        }

        public void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor = new Editor();

            model.LastClickedX = model.LastClickedControl.Location.X;
            model.LastClickedY = model.LastClickedControl.Location.Y;

            editor.Show(model.LastClickedControl);
        }
    }
}