using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager.Views
{
    class ToolStripView : IView
    {
        int CurrentButtonIndex = 0;
        int CurrentTabIndex = 0;

        Dictionary<CToolStripButton, CTabPage> ButtonTabDict;

        Model model;
        ToolStrip navigationBar;
        TabControl configurationTabs;

        CToolStripButton ctsb;
        CTabPage ctp;

        XDocument xdoc;
        ControlFactory cf;

        CToolStripButton CurrentButton { get; set; }
        CTabPage CurrentTab { get; set; }

        List<CTabPage> CTabPages;
        List<CToolStripButton> CToolStripButtons;

        public ToolStripView(ToolStrip ts, TabControl tc, Model model)
        {
            CTabPages = new List<CTabPage>();
            CToolStripButtons = new List<CToolStripButton>();
            ButtonTabDict = new Dictionary<CToolStripButton, CTabPage>();

            this.model = model;
            this.navigationBar = ts;
            this.configurationTabs = tc;
            this.xdoc = Resources.getInstance().ConfigObjects;
            this.cf = ControlFactory.getInstance();
        }

        // Takes the info from the UI, gets the changes made by the 
        // user and stores them inside the model  / views.
        public void saveToModel()
        {
        }

        // Reads the info from the model / views and fills-out the
        // UI with that info, refreshing the components.
        public void readAndShow()
        {
        }

        public void AddNewSection(String name)
        {
            CreateToolStripButton();
            CreateTabPage();
        }

        public void RemoveSection(String name)
        {
            RemoveToolStripButton();
            RemoveTabPage();
        }

        private void RemoveTabPage()
        {
        }

        public void ReadObjectDefinitionFile()
        {
            var items = from item in xdoc.Descendants("NavigationBar")
                        .Descendants("Objects")
                        .Descendants("ToolStripButton")
                        select item;
 

            System.Diagnostics.Debug.WriteLine("** Reading Object Definition File **");
        }

        public void AddNewToolStripButton()
        {
            ButtonTabDict.Add(CreateToolStripButton(), CreateTabPage());
            ctsb.Click += ToolStripButton_Click;
        }

        // Reads the objects contained inside the ObjectDefinition file.
        // Taking care only of the ToolStripButton ones.
        public void ReadObjectDefinitionFile()
        {
            var items = from item in xdoc.Descendants("NavigationBar")
                        .Descendants("Objects")
                        .Descendants("ToolStripButton")
                        select item;

            System.Diagnostics.Debug.WriteLine("** Reading Object Definition File **");

            foreach (var i in items)
            {
                CreateToolStripButton();
                CreateTabPage();
                ctsb.Click += ToolStripButton_Click;
            }

            System.Diagnostics.Debug.WriteLine("** End of Object Definition File **");
        }

        private CToolStripButton CreateToolStripButton()
        {
            CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(null);
            CurrentButtonIndex = navigationBar.Items.Add(ctsb);

            System.Diagnostics.Debug.WriteLine("+ Added: " + ctsb.Name);

            return ctsb;
        }

        private CTabPage CreateTabPage()
        {
            CTabPage ctp = ControlFactory.getInstance().BuildCTabPage(null);
            configurationTabs.TabPages.Add(ctp);
            CurrentTabIndex = configurationTabs.TabPages.IndexOf(ctp);

            System.Diagnostics.Debug.WriteLine("+ Added: " + ctp.Name);

            return ctp;
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                CToolStripButton b = (CToolStripButton)sender;

                System.Diagnostics.Debug.WriteLine("! Clicked: " + b.Name + " ID: " + b.RelatedTabPageIndex);
                configurationTabs.SelectTab(b.TypeId);
                System.Diagnostics.Debug.WriteLine("! Selected Tab: " + configurationTabs.SelectedTab.Name);
            }
        }

        public void RemoveToolStripButton()
        {
            navigationBar.Items.RemoveAt(CurrentButtonIndex);
            configurationTabs.TabPages.RemoveAt(CurrentTabIndex);

            System.Diagnostics.Debug.WriteLine("- Removed: " + ctsb.Name);
            System.Diagnostics.Debug.WriteLine("- Removed: " + ctp.Name);
        }

        public void SelectCToolStripButton(int x, int y)
        {
            if (navigationBar.GetItemAt(x, y) is CToolStripButton)
            {
                navigationBar.GetItemAt(x, y).PerformClick();

                CurrentTabIndex = configurationTabs.SelectedIndex;
                CurrentButtonIndex = navigationBar.Items.IndexOf((CToolStripButton)navigationBar.GetItemAt(x, y));
            }
        }

        private void UpdateCurrents()
        {
            CurrentTabIndex = configurationTabs.SelectedIndex;
            CurrentButtonIndex = navigationBar.Items.IndexOf(CurrentButton);
        }
    }
}
