using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

/* 
 * Implementation of the Factory Design Pattern, 
 * meant to create objects on demand.
 */

namespace Configuration_Manager
{
    class ControlFactory
    {
        private static ControlFactory cf;
        CustomHandler ch;
		Util.TokenTextTranslator tr = new Util.TokenTextTranslator("@@");

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

			if (selected) ctsb.PerformClick();
			Section s = new Section(ctsb, tp, text, selected);

			Model.getInstance().CurrentSection = s;

			return s;
		}

		public TabPage BuildTabPage(String name)
		{
			TabPage tp = new TabPage(name);
			return tp;
		}

        public CLabel BuildCLabel(Control parent)
        {
            CLabel c = new CLabel();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;
			c.MouseMove += ch.OnMouseMove;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.cd.RealText = c.cd.Text;

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

        // This builder is rubish. It is needed, but do not use it!
		// It is a "fake" tab page for the creation of a TabControl.
        // Here the tabpage is not referenced inside the Allcontrols.
        public CTabPage BuildCTabPage()
        {
            CTabPage c = new CTabPage();
			c.MouseDown += ch.Control_Click;
            //Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CTabPage BuildCTabPage(Control parent)
        {
            CTabPage c = new CTabPage();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.DragDrop += ch.OnDragDrop;
			c.DragEnter += ch.OnDragEnter;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();
            c.Parent = parent;

			c.cd.RealText = c.cd.Text;

            return c;
        }

        public CTabControl BuildCTabControl(Control parent)
        {
            CTabControl c = new CTabControl(BuildCTabPage());
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;

            c.SetControlDescription();
			c.cd.RealText = c.cd.Text;

            Model.getInstance().AllControls.Add(c);

            return c;
        }

        public CComboBox BuildCComboBox(Control parent)
        {
            CComboBox c = new CComboBox();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.DropDownStyle = ComboBoxStyle.DropDownList;

            return c;
        }

        public CTextBox BuildCTextBox(Control parent)
        {
            CTextBox c = new CTextBox();
            parent.Controls.Add(c);

            c.MouseDown += ch.CTextBox_RightClick;
			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;
			c.TextChanged += ch.TextChanged;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.cd.RealText = c.cd.Text;

            return c;
        }

        public CCheckBox BuildCCheckBox(Control parent)
        {
            CCheckBox c = new CCheckBox();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.cd.RealText = c.cd.Text;

            return c;
        }

        public CGroupBox BuildCGroupBox(Control parent)
        {
            CGroupBox c = new CGroupBox();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;

			c.DragDrop += ch.OnDragDrop;
			c.DragEnter += ch.OnDragEnter;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.cd.RealText = c.cd.Text;
			c.AllowDrop = true;
			
            return c;
        }

        public CPanel BuildCPanel(Control parent)
        {
            CPanel c = new CPanel();
            parent.Controls.Add(c);

			c.MouseDown += ch.Control_Click;
			c.MouseUp += ch.CancelDragDropTimer;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

			c.cd.RealText = c.cd.Text;
			c.AllowDrop = true;

            return c;
        }
    }
}