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

                if (s.Selected == true)
                {
                    s.Button.PerformClick();
                }
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
				Section s = cf.BuildSection(text, text, true);

                if (!model.Sections.Contains(s))
                {
                    model.Sections.Add(s);
                    Debug.WriteLine("+ Added: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name+"}");
                }

				model.logCreator.Append("+ Added: " + s.Name);

				UnCheckButtons();
				s.Button.Checked = true;

				UnSelectSections();
				s.Selected = true;
            }
            readAndShow();
        }        

        public void RemoveSection()
        {
            if (SelectedButton != null && model.Sections.Count > 0)
            {
                Section s = model.Sections.Find(se => se.Button == SelectedButton);
                Debug.WriteLine("! Removed: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");

				model.logCreator.Append("- Removed: " + s.Name);
				model.DeleteControl(s.Tab);
                model.Sections.Remove(s);
            }

			if (model.Sections.Count > 0) model.Sections[0].Selected = true;
            readAndShow();

			// Delete all the controls that have this section as parent.
        }

        private CToolStripButton CreateToolStripButton(String text)
        {
            CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(text);
            navigationBar.Items.Add(ctsb);
            return ctsb;
        }

        private TabPage CreateTabPage()
        {
            TabPage ctp = new TabPage();
            configurationTabs.TabPages.Add(ctp);
            return ctp;
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is CToolStripButton)
            {
                CToolStripButton b = (CToolStripButton)sender;
                SelectedButton = b;

				UnCheckButtons();
				SelectedButton.Checked = true;
                
                Section s = model.Sections.Find(se => se.Button == SelectedButton);
				
				UnSelectSections();
				s.Selected = true;
                
				model.CurrentSection = s;

                Debug.WriteLine("! Clicked: " + b.Name + " \"" + b.Text +"\"");
                configurationTabs.SelectTab(s.Tab);
                Debug.WriteLine("! Selected: " + model.CurrentSection.Name + " with Text: "+ s.Text);
            }
        }

        private void UnCheckButtons()
        {
            foreach (CToolStripButton bt in navigationBar.Items)
            {
                bt.Checked = false;
            }
        }

		private void UnSelectSections()
		{
			foreach(Section se in model.Sections)
			{
				se.Selected = false;
			}
		}

        public void SetSelectedButton(CToolStripButton toolStripItem)
        {
            if (toolStripItem == null) throw new ArgumentNullException();

			UnCheckButtons();
            SelectedButton = toolStripItem;
        }

        private bool MaxSectionsReached()
        {
			if (model.Sections.Count < Model.getInstance().maxSections) return false;
            else return true;
        }
    }
}
