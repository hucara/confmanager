﻿using System;
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
		private ControlFactory cf = ControlFactory.getInstance();
		private Model model = Model.getInstance();

		private static ObjectDefinitionManager odm;
		private XDocument xdoc;

		private TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
		private TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));

		private Util.TokenTextTranslator ttt = Util.TokenTextTranslator.GetInstance();
        private Util.TokenControlTranslator tct = Util.TokenControlTranslator.GetInstance();

		public static ObjectDefinitionManager getInstance()
		{
			if (odm == null)
			{
				odm = new ObjectDefinitionManager();
			}
			return odm;
		}

		public void SetDocument(XDocument xdoc)
		{
			this.xdoc = xdoc;
		}

		public void RestoreOldUI()
		{
			System.Diagnostics.Debug.WriteLine("** Reading Object Definition File **");
			System.Diagnostics.Debug.WriteLine("** Setting up last UI **");

			model.logCreator.Append(" ");
			model.logCreator.AppendCenteredWithFrame(" Reading Object Definition File ");
			model.logCreator.AppendCenteredWithFrame(" Setting up last UI ");
			model.logCreator.Append(" ");

			BuildDefinedSectionList(xdoc);
			CreateDefinedControls(xdoc);

			model.logCreator.Append(" ");
			System.Diagnostics.Debug.WriteLine("** End of Object Definition File **");
			model.logCreator.AppendCenteredWithFrame("End of Object Definition File");
			model.logCreator.Append(" ");
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
					if (model.Sections.Count < Model.getInstance().maxSections)
					{
						Section s = CreateDefinedSection(i);
						model.Sections.Add(s);

						if (s.Selected) model.CurrentSection = s;

						System.Diagnostics.Debug.WriteLine("+ Read: (" + s.Text + ") \t" + s.Name + " {" + s.Button.Name + " , " + s.Tab.Name + "}");
					}
				}
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("! [ERROR] Something went wrong while reading Sections in Object Definition File.");
				model.logCreator.Append("! ERROR: Something went wrong while reading Sections in Object Definition File");
			}
		}

		private Section CreateDefinedSection(XElement i)
		{
			String name = i.Name.ToString() + i.FirstAttribute.Value.ToString();
			String text = i.Element("Text").Value.ToString();
			bool selected = false;

			if (i.Element("Selected").Value.ToString() == "true")
			{
				selected = true;
			}

			return cf.BuildSection(name, text, selected);
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
									new XElement("Text", item.cd.RealText),
									new XElement("Hint", item.cd.Hint.Replace("\r\n","&#13;&#10;")),
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
										new XElement("BackColor", colorConverter.ConvertToString(item.cd.BackColor)),
                                        new XElement("DisplayRight", item.cd.DisplayRight),
                                        new XElement("ModificationRight", item.cd.ModificationRight)
									),

									item.cd.Type == "CComboBox" ?
									new XElement("Items",
										  WriteComboBoxItems(item as CComboBox)) : null,

									item.cd.Type == "CCheckBox" ?
									new XElement("Checked", (item as CheckBox).Checked.ToString()) : null,

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
				doc.Save(Model.getInstance().ObjectDefinitionsPath);
				System.Diagnostics.Debug.WriteLine("*** Object Definition File created ***");
				model.logCreator.AppendCenteredWithFrame(" Object Definition File saved ");
			}
			catch (Exception e)
			{
				String errMsg = "Something went wrong while writing the Object Definition file.\nPlease, try again.";
				MessageBox.Show(errMsg, " Error creating XML file", MessageBoxButtons.OK, MessageBoxIcon.Error);

				System.Diagnostics.Debug.WriteLine("[ERROR] Something went wrong when creating Object Definition File.");
				model.logCreator.Append("! ERROR: Something went wrong when creating Object Definition File.");
				System.Diagnostics.Debug.WriteLine(e);
			}
		}

		private IEnumerable<XElement> WriteComboBoxItems(CComboBox cb)
		{
			if (cb.SelectedIndex > -1)
			{
				yield return new XElement("Selected", cb.cd.ComboBoxRealItems[cb.SelectedIndex]);
			}
			foreach (String s in cb.cd.ComboBoxRealItems)
			{
				yield return new XElement("Item", s);
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

			// Create a "preview" of the controls. 
			// Everyone is assigned a section as its parent.
			foreach (var i in items)
			{
				if (i.Attribute("type").Value == "CTabPage") CTabs.Add(i);
				else
				{
					Section s = model.Sections.Find(se => se.Name == i.Element("Section").Value);
					CreatePreviewControls(s, i);
				}
			}

			// CTabControls are created and in place.
			// Now it's time to set the Custom Tabs.
			if (CTabs.Count > 0)
			{
				foreach (XElement e in CTabs)
				{
					CTabControl parentControl = model.AllControls.Find(p => p.cd.Name == e.Element("Parent").Value) as CTabControl;

					CTabPage ctp = cf.BuildCTabPage(parentControl);
					ctp.cd.Name = e.Element("Name").Value;
					ctp.cd.RealText = e.Element("Text").Value;
				}
			}

			// Fill out the controls with the info from ObjectDefinition.xml
			foreach (var i in items)
			{
				foreach (ICustomControl c in model.AllControls)
				{
					if (c.cd.Name == i.Element("Name").Value)
					{
						SetRealParent(c, i as XContainer);

						SetRealProperties(c, i);
						SetPaths(c, i);
						SetControlSpecificProperties(c, i);

						SetRelatedReadList(c, i);
						SetRelatedWriteList(c, i);
						SetRelatedVisibility(c, i);
						SetCoupledControls(c, i);

                        ReadValuesFromDestination(c);

						System.Diagnostics.Debug.WriteLine("+ Added : " + c.cd.Name + " with parent: " + c.cd.Parent.Name + " in Section: " + c.cd.ParentSection.Name);
					}
				}
			}
		}

		private void SetControlSpecificProperties(ICustomControl c, XElement i)
		{
			if (c.cd.Type == "CComboBox" && c.cd.SubDestination == "")
			{
				// Set items of the ComboBox
				ComboBox cb = c as ComboBox;
				foreach (XElement e in i.Element("Items").Descendants("Item"))
				{
                    String value = e.Value.ToString();
                    
                    c.cd.ComboBoxRealItems.Add(value);

                    value = ttt.TranslateFromTextFile(value);
                    value = tct.TranslateFromControl(value);
            
					cb.Items.Add(value);
				}

				try
				{
					if (!i.Element("Items").Element("Selected").IsEmpty)
					{
                        String value = ttt.TranslateFromTextFile(i.Element("Items").Element("Selected").Value);
                        value = tct.TranslateFromControl(value);

						cb.SelectedItem = value;
					}
				}
				catch (NullReferenceException e)
				{
				}
			}
			else if (c.cd.Type == "CCheckBox")
			{
				CheckBox cb = c as CheckBox;
				try
				{
					if (i.Element("Checked").Value == "True") cb.Checked = true;
					else cb.Checked = false;
				}
				catch (NullReferenceException e)
				{
				}
			}
		}

		private void SetPaths(ICustomControl c, XElement i)
		{
			c.cd.DestinationType = i.Element("Paths").Element("DestinationType").Value;
			c.cd.MainDestination = i.Element("Paths").Element("DestinationFile").Value;
			c.cd.RealSubDestination = i.Element("Paths").Element("SubDestination").Value;
            c.cd.SubDestination = i.Element("Paths").Element("SubDestination").Value;
		}

		private void CreatePreviewControls(Section s, XElement i)
		{
			switch (i.Attribute("type").Value)
			{
				case "CLabel":
					CLabel lbl = cf.BuildCLabel(s.Tab);
					lbl.cd.Name = i.Element("Name").Value;
					break;

				case "CComboBox":
					CComboBox cb = cf.BuildCComboBox(s.Tab);
					cb.cd.Name = i.Element("Name").Value;
					break;

				case "CGroupBox":
					CGroupBox gb = cf.BuildCGroupBox(s.Tab);
					gb.cd.Name = i.Element("Name").Value;
					break;

				case "CPanel":
					CPanel pl = cf.BuildCPanel(s.Tab);
					pl.cd.Name = i.Element("Name").Value;
					break;

				case "CTextBox":
					CTextBox tb = cf.BuildCTextBox(s.Tab);
					tb.cd.Name = i.Element("Name").Value;
					break;

				case "CCheckBox":
					CCheckBox ccb = cf.BuildCCheckBox(s.Tab);
					ccb.cd.Name = i.Element("Name").Value;
					break;

				case "CTabControl":
					CTabControl ctc = cf.BuildCTabControl(s.Tab);
					ctc.cd.Name = i.Element("Name").Value;
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

			if (i.Element("Parent").Value.Contains("Section"))
			{
				c.cd.Parent = model.Sections.Find(s => s.Name == i.Element("Parent").Value).Tab;
			}
			else
			{
				String definedParent = i.Element("Parent").Value;
				String definedName = i.Element("Name").Value;

				foreach (ICustomControl p in Model.getInstance().AllControls)
				{
					if (p.cd.Name == definedParent)
					{
						c.cd.Parent = p as Control;
						System.Diagnostics.Debug.WriteLine("\n! Building: " +c.cd.Name+" with Parent: "+p.cd.Name);
					}
				}
			}
		}

		private void SetRealProperties(ICustomControl c, XElement i)
		{
			Font newFont;
			Color newColor;

			c.cd.RealText = i.Element("Text").Value;

            String text = ttt.TranslateFromTextFile(c.cd.RealText);
            c.cd.Text = tct.TranslateFromControl(text);

			c.cd.Hint = i.Element("Hint").Value;
			c.cd.Hint = c.cd.Hint.Replace("&#13;&#10;", "\r\n");

			c.cd.ParentSection = Model.getInstance().Sections.Find(se => se.Name == i.Element("Section").Value);

			c.cd.Top = Convert.ToInt32(i.Element("Settings").Element("Top").Value);
			c.cd.Left = Convert.ToInt32(i.Element("Settings").Element("Left").Value);
			c.cd.Width = Convert.ToInt32(i.Element("Settings").Element("Width").Value);
			c.cd.Height = Convert.ToInt32(i.Element("Settings").Element("Height").Value);

			newFont = (Font)fontConverter.ConvertFromString(i.Element("Settings").Element("Font").Value);
			c.cd.CurrentFont = newFont;

			colorConverter = TypeDescriptor.GetConverter(typeof(Color));
			newColor = (Color)colorConverter.ConvertFromString(i.Element("Settings").Element("FontColor").Value);
			c.cd.ForeColor = newColor;
			newColor = (Color)colorConverter.ConvertFromString(i.Element("Settings").Element("BackColor").Value);
			c.cd.BackColor = newColor;

            c.cd.DisplayRight = i.Element("Settings").Element("DisplayRight").Value;
            c.cd.ModificationRight = i.Element("Settings").Element("ModificationRight").Value;

            c.cd.userVisibility = model.ObtainLogicAnd(c.cd.DisplayRight, model.DisplayRights);
            c.cd.userModification = model.ObtainLogicAnd(c.cd.ModificationRight, model.ModificatioRights);
		}

		private void SetRelatedReadList(ICustomControl c, XElement i)
		{
			string s = i.Element("Relations").Element("Read").Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.RelatedRead.Add(model.AllControls.Find(p => p.cd.Name == r));
			}
		}

		private void SetCoupledControls(ICustomControl c, XElement i)
		{
			string s = i.Element("Relations").Element("Coupled").Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.CoupledControls.Add(model.AllControls.Find(p => p.cd.Name == r));
			}
		}

		private void SetRelatedVisibility(ICustomControl c, XElement i)
		{
			string s = i.Element("Relations").Element("Visibility").Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.RelatedVisibility.Add(model.AllControls.Find(p => p.cd.Name == r));
			}
		}

		private void SetRelatedWriteList(ICustomControl c, XElement i)
		{
			string s = i.Element("Relations").Element("Write").Value;
			string[] f = { ", " };

			List<String> rel = s.Split(f, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (String r in rel)
			{
				c.cd.RelatedWrite.Add(model.AllControls.Find(p => p.cd.Name == r));
			}
		}

        private void ReadValuesFromDestination(ICustomControl c)
        {
            String type = GetFileType(c.cd.MainDestination);

            String path = ttt.TranslateFromTextFile(c.cd.SubDestination);
            path = tct.TranslateFromControl(path);

            List<String> nodes = path.Split('\\').ToList();

            if (type == "ini")
            {
                Util.IniFile file = new Util.IniFile(c.cd.MainDestination);

                if (c.cd.Type == "CComboBox")
                {
                    String item = file.IniReadValue(nodes[0], nodes[1]);
                    (c as ComboBox).Items.Add(item);

                    if (c.cd.ComboBoxRealItems.Count != 0) (c as ComboBox).SelectedItem = c.cd.ComboBoxRealItems[0];
                    else (c as ComboBox).SelectedIndex = 0;
                }
                else
                {
                    c.cd.Text = file.IniReadValue(nodes[0], nodes[1]);
                }
            }
        }

        private void FillComboBoxFromFile(CComboBox c)
        {
            
        }

        private String GetFileType(string p)
        {
            if (p != "" && p != null) return p.Remove(0, p.Length - 3);
            else return "";
        }
	}
}
