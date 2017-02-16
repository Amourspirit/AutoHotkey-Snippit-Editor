using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions
{
    /// <summary>
    /// Extension Method for working with End Chars
    /// </summary>
    public static class EnumExtensions
    {
        #region EndCharsEnum
        /// <summary>
        /// Gets <see cref="AhkKeyMapAhkMapValue"/> the represents extended information about
        /// the <see cref="EndCharsEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="EndCharsEnum"/></param>
        /// <returns>
        /// <see cref="AhkKeyMapAhkMapValue"/> instance for the Enum
        /// </returns>
        /// <remarks>
        /// <seealso cref="EndCharKeyMap"/>
        /// <seealso cref="AhkKeyMapAhkMapValue"/>
        /// <seealso cref="EndCharsEnum"/>
        /// </remarks>
        /// <example>
        /// AhkKeyMapAhkMapValue info = EndCharsEnum.A.GetAhkMapValue();
        /// </example>
        public static AhkKeyMapAhkMapValue GetAhkMapValue(this EndCharsEnum @this)
        {

            string EnumName = @this.ToString();
            return EndCharKeyMap.Instance[EnumName];

        }



        /// <summary>
        /// Gets The AutoHotkey End Char symbol that matched the <see cref="EndCharsEnum"/> Value
        /// </summary>
        /// <param name="this"></param>
        /// <returns>
        /// String that Represents the AutoHotkey End Char Symbol
        /// </returns>
        /// <remarks>
        /// <seealso cref="EndCharKeyMap"/>
        /// <seealso cref="AhkKeyMapAhkMapValue"/>
        /// <seealso cref="EndCharsEnum"/>
        /// </remarks>
        /// <example>
        /// string strSymbol = EndCharsEnum.CloseParenthesis.GetAutoHotKeySynbol();
        /// </example>
        public static string GetAutoHotKeySynbol(this EndCharsEnum @this)
        {
            string EnumName = @this.ToString();
            
            var itm = EndCharKeyMap.Instance[EnumName];
            if (itm == null)
            {
                return string.Empty;
            }
            return itm.AutoHotKeyValue;
        }
        #endregion

        #region HotStringSendEnum
        /// <summary>
        /// Gets <see cref="AhkKeyMapAhkMapValue"/> the represents extended information about
        /// the <see cref="HotStringSendEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="HotStringSendEnum"/></param>
        /// <returns>
        /// <see cref="AhkKeyMapAhkMapValue"/> instance for the Enum
        /// </returns>
        /// <remarks>
        /// <seealso cref="EndCharKeyMap"/>
        /// <seealso cref="AhkKeyMapAhkMapValue"/>
        /// <seealso cref="HotStringSendEnum"/>
        /// </remarks>
        /// <example>
        /// AhkKeyMapAhkMapValue info = HotStringSendEnum.SendEvent.GetAhkMapValue();
        /// </example>
        public static AhkKeyMapAhkMapValue GetAhkMapValue(this HotStringSendEnum @this)
        {
            string EnumName = @this.ToString();
            return HotstringSendMethodMap.Instance[EnumName];
        }

        /// <summary>
        /// Gets The AutoHotkey symbol that matched the <see cref="HotStringSendEnum"/> Value
        /// </summary>
        /// <param name="this"></param>
        /// <returns>
        /// String that Represents the AutoHotkey End Char Symbol
        /// </returns>
        /// <remarks>
        /// <seealso cref="EndCharKeyMap"/>
        /// <seealso cref="AhkKeyMapAhkMapValue"/>
        /// <seealso cref="HotStringSendEnum"/>
        /// </remarks>
        /// <example>
        /// string strSymbol = HotStringSendEnum.SendEvent.GetAutoHotKeySynbol();
        /// </example>
        public static string GetAutoHotKeySynbol(this HotStringSendEnum @this)
        {
            string EnumName = @this.ToString();

            var itm = HotstringSendMethodMap.Instance[EnumName];
            if (itm == null)
            {
                return string.Empty;
            }
            return itm.AutoHotKeyValue;
        }

        #endregion

        #region HotStringOptionsEnum

        /// <summary>
        /// Gets a string that represents the value information of <see cref="mapItem"/> for
        /// the <see cref="HotStringOptionsEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="HotStringOptionsEnum"/></param>
        /// <returns>
        /// String of <see cref="mapItem"/> value
        /// </returns>
        /// <remarks>
        /// <seealso cref="HotstringSelectedOptions"/>
        /// <seealso cref="mapItem"/>
        /// <seealso cref="HotStringOptionsEnum"/>
        /// </remarks>
        /// <example>
        /// string strValue = HotStringOptionsEnum.AutomaticBackSpaceOff.GetMapValue();
        /// </example>
        public static string GetMapValue(this HotStringOptionsEnum @this)
        {
            string EnumName = @this.ToString();
            return HotstringOptionMap.Instance[EnumName].value;
           
        }

       

        /// <summary>
        /// Gets <see cref="mapItem"/> the represents extended information about
        /// the <see cref="HotStringOptionsEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="EndCharsEnum"/></param>
        /// <returns>
        /// <see cref="mapItem"/> instance for the Enum;
        /// </returns>
        /// <remarks>
        /// <seealso cref="HotstringSelectedOptions"/>;
        /// <seealso cref="mapItem"/>
        /// <seealso cref="HotStringOptionsEnum"/>
        /// </remarks>
        /// <example>
        /// mapItem item = HotStringOptionsEnum.AutomaticBackSpaceOff.GetMapItem();
        /// </example>
        public static mapItem GetMapItem(this HotStringOptionsEnum @this)
        {
            string EnumName = @this.ToString();
            return HotstringOptionMap.Instance[EnumName];

        }

        #endregion

        #region HotkeysEnum
        /// <summary>
        /// Gets a string that represents the value information of <see cref="mapItem"/> for
        /// the <see cref="HotkeysEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="HotkeysEnum"/></param>
        /// <returns>
        /// String of <see cref="mapItem"/> value
        /// </returns>
        /// <remarks>
        /// When second keepress hotkey is created the value for the seccond key may be different then
        /// the value of the first key. This method get the second value from a map if it contained in the map.
        /// <seealso cref="Has2MapValue(HotkeysEnum)"/>
        /// <seealso cref="HotKey2Map"/>
        /// <seealso cref="mapItem"/>
        /// <seealso cref="HotkeysEnum"/>
        /// </remarks>
        /// <example>
        /// string strValue = HotkeysEnum.Number0.GetKey2MapValue();
        /// </example>
        public static string GetKey2MapValue(this HotkeysEnum @this)
        {
            string EnumName = @this.ToString();
            if (HotKey2Map.Instance.HasKey[EnumName])
            {
                return HotKey2Map.Instance[EnumName].value;
            }
            return EnumName;

        }

        /// <summary>
        /// Gets if a map value exist for a <see cref="HotkeysEnum"/> value.
        /// </summary>
        /// <param name="this"><see cref="HotkeysEnum"/></param>
        /// <returns>
        /// True if a map value exist in the map for <paramref name="this"/>; Otherwise false.
        /// </returns>
        /// <remarks>
        /// When second keepress hotkey is created the value for the seccond key may be different then
        /// the value of the first key. This method get the second value from a map if it contained in the map.
        /// <seealso cref="GetKey2MapValue(HotkeysEnum)"/>
        /// <seealso cref="HotKey2Map"/>
        /// <seealso cref="mapItem"/>
        /// <seealso cref="HotkeysEnum"/>
        /// </remarks>
        /// <example>
        /// bool bValue = HotkeysEnum.Number0.Has2MapValue();
        /// </example>
        public static bool Has2MapValue(this HotkeysEnum @this)
        {
            string EnumName = @this.ToString();
            if (HotKey2Map.Instance.HasKey[EnumName])
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
