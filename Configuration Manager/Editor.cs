using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;
using System.Xml.Linq;

namespace Configuration_Manager
{
    public partial class Editor : Form
    {
        const String DEFAULT_READ = "Related Read";
        const String DEFAULT_VISIBILITY = "Related Visibility";
        const String DEFAULT_COUPLED = "Coupled Controls";

        const bool OK = true;
        const bool ERROR = false;
        const int MR = 12;

        public ICustomControl control;

        int top, left, height, width;

        String read = DEFAULT_READ;
        String visibility = DEFAULT_VISIBILITY;
        String coupled = DEFAULT_COUPLED;

        String type;
        String ErrorMsg;

        Font controlFont;
        Color fontColor, backColor;
		Util.TokenTextTranslator ttt;
        Util.TokenControlTranslator tct;

        Control parent;
        List<ICustomControl> currentVisibleList;
        //ControlFactory cf;
        Model model;


        public Editor()
        {
            InitializeComponent();

#if DEBUG
            updateButton.Visible = true;
#else
            updateButton.Visible = false;
#endif

            //this.cf = ControlFactory.getInstance();
            this.model = Model.getInstance();
			this.ttt = Util.TokenTextTranslator.GetInstance();
            this.tct = Util.TokenControlTranslator.GetInstance();

            this.ErrorMsg = "";

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;

            SetOpenFileDialog();

            FillOutFileTypeComboBox();
            ReplaceLabels();
        }

		private void SetLocation()
		{
            if (MainForm.ActiveForm != null)
            {
                this.Top = MainForm.ActiveForm.Location.Y;
                this.Height = 600;
                this.Left = MainForm.ActiveForm.Location.X + MainForm.ActiveForm.Width;
            }
		}


        // Control.Show() method overload
        public void Show(Control c)
        {
            this.control = (ICustomControl)c;
            this.parent = control.cd.Parent;
            this.type = c.GetType().Name;
            this.control.cd.Changed = true;

            ShowMousePosition();
            ShowHeadLine();
            ShowDefaultSize();
            DisableControls(type);

            FillOutCheckedListBox();
            SetRelationComboBox();
            ReadFromControl();
			SetLocation();
            SetCheckBoxValuesOptions();

            base.Show();
        }

        private void SetOpenFileDialog()
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "INI Files(*.ini)|*.ini|XML Files(*.xml)|*.xml|Text Files(*.txt)|*.txt";
        }

        private void ShowDefaultSize()
        {
            this.widthTextBox.Text = control.cd.Width.ToString();
            this.heightTextBox.Text = control.cd.Height.ToString();
        }

        private String ReadControlType(Control control)
        {
            return control.GetType().Name.ToString();
        }

        private void ShowHeadLine()
        {
            controlNameLabel.Text = "Control: " + control.cd.Name;
			parentNameLabel.Text = "Parent: " + control.cd.Parent.Name;
        }

