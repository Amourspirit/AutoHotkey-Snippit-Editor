using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums
{
    /// <summary>
    /// Specify Option of Hotstring Send Method
    /// </summary>
    public enum HotStringSendEnum
    {
        /// <summary>
        /// No Option selected
        /// </summary>
        None,
        /// <summary>
        /// Hotstring Send Method (default)
        /// </summary>
        Send,
        /// <summary>
        /// Hotstring SendInput method
        /// </summary>
        SendInput,
        /// <summary>
        /// Hotstring SendPlay method
        /// </summary>
        SendPlay,
        /// <summary>
        /// Hotstring SendEvent Method
        /// </summary>
        SendEvent
    }
}
