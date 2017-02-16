using System;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Constants;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Exceptions;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys
{
    /// <summary>
    /// Represents an AutoHotkey Hotkey with different Options and modifiers
    /// </summary>
    /// <remarks>
    /// The general definition of a Combined hotkey is a Hotkeys that contains only two keys with the only possible
    /// options set to true being <see cref="HotkeyKeys.NativeBlock"/>.
    /// 
    /// The general definition of a Multi-key hotkeys is a Hotkeys that contains two keys with some sort of modifier
    /// applies such as <see cref="HotkeyKeys.Alt"/>, <see cref="HotkeyKeys.Ctrl"/>, <see cref="HotkeyKeys.Shift"/>,
    /// <see cref="HotkeyKeys.Win"/> and possibly combined with <see cref="HotkeyKeys.Left"/> or <see cref="HotkeyKeys.Right"/>.
    /// 
    /// Also See AutoHotkey documentation for more information.
    /// </remarks>
    public partial class HotkeyKeys
    {
        #region Properties
        /// <summary>
        /// Specifies if the Hotkey as a Control Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// If true then <see cref="IsCombine"/> will always be false.
        /// </remarks>
        public bool Ctrl { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey as a Alt Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// If true then <see cref="IsCombine"/> will always be false.
        /// </remarks>
        public bool Alt { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey as a Windows Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// If true then <see cref="IsCombine"/> will always be false.
        /// </remarks>
        public bool Win { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey as a Shift Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// If true then <see cref="IsCombine"/> will always be false.
        /// </remarks>
        public bool Shift { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey as a Left Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// Left is Ignored for methods such as <see cref="ToString"/> and
        /// <see cref="ToReadableString"/> if <see cref="IsMainModKey"/> is false;
        /// </remarks>
        public bool Left { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey as a Right Key Modifier. Default is False
        /// </summary>
        /// <remarks>
        /// Left is Ignored for methods such as <see cref="ToString"/> and
        /// <see cref="ToReadableString"/> if <see cref="IsMainModKey"/> is false;
        /// </remarks>
        public bool Right { get; set; } = false;
        /// <summary>
        /// Specifies if the Up Modifier is in effect. Default is false.
        /// </summary>
        /// <remarks>
        /// When Up is true methods such <see cref="ToString"/> and
        /// <see cref="ToReadableString"/> will Ignore <see cref="Key2"/> and 
        /// <see cref="IsMultiKey"/> and <see cref="IsCombine"/> will be false
        /// </remarks>
        public bool UP { get; set; } = false;
        // public bool Combine { get; set; } = false;
        /// <summary>
        /// Specifies if the Hotkey will be sent directly to the active window. Blocking the
        /// Active window native hotkey. Default is false.
        /// </summary>
        /// <remarks>
        /// Active window Depends on the type of profile that is currently set up. If the Profile is not a
        /// global profile then Active window must also be a match in the Window list for the profile.
        /// </remarks>
        public bool NativeBlock { get; set; } = false;

        /// <summary>
        /// Specifies if the hotkey fires even if extra modifiers are being held down. Default is false.
        /// </summary>
        /// <remarks>
        /// This is often used in conjunction with remapping keys or buttons. 
        /// </remarks>
        public bool WildCard { get; set; } = false;
        /// <summary>
        /// Specifies if The Keyboard Hook Will be Installed. Default is false.
        /// </summary>
        /// <remarks>
        /// When InstallHook is true methods such <see cref="ToString"/> and
        /// <see cref="ToReadableString"/> will Ignore <see cref="Key2"/> and 
        /// <see cref="IsMultiKey"/> and <see cref="IsCombine"/> will be false
        /// </remarks>
        public bool InstallHook { get; set; } = false;

        /// <summary>
        /// Gets if one or more of the Main Modifier keys are used.
        /// True if Alt, Ctrl, Shift or Win are true.
        /// </summary>
        public bool IsMainModKey
        {
            get
            {
                bool retval = false;
                retval |= this.Alt;
                retval |= this.Ctrl;
                retval |= this.Shift;
                retval |= this.Win;
                return retval;
            }
        }

        /// <summary>
        /// Gets if the Hokey is a multi-key.
        /// </summary>
        /// <remarks>
        /// Multi-keys are handled differently than Combined Key ( combined using AutoHotkey &amp; )
        /// IsMultikey will not be true if <see cref="IsCombine"/> is true and vise versa.
        /// </remarks>
        public bool IsMultiKey
        {
            get
            {
                bool retval = true;
                retval &= !this.InstallHook;
                retval &= !this.WildCard;
                retval &= !this.UP;
                if (retval == false)
                {
                    return retval;
                }
                retval = this.IsMainModKey;
                // left and right are ignored here as having a Main Modifier is enough
               
                if (retval == false)
                {
                    return retval;
                }
                retval &= Key1 != HotkeysEnum.None;
                retval &= Key2 != HotkeysEnum.None;
                retval &= Key1 != Key2;
                retval &= Key1 != HotkeysEnum.CtrlBreak;
                retval &= Key2 != HotkeysEnum.CtrlBreak;
                return retval;
            }
        }

        /// <summary>
        /// Gets if the Hotkey Meets the conditions to be a Combine key such as A &amp; T
        /// </summary>
        /// <remarks>
        /// Combined keys are handled differently than multi-keys
        /// IsCombine will not be true if <see cref="IsMultiKey"/> is true and vise versa.
        /// </remarks>
        public bool IsCombine
        {
            get
            {
                if (this.IsMainModKey == true)
                {
                    return false;
                }
                bool retval = true;
                retval &= !this.InstallHook;

                // Left and Right will be Ignored if IsMainModKey is true
   
                
                retval &= !this.UP;
                retval &= !this.WildCard;
                
                if (retval == true)
                {
                    retval &= Key1 != HotkeysEnum.None;
                    retval &= Key2 != HotkeysEnum.None;
                    retval &= Key1 != Key2;
                    retval &= Key1 != HotkeysEnum.CtrlBreak;
                    retval &= Key2 != HotkeysEnum.CtrlBreak;
                }
                return retval;
            }
        }

        /// <summary>
        /// Gets if the configuration of the current instance of <see cref="HotkeyKeys"/> is Valid
        /// </summary>
        /// <returns>
        /// True if Valid configuration; Otherwise, false
        /// </returns>
        public bool IsValid
        {
            get
            {
                if (this.IsCombine || this.IsMultiKey)
                {
                    return true;
                }
                bool b = true;
                b &= Key1 != HotkeysEnum.None;
                b &= Key2 == HotkeysEnum.None;
                // the only thing allowed in front ControlBreak is NativeBlock and Install Hook
                if (Key1 == HotkeysEnum.CtrlBreak)
                {
                    b &= !this.Alt;
                    b &= !this.Ctrl;
                    b &= !this.Left;
                    b &= !this.Right;
                    b &= !this.Shift;
                    b &= !this.UP;
                    b &= !this.WildCard;
                    b &= !this.Win;
                }
                if (this.Left || this.Right)
                {
                    // left or right only valid if is a Modifier key
                    b &= this.IsMainModKey;
                    // left and right cant be active at the same time
                    b &= !(this.Left & this.Right);
                }
                return b;
            }

        }

        /// <summary>
        /// Specifies the First Key of the Hotkeys
        /// </summary>
        public HotkeysEnum Key1 { get; set; } = HotkeysEnum.None;
        /// <summary>
        /// Specifies the Second Key of the Hotkeys used when Combining Hotkeys or creating a Multi-Key hotkey
        /// </summary>
        /// <remarks>
        /// Second Key is only taken into consideration when <see cref="IsCombine"/> is true
        /// or <see cref="IsMainModKey"/> is true for methods such as <see cref="ToString"/> and
        /// <see cref="ToReadableString"/>
        /// </remarks>
        public HotkeysEnum Key2 { get; set; } = HotkeysEnum.None;
        #endregion

        #region Method

        #region Parse
        /// <summary>
        /// Parses a Hotkeys From a string value
        /// </summary>
        /// <param name="s">The Hotkeys to Parse</param>
        /// <returns>
        /// An instance of <see cref="HotkeyKeys"/> if the parse was successful.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HotKeyFormatInvalidException"></exception>
        /// <exception cref="HotkeysCombineException">
        /// If <paramref name="s"/> contains more than one <see cref="AhkHotkeys.CombineString"/> or 
        /// if left or right of <see cref="AhkHotkeys.CombineString"/> is not the correct type of value.
        /// </exception>
        /// <exception cref="KeyNotSupportedException">
        /// If any of the keys are not a key found in <see cref="HotkeysEnum"/>.
        /// </exception>
        public static HotkeyKeys Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException();
            }
            HotkeyKeys retval = new HotkeyKeys();
            // by splitting we can get how many modifier there are at the start of the string.
            var Sections = s.Split(new char[] { AhkHotkeys.Alt
                , AhkHotkeys.Shift
                , AhkHotkeys.Ctrl
                , AhkHotkeys.Win
                , AhkHotkeys.Left
                , AhkHotkeys.Right
                , AhkHotkeys.WildCard
                , AhkHotkeys.InstallHook
                , AhkHotkeys.NativeBlock
            });
            int iPrefexLength = 0;
            foreach (var str in Sections)
            {
                if (string.IsNullOrEmpty(str))
                {
                    iPrefexLength++;
                }
                else
                {
                    break;
                }
            }
            // iPrefexLength now contains the number of modifiers in the hotkey
            // Get the Modifiers and assign the to retval
            if (iPrefexLength > 0)
            {
                string Mod = s.Substring(0, iPrefexLength);

                if (Mod.IndexOf(AhkHotkeys.Alt) >= 0)
                {
                    retval.Alt = true;
                }
                if (Mod.IndexOf(AhkHotkeys.Ctrl) >= 0)
                {
                    retval.Ctrl = true;
                }
                if (Mod.IndexOf(AhkHotkeys.InstallHook) >= 0)
                {
                    retval.InstallHook = true;
                }
                if (Mod.IndexOf(AhkHotkeys.Left) >= 0)
                {
                    retval.Left = true;
                }
                if (Mod.IndexOf(AhkHotkeys.NativeBlock) >= 0)
                {
                    retval.NativeBlock = true;
                }
                if (Mod.IndexOf(AhkHotkeys.Right) >= 0)
                {
                    retval.Right = true;
                }
                if (Mod.IndexOf(AhkHotkeys.Shift) >= 0)
                {
                    retval.Shift = true;
                }
                if (Mod.IndexOf(AhkHotkeys.WildCard) >= 0)
                {
                    retval.WildCard = true;
                }
                if (Mod.IndexOf(AhkHotkeys.Win) >= 0)
                {
                    retval.Win = true;
                }
            }

            string NoMod = s;
            if (iPrefexLength > 0 && s.Length > iPrefexLength)
            {
                NoMod = s.Substring(iPrefexLength);
            }
            if (NoMod.Length == 0)
            {
                throw new HotKeyFormatInvalidException(Properties.Resources.HotKeyFormatInvalidExceptionMissingTriggerKey);
            }

            // NoMod now contains is the remainder of the hotkey without any modifier keys.
            // Now we need to check for the UP modifier
            if (NoMod.Length > 3)
            {
                if (NoMod.EndsWith(AhkHotkeys.UpString, StringComparison.CurrentCultureIgnoreCase))
                {
                    retval.UP = true;
                    NoMod = NoMod.Substring(0, NoMod.Length - AhkHotkeys.UpString.Length);
                }
            }
            // If NoMod had a up modifier has been removed at this point
            // Now we need to check for combine key (&)
            
            bool comb = false;
            if (NoMod.Length > 2 && NoMod.IndexOf(AhkHotkeys.Combine) >= 0)
            {
                comb = true;
            }

            // I am thinking that combine when used on a hotkey that does support Combining but does have two single keys
            // should automatically become a multi-key hotkey; such as ^a & s 
            // If this works out then all existing multi-key hotkeys need to be converted from format ^ab to ^a & b
            // If the HotKey can combine is false but combine is true then it is a multi key

            if (comb == true)
            {
                // Splitting with RemoveEmptyEntries will ignore whitespace on either side of the & so C   & f are the same as c&f
                var keys = NoMod.Split(new char[] { AhkHotkeys.Combine }, StringSplitOptions.RemoveEmptyEntries);
                if (keys.Length > 2)
                {
                    throw new HotkeysCombineException(Properties.Resources.HotkeysCombineExceptionTooManyKeys);
                }
                if (keys.Length < 2)
                {
                    throw new HotkeysCombineException(Properties.Resources.HotkeysCombineExceptionToFiewKeys);
                }

                HotkeysEnum k1 = HotkeysEnum.None;
                if (Enum.TryParse<HotkeysEnum>(keys[0], true, out k1) == false)
                {
                    throw new KeyNotSupportedException(string.Format(Properties.Resources.KeyNotSupportedExceptionDefault
                        , keys[0]));
                }
                retval.Key1 = k1;

                HotkeysEnum k2 = HotkeysEnum.None;
                if (Enum.TryParse<HotkeysEnum>(keys[1], true, out k2) == false)
                {
                    throw new KeyNotSupportedException(string.Format(Properties.Resources.KeyNotSupportedExceptionDefault
                        , keys[1]));
                }
                retval.Key2 = k2;

            }
            else
            {
                HotkeysEnum kk = HotkeysEnum.None;
                if (Enum.TryParse<HotkeysEnum>(NoMod, true, out kk) == false)
                {
                    throw new KeyNotSupportedException(string.Format(Properties.Resources.KeyNotSupportedExceptionDefault
                        , NoMod));
                }
                retval.Key1 = kk;
            }

            if (retval.IsValid == false)
            {
                throw new HotKeyFormatInvalidException(Properties.Resources.HotKeyFormatInvalidExceptionInvalidCombination);
            }
            return retval;
        }

        /// <summary>
        /// Attempts to Parse a string representing a Hotkeys.
        /// </summary>
        /// <param name="s">The Hotkeys to Parse</param>
        /// <param name="hks">The <see cref="AhkHotkeys"/> that represents the parsed results</param>
        /// <returns>
        /// Returns True if the parse was successful; Otherwise false
        /// </returns>
        public static bool TryParse(string s, out HotkeyKeys hks)
        {
            bool retval = false;
            HotkeyKeys outHks = null;
            try
            {
                HotkeyKeys hk = HotkeyKeys.Parse(s);
                outHks = new HotkeyKeys();
                outHks.Alt = hk.Alt;
                // outHks.Combine = hk.Combine;
                outHks.Ctrl = hk.Ctrl;
                outHks.InstallHook = hk.InstallHook;
                outHks.Key1 = hk.Key1;
                outHks.Key2 = hk.Key2;
                outHks.Left = hk.Left;
                outHks.NativeBlock = hk.NativeBlock;
                outHks.Right = hk.Right;
                outHks.Shift = hk.Shift;
                outHks.UP = hk.UP;
                outHks.WildCard = hk.WildCard;
                outHks.Win = hk.Win;
                retval = true;
            }
            catch (Exception)
            {
            }
            hks = outHks;
            return retval;
        }
        #endregion

        #region String Output Methods
        /// <summary>
        /// Gets a string representing the Hotkey prefix in AutoHotkey format such as !^
        /// </summary>
        /// <returns>
        /// String representing the Hotkey prefix.
        /// </returns>
        /// <remarks>
        /// The return value will represent the Hotkey prefix after internal rules are applied. Some of the properties
        /// of the instance may be ignored if the break AutoHotkey hotkey rules. See the various properties in
        /// <see cref="AhkHotkeys"/> for more information. Also you may want to review AutoHotkey Documentation.
        /// </remarks>
        public string GetPrefix()
        {
            if (Key1 == HotkeysEnum.None)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            var combine = this.IsCombine;
            var multiKey = this.IsMultiKey;

            if (this.InstallHook == true && combine == false && multiKey == false)
            {
                sb.Append(AhkHotkeys.InstallHook);
            }
            if (this.NativeBlock == true)
            {
                sb.Append(AhkHotkeys.NativeBlock);
            }
            if (Key1 == HotkeysEnum.CtrlBreak)
            {
                // CtrlBreak is a special case and is not to be combined with any other keys
                // other than InstallHook and NativeBlock
                // IsCombine and IsMultikey takes care of Hey2 being CtrlBreak so no other action
                // is required in this method
                sb.Append(Key1.ToString());
                return sb.ToString();
            }

            if (this.WildCard == true && combine == false && multiKey == false)
            {
                sb.Append(AhkHotkeys.WildCard);
            }
            if (this.Right == true && this.IsMainModKey == true)
            {
                sb.Append(AhkHotkeys.Right);
            }
            if (this.Right == false && this.Left == true && this.IsMainModKey == true)
            {
                sb.Append(AhkHotkeys.Left);
            }
            if (this.Ctrl == true)
            {
                sb.Append(AhkHotkeys.Ctrl);
            }
            if (this.Alt == true)
            {
                sb.Append(AhkHotkeys.Alt);
            }
            if (this.Shift == true)
            {
                sb.Append(AhkHotkeys.Shift);
            }
            if (this.Win == true)
            {
                sb.Append(AhkHotkeys.Win);
            }
            return sb.ToString();

        }

        /// <summary>
        /// Gets a string representing the Hotkey in AutoHotkey format such as !^G
        /// </summary>
        /// <returns>
        /// String representing the Hotkey.
        /// </returns>
        /// <remarks>
        /// The return value will represent the Hotkey after internal rules are applied. Some of the properties
        /// of the instance may be ignored if the break AutoHotkey hotkey rules. See the various properties in
        /// <see cref="AhkHotkeys"/> for more information. Also you may want to review AutoHotkey Documentation.
        /// The Internal rules are the same for the <see cref="ToReadableString"/> method. This ensures that both methods
        /// represent the same values.
        /// </remarks>
        public override string ToString()
        {
            if (Key1 == HotkeysEnum.None)
            {
                return string.Empty;
            }
           
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetPrefix());

            var combine = this.IsCombine;
            var multiKey = this.IsMultiKey;

            sb.Append(Key1.ToString());
            if (combine == true || multiKey)
            {
                sb.Append(AhkHotkeys.CombineString);
                sb.Append(Key2.ToString());
            }
            if (this.UP == true && combine == false && multiKey == false)
            {
                sb.Append(AhkHotkeys.UpString);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representing the Hotkey in Human Readable format such as Alt + Ctrl + G
        /// </summary>
        /// <returns>
        /// String representing the Hotkey in a human readable format.
        /// </returns>
        /// <remarks>
        /// The return value will represent the Hotkey after internal rules are applied. Some of the properties
        /// of the instance may be ignored if the break AutoHotkey hotkey rules. See the various properties in
        /// <see cref="AhkHotkeys"/> for more information. Also you may want to review AutoHotkey Documentation.
        /// The Internal rules are the same for the <see cref="ToString"/> method. This ensures that both methods
        /// represent the same values.
        /// </remarks>
        public string ToReadableString()
        {
            if (Key1 == HotkeysEnum.None)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            if (this.InstallHook == true)
            {
                sb.Append(Properties.Resources.KeysHook);
                sb.Append(" + ");
            }
            if (this.NativeBlock == true)
            {
                sb.Append(Properties.Resources.KeysNativeBlock);
                sb.Append(" + ");
            }
            if (Key1 == HotkeysEnum.CtrlBreak)
            {
                // CtrlBreak is a special case and is not to be combined with any other keys
                // other than InstallHook and NativeBlock
                // IsCombine and IsMultikey takes care of Hey2 being CtrlBreak so no other action
                // is required in this method
                sb.Append(Key1.ToString());
                return sb.ToString();
            }
            if (this.WildCard == true)
            {
                sb.Append(Properties.Resources.KeysWildcard);
                sb.Append(" + ");
            }
            if (this.Right == true && this.IsMainModKey == true)
            {
                sb.Append(Properties.Resources.KeyRight);
                sb.Append(" + ");
            }
            if (this.Right == false && this.Left == true && this.IsMainModKey == true)
            {
                sb.Append(Properties.Resources.KeyLeft);
                sb.Append(" + ");
            }
            if (this.Ctrl == true)
            {
                sb.Append(Properties.Resources.KeyCtrl);
                sb.Append(" + ");
            }
            if (this.Alt == true)
            {
                sb.Append(Properties.Resources.KeyAlt);
                sb.Append(" + ");
            }
            if (this.Shift == true)
            {
                sb.Append(Properties.Resources.KeyShift);
                sb.Append(" + ");
            }
            if (this.Win == true)
            {
                sb.Append(Properties.Resources.KeyWin);
                sb.Append(" + ");
            }
            //sb.Length -= 3; // remove the last +

            sb.Append(Key1.ToString());
            if (this.IsCombine == true || this.IsMultiKey == true)
            {
                sb.Append(' ');
                sb.Append(Properties.Resources.KeysCombine);
                sb.Append(' ');
                sb.Append(Key2.ToString());
            }
            if (this.UP == true)
            {
                sb.Append(' ');
                sb.Append(Properties.Resources.KeyUP);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representing the Hotkey Modifiers in Human Readable format such as Alt + Ctrl
        /// </summary>
        /// <returns>
        /// If Modifiers are applied the a string representing the Hotkey Modifiers in a human readable format; Otherwise, Empty.String
        /// </returns>
        /// <remarks>
        /// <see cref="HotkeyKeys.InstallHook"/> and <see cref="HotkeyKeys.NativeBlock"/> are ignored in this method. However
        /// <see cref="HotkeyKeys.Right"/> and <see cref="HotkeyKeys.Left"/> are applied if they are true.
        /// </remarks>
        public string ToReadableMainModiferString()
        {
            if (this.IsMainModKey == false)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            if (this.Right == true)
            {
                sb.Append(Properties.Resources.KeyRight);
                sb.Append(" + ");
            }
            if (this.Right == false && this.Left == true)
            {
                sb.Append(Properties.Resources.KeyLeft);
                sb.Append(" + ");
            }
            if (this.Ctrl == true)
            {
                sb.Append(Properties.Resources.KeyCtrl);
                sb.Append(" + ");
            }
            if (this.Alt == true)
            {
                sb.Append(Properties.Resources.KeyAlt);
                sb.Append(" + ");
            }
            if (this.Shift == true)
            {
                sb.Append(Properties.Resources.KeyShift);
                sb.Append(" + ");
            }
            if (this.Win == true)
            {
                sb.Append(Properties.Resources.KeyWin);
                sb.Append(" + ");
            }
            if (sb.Length > 3)
            {
                sb.Length -= 3; // remove the last +
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representing the key or keys such as R or R & T. Modifiers are not included
        /// </summary>
        /// <returns>
        /// String, if <see cref="HotkeyKeys.IsCombine"/> Or <see cref="HotkeyKeys.IsMainModKey"/> is true then
        /// a combined format such as R & T; Otherwise, a single format such as T
        /// </returns>
        public string ToReeadableKeys()
        {
            if (this.IsCombine == true || this.IsMultiKey == true)
            {
                return string.Format("{0} {1} {2}", Key1.ToString(), Properties.Resources.KeysCombine, Key2.ToString());
            }
            else
            {
                return Key1.ToString();
            }
        }
        #endregion

        #endregion
    }
}
