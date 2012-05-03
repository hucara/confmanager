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

        Util.TokenTextTranslator ttt = Util.TokenTextTranslator.GetInstance();
        Util.TokenControlTranslator tct = Util.TokenControlTranslator.GetInstance();

        public ComboBoxEditor(Configuration_Manager.CustomControls.ICustomControl cb)
        {
            InitializeComponent();

            this.cb = cb;
            this.shownValues.SelectionMode = SelectionMode.One;
            this.configValues.SelectionMode = SelectionMode.One;

            RefreshItemLists();
            SetAddDeleteButtons();
            SetMoveButtons();
            RefreshComboBox();
        }

        private void configValues_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.shownValues.SelectedIndex = this.configValues.SelectedIndex;
            SetMoveButtons();
        }

        private void shownValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configValues.SelectedIndex = this.shownValues.SelectedIndex;
            SetMoveButtons();
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
                    String translated = ttt.TranslateFromTextFile(shownTextBox.Text);
                    translated = tct.TranslateFromControl(translated);

                    cb.cd.comboBoxItems.Add(translated);
                    cb.cd.comboBoxRealItems.Add(shownTextBox.Text);
                    cb.cd.comboBoxConfigItems.Add(configTextBox.Text);
                }
            }

            if (shownTextBox.Text != "" && shownTextBox.Text != null &&
                configTextBox.Text == "")
            {
                if (!shownValues.Items.Contains(shownTextBox.Text))
                {
                    String translated = ttt.TranslateFromTextFile(shownTextBox.Text);
                    translated = tct.TranslateFromControl(translated);

                    cb.cd.comboBoxItems.Add(translated);
                    cb.cd.comboBoxRealItems.Add(shownTextBox.Text);
                    cb.cd.comboBoxConfigItems.Add(String.Empty);
                }
            }

            RefreshItemLists();
            SetMoveButtons();
            SetAddDeleteButtons();
            RefreshComboBox();

            shownValues.SelectedItem = shownValues.Items[shownValues.Items.Count -1];

            shownTextBox.Text = "";
            configTextBox.Text = "";
        }

        private void RefreshComboBox()
        {
            (cb as ComboBox).Items.Clear();

            foreach (String s in cb.cd.comboBoxItems)
            {
                (cb as ComboBox).Items.Add(s);
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

            cb.cd.comboBoxItems.RemoveAt(index);
            cb.cd.comboBoxRealItems.RemoveAt(index);
            cb.cd.comboBoxConfigItems.RemoveAt(index);

            RefreshItemLists();
            SetMoveButtons();
            SetAddDeleteButtons();
            RefreshComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (shownValues.SelectedItems.Count > 0)
            {
                int count = shownValues.Items.Count;
                int indx = shownValues.SelectedIndex;

                //object shownSel = shownValues.SelectedItem;
                //object configSel = configValues.SelectedItem;

                string shownText = cb.cd.comboBoxItems[indx];
                string configText = cb.cd.comboBoxConfigItems[indx];
                string realText = cb.cd.comboBoxRealItems[indx];

                if (indx == 0)
                {
                    // Relocate the items
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(count - 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(count - 1, configText);
                    cb.cd.comboBoxRealItems.Insert(count - 1, realText);

                    //shownValues.Items.Remove(shownSel);
                    //configValues.Items.Remove(configSel);

                    //shownValues.Items.Insert(count - 1, shownSel);
                    //configValues.Items.Insert(count - 1, configSel);

                    shownValues.SetSelected(count - 1, true);
                    configValues.SetSelected(count - 1, true);
                }
                else
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(indx - 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(indx - 1, configText);
                    cb.cd.comboBoxRealItems.Insert(indx - 1, realText);

                    shownValues.SetSelected(indx - 1, true);
                    configValues.SetSelected(indx - 1, true);
                }
            }

            RefreshItemLists();
            SetMoveButtons();
            SetAddDeleteButtons();
            RefreshComboBox();
        }

        private void SetAddDeleteButtons()
        {
            int n = shownValues.Items.Count;

            if (n > 0) deleteButton.Enabled = true;
            else deleteButton.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (shownValues.SelectedItems.Count > 0)
            {
                int count = shownValues.Items.Count;
                int indx = shownValues.SelectedIndex;

                string shownText = cb.cd.comboBoxItems[indx];
                string configText = cb.cd.comboBoxConfigItems[indx];
                string realText = cb.cd.comboBoxRealItems[indx];

                if (indx == count - 1)
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(0, shownText);
                    cb.cd.comboBoxConfigItems.Insert(0, configText);
                    cb.cd.comboBoxRealItems.Insert(0, realText);

                    shownValues.SetSelected(0, true);
                    configValues.SetSelected(0, true);
                }
                else
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(indx + 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(indx + 1, configText);
                    cb.cd.comboBoxRealItems.Insert(indx + 1, realText);

                    shownValues.SetSelected(indx + 1, true);
                    configValues.SetSelected(indx + 1, true);
                }
            }

            RefreshItemLists();
            SetMoveButtons();
            RefreshComboBox();
        }

        private void SetMoveButtons()
        {
            if (shownValues.SelectedItem != null)
            {
                if (shownValues.SelectedItem == shownValues.Items[0])
                    moveUpButton.Enabled = false;
                else
                    moveUpButton.Enabled = true;

                if (shownValues.SelectedItem == shownValues.Items[shownValues.Items.Count - 1])
                    moveDownButton.Enabled = false;
                else
                    moveDownButton.Enabled = true;
            }
            else
            {
                moveUpButton.Enabled = false;
                moveDownButton.Enabled = false;
            }
        }

        private void RefreshItemLists()
        {
            shownValues.Items.Clear();
            configValues.Items.Clear();

            for (int i = 0; i < cb.cd.comboBoxItems.Count; i++)
            {
                shownValues.Items.Add(cb.cd.comboBoxItems[i]);
                configValues.Items.Add(cb.cd.comboBoxConfigItems[i]);
            }
        }
    }
}
