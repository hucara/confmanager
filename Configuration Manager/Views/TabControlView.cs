using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.Views
{
    class TabControlView : IView
    {
        System.Windows.Forms.TabControl tc;
        Model model;

        public TabControlView(System.Windows.Forms.TabControl tc, Model model)
        {
            this.tc = tc;
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
        }
    }
}
