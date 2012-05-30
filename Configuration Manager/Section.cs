using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    public class Section
    {
        public static int count = 0;
        private CToolStripButton ctsb;
        private System.Windows.Forms.TabPage tp;
        public bool Selected { get; set; }

        public int Id { get; private set; }
        public int GlobalId { get; private set; }
        public int RelatedTabIndex { get; private set; }

        public CToolStripButton Button { get; set; }
        public System.Windows.Forms.TabPage Tab {get; set;}

        public String Name { get; set; }
        public String Text { get; private set; }

        public Section(CToolStripButton b, System.Windows.Forms.TabPage t, String text, bool Selected)
        {
            this.Name = "Section" + count;
            this.Text = text;
            this.Selected = Selected;
            this.Id = count;

            this.Button = b;
            this.Tab = t;

			t.Name = this.Name;

            this.Button.Text = text;
            count++;
        }

        public Section(CToolStripButton ctsb, System.Windows.Forms.TabPage tp, string name, string text, bool selected)
        {
            this.Name = name;
            this.Text = text;
            this.Selected = selected;
            this.Button = ctsb;
            this.Tab = tp;
            this.Id = count;

            tp.Name = this.Name;

            this.Button.Text = text;
            count++;
        }
    }
}
