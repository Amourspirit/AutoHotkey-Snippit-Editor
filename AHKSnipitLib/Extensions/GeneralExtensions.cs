using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions
{
    /// <summary>
    /// General Extensions
    /// </summary>
    public static class GeneralExtensions
    {
        internal static Regex LineTermRegex = new Regex(
              "\\r\\n|\\n",
           RegexOptions.Compiled
            | RegexOptions.Multiline
           );
        /// <summary>
        /// Replaces any line ending in only new line Terminator with Carriage Return and New Line
        /// </summary>
        /// <param name="str">The String source</param>
        /// <returns>
        /// string with all lines terminated with Carriage Return and New Line
        /// </returns>
        public static string CleanNewLine(this string str)
        {
            if (str.Length == 0)
            {
                return string.Empty;
            }
            string retval = LineTermRegex.Replace(str, Environment.NewLine);
            return retval;
        }
    }
}
