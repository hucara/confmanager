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
    public partial class ComboBoxEditor : Form
    {
        Configuration_Manager.CustomControls.ICustomControl cb;

        public ComboBoxEditor(Configuration_Manager.CustomControls.ICustomControl cb)
        {
            InitializeComponent();

            this.cb = cb;
            this.shownValues.SelectionMode = SelectionMode.One;
            this.configValues.SelectionMode = SelectionMode.One;
        }

        private void configValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.shownValues.SelectedIndex = this.configValues.SelectedIndex;
            System.Diagnostics.Debug.WriteLine("Pim!");

        }

        private void shownValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configValues.SelectedIndex = this.shownValues.SelectedIndex;
            System.Diagnostics.Debug.WriteLine("Pam!");
        }

        // Adding the new item
        private void button3_Click(object sender, EventArgs e)
        {
            if (shownTextBox.Text != "" && shownTextBox.Text != null &&
                configTextBox.Text != "" && configTextBox.Text != null)
            {
                if(!shownValues.Items.Contains(shownTextBox.Text) &&
                    !configValues.Items.Contains(configTextBox.Text))
                {
                    shownValues.Items.Add(shownTextBox.Text);
                    configValues.Items.Add(configTextBox.Text);

                    cb.cd.ComboBoxRealItems.Add(shownTextBox.Text);
                    cb.cd.ComboBoxConfigItems.Add(configTextBox.Text);
                }
            }
        }

        // Deleting the item
        private void button4_Click(object sender, EventArgs e)
        {
            int index = shownValues.SelectedIndex;

            string shown = shownValues.SelectedItem.ToString();
            string config = configValues.SelectedItem.ToString();

            shownValues.Items.RemoveAt(index);
            configValues.Items.RemoveAt(index);

            cb.cd.ComboBoxRealItems.Remove(shown);
            cb.cd.ComboBoxConfigItems.Remove(config);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (shownValues.SelectedIndex > 0 && configValues.SelectedIndex > 0)
            {
                int oldIndex = shownValues.SelectedIndex;

                shownValues.Items.Insert(oldIndex -1, shownValues.SelectedItem);
                configValues.Items.Insert(oldIndex -1, configValues.SelectedItem);

                shownValues.SelectedIndex = oldIndex - 2;
                configValues.SelectedIndex = oldIndex - 2;

                shownValues.Items.RemoveAt(oldIndex + 2);
                shownValues.Items.RemoveAt(oldIndex + 2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
