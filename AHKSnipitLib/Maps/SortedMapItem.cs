using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps
{
    /// <summary>
    /// Represents a Key, Value, sort item with Option to be marked as selected
    /// </summary>
    public partial class SortedMapItem
    {

        /// <summary>
        /// Parent List
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<SortedMapItem> Parent { get; set; }

        /// <summary>
        /// Specifies if <see cref="SortedMapItem"/> instance is selected. Mostly used for List
        /// </summary>
        [XmlIgnore()]
        public bool Selected { get; set; } = false;
    }
}
