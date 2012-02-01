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

        public CComboBox BuildCButton(ControlDescription cd)
        {
            return null;
        }

        public CLabel BuildCLabel(ControlDescription cd)
        {
            CLabel lbl = new CLabel(new Label());
            lbl.SetControlDescription(cd);
            return lbl;
        }

        public CToolStripButton BuildCToolStripButton(ControlDescription cd)
        {
            CToolStripButton tsb = new CToolStripButton();
            //CToolStripButton tsb = new CToolStripButton(new ToolStripButton());
            tsb.SetControlDescription(cd);
            return tsb;
        }

        public CTabPage BuildCTabPage(ControlDescription cd)
        {
            CTabPage ctp = new CTabPage(new TabPage());
            ctp.SetControlDescription(cd);
            return ctp;
        }
    }
}
