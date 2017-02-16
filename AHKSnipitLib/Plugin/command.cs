using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents a Hotkey Command
    /// </summary>
    partial class command : IDataErrorInfo
    {
       

        #region Methods
        /// <summary>
        /// Populates instance class with another instance of class, Arrays are Ignored
        /// </summary>
        /// <param name="oth">The other instance to populate from</param>
        public void Populate(command oth)
        {
            Populate(oth, true);

        }

        /// <summary>
        /// Populates instance class with another instance of class
        /// </summary>
        /// <param name="oth">The other instance to populate from</param>
        /// <param name="IgnoreArray">If True then Arrays are Ignored; Otherwise arrays are included</param>
        public void Populate(command oth, bool IgnoreArray)
        {
            foreach (PropertyInfo sourcePropertyInfo in oth.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (sourcePropertyInfo.Name == @"Parent")
                {
                    continue;
                }
                if ((IgnoreArray == true) && (sourcePropertyInfo.PropertyType.IsArray == true))
                {
                    continue;
                }
                PropertyInfo destPropertyInfo = this.GetType().GetProperty(sourcePropertyInfo.Name);

                if (destPropertyInfo.CanWrite == true)
                {
                    destPropertyInfo.SetValue(
                       this,
                       sourcePropertyInfo.GetValue(oth, null),
                       null);
                }
            }

        }
        #endregion

        #region IDataErrorInfo
        /// <summary>
        /// Validates the specified for the column
        /// </summary>
        /// <param name="columnName">The Column to get Validate</param>
        /// <returns>Empty String if validation passes; Otherwise, validation error string</returns>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string this[string columnName]
        {
            get
            {
                return GetResult(columnName);
            }
        }
        /// <summary>
        /// Gets Error string
        /// </summary>
        /// <remarks>
        /// Not used in this class. Returns Empty String
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        private string GetResult(string columnName)
        {
            string retval = string.Empty;
            switch (columnName)
            {
                case "hotkey":
                    retval = ValidateHotkey();
                    break;
                case "name":
                    retval = ValidateName();
                    break;
                case "label":
                    retval = ValidateLabel();
                    break;
                case "code":
                    retval = ValidateCode();
                    break;
                default:
                    break;
            }
                      
            return retval;
        }

        #region Validate Field Methods
        /// <summary>
        /// Method to Validate <see cref="command.hotkey"/> Value
        /// </summary>
        /// <returns>
        /// Returns true if Validation passes; Otherwise, false.
        /// </returns>
        protected string ValidateHotkey()
        {
            if (string.IsNullOrWhiteSpace(this.hotkey))
                return Properties.Resources.ErrorValueMutNotBeNull;

            HotkeyKeys hks;
            if (HotkeyKeys.TryParse(this.hotkey, out hks))
            {
                if (Parent != null && Parent.Any(
                     x => string.Equals(x.hotkey, this.hotkey, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, this)))
                {
                    return Properties.Resources.ErrorDuplicateNotAllowed;
                }
                return string.Empty;
            }
            return Properties.Resources.ErrorInvalidFormat;
                       
        }

        /// <summary>
        /// Method to Validate <see cref="command.name"/> Value
        /// </summary>
        /// <returns>
        /// Returns true if Validation passes; Otherwise, false.
        /// </returns>
        protected string ValidateName()
        {
            if (string.IsNullOrWhiteSpace(this.name))
                return Properties.Resources.ErrorValueMutNotBeNull;

            return string.Empty;
        }

        /// <summary>
        /// Method to Validate <see cref="command.label"/> Value
        /// </summary>
        /// <returns>
        /// Returns true if Validation passes; Otherwise, false.
        /// </returns>
        protected string ValidateLabel()
        {
            if (string.IsNullOrWhiteSpace(this.label))
                return Properties.Resources.ErrorValueMutNotBeNull;

            bool IsMatch = RegularExpressions.LetternNummberUnderscoreRegex.IsMatch(this.label);
            if (IsMatch == false)
                return Properties.Resources.ErrroNumberLettersUnserscore;

            return string.Empty;
        }

        /// <summary>
        /// Method to Validate <see cref="command.code"/> Value
        /// </summary>
        /// <returns>
        /// Returns true if Validation passes; Otherwise, false.
        /// </returns>
        protected string ValidateCode()
        {
            if (string.IsNullOrWhiteSpace(this.code))
                return Properties.Resources.ErrorValueMutNotBeNull;

            return string.Empty;
        }
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Specifies the Parent List the Command is part of
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<command> Parent { get; set; }

        HotkeyKeys m_KeyData;
        /// <summary>
        /// Gets the KeyDaa for the current HotKey
        /// </summary>
        [XmlIgnore(), Browsable(false)]
        public HotkeyKeys KeyData
        {
            get
            {
                if (this.m_KeyData == null)
                {
                    if (!HotkeyKeys.TryParse(this.hotkey, out m_KeyData))
                    {
                        this.m_KeyData = new HotkeyKeys();
                    }
                }
                return this.m_KeyData;
            }
        }
      

        private string m_hash;
        /// <summary>
        /// Gets a Unique Name for the Command in MDF formate.
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
                    sb.Append(this.category);
                    sb.Append(this.enabled.ToString());
                    sb.Append(this.label);
                    sb.Append(this.hotkey);
                    sb.Append(this.type.ToString());
                    // Add the code to unique name. This will force a different name if only
                    // the code has changed. Later this will be used to help decide if the contents
                    // of a plugin are changed for writing to disk.
                    sb.Append(this.code);
                      
                    this.m_hash = Util.GetHashString(sb.ToString());
                }
                return this.m_hash;
            }
        }
        #endregion

    }
}
