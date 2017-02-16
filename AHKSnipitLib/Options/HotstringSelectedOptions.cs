using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Index Property for HotstringSelectedOptions
    /// </summary>
    public class HotStringKeyExist : Generic.KeyExist<string>
    {
        private EnumRule<HotStringOptionsEnum> m_rules;
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="keys">HashSet that contains the keys</param>
        /// <param name="Rules">The Rules to apply</param>
        public HotStringKeyExist(ref HashSet<string> keys, EnumRule<HotStringOptionsEnum> Rules)
            : base(ref keys)
        {
            this.m_rules = Rules;
        }

        /// <summary>
        /// Gets if the Key exist in the internal Keys
        /// </summary>
        /// <param name="Key">The Key to Check</param>
        /// <returns>
        /// True if the Key exist; Otherwise, false.
        /// </returns>
        public new bool this[string Key]
        {
            get
            {
                bool b = true;
                b &= base[Key];
                b &= !this.m_rules.HasRule(Key); // make sure there is no exclude rule
                return b;
                
            }
        }
    }
    /// <summary>
    /// Set of options from <see cref="HotStringOptionsEnum"/>;
    /// </summary>
    public class HotstringSelectedOptions : SelectedOptionRule<HotStringOptionsEnum>
    {
       
        #region Properties

        /// <summary>
        /// Gets if the <see cref="HotstringSelectedOptions"/> Contains a key. All Keys are names from <see cref="HotStringOptionsEnum"/> names.
        /// </summary>
        /// <remarks>
        /// Will Return False if there is an exclude rule for the key
        /// </remarks>
        public new HotStringKeyExist HasKey
        {
            get
            {
                HotStringKeyExist ke = new HotStringKeyExist(ref this.Keys, this.ExcludeRules);
                return ke;
            }
        }
        #endregion
        /// <summary>
        /// Gets the Send Method for the current Options
        /// </summary>
        /// <remarks>
        /// <see cref="HotstringSelectedOptions"/> can have Send of SendEvent or SendPlay or SendInput or none
        /// </remarks>
        /// <seealso cref="Enums.HotStringSendEnum"/>
        public Enums.HotStringSendEnum SendMethod
        {
            get
            {
               
                if (this[HotStringOptionsEnum.SendEvent])
                {
                    return Enums.HotStringSendEnum.SendEvent;
                }
                else if (this[HotStringOptionsEnum.SendPlay])
                {
                    return Enums.HotStringSendEnum.SendPlay;
                }
                else if (this[HotStringOptionsEnum.SendInput])
                {
                    return Enums.HotStringSendEnum.SendInput;
                }
                else if (this[HotStringOptionsEnum.Send])
                {
                    return Enums.HotStringSendEnum.Send;
                }
                return Enums.HotStringSendEnum.None;
            }
            set
            {
                switch (value)
                {
                   
                    case Enums.HotStringSendEnum.SendInput:
                        this.Remove(HotStringOptionsEnum.Send);
                        this.Remove(HotStringOptionsEnum.SendEvent);
                        this.Add(HotStringOptionsEnum.SendInput);
                        this.Remove(HotStringOptionsEnum.SendPlay);
                        break;
                    case Enums.HotStringSendEnum.SendPlay:
                        this.Remove(HotStringOptionsEnum.Send);
                        this.Remove(HotStringOptionsEnum.SendEvent);
                        this.Remove(HotStringOptionsEnum.SendInput);
                        this.Add(HotStringOptionsEnum.SendPlay);
                        break;
                    case Enums.HotStringSendEnum.SendEvent:
                        this.Remove(HotStringOptionsEnum.Send);
                        this.Add(HotStringOptionsEnum.SendEvent);
                        this.Remove(HotStringOptionsEnum.SendInput);
                        this.Remove(HotStringOptionsEnum.SendPlay);
                        break;
                    case Enums.HotStringSendEnum.Send:
                        this.Add(HotStringOptionsEnum.Send);
                        this.Remove(HotStringOptionsEnum.SendEvent);
                        this.Remove(HotStringOptionsEnum.SendInput);
                        this.Remove(HotStringOptionsEnum.SendPlay);
                        break;
                    default:
                        this.Remove(HotStringOptionsEnum.Send);
                        this.Remove(HotStringOptionsEnum.SendEvent);
                        this.Remove(HotStringOptionsEnum.SendInput);
                        this.Remove(HotStringOptionsEnum.SendPlay);
                        break;
                }
            }
        }

        #region List

        #region From List
        /// <summary>
        /// Gets a new <see cref="HotstringSelectedOptions"/> instance containing the selected values of <paramref name="ListItems"/>
        /// </summary>
        /// <param name="ListItems">The list containing one or more Selected Items to add to the list</param>
        /// <returns>
        /// A new Instance of <see cref="HotstringSelectedOptions"/> with the values that were marked as Selected in the
        /// <paramref name="ListItems"/>
        /// </returns>
        /// <remarks>
        /// Only <see cref="mapItem"/> items that have <see cref="mapItem.Selected"/> set to true
        /// will be included in the <see cref="HotstringSelectedOptions"/> instance.
        /// </remarks>
        public static HotstringSelectedOptions FromList(IList<mapItem> ListItems)
        {
            var ho = new HotstringSelectedOptions();
            if (ListItems.Count == 0)
            {
                return ho;
            }
            foreach (var item in ListItems)
            {
                if (item.Selected == true)
                {
                    if (ho.HasKey[item.key] == false)
                    {
                        if (Enum.IsDefined(typeof(HotStringOptionsEnum), item.key))
                        {
                            HotStringOptionsEnum en;
                            if (Enum.TryParse(item.key, out en) == true)
                            {
                                ho.Options.Add(en);
                                ho.Keys.Add(item.key);
                            }
                        }
                    }
                }
            } // foreach (var item in ListItems)
            return ho;
        }
        #endregion

        #region FromArray
        /// <summary>
        /// Creates a new instance of <see cref="HotstringSelectedOptions"/> populated with the 
        /// values in <paramref name="Options"/>
        /// </summary>
        /// <param name="Options">The Array of Enum Values use to populate the instance</param>
        /// <returns>
        /// Instance of <see cref="HotstringSelectedOptions"/>. If <paramref name="Options"/> is null
        /// then return instance will have no values.
        /// </returns>
        public new static HotstringSelectedOptions FromArray(HotStringOptionsEnum[] Options)
        {
            var so = SelectedOptionRule<HotStringOptionsEnum>.FromArray(Options);
            HotstringSelectedOptions hs = new HotstringSelectedOptions();
            hs.Keys = so.Keys;
            hs.Options = so.Options;

            return hs;
        }
        #endregion
        #endregion

        #region To AutoHotkey
        /// <summary>
        /// Gets the Options in AutoHotkey Format
        /// </summary>
        /// <returns>
        /// String of Hotstring options for AutoHotkey. If no options then Empty Sting.
        /// </returns>
        public string ToAutoHotkeyString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            
            // HasKey also checks to make sure there is no exclude rule
            if (this.HasKey[HotStringOptionsEnum.CaseSensitive.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.CaseSensitive.GetMapValue());
                
                    
            }
            if (this.HasKey[HotStringOptionsEnum.OmitEndChar.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.OmitEndChar.GetMapValue());
                                  
            }
            if (this.HasKey[HotStringOptionsEnum.TriggerInside.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.TriggerInside.GetMapValue());
                                
            }
            if (this.HasKey[HotStringOptionsEnum.AutomaticBackSpaceOff.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.AutomaticBackSpaceOff.GetMapValue());
               
            }
            if (this.HasKey[HotStringOptionsEnum.NoConform.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.NoConform.GetMapValue());
            }
            if (this.HasKey[HotStringOptionsEnum.SendRaw.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.SendRaw.GetMapValue());
            }

            // Note: Send has not AutoHotkey value and is the default
            if (this.HasKey[HotStringOptionsEnum.SendInput.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.SendInput.GetMapValue());
            }
            else if (this.HasKey[HotStringOptionsEnum.SendPlay.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.SendPlay.GetMapValue());
            }
            else if (this.HasKey[HotStringOptionsEnum.SendEvent.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.SendEvent.GetMapValue());
            }
            if (this.HasKey[HotStringOptionsEnum.ResetRecognizer.ToString()] == true)
            {
                sb.Append(HotStringOptionsEnum.ResetRecognizer.GetMapValue());
            }

            return sb.ToString();
        }
        #endregion

        #region Parse
        /// <summary>
        /// Gets a Instance of <see cref="HotstringSelectedOptions"/> From a String value
        /// </summary>
        /// <param name="s">The String to parse for values</param>
        /// <returns>
        /// Instance of <see cref="HotstringSelectedOptions"/> if Parse succeeded; Otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public new static HotstringSelectedOptions Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }
            try
            {
                // all chars except for `n and `t are single chars
                var opt = new HotstringSelectedOptions();
                var so = SelectedOptionRule<HotStringOptionsEnum>.Parse(s);
                opt.Keys = so.Keys;
                opt.Options = so.Options;
                opt.ExcludeRules = so.ExcludeRules;
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
        /// <param name="ec">The <see cref="HotstringSelectedOptions"/> that represents the parsed results</param>
        /// <returns>
        /// Returns True if the parse was successful; Otherwise false
        /// </returns>
        public static bool TryParse(string s, out HotstringSelectedOptions ec)
        {
            bool retval = false;
            HotstringSelectedOptions outec = null;
            try
            {
                HotstringSelectedOptions ecs = HotstringSelectedOptions.Parse(s);
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

       
    }
}
