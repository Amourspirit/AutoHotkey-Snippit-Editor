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
    class HotStringUnKnowExcludeRules : EnumRule<HotStringOptionsEnum>
    {
        /// <summary>
        /// Creates a new instance of the class with predetermined rules
        /// </summary>
        public HotStringUnKnowExcludeRules()
            :base()
        {
            this.Add(HotStringOptionsEnum.AutomaticBackSpaceOff);
            this.Add(HotStringOptionsEnum.CaseSensitive);
            this.Add(HotStringOptionsEnum.NoConform);
            this.Add(HotStringOptionsEnum.OmitEndChar);
            this.Add(HotStringOptionsEnum.ResetRecognizer);
            this.Add(HotStringOptionsEnum.SendEvent);
            this.Add(HotStringOptionsEnum.SendInput);
            this.Add(HotStringOptionsEnum.SendPlay);
            this.Add(HotStringOptionsEnum.SendRaw);
            this.Add(HotStringOptionsEnum.TriggerInside);

        }
    }
}
