using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Properties;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib
{
    /// <summary>
    /// Contains Common Settings and Paths
    /// </summary>
    public sealed class AppCommon
    {
        /// <summary>
        /// Represents the All Key for list and other various items that use the all or "Select All"
        /// </summary>
        public const string All = "All";

        /// <summary>
        /// Gets if <paramref name="Text"/> is equal to <see cref="All"/> Constant Value. Case is Ignored
        /// </summary>
        /// <param name="Text">The string to compare to <see cref="All"/></param>
        /// <returns>True if <paramref name="Text"/> is equal to <see cref="All"/>: Otherwise false</returns>
        public static bool IsEqualToAll(string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return false;
            }
            return string.Equals(AppCommon.All, Text, StringComparison.CurrentCultureIgnoreCase);
        }
        
        AppCommon()
        {
            this.InitPaths();
             this.m_DefaultSnippitExt = Properties.Settings.Default.DefaultSnippitExt;
        }

        private static readonly AppCommon _instance = new AppCommon();
        /// <summary>
        /// Instance of AppCommon
        /// </summary>
        /// <remarks>
        /// <see cref="AppCommon"/> uses a singleton pattern
        /// </remarks>
        public static AppCommon Instance
        {
            get
            {
                return _instance;
            }
        }

        private void InitPaths()
        {
            var sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.PathData = Path.Combine(sPath, Settings.Default.DataFolder);
            this.PathDataFiles = Path.Combine(this.PathData, Settings.Default.DataItemsFolder);
            this.PathXml = Path.Combine(this.PathData, Settings.Default.XmlFolder);
            this.PathProfiles = Path.Combine(this.PathData, Settings.Default.ProfilesFolder);
            this.PathSnips = Path.Combine(this.PathData, Settings.Default.SnipsFolder);

            this.PathPlugins = Path.Combine(this.PathData, Settings.Default.PluginsFolder);
            this.SetingsIniFile = Path.Combine(this.PathData, Settings.Default.SettingsIni);
            this.AppIniFile = Path.Combine(this.PathData, Settings.Default.AppIni);
            this.DefaultMinVersion = Version.Parse(Properties.Settings.Default.DefaultMinVersionString);
        }
        #region Properties

        #region Resource Properties
        /// <summary>
        /// Get the URL string for donations
        /// </summary>
        public string UrlDonate
        {
            get
            {
                return Properties.Settings.Default.UrlDonate;
            }
        }

        /// <summary>
        /// Gets the Url to the project home page.
        /// </summary>
        public string UrlProjectHome
        {
            get
            {
                return Properties.Settings.Default.UrlProjectHome;
            }
        }
        private string m_DefaultSnippitExt;
        /// <summary>
        /// Gets The Default extension used for Snippit Files
        /// </summary>
        public string DefaultSnippitExt
        {
            get
            {
                return this.m_DefaultSnippitExt;
            }
        }

        private string m_CodeLicenseHeader;
        /// <summary>
        /// Gets The Default License text to add to the header of code generate files for AutoHotkey
        /// </summary>
        public string CodeLicenseHeader
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CodeLicenseHeader))
                {
                    this.m_CodeLicenseHeader = Properties.Resources.FileLicenseHeaderText;
                }
                return this.m_CodeLicenseHeader;
            }
        }
        /// <summary>
        /// Gets the Minimum allow schema version accepted. Schema Version is read from root element schemaVersion attribute.
        /// </summary>
        public decimal MinAllowSchemaVersion
        {
            get { return Properties.Settings.Default.MinAllowedSchemaVersion; }
        }

        /// <summary>
        /// Gets the Maximum allow schema version accepted. Schema Version is read from root element schemaVersion attribute.
        /// </summary>
        public decimal MAxAllowSchemaVersion
        {
            get { return Properties.Settings.Default.MaxAllowedSchemaVersion; }
        }

        private string m_CodeAutoGenMsg;
        /// <summary>
        /// Gets The Default Auto Generate text to add to the header of code generate files for AutoHotkey
        /// </summary>
        public string CodeAutoGenMsg
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CodeAutoGenMsg))
                {
                    this.m_CodeAutoGenMsg = Properties.Resources.FileAutoGenMessage;
                }
                return this.m_CodeAutoGenMsg;
            }
        }
        #endregion
        /// <summary>
        /// Path to XML Hot string/Hotkey files
        /// </summary>
        public string PathXml { get; set; }
        /// <summary>
        /// Path to Application folder. This is the root folder the app uses in Application Data Folder for the user.
        /// </summary>
        public string PathData { get; set; }

        /// <summary>
        /// Path to Data Files. This is the root path to where data items will be installed. A sub-folder will be created for each profile.
        /// </summary>
        public string PathDataFiles { get; set; }

        /// <summary>
        /// Path to Profiles Folder
        /// </summary>
        public string PathProfiles { get; set; }

        /// <summary>
        /// Path to Snips Folder
        /// </summary>
        public string PathSnips { get; set; }

        /// <summary>
        /// Path to Plugins folder
        /// </summary>
        public string PathPlugins { get; set; }

        /// <summary>
        /// Path to Settings.ini file
        /// </summary>
        public string SetingsIniFile { get; set; }

        /// <summary>
        /// Path to app.ini file
        /// </summary>
        public string AppIniFile { get; set; }

        /// <summary>
        /// The Default Min version allowed for importing plugins if the plugin does not contain a default min version.
        /// </summary>
        public Version DefaultMinVersion { get; set; }
        #endregion

        #region Get Schema From Embeded resource
        private string m_SiInstall1_1;      

        /// <summary>
        /// Gets Version 1.1 SnippitInstal Schema
        /// </summary>
        public string SchemaSnippitInstal1_1
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_SiInstall1_1))
                { 
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Schema.SnippitInstall1_1.xsd";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.m_SiInstall1_1 = reader.ReadToEnd();
                    }
                }
                return m_SiInstall1_1;
            }
        }
        private string m_Profile1_1;
        /// <summary>
        /// Get Profile version 1.1 Schema
        /// </summary>
        public string SchemaProfile1_1
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Profile1_1))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Schema.profile1_1.xsd";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.m_Profile1_1 = reader.ReadToEnd();
                    }
                }
                return m_Profile1_1;
            }
        }

        private string m_Plugin1_1;
        /// <summary>
        /// Get Plugin version 1.1 Schema
        /// </summary>
        public string SchemaPlugin1_1
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Plugin1_1))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Schema.plugin1_1.xsd";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.m_Plugin1_1 = reader.ReadToEnd();
                    }
                }
                return m_Plugin1_1;
            }
        }

        #endregion

    }
}
