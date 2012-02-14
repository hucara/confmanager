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
            //lbl.SetControlDescription(cd);
            return lbl;
        }

        public CToolStripButton BuildCToolStripButton(Section s)
        {
            CToolStripButton tsb = new CToolStripButton(s);
            tsb.SetSectionDescription(s);
            return tsb;
        }

        public CTabPage BuildCTabPage(Section s)
        {
            CTabPage ctp = new CTabPage();
            ctp.SetNavBarDescription(s);
            return ctp;
        }

        public CToolStripButton BuildCToolStripButton(String s)
        {
            CToolStripButton tsb = new CToolStripButton();
            tsb.SetSectionName(s);
            return tsb;
        }

        public CTabPage BuildCTabPage()
        {
            CTabPage ctp = new CTabPage();
            return ctp;
        }

        public CComboBox BuildCComboBox(ControlDescription cd)
        {
            CComboBox ccb = new CComboBox();
            //ccb.SetControlDescription(cd);
            return ccb;
        }

        public CTextBox BuildCTextBox(ControlDescription cd)
        {
            CTextBox ctb = new CTextBox();
            //ccb.SetControlDescription(cd);
            return ctb;
        }

        public CCheckBox BuildCCheckBox(ControlDescription cd)
        {
            CCheckBox ccb = new CCheckBox();
            return ccb;
        }

        public CGroupBox BuildCGroupBox(ControlDescription cd)
        {
            CGroupBox cgb = new CGroupBox();
            return cgb;
        }

        public CPanel BuildCPanel(ControlDescription cd)
        {
            CPanel cpn = new CPanel();
            return cpn;
        }
    }
}
