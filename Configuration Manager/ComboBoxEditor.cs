using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Configuration_Manager.Util;

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

            int index = (cb as ComboBox).SelectedIndex;

            RefreshItemLists();
            SetButtons();
            SetMoveButtons();
            RefreshActualComboBox();
            ReplaceLabels();

            if ((cb as ComboBox).Items.Count > 0)
                (cb as ComboBox).SelectedIndex = index;
        }

        private void ReplaceLabels()
        {
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                // Title of the form
                this.Text = texts.Single(x => (int?)x.Attribute("id") == 59).Value;

                // Labels of the form
                this.valuesLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 54).Value;
                this.configValuesLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 55).Value;
                this.okButton.Text = texts.Single(x => (int?)x.Attribute("id") == 21).Value;

                this.moveUpButton.Text = "";
                this.moveDownButton.Text = "";

                this.addButton.Text = texts.Single(x => (int?)x.Attribute("id") == 58).Value;
                this.deleteButton.Text = texts.Single(x => (int?)x.Attribute("id") == 29).Value;
                this.editButton.Text = texts.Single(x=> (int?)x.Attribute("id") == 28).Value;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading texts for the Editor Form.");
                Model.getInstance().logCreator.Append("[ERROR] There was a problem reading texts for the ComboBox Editor form.");
            }
            
        }

        private void configValues_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.shownValues.SelectedIndex = this.configValues.SelectedIndex;
            SetButtons();
            FillOutTextInputBoxes();
            SetMoveButtons();
        }

        private void shownValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configValues.SelectedIndex = this.shownValues.SelectedIndex;
            SetButtons();
            FillOutTextInputBoxes();
            SetMoveButtons();
        }

        private void FillOutTextInputBoxes()
        {
            if (shownValues.SelectedIndex > -1)
            {
                this.shownTextBox.Text = cb.cd.comboBoxRealItems[shownValues.SelectedIndex];
                this.configTextBox.Text = cb.cd.comboBoxConfigItems[shownValues.SelectedIndex];
            }
        }

        // Adding the new item
        private void AddButton_Click(object sender, EventArgs e)
        {
            int index = 0;
            if((cb as ComboBox).Items.Count > 1) index = (cb as ComboBox).SelectedIndex;

            if (shownTextBox.Text != "" && shownTextBox.Text != null &&
                configTextBox.Text != "" && configTextBox.Text != null)
            {
                if (!ItemAlreadyExists())
                {
                    String translated = TokenTextTranslator.TranslateFromTextFile(shownTextBox.Text);
                    translated = TokenControlTranslator.TranslateFromControl(translated);

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
                    String translated = TokenTextTranslator.TranslateFromTextFile(shownTextBox.Text);
                    translated = TokenControlTranslator.TranslateFromControl(translated);

                    cb.cd.comboBoxItems.Add(translated);
                    cb.cd.comboBoxRealItems.Add(shownTextBox.Text);
                    cb.cd.comboBoxConfigItems.Add(String.Empty);
                }
            }

            RefreshItemLists();
            SetMoveButtons();
            SetButtons();
            RefreshActualComboBox();

            //shownValues.SelectedItem = shownValues.Items[shownValues.Items.Count -1];

            if ((cb as ComboBox).Items.Count == 1) (cb as ComboBox).SelectedIndex = 0;
            else (cb as ComboBox).SelectedIndex = index;

            shownTextBox.Text = "";
            configTextBox.Text = "";
        }

        private void RefreshActualComboBox()
        {
            (cb as ComboBox).Items.Clear();
            foreach (String s in cb.cd.comboBoxItems)
                (cb as ComboBox).Items.Add(s);
        }

        // Deleting the item
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (shownValues.SelectedIndex > -1)
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
                SetButtons();
                RefreshActualComboBox();

                int count = (cb as ComboBox).Items.Count;

                if (index >= count) (cb as ComboBox).SelectedIndex = index - 1;
                else if (index < count) (cb as ComboBox).SelectedIndex = index;
                else if (count == 0) (cb as ComboBox).SelectedIndex = -1;
            }

            shownTextBox.Text = "";
            configTextBox.Text = "";
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveUpItem();
        }

        private void MoveUpItem()
        {
            if (shownValues.SelectedItems.Count > 0)
            {
                int count = shownValues.Items.Count;
                int indx = shownValues.SelectedIndex;
                int aux = indx;

                string shownText = cb.cd.comboBoxItems[indx];
                string configText = cb.cd.comboBoxConfigItems[indx];
                string realText = cb.cd.comboBoxRealItems[indx];

                if (indx == 0)
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(count - 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(count - 1, configText);
                    cb.cd.comboBoxRealItems.Insert(count - 1, realText);

                    aux = count - 1;
                }
                else
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(indx - 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(indx - 1, configText);
                    cb.cd.comboBoxRealItems.Insert(indx - 1, realText);

                    aux = indx - 1;
                }

                RefreshItemLists();
                SetMoveButtons();
                SetButtons();
                RefreshActualComboBox();

                shownValues.SetSelected(aux, true);
                configValues.SetSelected(aux, true);

                if (shownValues.SelectedIndex > -1)
                    (cb as ComboBox).SelectedIndex = shownValues.SelectedIndex;
            }
        }

        private void SetButtons()
        {
            int n = shownValues.Items.Count;

            if (n > 0 && shownValues.SelectedIndex != -1)
            {
                deleteButton.Enabled = true;
                editButton.Enabled = true;
            }
            else
            {
                deleteButton.Enabled = false;
                editButton.Enabled = false;
            }

            if (shownTextBox.Text != "" && shownTextBox != null) addButton.Enabled = true;
            else addButton.Enabled = false;
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveDownItem();
        }

        private void MoveDownItem()
        {
            if (shownValues.SelectedItems.Count > 0)
            {
                int count = shownValues.Items.Count;
                int indx = shownValues.SelectedIndex;
                int aux = indx;

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

                    aux = 0;
                }
                else
                {
                    cb.cd.comboBoxItems.RemoveAt(indx);
                    cb.cd.comboBoxConfigItems.RemoveAt(indx);
                    cb.cd.comboBoxRealItems.RemoveAt(indx);

                    cb.cd.comboBoxItems.Insert(indx + 1, shownText);
                    cb.cd.comboBoxConfigItems.Insert(indx + 1, configText);
                    cb.cd.comboBoxRealItems.Insert(indx + 1, realText);

                    aux = indx + 1;
                }

                RefreshItemLists();
                SetMoveButtons();
                SetButtons();
                RefreshActualComboBox();

                shownValues.SetSelected(aux, true);
                configValues.SetSelected(aux, true);

                if (shownValues.SelectedIndex > -1)
                    (cb as ComboBox).SelectedIndex = shownValues.SelectedIndex;
            }
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

        private void shownTextBox_TextChanged(object sender, EventArgs e)
        {
            SetButtons();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shownTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                this.configTextBox.Focus();
        }

        private void configTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.addButton.PerformClick();
                this.shownTextBox.Focus();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int index = shownValues.SelectedIndex;
            if (index != -1)
            {
                if (!ItemAlreadyExists())
                {
                    String translated = TokenTextTranslator.TranslateFromTextFile(shownTextBox.Text);
                    translated = TokenControlTranslator.TranslateFromControl(translated);

                    cb.cd.comboBoxConfigItems[index] = configTextBox.Text;
                    cb.cd.comboBoxRealItems[index] = shownTextBox.Text;
                    cb.cd.comboBoxItems[index] = translated;
                }
            }

            RefreshItemLists();
            SetMoveButtons();
            SetButtons();
            RefreshActualComboBox();

            if ((cb as ComboBox).Items.Count == 1) (cb as ComboBox).SelectedIndex = 0;
            else (cb as ComboBox).SelectedIndex = index;
        }

        private bool ItemAlreadyExists()
        {
            if (cb.cd.comboBoxRealItems.Contains(shownTextBox.Text) &&
                cb.cd.comboBoxConfigItems.Contains(configTextBox.Text))
                return true;
            else
                return false;
        }
    }
}
