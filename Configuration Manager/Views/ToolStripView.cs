using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;

namespace Configuration_Manager.Views
{
    class ToolStripView : IView
    {
        Model model;
        ControlFactory cf;
        ToolStrip navigationBar;
        TabControl configurationTabs;
        ContextMenuStrip tabContextMenu;

        CToolStripButton ctsb;
        CTabPage ctp;

        CToolStripButton SelectedButton;
        List<CToolStripButton> CToolStripButtons;

        public ToolStripView(ToolStrip ts, TabControl tc, ContextMenuStrip cms, Model model)
        {
            CToolStripButtons = new List<CToolStripButton>();

            this.model = model;
            this.navigationBar = ts;
            this.tabContextMenu = cms;
            this.configurationTabs = tc;

            this.cf = ControlFactory.getInstance();
        }

        // Takes the info from the UI, gets the changes made by the 
        // user and stores them inside the model  / views.
        public void saveToModel()
        {
        }

        // Reads the info from the model / views and fills-out the
        // UI with that info, refreshing the components.
        public void readAndShow()
        {
            CleanUpView();

            foreach (Section s in model.Sections)
            {
                navigationBar.Items.Add(s.Button);
                configurationTabs.TabPages.Add(s.Tab);

                // Set the handlers event. Safe in case of duplicated handlers.
                s.Button.Click -= ToolStripButton_Click;
                s.Button.Click += ToolStripButton_Click;
            }

            navigationBar.Refresh();
            configurationTabs.Refresh();
        }

        private void CleanUpView()
        {
            navigationBar.Items.Clear();
            configurationTabs.TabPages.Clear();
        }

        public void AddNewSection(String text)
        {
            if (!MaxSectionsReached())
            {
                ctsb = CreateToolStripButton(text);
                ctp = CreateTabPage();
                Section s = new Section(ctsb, ctp, text, false);

                if (!model.Sections.Contains(s))
                {
                    model.Sections.Add(s);
                    Debug.WriteLine("+ Added: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name+"}");
                }

                ctsb.PerformClick();
                UnCheckButtons(ctsb);
            }

            readAndShow();
        }        

        public void RemoveSection()
        {
            if (SelectedButton != null && model.Sections.Count > 0)
            {
                Section s = model.Sections.Find(se => se.Button == SelectedButton);
                Debug.WriteLine("! Removed: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");

                model.Sections.Remove(s);
            }

            readAndShow();
        }

        private CToolStripButton CreateToolStripButton(String text)
        {
            CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(text);
            navigationBar.Items.Add(ctsb);
            return ctsb;
        }

        private CTabPage CreateTabPage()
        {
            CTabPage ctp = ControlFactory.getInstance().BuildCTabPage();
            configurationTabs.TabPages.Add(ctp);
            return ctp;
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is CToolStripButton)
            {
                CToolStripButton b = (CToolStripButton)sender;
                SelectedButton = b;

                UnCheckButtons(b);

                Section s = model.Sections.Find(se => se.Button == SelectedButton);

                Debug.WriteLine("! Clicked: " + b.Name + " \"" + b.Text +"\"");
                configurationTabs.SelectTab(s.Tab);
                Debug.WriteLine("! Selected Tab: " + s.Tab.Name);
            }
        }

        private void UnCheckButtons(CToolStripButton b)
        {
            foreach (CToolStripButton bt in navigationBar.Items)
            {
                if (bt != b)
                {
                    bt.Checked = false;
                }
            }
        }

        public void SetSelectedButton(CToolStripButton toolStripItem)
        {
            if (toolStripItem == null) throw new ArgumentNullException();

            SelectedButton = toolStripItem;
            UnCheckButtons(SelectedButton);
        }

        private bool MaxSectionsReached()
        {
            if (model.Sections.Count < Model.MAX_SECTIONS) return false;
            else return true;
        }
    }
}
