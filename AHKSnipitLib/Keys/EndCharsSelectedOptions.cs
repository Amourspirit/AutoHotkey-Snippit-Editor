using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys
{
    /// <summary>
    /// Represents a group of EndChars Used in AutoHotkey Hotstrings
    /// </summary>
    public class EndCharsSelectedOptions : SelectedOptions<EndCharsEnum>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public EndCharsSelectedOptions()
            : base()
        {

        }
        
        /// <summary>
        /// Gets the EndChars in AutoHotkey Format
        /// </summary>
        /// <returns>
        /// String of Chars escaped for AutoHotkey. If no chars then Empty Sting.
        /// </returns>
        public string ToAutoHotkeyString()
        {
            if (this.Options.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            if (this.HasKey[EndCharsEnum.Space.ToString()] == true)
            {
                var strSpace = EndCharsEnum.Space.GetAutoHotKeySynbol();
                sb.Append(strSpace);
            }

            foreach (var e in this.Options)
            {
                if (e == EndCharsEnum.Space)
                {
                    continue;
                }
                var itm = e.GetAutoHotKeySynbol();
                if (string.IsNullOrEmpty(itm) == false)
                {
                    sb.Append(itm);
                }
            }
            // Space has to be given a little extra consideration. If Space is the only
            // char then it needs to have a escape appended to it. space will always be
            // the first char in the return string if it is set
            if (this.Count == 1 && this.HasKey[EndCharsEnum.Space.ToString()] == true)
            {
                sb.Append(Constants.AhkHotkeys.Escape);
            }
            return sb.ToString();
        }

        #region Overrides

        /// <summary>
        /// Gets a delimited string of Chars, string is made up of <see cref="EndCharsEnum"/> Value Names
        /// </summary>
        /// <returns>
        /// String of Delimited <see cref="EndCharsEnum"/> Names if Count is greater zero;
        /// Otherwise, Empty String
        /// </returns>
        /// <remarks>Output is in the format of BackQuote,Colon,CurlyBraceOpen,CurlyBraceClose</remarks>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region List
        /// <summary>
        /// Gets An array of all the End Chars in current instance
        /// </summary>
        /// <returns>
        /// Array of <see cref="EndCharsEnum"/> or if no values are present then Empty Array
        /// </returns>
        public override EndCharsEnum[] ToArray()
        {
            return base.ToArray();
        }

        /// <summary>
        /// Creates a new instance of <see cref="EndCharsSelectedOptions"/> populated with the 
        /// values in <paramref name="EndChars"/>
        /// </summary>
        /// <param name="EndChars">The Array use to populate the instance</param>
        /// <returns>
        /// Instance of <see cref="EndCharsSelectedOptions"/>. If <paramref name="EndChars"/> is null
        /// then return instance will have no values.
        /// </returns>
        public new static EndCharsSelectedOptions FromArray(EndCharsEnum[] EndChars)
        {
            EndCharsSelectedOptions ec = new EndCharsSelectedOptions();
            if (EndChars == null)
            {
                return ec;
            }
            foreach (var e in EndChars)
            {
                string sKey = e.ToString();
                if (ec.HasKey[sKey] == false)
                {
                    ec.Options.Add(e);
                    ec.Keys.Add(sKey);
                }
            }
            return ec;
        }
        #endregion
    }
}
