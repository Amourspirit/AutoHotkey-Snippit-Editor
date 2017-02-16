using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class profileWindow : IDataErrorInfo, IComparable, IComparable<profileWindow>
    {


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
        /// Gets/Sets the Parent Command for th Include Hotstring
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public include ParentInclude { get; set; }

        #region IComparable
        /// <summary>
        /// Compares this instance with a specified <see cref="profileWindow"/> and indicates whether this instance 
        /// precedes, follows, or appears in the same position in the sort order as the specified string.
        /// <param name="other">The <see cref="profileWindow"/> to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears
        /// in the same position in the sort order as the <paramref name="other"/> parameter.
        /// </returns>
        /// <remarks>
        /// Compare is done using <see cref="profileWindow.name"/>
        /// </remarks>
        public int CompareTo(profileWindow other)
        {
            return this.name.CompareTo(other.name);
        }

        /// <summary>
        /// Compares this instance with a specified <see cref="profileWindow"/> and indicates whether this instance 
        /// precedes, follows, or appears in the same position in the sort order as the specified string.
        /// <param name="other">The <see cref="profileWindow"/> to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears
        /// in the same position in the sort order as the <paramref name="other"/> parameter.
        /// </returns>
        /// <remarks>
        /// Compare is done using <see cref="profileWindow.name"/>
        /// </remarks>
        public int CompareTo(object other)
        {
            if (other is profileWindow)
            {
                profileWindow obj = (profileWindow)other;
                return this.name.CompareTo(obj.name);
            }
            return 1;
        }
        #endregion

        /// <summary>
        /// Specifies the parent list
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<profileWindow> Parent { get; set; }


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
                if (columnName == @"name")
                {
                    pNmae = PropertyNames.name;
                }
                else if (columnName == @"value")
                {
                    pNmae = PropertyNames.value;
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
        /// Gets the error message
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
            switch (pName)
            {
                // triggers can be of same name with different case
                case PropertyNames.name:
                    if (string.IsNullOrWhiteSpace(this.name))
                    {
                        hasError = true;
                        return Properties.Resources.ErrorValueMutNotBeNull;
                    }
                    break;
                case PropertyNames.value:
                    if (string.IsNullOrWhiteSpace(this.value))
                    {
                        hasError = true;
                        return Properties.Resources.ErrorValueMutNotBeNull;
                    }

                    if (Parent != null && Parent.Any(
                      x => string.Equals(x.value, this.value, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, this)))
                    {
                        hasError = true;
                        return Properties.Resources.ErrorDuplicateNotAllowed;
                    }

                    break;
                default:
                    break;
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
            name = 100,
            value = 101
        }
    }
}
