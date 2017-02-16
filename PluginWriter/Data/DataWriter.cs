using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Data
{
    class DataWriter : WriterBase
    {
        #region Constructor
        internal DataWriter() : base()
        {
            this.DataIncludeFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.PluginDataFile);

            if (CurrentInstal == null)
            {
                ExitReason = ExitCodeEnum.NoInstall;
                return;
            }

            if (CurrentInstal.plugin == null || CurrentInstal.plugin.Length == 0)
            {
                ExitReason = ExitCodeEnum.NoPlugins;
                return;
            }

            this.m_DataItemPath = this.CurrentInstal.profile.codeLanguage.paths.DataItemsFullPath;

            baseTextWriter = new System.IO.StringWriter();
            w = new IndentedTextWriter(baseTextWriter);
        }

        internal DataWriter(SnippitInstal si) : this()
        {
            this.m_CurrentInstal = si;
        }
        #endregion

        public string DataIncludeFile { get; set; }

        protected internal override void Write()
        {
                

            try
            {
                if (this.CurrentInstal == null)
                {
                    this.SetExitReason(ExitCodeEnum.NoInstall);
                    return;
                }
                if (this.m_CurrentInstal.profile == null)
                {
                    this.SetExitReason(ExitCodeEnum.NoProfile);
                    return;
                }
                if (this.m_CurrentInstal.plugin == null)
                {
                    this.SetExitReason(ExitCodeEnum.NoPlugins);
                    // instead of exiting out if ther are no plugin will write global var to ""
                    //return;
                }

                if (this.ScriptMinVersion < this.CurrentInstal.profile.FullMinVersion)
                {
                    this.SetExitReason(ExitCodeEnum.MinVersionError);
                    return;
                }

                if (this.RootFolderCheck() == false)
                {
                    return;
                }

                // write the include file for globlal data location var
                w.WriteLine(AppCommon.Instance.CodeAutoGenMsg);
                w.WriteLine(AppCommon.Instance.CodeLicenseHeader);

               
                w.WriteLine(@"; Super Global ");
                w.Write(@"Global AS_AppDatapath := ");
                w.Write(q);
                if (this.m_CurrentInstal.plugin != null)
                {
                    w.Write(this.DataItemPath);
                }
                w.WriteLine(q);

                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.NoError;
                }
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.PlugingHotstringWriteGeneralFail);
                return;
            }
        }

        
        internal void WriteDataFiles()
        {
            try
            {
                string sPath = this.CurrentInstal.profile.codeLanguage.paths.DataItemsFullPath;
                try
                {
                    if (Directory.Exists(sPath) == false)
                    {
                        Directory.CreateDirectory(sPath);
                    }
                }
                catch (Exception)
                {
                    this.SetExitReason(ExitCodeEnum.DataFolderFail);
                    throw;
                }
               

                foreach (var p in this.CurrentInstal.plugin)
                {
                    if (p.enabled == false)
                    {
                        continue;
                    }

                    if (this.ScriptMinVersion < p.FullMinVersion)
                    {
                        // The Script version is not high enough to run this plugin.
                        continue;
                    }


                    this.WritePlugin(p);
                }

              
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.DataGenerError);
                return;
            }
        }

        private void WritePlugin(plugin p)
        {
            if (p.dataItems == null)
            {
                return;
            }
            try
            {
                foreach (var item in p.dataItems)
                {
                    string sFile = Path.Combine(this.DataItemPath, item.dataFileName);
                    if ((item.overwrite == false) && (File.Exists(sFile) == true))
                    {
                        continue;
                    }
                    if (File.Exists(sFile))
                    {
                        File.Delete(sFile);
                    }
                    // if there is no file data then jsut contiinue
                    if (string.IsNullOrEmpty(item.dataValue))
                    {
                        continue;
                    }
                    
                    if (item.encoded == true)
                    {
                        Util.FileSaveFromBase64(sFile, item.dataValue);
                    }
                    else
                    {
                        File.WriteAllText(sFile, item.dataValue.CleanNewLine());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region Helper Methods
        /// <summary>
        /// Checks to see if root data folder exist and creats it if needed
        /// </summary>
        /// <returns>
        /// Returns True if folder ewist or was created successfully; Otherwise False
        /// </returns>
        private bool RootFolderCheck()
        {
            if (Directory.Exists(AppCommon.Instance.PathDataFiles))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(AppCommon.Instance.PathDataFiles);
            }
            catch
            {
                this.ExitReason = ExitCodeEnum.DataRootFolderFail;
                return false;
            }
            if (Directory.Exists(AppCommon.Instance.PathDataFiles))
            {
                return true;
            }
            this.ExitReason = ExitCodeEnum.DataRootFolderFail;
            return false;
        }
        #endregion

        #region  Properties

        private SnippitInstal m_CurrentInstal;
        private SnippitInstal CurrentInstal
        {
            get
            {
                if (this.m_CurrentInstal == null)
                {
                    this.m_CurrentInstal = ReadWrite.ReadCurrentInstall();
                }
                return this.m_CurrentInstal;
            }
        }

        private string m_DataItemPath;
        internal string DataItemPath
        {
            get
            {
              return this.m_DataItemPath;
            }
        }
        #endregion
    }
}
