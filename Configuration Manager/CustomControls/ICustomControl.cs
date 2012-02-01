using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    interface ICustomControl
    {
        //List<Control> RelatedControls { get; set; }

        void SetControlDescription(ControlDescription cd);
        //void addRelatedControl(Control c);
    }
}
