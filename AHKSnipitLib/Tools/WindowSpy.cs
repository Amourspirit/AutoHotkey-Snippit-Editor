using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Tools
{
    /// <summary>
    /// Location information for Window spy utility
    /// </summary>
    public static class WindowSpy
    {
        private const string SpyName = "AU3_Spy.exe";

        /// <summary>
        /// Gets the string value of the current Window Spy exe full path and file name.
        /// </summary>
        /// <returns>
        /// String of the full path and file name if the file exist; Otherwise, empty string.
        /// </returns>
        /// <remarks>
        /// The value is searched for in the setting.ini file. The setting key is windowspy and is in the APPS section.
        /// If a valid value is not found in the ini file then other known AutoHotkey paths are searched.
        /// If ini value is not valid but the window spy is found then the ini file will be updated to reflect
        /// the new location.
        /// </remarks>
        public static string GetPath()
        {


            string sPath;
            sPath = IniHelper.GetWindowSpyApp();
            if (string.IsNullOrEmpty(sPath) == false && File.Exists(sPath) == true)
            {
                return sPath;
            }
            string sFile;

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\AutoHotkey"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("InstallDir");
                        if (o != null)
                        {
                            string InstallDir = o as String;  //"as" because it's REG_SZ...otherwise ToString() might be safe(r)
                            sFile = Path.Combine(InstallDir, WindowSpy.SpyName);
                            if (File.Exists(sFile))
                            {
                                IniHelper.SetWindowSpyApp(sFile);
                                return sFile;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // not found in registry                
            }

            sPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            sFile = Path.Combine(sPath, @"AutoHotkey", WindowSpy.SpyName);
            if (File.Exists(sFile))
            {
                IniHelper.SetWindowSpyApp(sFile);
                return sFile;
            }
            if (Environment.Is64BitOperatingSystem == true)
            {
                sPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                sFile = Path.Combine(sPath, @"AutoHotkey", WindowSpy.SpyName);
                if (File.Exists(sFile))
                {
                    IniHelper.SetWindowSpyApp(sFile);
                    return sFile;
                }
            }

            sPath = "C:\\PortableApps\\AutoHotkey\\AutoHotKey_C";

            sFile = Path.Combine(sPath, WindowSpy.SpyName);
            if (File.Exists(sFile))
            {
                IniHelper.SetWindowSpyApp(sFile);
                return sFile;
            }

            sPath = "C:\\PortableApps\\AutoHotkey\\AutoHotKey_L";

            sFile = Path.Combine(sPath, WindowSpy.SpyName);
            if (File.Exists(sFile))
            {
                IniHelper.SetWindowSpyApp(sFile);
                return sFile;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the path and file to the Window Spy exe file.
        /// </summary>
        /// <param name="value">The path and file of Window Spy</param>
        public static void SetPath(string value)
        {
            IniHelper.SetWindowSpyApp(value);
        }
    }
}
