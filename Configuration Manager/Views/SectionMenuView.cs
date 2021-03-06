﻿using System;
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
			foreach (Section s in model.sections)
			{
				sectionMenu.Items.Add(s.Button);
				sectionTabControl.TabPages.Add(s.Tab);

				// Set the handlers event. Safe in case of duplicated handlers.
				s.Button.Click -= ToolStripButton_Click;
				s.Button.Click += ToolStripButton_Click;

				if (s.Selected)
				{
					SetUpInfoTextBox();
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
                s.Button.Text = s.Text;
                s.Tab.Text = s.Text;

                Debug.WriteLine("+ Added: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
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
            foreach (Section s in model.sections)
            {
                if (s.Button.Text == oldName)
                    s.Text = newName;
            }
        }

		public void RemoveSection()
		{
			if (SelectedButton != null && model.sections.Count > 0)
			{
				Section s = model.sections.Find(se => se.Button == SelectedButton);
				Debug.WriteLine("! Removed: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");

				model.logCreator.Append("- Removed: " + s.Name);
				model.DeleteControl(s.Tab, true);
				model.sections.Remove(s);

                Model.getInstance().uiChanged = true;
			}

			if (model.sections.Count > 0) model.sections[0].Selected = true;
			readAndShow();
		}

        public void RemoveSection(Section s)
        {
            if (SelectedButton != null && model.sections.Count > 0)
            {
                model.sections.Remove(s);
                Model.getInstance().uiChanged = true;

                Debug.WriteLine("! Removed: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
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

				Section s = model.sections.Find(se => se.Button == SelectedButton);

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

				model.currentSection = s;

				Debug.WriteLine("! Clicked: " + b.Name + " \"" + b.Text + "\"");
				sectionTabControl.SelectTab(s.Tab);
				Debug.WriteLine("! Selected: " + model.currentSection.Name + " with Text: " + s.Text);
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
			foreach (Section se in model.sections)
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
			if (model.sections.Count < Model.getInstance().maxSections) return false;
			else return true;
		}

		private void SetUpInfoTextBox()
		{
			if (sectionMenu.Items.Count > 0)
			{
				ToolStripTextBox infoTextBox = new ToolStripTextBox();

				// We should add the label
				int index = sectionMenu.Items.Add(infoTextBox);

				infoTextBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
				infoTextBox.AutoSize = false;
				infoTextBox.MaxLength = 320;
				infoTextBox.BorderStyle = BorderStyle.None;
                infoTextBox.BackColor = System.Drawing.SystemColors.Control;

                
				infoTextBox.TextBox.MinimumSize = new System.Drawing.Size(100, model.infoTextBoxHeight);
				infoTextBox.TextBox.Enabled = false;
				infoTextBox.TextBox.Multiline = true;
				
				infoTextBox.ReadOnly = true;

				int selButtonIndex = sectionMenu.Items.IndexOf(SelectedButton);
				int lablIndex = sectionMenu.Items.IndexOf(infoTextBox);

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
				model.infoLabel = infoLabel;
			}
			else
			{
				if (buttonIndex >= labelIndex) buttonIndex++;
				ToolStripButton fb = sectionMenu.Items[buttonIndex] as ToolStripButton;
				sectionMenu.Items.Insert(buttonIndex, infoLabel);
				sectionMenu.Items.Remove(fb);
				sectionMenu.Items.Insert(buttonIndex, fb);

				model.infoLabel = infoLabel;
			}
		}
	}
}