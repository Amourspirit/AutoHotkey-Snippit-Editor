using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents Include type Pluging Commmand Hotkey Descriptiong Item
    /// </summary>
    public partial class includeCommand : IComparable, IComparable<includeCommand>, IDataErrorInfo
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inCmd">An Instance of <see cref="includeCommand"/> to populate this instance with</param>
        public includeCommand(includeCommand inCmd) : this()
        {
            this.category = inCmd.category;
            this.description = inCmd.description;
            this.name = inCmd.name;
            this.hotkey = inCmd.hotkey;
        }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inCmd">An Instance of <see cref="includeCommand"/> to populate this instance with</param>
        /// <param name="FileName">The XML file name associated with instance</param>
        public includeCommand(includeCommand inCmd, string FileName) : this(inCmd)
        {
            this.File = FileName;
        }
        #endregion

        private HashSet<string> ErrorProperties = new HashSet<string>();

        /// <summary>
        /// Property that keeps track of the number of errors
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IErrorCount Errors { get; set; }

        /// <summary>
        /// Gets the current Number of errors for the instance
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public int ErrorCount
        {
            get
            {
                return ErrorProperties.Count;
            }
        }

        /// <summary>
        /// Gets/Sets the Parent Command for th Include Command
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public include ParentInclude { get; set; }

        /// <summary>
        /// Specifies the Parent List the Command is part of
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<includeCommand> Parent { get; set; }

        /// <summary>
        /// Specifies the XML file associated with instance
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string File { get; set; }

        #region IComparable
        public int CompareTo(includeCommand other)
        {
            return this.name.CompareTo(other.name);
        }

        public int CompareTo(object obj)
        {
            if (obj is includeCommand)
            {
                includeCommand oth = (includeCommand)obj;
                return this.name.CompareTo(oth.name);
            }
            return 1;
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

                PropertyNames pNmae = PropertyNames.None;
                if (columnName == @"hotkey")
                {
                    pNmae = PropertyNames.hotkey;
                }
                else if (columnName == @"name")
                {
                    pNmae = PropertyNames.name;
                }
                bool hasError = false;
                this.m_error = GetResult(pNmae, out hasError);
                if (pNmae > PropertyNames.None)
                {
                    if (hasError == false)
                    {
                        if (this.ErrorProperties.Contains(columnName))
                        {
                            this.ErrorProperties.Remove(columnName);
                            if (this.Errors != null)
                            {
                                this.Errors.ErrorCount--;
                            }
                        }
                    }
                    else
                    {
                        if (this.ErrorProperties.Contains(columnName) == false)
                        {
                            this.ErrorProperties.Add(columnName);
                            if (this.Errors != null)
                            {
                                this.Errors.ErrorCount++;
                            }
                        }
                    }
                }
               
             
                return this.m_error;
            }
        }

        private string m_error = string.Empty;
        /// <summary>
        /// Gets Error message
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string Error
        {
            get
            {
                return this.m_error;
            }
        }

        /// <summary>
        /// Gets error Text of any Property error
        /// </summary>
        /// <param name="pName">The Property</param>
        /// <param name="hasError">True if the property has an error; Otherwise, false</param>
        /// <returns>
        /// String containing error message if the property has an error; Otherwise, Empty string.
        /// </returns>
        private string GetResult(PropertyNames pName, out bool hasError)
        {
            hasError = false;
            if (pName == PropertyNames.hotkey)
            {
                if (string.IsNullOrWhiteSpace(this.hotkey))
                {
                    hasError = true;
                    return Properties.Resources.ErrorValueMutNotBeNull;
                }
                    

                HotkeyKeys hks;
                if (HotkeyKeys.TryParse(this.hotkey, out hks))
                {
                    if (Parent != null && Parent.Any(
                        x => string.Equals(x.hotkey, this.hotkey, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, this)))
                    {
                        hasError = true;
                        return Properties.Resources.ErrorDuplicateNotAllowed;
                    }
                    return string.Empty;
                }
                hasError = true;
                return Properties.Resources.ErrorInvalidFormat;

            }
            else if (pName == PropertyNames.name)
            {
                if (string.IsNullOrWhiteSpace(this.name))
                {
                    hasError = true;
                    return Properties.Resources.ErrorValueMutNotBeNull;
                }
            }

            return string.Empty;
        }
        #endregion

        /// <summary>
        /// Clears all errors for the instance and Removes error count from <see cref="Errors"/>
        /// </summary>
        public void ClearErrors()
        {
            if (this.ErrorCount > 0)
            {
                if (this.Errors != null)
                {
                    this.Errors.ErrorCount -= this.ErrorCount;
                }
                this.ErrorProperties.Clear();
            }
        }

        private enum PropertyNames : byte
        {
            None = 0,
            hotkey = 100,
            name = 101
        }
    }
}
