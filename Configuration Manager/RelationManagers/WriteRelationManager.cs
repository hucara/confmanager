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

        public void SaveConfiguration()
        {
            List<ICustomControl> ContainRelatedWrite =  Model.getInstance().AllControls.Where(c => c.cd.RelatedWrite.Count > 0).ToList();
            Util.TokenControlTranslator tct = Util.TokenControlTranslator.GetInstance();

            foreach (ICustomControl control in ContainRelatedWrite)
            {
                Dictionary<string, string> dic = tct.GetValueTranslatedPairs(control.cd.RealSubDestination);

                foreach (string name in dic.Keys.ToList())
                {
                    int startInd = control.cd.RealSubDestination.IndexOf(name) - 2;
                    int endInd = startInd + 4 + name.Length;

                    String key = Model.getInstance().controlToken + name +Model.getInstance().controlToken;
                    String value = "";

                    dic.TryGetValue(key, out value);

                    System.Diagnostics.Debug.WriteLine("SubDestination replaced for control: " + control.cd.Name);
                    System.Diagnostics.Debug.WriteLine("\t - SubDestination = " +control.cd.RealSubDestination);

                    control.cd.RealSubDestination.Replace(key, value);

                    System.Diagnostics.Debug.WriteLine("\t - Replace = " + control.cd.SubDestination);


                    //TODO THIS IS THE END
                }
            }
        }
    }
}
