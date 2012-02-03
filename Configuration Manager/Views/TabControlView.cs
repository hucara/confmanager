using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager.Views
{
    class TabControlView : IView
    {
        int xPos;
        int yPos;

        TabControl TabControl;
        ContextMenuStrip mStrip;
        Model model;

        public TabControlView(TabControl tc, ContextMenuStrip cms, Model model)
        {
            this.mStrip = cms;
            this.TabControl = tc;
            this.model = model;
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

        public void SetProgModeHandlers()
        {
            foreach (TabPage t in TabControl.TabPages)
            {
                t.Click += new EventHandler(this.TabPage_ProgMode_Click);
            }
        }

        public void TabPage_ProgMode_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            MouseButtons buttonPushed = me.Button;

            Control c = sender as Control;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                xPos = me.X;
                yPos = me.Y;

                mStrip.Show(c, new System.Drawing.Point(xPos, yPos));
                System.Diagnostics.Debug.WriteLine("! Clicked : " + c.Name + " in X: " + xPos + " and Y: " + yPos);
            }
        }
    }
}