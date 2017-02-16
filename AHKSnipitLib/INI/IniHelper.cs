using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI
{
    /// <summary>
    /// Class to read and write ini files
    /// </summary>
    public static class IniHelper
    {
        #region Settings Ini
        /// <summary>
        /// Gets the setting in the ini file for the current profile Value PROFILE/current
        /// </summary>
        /// <returns>
        /// String Representing the setting of PROFILE/current or an empty string if not found.
        /// </returns>
        public static string GetCurrentProfile()
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            string retval = string.Empty;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                retval = data[Properties.Settings.Default.IniProfileSection][Properties.Settings.Default.IniProfileCurrent];
            }
            return retval;
        }

        /// <summary>
        /// Gets the Window Spy Location from Settings.ini, Apps, windowspy key
        /// </summary>
        /// <returns>
        /// The value from the ini file if it exist; Otherwise, empty string.
        /// </returns>
        /// <remarks>
        /// This Gives the user a way to set the Window Spy location if it is not found at its default location.
        /// </remarks>
        public static string GetWindowSpyApp()
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            string retval = string.Empty;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                retval = data[Properties.Settings.Default.IniSettingsAppSection][Properties.Settings.Default.IniSettingsAppWindowSpy];
            }
            return retval;
        }

        // <summary>
        /// Sets the location to the Window Spy in the ini file
        /// </summary>
        /// <param name="value">The value to set</param>
        public static void SetWindowSpyApp(string value)
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            var ini = new IniParser.FileIniDataParser();
            IniParser.Model.IniData data;
            if (File.Exists(iniFile))
            {
                data = ini.ReadFile(iniFile);
                if (data.Sections.ContainsSection(Properties.Settings.Default.IniSettingsAppSection) == false)
                {
                    data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniSettingsAppSection));
                }
                data[Properties.Settings.Default.IniSettingsAppSection][Properties.Settings.Default.IniSettingsAppWindowSpy] = value;
            }
            else
            {
                data = new IniParser.Model.IniData();
                data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniSettingsAppSection));
                data[Properties.Settings.Default.IniSettingsAppSection][Properties.Settings.Default.IniSettingsAppWindowSpy] = value;
            }
            ini.WriteFile(iniFile, data);

        }

        /// <summary>
        /// Sets the current profile value Profile/current
        /// </summary>
        /// <param name="value">The value to set the PROFILE to</param>
        public static void SetCurrentProfile(string value)
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            var ini = new IniParser.FileIniDataParser();
            IniParser.Model.IniData data;
            if (File.Exists(iniFile))
            {
                data = ini.ReadFile(iniFile);
                if (data.Sections.ContainsSection(Properties.Settings.Default.IniProfileSection) == false)
                {
                    data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniProfileSection));
                }
                data[Properties.Settings.Default.IniProfileSection][Properties.Settings.Default.IniProfileCurrent] = Path.GetFileName(value); // will return filename even if there no path prepended
            }
            else
            {
                data = new IniParser.Model.IniData();
                data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniProfileSection));
                data[Properties.Settings.Default.IniProfileSection][Properties.Settings.Default.IniProfileCurrent] = Path.GetFileName(value); // will return filename even if there no path prepended
            }
            ini.WriteFile(iniFile, data);

        }

        /// <summary>
        /// Reads INI file and get the SCRIPT/Version. If INI Value is not present then Resource Min Version is returned.
        /// </summary>
        /// <returns>Version Representing the script version</returns>
        public static Version GetScriptVersion()
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            string strVersion = string.Empty;
            if (File.Exists(iniFile))
            {
                try
                {
                    var ini = new IniParser.FileIniDataParser();
                    IniParser.Model.IniData data = ini.ReadFile(iniFile);
                    strVersion = data[Properties.Settings.Default.IniScriptSection][Properties.Settings.Default.IniScriptVersion];
                }
                catch (Exception)
                {
                    strVersion = string.Empty;
                }
            }
            Version ver;
            if (string.IsNullOrEmpty(strVersion))
            {
                ver = AppCommon.Instance.DefaultMinVersion;
            }
            else
            {
                if (!Version.TryParse(strVersion, out ver))
                {
                    ver = AppCommon.Instance.DefaultMinVersion;
                }
            }
            return ver;
        }
        #endregion

        #region AppINI
        /// <summary>
        /// Gets the Path to the current running AutoHotkey Script
        /// </summary>
        /// <returns>
        /// Full Path to script; Otherwise empty string.
        /// </returns>
        public static string GetRunningScriptPath()
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            string scriptPath = string.Empty;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                scriptPath = data[Properties.Settings.Default.IniScriptSection][Properties.Settings.Default.IniScriptPath];
            }
            if (string.IsNullOrEmpty(scriptPath))
            {
                return string.Empty;
            }
            return scriptPath;
        }

        /// <summary>
        /// Gets the Path to the Launch AutoHotkey Script
        /// </summary>
        /// <returns>
        /// Full Path to script if the Launch Script. If the Ini value is not set or
        /// the ini path to script file is invalid then empty string is returned.
        /// </returns>
        public static string GetLauncherScriptPath()
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            string scriptPath = string.Empty;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                scriptPath = data[Properties.Settings.Default.IniScriptSection][Properties.Settings.Default.IniLaunchPath];
            }
           
            if (string.IsNullOrEmpty(scriptPath))
            {
                return string.Empty;
            }
            if (File.Exists(scriptPath) == false)
            {
                return string.Empty;
            }
            return scriptPath;
        }

        /// <summary>
        /// Gets if the AutoHotkey Script is running based upon the ini load_state setting
        /// </summary>
        /// <returns>
        /// True if the script is running; Otherwise false
        /// </returns>
        public static bool GetScriptIsRunning()
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            bool retval = false;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                string state = data[Properties.Settings.Default.IniScriptSection][Properties.Settings.Default.IniScriptLoadState];
                bool Parsed = false;
                // attempt to parse as boolean if that does not work attempt to parse as int
                if (bool.TryParse(state, out retval))
                {
                    Parsed = true;

                }
                if (Parsed == false)
                {
                    int iState = 0;
                    if (int.TryParse(state, out iState))
                    {
                        retval = (iState > 0);
                    }
                }

            }
            return retval;
        }

        /// <summary>
        /// Set the Exit Code in the App ini file for the Writer Exit code
        /// </summary>
        /// <param name="Reason"></param>
        public static void SetWriterExitReason(int Reason)
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            var ini = new IniParser.FileIniDataParser();
            IniParser.Model.IniData data;
            if (File.Exists(iniFile))
            {
                data = ini.ReadFile(iniFile);
                if (data.Sections.ContainsSection(Properties.Settings.Default.IniWriterSection) == false)
                {
                    data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniWriterSection));
                }
                data[Properties.Settings.Default.IniWriterSection][Properties.Settings.Default.IniWriterExitCode] = Reason.ToString(); // will return filename even if there no path prepended
            }
            else
            {
                data = new IniParser.Model.IniData();
                data.Sections.Add(new IniParser.Model.SectionData(Properties.Settings.Default.IniWriterSection));
                data[Properties.Settings.Default.IniWriterSection][Properties.Settings.Default.IniWriterExitCode] = Reason.ToString(); // will return filename even if there no path prepended
            }
            ini.WriteFile(iniFile, data);
        }

        /// <summary>
        /// Gets the Exit Code in the App ini for the writers last exit code;
        /// </summary>
        /// <returns></returns>
        public static int GetWriterExitReason()
        {
            string iniFile = AppCommon.Instance.AppIniFile;
            int Reason = 0;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                string iniReason = data[Properties.Settings.Default.IniWriterSection][Properties.Settings.Default.IniWriterExitCode];
                if (int.TryParse(iniReason, out Reason) == false)
                {
                    Reason = 0;
                }
            }
            return Reason;
        }
        #endregion
    }
}
