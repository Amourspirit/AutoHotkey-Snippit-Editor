using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class itemType : IDataErrorInfo
    {

        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<itemType> Parent { get; set; }

        #region Methods
        /// <summary>
        /// Popululates instance class with another instance of class, Arrays are Ignored
        /// </summary>
        /// <param name="h">The other instance to populate from</param>
        //public void Populate(itemType oth)
        //{
        //    Populate(oth, true);

        //}

        /// <summary>
        /// Popululates instance class with another instance of class
        /// </summary>
        /// <param name="c">The other instance to populate from</param>
        /// <param name="IgnoreArray">If True then Arrays are Ignored; Otherwise arrays are included</param>
        //public void Populate(itemType oth, bool IgnoreArray)
        //{
        //    foreach (PropertyInfo sourcePropertyInfo in oth.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //    {
        //        if (sourcePropertyInfo.Name == @"Parent")
        //        {
        //            continue;
        //        }
        //        if ((IgnoreArray == true) && (sourcePropertyInfo.PropertyType.IsArray == true))
        //        {
        //            continue;
        //        }
        //        PropertyInfo destPropertyInfo = this.GetType().GetProperty(sourcePropertyInfo.Name);

        //        if (destPropertyInfo.CanWrite == true)
        //        {
        //            destPropertyInfo.SetValue(
        //               this,
        //               sourcePropertyInfo.GetValue(oth, null),
        //               null);
        //        }
               
        //    }

        //}
        #endregion

        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                return this.GetResult(columnName);
            }
        }
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
            // column default is an enum that only has one value of @true
            // enums can not be null value so we use defaultSpecified. If DefaultSpecified is
            // true then that the Value property of that class will be considered the default
            // of the IList. There can only be on valid default value for any list.
            //
            // Value property must also be unique within the list. No Duplicates allowed
            PropertyInfo pi = this.GetType().GetProperty(columnName);
            if (pi != null)
            {
                if (columnName == @"Value")
                {
                    string strName = pi.GetValue(this, null) as string;
                    if (string.IsNullOrWhiteSpace(strName))
                        return string.Format(Properties.Resources.ValueHasToBeSet, @"Value");
                }
                else if (columnName == @"default")
                {
                    defaultType dt = (defaultType)pi.GetValue(this, null);
                    if (dt == defaultType.@true && Parent != null && Parent.Any(
                        x => x.@default == defaultType.@true && !ReferenceEquals(x, this)))
                    {
                        return Properties.Resources.ErrorItemTypeDefault;
                    }
                }
                else if(columnName == @"defaultSpecified")
                {
                    bool bvalue = (bool)pi.GetValue(this, null);
                    if (bvalue == true && Parent != null && Parent.Any(
                        x => x.defaultSpecified == true && !ReferenceEquals(x, this)))
                    {
                        return Properties.Resources.ErrorItemTypeDefault;
                    }

                }
               

            }
            if (columnName == @"Value" && Parent != null && Parent.Any(
                   x => string.Equals(x.Value, this.Value, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, this)))
            {
                return Properties.Resources.ErrorDuplicateNotAllowed;
            } 
            return string.Empty;
        }
        #endregion

    }
}
