using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    public partial class Editor : Form
    {
        const bool OK = true;
        const bool ERROR = false;
        const int MR = 10;

        int top, left, height, width;

        String type;
        String ErrorMsg;

        Font controlFont;
        Color fontColor, backColor;

        Control parent;
        Control control;
        ControlFactory cf;
        ControlDescription cd;
        Model model;

        // //
        // Constructor
        // //
        public Editor()
        {
            InitializeComponent();

            this.cf = ControlFactory.getInstance();
            this.model = Model.getInstance();
            this.cd = new ControlDescription();
            this.ErrorMsg = "";

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;
            visibleCheckBox.Checked = true;

            SetOpenFileDialog();
            FillOutCheckedListBox();
            FillOutRelationsComboBox();
            FillOutFileTypeComboBox();
        }

        // //
        // Control.Show() method overload
        // //
        public void Show(Control c)
        {
            this.control = c;
            this.parent = control.Parent;
            this.type = ReadControlType(control);

            ShowMousePosition();
            ShowHeadLine();
            ShowDefaultSize();
            DisableControls(type);

            base.Show();
        }

        //public Editor(Control control)
        //{
            //InitializeComponent();

            //this.control = control;
            //this.cf = ControlFactory.getInstance();
            //this.model = Model.getInstance();
            //this.cd = new ControlDescription();

            //ShowMousePosition();

            //this.type = ReadControlType(control);
            //this.ErrorMsg = "";

            //ShowHeadLine();
            //ShowDefaultSize();

            //this.parent = control.Parent;

            //fontDialog1.ShowColor = true;
            //fontDialog1.ShowApply = true;
            //visibleCheckBox.Checked = true;

            //DisableControls(type);
            //SetOpenFileDialog();
            //FillOutCheckedListBox();
            //FillOutRelationsComboBox();
            //FillOutFileTypeComboBox();
        //}

        private void SetOpenFileDialog()
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "INI File | *.ini |XML File | *.xml|Text File|*.txt";
        }

        private void ShowDefaultSize()
        {
            this.widthTextBox.Text = control.Width.ToString();
            this.heightTextBox.Text = control.Height.ToString();
        }

        private String ReadControlType(Control control)
        {
            return control.GetType().Name.ToString();
        }

        private void ShowHeadLine()
        {
            controlNameLabel.Text = "Control: " + type;
            parentNameLabel.Text = "Parent: " + model.CurrentClickParent.Name;

            textTextBox.Text = control.Name;
        }

        private void ShowMousePosition()
        {
            this.topTextBox.Text = model.CurrentY.ToString();
            this.control.Top = model.CurrentY;

            this.leftTextBox.Text = model.CurrentX.ToString();
            this.control.Left = model.CurrentX;

            this.control.Refresh();
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                controlFont = fontDialog1.Font;
                fontColor = fontDialog1.Color;

                fontLabel.Font = controlFont;
                fontLabel.Font = new Font(controlFont.FontFamily, 12, controlFont.Style);
                fontLabel.ForeColor = fontDialog1.Color;

                fontLabel.Text = fontDialog1.Font.Name +" "+ fontDialog1.Font.Size;
            }
        }

        // //
        // Filling out stuff inside the Edit Form
        // //
        private void FillOutCheckedListBox()
        {
            foreach (ICustomControl c in model.AllControls)
            {
                controlListBox.Items.Add((c as Control).Name);
            }
        }

        private void FillOutRelationsComboBox()
        {
            if (type == "CComboBox" || type == "CCheckBox")
            {
                relationsComboBox.Items.Add("Related Write");
                relationsComboBox.Items.Add("Related Read");
                relationsComboBox.Items.Add("Related Visibility");
                relationsComboBox.Items.Add("Coupled controls");

                relationsComboBox.SelectedIndex = 0;
            }
            else if (type == "CTextBox")
            {
                relationsComboBox.Items.Add("Related Write");
                relationsComboBox.Items.Add("Related Read");

                relationsComboBox.SelectedIndex = 0;
            }
            else
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
                    break;

                case "CLabel":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    visibleCheckBox.Enabled = false;
                    break;

                case "CShape":
                    textTextBox.Enabled = false;
                    fontButton.Enabled = false;
                    fontLabel.Enabled = false;
                    
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    visibleCheckBox.Enabled = false;

                    destinationTypeLabel.Enabled = false;
                    destinationTypeComboBox.Enabled = false;
                    fileDestinationLabel.Enabled = false;
                    fileDestinationTextBox.Enabled = false;
                    fileDestinationButton.Enabled = false;
                    subDestinationLabel.Enabled = false;
                    subDestinatonTextBox.Enabled = false;
                    break;
                
                case "CTextBox":
                    textTextBox.Enabled = false;
                    visibleCheckBox.Enabled = false;
                    break;

                case "CCheckBox":
                    //Everything is enabled
                    break;

                case "CGroupBox":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    break;
            }
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
                fontLabel.BackColor = backColor;
            }
        }

        private void fileDestinationButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK && openFileDialog1.CheckFileExists)
            {
                fileDestinationTextBox.Text = openFileDialog1.FileName;
            }
        }

        private void SetLabel()
        {
            CheckLabelInfo();

            cd.Hint = hintTextBox.Text;
            cd.Text = textTextBox.Text;

            cf.BuildCLabel(cd);
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

            if (!CheckHint()) ErrorMsg += "\n- Hint is empty.";
            if (!CheckTop()) ErrorMsg += "\n- Top value is too low or too high.";
            if (!CheckLeft()) ErrorMsg += "\n- Left value is too low or too high.";
            if (!CheckWidth()) ErrorMsg += "\n- Width value is too low or too high.";
            if (!CheckHeight()) ErrorMsg += "\n- Height value is too low or too high.";
            if (!CheckSubDestination()) ErrorMsg += "\n- Sub Destination value is empty.";
        }

        private void ParseTop()
        {
            if (!Int32.TryParse(topTextBox.Text, out top))
            {
                ErrorMsg += "\n- Top is not a valid value.";
            }
        }

        private void ParseLeft()
        {
            if (!Int32.TryParse(leftTextBox.Text, out left))
            {
                ErrorMsg += "\n- Left is not a valid value.";
            }
        }

        private void ParseWidth()
        {
            if (!Int32.TryParse(widthTextBox.Text, out width))
            {
                ErrorMsg += "\n- Width is not a valid value.";
            }
        }

        private void ParseHeight()
        {
            if (!Int32.TryParse(heightTextBox.Text, out height))
            {
                ErrorMsg += "\n- Height is not a valid value.";
            }
        }

        private bool CheckText()
        {
            if (textTextBox.Text != "") return OK;
            return ERROR;
        }

        private bool CheckHint()
        {
            if (hintTextBox.Text != "") return OK;
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

        private bool CheckSubDestination()
        {
            if (subDestinatonTextBox.Text == "") return ERROR;
            return OK;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //SetControlDescription here
            CheckCommonAttributes();
            if (ErrorMsg != "")
            {
                ErrorMsg = "Some problems were found: \n" + ErrorMsg;
                MessageBox.Show(ErrorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                switch (this.type)
                {
                    case "CLabel":
                        CheckLabelInfo();
                        (control as CLabel).SetControlDescription(null);
                        break;
                    case "CComboBox":
                        (control as CComboBox).SetControlDescription(null);
                        break;
                    default:
                        break;
                }
            }

            ErrorMsg = "";
        }

        private void CheckBounds()
        {
            if (parent.Bounds.Contains(control.Bounds))
            {
                System.Diagnostics.Debug.WriteLine("! " + control.Name + " is inside " + parent.Name + ".");
                System.Diagnostics.Debug.WriteLine("\tC: " + control.Bounds.ToString());
                System.Diagnostics.Debug.WriteLine("\tP: " + parent.Bounds.ToString());
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            CheckLabelInfo();
            control.Top = top;
            control.Left = left;

            control.Width = width;
            control.Height = height;

            control.BackColor = backColor;
            control.Font = controlFont;
            control.ForeColor = fontColor;

            control.Text = textTextBox.Text;
            control.Visible = visibleCheckBox.Checked;

            CheckBounds();

            control.Refresh();
            ErrorMsg = "";
        }
    }
}
