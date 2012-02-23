using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration_Manager
{
    class ObjectDefinitionManager
    {
        private static ObjectDefinitionManager odr;
        private XDocument xdoc;

        public static ObjectDefinitionManager getInstance()
        {
            if (odr == null)
            {
                odr = new ObjectDefinitionManager();
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

                    if (s.Selected) Model.getInstance().CurrentSection = s;

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

            if (i.Descendants("Selected").FirstOrDefault().Value.ToString() == "true")
            {
                selected = true;
            }

            CustomControls.CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(text);
            //CTabPage ctp = ControlFactory.getInstance().BuildCTabPage();
            System.Windows.Forms.TabPage ctp = new System.Windows.Forms.TabPage();
            ctp.Name = name;


            return new Section(ctsb, ctp, text, selected);
        }

        public void SerializeObjectDefinition()
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment(""),
                new XElement("ObjectDefinition",
                    new XElement("Sections",
                        new XElement("Objects",
                            Model.getInstance().Sections.Select(item => new XElement("Section",
                                new XAttribute("id", item.Id),
                                new XElement("Selected", item.Selected),
                                new XElement("Text", item.Text),
                                new XElement("IdRelatedTab", item.RelatedTabIndex))
                            )
                        )
                    ),
                    new XElement("Controls",
                            Model.getInstance().AllControls.Select(item => new XElement("Control",
                                new XAttribute("id", item.cd.Id),
                                new XAttribute("type", item.cd.Type),
                                new XElement("Name", item.cd.Name),
                                new XElement("Text", item.cd.Text),
                                new XElement("Hint", item.cd.Hint),
                                new XElement("Parent", item.cd.Parent.Name),
                                new XElement("Section", item.cd.ParentSection.Text),
                                new XElement("Settings",
                                    new XElement("Top", item.cd.Top),
                                    new XElement("Left", item.cd.Left),
                                    new XElement("Width", item.cd.Width),
                                    new XElement("Height", item.cd.Height),
                                    new XElement("Visible", item.cd.Visible),
                                    new XElement("Font", item.cd.CurrentFont),
                                    new XElement("FontColor", item.cd.ForeColor),
                                    new XElement("Color", item.cd.BackColor)
                                ),
                                new XElement("Paths",
                                    new XElement("DestinationType", item.cd.DestinationType),
                                    new XElement("DestinationFile", item.cd.MainDestination),
                                    new XElement("SubDestination", item.cd.SubDestination)
                                ),
                                new XElement("Relations",
                                    new XElement("Write",
                                        item.cd.RelatedWrite.Select(write => write.cd.Name + ", ")),
                                    new XElement("Read",
                                        item.cd.RelatedRead.Select(read => read.cd.Name + ", ")),
                                    new XElement("Visibility",
                                        item.cd.RelatedVisibility.Select(view => view.cd.Name + ", ")),
                                    new XElement("Coupled",
                                        item.cd.CoupledControls.Select(coupled => coupled.cd.Name + ", "))
                                )
                            )
                        )
                    )
                )
            );

            doc.Save(Resources.getInstance().ConfigFolderPath + "\\testing.xml");
            System.Diagnostics.Debug.WriteLine("*** Testing Object Definition File created ***");
        }
    }
}
