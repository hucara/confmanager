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

        public CLabel BuildCLabel(Control parent)
        {
            CLabel lbl = new CLabel();
            parent.Controls.Add(lbl);

            Model.getInstance().AllControls.Add(lbl);
            lbl.SetControlDescription();

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

            Model.getInstance().AllControls.Add(ctp);
            ctp.SetControlDescription();

            return ctp;
        }

        public CTabPage BuildCTabPage(Control parent)
        {
            CTabPage ctp = new CTabPage();
            parent.Controls.Add(ctp);

            Model.getInstance().AllControls.Add(ctp);
            ctp.SetControlDescription();
            ctp.Parent = parent;

            return ctp;
        }

        public CTabControl BuildCTabControl(Control parent)
        {
            CTabControl ctc = new CTabControl(BuildCTabPage());
            parent.Controls.Add(ctc);

            ctc.SetControlDescription();
            Model.getInstance().AllControls.Add(ctc);

            return ctc;
        }

        public CComboBox BuildCComboBox(Control parent)
        {
            CComboBox ccb = new CComboBox();
            parent.Controls.Add(ccb);

            Model.getInstance().AllControls.Add(ccb);
            ccb.SetControlDescription();

            return ccb;
        }

        public CTextBox BuildCTextBox(Control parent)
        {
            CTextBox ctb = new CTextBox();
            parent.Controls.Add(ctb);

            Model.getInstance().AllControls.Add(ctb);
            ctb.SetControlDescription();

            return ctb;
        }

        public CCheckBox BuildCCheckBox(Control parent)
        {
            CCheckBox ccb = new CCheckBox();
            parent.Controls.Add(ccb);

            Model.getInstance().AllControls.Add(ccb);
            ccb.SetControlDescription();

            return ccb;
        }

        public CGroupBox BuildCGroupBox(Control parent)
        {
            CGroupBox cgb = new CGroupBox();
            parent.Controls.Add(cgb);

            Model.getInstance().AllControls.Add(cgb);
            cgb.SetControlDescription();

            return cgb;
        }

        public CPanel BuildCPanel(Control parent)
        {
            CPanel cpn = new CPanel();
            parent.Controls.Add(cpn);

            Model.getInstance().AllControls.Add(cpn);
            cpn.SetControlDescription();

            return cpn;
        }
    }
}
