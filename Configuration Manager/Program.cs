using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Configuration_Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string []args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
			//Model.getInstance().GetArguments(args);
			//MainForm mf = new MainForm();

			Application.Run(new MainForm());
        }
    }
}