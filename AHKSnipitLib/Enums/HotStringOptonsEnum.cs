using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums
{
    /// <summary>
    /// Enum of Options for Hotstrings
    /// </summary>
    public enum HotStringOptionsEnum
    {
        /// <summary>
        /// The hotstring will be triggered even when it is inside another word
        /// </summary>
        TriggerInside,
        /// <summary>
        /// Automatic backspacing is not done to erase the abbreviation you type.
        /// </summary>
        AutomaticBackSpaceOff,
        /// <summary>
        /// When you type an abbreviation, it must exactly match the case defined in the script.
        /// </summary>
        CaseSensitive,
        /// <summary>
        /// Do not conform to typed case
        /// </summary>
        NoConform,
        /// <summary>
        /// Omit the ending character of auto-replace hotstrings when the replacement is produced
        /// </summary>
        OmitEndChar,
        /// <summary>
        /// Send the replacement text raw; that is, exactly as it appears rather than translating {Enter} to an ENTER keystroke, ^c to Control-C, etc
        /// </summary>
        SendRaw,
        /// <summary>
        /// Send using SendInput mode
        /// </summary>
        SendInput,
        /// <summary>
        /// Send using SendPlay mode
        /// </summary>
        SendPlay,
        /// <summary>
        /// Send using SendEvent Mode
        /// </summary>
        SendEvent,
        /// <summary>
        /// Resets the hotstring recognizer after each triggering of the hotstring.
        /// </summary>
        ResetRecognizer
    }
}
