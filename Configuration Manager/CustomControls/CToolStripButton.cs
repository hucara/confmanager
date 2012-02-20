using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager.CustomControls
{
    class CToolStripButton : ToolStripButton, ICustomControl
    {
        static int count = 0;

        public int TypeId { get; private set; }
        public int Id { get; private set; }
        public String Hint { get; private set; }

        public List<ICustomControl> RelatedRead { get; private set; }
        public List<ICustomControl> RelatedWrite { get; private set; }
        public List<ICustomControl> RelatedVisibility { get; private set; }
        public List<ICustomControl> CoupledControls { get; private set; }

        public CTabPage RelatedTabPage;

        public CToolStripButton()
        {
            this.TypeId = count;
            this.Name = "CToolStripButton" + count;
            this.CheckOnClick = true;

            count++;
        }

        public CToolStripButton(Section s)
        {
            if (s == null) throw new ArgumentNullException();

            this.TypeId = count;
            this.Name = s.Name;
            this.Text = s.Text;
            this.RelatedTabPage = null;

            this.CheckOnClick = true;

            count++;
        }

        public void SetControlDescription(ControlDescription cd)
        {
        }

        public void SetSectionDescription(Section s)
        {
            if (s == null) throw new ArgumentNullException();

            this.Name = s.Name;
            this.Text = s.Text;
        }

        public void SetSectionName(String s)
        {
            this.Text = s;
        }

        public ControlDescription cd
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
