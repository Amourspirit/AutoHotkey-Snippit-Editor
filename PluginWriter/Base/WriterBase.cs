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
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base
{
    /// <summary>
    /// Writer Base Class for writing code.
    /// </summary>
    internal abstract class WriterBase
    {
        internal WriterBase()
        {
            baseTextWriter = new System.IO.StringWriter();
            w = new IndentedTextWriter(baseTextWriter);
            this.ScriptMinVersion = IniHelper.GetScriptVersion();
            this.HashBuilder = new StringBuilder();
        }

        #region Constants
        protected const char q = '\u0022';
        protected const char bo = '{';
        protected const char bc = '}';
        #endregion

        #region Properties
        /// <summary>
        /// Specifies the reason for exiting instance.
        /// </summary>
        public ExitCodeEnum ExitReason { get; set; } = ExitCodeEnum.NotSet;
        /// <summary>
        /// Specifies the Minimum Version for the Script
        /// </summary>
        public Version ScriptMinVersion { get; set; }
        #endregion

        #region Fields/Members
        /// <summary>
        /// Base writer for <see cref="w"/>
        /// </summary>
        protected System.IO.StringWriter baseTextWriter;
        /// <summary>
        /// Writer to write all output into.
        /// </summary>
        protected IndentedTextWriter w;
        /// <summary>
        /// String Builder for adding Hash Names of various Unique Names.
        /// </summary>
        protected StringBuilder HashBuilder;
        #endregion


        protected internal abstract void Write();


        public override string ToString()
        {
            return this.baseTextWriter.ToString();
        }

        /// <summary>
        /// Gets A Hash on the current Contents
        /// </summary>
        /// <returns></returns>
        protected internal virtual string GetHash()
        {
            string contents = this.baseTextWriter.ToString();
            return Util.GetHashString(contents);
        }

        /// <summary>
        /// Gets A Hash on the current Hash Builder Contents
        /// </summary>
        /// <returns></returns>
        protected internal virtual string GeHashBuildertHash()
        {
            string contents = this.HashBuilder.ToString();
            return Util.GetHashString(contents);
        }

        /// <summary>
        /// Sets the reason for exiting
        /// </summary>
        /// <param name="Reason">The Enum value of the reason to set</param>
        /// <returns>
        /// Returns true if a new reason has be set; Otherwise false.
        /// </returns>
        /// <remarks>
        /// Will only set <paramref name="Reason"/> if the current <see cref="WriterBase.ExitReason"/> is not set.
        /// </remarks>
        protected bool SetExitReason(ExitCodeEnum Reason)
        {
            if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
            {
                this.ExitReason = Reason;
                return true;
            }
            return false;
        }
    }
}
