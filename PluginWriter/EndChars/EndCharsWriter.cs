using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.EndChars
{
    internal class EndCharsWriter : WriterBase
    {
        #region Constructor
        internal EndCharsWriter() : base()
        {
            this.m_EndCharsIncludeFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.IncludeEndCharsFile);

        }
        internal EndCharsWriter(profile p) : this()
        {
            this.m_CurrentProfile = p;
        }
        #endregion;

       
        #region Properties
       

        private string m_EndCharsIncludeFile;
        /// <summary>
        /// Specifies the Include file for the End Chars in HotHotkey.
        /// </summary>
        public string EndCharsIncludeFile
        {
            get
            {
                return m_EndCharsIncludeFile;
            }
        }
        private profile m_CurrentProfile;
        private profile CurrentProfile
        {
            get
            {
                if (this.m_CurrentProfile == null)
                {
                    this.m_CurrentProfile = ReadWrite.ReadCurrentProfile();
                }
                return this.m_CurrentProfile;
            }
        }
        #endregion

        #region Write Methods
        protected internal override void Write()
        {
            try
            {
                var ScriptVer = IniHelper.GetScriptVersion();
                if (ScriptVer < this.CurrentProfile.FullMinVersion)
                {
                    this.ExitReason = ExitCodeEnum.MinVersionError;
                    return;
                }

                w.WriteLine(AppCommon.Instance.CodeAutoGenMsg);
                w.WriteLine(AppCommon.Instance.CodeLicenseHeader);
                if (this.CurrentProfile.profileEndChars != null && this.CurrentProfile.profileEndChars.Length > 0)
                {
                    EndCharsSelectedOptions EndingChars = (EndCharsSelectedOptions)EndCharsSelectedOptions.FromArray(this.CurrentProfile.profileEndChars);

                    if (EndingChars.Count > 0)
                    {
                        w.Write("#Hotstring EndChars ");
                        w.WriteLine(EndingChars.ToAutoHotkeyString());
                    }

                }
                this.ExitReason = ExitCodeEnum.NoError;
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                {
                    this.ExitReason = ExitCodeEnum.EndCharsGeneralError;
                }
                return;
            }
            
        }
        #endregion

        
    }
}
