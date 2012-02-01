using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.Views
{
    interface IView
    {
        // Takes the info from the UI, gets the changes made by the 
        // user and stores them inside the model  / views.
        void saveToModel();

        // Reads the info from the model / views and fills-out the
        // UI with that info, refreshing the components.
        void readAndShow();
    }
}