        private void ShowMousePosition()
        {
            this.topTextBox.Text = model.LastClickedY.ToString();
            this.control.cd.Top = model.LastClickedY;

            this.leftTextBox.Text = model.LastClickedX.ToString();
            this.control.cd.Left = model.LastClickedX;

            ((Control)this.control).Refresh();
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                controlFont = fontDialog1.Font;
                fontColor = fontDialog1.Color;

                SetExampleTextLabel();
            }
        }

        private void SetExampleTextLabel()
        {
            fontLabel.Font = controlFont;
            fontLabel.Font = new Font(controlFont.FontFamily, 12, controlFont.Style);
            fontLabel.ForeColor = fontColor;
            fontLabel.BackColor = backColor;

            fontLabel.Text = controlFont.Name + " " + fontDialog1.Font.Size;
        }

        private void FillOutCheckedListBox()
        {
            foreach (ICustomControl c in model.AllControls)
                controlListBox.Items.Add(c.cd.Name);

            controlListBox.Items.Remove(control.cd.Name);
        }

        private void SetRelationComboBox()
        {
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                // Relations
                read = texts.Single(x => (int?)x.Attribute("id") == 22).Value;
                visibility = texts.Single(x => (int?)x.Attribute("id") == 24).Value;
                coupled = texts.Single(x => (int?)x.Attribute("id") == 25).Value;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading the relation names inside editor.");
            }

            relationsComboBox.Items.Clear();

            if (type == "CComboBox" || type == "CCheckBox")
            {
                relationsComboBox.Items.Add(read);
                relationsComboBox.Items.Add(visibility);
                relationsComboBox.Items.Add(coupled);

                relationsComboBox.SelectedItem = read;
            }
            else if (type == "CTextBox")
            {
                relationsComboBox.Items.Add(read);
                relationsComboBox.SelectedItem = read;
            }
            else if (type == "CLabel" || type == "CPanel" || type == "CGroupBox" || type == "CTabPage")
            {
                //deactivated or empty
            }
        }

        private void FillOutFileTypeComboBox()
        {
            foreach (String s in model.DestinationFileTypes)
            {
                destinationTypeComboBox.Items.Add(s);
            }
        }

        private void EnableControls()
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
                c.Visible = true;
            }
        }

        public void DisableControls(String type)
        {
            // Remember the fileDialogbutton
            EnableControls();

            switch (type)
            {
                case "CComboBox":
                    // Everything is enabled
					CComboBoxEditorSetUp();
                    break;

                case "CLabel":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    //visibleCheckBox.Enabled = false;
                    break;

                case "CPanel":
                    textTextBox.Enabled = false;
                    fontButton.Enabled = false;
                    fontLabel.Enabled = false;

                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    //visibleCheckBox.Enabled = false;

                    destinationTypeLabel.Enabled = false;
                    destinationTypeComboBox.Enabled = false;
                    fileDestinationLabel.Enabled = false;
                    fileDestinationTextBox.Enabled = false;
                    fileDestinationButton.Enabled = false;
                    subDestinationLabel.Enabled = false;
                    subDestinatonTextBox.Enabled = false;
                    break;

                case "CTextBox":
					CTextBoxEditorSetup();
                    textTextBox.Enabled = false;
                    //visibleCheckBox.Enabled = false;
                    break;

                case "CCheckBox":
                    //Everything is enabled
                    break;

                case "CGroupBox":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    break;

                case "CTabPage":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    displayRightLabel.Enabled = false;
                    displayRightTextBox.Enabled = false;
                    //visibleCheckBox.Enabled = false;
                    break;

                case "CTabControl":
                    textTextBox.Enabled = false;
                    break;
            }
        }

		private void CTextBoxEditorSetup()
		{
			textTextBox.Text = (control as TextBox).Text;
		}

        // //
        // Handlers
        // //
        private void backColorButton_Click(object sender, EventArgs e)
        {
            int c = ColorTranslator.ToWin32(Color.FromKnownColor(KnownColor.Control));
            colorDialog1.CustomColors = new int[] { c };
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                backColor = colorDialog1.Color;
                SetExampleTextLabel();
            }
        }

        private void fileDestinationButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK && openFileDialog1.CheckFileExists)
            {
                fileDestinationTextBox.Text = openFileDialog1.FileName;
            }

            String fileType = openFileDialog1.FileName.Substring(openFileDialog1.FileName.Length - 4, 4);
            switch (fileType)
            {
                case ".xml":
                    destinationTypeComboBox.Text = ".XML";
                    break;
                case ".ini":
                    destinationTypeComboBox.Text = ".INI";
                    break;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CheckCommonAttributes();
            if (ErrorMsg != "")
            {
                ErrorMsg = "Some problems were found: \n" + ErrorMsg;
                MessageBox.Show(ErrorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveToControl();
            }

            ErrorMsg = "";
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            CheckLabelInfo();
            SaveToControl();
            parent.Refresh();
            ErrorMsg = "";
        }

        private void CheckLabelInfo()
        {
            CheckText();
            CheckCommonAttributes();
        }

        private void CheckCommonAttributes()
        {
            ParseTop();
            ParseLeft();
            ParseWidth();
            ParseHeight();

            if (!CheckText() && control.cd.Type != "CComboBox") ErrorMsg += "\n- Text is empty.";
            //if (!CheckHint()) ErrorMsg += "\n- Hint is empty.";
            if (!CheckTop()) ErrorMsg += "\n- Top value is too low or too high.";
            if (!CheckLeft()) ErrorMsg += "\n- Left value is too low or too high.";
            //if (!CheckWidth()) ErrorMsg += "\n- Width value is too low or too high.";
            //if (!CheckHeight()) ErrorMsg += "\n- Height value is too low or too high.";
            //if (!CheckFileDestination()) ErrorMsg += "\n- File Destination value is empty.";
            //if (!CheckSubDestination()) ErrorMsg += "\n- Sub Destination value is empty.";

			if (!CheckDisplayRight()) ErrorMsg += "\n - Display Right value has non valid characters.";
			if (!CheckModificationRight()) ErrorMsg += "\n - Modification Right value has non valid characters.";
		}

		private bool CheckDisplayRight()
		{
			char []text = displayRightTextBox.Text.ToArray();

			for (int i = 2; i < text.Length; i++)
			{
				if (text[i] > 'F') return false;
			}
			return true;
		}

		private bool CheckModificationRight()
		{
			char [] text = modificationRightTextBox.Text.ToArray();

			for (int i = 2; i < text.Length; i++)
			{
				if (text[i] > 'F') return false;
			}
			return true;
		}

        private void ParseTop()
        {
            if (topTextBox.Enabled && !Int32.TryParse(topTextBox.Text, out top))
            {
                ErrorMsg += "\n- Top is not a valid value.";
            }
        }

        private void ParseLeft()
        {
            if (leftTextBox.Enabled && !Int32.TryParse(leftTextBox.Text, out left))
            {
                ErrorMsg += "\n- Left is not a valid value.";
            }
        }

        private void ParseWidth()
        {
            if (widthTextBox.Enabled && !Int32.TryParse(widthTextBox.Text, out width))
            {
                ErrorMsg += "\n- Width is not a valid value.";
            }
        }

        private void ParseHeight()
        {
            if (heightTextBox.Enabled && !Int32.TryParse(heightTextBox.Text, out height))
            {
                ErrorMsg += "\n- Height is not a valid value.";
            }
        }

        private bool CheckText()
        {
            if (textTextBox.Enabled && textTextBox.Text != "") return OK;
            if (!textTextBox.Enabled) return OK;
            return ERROR;
        }

        private bool CheckHint()
        {
            if (hintTextBox.Enabled && hintTextBox.Text != "") return OK;
            return ERROR;
        }

        private bool CheckTop()
        {
            if (top < MR || top + height > parent.Height - MR) return ERROR;
            return OK;
        }

        private bool CheckLeft()
        {
            if (left < MR || left + width > parent.Width - MR) return ERROR;
            return OK;
        }

        private bool CheckHeight()
        {
            if (height < MR || height + top > parent.Height - MR) return ERROR;
            return OK;
        }

        private bool CheckWidth()
        {
            if (width < MR || width + left > parent.Width - MR) return ERROR;
            return OK;
        }

        private bool CheckFileDestination()
        {
            if (fileDestinationTextBox.Enabled &&
                fileDestinationTextBox.Text == "") return ERROR;
            return OK;
        }

        private bool CheckSubDestination()
        {
            if (subDestinatonTextBox.Enabled &&
                subDestinatonTextBox.Text == "") return ERROR;
            return OK;
        }

        private void SaveToControl()
        {
            String text = ttt.TranslateFromTextFile(this.textTextBox.Text);
            control.cd.Text = tct.TranslateFromControl(text);
            control.cd.RealText = this.textTextBox.Text;
            control.cd.Type = this.type;
			control.cd.Hint = this.hintTextBox.Text;
            control.cd.CurrentFont = this.controlFont;
            control.cd.BackColor = this.backColor;
            control.cd.ForeColor = this.fontColor;
            control.cd.Width = this.width;
            control.cd.Height = this.height;
            control.cd.Top = this.top;
            control.cd.Left = this.left;

            control.cd.DestinationType = this.destinationTypeComboBox.Text;
            control.cd.MainDestination = this.fileDestinationTextBox.Text;
            control.cd.RealSubDestination = this.subDestinatonTextBox.Text;

            control.cd.ModificationRight = this.modificationRightTextBox.Text.Substring(2);
            control.cd.DisplayRight = this.displayRightTextBox.Text.Substring(2);

            control.cd.ModificationBytes = Model.HexToData(control.cd.ModificationRight);
            control.cd.DisplayBytes = Model.HexToData(control.cd.DisplayRight);

            control.cd.operatorModification = model.ObtainRights(control.cd.ModificationBytes, model.MainModificationRights);
            control.cd.operatorVisibility = model.ObtainRights(control.cd.DisplayBytes, model.MainDisplayRights);

            if(control is CCheckBox) control.cd.CheckBoxValues = this.checkBoxValueComboBox.Text;
        }

        private void ReadFromControl()
        {
            this.textTextBox.Text = control.cd.RealText;
            this.type = control.cd.Type;
            this.hintTextBox.Text = control.cd.Hint;
            this.controlFont = control.cd.CurrentFont;
            this.backColor = control.cd.BackColor;
            this.fontColor = control.cd.ForeColor;

            this.width = control.cd.Width;
            this.widthTextBox.Text = this.width.ToString();

            this.height = control.cd.Height;
            this.heightTextBox.Text = this.height.ToString();

            this.top = control.cd.Top;
            this.topTextBox.Text = this.top.ToString();

            this.left = control.cd.Left;
            this.leftTextBox.Text = this.left.ToString();

            this.fileDestinationTextBox.Text = control.cd.MainDestination;
            this.subDestinatonTextBox.Text = control.cd.RealSubDestination;

            if (control.cd.DisplayRight != null) this.displayRightTextBox.Text = control.cd.DisplayRight.ToString();
            if (control.cd.ModificationRight != null) this.modificationRightTextBox.Text = control.cd.ModificationRight.ToString();

            if (control.cd.CheckBoxValues != null && control.cd.CheckBoxValues != "") 
                this.checkBoxValueComboBox.Text = control.cd.CheckBoxValues;
            else this.checkBoxValueComboBox.SelectedIndex = 0;

            if (control.cd.DestinationType != null && control.cd.DestinationType != "") 
                this.destinationTypeComboBox.Text = control.cd.DestinationType;
            else this.destinationTypeComboBox.SelectedIndex = 0;

            // Update the example font label
            SetExampleTextLabel();
        }

        private void controlListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Get the control that was checked.
            ICustomControl checkedControl = model.AllControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString());

            System.Diagnostics.Debug.WriteLine(this.control.cd.RelatedRead.Count);   

            // If item is being activated
            if (!controlListBox.GetItemChecked(e.Index))
            {
                if (relationsComboBox.SelectedItem.ToString() == coupled)
                {
                    // Manage the coupled relation
                    CoupledRelationsListChanged(checkedControl, e);
                }
                else if (relationsComboBox.SelectedItem.ToString() == visibility)
                {
                    // Manage the visibility relation
                    if (!currentVisibleList.Contains(checkedControl))
                        currentVisibleList.Add(checkedControl);
                    checkedControl.cd.inRelatedVisibility = true;
                }
                else if (relationsComboBox.SelectedItem.ToString() == read)
                {
                    // Manage the read relation
                    if(!currentVisibleList.Contains(checkedControl))
                        currentVisibleList.Add(checkedControl);
                }
            }
            else
            {
                currentVisibleList.Remove(checkedControl);
                if (relationsComboBox.Text == visibility) checkedControl.cd.inRelatedVisibility = false;

                System.Diagnostics.Debug.WriteLine("- [" + relationsComboBox.SelectedItem + "] Unchecked: " + checkedControl.cd.Name);
            }

            checkedControl.cd.Visible = true;
            System.Diagnostics.Debug.WriteLine("! " + relationsComboBox.SelectedItem + " has: " + currentVisibleList.Count + " items.");
        }

        private void CoupledRelationsListChanged(ICustomControl checkedControl, ItemCheckEventArgs e)
        {
            if (checkedControl is CComboBox && validCoupleComboBox(checkedControl))
            {
                currentVisibleList.Add(checkedControl);
                System.Diagnostics.Debug.WriteLine("+ [" + relationsComboBox.SelectedItem + "] Checked: " + checkedControl.cd.Name);
            }
            else if (checkedControl is CComboBox && !validCoupleComboBox(checkedControl))
            {
                //They can't be coupled!
                e.NewValue = CheckState.Unchecked;
                String msg = checkedControl.cd.Name + " must contain the same number of items than " + control.cd.Name;
                MessageBox.Show(msg, " Error coupling controls.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(checkedControl is CComboBox))
            {
                e.NewValue = CheckState.Unchecked;
                String msg = "Coupled relations are only allowed between ComboBox and ComboBox controls";
                MessageBox.Show(msg, " Error coupling controls.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private Boolean validCoupleComboBox(ICustomControl checkedControl)
        {
            if (checkedControl.cd.comboBoxRealItems.Count != control.cd.comboBoxRealItems.Count) return false;
            else return true;
        }

        private void relationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentVisibleList = GetCurrentVisibleList();

            for (int i = 0; i < controlListBox.Items.Count; i++)
            {
                if (currentVisibleList.Exists(c => c.cd.Name == controlListBox.Items[i].ToString()))
                    controlListBox.SetItemChecked(i, true);
                else
                    controlListBox.SetItemChecked(i, false);
            }
        }

        private List<ICustomControl> GetCurrentVisibleList()
        {
            String selectedList = relationsComboBox.SelectedItem.ToString();

            System.Diagnostics.Debug.WriteLine("! Related List: " + selectedList);
            if (selectedList == this.read)
                return control.cd.RelatedRead;
            else if (selectedList == this.visibility)
                return control.cd.RelatedVisibility;
            else if (selectedList == this.coupled)
                return control.cd.CoupledControls;
            return control.cd.RelatedRead;
        }

		//
		// Function not working or creating weird / nullException stuff
		//
        private void HighLightRequiredControls()
        {
            Rectangle rect = default(Rectangle);
            Pen p = new Pen(Color.Red, 3);
            Graphics g = parent.CreateGraphics();

			foreach (Control c in currentVisibleList)
			{
				if (c.Visible && model.AllControls.Contains(c as ICustomControl))
				{
					rect = c.Bounds;
					rect.Inflate(3, 3);
					g.DrawRectangle(p, rect);
					g.DrawLine(p, (control as Control).Location, c.Location);
				}
			}
        }

		private void CComboBoxEditorSetUp()
		{
			EnableControls();

			textTextBox.Hide();
            textTextBox.Text = control.cd.Name;
			comboBoxEditPanel.Show();
		}

		private void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.S && e.Control)
			{
				// Save and close
				CheckCommonAttributes();
				if (ErrorMsg != "")
				{
					ErrorMsg = "Some problems were found: \n" + ErrorMsg;
					MessageBox.Show(ErrorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					SaveToControl();
					this.Close();
				}

				ErrorMsg = "";
			}
		}

        private void SetCheckBoxValuesOptions()
        {
            if (this.control is CCheckBox)
                checkBoxValuePanel.Visible = true;
            else
                checkBoxValuePanel.Visible = false;
        }

        private void ReplaceLabels()
        {
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load(Model.getInstance().CurrentLangPath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");

                // Title of the form
                this.Text = texts.Single(x => (int?)x.Attribute("id") == 1).Value;

                // Description labels
                this.controlNameLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 2).Value;     
                this.parentNameLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 3).Value;
                this.textLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 4).Value;
                this.fontButton.Text = texts.Single(x => (int?)x.Attribute("id") == 5).Value;
                this.backColorButton.Text = texts.Single(x => (int?)x.Attribute("id") == 6).Value;
                this.HintLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 7).Value;
                this.widthLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 8).Value;
                this.heightLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 9).Value;
                this.topLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 10).Value;
                this.leftLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 11).Value;
                this.groupBox1.Text = texts.Single(x => (int?)x.Attribute("id") == 12).Value;

                // Attributes labels
                this.groupBox2.Text = texts.Single(x => (int?)x.Attribute("id") == 13).Value;
                this.destinationTypeLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 14).Value;
                this.fileDestinationLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 15).Value;
                this.subDestinationLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 16).Value;
                this.displayRightLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 17).Value;
                this.modificationRightLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 18).Value;

                // Bottom Buttons
                this.updateButton.Text = texts.Single(x => (int?)x.Attribute("id") == 19).Value;
                this.cancelButton.Text = texts.Single(x => (int?)x.Attribute("id") == 20).Value;
                this.okButton.Text = texts.Single(x => (int?)x.Attribute("id") == 21).Value;

                this.editComboBoxButton.Text = texts.Single(x => (int?)x.Attribute("id") == 60).Value;
                this.fileDestinationButton.Text = "";

                this.checkBoxValueLabel.Text = texts.Single(x => (int?)x.Attribute("id") == 55).Value;
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading texts for the Editor Form.");
                Model.getInstance().logCreator.Append("[ERROR] There was a problem reading texts for the Editor form.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ComboBoxEditor cbe = new ComboBoxEditor(this.control);
            cbe.ShowDialog();
        }
	}
}
