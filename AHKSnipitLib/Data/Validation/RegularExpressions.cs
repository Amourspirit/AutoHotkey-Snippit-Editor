using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation
{
    /// <summary>
    /// Regular Expression use for Data Validation in Various Methods
    /// </summary>
    public static class RegularExpressions
    {
        /// <summary>
        /// Regular Expression for testing Strings Representing Version such as '2.3.1.0'
        /// </summary>
        public static readonly Regex VersionRegex;

        /// <summary>
        /// Regular Expression to For Hotkey matching function key triggers such as '!^F3'
        /// </summary>
        public static readonly Regex HotkeyRegex;

        /// <summary>
        /// Regular Expression for Hotkey With one or two chars such as '#rt' or '!^b'
        /// </summary>
        public static readonly Regex HotKeyOneOrTwoCharsRegex;

        /// <summary>
        /// Regular expression for String with Only Letters, Numbers and Underscore characters
        /// </summary>
        public static Regex LetternNummberUnderscoreRegex;

        static RegularExpressions()
        {
            VersionRegex = new Regex(
                Properties.Settings.Default.RegexVersion,
                    RegexOptions.Singleline
                    | RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                );

            LetternNummberUnderscoreRegex = new Regex(
                Properties.Settings.Default.RegexLetterNumberUnderscore,
                    RegexOptions.Singleline
                    | RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                );

            HotkeyRegex = new Regex(
                 Properties.Settings.Default.RegexHotstring,
                    RegexOptions.Singleline
                    | RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    | RegexOptions.IgnoreCase
                    );

           

        }
    }
}
