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
        Model model;
        ToolStrip ToolStrip;
        TabControl TabControl;

        XDocument xdoc;
        ControlFactory cf;

        List<CTabPage> CTabPages;
        List<CToolStripButton> CToolStripButtons;

        public ToolStripView(ToolStrip ts, TabControl tc, Model model)
        {
            CTabPages = new List<CTabPage>();
            CToolStripButtons = new List<CToolStripButton>();

            this.model = model;
            this.ToolStrip = ts;
            this.TabControl = tc;
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

        // Reads the objects contained inside the ObjectDefinition file.
        // Taking care only of the ToolStripButton ones.
        public void ReadObjectDefinition()
        {
            var items = from item in xdoc.Descendants("NavigationBar")
                        .Descendants("Objects")
                        .Descendants("ToolStripButton")
                        select item;

            foreach (var i in items)
            {
                CreateToolStripButton();
                CreateTabPage();
            }

            SetUpToolStripButtonHandlers();
        }

        private ToolStripButton CreateToolStripButton()
        {
            CToolStripButton ctsb = ControlFactory.getInstance().BuildCToolStripButton(null);
            //ts.Items.Add(ctsb.GetToolStripButton());      // Add the wrapped ToolStripButton
            ToolStrip.Items.Add(ctsb);                      // Add the CToolStripButton

            System.Diagnostics.Debug.WriteLine("+ Loaded: " + ctsb.Name);

            return ctsb;
        }

        private TabPage CreateTabPage()
        {
            CTabPage ctp = ControlFactory.getInstance().BuildCTabPage(null);
            //tc.TabPages.Add(ctp.GetTabPage());          // Add the wrapped TabPage
            TabControl.TabPages.Add(ctp);                 // Add the CTabPage

            System.Diagnostics.Debug.WriteLine("+ Loaded: " + ctp.Name);

            return ctp;
        }

        private void SetUpToolStripButtonHandlers()
        {
            for (int i = 0; i < ToolStrip.Items.Count; i++)
            {
                if (ToolStrip.Items[i] is CToolStripButton)
                {
                    ToolStrip.Items[i].Click += new EventHandler(this.ToolStripButton_Click);
                }
            }
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                CToolStripButton b = (CToolStripButton)sender;
                System.Diagnostics.Debug.WriteLine("! Clicked: " + b.Name + " ID: " +b.Id);
                TabControl.SelectTab(b.Id);
                System.Diagnostics.Debug.WriteLine("! Selected Tab: " + TabControl.SelectedTab.Name);
            } 
        }
    }
}
