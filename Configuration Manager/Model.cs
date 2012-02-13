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
        
        public List<System.Windows.Forms.ToolStripItem> toolStripItems {get; private set;}
        public List<ICustomControl> AllControls { get; private set; }
        public Dictionary<int, ICustomControl> DictControls { get; private set; }

        public Control CurrentClickParent { get; set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }

        public List<Section> Sections { get; set; }
        
        // Private constructor
        private Model()
        {
            AllControls = new List<ICustomControl>();
            Sections = new List<Section>();

            ObjectDefinitionExists = false;
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
    }
}
