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

        Font controlFont;
        Color fontColor, backColor;

        Control parent;
        Control control;
        ControlFactory cf;
        ControlDescription cd;
        Model model;
        
        public Editor(String type, Control parent)
        {
            InitializeComponent();

            this.cf = ControlFactory.getInstance();
            this.model = Model.getInstance();
            this.cd = new ControlDescription();

            this.type = type;
            this.parent = parent;

            ShowMousePosition();
            ShowHeadLine();

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;

            DisableControls(type);
        }

        public Editor(Control control)
        {
            InitializeComponent();

            this.control = control;
            this.cf = ControlFactory.getInstance();
            this.model = Model.getInstance();
            this.cd = new ControlDescription();

            ShowMousePosition();

            this.type = ReadControlType(control);

            ShowHeadLine();
            ShowDefaultSize();
            //this.type = type;
            this.parent = parent;

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;

            DisableControls(type);
        }

        private void ShowDefaultSize()
        {
            this.widthTextBox.Text = control.Width.ToString();
            this.heightTextBox.Text = control.Height.ToString();
        }

        private String ReadControlType(Control control)
        {
            if (control is CLabel)
            {
                //control.Top = this.Top;
                //this.Left = this.Left + 112;
                return "CLabel";
            }
            else return "empty";
        }

        private void ShowHeadLine()
        {
            controlNameLabel.Text = "Control: " + type;
            parentNameLabel.Text = "Parent: " + model.CurrentClickParent.Name;
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
            }
        }

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
            openFileDialog1.ShowDialog();
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
            CheckHint();
            CheckText();

            ParseTop();
            ParseLeft();
            ParseWidth();
            ParseHeight();

            CheckTop();
            CheckLeft();
            CheckWidth();
            CheckHeight();
        }

        private bool ParseTop()
        {
            return Int32.TryParse(topTextBox.Text, out top);
        }

        private bool ParseLeft()
        {
            return Int32.TryParse(leftTextBox.Text, out left);
        }

        private bool ParseWidth()
        {
            return Int32.TryParse(widthTextBox.Text, out width);
        }

        private bool ParseHeight()
        {
            return Int32.TryParse(heightTextBox.Text, out height);
        }

        private bool CheckText()
        {
            if (hintTextBox.Text != "") return OK;
            return ERROR;
        }

        private bool CheckHint()
        {
            if (hintTextBox.Text != "") return OK;
            return ERROR;
        }

        private bool CheckTop()
        {
            if (top < MR && top + height > parent.Height - MR) return ERROR;
            return OK;
        }

        private bool CheckLeft()
        {
            if (left < MR && left + width > parent.Width - MR) return ERROR;
            return OK;
        }

        private bool CheckHeight()
        {
            if (height < MR && height + top > parent.Height - MR) return ERROR;
            return OK;
        }

        private bool CheckWidth()
        {
            if (width < MR && width + left > parent.Width - MR) return ERROR;
            return OK;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            switch (this.type)
            {
                case "CLabel":
                    cf.BuildCLabel(cd);
                    break;
                case "CComboBox":
                    cf.BuildCComboBox(cd);
                    break;
                default:
                    break;
            }
        }
    }
}
