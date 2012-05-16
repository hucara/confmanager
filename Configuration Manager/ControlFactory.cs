using System;
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
    class ControlFactory
    {
        private static ControlFactory cf;
        private CustomHandler ch;
        private ReadRelationManager rm = new ReadRelationManager();

        private Model model = Model.getInstance();

        public static ControlFactory getInstance()
        {
            if (cf == null)
            {
                cf = new ControlFactory();
            }
            return cf;
        }

        public void SetCustomHandler(CustomHandler ch)
        {
            this.ch = ch;
        }

        public Section BuildSection(String name, String text, bool selected)
        {
            CToolStripButton ctsb = BuildCToolStripButton(text);
            TabPage tp = BuildTabPage(name);

            Section s = new Section(ctsb, tp, text, selected);
            Model.getInstance().CurrentSection = s;

            model.logCreator.Append("+ Added: " + s.Name);

            return s;
        }

        public TabPage BuildTabPage(String name)
        {
            TabPage tp = new TabPage(name);
            return tp;
        }

        public CLabel BuildCLabel(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CLabel" + CLabel.count)) CLabel.count++;

            CLabel c = new CLabel();
            parent.Controls.Add(c);
            c.SetControlDescription();
            SetCommonHandlers(c);

            Model.getInstance().AllControls.Add(c);
            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CToolStripButton BuildCToolStripButton(Section s)
        {
            CToolStripButton c = new CToolStripButton(s);
            c.SetSectionDescription(s);

            return c;
        }

        public CToolStripButton BuildCToolStripButton(String s)
        {
            CToolStripButton c = new CToolStripButton();
            c.SetSectionName(s);

            return c;
        }

        public CTabPage BuildCTabPage(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CTabPage" + CTabPage.count)) CTabPage.count++;
            CTabPage c = new CTabPage();
            parent.Controls.Add(c);

            (parent as CTabControl).SelectedTab = c;

            SetCommonHandlers(c);
            c.DragDrop += ch.OnDragDrop;
            c.DragEnter += ch.OnDragEnter;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();
            c.Parent = parent;

            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CTabControl BuildCTabControl(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CTabControl" + CTabControl.count)) CTabControl.count++;
            CTabControl c = new CTabControl();
            parent.Controls.Add(c);

            SetCommonHandlers(c);

            c.SetControlDescription();
            c.cd.RealText = c.cd.Text;

            c.SelectedIndexChanged += IndexChanged;

            Model.getInstance().AllControls.Add(c);

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CComboBox BuildCComboBox(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CComboBox" + CComboBox.count)) CComboBox.count++;
            CComboBox c = new CComboBox();
            parent.Controls.Add(c);

            c.SelectedIndexChanged += CoupledControlsManager.ComboBoxCoupled;
            c.SelectedIndexChanged += VisibilityRelationManager.ComboBoxVisibility;
            c.SelectedIndexChanged += rm.ReadRelationUpdate;

            SetCommonHandlers(c);
            SetChangesHandler(c);

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            c.DropDownStyle = ComboBoxStyle.DropDownList;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CTextBox BuildCTextBox(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CTextBox" + CTextBox.count)) CTextBox.count++;
            CTextBox c = new CTextBox();
            parent.Controls.Add(c);

            SetCommonHandlers(c);
            SetChangesHandler(c);

            c.MouseDown += ch.CTextBox_RightClick;
            c.TextChanged += ch.TextChanged;
            c.TextChanged += rm.ReadRelationUpdate;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CCheckBox BuildCCheckBox(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CCheckBox" + CCheckBox.count)) CCheckBox.count++;
            CCheckBox c = new CCheckBox();
            parent.Controls.Add(c);

            c.CheckStateChanged += CoupledControlsManager.CheckBoxCoupled;
            c.CheckStateChanged += VisibilityRelationManager.CheckBoxVisibility;
            c.CheckStateChanged += rm.ReadRelationUpdate;

            SetCommonHandlers(c);
            SetChangesHandler(c);

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CGroupBox BuildCGroupBox(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CGroupBox" + CGroupBox.count)) CGroupBox.count++;
            CGroupBox c = new CGroupBox();
            parent.Controls.Add(c);
            SetCommonHandlers(c);

            c.DragDrop += ch.OnDragDrop;
            c.DragEnter += ch.OnDragEnter;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.AllowDrop = true;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        public CPanel BuildCPanel(Control parent)
        {
            while (model.AllControls.Exists(l => l.cd.Name == "CPanel" + CPanel.count)) CPanel.count++;
            CPanel c = new CPanel();
            parent.Controls.Add(c);

            SetCommonHandlers(c);

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            c.cd.RealText = c.cd.Text;
            c.AllowDrop = true;

            model.logCreator.Append("+ Added: " + c.cd.Name);

            return c;
        }

        private void SetCommonHandlers(Control c)
        {
            c.MouseDown += ch.Control_Click;
            c.MouseUp += ch.CancelDragDropTimer;
            c.MouseHover += model.UpdateInfoLabel;
            c.MouseLeave += model.EraseInfoLabel;
        }

        private void SetDragDropHandlers(Control c)
        {
        }

        private void SetChangesHandler(Control c)
        {
            if (c is CComboBox) (c as CComboBox).SelectedIndexChanged += ch.Changed;
            else if (c is CTextBox) (c as CTextBox).TextChanged += ch.Changed;
            else if (c is CCheckBox) (c as CCheckBox).CheckStateChanged += ch.Changed;
        }

        public void IndexChanged(object sender, EventArgs e)
        {
            (sender as ICustomControl).cd.SelectedTab = (sender as TabControl).TabIndex;
        }
    }
}