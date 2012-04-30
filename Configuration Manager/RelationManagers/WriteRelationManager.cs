using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration_Manager.CustomControls
{
    class WriteRelationManager
    {

        /*   WRITE RELATIONS   */

        // *** Write CheckBox Management *** //
        public void CheckBoxWrite(object sender, EventArgs e)
        {
            CCheckBox control = sender as CCheckBox;

            foreach (ICustomControl related in control.cd.RelatedWrite)
            {
                //Modify the subdestination
            }
        }

        public void ComboBoxWrite(object sender, EventArgs e)
        {
            CComboBox control = sender as CComboBox;

            foreach (ICustomControl related in control.cd.RelatedWrite)
            {
                //Modify the subdestination
            }
        }

        public void GroupBoxWrite(object sender, EventArgs e)
        {
            CGroupBox control = sender as CGroupBox;

            foreach (ICustomControl related in control.cd.RelatedWrite)
            {
                //Modify the subdestination
            }
        }

        public void TextBoxWrite(object sender, EventArgs e)
        {
            CTextBox control = sender as CTextBox;

            foreach (ICustomControl related in control.cd.RelatedWrite)
            {
                //Modify the subdestination
            }
        }

        // For Each control with Write Relations, replaces the tokens in the Subdestination path
        // and sets the real path according to the value of that token.
        public void TranslateSubDestiantions()
        {
            List<ICustomControl> ContainRelatedWrite =  Model.getInstance().AllControls.Where(c => c.cd.RelatedWrite.Count > 0).ToList();
            Util.TokenControlTranslator tct = Util.TokenControlTranslator.GetInstance();

            foreach (ICustomControl control in ContainRelatedWrite)
            {
                Dictionary<string, string> dic = tct.GetValueTranslatedPairs(control.cd.SubDestination);

                String destination = "";
                destination = control.cd.SubDestination;

                foreach (string name in dic.Keys.ToList())
                {
                    String key = name;
                    String value = "";

                    if (dic.TryGetValue(key, out value))
                    {
                        key = Model.getInstance().controlToken + key + Model.getInstance().controlToken;
                        control.cd.RealSubDestination = destination.Replace(key, value);

                        destination = control.cd.RealSubDestination;
                    }
                }

                System.Diagnostics.Debug.WriteLine("SubDestination replaced for control: " + control.cd.Name);
                System.Diagnostics.Debug.WriteLine("\t - SubDestination = " + control.cd.SubDestination);
                System.Diagnostics.Debug.WriteLine("\t - Replaced = " + control.cd.RealSubDestination);
            }
        }
    }
}
