using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    public interface ICustomControl
    {
        ControlDescription cd { get; set; }
        void SetControlDescription();
    }
}
