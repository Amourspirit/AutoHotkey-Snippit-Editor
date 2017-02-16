using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;
using System.Reflection;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents an Include Plugin type for AutoHotkey
    /// </summary>
    public partial class dataItem
    {
        #region Properties
        /// <summary>
        /// Specifies the Parent List the Command is part of
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<dataItem> Parent { get; set; }

        #endregion
       
    }
}
