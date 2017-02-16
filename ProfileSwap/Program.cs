using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BigByteTechnologies.Windows.AHKSnipit.ProfileSwap
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
            SingleApplication.Run(new Swap());
            //Application.Run(new Swap());
        }
    }
}
