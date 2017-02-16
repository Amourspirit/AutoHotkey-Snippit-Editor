using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents an Include Plugin type for AutoHotkey
    /// </summary>
    public partial class include
    {
        #region Properties
        /// <summary>
        /// Specifies the Parent List the Command is part of
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<include> Parent { get; set; }

        private string m_hash;
        /// <summary>
        /// Gets a Unique Name for the Include in MDF formate.
        /// </summary>
        /// <remarks>
        /// UniqueName is valid for file names.
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string UniqueName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_hash))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(this.name);
                    sb.Append(this.snippit);
                    sb.Append(this.enabled.ToString());
                    // Add the code to unique name. This will force a different name if only
                    // the code has changed. Later this will be used to help decide if the contents
                    // of a plugin are changed for writing to disk.
                    sb.Append(this.code);
                    if (this.hotstrings != null)
                    {
                        sb.Append(this.hotstrings.Length);
                    }
                    if (this.commands != null)
                    {
                        sb.Append(this.commands.Length);
                    }

                    this.m_hash = Util.GetHashString(sb.ToString());
                }
                return this.m_hash;
            }
        }
        #endregion
    }
}
