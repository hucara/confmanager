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

        private bool progMode = false;
        private bool editingUI = false;
        private bool creatingNewComponent = false;
        private bool editingOldComponent = false;

        public bool ObjectDefinitionExists { get; set; }

        private List<String> sections;
        
        public List<System.Windows.Forms.ToolStripItem> toolStripItems {get; private set;}
        public List<ICustomControl> AllControls { get; private set; }

        private String CurrentLanguageFile;
        
        // Constructor
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
