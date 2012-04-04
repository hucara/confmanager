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
    public partial class SectionForm : Form
    {
        public String SectionName { get; set; }

        public SectionForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text != "" && NameTextBox.Text != null)
            {
                this.SectionName = NameTextBox.Text;
                this.Close();
            }
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                OkButton.PerformClick();
            }
        }
    }
}
