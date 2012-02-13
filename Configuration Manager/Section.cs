using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    class Section
    {
        static int count = 0;
        public bool Selected { get; private set; }

        public int Id { get; private set; }
        public int GlobalId { get; private set; }
        public int RelatedTabIndex { get; private set; }

        public CToolStripButton Button { get; set; }
        public CTabPage Tab {get; set;}

        public String Name { get; private set; }
        public String Text { get; private set; }

        public Section(CToolStripButton b, CTabPage t, String text, bool Selected)
        {
            this.Name = "Section" + count.ToString();
            this.Text = text;
            this.Selected = Selected;

            this.Button = b;
            this.Tab = t;

            this.Button.Text = text;
            count++;
        }
    }
}
