﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;
using System.Xml.Linq;
using Configuration_Manager.Util;

namespace Configuration_Manager
{
    public partial class ControlEditor : Form
    {
        const String DEFAULT_READ = "Related Read";
        const String DEFAULT_VISIBILITY = "Related Visibility";
        const String DEFAULT_COUPLED = "Coupled Controls";

        const String DEFAULT_LEFT_ORIENTATION = "Left";
        const String DEFAULT_RIGHT_ORIENTATION = "Right";
        const String DEFAULT_CENTER_ORIENTATION = "Center";

        const bool ERROR = false;
        const bool OK = true;

        const int TOO_HIGH = 1;
        const int TOO_LOW = -1;
        const int VALUE_OK = 0;

        const int MR = -1;

        uint topMargin = 0;
        uint leftMargin = 0;

        public ICustomControl control;

        int top, left, height, width;

        String READ = DEFAULT_READ;
        String VISIBILITY = DEFAULT_VISIBILITY;
        String COUPLED = DEFAULT_COUPLED;

        String LEFT_OR = DEFAULT_LEFT_ORIENTATION;
        String RIGHT_OR = DEFAULT_RIGHT_ORIENTATION;
        String CENTER_OR = DEFAULT_CENTER_ORIENTATION;

        String type;
        String ErrorMsg;
        String RootKeyText = "root key";
        String MainDestinationText = "main destination";
        Font controlFont;

        Color fontColor, backColor;
        Control parent;
        List<ICustomControl> currentVisibleList;
        Model model;

        public ControlEditor()
        {
            InitializeComponent();

            this.model = Model.getInstance();
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
                //this.ClientSize = new Size(685, 700);
                this.Left = MainForm.ActiveForm.Location.X + MainForm.ActiveForm.Width;
            }
        }

        // Control.Show() method overload
        public void Show(Control c)
        {
            this.control = (ICustomControl)c;
            if (this.control == null)
            {
                this.Close();
                return;
            }

            this.parent = control.cd.Parent;
            this.type = c.GetType().Name;

            ShowMousePosition();
            ShowHeadLine();
            ShowDefaultSize();
            DisableControls(type);

            if (this.control is CLabel)
                SetOrientationComboBox();

            if (this.control is CComboBox)
                textLabel.Text = "";

            SetRelationComboBox();
            ReadFromControl();
            SetLocation();
            SetCheckBoxValuesOptions();

            base.Show();
            //SaveToControl();
        }

