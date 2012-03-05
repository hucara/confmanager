using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Configuration_Manager
{
	class ObjectDefinitionManager
	{
		private static ObjectDefinitionManager odr;
		private XDocument xdoc;

		private TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
		private TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));

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

		public void RestoreOldUI()
		{
			System.Diagnostics.Debug.WriteLine("** Reading Object Definition File **");
			BuildDefinedSectionList(xdoc);
			CreateDefinedControls(xdoc);
			System.Diagnostics.Debug.WriteLine("** End of Object Definition File **");
		}

		// Reads the sections defined inside the ObjectDefinition file.
		// Then, creates and adds those sections to the Sections list.
		private void BuildDefinedSectionList(XDocument xdoc)
		{
			if (xdoc == null) throw new ArgumentNullException();

			try
			{
				var items = from item in xdoc.Descendants("Sections")
							.Descendants("Section")
							select item;

				foreach (var i in items)
				{
					if (Model.getInstance().Sections.Count < Model.MAX_SECTIONS)
					{
						Section s = CreateDefinedSection(i);
						Model.getInstance().Sections.Add(s);

						if (s.Selected) Model.getInstance().CurrentSection = s;

						System.Diagnostics.Debug.WriteLine("+ Read: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
					}
				}
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("! [ERROR] Something went wrong while reading Sections in Object Definition File.");
			}
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
			System.Windows.Forms.TabPage ctp = new System.Windows.Forms.TabPage();
			ctp.Name = name;

			if (selected) ctsb.PerformClick();
			return new Section(ctsb, ctp, text, selected);
		}

		public void SerializeObjectDefinition()
		{
			try
			{
				XDocument doc = new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					new XComment(""),
					new XElement("ObjectDefinition",
						new XElement("Sections",
								Model.getInstance().Sections.Select(item =>
								new XElement("Section",
									new XAttribute("id", item.Id),
									new XElement("Name", item.Name),
									new XElement("Selected", item.Selected),
									new XElement("Text", item.Text)
								)   //end Section
								)
						),       //end Sections
						new XElement("Controls",
								Model.getInstance().AllControls.Select(item =>
								new XElement("Control",
									new XAttribute("id", item.cd.Id),
									new XAttribute("type", item.cd.Type),
									new XElement("Name", item.cd.Name),
									new XElement("Text", item.cd.Text),
									new XElement("Hint", item.cd.Hint),
									new XElement("Parent", item.cd.Parent.Name),
									new XElement("Section", item.cd.ParentSection.Name),
									new XElement("Settings",
										new XElement("Top", item.cd.Top),
										new XElement("Left", item.cd.Left),
										new XElement("Width", item.cd.Width),
										new XElement("Height", item.cd.Height),
										new XElement("Visible", item.cd.Visible),
										new XElement("Font", fontConverter.ConvertToString(item.cd.CurrentFont)),
										new XElement("FontColor", colorConverter.ConvertToString(item.cd.ForeColor)),
										new XElement("BackColor", colorConverter.ConvertToString(item.cd.BackColor))
									),
									new XElement("Paths",
										new XElement("DestinationType", item.cd.DestinationType),
										new XElement("DestinationFile", item.cd.MainDestination),
										new XElement("SubDestination", item.cd.SubDestination)
									),
									new XElement("Relations",
										new XElement("Write",
											item.cd.RelatedWrite.Select(write => write.cd.Name + ", ")
										),
										new XElement("Read",
											item.cd.RelatedRead.Select(read => read.cd.Name + ", ")
										),
										new XElement("Visibility",
											item.cd.RelatedVisibility.Select(view => view.cd.Name + ", ")
										),
										new XElement("Coupled",
											item.cd.CoupledControls.Select(coupled => coupled.cd.Name + ", ")
										)
									)
								)
							)
							)
							)
						);

				System.Diagnostics.Debug.WriteLine("XML: " + doc.ToString());
				doc.Save(Resources.getInstance().ConfigFolderPath + "\\testing.xml");
				System.Diagnostics.Debug.WriteLine("*** Testing Object Definition File created ***");
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("[ERROR] Something went wrong when creating Object Definition File.");
				System.Diagnostics.Debug.WriteLine(e);
			}
		}

		private void CreateDefinedControls(XDocument xdoc)
		{
			List<XContainer> CTabs = new List<XContainer>();
			List<XContainer> CTabControls = new List<XContainer>();

			this.xdoc = xdoc;
			var items = from item in xdoc.Descendants("Controls")
							.Descendants("Control")
						select item;

			// Create preview controls. Everyone with its section as a parent
			foreach (var i in items)
			{
				if (i.Attribute("type").Value == "CTabPage") CTabs.Add(i);
				else
				{
					Section s = Model.getInstance().Sections.Find(se => se.Name == i.Descendants("Section").FirstOrDefault().Value);
					CreatePreviewControls(s, i);
				}
			}

			// CTabControls are created and in place.
			// Now it's time to set the Custom Tabs.
			if (CTabs.Count > 0)
			{
				foreach (XElement e in CTabs)
				{
					CTabControl parentControl = Model.getInstance().AllControls.Find(p => p.cd.Name == e.Descendants("Parent").FirstOrDefault().Value) as CTabControl;

					CTabPage ctp = ControlFactory.getInstance().BuildCTabPage(parentControl);
					ctp.cd.Name = e.Descendants("Name").FirstOrDefault().Value;
					ctp.cd.Text = e.Descendants("Text").FirstOrDefault().Value;
				}
			}

			// Now it is time to fill out the controls with the info from ObjectDefinition.xml
			foreach (var i in items)
			{
				foreach (ICustomControl c in Model.getInstance().AllControls)
				{
					if (c.cd.Name == i.Descendants("Name").FirstOrDefault().Value)
					{
						
						SetRealParent(c, i as XContainer);
						
						SetRealProperties(c, i);
						SetPaths(c, i);

						SetRelatedReadList(c, i);
						SetRelatedWriteList(c, i);
						SetRelatedVisibility(c, i);
						SetCoupledControls(c, i);

						System.Diagnostics.Debug.WriteLine("! Building : " + c.cd.Name + " with parent: " + c.cd.Parent.Name + " in Section: " + c.cd.ParentSection.Name);
					}
				}
			}
		}

		private void SetPaths(ICustomControl c, XElement i)
		{
			c.cd.DestinationType = i.Descendants("Paths").Descendants("DestinationType").FirstOrDefault().Value;
			c.cd.MainDestination = i.Descendants("Paths").Descendants("DestinationFile").FirstOrDefault().Value;
			c.cd.SubDestination = i.Descendants("Paths").Descendants("SubDestination").FirstOrDefault().Value;
		}

		private void CreatePreviewControls(Section s, XElement i)
		{
			switch (i.Attribute("type").Value)
			{
				case "CLabel":
					CLabel lbl = ControlFactory.getInstance().BuildCLabel(s.Tab);
					lbl.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CComboBox":
					CComboBox cb = ControlFactory.getInstance().BuildCComboBox(s.Tab);
					cb.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CGroupBox":
					CGroupBox gb = ControlFactory.getInstance().BuildCGroupBox(s.Tab);
					gb.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CPanel":
					CPanel pl = ControlFactory.getInstance().BuildCPanel(s.Tab);
					pl.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CTextBox":
					CTextBox tb = ControlFactory.getInstance().BuildCTextBox(s.Tab);
					tb.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CCheckBox":
					CCheckBox ccb = ControlFactory.getInstance().BuildCCheckBox(s.Tab);
					ccb.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					break;

				case "CTabControl":
					CTabControl ctc = ControlFactory.getInstance().BuildCTabControl(s.Tab);
					ctc.cd.Name = i.Descendants("Name").FirstOrDefault().Value;
					ctc.TabPages.Clear();
					break;

				case "CTabPage":
					//Tab pages require its parent TabControl to be created first.
					break;
			}
		}

		private void SetRealParent(ICustomControl c, XContainer i)
		{
			if (c == null) throw new ArgumentNullException();

			if (i.Descendants("Parent").FirstOrDefault().Value.Contains("Section"))
			{
				c.cd.Parent = Model.getInstance().Sections.Find(s => s.Name == i.Descendants("Parent").FirstOrDefault().Value).Tab;
			}
			else
			{
				String definedParent = i.Descendants("Parent").FirstOrDefault().Value;
				String definedName = i.Descendants("Name").FirstOrDefault().Value;

				System.Diagnostics.Debug.WriteLine("Defined parent: " + definedParent);

				foreach (ICustomControl p in Model.getInstance().AllControls)
				{
					if (p.cd.Name == definedParent)
					{
						c.cd.Parent = p as Control;
					}
				}
			}
		}

		private void SetRealProperties(ICustomControl c, XElement i)
		{
			c.cd.Text = i.Descendants("Text").FirstOrDefault().Value;
			c.cd.ParentSection = Model.getInstance().Sections.Find(se => se.Name == i.Descendants("Section").FirstOrDefault().Value);

			c.cd.Top = Convert.ToInt32(i.Descendants("Top").FirstOrDefault().Value);
			c.cd.Left = Convert.ToInt32(i.Descendants("Left").FirstOrDefault().Value);
			c.cd.Width = Convert.ToInt32(i.Descendants("Width").FirstOrDefault().Value);
			c.cd.Height = Convert.ToInt32(i.Descendants("Height").FirstOrDefault().Value);

			Font newFont = (Font)fontConverter.ConvertFromString(i.Descendants("Font").FirstOrDefault().Value);
			c.cd.CurrentFont = newFont;

			colorConverter = TypeDescriptor.GetConverter(typeof(Color));

			Color newColor = (Color)colorConverter.ConvertFromString(i.Descendants("FontColor").FirstOrDefault().Value);
			c.cd.ForeColor = newColor;

			newColor = (Color)colorConverter.ConvertFromString(i.Descendants("BackColor").FirstOrDefault().Value);
			c.cd.BackColor = newColor;
		}

		private void SetRelatedReadList(ICustomControl c, XElement i)
		{
			string s = i.Descendants("Relations").Descendants("Read").FirstOrDefault().Value;
			string[] f = {", "};

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();
			
			foreach (String r in rel)
			{
				c.cd.RelatedRead.Add(Model.getInstance().AllControls.Find(p => p.cd.Name == r));
			}

		}

		private void SetCoupledControls(ICustomControl c, XElement i)
		{
			string s = i.Descendants("Relations").Descendants("Coupled").FirstOrDefault().Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.CoupledControls.Add(Model.getInstance().AllControls.Find(p => p.cd.Name == r));
			}
		}

		private void SetRelatedVisibility(ICustomControl c, XElement i)
		{
			string s = i.Descendants("Relations").Descendants("Visibility").FirstOrDefault().Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.RelatedVisibility.Add(Model.getInstance().AllControls.Find(p => p.cd.Name == r));
			}
		}

		private void SetRelatedWriteList(ICustomControl c, XElement i)
		{
			string s = i.Descendants("Relations").Descendants("Write").FirstOrDefault().Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.RelatedWrite.Add(Model.getInstance().AllControls.Find(p => p.cd.Name == r));
			}
		}
	}
}
