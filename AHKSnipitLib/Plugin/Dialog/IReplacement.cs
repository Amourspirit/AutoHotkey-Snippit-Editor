using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.Dialog
{
    public interface IReplacement
    {
        string ReplacementName { get; }
        ReplacementEnum ReplacementType { get; }
        string ReplacementTitle { get; }
    }
    public enum ReplacementEnum:byte
    {
        InputDialog,
        InputFixedList
    }


}
