using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    interface ICustomControl
    {
        public Control c;
        public List<Control> RelatedControls;
    }
}
