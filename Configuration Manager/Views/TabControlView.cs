using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;

namespace Configuration_Manager.Views
{
    class TabControlView : IView
    {
        TabControl configurationTabs;
        ContextMenuStrip tabContextMenu;
        Model model;

        public TabControlView(TabControl tc, ContextMenuStrip cms, Model model)
        {
            this.tabContextMenu = cms;
            this.configurationTabs = tc;
            this.model = model;
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
            foreach (Section s in model.Sections)
            {
                s.Tab.Click -= TabControl_RightClick;
                s.Tab.Click += TabControl_RightClick;
            }

            configurationTabs.Refresh();
        }

        private void CleanUpView()
        {
        }

        private void TabControl_RightClick(object sender, EventArgs e)
        {
            Control c = sender as Control;

            MouseEventArgs me = e as MouseEventArgs;
            TabPage tc = sender as TabPage;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                tabContextMenu.Show(tc, me.X, me.Y);

                model.CurrentClickParent = c;
                model.CurrentX = me.X;
                model.CurrentY = me.Y;

                Debug.WriteLine("! Clicked: " + c.Name);
                Debug.WriteLine("! Clicked: " + model.CurrentClickParent + " in X: " +model.CurrentX+ " - Y: "+model.CurrentY);
            }
        }
    }
}