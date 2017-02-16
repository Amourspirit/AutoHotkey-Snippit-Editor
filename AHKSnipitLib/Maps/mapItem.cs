using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps
{
    /// <summary>
    /// Represents a Key - Value item with Option to be marked as selected
    /// </summary>
    public partial class mapItem
    {
        /// <summary>
        /// Parent List
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<mapItem> Parent { get; set; }

        /// <summary>
        /// Specifies if <see cref="mapItem"/> instance is selected. Mostly used for List
        /// </summary>
        [XmlIgnore()]
        public bool Selected { get; set; } = false;

    }
}
