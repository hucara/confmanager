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
        Util.TokenTextTranslator ttt = Util.TokenTextTranslator.GetInstance();


        public SectionForm()
        {
            InitializeComponent();
        }

        public SectionForm(String oldName)
        {
            InitializeComponent();
            this.NameTextBox.Text = oldName;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            String text = ttt.TranslateFromTextFile(NameTextBox.Text);
            text.Trim();

			if (text != "" && text != null)
			{
				this.SectionName = NameTextBox.Text;
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
