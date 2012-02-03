using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_Manager.CustomControls;

namespace Configuration_Manager
{
    class Model
    {
        private static Model model;

        public bool progMode = false;
        public bool editingUI = false;
        public bool creatingNewComponent = false;
        public bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }

        public List<String> sections;
        
        public List<System.Windows.Forms.ToolStripItem> toolStripItems {get; private set;}
        public List<ICustomControl> AllControls { get; private set; }
        public Dictionary<int, ICustomControl> DictControls { get; private set; }

        public String CurrentLanguageFile;
        
        // Private constructor
        private Model()
        {
            AllControls = new List<ICustomControl>();
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
