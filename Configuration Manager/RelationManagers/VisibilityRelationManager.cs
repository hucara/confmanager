using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class VisibilityRelationManager
    {

        /*  VISIBILITY RELATIONS */

        // *** Visibility ComboBox Management *** //
        // When a Combobox has relation visibility, the one that defines if
        // the control related is visible is the 0 index item. If the item
        // selected is 0, then means that related controls are not visible.
        // If the item selected is >0 then they are visible.
        public void ComboBoxVisibility(object sender, EventArgs e)
        {
            CComboBox control = sender as CComboBox;

            foreach (ICustomControl related in control.cd.RelatedVisibility)
            {
                if (control.SelectedIndex == 0) related.cd.Visible = false;
                else if (control.SelectedIndex > 0) related.cd.Visible = true;
            }
        }

        // *** Visibility CheckBox Management *** //
        // When a Checkbox has a relation visibility, if it is checked,
        // then the related controls, are visible. If not, then they aren't.
        public void CheckBoxVisibility(object sender, EventArgs e)
        {
            CCheckBox control = sender as CCheckBox;

            foreach (ICustomControl related in control.cd.RelatedVisibility)
            {
                if (control.Checked) related.cd.Visible = true;
                else related.cd.Visible = false;
            }
        }

    }
}