        private void SetOpenFileDialog()
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
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
            controlNameLabel.Text = Model.GetTranslationFromID(2) + ": " + control.cd.Name;
            parentNameLabel.Text = Model.GetTranslationFromID(3) + ": " + control.cd.Parent.Name;
        }

        private void ShowMousePosition()
        {
            this.topTextBox.Text = model.lastClickedY.ToString();
            this.control.cd.Top = model.lastClickedY;

            this.leftTextBox.Text = model.lastClickedX.ToString();
            this.control.cd.Left = model.lastClickedX;

            ((Control)this.control).Refresh();
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            if (fontDialog1.Font.Name == System.Drawing.SystemFonts.DefaultFont.Name &&
                fontDialog1.Font.Size == System.Drawing.SystemFonts.DefaultFont.Size)
                fontDialog1.Font = Model.getInstance().lastSelectedFont;
            else
                fontDialog1.Font = control.cd.CurrentFont;

            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                controlFont = fontDialog1.Font;
                fontColor = fontDialog1.Color;

                Model.getInstance().lastSelectedFont = controlFont;

                SetExampleTextLabel();
            }
        }

        private void SetExampleTextLabel()
        {
            fontLabel.Font = controlFont;
            fontLabel.Font = new Font(controlFont.Name, 12, controlFont.Style);
            fontLabel.ForeColor = fontColor;
            fontLabel.BackColor = backColor;

            fontLabel.Text = controlFont.Name + " " + Math.Round(controlFont.Size);

            if (control is CPanel)
                fontLabel.Text = "";
        }

        private void FillOutCheckedListBox()
        {
            controlListBox.Items.Clear();
            if (relationsComboBox.SelectedIndex > -1)
            {
                if (relationsComboBox.SelectedItem.ToString() == this.COUPLED && control is CComboBox)
                {
                    foreach (ICustomControl c in model.allControls.Where(p => p is CComboBox))
                        controlListBox.Items.Add(c.cd.Name);
                }
                else if (relationsComboBox.SelectedItem.ToString() == this.COUPLED && control is CCheckBox)
                {
                    foreach (ICustomControl c in model.allControls.Where(p => p is CCheckBox))
                        controlListBox.Items.Add(c.cd.Name);
                }
                else
                {
                    foreach (ICustomControl c in model.allControls)
                        controlListBox.Items.Add(c.cd.Name);
                }
                controlListBox.Items.Remove(control.cd.Name);
            }

            controlListBox.Sorted = true;
        }

        private void SetRelationComboBox()
        {
            try
            {
                // Relations
                READ = Model.GetTranslationFromID(22);
                VISIBILITY = Model.GetTranslationFromID(24);
                COUPLED = Model.GetTranslationFromID(25);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading the relation names inside editor.");
            }

            relationsComboBox.Items.Clear();

            if (control is CComboBox || control is CCheckBox)
            {
                relationsComboBox.Items.Add(READ);
                relationsComboBox.Items.Add(VISIBILITY);
                relationsComboBox.Items.Add(COUPLED);

                relationsComboBox.SelectedItem = READ;
            }
            else if (control is CTextBox)
            {
                relationsComboBox.Items.Add(READ);
                relationsComboBox.SelectedItem = READ;
            }
            //else if (type == "CLabel" || type == "CPanel" || type == "CGroupBox" || type == "CTabPage")
            //{
            //    //deactivated or empty
            //}
        }

        private void FillOutFileTypeComboBox()
        {
            foreach (String s in model.destinationFileTypes)
                destinationTypeComboBox.Items.Add(s);
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
            labelOrientationPanel.Visible = false;

            switch (type)
            {
                case "CComboBox":
                    // Everything is enabled
                    CComboBoxEditorSetUp();
                    exePathPanel.Visible = false;
                    break;

                case "CLabel":
                    labelOrientationPanel.Visible = true;
                    controlListBox.Visible = false;
                    relationsComboBox.Visible = false;
                    exePathPanel.Visible = false;
                    break;

                case "CPanel":
                    textLabel.Visible = false;
                    textTextBox.Visible = false;
                    fontButton.Visible = false;
                    fontLabel.Visible = true;

                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;

                    destinationTypeLabel.Visible = false;
                    destinationTypeComboBox.Visible = false;
                    fileDestinationLabel.Visible = false;
                    fileDestinationTextBox.Visible = false;
                    fileDestinationButton.Visible = false;
                    subDestinationLabel.Visible = false;
                    subDestinatonTextBox.Visible = false;
                    exePathPanel.Visible = false;

                    formattingLabel.Visible = false;
                    formattingTextBox.Visible = false;
                    break;

                case "CTextBox":
                    CTextBoxEditorSetup();
                    textTextBox.Enabled = false;
                    exePathPanel.Visible = false;
                    break;

                case "CCheckBox":
                    exePathPanel.Visible = false;
                    break;

                case "CGroupBox":
                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;
                    exePathPanel.Visible = false;
                    break;

                case "CTabPage":
                    relationsComboBox.Visible = false;
                    fontButton.Visible = false;
                    controlListBox.Visible = false;
                    displayRightLabel.Visible = false;
                    displayRightTextBox.Visible = false;
                    topTextBox.Visible = false;
                    leftTextBox.Visible = false;
                    widthTextBox.Visible = false;
                    heightTextBox.Visible = false;
                    topLabel.Visible = false;
                    leftLabel.Visible = false;
                    widthLabel.Visible = false;
                    heightLabel.Visible = false;

                    destinationTypeLabel.Visible = false;
                    destinationTypeComboBox.Visible = false;
                    fileDestinationLabel.Visible = false;
                    fileDestinationTextBox.Visible = false;
                    fileDestinationButton.Visible = false;
                    subDestinationLabel.Visible = false;
                    subDestinatonTextBox.Visible = false;
                    exePathPanel.Visible = false;

                    formattingLabel.Visible = false;
                    formattingTextBox.Visible = false;
                    break;

                case "CTabControl":
                    textLabel.Visible = false;
                    textTextBox.Visible = false;
                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;

                    destinationTypeLabel.Visible = false;
                    destinationTypeComboBox.Visible = false;
                    fileDestinationLabel.Visible = false;
                    fileDestinationTextBox.Visible = false;
                    fileDestinationButton.Visible = false;
                    subDestinationLabel.Visible = false;
                    subDestinatonTextBox.Visible = false;
                    exePathPanel.Visible = false;

                    formattingLabel.Visible = false;
                    formattingTextBox.Visible = false;
                    break;

                case "CButton":
                    destinationTypeLabel.Visible = false;
                    destinationTypeComboBox.Visible = false;
                    fileDestinationLabel.Visible = false;
                    fileDestinationTextBox.Visible = false;
                    fileDestinationButton.Visible = false;
                    subDestinationLabel.Visible = false;
                    subDestinatonTextBox.Visible = false;
                    formattingLabel.Visible = false;
                    formattingTextBox.Visible = false;

                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;

                    labelOrientationPanel.Visible = false;
                    checkBoxValuePanel.Visible = false;
                    break;

                case "CBitmap":
                    // shut. down. EVERYTHING.
                    textTextBox.Enabled = false;
                    textLabel.Visible = false;
                    textTextBox.Visible = false;
                    fontButton.Visible = false;
                    backColorButton.Visible = false;
                    fontLabel.Visible = false;
                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;
                    destinationTypeLabel.Visible = false;
                    destinationTypeComboBox.Visible = false;
                    fileDestinationLabel.Visible = false;
                    fileDestinationTextBox.Visible = false;
                    fileDestinationButton.Visible = false;
                    subDestinationLabel.Visible = false;
                    subDestinatonTextBox.Visible = false;
                    formattingLabel.Visible = false;
                    formattingTextBox.Visible = false;

                    relationsComboBox.Visible = false;
                    controlListBox.Visible = false;

                    labelOrientationPanel.Visible = false;
                    checkBoxValuePanel.Visible = false;
                    exeArgumentsLabel.Visible = false;
                    exeArgumentsTextBox.Visible = false;
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
            int c = ColorTranslator.ToWin32(System.Drawing.SystemColors.Control);
            //int c = ColorTranslator.ToWin32(Color.FromKnownColor(KnownColor.Control));
            colorDialog1.CustomColors = new int[] { c };
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                backColor = colorDialog1.Color;
                if (colorDialog1.Color.ToArgb().Equals(System.Drawing.SystemColors.Control.ToArgb()))
                    backColor = System.Drawing.SystemColors.Control;
                SetExampleTextLabel();
            }
        }

        private void SetOrientationComboBox()
        {
            try
            {
                // Orientations
                LEFT_OR = Model.GetTranslationFromID(11);
                RIGHT_OR = Model.GetTranslationFromID(101);
                CENTER_OR = Model.GetTranslationFromID(102);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was a problem reading the relation names inside editor.");
            }

            orientationComboBox.Items.Clear();
            orientationComboBox.Items.Add(LEFT_OR);
            orientationComboBox.Items.Add(RIGHT_OR);
            orientationComboBox.Items.Add(CENTER_OR);
            orientationComboBox.SelectedIndex = 0;
        }

        private void fileDestinationButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "INI files (*.ini)|*.ini|XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 3;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK && openFileDialog1.CheckFileExists)
            {
                fileDestinationTextBox.Text = openFileDialog1.FileName;

                String fileType = openFileDialog1.FileName.Substring(openFileDialog1.FileName.Length - 4, 4);
                switch (fileType)
                {
                    case ".xml":
                        destinationTypeComboBox.Text = ".XML";
                        break;
                    case ".ini":
                        destinationTypeComboBox.Text = ".INI";
                        break;
                    case "conf":
                        destinationTypeComboBox.Text = ".INI";
                        break;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool vmd = ValidMainDestination();
            CheckCommonAttributes();
            if (ErrorMsg != "" && !(control is CTabPage) && vmd)
            {
                String caption = Model.GetTranslationFromID(37);
                ErrorMsg = Model.GetTranslationFromID(57) + ErrorMsg;
                MessageBox.Show(ErrorMsg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ValidMainDestination())
            {
                //this.control.cd.Changed = true;
                Model.getInstance().uiChanged = true;
                SaveToControl();
                this.Close();
            }

            ErrorMsg = "";
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            CheckCommonAttributes();
            if (ErrorMsg != "" && !(control is CTabPage))
            {
                String caption = Model.GetTranslationFromID(37);
                ErrorMsg = Model.GetTranslationFromID(57) + ErrorMsg;
                MessageBox.Show(ErrorMsg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ValidMainDestination())
            {
                SaveToControl();
                ReadRelationManager.ReadConfiguration(control as ICustomControl);
            }
            ErrorMsg = "";
        }

        private void CheckLabelInfo()
        {
            CheckText();
            CheckCommonAttributes();
        }

        private void CheckCommonAttributes()
        {
            if (!(control is CTabPage))
            {
                ParseTop();
                ParseLeft();
                ParseWidth();
                ParseHeight();

                if (!(control is CComboBox) && !(control is CTabControl) && !CheckText()) ErrorMsg += "\n - " + Model.GetTranslationFromID(140);

                if (CheckTop() > 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(147) + " " + Model.GetTranslationFromID(145);
                if (CheckLeft() > 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(148) + " " + Model.GetTranslationFromID(145);
                if (CheckWidth() > 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(150) + " " + Model.GetTranslationFromID(145);
                if (CheckHeight() > 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(151) + " " + Model.GetTranslationFromID(145);

                if (CheckTop() < 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(147) + " " + Model.GetTranslationFromID(146);
                if (CheckLeft() < 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(148) + " " + Model.GetTranslationFromID(146);
                if (CheckWidth() < 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(150) + " " + Model.GetTranslationFromID(146);
                if (CheckHeight() < 0) ErrorMsg += "\n- " + Model.GetTranslationFromID(151) + " " + Model.GetTranslationFromID(146);

                if (!CheckDisplayRight()) ErrorMsg += "\n - " + Model.GetTranslationFromID(142);
                if (!CheckModificationRight()) ErrorMsg += "\n - " + Model.GetTranslationFromID(143);

                if (control is CCheckBox && !CheckCheckBoxAttributes()) ErrorMsg += "\n - " + Model.GetTranslationFromID(153);
                if (control is CButton && !CheckButtonExePath()) ErrorMsg += "\n - " + Model.GetTranslationFromID(67);
            }
        }

        private bool CheckButtonExePath()
        {
            if (String.IsNullOrEmpty(exePathTextBox.Text) || !System.IO.File.Exists(exePathTextBox.Text)) return false;
            return true;
        }

        private bool CheckCheckBoxAttributes()
        {
            if (checkedTextBox.Text == uncheckedTextBox.Text) return false;
            if (String.IsNullOrEmpty(checkedTextBox.Text)) return false;
            if (String.IsNullOrEmpty(uncheckedTextBox.Text)) return false;
            return true;
        }

        private bool CheckDisplayRight()
        {
            char[] text = displayRightTextBox.Text.ToArray();
            for (int i = 2; i < text.Length; i++)
                if (text[i] > 'F') return false;
            return true;
        }

        private bool CheckModificationRight()
        {
            char[] text = modificationRightTextBox.Text.ToArray();
            for (int i = 2; i < text.Length; i++)
                if (text[i] > 'F') return false;
            return true;
        }

        private void ParseTop()
        {
            if (topTextBox.Enabled && !Int32.TryParse(topTextBox.Text, out top))
                ErrorMsg += "\n- " + Model.GetTranslationFromID(10) + " " + Model.GetTranslationFromID(152);
        }

        private void ParseLeft()
        {
            if (leftTextBox.Enabled && !Int32.TryParse(leftTextBox.Text, out left))
                ErrorMsg += "\n- " + Model.GetTranslationFromID(11) + " " + Model.GetTranslationFromID(152);
        }

        private void ParseWidth()
        {
            if (widthTextBox.Enabled && !Int32.TryParse(widthTextBox.Text, out width))
                ErrorMsg += "\n- " + Model.GetTranslationFromID(8) + " " + Model.GetTranslationFromID(152);
        }

        private void ParseHeight()
        {
            if (heightTextBox.Enabled && !Int32.TryParse(heightTextBox.Text, out height))
                ErrorMsg += "\n- " + Model.GetTranslationFromID(9) + " " + Model.GetTranslationFromID(152);
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

        private int CheckTop()
        {
            if (top < topMargin) return TOO_LOW;
            if (top + height > parent.Height - leftMargin) return TOO_HIGH;
            return VALUE_OK;
        }

        private int CheckLeft()
        {
            if (left < leftMargin) return TOO_LOW;
            if (left + width > parent.Width - leftMargin) return TOO_HIGH;
            return VALUE_OK;
        }

        private int CheckHeight()
        {
            if (height < MR) return TOO_LOW;
            if (height + top > parent.Height) return TOO_HIGH;
            return VALUE_OK;
        }

        private int CheckWidth()
        {
            if (width < 0) return TOO_LOW;
            if (width + left > parent.Width) return TOO_HIGH;
            return VALUE_OK;
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
            String text = TokenTextTranslator.TranslateFromTextFile(this.textTextBox.Text);
            control.cd.Text = TokenControlTranslator.TranslateFromControl(text);
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
            control.cd.RealMainDestination = this.fileDestinationTextBox.Text;
            control.cd.RealSubDestination = this.subDestinatonTextBox.Text;
            control.cd.Format = this.formattingTextBox.Text;

            control.cd.ModificationRight = this.modificationRightTextBox.Text.Substring(2);
            control.cd.DisplayRight = this.displayRightTextBox.Text.Substring(2);

            if (control is CCheckBox)
            {
                control.cd.checkBoxCheckedValue = this.checkedTextBox.Text;
                control.cd.checkBoxUncheckedValue = this.uncheckedTextBox.Text;
            }

            if (control is CLabel)
                SetOrientationToLabel();

            if (control is CButton)
            {
                control.cd.RealPath = exePathTextBox.Text;
                control.cd.Parameters = exeArgumentsTextBox.Text;
            }

            if (control is CBitmap)
                control.cd.RealPath = exePathTextBox.Text;

            model.ApplyRelations(control);
            ReadRelationManager.ReadConfiguration(control as ICustomControl);

            if (control.cd.Format != null && control.cd.Format != "")
                FormatValue(control);

            model.uiChanged = true;
        }

        private void FormatValue(ICustomControl control)
        {
            if (control is CComboBox)
            {
                int selectedIndex = (control as CComboBox).SelectedIndex;
                if (selectedIndex != -1)
                    (control as CComboBox).SelectedText = control.cd.comboBoxConfigItems[selectedIndex];
                else
                    (control as CComboBox).SelectedText = "";
            }
            else
                (control as Control).Text = Util.StringFormatter.GetFormattedText((control as Control).Text, control.cd.Format);
        }

        public void ReadFromControl()
        {
            if (control == null)
            {
                this.Close();
                return;
            }

            ShowHeadLine();
            model.ApplyRelations(control);

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

            this.fileDestinationTextBox.Text = control.cd.RealMainDestination;
            this.subDestinatonTextBox.Text = control.cd.RealSubDestination;
            this.formattingTextBox.Text = control.cd.Format;

            if (control.cd.DisplayRight != null) this.displayRightTextBox.Text = control.cd.DisplayRight.ToString();
            if (control.cd.ModificationRight != null) this.modificationRightTextBox.Text = control.cd.ModificationRight.ToString();

            if (control.cd.checkBoxCheckedValue != null && control.cd.checkBoxCheckedValue != "")
                this.checkedTextBox.Text = control.cd.checkBoxCheckedValue;

            if (control.cd.checkBoxUncheckedValue != null && control.cd.checkBoxUncheckedValue != "")
                this.uncheckedTextBox.Text = control.cd.checkBoxUncheckedValue;

            if (control.cd.DestinationType != null && control.cd.DestinationType != "")
                this.destinationTypeComboBox.Text = control.cd.DestinationType;
            else this.destinationTypeComboBox.SelectedIndex = 0;

            if (control is CLabel)
                ReadTextOrientation(control as CLabel);

            if (control is CButton)
            {
                exePathTextBox.Text = control.cd.RealPath;
                exeArgumentsTextBox.Text = control.cd.Parameters;
            }

            if (control is CBitmap)
                exePathTextBox.Text = control.cd.RealPath;

            // Update the example font label
            SetExampleTextLabel();
        }

        private void ReadTextOrientation(CLabel cLabel)
        {
            if (cLabel.TextAlign == ContentAlignment.TopLeft)
                orientationComboBox.SelectedIndex = 0;
            else if (cLabel.TextAlign == ContentAlignment.TopRight)
                orientationComboBox.SelectedIndex = 1;
            else if (cLabel.TextAlign == ContentAlignment.TopCenter)
                orientationComboBox.SelectedIndex = 2;
            else
                orientationComboBox.SelectedIndex = 0;
        }

        private void controlListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Get the control that was checked.
            ICustomControl checkedControl = model.allControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString());
            System.Diagnostics.Debug.WriteLine(this.control.cd.RelatedRead.Count);

            // If item is being activated
            if (!controlListBox.GetItemChecked(e.Index))
            {
                if (relationsComboBox.SelectedItem.ToString() == COUPLED)
                {
                    // Manage the coupled relation
                    if (!currentVisibleList.Contains(checkedControl))
                        CoupledRelationsListChanged(checkedControl, e);
                }
                else if (relationsComboBox.SelectedItem.ToString() == VISIBILITY)
                {
                    // Manage the visibility relation
                    if (!currentVisibleList.Contains(checkedControl))
                        currentVisibleList.Add(checkedControl);
                    checkedControl.cd.inRelatedVisibility = true;
                }
                else if (relationsComboBox.SelectedItem.ToString() == READ)
                {
                    // Manage the read relation
                    bool looping = false;
                    if (control is CComboBox && checkedControl is CComboBox)
                    {
                        if (checkedControl.cd.RelatedRead.Contains(this.control))
                        {
                            looping = true;
                            e.NewValue = CheckState.Unchecked;
                            String caption = Model.GetTranslationFromID(37);
                            String msg = Model.GetTranslationFromID(63);
                            MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    if (!currentVisibleList.Contains(checkedControl) && !looping)
                        currentVisibleList.Add(checkedControl);
                }
            }
            else
            {
                currentVisibleList.Remove(checkedControl);
                if (relationsComboBox.Text == VISIBILITY)
                    checkedControl.cd.inRelatedVisibility = false;
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
            else if (checkedControl is CCheckBox)
            {
                currentVisibleList.Add(checkedControl);
            }
            else if (checkedControl is CComboBox && !validCoupleComboBox(checkedControl))
            {
                //They can't be coupled!
                e.NewValue = CheckState.Unchecked;
                String caption = Model.GetTranslationFromID(37);
                String msg = checkedControl.cd.Name + Model.GetTranslationFromID(64) + control.cd.Name;
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!(checkedControl is CComboBox) && !(checkedControl is CCheckBox))
            {
                e.NewValue = CheckState.Unchecked;
                String caption = Model.GetTranslationFromID(37);
                String msg = Model.GetTranslationFromID(65);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            FillOutCheckedListBox();

            if (relationsComboBox.SelectedItem.ToString() != this.COUPLED)
            {
                for (int i = 0; i < controlListBox.Items.Count; i++)
                {
                    if (currentVisibleList.Exists(c => c.cd.Name == controlListBox.Items[i].ToString()))
                        controlListBox.SetItemChecked(i, true);
                    else
                        controlListBox.SetItemChecked(i, false);
                }
            }
            else
            {
                for (int i = 0; i < controlListBox.Items.Count; i++)
                {
                    if (currentVisibleList.Exists(c => c.cd.Name == controlListBox.Items[i].ToString()))
                        controlListBox.SetItemChecked(i, true);
                    else
                        controlListBox.SetItemChecked(i, false);
                }
            }
        }

        private List<ICustomControl> GetCurrentVisibleList()
        {
            String selectedList = relationsComboBox.SelectedItem.ToString();

            System.Diagnostics.Debug.WriteLine("! Related List: " + selectedList);
            if (selectedList == this.READ)
                return control.cd.RelatedRead;
            else if (selectedList == this.VISIBILITY)
                return control.cd.RelatedVisibility;
            else if (selectedList == this.COUPLED)
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
                if (c.Visible && model.allControls.Contains(c as ICustomControl))
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
                    String caption = Model.GetTranslationFromID(37);
                    ErrorMsg = Model.GetTranslationFromID(57) + ErrorMsg;
                    MessageBox.Show(ErrorMsg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SaveToControl();
                    this.Close();
                }
                ErrorMsg = "";
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                CheckCommonAttributes();
                if (ErrorMsg != "")
                {
                    String caption = Model.GetTranslationFromID(37);
                    ErrorMsg = Model.GetTranslationFromID(57) + ErrorMsg;
                    MessageBox.Show(ErrorMsg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    updateButton.PerformClick();
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
            try
            {
                // Title of the form
                this.Text = Model.GetTranslationFromID(1);

                // Description labels
                this.controlNameLabel.Text = Model.GetTranslationFromID(2);
                this.parentNameLabel.Text = Model.GetTranslationFromID(3);
                this.textLabel.Text = Model.GetTranslationFromID(4);
                this.fontButton.Text = Model.GetTranslationFromID(5);
                this.backColorButton.Text = Model.GetTranslationFromID(6);
                this.HintLabel.Text = Model.GetTranslationFromID(7);
                this.widthLabel.Text = Model.GetTranslationFromID(8);
                this.heightLabel.Text = Model.GetTranslationFromID(9);
                this.topLabel.Text = Model.GetTranslationFromID(10);
                this.leftLabel.Text = Model.GetTranslationFromID(11);
                this.groupBox1.Text = Model.GetTranslationFromID(12);

                // Attributes labels
                this.groupBox2.Text = Model.GetTranslationFromID(13);
                this.destinationTypeLabel.Text = Model.GetTranslationFromID(14);
                this.fileDestinationLabel.Text = Model.GetTranslationFromID(15);
                this.subDestinationLabel.Text = Model.GetTranslationFromID(16);
                this.displayRightLabel.Text = Model.GetTranslationFromID(17);
                this.modificationRightLabel.Text = Model.GetTranslationFromID(18);
                this.editComboBoxButton.Text = Model.GetTranslationFromID(95);
                this.checkedLabel.Text = Model.GetTranslationFromID(97);
                this.uncheckedLabel.Text = Model.GetTranslationFromID(98);
                this.formattingLabel.Text = Model.GetTranslationFromID(99);
                this.orientationLabel.Text = Model.GetTranslationFromID(100);

                this.exePathLabel.Text = Model.GetTranslationFromID(105);
                this.exeArgumentsLabel.Text = Model.GetTranslationFromID(106);

                // Saving labels to avoid reading from file again
                MainDestinationText = this.fileDestinationLabel.Text;
                RootKeyText = Model.GetTranslationFromID(96);
                this.fileDestinationButton.Text = "";

                // Bottom Buttons
                this.updateButton.Text = Model.GetTranslationFromID(19);
                this.cancelButton.Text = Model.GetTranslationFromID(20);
                this.okButton.Text = Model.GetTranslationFromID(21);
            }
            catch (Exception)
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.control.cd.Changed = false;
            this.Close();
        }

        private void destinationTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReplaceRegSelectedLabels();
        }

        private void ReplaceRegSelectedLabels()
        {
            if (destinationTypeComboBox.SelectedItem.ToString() == "REG")
            {
                fileDestinationLabel.Text = RootKeyText;
                fileDestinationButton.Visible = false;
            }
            else
            {
                fileDestinationLabel.Text = MainDestinationText;
                //fileDestinationButton.Visible = true;
            }

        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.control.cd.Changed = false;
        }

        private static bool HasLoopRelation(ICustomControl c)
        {
            List<ICustomControl> rels = new List<ICustomControl>();
            foreach (ICustomControl r in c.cd.RelatedRead)
            {
                rels = r.cd.RelatedRead;
                if (rels.Contains(c)) return true;
            }
            return false;
        }

        private bool ValidMainDestination()
        {
            bool isFile;
            if (fileDestinationTextBox.Text.Contains(@":\"))
                isFile = true;
            else
                isFile = false;

            if (destinationTypeComboBox.SelectedItem.ToString() == "REG" && isFile)
            {
                String caption = Model.GetTranslationFromID(37);
                String msg = Model.GetTranslationFromID(66);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void SetOrientationToLabel()
        {
            int index = orientationComboBox.SelectedIndex;
            String align = orientationComboBox.SelectedItem.ToString();

            if (align == LEFT_OR)
                (control as CLabel).TextAlign = ContentAlignment.TopLeft;
            else if (align == RIGHT_OR)
                (control as CLabel).TextAlign = ContentAlignment.TopRight;
            else if (align == CENTER_OR)
                (control as CLabel).TextAlign = ContentAlignment.TopCenter;
            else
                (control as CLabel).TextAlign = ContentAlignment.TopLeft;
        }

        private void exePathButton_Click(object sender, EventArgs e)
        {
            if (this.control is CButton)
            {
                openFileDialog1.Filter = "Executable files (*.exe)|*.exe";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK && openFileDialog1.CheckFileExists)
                {
                    exePathTextBox.Text = openFileDialog1.FileName;
                }
            }
            else if (this.control is CBitmap)
            {
                openFileDialog1.Filter = "Image files (*.jpg; *.bmp; *.jpeg; *.png; *.gif)|*.jpg; *.bmp; *.jpeg; *.png; *.gif";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK && openFileDialog1.CheckFileExists)
                {
                    exePathTextBox.Text = openFileDialog1.FileName;
                }
            }
        }
    }
}
