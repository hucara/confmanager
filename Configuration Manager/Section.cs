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
        public bool Selected { get; set; }

        public int Id { get; private set; }
        public int GlobalId { get; private set; }
        public int RelatedTabIndex { get; private set; }

        public CToolStripButton Button { get; set; }
        public System.Windows.Forms.TabPage Tab {get; set;}

        public String Name { get; set; }
        public String text { get; private set; }
        public String RealText { get; set; }
        public String Hint { get; set; }

        private String displayRight = "00000000";
        private String modificationRight = "00000000";

        public byte[] DisplayBytes = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public byte[] ModificationBytes = new byte[4] { 0x00, 0x00, 0x00, 0x00 };

        public Section(CToolStripButton b, System.Windows.Forms.TabPage t, String text, bool Selected)
        {
            this.Name = "Section" + count;
            this.text = text;
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
            this.text = text;
            this.Selected = selected;
            this.Button = ctsb;
            this.Tab = tp;
            this.Id = count;

            tp.Name = this.Name;

            this.Button.Text = text;
            count++;
        }

        public String DisplayRight
        {
            get
            {
                return this.displayRight;
            }
            set
            {
                this.displayRight = value;
                this.DisplayBytes = Model.HexToData(value);
            }
        }

        public String ModificationRight
        {
            get
            {
                return this.modificationRight;
            }
            set
            {
                this.modificationRight = value;
                this.ModificationBytes = Model.HexToData(value);
            }
        }

        public String Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.Button.Text = value;
            }
        }
    }
}
