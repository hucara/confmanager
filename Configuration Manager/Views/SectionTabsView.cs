using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;

namespace Configuration_Manager.Views
{
    class SectionTabsView : IView
    {
        TabControl tabControl;
        CustomHandler ch;
        //ControlFactory cf;
        Model model;
        ControlEditor editor = new ControlEditor();

        public SectionTabsView(TabControl tc, CustomHandler ch)
        {
            this.tabControl = tc;
            this.ch = ch;

            this.model = Model.getInstance();
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
            foreach (Section s in model.sections)
            {
				s.Tab.MouseClick -= ch.Control_Click;
				s.Tab.MouseClick += ch.Control_Click;
            }

            tabControl.Refresh();
        }

        private void CleanUpView()
        {
        }
    }
}