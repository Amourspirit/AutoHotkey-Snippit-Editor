using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents a AutoHotkey Snippit profile
    /// </summary>
    public partial class profile
    {
        #region Properties
        /// <summary>
        /// Gets/Sets the file to the XML for the current profile instance
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string File { get; set; }

        private string m_hash;

        /// <summary>
        /// Gets a Unique Name for the profile in MD5 formate.
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
                    sb.AppendLine(this.globalProfile.ToString());
                    sb.AppendLine(this.codeLanguage.codeName);
                    sb.AppendLine(this.File);
                    sb.AppendLine(this.minVersion);
                    
                    sb.AppendLine(this.version);
                    sb.AppendLine(this.codeLanguage.paths.MainDataFullPath);
                    sb.AppendLine(this.EndCharOptions.ToString());
                    if (this.windows != null)
                    {
                        foreach (var window in this.windows)
                        {
                            sb.AppendLine(window.value);
                        }
                    }


                    this.m_hash = Util.GetHashString(sb.ToString());
                }
                return this.m_hash;
            }
        }

        EndCharsSelectedOptions m_EndCharOptions;
        /// <summary>
        /// Gets the <see cref="EndCharsSelectedOptions"/> for the current instance of <see cref="profile"/>
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        EndCharsSelectedOptions EndCharOptions
        {
            get
            {
                if (m_EndCharOptions == null)
                {
                    if (this.profileEndChars == null || this.profileEndChars.Length == 0)
                    {
                        this.m_EndCharOptions = new EndCharsSelectedOptions();
                    }
                    else
                    {
                        this.m_EndCharOptions = EndCharsSelectedOptions.FromArray(this.profileEndChars);
                    }
                }
                return m_EndCharOptions;
            }
        }


        #endregion

        #region Version
        private System.Version m_FullVersion = null;
        /// <summary>
        /// Get The Version of the Current Profile
        /// </summary>
        [XmlIgnore(), Bindable(false)]
        public System.Version FullVersion
        {
            get
            {
                if (this.m_FullVersion == null)
                {
                    if (! System.Version.TryParse(this.version, out this.m_FullVersion))
                    {
                        this.m_FullVersion = new System.Version();
                    }
                }
                return this.m_FullVersion;
            }
        }

        private System.Version m_FullMinVersion = null;
        /// <summary>
        /// Gets the Minimum Version the Current Profile Can Run on.
        /// </summary>
        [XmlIgnore(), Bindable(false)]
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

        #region Methods
        /// <summary>
        /// Populate instance class with another instance of class
        /// </summary>
        /// <param name="p">The other instance to populate from</param>
        public void Populate(profile p)
        {
            foreach (PropertyInfo sourcePropertyInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo destPropertyInfo = this.GetType().GetProperty(sourcePropertyInfo.Name);

                if (destPropertyInfo.CanWrite == true)
                {
                    destPropertyInfo.SetValue(
                       this,
                       sourcePropertyInfo.GetValue(p, null),
                       null);
                }
            }
                     
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Creates a new instance of <see cref="profile"/> from <paramref name="FileName"/>
        /// </summary>
        /// <param name="FileName">The FileName to the xml that represents the profile</param>
        /// <returns><see cref="profile"/> instance</returns>
        /// <exception cref="System.Exception">If there are Validation errors with the xml</exception>
        public static profile FromFile(string FileName)
        {
            ValidationResult vResult = ValidateXml.ValidateProfileFile(FileName);
            if (vResult.HasErrors == true)
            {
                var ex = new System.Exception(vResult.ToString());
                throw ex;
            }
           
            XmlSerializer serializer = new XmlSerializer(typeof(profile));

            System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
            var p = (profile)serializer.Deserialize(reader);
            reader.Close();
            p.File = FileName;
            return p;
        }

        /// <summary>
        /// Creates a new instance of <see cref="profile"/> from <paramref name="xml"/>
        /// </summary>
        /// <param name="xml">The xml text that represents the profile</param>
        /// <returns><see cref="profile"/> instance</returns>
        /// <exception cref="System.Exception">If there are Validation errors with the xml</exception>
        public static profile FromXml(string xml)
        {
            ValidationResult vResult = ValidateXml.ValidateProfileXml(xml);
            if (vResult.HasErrors == true)
            {
                var ex = new System.Exception(vResult.ToString());
                throw ex;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(profile));

            var reader = new System.IO.StringReader(xml);
            var p = (profile)serializer.Deserialize(reader);
            reader.Close();
            return p;
        }

        #endregion

        #region Validation
        /// <summary>
        /// Gets if the current instance of <see cref="profile"/> is valid and contains expected data
        /// </summary>
        /// <returns>
        /// True if Valid; Otherwise false
        /// </returns>
        /// <remarks>This method is not exhaustive and is intended as a quick check for the basics that are required</remarks>
        public bool GetIsValid()
        {
            bool bValid = true;
            bValid &= !string.IsNullOrEmpty(this.name);
            bValid &= !string.IsNullOrEmpty(this.version);
            bValid &= !string.IsNullOrEmpty(this.minVersion);
            bValid &= this.codeLanguage != null;
            return bValid;
        }
        #endregion
    }
}
