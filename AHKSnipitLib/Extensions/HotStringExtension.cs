using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions
{
    /// <summary>
    /// Extensions for Hotstring
    /// </summary>
    public static class HotStringExtension
    {
        /// <summary>
        /// Gets and instance of <see cref="EnumRule{T}"/> or derived class.
        /// </summary>
        /// <param name="this">The enum value to get the instance for</param>
        /// <returns>
        /// <see cref="EnumRule{T}"/> or derived class.
        /// </returns>
        /// <seealso cref="HotstringCodeExcludeRules"/>
        /// <seealso cref="HotStringPasteExcludeRules"/>
        /// <seealso cref="HotStringUnKnowExcludeRules"/>
        public static EnumRule<HotStringOptionsEnum> GetHotstringRules(this hotstringType @this)
        {
            switch (@this)
            {
                case hotstringType.UnKnown:
                    return new HotStringUnKnowExcludeRules();
                case hotstringType.Paste:
                    return new HotStringPasteExcludeRules();
                
                case hotstringType.Code:
                    return new HotstringCodeExcludeRules();
                default:
                    return new EnumRule<HotStringOptionsEnum>();
            }
        }
    }
}
