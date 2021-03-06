﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;
using Configuration_Manager.Views;

/* 
 * Implementation of the Factory Design Pattern, 
 * meant to create objects on demand.
 */

namespace Configuration_Manager
{
    static class ControlFactory
    {
        static private CustomHandler ch;
        static private Model model = Model.getInstance();

        static public void SetCustomHandler(CustomHandler custom)
        {
            ch = custom;
        }

        static public Section BuildSection(String name, String text, bool selected)
        {
            while (model.sections.Exists(e => e.Name == "Section" + Section.count)) Section.count++;
            CToolStripButton ctsb = BuildCToolStripButton(text);
            TabPage tp = BuildTabPage(name);

            Section s = new Section(ctsb, tp, text, selected);
            Model.getInstance().currentSection = s;
            Model.getInstance().sections.Add(s);

            model.logCreator.Append("+ Added: " + s.Name);
            return s;
        }

        static public TabPage BuildTabPage(String name)
        {
            TabPage tp = new TabPage(name);
            SetDragDropHandlers(tp);
            tp.AllowDrop = true;
            //tp.MouseHover += model.UpdateInfoLabel;

            return tp;
        }

        static public CLabel BuildCLabel(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CLabel" + CLabel.count)) CLabel.count++;

            CLabel c = new CLabel();
            parent.Controls.Add(c);

            c.SetControlDescription();
            SetCommonHandlers(c);

            Model.getInstance().allControls.Add(c);
            c.cd.RealText = c.cd.Text;
            model.logCreator.Append("+ Added: " + c.cd.Name);
            return c;
        }

        static public CToolStripButton BuildCToolStripButton(Section s)
        {
            CToolStripButton c = new CToolStripButton(s);
            c.SetSectionDescription(s);
            c.MouseHover += model.UpdateInfoLabel;
            c.MouseLeave += model.EraseInfoLabel;
            return c;
        }

        static public CToolStripButton BuildCToolStripButton(String s)
        {
            CToolStripButton c = new CToolStripButton();
            c.SetSectionName(s);
            c.MouseHover += model.UpdateInfoLabel;
            c.MouseLeave += model.EraseInfoLabel;
            return c;
        }

        static public CTabPage BuildCTabPage(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CTabPage" + CTabPage.count)) CTabPage.count++;
            CTabPage c = new CTabPage();
            parent.Controls.Add(c);
            (parent as CTabControl).SelectedTab = c;

            SetCommonHandlers(c);
            SetDragDropHandlers(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();
            c.Parent = parent;

            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);
            return c;
        }

        static public CTabControl BuildCTabControl(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CTabControl" + CTabControl.count)) CTabControl.count++;
            CTabControl c = new CTabControl();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            c.AllowDrop = true;

            c.SetControlDescription();
            c.cd.RealText = c.cd.Text;
            //c.SelectedIndexChanged += IndexChanged;

            Model.getInstance().allControls.Add(c);
            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        static public CComboBox BuildCComboBox(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CComboBox" + CComboBox.count)) CComboBox.count++;
            CComboBox c = new CComboBox();
            parent.Controls.Add(c);

            c.SelectedIndexChanged += CoupledControlsManager.ComboBoxCoupled;
            c.SelectedIndexChanged += VisibilityRelationManager.ComboBoxVisibility;
            c.SelectedIndexChanged += ReadRelationManager.ReadRelationUpdate;

            SetCommonHandlers(c);
            SetChangesHandler(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();
            c.DropDownStyle = ComboBoxStyle.DropDownList;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        static public CTextBox BuildCTextBox(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CTextBox" + CTextBox.count)) CTextBox.count++;
            CTextBox c = new CTextBox();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            SetChangesHandler(c);

            c.MouseDown += ch.CTextBox_RightClick;
            c.TextChanged += ch.TextChanged;
            c.TextChanged += ReadRelationManager.ReadRelationUpdate;

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.AllowDrop = false;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        static public CButton BuildCButton(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CButton" + CButton.count)) CButton.count++;
            CButton c = new CButton();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            //SetChangesHandler(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.Click += ch.CButton_Click;
            c.AllowDrop = false;

            model.logCreator.Append("+ Added " + c.cd.Name);
            return c;
        }

        static public CBitmap BuildCBitmap(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CBitmap" + CBitmap.count)) CBitmap.count++;
            CBitmap c = new CBitmap();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            SetChangesHandler(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();
            (c as PictureBox).SizeMode = PictureBoxSizeMode.StretchImage;
            (c as PictureBox).Image = System.Drawing.SystemIcons.Error.ToBitmap();

            model.logCreator.Append("+ Added " + c.cd.Name);
            return c;
        }

        static public CCheckBox BuildCCheckBox(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CCheckBox" + CCheckBox.count)) CCheckBox.count++;
            CCheckBox c = new CCheckBox();
            parent.Controls.Add(c);

            c.CheckStateChanged += CoupledControlsManager.CheckBoxCoupled;
            c.CheckStateChanged += VisibilityRelationManager.CheckBoxVisibility;
            c.CheckStateChanged += ReadRelationManager.ReadRelationUpdate;

            SetCommonHandlers(c);
            SetChangesHandler(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        static public CGroupBox BuildCGroupBox(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CGroupBox" + CGroupBox.count)) CGroupBox.count++;
            CGroupBox c = new CGroupBox();
            parent.Controls.Add(c);
            SetCommonHandlers(c);
            SetDragDropHandlers(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.AllowDrop = true;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            //AddSnapGrid(c);

            return c;
        }

        static public CPanel BuildCPanel(Control parent)
        {
            while (model.allControls.Exists(l => l.cd.Name == "CPanel" + CPanel.count)) CPanel.count++;
            CPanel c = new CPanel();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            SetDragDropHandlers(c);

            Model.getInstance().allControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.AllowDrop = true;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        static private void SetCommonHandlers(Control c)
        {
            c.AllowDrop = false;
            c.MouseDown += ch.Control_Click;
            c.MouseUp += ch.CancelDragDropTimer;
            c.MouseHover += model.UpdateInfoLabel;
            //c.MouseHover += ch.ShowControlInfoHint;
            c.MouseLeave += model.EraseInfoLabel;

            Model.getInstance().uiChanged = true;
        }

        static private void SetDragDropHandlers(Control c)
        {
            c.AllowDrop = true;
        }

        static private void SetChangesHandler(Control c)
        {
            if (c is CComboBox) (c as CComboBox).SelectedIndexChanged += ch.ValueChanged;
            else if (c is CTextBox) (c as CTextBox).TextChanged += ch.ValueChanged;
            else if (c is CCheckBox) (c as CCheckBox).CheckStateChanged += ch.ValueChanged;
        }

        static public void IndexChanged(object sender, EventArgs e)
        {
            (sender as ICustomControl).cd.SelectedTab = (sender as TabControl).TabIndex;
        }

        static public void AddSnapGrid(Control c)
        {
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.Size = c.Size;
            tlp.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tlp.RowCount = c.Size.Height / 30;
            tlp.ColumnCount = c.Size.Width / 75;
            tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlp.Top = 20;
            tlp.Left = 10;
            tlp.Visible = true;
            c.Controls.Add(tlp);
        }
    }
}