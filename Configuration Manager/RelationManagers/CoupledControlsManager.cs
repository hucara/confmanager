using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    static class CoupledControlsManager
    {
        //  *** Coupled ComboBox Management ***
        //  When a combo box is coupled with another one, the
        //  index of those must change at the same time to the same value.
        public static void ComboBoxCoupled(object sender, EventArgs e)
        {
            CComboBox control = sender as CComboBox;

            foreach (CComboBox related in control.cd.CoupledControls.Where(r => r.cd.Type == "CComboBox"))
            {
                if (related.Items.Count == control.Items.Count)
                    (related as CComboBox).SelectedIndex = control.SelectedIndex;
            }
        }

        //  *** Coupled CheckBox Management ***
        //  When a CheckBox is coupled with another one, the
        //  state of those must change at the same time to the same value.
        public static void CheckBoxCoupled(object sender, EventArgs e)
        {
            CCheckBox control = sender as CCheckBox;

            foreach (CCheckBox related in control.cd.CoupledControls.Where(r => r.cd.Type == "CCheckBox"))
            {
                related.CheckState = control.CheckState;

                if (control.Checked) related.Enabled = false;
                else related.Enabled = true;
            }
        }
    }
}