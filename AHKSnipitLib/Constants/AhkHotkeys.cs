using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Constants
{
    /// <summary>
    /// Constant Values of AutoHotkey Modifiers and Combiners
    /// </summary>
    public sealed class AhkHotkeys
    {
        /// <summary>
        /// AutoHotkey Modifier Win
        /// </summary>
        public const char Win = '#';
        /// <summary>
        /// AutoHotkey Modifier Shift
        /// </summary>
        public const char Shift = '+';
        /// <summary>
        /// AutoHotkey Modifier Alt
        /// </summary>
        public const char Alt = '!';
        /// <summary>
        /// AutoHotkey Modifier Ctrl
        /// </summary>
        public const char Ctrl = '^';
        /// <summary>
        /// AutoHotkey Modifier Wildcard
        /// </summary>
        public const char WildCard = '*';
        /// <summary>
        /// AutoHotkey Modifier Right
        /// </summary>
        public const char Right = '>';
        /// <summary>
        /// AutoHotkey Modifier Left
        /// </summary>
        public const char Left = '<';
        /// <summary>
        /// AutoHotkey Key Combine Char
        /// </summary>
        public const char Combine = '&';
        /// <summary>
        /// AutoHotkey Modifier Native Block
        /// </summary>
        public const char NativeBlock = '~';
        /// <summary>
        /// AutoHotkey Escape Char
        /// </summary>
        public const char Escape = '`';
        /// <summary>
        /// AutoHotkey Install keyboard hook
        /// </summary>
        public const char InstallHook = '$';
        /// <summary>
        /// AutoHotkey Modifier Combine Keys with correct spacing
        /// </summary>
        public const string CombineString = @" & ";
        /// <summary>
        /// AutoHotkey Hotkey UP Modifier with correct Spacing
        /// </summary>
        public const string UpString = @" UP";
    }
}
