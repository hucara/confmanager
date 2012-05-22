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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComboBoxEditor));
            this.shownValues = new System.Windows.Forms.ListBox();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.configTextBox = new System.Windows.Forms.TextBox();
            this.shownTextBox = new System.Windows.Forms.TextBox();
            this.valuesLabel = new System.Windows.Forms.Label();
            this.configValuesLabel = new System.Windows.Forms.Label();
            this.configValues = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // shownValues
            // 
            this.shownValues.FormattingEnabled = true;
            this.shownValues.Location = new System.Drawing.Point(12, 38);
            this.shownValues.Name = "shownValues";
            this.shownValues.Size = new System.Drawing.Size(164, 108);
            this.shownValues.TabIndex = 1;
            this.shownValues.SelectedIndexChanged += new System.EventHandler(this.shownValues_SelectedIndexChanged);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("moveUpButton.Image")));
            this.moveUpButton.Location = new System.Drawing.Point(376, 47);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(44, 36);
            this.moveUpButton.TabIndex = 6;
            this.moveUpButton.Text = "move up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("moveDownButton.Image")));
            this.moveDownButton.Location = new System.Drawing.Point(376, 98);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(44, 36);
            this.moveDownButton.TabIndex = 7;
            this.moveDownButton.Text = "move down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(359, 150);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(359, 181);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // configTextBox
            // 
            this.configTextBox.Location = new System.Drawing.Point(182, 152);
            this.configTextBox.Name = "configTextBox";
            this.configTextBox.Size = new System.Drawing.Size(164, 20);
            this.configTextBox.TabIndex = 4;
            // 
            // shownTextBox
            // 
            this.shownTextBox.Location = new System.Drawing.Point(12, 152);
            this.shownTextBox.Name = "shownTextBox";
            this.shownTextBox.Size = new System.Drawing.Size(164, 20);
            this.shownTextBox.TabIndex = 3;
            // 
            // valuesLabel
            // 
            this.valuesLabel.AutoSize = true;
            this.valuesLabel.Location = new System.Drawing.Point(12, 9);
            this.valuesLabel.Name = "valuesLabel";
            this.valuesLabel.Size = new System.Drawing.Size(38, 13);
            this.valuesLabel.TabIndex = 8;
            this.valuesLabel.Text = "values";
            // 
            // configValuesLabel
            // 
            this.configValuesLabel.AutoSize = true;
            this.configValuesLabel.Location = new System.Drawing.Point(179, 9);
            this.configValuesLabel.Name = "configValuesLabel";
            this.configValuesLabel.Size = new System.Drawing.Size(70, 13);
            this.configValuesLabel.TabIndex = 9;
            this.configValuesLabel.Text = "config values";
            // 
            // configValues
            // 
            this.configValues.FormattingEnabled = true;
            this.configValues.Location = new System.Drawing.Point(182, 38);
            this.configValues.Name = "configValues";
            this.configValues.Size = new System.Drawing.Size(164, 108);
            this.configValues.TabIndex = 2;
            this.configValues.SelectedIndexChanged += new System.EventHandler(this.configValues_SelectedIndexChanged_1);
            // 
            // ComboBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 215);
            this.Controls.Add(this.configValues);
            this.Controls.Add(this.configValuesLabel);
            this.Controls.Add(this.valuesLabel);
            this.Controls.Add(this.shownTextBox);
            this.Controls.Add(this.configTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.shownValues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox configTextBox;
        private System.Windows.Forms.TextBox shownTextBox;
        private System.Windows.Forms.Label valuesLabel;
        private System.Windows.Forms.Label configValuesLabel;
        private System.Windows.Forms.ListBox configValues;
    }
}