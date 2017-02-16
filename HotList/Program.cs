using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingleApplication.Run(new SnippitList());
            //Application.Run(new frmtest());
            AppCommon aCommon = AppCommon.Instance;
        }
    }
}
