using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Configuration_Manager.Util
{

    // *** Registry Manager *** //

    /*
     * Simple class to ease the use of the Registry read and write.
     * Allows reading and writing keys from the user registry.
     */
    class RegistryManager
    {
        RegistryKey root;

        public RegistryManager(String rootPath)
        {
            root = SetRootKey(rootPath);
        }

        private RegistryKey SetRootKey(string rootPath)
        {
            if(rootPath == "" || rootPath == null) return null;

            rootPath = rootPath.Trim();

            switch (rootPath)
            {
                case "HKEY_CLASSES_ROOT":
                    return Registry.ClassesRoot;

                case "HKEY_CURRENT_USER":
                    return Registry.CurrentUser;

                case "HKEY_CURRENT_CONFIG":
                    return Registry.CurrentConfig;

                case "HKEY_USERS":
                    return Registry.Users;

                case "HKEY_LOCAL_MACHINE":
                    return Registry.LocalMachine;
            }

            return null;
        }

        public string GetValue(string path)
        {
            if(path == "" || path == null) return "";
           
            RegistryKey k = root;
            List<string> subKeys = path.Trim().Split('/').ToList();

            for(int i = 1; i < subKeys.Count -1; i++)
            {
                k = k.OpenSubKey(subKeys[i]);
            }

            return k.GetValue(subKeys[subKeys.Count -1]).ToString();
        }

        public void SetValue(string path, string value)
        {
            if (path != "" && path != null)
            {
                RegistryKey k = root;
                List<string> subKeys = path.Trim().Split('/').ToList();

                for (int i = 1; i < subKeys.Count - 1; i++)
                {
                    k = k.OpenSubKey(subKeys[i], true);
                }

                k.SetValue(subKeys[subKeys.Count - 1], value);
            }
        }
    }
}
