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
        const int MR = 5;

        int top, left, height, width;

        String type;
        String ErrorMsg;
        String hint;

        Font controlFont;
        Color fontColor, backColor;

        Control parent;
        ICustomControl control;
        List<ICustomControl> currentVisibleList;
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
            //this.cd = new ControlDescription();
            this.ErrorMsg = "";

            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;
            visibleCheckBox.Checked = true;

            SetOpenFileDialog();

            FillOutRelationsComboBox();
            FillOutFileTypeComboBox();
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
            base.Show();
        }

        private void SetOpenFileDialog()
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "INI File | *.ini |XML File | *.xml|Text File|*.txt";
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
                if (c != this.control) controlListBox.Items.Add((c as Control).Name);
            }
        }

        private void FillOutRelationsComboBox()
        {
            relationsComboBox.Items.Clear();
            if (type == "CComboBox" || type == "CCheckBox")
            {
                relationsComboBox.Items.Add("Related Read");
                relationsComboBox.Items.Add("Related Write");
                relationsComboBox.Items.Add("Related Visibility");
                relationsComboBox.Items.Add("Coupled Controls");

                relationsComboBox.SelectedItem = "Related Read";
            }
            else if (type == "CTextBox")
            {
                relationsComboBox.Items.Add("Related Read");
                relationsComboBox.Items.Add("Related Write");

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
                    break;

                case "CLabel":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    visibleCheckBox.Enabled = false;
                    break;

                case "CPanel":
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

                case "CTabPage":
                    relationsComboBox.Enabled = false;
                    controlListBox.Enabled = false;
                    visibleCheckBox.Enabled = false;
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
                switch (this.type)
                {
                    case "CLabel":
                        CheckLabelInfo();
                        SaveToControl();
                        //(control as CLabel).SetControlDescription(null);
                        break;
                    case "CComboBox":
                        //(control as CComboBox).SetControlDescription(null);
                        break;
                    default:
                        break;
                }
            }

            ErrorMsg = "";
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            CheckLabelInfo();
            SaveToControl();
            //HighLightRequiredControls();
            parent.Refresh();
            ErrorMsg = "";
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
            if (!CheckFileDestination()) ErrorMsg += "\n- File Destination value is empty.";
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

        private bool CheckFileDestination()
        {
            if (fileDestinationTextBox.Text == "") return ERROR;
            return OK;
        }

        private bool CheckSubDestination()
        {
            if (subDestinatonTextBox.Text == "") return ERROR;
            return OK;
        }

        private void SaveToControl()
        {
            control.cd.Text = this.textTextBox.Text;
            control.cd.Type = this.type;
            control.cd.Hint = this.hintTextBox.Text;
            control.cd.CurrentFont = this.controlFont;
            control.cd.BackColor = this.backColor;
            control.cd.ForeColor = this.fontColor;
            control.cd.Width = this.width;
            control.cd.Height = this.height;
            control.cd.Top = this.top;
            control.cd.Left = this.left;

            control.cd.Visible = this.visibleCheckBox.Checked;
            control.cd.DestinationType = this.destinationTypeComboBox.SelectedText;
            control.cd.MainDestination = this.fileDestinationTextBox.Text;
            control.cd.SubDestination = this.subDestinatonTextBox.Text;
        }

        private void ReadFromControl()
        {
            this.textTextBox.Text = control.cd.Text;
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

            this.visibleCheckBox.Checked = control.cd.Visible;

            this.fileDestinationTextBox.Text = control.cd.MainDestination;
            this.subDestinatonTextBox.Text = control.cd.SubDestination;

            // Update the example font label
            SetExampleTextLabel();
        }

        private void controlListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!controlListBox.GetItemChecked(e.Index))
            {
                if (!currentVisibleList.Contains(model.AllControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString())))
                {
                    currentVisibleList.Add(model.AllControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString()));
                    System.Diagnostics.Debug.WriteLine("+ [" + relationsComboBox.SelectedItem + "] Checked: " + controlListBox.Items[e.Index].ToString());
                }
                HighLightRequiredControls();
            }
            else
            {
                currentVisibleList.Remove(model.AllControls.Find(c => c.cd.Name == controlListBox.Items[e.Index].ToString()));
                System.Diagnostics.Debug.WriteLine("- [" + relationsComboBox.SelectedItem + "] Unchecked: " + controlListBox.Items[e.Index].ToString());
            }

            System.Diagnostics.Debug.WriteLine("! "+relationsComboBox.SelectedItem +" has: "+ currentVisibleList.Count+" items.");
            //if (!controlListBox.GetItemChecked(controlListBox.SelectedIndex))
            //{
            //    //if (model.AllControls.Exists(c => c.cd.Name == controlListBox.SelectedItem.ToString()))
            //    if (!currentVisibleList.Contains(model.AllControls.Find(c => c.cd.Name == controlListBox.SelectedItem.ToString())))
            //    {
            //        currentVisibleList.Add(model.AllControls.Find(c => c.cd.Name == controlListBox.SelectedItem.ToString()));
            //        //System.Diagnostics.Debug.WriteLine("! Checked: " + controlListBox.SelectedItem.ToString());
            //        System.Diagnostics.Debug.WriteLine("+ [" + relationsComboBox.SelectedItem + "] Checked: " + controlListBox.Items[e.Index].ToString());
            //    }
            //}
            //else
            //{
            //    //if (model.AllControls.Exists(c => c.cd.Name == controlListBox.SelectedItem.ToString()))
            //    if (currentVisibleList.Contains(model.AllControls.Find(c => c.cd.Name == controlListBox.SelectedItem.ToString())))
            //    {
            //        if (currentVisibleList.Remove(model.AllControls.Find(c => c.cd.Name == controlListBox.SelectedItem.ToString())))                    //System.Diagnostics.Debug.WriteLine("! Unchecked: " + controlListBox.SelectedItem.ToString());
            //            System.Diagnostics.Debug.WriteLine("- [" + relationsComboBox.SelectedItem + "] Unchecked: " + controlListBox.Items[e.Index].ToString());
            //        else
            //            System.Diagnostics.Debug.WriteLine("ERROR :: Tried to delete: " + relationsComboBox.SelectedItem + "--" + controlListBox.Items[e.Index].ToString());
            //    }
            //}
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

        private void HighLightRequiredControls()
        {
            Rectangle rect = default(Rectangle);
            
            Pen p = new Pen(Color.Red, 3);
            Graphics g = parent.CreateGraphics();

            foreach (Control c in currentVisibleList)
            {
                if(c.Visible)
                {
                    //g = c.CreateGraphics();
                    rect = c.Bounds;
                    rect.Inflate(3, 3);
                    g.DrawRectangle(p, rect);
                    g.DrawLine(p, (control as Control).Location, c.Location);
                }
            }
        }
    }
}
