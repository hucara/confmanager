namespace Configuration_Manager
{
    partial class SectionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SectionEditor));
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sectionLabel = new System.Windows.Forms.Label();
            this.modificationRightLabel = new System.Windows.Forms.Label();
            this.displayRightLabel = new System.Windows.Forms.Label();
            this.displayRightTextBox = new System.Windows.Forms.MaskedTextBox();
            this.modificationRightTextBox = new System.Windows.Forms.MaskedTextBox();
            this.HintLabel = new System.Windows.Forms.Label();
            this.hintTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(19, 26);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(41, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name: ";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(124, 23);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(164, 20);
            this.NameTextBox.TabIndex = 1;
            this.NameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyDown);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(422, 224);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.HintLabel);
            this.groupBox1.Controls.Add(this.hintTextBox);
            this.groupBox1.Controls.Add(this.modificationRightLabel);
            this.groupBox1.Controls.Add(this.displayRightLabel);
            this.groupBox1.Controls.Add(this.displayRightTextBox);
            this.groupBox1.Controls.Add(this.modificationRightTextBox);
            this.groupBox1.Controls.Add(this.NameLabel);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 187);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " description ";
            // 
            // sectionLabel
            // 
            this.sectionLabel.AutoSize = true;
            this.sectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionLabel.Location = new System.Drawing.Point(12, 9);
            this.sectionLabel.Name = "sectionLabel";
            this.sectionLabel.Size = new System.Drawing.Size(51, 16);
            this.sectionLabel.TabIndex = 4;
            this.sectionLabel.Text = "section";
            // 
            // modificationRightLabel
            // 
            this.modificationRightLabel.AutoSize = true;
            this.modificationRightLabel.Location = new System.Drawing.Point(19, 160);
            this.modificationRightLabel.Name = "modificationRightLabel";
            this.modificationRightLabel.Size = new System.Drawing.Size(86, 13);
            this.modificationRightLabel.TabIndex = 23;
            this.modificationRightLabel.Text = "modification right";
            // 
            // displayRightLabel
            // 
            this.displayRightLabel.AutoSize = true;
            this.displayRightLabel.Location = new System.Drawing.Point(19, 134);
            this.displayRightLabel.Name = "displayRightLabel";
            this.displayRightLabel.Size = new System.Drawing.Size(62, 13);
            this.displayRightLabel.TabIndex = 22;
            this.displayRightLabel.Text = "display right";
            // 
            // displayRightTextBox
            // 
            this.displayRightTextBox.AsciiOnly = true;
            this.displayRightTextBox.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.displayRightTextBox.Location = new System.Drawing.Point(188, 131);
            this.displayRightTextBox.Mask = "\\0\\x>AAAAAAAA<";
            this.displayRightTextBox.Name = "displayRightTextBox";
            this.displayRightTextBox.PromptChar = '0';
            this.displayRightTextBox.RejectInputOnFirstFailure = true;
            this.displayRightTextBox.Size = new System.Drawing.Size(100, 20);
            this.displayRightTextBox.SkipLiterals = false;
            this.displayRightTextBox.TabIndex = 21;
            this.displayRightTextBox.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // modificationRightTextBox
            // 
            this.modificationRightTextBox.AsciiOnly = true;
            this.modificationRightTextBox.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.modificationRightTextBox.Location = new System.Drawing.Point(188, 157);
            this.modificationRightTextBox.Mask = "\\0\\x>AAAAAAAA<";
            this.modificationRightTextBox.Name = "modificationRightTextBox";
            this.modificationRightTextBox.PromptChar = '0';
            this.modificationRightTextBox.Size = new System.Drawing.Size(100, 20);
            this.modificationRightTextBox.SkipLiterals = false;
            this.modificationRightTextBox.TabIndex = 20;
            this.modificationRightTextBox.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // HintLabel
            // 
            this.HintLabel.AutoSize = true;
            this.HintLabel.Location = new System.Drawing.Point(357, 26);
            this.HintLabel.Name = "HintLabel";
            this.HintLabel.Size = new System.Drawing.Size(27, 13);
            this.HintLabel.TabIndex = 25;
            this.HintLabel.Text = "hint:";
            // 
            // hintTextBox
            // 
            this.hintTextBox.Location = new System.Drawing.Point(351, 42);
            this.hintTextBox.MaxLength = 320;
            this.hintTextBox.Multiline = true;
            this.hintTextBox.Name = "hintTextBox";
            this.hintTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.hintTextBox.Size = new System.Drawing.Size(131, 135);
            this.hintTextBox.TabIndex = 24;
            // 
            // SectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 259);
            this.Controls.Add(this.sectionLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SectionEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Section Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label sectionLabel;
        private System.Windows.Forms.Label modificationRightLabel;
        private System.Windows.Forms.Label displayRightLabel;
        private System.Windows.Forms.MaskedTextBox displayRightTextBox;
        private System.Windows.Forms.MaskedTextBox modificationRightTextBox;
        private System.Windows.Forms.Label HintLabel;
        private System.Windows.Forms.TextBox hintTextBox;
    }
}