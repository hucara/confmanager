using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    static class VisibilityRelationManager
    {

        /*  VISIBILITY RELATIONS */

        // *** Visibility ComboBox Management *** //
        // When a Combobox has relation visibility, the one that defines if
        // the control related is visible is the 0 index item. If the item
        // selected is 0, then means that related controls are not visible.
        // If the item selected is >0 then they are visible.
        public static void ComboBoxVisibility(object sender, EventArgs e)
        {
            CComboBox control = sender as CComboBox;
            //SetComboboxVisibility(control);

            foreach (ICustomControl related in control.cd.RelatedVisibility)
            {
                if (control.SelectedIndex > 0 || Model.getInstance().progMode)
                {
                    related.cd.Visible = true;
                }
                else if (control.SelectedIndex <= 0)
                {
                    if (related is CCheckBox || related is CComboBox)
                        SetDependentVisibility(related);
                    related.cd.Visible = false;
                }

                if (!related.cd.operatorVisibility && !Model.getInstance().progMode)
                    related.cd.Visible = false;
            }
        }

        // *** Visibility CheckBox Management *** //
        // When a Checkbox has a relation visibility, if it is checked,
        // then the related controls, are visible. If not, then they aren't.
        public static void CheckBoxVisibility(object sender, EventArgs e)
        {
            CCheckBox control = sender as CCheckBox;

            foreach (ICustomControl related in control.cd.RelatedVisibility)
            {
                if (control.Checked || Model.getInstance().progMode)
                    related.cd.Visible = true;
                else
                {
                    if (related is CCheckBox || related is CComboBox)
                        SetDependentVisibility(related);
                    related.cd.Visible = false;
                }

                if (!related.cd.operatorVisibility && !Model.getInstance().progMode)
                    related.cd.Visible = false;
            }
        }

        private static void SetDependentVisibility(ICustomControl r)
        {
            if (r is CCheckBox)
            {
                (r as CCheckBox).CheckState = System.Windows.Forms.CheckState.Unchecked;
                ReadRelationManager.ReadConfiguration(r);
            }
            if (r is CComboBox)
                //(r as CComboBox).SelectedIndex = -1;
                ReadRelationManager.ReadConfiguration(r);
            else
                r.cd.Visible = false;

            foreach (ICustomControl c in r.cd.RelatedVisibility)
                SetDependentVisibility(c);
        }
    }
}
