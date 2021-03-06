﻿using Configuration_Manager.Views;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.sectionBar = new System.Windows.Forms.ToolStrip();
            this.contextNavMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSectionNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.copyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.notificationPopUp = new System.Windows.Forms.GroupBox();
            this.notificationLabel = new System.Windows.Forms.Label();
            this.notificationPictureBox = new System.Windows.Forms.PictureBox();
            this.contextNavMenu.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.notificationPopUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notificationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionBar
            // 
            this.sectionBar.AutoSize = false;
            this.sectionBar.CanOverflow = false;
            this.sectionBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sectionBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.sectionBar.Location = new System.Drawing.Point(0, 0);
            this.sectionBar.Name = "sectionBar";
            this.sectionBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.sectionBar.ShowItemToolTips = false;
            this.sectionBar.Size = new System.Drawing.Size(180, 709);
            this.sectionBar.TabIndex = 1;
            this.sectionBar.Text = "toolStrip1";
            this.sectionBar.SizeChanged += new System.EventHandler(this.toolStrip_SizeChanged);
            this.sectionBar.Click += new System.EventHandler(this.toolStrip_RightClick);
            // 
            // contextNavMenu
            // 
            this.contextNavMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSectionToolStripMenuItem,
            this.editSectionNameToolStripMenuItem,
            this.deleteSectionToolStripMenuItem});
            this.contextNavMenu.Name = "contextMenuStrip1";
            this.contextNavMenu.Size = new System.Drawing.Size(176, 76);
            // 
            // newSectionToolStripMenuItem
            // 
            this.newSectionToolStripMenuItem.Name = "newSectionToolStripMenuItem";
            this.newSectionToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.newSectionToolStripMenuItem.Text = "New Section";
            this.newSectionToolStripMenuItem.Click += new System.EventHandler(this.newSectionToolStripMenuItem_Click);
            // 
            // editSectionNameToolStripMenuItem
            // 
            this.editSectionNameToolStripMenuItem.Name = "editSectionNameToolStripMenuItem";
            this.editSectionNameToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.editSectionNameToolStripMenuItem.Text = "Edit Section";
            this.editSectionNameToolStripMenuItem.Click += new System.EventHandler(this.editSectionNameToolStripMenuItem_Click);
            // 
            // deleteSectionToolStripMenuItem
            // 
            this.deleteSectionToolStripMenuItem.Name = "deleteSectionToolStripMenuItem";
            this.deleteSectionToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.deleteSectionToolStripMenuItem.Text = "Delete Section";
            this.deleteSectionToolStripMenuItem.Click += new System.EventHandler(this.deleteSectionToolStripMenuItem_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripSeparator4,
            this.copyStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator3,
            this.deleteToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.Size = new System.Drawing.Size(98, 160);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelToolStripMenuItem,
            this.textBoxToolStripMenuItem,
            this.checkBoxToolStripMenuItem,
            this.comboBoxToolStripMenuItem,
            this.buttonToolStripMenuItem,
            this.ImageToolStripMenuItem,
            this.toolStripSeparator1,
            this.groupBoxToolStripMenuItem,
            this.shapeToolStripMenuItem,
            this.toolStripSeparator2,
            this.tabControlMenuItem,
            this.tabPageMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.newToolStripMenuItem.Text = "New";
            // 
            // labelToolStripMenuItem
            // 
            this.labelToolStripMenuItem.Name = "labelToolStripMenuItem";
            this.labelToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.labelToolStripMenuItem.Tag = "label";
            this.labelToolStripMenuItem.Text = "Label";
            // 
            // textBoxToolStripMenuItem
            // 
            this.textBoxToolStripMenuItem.Name = "textBoxToolStripMenuItem";
            this.textBoxToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.textBoxToolStripMenuItem.Tag = "textbox";
            this.textBoxToolStripMenuItem.Text = "TextBox";
            // 
            // checkBoxToolStripMenuItem
            // 
            this.checkBoxToolStripMenuItem.Name = "checkBoxToolStripMenuItem";
            this.checkBoxToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.checkBoxToolStripMenuItem.Tag = "checkbox";
            this.checkBoxToolStripMenuItem.Text = "CheckBox";
            // 
            // comboBoxToolStripMenuItem
            // 
            this.comboBoxToolStripMenuItem.Name = "comboBoxToolStripMenuItem";
            this.comboBoxToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.comboBoxToolStripMenuItem.Tag = "combobox";
            this.comboBoxToolStripMenuItem.Text = "ComboBox";
            // 
            // buttonToolStripMenuItem
            // 
            this.buttonToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonToolStripMenuItem.Name = "buttonToolStripMenuItem";
            this.buttonToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.buttonToolStripMenuItem.Tag = "button";
            this.buttonToolStripMenuItem.Text = "Button";
            // 
            // ImageToolStripMenuItem
            // 
            this.ImageToolStripMenuItem.Name = "ImageToolStripMenuItem";
            this.ImageToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.ImageToolStripMenuItem.Tag = "bitmap";
            this.ImageToolStripMenuItem.Text = "Image";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // groupBoxToolStripMenuItem
            // 
            this.groupBoxToolStripMenuItem.Name = "groupBoxToolStripMenuItem";
            this.groupBoxToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.groupBoxToolStripMenuItem.Tag = "groupbox";
            this.groupBoxToolStripMenuItem.Text = "GroupBox";
            // 
            // shapeToolStripMenuItem
            // 
            this.shapeToolStripMenuItem.Name = "shapeToolStripMenuItem";
            this.shapeToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.shapeToolStripMenuItem.Tag = "shape";
            this.shapeToolStripMenuItem.Text = "Shape";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // tabControlMenuItem
            // 
            this.tabControlMenuItem.Name = "tabControlMenuItem";
            this.tabControlMenuItem.Size = new System.Drawing.Size(152, 24);
            this.tabControlMenuItem.Tag = "tabcontrol";
            this.tabControlMenuItem.Text = "TabControl";
            // 
            // tabPageMenuItem
            // 
            this.tabPageMenuItem.Name = "tabPageMenuItem";
            this.tabPageMenuItem.Size = new System.Drawing.Size(152, 24);
            this.tabPageMenuItem.Tag = "tabpage";
            this.tabPageMenuItem.Text = "TabPage";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(94, 6);
            // 
            // copyStripMenuItem
            // 
            this.copyStripMenuItem.Name = "copyStripMenuItem";
            this.copyStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.copyStripMenuItem.Text = "Copy";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(94, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // tabControl
            // 
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl.Location = new System.Drawing.Point(185, -31);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(810, 740);
            this.tabControl.TabIndex = 0;
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            // 
            // notificationPopUp
            // 
            this.notificationPopUp.Controls.Add(this.notificationLabel);
            this.notificationPopUp.Controls.Add(this.notificationPictureBox);
            this.notificationPopUp.Location = new System.Drawing.Point(0, 565);
            this.notificationPopUp.Name = "notificationPopUp";
            this.notificationPopUp.Size = new System.Drawing.Size(180, 144);
            this.notificationPopUp.TabIndex = 3;
            this.notificationPopUp.TabStop = false;
            this.notificationPopUp.Text = "groupBox1";
            // 
            // notificationLabel
            // 
            this.notificationLabel.Location = new System.Drawing.Point(12, 56);
            this.notificationLabel.Name = "notificationLabel";
            this.notificationLabel.Size = new System.Drawing.Size(159, 65);
            this.notificationLabel.TabIndex = 1;
            this.notificationLabel.Text = "label1";
            // 
            // notificationPictureBox
            // 
            this.notificationPictureBox.Image = global::Configuration_Manager.Properties.Resources._1350628015_stock_save;
            this.notificationPictureBox.Location = new System.Drawing.Point(12, 22);
            this.notificationPictureBox.Name = "notificationPictureBox";
            this.notificationPictureBox.Size = new System.Drawing.Size(32, 31);
            this.notificationPictureBox.TabIndex = 0;
            this.notificationPictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(992, 709);
            this.Controls.Add(this.notificationPopUp);
            this.Controls.Add(this.sectionBar);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Configuration Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextNavMenu.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.notificationPopUp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.notificationPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip sectionBar;
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
        private System.Windows.Forms.ToolStripMenuItem editSectionNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buttonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem copyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.GroupBox notificationPopUp;
        private System.Windows.Forms.Label notificationLabel;
        private System.Windows.Forms.PictureBox notificationPictureBox;

    }
}

