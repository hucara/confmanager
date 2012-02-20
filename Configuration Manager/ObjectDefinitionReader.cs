using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration_Manager
{
    class ObjectDefinitionReader
    {
        private static ObjectDefinitionReader odr;
        private XDocument xdoc;

        public static ObjectDefinitionReader getInstance()
        {
            if (odr == null)
            {
                odr = new ObjectDefinitionReader();
            }
            return odr;
        }

        public void SetDocument(XDocument xdoc)
        {
            this.xdoc = xdoc;
        }

        // Reads the sections defined inside the ObjectDefinition file.
        // Then, creates and adds those sections to the Sections list.
        public void BuildDefinedSectionList(XDocument xdoc)
        {
            if (xdoc == null) throw new ArgumentNullException();

            var items = from item in xdoc.Descendants("Sections")
                        .Descendants("Objects")
                        .Descendants("Section")
                        select item;

            System.Diagnostics.Debug.WriteLine("** Reading Object Definition File **");
            foreach (var i in items)
            {
                if (Model.getInstance().Sections.Count < Model.MAX_SECTIONS)
                {
                    Section s = CreateDefinedSection(i);
                    Model.getInstance().Sections.Add(s);

                    System.Diagnostics.Debug.WriteLine("! Read: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name+"}");
                }

            }
            System.Diagnostics.Debug.WriteLine("** End of Object Definition File **");
        }


        private Section CreateDefinedSection(XElement i)
        {
            String name = i.Name.ToString() + i.FirstAttribute.Value.ToString();
            String text = i.Descendants("Text").FirstOrDefault().Value.ToString();
            bool selected = false;

            if (i.Descendants("Selected").FirstOrDefault().Value.ToString() == "1") selected = true;

            CustomControls.CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(text);
            CustomControls.CTabPage ctp = ControlFactory.getInstance().BuildCTabPage();

            return new Section(ctsb, ctp, text, selected);
        }


    }
}
