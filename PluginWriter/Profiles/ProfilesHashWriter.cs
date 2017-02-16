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
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Profiles
{
    /// <summary>
    /// Writes only the values for Proflie that would need a script restart.
    /// </summary>
    internal class ProfilesHashWriter : WriterBase
    {
        public ProfilesHashWriter() : base()
        {
        }

        internal ProfilesHashWriter(SnippitInstal si) : this()
        {
            this.m_CurrentInstal = si;
        }

        #region Properties
        
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

        protected internal override void Write()
        {
            this.ExitReason = ExitCodeEnum.NoError;
            w.WriteLine(this.CurrentInstal.profile.UniqueName);
        }

       
    }
}
