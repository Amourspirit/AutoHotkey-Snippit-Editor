using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Include
{
    internal class IncludeWriter : WriterBase
    {

        #region Constructor
        internal IncludeWriter() : base()
        {
            this.m_PluginIncludFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.PluginFile);

        }
        internal IncludeWriter(SnippitInstal si) : this()
        {
            this.m_CurrentInstal = si;
        }
        #endregion;

        

        #region Properties
      

        private string m_PluginIncludFile;
        /// <summary>
        /// Specifies the Include file for the End Chars in HotHotkey.
        /// </summary>
        public string PluginIncludFile
        {
            get
            {
                return m_PluginIncludFile;
            }
        }
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
        #endregion
        #region Write Methods
        
        protected internal override void Write()
        {
            
            try
            {
                if (this.CurrentInstal == null)
                {
                    this.ExitReason = ExitCodeEnum.NoInstall;
                    return;
                }

                if (this.m_CurrentInstal.profile == null)
                {
                    this.ExitReason = ExitCodeEnum.NoProfile;
                    return;
                }
               
                
                if (this.ScriptMinVersion < this.CurrentInstal.profile.FullMinVersion)
                {
                    this.ExitReason = ExitCodeEnum.MinVersionError;
                    return;
                }

                w.WriteLine(AppCommon.Instance.CodeAutoGenMsg);
                w.WriteLine(AppCommon.Instance.CodeLicenseHeader);

                w.Write("; Profile Hash:");
                w.WriteLine(this.CurrentInstal.profile.UniqueName);

                //string PluginKeyFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.PluginKeysFile);
                //w.Write(@"#Include *I ");
                //w.WriteLine(PluginIncludFile);

                if (this.m_CurrentInstal.plugin == null)
                {
                    this.SetExitReason(ExitCodeEnum.NoPlugins);
                    return;
                }
                string sPath = this.m_CurrentInstal.profile.codeLanguage.paths.PluginFullPath;
                string sPathInc = this.m_CurrentInstal.profile.codeLanguage.paths.PluginIncludeFullPath;
                foreach (var p in this.m_CurrentInstal.plugin)
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
                   
                    HashBuilder.AppendLine(p.UniqueName);

                    if (p.commands != null)
                    {
                        foreach (var cmd in p.commands)
                        {
                            if (cmd.enabled == false)
                            {
                                continue;
                            }
                           
                            if (cmd.type == commandType.include)
                            {
                                string sFile = cmd.UniqueName + ".ahk";
                                w.Write(@"#Include *I ");
                                w.WriteLine(Path.Combine(sPath, sFile));
                                
                            }
                        }
                        
                    }
                    if (p.includes != null)
                    {
                        foreach (var inc in p.includes)
                        {
                            if (inc.enabled == false)
                            {
                                continue;
                            }
                            string sFile = inc.UniqueName + ".ahk";
                            w.Write(@"#Include *I ");
                            w.WriteLine(Path.Combine(sPathInc, sFile));
                            
                        }
                    }
                } // foreach (var p in this.m_CurrentInstal.plugin)

                if (HashBuilder.Length > 0)
                {
                    w.WriteLine();
                    w.Write("; Plugin Hash Values:");
                    w.WriteLine(this.GeHashBuildertHash());
                }
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.IwGeneralFail);
             }

            if (this.ExitReason == ExitCodeEnum.NotSet)
            {
                this.ExitReason = ExitCodeEnum.NoError;
            }


        }
        #endregion

        internal void SaveIncludePlugins()
        {
            try
            {
                if (this.CurrentInstal == null)
                {
                    this.ExitReason = ExitCodeEnum.NoInstall;
                    return;
                }
                if (this.CurrentInstal.profile == null)
                {
                    this.ExitReason = ExitCodeEnum.NoProfile;
                    return;
                }
                if (this.CurrentInstal.plugin == null)
                {
                    return;
                }
                
                string sPath = this.CurrentInstal.profile.codeLanguage.paths.PluginIncludeFullPath;
                if (Directory.Exists(sPath))
                {
                    string searchP = "*.ahk";
                    var files = Directory.GetFiles(sPath, searchP);
                    Parallel.ForEach(files, f => {
                        File.Delete(f);
                    }) ;
                   
                }
                else
                {
                    Directory.CreateDirectory(sPath);
                }
                foreach (var p in this.m_CurrentInstal.plugin)
                {
                    if (p.enabled == false)
                    {
                        continue;
                    }
                    if (p.includes == null)
                    {
                        continue;
                    }
                    try
                    {
                        Parallel.ForEach(p.includes, inc =>
                        {
                            if (inc.enabled == true)
                            {
                                string sFile = inc.UniqueName + ".ahk";
                                string sFilePath = Path.Combine(sPath, sFile);
                                string code = inc.code.CleanNewLine();
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine(AppCommon.Instance.CodeAutoGenMsg);
                                sb.AppendLine(AppCommon.Instance.CodeLicenseHeader);
                                sb.Append(code);
                                File.WriteAllText(sFilePath, sb.ToString(), Encoding.UTF8);
                            }
                        });
                    }
                    catch (Exception)
                    {
                        if (this.ExitReason == ExitCodeEnum.NoError || this.ExitReason == ExitCodeEnum.NotSet)
                        {
                            this.ExitReason = ExitCodeEnum.FileWriteError;
                        }
                        throw;
                    }
                   }

            }
            catch (Exception)
            {
                this.ExitReason = ExitCodeEnum.IwSaveIncludePlugins;
            }
        }
    }
}
