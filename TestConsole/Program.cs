using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            HotStringOptionsEnum[] values = new HotStringOptionsEnum[] { HotStringOptionsEnum.AutomaticBackSpaceOff
                ,HotStringOptionsEnum.CaseSensitive
                ,HotStringOptionsEnum.SendEvent};
            HotstringSelectedOptions opt = HotstringSelectedOptions.FromArray(values);
            //opt.ExcludeRules.AddRange(new HotStringOptionsEnum[] { HotStringOptionsEnum.AutomaticBackSpaceOff, HotStringOptionsEnum.CaseSensitive});
            Console.WriteLine(opt.ToString());

            Console.WriteLine(opt.ToAutoHotkeyString());
            Console.ReadLine();
        }
    }
}
