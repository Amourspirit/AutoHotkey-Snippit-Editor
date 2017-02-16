using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class profileCodeLanguagePaths
    {
        private bool m_mainDataFullPathSet = false;
        private string m_mainDataFullPath;
        /// <summary>
        /// Gets the full path of the Main Data folder to the XML Plugin files for the current profile.
        /// </summary>
        public string MainDataFullPath
        {
            get
            {
                if (this.m_mainDataFullPathSet == false)
                {
                    this.m_mainDataFullPathSet = true;
                    if (string.IsNullOrWhiteSpace(this.mainData))
                    {
                        this.m_mainDataFullPath = string.Empty;
                    }
                    else
                    {
                        if (this.mainData.IndexOf(Path.DirectorySeparatorChar) > -1)
                        {
                            // may be a full path here
                            this.m_mainDataFullPath = this.mainData;
                        }
                        else
                        {
                            this.m_mainDataFullPath = Path.Combine(AppCommon.Instance.PathXml, this.mainData);
                        }
                    }
                }
                return this.m_mainDataFullPath;
            }
        }

        /// <summary>
        /// Gets the Folder for the Hot strings that are of the <see cref="hotstringType.Paste"/>
        /// </summary>
        /// <remarks>
        /// The contents of this folder will be deleted and re-created each time the profile is reloaded.
        /// </remarks>
        public string SnippitInlinePath
        {
            get
            {
                return Path.Combine(this.SnipsFullPath, Properties.Settings.Default.SnipsFolderInline);
            }
        }

       

        private bool m_pluginPathSet = false;
        private string m_pluginFullPath;
        /// <summary>
        /// Gets the full path of the Plugins folder. The Path the Hotkey (command) Plugins are stored in for the profile
        /// </summary>
        public string PluginFullPath
        {
            get
            {
                if (this.m_pluginPathSet == false)
                {
                    this.m_pluginPathSet = true;
                    this.m_pluginFullPath = string.Empty;
                    if ((string.IsNullOrWhiteSpace(this.plugin) == true) && (string.IsNullOrEmpty(this.MainDataFullPath) == false))
                    {
                        // if the plugin value is null or empty then default it to the main_data folder
                        string dirName = new DirectoryInfo(this.MainDataFullPath).Name;
                        this.m_pluginFullPath = Path.Combine(AppCommon.Instance.PathPlugins, dirName, Properties.Settings.Default.PluginCmdSubFolder);
                    }
                    else
                    {
                        if (this.plugin.IndexOf(Path.DirectorySeparatorChar) > -1)
                        {
                            // may be a full path here
                            this.m_pluginFullPath = Path.Combine(this.plugin, Properties.Settings.Default.PluginCmdSubFolder);
                        }
                        else
                        {
                            this.m_pluginFullPath = Path.Combine(AppCommon.Instance.PathPlugins, this.plugin, Properties.Settings.Default.PluginCmdSubFolder);
                        }
                    }
                }
                return this.m_pluginFullPath;
            }
        }

        private bool m_datatItemsPathSet = false;
        private string m_dataItemsFullPath;
        /// <summary>
        /// Gets the full path of the Data Items folder. The Path the Data files are stored in for the profile
        /// </summary>
        public string DataItemsFullPath
        {
            get
            {
                if (this.m_datatItemsPathSet == false)
                {
                    this.m_datatItemsPathSet = true;
                    this.m_dataItemsFullPath = string.Empty;
                    if ((string.IsNullOrWhiteSpace(this.dataItemsPath) == true) && (string.IsNullOrEmpty(this.MainDataFullPath) == false))
                    {
                        // if the plugin value is null or empty then default it to the main_data folder
                        string dirName = new DirectoryInfo(this.MainDataFullPath).Name;
                        this.m_dataItemsFullPath = Path.Combine(AppCommon.Instance.PathDataFiles, dirName);
                    }
                    else
                    {
                        if (this.dataItemsPath.IndexOf(Path.DirectorySeparatorChar) > -1)
                        {
                            // may be a full path here
                            this.m_dataItemsFullPath = this.dataItemsPath.Trim();
                        }
                        else
                        {
                            this.m_dataItemsFullPath = Path.Combine(AppCommon.Instance.PathDataFiles, this.dataItemsPath);
                        }
                    }
                }
                return this.m_dataItemsFullPath;
            }
        }


        private bool m_PluginIncludeFullPathSet = false;
        private string m_PluginIncludeFullPath;
        /// <summary>
        /// Gets the full path of the folder for plugins of type <see cref="include"/>. The Path the Include Plugins are stored in for the profile
        /// </summary>
        public string PluginIncludeFullPath
        {
            get
            {
                if (this.m_PluginIncludeFullPathSet == false)
                {
                    this.m_PluginIncludeFullPathSet = true;
                    this.m_PluginIncludeFullPath = string.Empty;
                    if ((string.IsNullOrWhiteSpace(this.plugin) == true) && (string.IsNullOrEmpty(this.MainDataFullPath) == false))
                    {
                        // if the plugin value is null or empty then default it to the main_data folder
                        string dirName = new DirectoryInfo(this.MainDataFullPath).Name;
                        this.m_PluginIncludeFullPath = Path.Combine(AppCommon.Instance.PathPlugins, dirName, Properties.Settings.Default.PluginIncludeSubFolder);
                    }
                    else
                    {
                        if (this.plugin.IndexOf(Path.DirectorySeparatorChar) > -1)
                        {
                            // may be a full path here
                            this.m_PluginIncludeFullPath = Path.Combine(this.plugin, Properties.Settings.Default.PluginIncludeSubFolder);
                        }
                        else
                        {
                            this.m_PluginIncludeFullPath = Path.Combine(AppCommon.Instance.PathPlugins, this.plugin, Properties.Settings.Default.PluginIncludeSubFolder);
                        }
                    }
                }
                return this.m_PluginIncludeFullPath;
            }
        }



        private bool m_snipsPathSet = false;
        private string m_snipsFullPath;
        /// <summary>
        /// Gets the full path of the Snipits folder. The folder that Hotstring Snippits are stored in for the Profile.
        /// </summary>
        /// <remarks>
        /// Defaluts to <see cref="main_data"/> value to build path if <see cref="snips"/> is null or empty
        /// </remarks>
        public string SnipsFullPath
        {
            get
            {
                if (this.m_snipsPathSet == false)
                {
                    this.m_snipsPathSet = true;
                    this.m_snipsFullPath = string.Empty;
                    if ((string.IsNullOrWhiteSpace(this.snips) == true) && (string.IsNullOrEmpty(this.MainDataFullPath) == false))
                    {
                        // if the snips value is null or empty then default it to the main_data folder
                        string dirName = new DirectoryInfo(this.MainDataFullPath).Name;
                        this.m_snipsFullPath = Path.Combine(AppCommon.Instance.PathSnips, dirName);
                    }
                    else
                    {
                        if (this.plugin.IndexOf(Path.DirectorySeparatorChar) > -1)
                        {
                            // may be a full path here
                            this.m_snipsFullPath = this.snips;
                        }
                        else
                        {
                            this.m_snipsFullPath = Path.Combine(AppCommon.Instance.PathSnips, this.snips);
                        }
                    }
                }
                return this.m_snipsFullPath;
            }
        }

        private bool m_DataPathSet = false;
        private string m_DataFullPath;
        /// <summary>
        /// Gets the full path of the Snipits folder. The folder that Hotstring Snippits are stored in for the Profile.
        /// </summary>
        /// <remarks>
        /// Defaluts to <see cref="main_data"/> value to build path if <see cref="snips"/> is null or empty
        /// </remarks>
        //public string DataItemsFullPath
        //{
        //    get
        //    {
        //        if (this.m_DataPathSet == false)
        //        {
        //            this.m_DataPathSet = true;
        //            this.m_DataFullPath = string.Empty;
        //            if ((string.IsNullOrWhiteSpace(this.) == true) && (string.IsNullOrEmpty(this.MainDataFullPath) == false))
        //            {
        //                // if the snips value is null or empty then default it to the main_data folder
        //                string dirName = new DirectoryInfo(this.MainDataFullPath).Name;
        //                this.m_snipsFullPath = Path.Combine(AppCommon.Instance.PathSnips, dirName);
        //            }
        //            else
        //            {
        //                if (this.plugin.IndexOf(Path.DirectorySeparatorChar) > -1)
        //                {
        //                    // may be a full path here
        //                    this.m_DataFullPath = this.snips;
        //                }
        //                else
        //                {
        //                    this.m_DataFullPath = Path.Combine(AppCommon.Instance.PathSnips, this.snips);
        //                }
        //            }
        //        }
        //        return this.m_DataFullPath;
        //    }
        //}
    }
}
