namespace Configuration_Manager
{
    partial class ComboBoxEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shownValues = new System.Windows.Forms.ListBox();
            this.configValues = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.configTextBox = new System.Windows.Forms.TextBox();
            this.shownTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // shownValues
            // 
            this.shownValues.FormattingEnabled = true;
            this.shownValues.Items.AddRange(new object[] {
            "UNO",
            "DOS",
            "TRES"});
            this.shownValues.Location = new System.Drawing.Point(182, 12);
            this.shownValues.Name = "shownValues";
            this.shownValues.Size = new System.Drawing.Size(164, 134);
            this.shownValues.TabIndex = 1;
            this.shownValues.SelectedIndexChanged += new System.EventHandler(this.shownValues_SelectedIndexChanged);
            // 
            // configValues
            // 
            this.configValues.FormattingEnabled = true;
            this.configValues.Items.AddRange(new object[] {
            "un",
            "dos",
            "tres"});
            this.configValues.Location = new System.Drawing.Point(12, 12);
            this.configValues.Name = "configValues";
            this.configValues.Size = new System.Drawing.Size(164, 134);
            this.configValues.TabIndex = 0;
            this.configValues.SelectedIndexChanged += new System.EventHandler(this.configValues_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 36);
            this.button1.TabIndex = 2;
            this.button1.Text = "move up";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(373, 79);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 36);
            this.button2.TabIndex = 3;
            this.button2.Text = "move down";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(359, 152);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "add";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(359, 181);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "delete";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // configTextBox
            // 
            this.configTextBox.Location = new System.Drawing.Point(12, 154);
            this.configTextBox.Name = "configTextBox";
            this.configTextBox.Size = new System.Drawing.Size(164, 20);
            this.configTextBox.TabIndex = 6;
            // 
            // shownTextBox
            // 
            this.shownTextBox.Location = new System.Drawing.Point(182, 154);
            this.shownTextBox.Name = "shownTextBox";
            this.shownTextBox.Size = new System.Drawing.Size(164, 20);
            this.shownTextBox.TabIndex = 7;
            // 
            // ComboBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 215);
            this.Controls.Add(this.shownTextBox);
            this.Controls.Add(this.configTextBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.shownValues);
            this.Controls.Add(this.configValues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComboBoxEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ComboBox Editor";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox shownValues;
        private System.Windows.Forms.ListBox configValues;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox configTextBox;
        private System.Windows.Forms.TextBox shownTextBox;
    }
}