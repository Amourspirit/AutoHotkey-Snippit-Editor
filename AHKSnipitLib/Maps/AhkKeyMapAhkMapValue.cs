using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps
{
    public partial class AhkKeyMapAhkMapValue
    {
        /// <summary>
        /// Parent List
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<AhkKeyMapAhkMapValue> Parent { get; set; }

        /// <summary>
        /// Specifies if <see cref="AhkKeyMapAhkMapValue"/> instance is selected. Mostly used for List
        /// </summary>
        [XmlIgnore()]
        public bool Selected { get; set; } = false;

    }
}
