using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Specifies Exclude Rules for with <see cref="HotstringSelectedOptions"/>
    /// </summary>
    class HotstringCodeExcludeRules : EnumRule<HotStringOptionsEnum>
    {
        /// <summary>
        /// Creates a new instance of the class with predetermined rules
        /// </summary>
        public HotstringCodeExcludeRules()
            :base()
        {
            this.Add(HotStringOptionsEnum.NoConform);
            this.Add(HotStringOptionsEnum.SendRaw);
            this.Add(HotStringOptionsEnum.SendEvent);
            this.Add(HotStringOptionsEnum.SendInput);
            this.Add(HotStringOptionsEnum.SendPlay);
            this.Add(HotStringOptionsEnum.TriggerInside);
        }
    }
}
