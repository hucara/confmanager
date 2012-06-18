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
	class SectionMenuView : IView
	{
		Model model;
        //ControlFactory cf;
		ToolStrip sectionMenu;
		TabControl sectionTabControl;
		ContextMenuStrip tabContextMenu;

		CToolStripButton SelectedButton;
		List<CToolStripButton> CToolStripButtons;

		public SectionMenuView(ToolStrip ts, TabControl tc, ContextMenuStrip cms)
		{
			CToolStripButtons = new List<CToolStripButton>();

			this.model = Model.getInstance();
			this.sectionMenu = ts;
			this.tabContextMenu = cms;
			this.sectionTabControl = tc;

            //this.cf = ControlFactory.getInstance();
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

			//SetUpInfoLabel();
			foreach (Section s in model.Sections)
			{
				sectionMenu.Items.Add(s.Button);
				sectionTabControl.TabPages.Add(s.Tab);

				// Set the handlers event. Safe in case of duplicated handlers.
				s.Button.Click -= ToolStripButton_Click;
				s.Button.Click += ToolStripButton_Click;

				if (s.Selected == true)
				{
					SetUpInfoLabel();
					s.Button.PerformClick();
				}
			}

			sectionMenu.Refresh();
			sectionTabControl.Refresh();
		}

		private void CleanUpView()
		{
			sectionMenu.Items.Clear();
			sectionTabControl.TabPages.Clear();
		}

		public void AddNewSection(Section s)
		{
			if (!MaxSectionsReached())
			{
                //model.Sections.Add(s);
                s.Button.Text = s.text;
                s.Tab.Text = s.text;

                Debug.WriteLine("+ Added: (" + s.text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
                model.logCreator.Append("+ Added: " + s.Name);

                UnCheckButtons();
                s.Button.Checked = true;

                UnSelectSections();
                s.Selected = true;

                Model.getInstance().uiChanged = true;  
			}
			readAndShow();
		}

        public void RenameSection(String oldName, String newName)
        {
            foreach (Section s in model.Sections)
            {
                if (s.Button.Text == oldName)
                {
                    s.Text = newName;
                }
            }
        }

		public void RemoveSection()
		{
			if (SelectedButton != null && model.Sections.Count > 0)
			{
				Section s = model.Sections.Find(se => se.Button == SelectedButton);
				Debug.WriteLine("! Removed: (" + s.text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");

				model.logCreator.Append("- Removed: " + s.Name);
				model.DeleteControl(s.Tab, true);
				model.Sections.Remove(s);

                Model.getInstance().uiChanged = true;
			}

			if (model.Sections.Count > 0) model.Sections[0].Selected = true;
			readAndShow();
		}

        public void RemoveSection(Section s)
        {
            if (SelectedButton != null && model.Sections.Count > 0)
            {
                model.Sections.Remove(s);
                Model.getInstance().uiChanged = true;

                Debug.WriteLine("! Removed: (" + s.text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
            }
            readAndShow();
        }

		private CToolStripButton CreateToolStripButton(String text)
		{
			CToolStripButton ctsb = ControlFactory.BuildCToolStripButton(text);
			sectionMenu.Items.Add(ctsb);
			return ctsb;
		}

		private TabPage CreateTabPage()
		{
			TabPage ctp = new TabPage();
			sectionTabControl.TabPages.Add(ctp);
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

				int buttonIndex = sectionMenu.Items.IndexOf(SelectedButton);
				int labelIndex = 0;

				for (int i = 0; i < sectionMenu.Items.Count; i++)
				{
					if (sectionMenu.Items[i] is ToolStripTextBox) labelIndex = i;
				}

				MoveInfoLabel(buttonIndex, labelIndex);
				sectionMenu.Refresh();

				model.CurrentSection = s;

				Debug.WriteLine("! Clicked: " + b.Name + " \"" + b.Text + "\"");
				sectionTabControl.SelectTab(s.Tab);
				Debug.WriteLine("! Selected: " + model.CurrentSection.Name + " with Text: " + s.text);
			}
		}

		private void UnCheckButtons()
		{
			foreach (CToolStripButton bt in sectionMenu.Items.OfType<CToolStripButton>())
			{
				bt.Checked = false;
			}
		}

		private void UnSelectSections()
		{
			foreach (Section se in model.Sections)
			{
				se.Selected = false;
			}
		}

		public void SetSelectedButton(CToolStripButton toolStripItem)
		{
			if (toolStripItem != null)
			{
				UnCheckButtons();
				SelectedButton = toolStripItem;
			}
		}

		private bool MaxSectionsReached()
		{
			if (model.Sections.Count < Model.getInstance().maxSections) return false;
			else return true;
		}

		private void SetUpInfoLabel()
		{
			if (sectionMenu.Items.Count > 0)
			{
				ToolStripTextBox infoLabel = new ToolStripTextBox();

				// We should add the label
				int index = sectionMenu.Items.Add(infoLabel);

				infoLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
				infoLabel.AutoSize = false;
				infoLabel.MaxLength = 320;
				infoLabel.BorderStyle = BorderStyle.None;
                infoLabel.BackColor = System.Drawing.SystemColors.Control;

                
				infoLabel.TextBox.MinimumSize = new System.Drawing.Size(100, 275);
				infoLabel.TextBox.Enabled = false;
				infoLabel.TextBox.Multiline = true;
				
				infoLabel.ReadOnly = true;

				int selButtonIndex = sectionMenu.Items.IndexOf(SelectedButton);
				int lablIndex = sectionMenu.Items.IndexOf(infoLabel);

				MoveInfoLabel(index, lablIndex);
			}
		}

        //
        // Relocates the info label depending on the selected section.
        //
		private void MoveInfoLabel(int buttonIndex, int labelIndex)
		{
			System.Diagnostics.Debug.WriteLine("Moving Info Label - buttonIndex: " + buttonIndex + " - labelIndex: " + labelIndex);
			ToolStripTextBox infoLabel = sectionMenu.Items[labelIndex] as ToolStripTextBox;

			infoLabel.Text = "";

			if (buttonIndex +1 >= sectionMenu.Items.Count)
			{
				sectionMenu.Items.Add(infoLabel);
				
				model.InfoLabel = infoLabel;
			}
			else
			{
				if (buttonIndex >= labelIndex) buttonIndex++;
				ToolStripButton fb = sectionMenu.Items[buttonIndex] as ToolStripButton;
				sectionMenu.Items.Insert(buttonIndex, infoLabel);
				sectionMenu.Items.Remove(fb);
				sectionMenu.Items.Insert(buttonIndex, fb);

				model.InfoLabel = infoLabel;
			}
		}
	}
}