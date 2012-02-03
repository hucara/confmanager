using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class CShape
    {
        static int count = 0;
        public int typeId;

        public List<ICustomControl> RelatedControls { get; private set; }

        public CShape()
        {

        }

        public void setControlDescription(ControlDescription cd)
        {
        }
    }
}
