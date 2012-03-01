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
        TabControl tabControl;
        CustomHandler ch;
        ControlFactory cf;
        Model model;
        Editor editor = new Editor();

        public TabControlView(TabControl tc, CustomHandler ch)
        {
            this.tabControl = tc;
            this.ch = ch;

            this.model = Model.getInstance();
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
            foreach (Section s in model.Sections)
            {
                s.Tab.MouseClick -= ch.Control_RightClick;
                s.Tab.MouseClick += ch.Control_RightClick;
            }

            tabControl.Refresh();
        }

        private void CleanUpView()
        {
        }
    }
}