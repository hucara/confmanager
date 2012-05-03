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
        const bool OK = true;
        const bool ERROR = false;
        const int MR = 12;

        int top, left, height, width;

        String type;
        String ErrorMsg;

        Font controlFont;
        Color fontColor, backColor;
		Util.TokenTextTranslator ttt;
        Util.TokenControlTranslator tct;

        Control parent;
        public ICustomControl control;
        List<ICustomControl> currentVisibleList;
        ControlFactory cf;
        Model model;

		// //////////////////
        // Constructor
		// //////////////////
        public Editor()
        {
            InitializeComponent();

#if DEBUG
            updateButton.Visible = true;
#else
            updateButton.Visible = false;
#endif

            this.cf = ControlFactory.getInstance();
            this.model = Model.getInstance();
			this.ttt = Util.TokenTextTranslator.GetInstance();
            this.tct = Util.TokenControlTranslator.GetInstance();

            this.ErrorMsg = "";

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;

            SetOpenFileDialog();

            FillOutRelationsComboBox();
            FillOutFileTypeComboBox();
            ReplaceLabels();
        }

		private void SetLocation()
		{
            if (MainForm.ActiveForm != null)
            {
                this.Top = MainForm.ActiveForm.Location.Y;
                this.Height = 610;
                this.Left = MainForm.ActiveForm.Location.X + MainForm.ActiveForm.Width;
            }
		}

        // //
        // Control.Show() method overload
        // //
        public void Show(Control c)
        {
            this.control = (ICustomControl)c;
            this.parent = control.cd.Parent;
            this.type = c.GetType().Name;

            ShowMousePosition();
            ShowHeadLine();
            ShowDefaultSize();
            DisableControls(type);

            FillOutCheckedListBox();
            FillOutRelationsComboBox();

            ReadFromControl();
			SetLocation();
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

        // //
        // Filling out stuff inside the Edit Form
        // //
        private void FillOutCheckedListBox()
        {
            foreach (ICustomControl c in model.AllControls)
            {
                if (c.cd.Name != this.control.cd.Name) controlListBox.Items.Add(c.cd.Name);
            }
        }

        private void FillOutRelationsComboBox()
        {
            relationsComboBox.Items.Clear();

            // TODO Get the names of relations from text file
            if (type == "CComboBox" || type == "CCheckBox")
            {
                relationsComboBox.Items.Add("Related Read");
                relationsComboBox.Items.Add("Related Visibility");
                relationsComboBox.Items.Add("Coupled Controls");

                relationsComboBox.SelectedItem = "Related Read";
            }
            else if (type == "CTextBox")
            {
                relationsComboBox.Items.Add("Related Read");

                relationsComboBox.SelectedItem = "Related Read";
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
            destinationTypeComboBox.SelectedIndex = 0;
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
                    //visibleCheckBox.Enabled = false;
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
			//
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

            control.cd.DestinationType = this.destinationTypeComboBox.SelectedItem.ToString();
            control.cd.MainDestination = this.fileDestinationTextBox.Text;
            control.cd.RealSubDestination = this.subDestinatonTextBox.Text;

			control.cd.ModificationRight = this.modificationRightTextBox.Text.Remove(0,2);
            control.cd.DisplayRight = this.displayRightTextBox.Text.Remove(0, 2);

            control.cd.userModification = model.ObtainLogicAnd(control.cd.ModificationRight, model.ModificatioRights);
            control.cd.userVisibility = model.ObtainLogicAnd(control.cd.DisplayRight, model.DisplayRights);
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

            //this.visibleCheckBox.Checked = control.cd.Visible;

            this.destinationTypeComboBox.SelectedText = control.cd.DestinationType;
            this.fileDestinationTextBox.Text = control.cd.MainDestination;
            this.subDestinatonTextBox.Text = control.cd.RealSubDestination;

            if (control.cd.DisplayRight != null) this.displayRightTextBox.Text = control.cd.DisplayRight;
            if (control.cd.ModificationRight != null) this.modificationRightTextBox.Text = control.cd.ModificationRight;

            // Update the example font label
            SetExampleTextLabel();
        }

        private void controlListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Get the control that was checked.
            ICustomControl checkedControl = model.AllControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString());

            if (!controlListBox.GetItemChecked(e.Index))
            {
                if (!currentVisibleList.Contains(checkedControl))
                {
                    if (checkedControl is CComboBox && relationsComboBox.SelectedItem == "Coupled Controls"
                        && !validCoupleComboBox(checkedControl))
                    {
                        // ComboBox have NOT the same number of items
                        e.NewValue = CheckState.Unchecked;
                    }
                    else
                    {
                        // ComboBox have the same number of items
                        currentVisibleList.Add(checkedControl);
                        System.Diagnostics.Debug.WriteLine("+ [" + relationsComboBox.SelectedItem + "] Checked: " + checkedControl.cd.Name);
                    }
                }
            }
            else
            {
                currentVisibleList.Remove(checkedControl);
                System.Diagnostics.Debug.WriteLine("- [" + relationsComboBox.SelectedItem + "] Unchecked: " + checkedControl.cd.Name);
            }

            checkedControl.cd.Visible = true;
            System.Diagnostics.Debug.WriteLine("! " + relationsComboBox.SelectedItem + " has: " + currentVisibleList.Count + " items.");
        }

        private Boolean validCoupleComboBox(ICustomControl checkedControl)
        {
            if (checkedControl.cd.comboBoxRealItems.Count != control.cd.comboBoxRealItems.Count)
            {
                //They can't be coupled!
                String msg = checkedControl.cd.Name + " must contain the same number of items than " + control.cd.Name;
                MessageBox.Show(msg, " Error coupling controls.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void relationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentVisibleList = GetCurrentVisibleList();

            for (int i = 0; i < controlListBox.Items.Count; i++)
            {
                if (currentVisibleList.Exists(c => c.cd.Name == controlListBox.Items[i].ToString()))
                {
                    controlListBox.SetItemChecked(i, true);
                }
                else
                {
                    controlListBox.SetItemChecked(i, false);
                }
            }
        }

        private List<ICustomControl> GetCurrentVisibleList()
        {
            switch (relationsComboBox.SelectedItem.ToString())
            {
                case "Related Read":
                    System.Diagnostics.Debug.WriteLine("! Related List: Related Read");
                    return control.cd.RelatedRead;

                case "Related Write":
                    System.Diagnostics.Debug.WriteLine("! Related List: Related Write");
                    return control.cd.RelatedWrite;

                case "Related Visibility":
                    System.Diagnostics.Debug.WriteLine("! Related List: Related Visibility");
                    return control.cd.RelatedVisibility;

                case "Coupled Controls":
                    System.Diagnostics.Debug.WriteLine("! Related List: Coupled Controls");
                    return control.cd.CoupledControls;

                default:
                    return null;
            }
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
            }
            catch(ArgumentNullException)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading texts for the Editor Form.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ComboBoxEditor cbe = new ComboBoxEditor(this.control);
            cbe.ShowDialog();
        }
	}
}
