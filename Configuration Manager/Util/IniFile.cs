using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Configuration_Manager.Util
{
    public class IniFile
    {
        private String ThisExecutable = Process.GetCurrentProcess().MainModule.FileName;
        private String BasePath = "";

        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);


        public IniFile(string INIPath)
        {
            BasePath = ThisExecutable.Substring(0, ThisExecutable.LastIndexOf('\\'));
            path = INIPath;

            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException();
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }


        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }


        // ###################################################################################################################
        // ###################################################################################################################
        //
        // This method will read out a certain value from an ini-file
        //
        // Prameters are: 
        // 1: FileName:             The file name of the ini-file (just the name, not the complete paths)
        // 2: DirectoryLocation:    Looking from the exe-file location
        //                          0: Same directory
        //                          1: One directory up
        //                          2: Two directories up
        //                          ...and so on
        // 3: SubFolder:            Name of a subfolder like "config" or "config\\settings"
        // 4: Section:              The name of the section within the ini-file
        // 5: Key:                  The key to read
        //
        // ###################################################################################################################
        // ###################################################################################################################
        public String GetIniFileValue(String FileName, int DirectoryLocation, String SubFolder, String Section, String Key)
        {
            String IniFileValue = "";
            String File = BasePath;


            // Check the preconditions
            if ((FileName.Length > 0) && (DirectoryLocation > -1) && (Section.Length > 0) && (Key.Length > 0))
            {
                // Now check, if a backslash was added at the beginning of the subfolder name
                if (SubFolder.Length > 0)
                {
                    if (SubFolder[0] == '\\')
                        SubFolder.Remove(1, 1);
                }

                // Now check, if a backslash was added at the end of the subfolder name
                if (SubFolder.Length > 0)
                {
                    if (SubFolder[SubFolder.Length - 1] == '\\')
                        SubFolder.Remove(SubFolder.Length - 1, 1);
                }

                // Now look for the right location of the file
                while ((DirectoryLocation > 0) && File.Contains("\\"))
                {
                    File = File.Substring(0, File.LastIndexOf('\\'));
                    DirectoryLocation--;
                }

                // If "DirectoryLocation" is zero, the base location of the file was found
                if (DirectoryLocation == 0)
                {
                    // Now we are adding the subfolder(s) and the file name
                    File += "\\";
                    File += SubFolder;
                    File += "\\";
                    File += FileName;

                    // Check whether this file exists
                    if (System.IO.File.Exists(File) == true)
                    {
                        path = File;

                        IniFileValue = IniReadValue(Section, Key);
                    }
                    else
                        throw new ArgumentException();
                }
            }

            return IniFileValue;
        }
    }


}
