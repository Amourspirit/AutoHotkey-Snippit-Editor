using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions
{
    /// <summary>
    /// AutoHotkey Related Extensions Methods
    /// </summary>
    public static class AutoHotkeyExtensions
    {
        internal static Regex AutoHotKeySymbolRegex = new Regex(
             "([,%{0};])",
           RegexOptions.Compiled
           );
        internal static Regex DoubleQuoteRegex = new Regex(
             "(\")",
           RegexOptions.Compiled
           );
        /// <summary>
        /// Escapes AutoHotkey Symbols and double quotes
        /// </summary>
        /// <param name="str"></param>
        /// <param name="EscapeChar"></param>
        /// <returns></returns>
        public static string AutoHotkeyEscape(this string str, char EscapeChar = '`')
        {
            if (str.Length == 0)
            {
                return string.Empty;
            }
            string retval = AutoHotKeySymbolRegex.Replace(str, EscapeChar.ToString());
            retval = DoubleQuoteRegex.Replace(retval, "\"");
            return retval;
        }
    }
}
