using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

/* 
 * DESIGN PATTERN
 * ControlFactory.cs 
 * - Implementation of the Factory Design Pattern, 
 *   meant to create objects on demand.
 */

namespace Configuration_Manager
{
    class ControlFactory
    {
        private static ControlFactory cf;

        public static ControlFactory getInstance()
        {
            if (cf == null)
            {
                cf = new ControlFactory();
            }
            return cf;
        }

        public CButton BuildCButton(ControlDescription cd)
        {
            CButton cb = new CButton();
            cb.SetControlDescription(cd);
            return cb;
        }

        public CLabel BuildCLabel(ControlDescription cd)
        {
            CLabel lbl = new CLabel();
            lbl.SetControlDescription(cd);
            return lbl;
        }

        public CToolStripButton BuildCToolStripButton(ControlDescription cd)
        {
            CToolStripButton tsb = new CToolStripButton();
            tsb.SetControlDescription(cd);
            return tsb;
        }

        public CTabPage BuildCTabPage(ControlDescription cd)
        {
            CTabPage ctp = new CTabPage();
            ctp.SetControlDescription(cd);
            return ctp;
        }
    }
}
