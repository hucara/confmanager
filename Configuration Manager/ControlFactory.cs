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

        public CLabel BuildCLabel(Control parent)
        {
            CLabel c = new CLabel();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CToolStripButton BuildCToolStripButton(Section s)
        {
            CToolStripButton c = new CToolStripButton(s);
            c.SetSectionDescription(s);

            return c;
        }

        public CTabPage BuildCTabPage(Section s)
        {
            CTabPage c = new CTabPage();
            c.MouseDown += ch.Control_RightClick;
            c.SetNavBarDescription(s);

            return c;
        }

        public CToolStripButton BuildCToolStripButton(String s)
        {
            CToolStripButton c = new CToolStripButton();
            c.SetSectionName(s);

            return c;
        }

        // This builder is just for using with the TabControl.
        // Here the tabpage is not referenced inside the Allcontrols.
        // So be careful! D:
        public CTabPage BuildCTabPage()
        {
            CTabPage c = new CTabPage();
            c.MouseDown += ch.Control_RightClick;
            //Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CTabPage BuildCTabPage(Control parent)
        {
            CTabPage c = new CTabPage();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();
            c.Parent = parent;

            return c;
        }

        public CTabControl BuildCTabControl(Control parent)
        {
            CTabControl c = new CTabControl(BuildCTabPage());
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            c.SetControlDescription();
            Model.getInstance().AllControls.Add(c);

            return c;
        }

        public CTabControl BuildCTabControl(Control parent, int n)
        {
            CTabControl c = new CTabControl();

            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    c.TabPages.Add(BuildCTabPage(c as Control));
                }
            }
            else
            {
                c.TabPages.Clear();
            }

            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            c.SetControlDescription();
            Model.getInstance().AllControls.Add(c);

            return c;
        }

        public CComboBox BuildCComboBox(Control parent)
        {
            CComboBox c = new CComboBox();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CTextBox BuildCTextBox(Control parent)
        {
            CTextBox c = new CTextBox();
            parent.Controls.Add(c);

            c.MouseDown += ch.CTextBox_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CCheckBox BuildCCheckBox(Control parent)
        {
            CCheckBox c = new CCheckBox();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CGroupBox BuildCGroupBox(Control parent)
        {
            CGroupBox c = new CGroupBox();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }

        public CPanel BuildCPanel(Control parent)
        {
            CPanel c = new CPanel();
            parent.Controls.Add(c);

            c.MouseDown += ch.Control_RightClick;

            Model.getInstance().AllControls.Add(c);
            c.SetControlDescription();

            return c;
        }
    }
}