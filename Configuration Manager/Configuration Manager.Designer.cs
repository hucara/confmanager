using Configuration_Manager.Views;
namespace Configuration_Manager
{
    partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.contextNavMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.newSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.groupBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tabControlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.contextNavMenu.SuspendLayout();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.AutoSize = false;
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStrip.ShowItemToolTips = false;
			this.toolStrip.Size = new System.Drawing.Size(131, 572);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			this.toolStrip.Click += new System.EventHandler(this.toolStrip_RightClick);
			// 
			// contextNavMenu
			// 
			this.contextNavMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSectionToolStripMenuItem,
            this.deleteSectionToolStripMenuItem});
			this.contextNavMenu.Name = "contextMenuStrip1";
			this.contextNavMenu.Size = new System.Drawing.Size(150, 48);
			// 
			// newSectionToolStripMenuItem
			// 
			this.newSectionToolStripMenuItem.Name = "newSectionToolStripMenuItem";
			this.newSectionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.newSectionToolStripMenuItem.Text = "New Section";
			this.newSectionToolStripMenuItem.Click += new System.EventHandler(this.newSectionToolStripMenuItem_Click);
			// 
			// deleteSectionToolStripMenuItem
			// 
			this.deleteSectionToolStripMenuItem.Name = "deleteSectionToolStripMenuItem";
			this.deleteSectionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.deleteSectionToolStripMenuItem.Text = "Delete Section";
			this.deleteSectionToolStripMenuItem.Click += new System.EventHandler(this.deleteSectionToolStripMenuItem_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.ShowImageMargin = false;
			this.contextMenu.Size = new System.Drawing.Size(128, 92);
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelToolStripMenuItem,
            this.textBoxToolStripMenuItem,
            this.checkBoxToolStripMenuItem,
            this.comboBoxToolStripMenuItem,
            this.toolStripSeparator1,
            this.groupBoxToolStripMenuItem,
            this.shapeToolStripMenuItem,
            this.toolStripSeparator2,
            this.tabControlMenuItem,
            this.tabPageMenuItem});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.newToolStripMenuItem.Text = "New";
			// 
			// labelToolStripMenuItem
			// 
			this.labelToolStripMenuItem.Name = "labelToolStripMenuItem";
			this.labelToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.labelToolStripMenuItem.Text = "Label";
			// 
			// textBoxToolStripMenuItem
			// 
			this.textBoxToolStripMenuItem.Name = "textBoxToolStripMenuItem";
			this.textBoxToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.textBoxToolStripMenuItem.Text = "TextBox";
			// 
			// checkBoxToolStripMenuItem
			// 
			this.checkBoxToolStripMenuItem.Name = "checkBoxToolStripMenuItem";
			this.checkBoxToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.checkBoxToolStripMenuItem.Text = "CheckBox";
			// 
			// comboBoxToolStripMenuItem
			// 
			this.comboBoxToolStripMenuItem.Name = "comboBoxToolStripMenuItem";
			this.comboBoxToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.comboBoxToolStripMenuItem.Text = "ComboBox";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
			// 
			// groupBoxToolStripMenuItem
			// 
			this.groupBoxToolStripMenuItem.Name = "groupBoxToolStripMenuItem";
			this.groupBoxToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.groupBoxToolStripMenuItem.Text = "GroupBox";
			// 
			// shapeToolStripMenuItem
			// 
			this.shapeToolStripMenuItem.Name = "shapeToolStripMenuItem";
			this.shapeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.shapeToolStripMenuItem.Text = "Shape";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
			// 
			// tabControlMenuItem
			// 
			this.tabControlMenuItem.Name = "tabControlMenuItem";
			this.tabControlMenuItem.Size = new System.Drawing.Size(134, 22);
			this.tabControlMenuItem.Text = "TabControl";
			// 
			// tabPageMenuItem
			// 
			this.tabPageMenuItem.Name = "tabPageMenuItem";
			this.tabPageMenuItem.Size = new System.Drawing.Size(134, 22);
			this.tabPageMenuItem.Text = "TabPage";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			// 
			// tabControl
			// 
			this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl.Location = new System.Drawing.Point(134, -24);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(661, 596);
			this.tabControl.TabIndex = 0;
			this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 572);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.toolStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Configuration Manager";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.contextNavMenu.ResumeLayout(false);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comboBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem groupBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shapeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextNavMenu;
        private System.Windows.Forms.ToolStripMenuItem newSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSectionToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tabControlMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tabPageMenuItem;

    }
}

