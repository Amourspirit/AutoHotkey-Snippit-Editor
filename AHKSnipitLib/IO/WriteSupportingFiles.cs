using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO
{
    /// <summary>
    /// Contains Various methods for Reading and Writing Snippit XML Files
    /// </summary>
    public static class ReadWrite
    {
        private static Version ScriptVersion;
        private static object ProfileLock;
        static ReadWrite()
        {
            ScriptVersion = INI.IniHelper.GetScriptVersion();
            ProfileLock = new object();
        }
        /// <summary>
        /// Saves a profile to disk
        /// </summary>
        /// <param name="p">The profile to save</param>
        /// <exception cref="Exceptions.ProfileVersionException">If Profile min version is greater than scrip version</exception>
        public static void SaveProfile(profile p)
        {
            
            lock (ProfileLock)
            {
                if (p.FullMinVersion > ScriptVersion)
                {
                    throw new Exceptions.ProfileVersionException(Properties.Resources.ErrorProfileMinVersionNewer);
                }
                string sFile = Path.Combine(AppCommon.Instance.PathProfiles, p.name + ".xml");

                if (string.IsNullOrEmpty(p.codeLanguage.paths.mainData))
                {
                    p.codeLanguage.paths.mainData = p.name;
                }
                else
                {
                    if (p.codeLanguage.paths.mainData.IndexOf(Path.DirectorySeparatorChar) > -1)
                    {
                        if (!Directory.Exists(p.codeLanguage.paths.mainData))
                        {
                            p.codeLanguage.paths.mainData = p.name;

                        }
                        else
                        {
                            sFile = Path.Combine(p.codeLanguage.paths.mainData, p.name + ".xml");
                        }
                    }
                    else
                    {
                        p.codeLanguage.paths.mainData = p.name;
                    }
                }
                // check for an existing profile before overwriting
                if (File.Exists(sFile))
                {
                    // test the existing profile to see if it is valid
                    // test to see if the current existing file is a valid plugin
                    ValidationResult vFile = ValidateXml.ValidateProfileFile(sFile);
                    if (vFile.HasErrors == false)
                    {
                        // now that we have an existing plugin and it is valid lets check is version against the current plugin version.
                        var ExistingProfile = profile.FromFile(sFile);
                        if (ExistingProfile.FullVersion >= p.FullVersion)
                        {
                            // The existing plugin version is newer or the same as this plugin version
                            // skip, not a newer version.
                            return;
                        }
                        else
                        {
                            ExistingProfile = null;
                        }
                    }
                }

                XmlSerializer writer = new XmlSerializer(typeof(profile));
                FileStream file = File.Create(sFile);
                writer.Serialize(file, p);
                file.Close();
            }
            
        }

        /// <summary>
        /// Saves all plugins contain with <see cref="SnippitInstal"/> to disk
        /// </summary>
        /// <param name="si">The <see cref="SnippitInstal"/>that contains the plugin(s) to save</param>
        /// <param name="Overwrite">If True Any snippits and plugins files will be overwritten</param>
        /// <remarks>Plugins are saved in Parallel</remarks>
        public static void SavePlugin(SnippitInstal si, bool Overwrite)
        {
            if (si == null)
            {
                return;
            }
            if (si.profile == null)
            {
                return;
            }
            if (si.plugin == null)
            {
                return;
            }

            Parallel.ForEach(si.plugin, plg => {
                lock (si)
                {
                    if (plg.ParentProfile == null)
                    {
                        plg.ParentProfile = si.profile;
                    }
                    SavePlugin(plg, Overwrite);
                }
                
            });
        }

        /// <summary>
        /// Saves Plugin to Disk
        /// </summary>
        /// <param name="pf">The Profile the Plugin Belongs to</param>
        /// <param name="plg">The Plugin to save</param>
        /// <param name="Overwrite">If True then all supporting Plugins and Snippits will be overwritten</param>
        public static void SavePlugin(plugin plg, bool Overwrite)
        {
          
            if (plg == null)
            {
                return;
            }
            if (plg.ParentProfile == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(plg.File))
            {
                string strPlgFile = Path.Combine(plg.ParentProfile.codeLanguage.paths.MainDataFullPath, plg.name + @".xml");
                plg.File = strPlgFile;
            }
            if (File.Exists(plg.File))
            {
                // delete all the old plugin file before saving new again
                SnippitInstal si = new SnippitInstal();
                si.profile = plg.ParentProfile;
                si.plugin = new plugin[] { plg };
                si.File = plg.ParentProfile.File;
                ReadWrite.DeleteInstal(si, false, false); // delete plugin but leave profile and folders in tact
            }
            else
            {
                var sPath = Path.GetDirectoryName(plg.File);
                if (Directory.Exists(sPath) == false)
                {
                    Directory.CreateDirectory(sPath);
                }
            }
            XmlSerializer writer = new XmlSerializer(typeof(plugin));

            FileStream file = File.Create(plg.File);
            writer.Serialize(file, plg);
            file.Close();
            //ReadWrite.WritePluginHotkeySnippits(plg, Overwrite);
            //ReadWrite.WritePluginIncludeFiles(plg, Overwrite);
            //ReadWrite.WritePlugHotStringSnippits(plg, Overwrite);
        }

        /// <summary>
        /// Writes the Hotkey snippits to disk for the plugin
        /// </summary>
        /// <param name="pf">The Profile the plugin belongs to</param>
        /// <param name="plg">The Plugin that contains the Hotkeys to write snipps for</param>
        /// <param name="Overwrite"></param>
        [Obsolete("Now done by the Plugin Writer")]
        public static void WritePluginHotkeySnippits(plugin plg, bool Overwrite)
        {
            
            if (plg == null)
            {
                return;
            }
            if (plg.ParentProfile == null)
            {
                return;
            }
            if (plg.commands == null || plg.commands.Length == 0)
            {
                return;
            }

            string sSnippitPath = plg.ParentProfile.codeLanguage.paths.PluginFullPath;
            if (Directory.Exists(sSnippitPath) == false)
            {
                Directory.CreateDirectory(sSnippitPath);
            }
            Parallel.ForEach(plg.commands, c => {
                if (c.type != commandType.include)
                {
                    return;
                }
                if (string.IsNullOrEmpty(c.snippit) == true)
                {
                    return;
                }
                if (string.IsNullOrEmpty(c.code) == true)
                {
                    return;
                }
                string sFile = c.snippit.EndsWith(@".ahk", StringComparison.CurrentCultureIgnoreCase) ? c.snippit : c.snippit + @".ahk";
                sFile = Path.GetFileName(sFile); // gets the filename regardless if there is a path or not
                string sFilePath = Path.Combine(sSnippitPath, sFile);

                if (File.Exists(sFilePath) == true)
                {
                    if (Overwrite == false)
                    {
                        return;
                    }
                    File.Delete(sFilePath);
                }
                using (StreamWriter writer = new StreamWriter(sFilePath, false, Encoding.UTF8))
                {
                    writer.Write(c.code);
                }

            });
           
        }

        /// <summary>
        /// Writes the Include Files for the plugin to disk
        /// </summary>
        /// <param name="plg">The Plugin to write the includes for</param>
        /// <param name="Overwrite">If true any existing includes will be overwritten</param>
        [Obsolete("Now done by the Plugin Writer")]
        public static void WritePluginIncludeFiles(plugin plg, bool Overwrite)
        {
           
            if (plg == null)
            {
                return;
            }
            if (plg.ParentProfile == null)
            {
                return;
            }
            if (plg.includes == null || plg.includes.Length == 0)
            {
                return;
            }

            string sSnippitPath = plg.ParentProfile.codeLanguage.paths.PluginFullPath;
            if (!Directory.Exists(sSnippitPath))
            {
                Directory.CreateDirectory(sSnippitPath);
            }

            Parallel.ForEach(plg.includes, inc => {
                if (string.IsNullOrEmpty(inc.snippit) == true)
                {
                    return;
                }
                if (string.IsNullOrEmpty(inc.code) == true)
                {
                    return;
                }
                string sFile = inc.snippit.EndsWith(@".ahk", StringComparison.CurrentCultureIgnoreCase) ? inc.snippit : inc.snippit + @".ahk";
                sFile = Path.GetFileName(sFile); // gets the filename regardless if there is a path or not
                string sFilePath = Path.Combine(sSnippitPath, sFile);
                if (File.Exists(sFilePath) == true)
                {
                    if (Overwrite == false)
                    {
                        return;
                    }
                    File.Delete(sFilePath);
                }
                using (StreamWriter writer = new StreamWriter(sFilePath, false, Encoding.UTF8))
                {
                    writer.Write(inc.code);
                }
            });

        }

        

        /// <summary>
        /// Deletes Plugin from Disk. Also delete all the plugin supporting files such as snippits.
        /// </summary>
        /// <param name="plg">The Plugin to delete</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">If <see cref="plugin.ParentProfile"/>is null</exception>
        public static void DeletePlugin(plugin plg)
        {
            if (plg == null)
            {
                throw new ArgumentNullException();
            }
            if (plg.ParentProfile == null)
            {
                throw new ArgumentException(Properties.Resources.ErrorPluginParentNull);
            }
            SnippitInstal inst = new SnippitInstal();
            inst.profile = plg.ParentProfile;
            inst.plugin = new plugin[] { plg };
            ReadWrite.DeleteInstal(inst, false, true);
        }

        /// <summary>
        /// Deletes all the plugins in the SnipitInstall and the Profile as well if DeleteProfle is true
        /// </summary>
        /// <param name="inst">SnippitInstall instance</param>
        /// <param name="DeleteProfile">If true the profile will also be deleted if all the plugins are deleted for this profile</param>
        /// <remarks>Empty Folder will be deleted</remarks>
        public static void DeleteInstal(SnippitInstal inst, bool DeleteProfile)
        {
            ReadWrite.DeleteInstal(inst, DeleteProfile, true);
        }

        /// <summary>
        /// Deletes all the plugins in the SnipitInstall and the Profile as well if DeleteProfle is true
        /// </summary>
        /// <param name="inst">SnippitInstall instance</param>
        /// <param name="DeleteProfile">If true the profile will also be deleted if all the plugins are deleted for this profile</param>
        /// <param name="DeleteEmpytFolders">If true then empty folder will be deleted as well</param>
        public static void DeleteInstal(SnippitInstal inst, bool DeleteProfile, bool DeleteEmpytFolders)
        {
            if (inst == null)
            {
                return;
            }
            if (inst.profile == null)
            {
                throw new ArgumentException("Inst.Profile cannot be null");
            }
            // bool hasProfile = inst.profile != null;
            if (inst.plugin != null)
            {
               
                bool SnipsFolderExist = Directory.Exists(inst.profile.codeLanguage.paths.SnipsFullPath);
                bool PluginsFolderExist = Directory.Exists(inst.profile.codeLanguage.paths.PluginFullPath);
                if (Directory.Exists(inst.profile.codeLanguage.paths.PluginFullPath) == true)
                {
                    string searchP = "*.ahk";
                    var files = Directory.GetFiles(inst.profile.codeLanguage.paths.PluginFullPath, searchP);
                    Parallel.ForEach(files, f => {
                        File.Delete(f);
                    });
                    if (DeleteEmpytFolders == true && IsDirectoryEmpty(inst.profile.codeLanguage.paths.PluginFullPath) == true)
                    {
                        Directory.Delete(inst.profile.codeLanguage.paths.PluginFullPath);
                    }
                }

                if (Directory.Exists(inst.profile.codeLanguage.paths.PluginIncludeFullPath) == true)
                {
                    string searchP = "*.ahk";
                    var files = Directory.GetFiles(inst.profile.codeLanguage.paths.PluginIncludeFullPath, searchP);
                    Parallel.ForEach(files, f => {
                        File.Delete(f);
                    });
                    if (DeleteEmpytFolders == true && IsDirectoryEmpty(inst.profile.codeLanguage.paths.PluginIncludeFullPath) == true)
                    {
                        Directory.Delete(inst.profile.codeLanguage.paths.PluginIncludeFullPath);
                    }
                }

                if (Directory.Exists(inst.profile.codeLanguage.paths.SnippitInlinePath))
                {
                    string searchP = "*" + AppCommon.Instance.DefaultSnippitExt;
                    var files = Directory.GetFiles(inst.profile.codeLanguage.paths.SnippitInlinePath, searchP);
                    Parallel.ForEach(files, f => {
                        File.Delete(f);
                    });
                    if (DeleteEmpytFolders == true && IsDirectoryEmpty(inst.profile.codeLanguage.paths.SnippitInlinePath) == true)
                    {
                        Directory.Delete(inst.profile.codeLanguage.paths.SnippitInlinePath);
                    }
                }

                if (inst.plugin != null)
                {
                    foreach (var plg in inst.plugin)
                    {
                        if (string.IsNullOrEmpty(plg.File) == false && File.Exists(plg.File) == true)
                        {
                            File.Delete(plg.File);
                        }
                    }
                }

                //if (Directory.Exists(inst.profile.codeLanguage.paths.MainDataFullPath))
                //{
                //    string searchP = "*.xml";
                //    var files = Directory.GetFiles(inst.profile.codeLanguage.paths.MainDataFullPath, searchP);
                //    Parallel.ForEach(files, f => {
                //        File.Delete(f);
                //    });
                //}

                if (DeleteEmpytFolders == true && Directory.Exists(inst.profile.codeLanguage.paths.MainDataFullPath))
                {
                    if (IsDirectoryEmpty(inst.profile.codeLanguage.paths.MainDataFullPath))
                    {
                        Directory.Delete(inst.profile.codeLanguage.paths.MainDataFullPath);
                    }
                }

            }


            if (DeleteProfile == true)
            {
                // check and see if all the XML file have been removed from the data folder before deleting profile
                if (Directory.Exists(inst.profile.codeLanguage.paths.MainDataFullPath))
                {
                    // if the folder exist we have to test for any valid plugin
                    var files = Directory.GetFiles(inst.profile.codeLanguage.paths.MainDataFullPath, "*.xml");
                    bool bPluginExist = false;
                    foreach (var file in files)
                    {
                        var vResult = ValidateXml.ValidatePluginFile(file);
                        if (vResult.HasErrors == false)
                        {
                            bPluginExist = true;
                            break;
                        }
                    }
                    if ((bPluginExist == false) && (File.Exists(inst.profile.File)))
                    {
                        File.Delete(inst.profile.File);
                    }
                }
                else
                {
                    if (File.Exists(inst.profile.File))
                    {
                        File.Delete(inst.profile.File);
                    }
                }
            }
        }

        /// <summary>
        /// Gets if a a path is an empty directory
        /// </summary>
        /// <param name="path">The Path to search</param>
        /// <returns>
        /// True if the path contains files; Otherwise false.
        /// </returns>
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Reads the Current Profile from the INI file and creates a new
        /// <see cref="SnippitInstal"/> Populated with the profile and the Plugins of
        /// the current Profile
        /// </summary>
        /// <returns>
        /// SnippitInstall Populated with Profile and Plugins. If current Profile in the ini is invalid then Null.
        /// </returns>
        /// <remarks>
        /// Each <see cref="plugin"/> has it <see cref="plugin.ParentProfile"/> set to the current <see cref="profile"/>
        /// </remarks>
        public static SnippitInstal ReadCurrentInstall()
        {
            profile p = ReadCurrentProfile();
            if (p == null)
            {
                return null;
            }
            SnippitInstal si = new SnippitInstal();
            si.profile = p;
            si.File = p.File;

            string PluginPath = p.codeLanguage.paths.MainDataFullPath;

            if (!Directory.Exists(PluginPath))
            {
                return si;
            }
            var files = Directory.GetFiles(PluginPath, "*.xml");
            List<plugin> plgs = new List<plugin>();
            foreach (string f in files)
            {
                try
                {
                    plugin plg = plugin.FromFile(f);
                    plg.ParentProfile = si.profile;
                    plgs.Add(plg);
                }
                catch (Exception)
                {
                }
            }
            si.plugin = plgs.ToArray();
            return si;
        }

        /// <summary>
        /// Reads the current profile from the INI
        /// </summary>
        /// <returns>
        /// <see cref="profile"/> that represents the current profile if set; Otherwise, Null
        /// </returns>
        public static profile ReadCurrentProfile()
        {
            string IniProfile = IniHelper.GetCurrentProfile();
            if (string.IsNullOrEmpty(IniProfile))
            {
                return null;
            }
            string ProfilePath = Path.Combine(AppCommon.Instance.PathProfiles, IniProfile);
            if (!File.Exists(ProfilePath))
            {
                return null;
            }
            profile p = profile.FromFile(ProfilePath);
            return p;
        }
    }
}
