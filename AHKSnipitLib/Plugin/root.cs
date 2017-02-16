using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents a AutoHotkey Snippit plugin
    /// </summary>
    public partial class plugin
    {

        #region Properties
        private string m_hash;
        /// <summary>
        /// Gets a Unique Name for the plugin in MD5 formate.
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
                    sb.AppendLine(this.name);
                    sb.AppendLine(this.enabled.ToString());
                    sb.AppendLine(this.File);
                    sb.AppendLine(this.minVersion);
                    sb.AppendLine(this.version);

                    if (this.ParentProfile != null)
                    {
                        sb.AppendLine(this.ParentProfile.UniqueName);
                    }

                    if (this.commands == null)
                    {
                        sb.AppendLine("nocmd");
                    }
                    else
                    {
                        sb.AppendLine(this.commands.Length.ToString());
                    }

                    if (this.hotstrings == null)
                    {
                        sb.AppendLine("nohs");
                    }
                    else
                    {
                        sb.AppendLine(this.hotstrings.Length.ToString());
                    }


                    if (this.includes == null)
                    {
                        sb.AppendLine("noinc");
                    }
                    else
                    {
                        sb.AppendLine(this.includes.Length.ToString());
                    }


                    this.m_hash = Util.GetHashString(sb.ToString());
                }
                return this.m_hash;
            }
        }

        /// <summary>
        /// Gets/Sets the XML file used to populate the class
        /// </summary>
        [XmlIgnore()]
        public string File { get; set; }
        /// <summary>
        /// The Profile this instance of plugin belongs to.
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public profile ParentProfile { get; set; }

        #region Version
        private System.Version m_FullVersion = null;
        [XmlIgnore()]
        public System.Version FullVersion
        {
            get
            {
                if (this.m_FullVersion == null)
                {
                    if (!System.Version.TryParse(this.version, out this.m_FullVersion))
                    {
                        this.m_FullVersion = AppCommon.Instance.DefaultMinVersion;
                    }
                }
                return this.m_FullVersion;
            }
        }

        private System.Version m_FullMinVersion = null;
        [XmlIgnore()]
        public System.Version FullMinVersion
        {
            get
            {
                if (this.m_FullMinVersion == null)
                {
                    if (!System.Version.TryParse(this.minVersion, out this.m_FullMinVersion))
                    {
                        this.m_FullMinVersion = AppCommon.Instance.DefaultMinVersion;
                    }
                }
                return this.m_FullMinVersion;
            }
        }
        #endregion

        #endregion

        #region Static Methods
        /// <summary>
        /// Creates a new instance of <see cref="root"/> from <paramref name="FileName"/>
        /// </summary>
        /// <param name="FileName">The FileName to the xml that represents the profile</param>
        /// <returns><see cref="root"/> instance</returns>
        /// <exception cref="System.Exception">If there are Validation errors with the xml</exception>
        public static plugin FromFile(string FileName)
        {
            ValidationResult vResult = ValidateXml.ValidatePluginFile(FileName);
            if (vResult.HasErrors == true)
            {
                var ex = new System.Exception(vResult.ToString());
                throw ex;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(plugin));

            System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
            var r = (plugin)serializer.Deserialize(reader);
            reader.Close();
            r.File = FileName;
            return r;
        }
        #endregion

        

       

    }
}
