using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;
using System.Windows.Forms;

namespace Configuration_Manager
{
    class Model
    {
        private static Model model;
        public const int MAX_SECTIONS = 6;
        public const int MAX_COMPONENTS_PER_PAGE = 30;

#if DEBUG
        public bool progMode = true;
#else
        public bool progMode = false;
#endif
        public bool editingUI = false;
        public bool creatingNewComponent = false;
        public bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }

        public List<String> DestinationFileTypes;
        public List<String> RelationTypes;
       
        public List<ICustomControl> AllControls { get; private set; }

        public Control CurrentClickedControl { get; set; }
        public ControlDescription CurrentControlDescription { get; set; }
        public int LastClickedX { get; set; }
        public int LastClickedY { get; set; }

        public Section CurrentSection;
        public List<Section> Sections { get; set; }
        
        // Private constructor
        private Model()
        {
            AllControls = new List<ICustomControl>();
            Sections = new List<Section>();
            DestinationFileTypes = new List<String>();
            RelationTypes = new List<String>();

            ObjectDefinitionExists = false;

            FillDestinationFileTypes();
            FillOutRelationTypes();
        }

        // Singleton approach
        public static Model getInstance()
        {
            if (model == null)
            {
                model = new Model();
            }
            return model;
        }

        private void FillDestinationFileTypes()
        {
            DestinationFileTypes.Add(".INI");
            DestinationFileTypes.Add(".XML");
            DestinationFileTypes.Add(".TXT");
        }

        private void FillOutRelationTypes()
        {
            //RelationTypes.Add("Write related");
            //RelationTypes.Add("Read related");
            //RelationTypes.Add("Visibility related");
            //RelationTypes.Add("Coupled controls");
        }

        public String DeleteControlReferences(Control c)
        {
            ICustomControl d = c as ICustomControl;

            foreach (ICustomControl s in AllControls)
            {
                s.cd.RelatedRead.Remove(d);
                s.cd.RelatedVisibility.Remove(d);
                s.cd.RelatedWrite.Remove(d);
                s.cd.CoupledControls.Remove(d);
            }

            return "";
        }

		public void ReadConfigurationFile()
		{

		}
    }
}
