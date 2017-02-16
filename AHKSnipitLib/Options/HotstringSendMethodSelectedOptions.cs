using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Gan instance of the Selected Options for <see cref="Enums.HotStringSendEnum"/>;
    /// </summary>
    public class HotstringSendMethodSelectedOptions : SelectedOptions<Enums.HotStringSendEnum>
    {
        #region Parse
        /// <summary>
        /// Gets a Instance of <see cref="HotstringSendMethodSelectedOptions"/> From a String value
        /// </summary>
        /// <param name="s">The String to parse for values</param>
        /// <returns>
        /// Instance of <see cref="HotstringSendMethodSelectedOptions"/> if Parse succeeded; Otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public new static HotstringSendMethodSelectedOptions Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }
            try
            {
                // all chars except for `n and `t are single chars
                var opt = new HotstringSendMethodSelectedOptions();
                var so = SelectedOptions<Enums.HotStringSendEnum>.Parse(s);
                opt.Keys = so.Keys;
                opt.Options = so.Options;
                return opt;
             
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Attempts to Parse a string representing one or more Options.
        /// </summary>
        /// <param name="s">The Options to Parse</param>
        /// <param name="ec">The <see cref="HotstringSendMethodSelectedOptions"/> that represents the parsed results</param>
        /// <returns>
        /// Returns True if the parse was successful; Otherwise false
        /// </returns>
        public static bool TryParse(string s, out HotstringSendMethodSelectedOptions ec)
        {
            bool retval = false;
            HotstringSendMethodSelectedOptions outec = null;
            try
            {
                HotstringSendMethodSelectedOptions ecs = HotstringSendMethodSelectedOptions.Parse(s);
                outec = ecs;
                retval = true;
            }
            catch (Exception)
            {
            }
            ec = outec;
            return retval;
        }
        #endregion

        #region FromArray
        /// <summary>
        /// Creates a new instance of <see cref="HotstringSendMethodSelectedOptions"/> populated with the 
        /// values in <paramref name="Options"/>
        /// </summary>
        /// <param name="Options">The Array of Enum Values use to populate the instance</param>
        /// <returns>
        /// Instance of <see cref="HotstringSendMethodSelectedOptions"/>. If <paramref name="Options"/> is null
        /// then return instance will have no values.
        /// </returns>
        public new HotstringSendMethodSelectedOptions FromArray(Enums.HotStringSendEnum[] Options)
        {
            var so = SelectedOptions<Enums.HotStringSendEnum>.FromArray(Options);
            HotstringSendMethodSelectedOptions hs = new HotstringSendMethodSelectedOptions();
            hs.Keys = so.Keys;
            hs.Options = so.Options;
            
            return hs;
        }
        #endregion

        /// <summary>
        /// Gets The AutoHotkey String representation of the options
        /// </summary>
        /// <returns>
        /// String of Options if Count greater then zero; Otherwise empty string.
        /// </returns>
        public string ToAutoHotKeyString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            
            foreach (var item in this.Options)
            {
                if (item == Enums.HotStringSendEnum.None)
                {
                    continue;
                }
                string AhkValue = item.GetAutoHotKeySynbol();
                if (string.IsNullOrEmpty(AhkValue))
                {
                    continue;
                }
                sb.Append(AhkValue);
               
            }
            return sb.ToString();
        }
    }
}
