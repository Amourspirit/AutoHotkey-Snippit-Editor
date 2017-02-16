using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums
{
    /// <summary>
    /// Enumeration of the different kinds of available Xml Formats that can be consumed by Application
    /// </summary>
    public enum XmlKindEnum : byte
    {
        /// <summary>
        /// None: Value 0
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents SnippitInstal xml. Value: 1
        /// </summary>
        SnippitInstl = 1,
        /// <summary>
        /// Represents profile xml. Value 2
        /// </summary>
        Profile = 2,
        /// <summary>
        /// Represents Plugin xml. Value 3
        /// </summary>
        Plugin = 3
    }
}
