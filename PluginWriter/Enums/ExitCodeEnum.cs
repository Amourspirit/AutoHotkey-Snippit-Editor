using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums
{
    /// <summary>
    /// Codes that Define the Exit Code
    /// </summary>
    internal enum ExitCodeEnum
    {
        /// <summary>
        /// No Error has Occurred
        /// </summary>
        NoError = 0,
        /// <summary>
        /// Exit code has not been set
        /// </summary>
        NotSet = 1,
        /// <summary>
        /// No Valid Install could be found
        /// </summary>
        /// <remarks>
        /// May be due to ini value not set for current profile
        /// </remarks>
        NoInstall = 1000,
        /// <summary>
        /// No Profile was found for the install
        /// </summary>
        NoProfile = 1001,
        /// <summary>
        /// No Plugins found for the install.
        /// </summary>
        /// <remarks>
        /// There may have been a profile but the install did not contain any plugins.
        /// </remarks>
        NoPlugins = 1002,        
        /// <summary>
        /// Error in WriteHotstrinPasteCode method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsWriteHotstrinPasteCode = 2000,
        /// <summary>
        /// Error in WriteReplacements method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsWriteReplacements = 2001,
        /// <summary>
        /// Error in WriteReplacementInputFixedList method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsWriteReplacementInputFixedList = 2002,
        /// <summary>
        /// Error in WriteReplacementInputReplacement method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsWriteReplacementInputReplacement = 2003,
        /// <summary>
        /// Error in SaveSnippitToFile method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsInlineSaveSnippitToFile = 2004,
        /// <summary>
        /// Error in WriteHotstringCode method of <see cref="Windows.Console.AHKSnipit.PluginWriter.Hotstrings.HotstringWriter"/>
        /// </summary>
        HsWriteHotstringCode = 2005,
        /// <summary>
        /// General EndChars Write Error
        /// </summary>
        EndCharsGeneralError = 3000,
        /// <summary>
        /// Error is Save SaveCodeToFile method of <see cref="HotKeys.HotKeyWriter"/>
        /// </summary>
        HkSaveCodeToFile = 4000,
        /// <summary>
        /// General HotKeyWriter Error
        /// </summary>
        HkGeneralError = 4001,
        /// <summary>
        /// Error is in WriteRegular method of <see cref="HotKeys.HotKeyWriter"/>
        /// </summary>
        HkWriteRegular = 4002,
        /// <summary>
        /// Error in WriteMultiKeys method of <see cref="HotKeys.HotKeyWriter"/>
        /// </summary>
        HkWriteMultiKeys = 4003,
        /// <summary>
        /// Error writing main include plugin file. Write method of <see cref="Include.IncludeWriter"/> failed
        /// </summary>
        IwGeneralFail = 5000,
        /// <summary>
        /// Error is SaveIncludePlugins method of <see cref="Include.IncludeWriter"/>
        /// </summary>
        IwSaveIncludePlugins = 5001,
        /// <summary>
        /// General Failure trying to parse plugins
        /// </summary>
        PlugingHotstringWriteGeneralFail = 6000,
        /// <summary>
        /// General DataWriter error
        /// </summary>
        DataGenerError = 7000,
        /// <summary>
        /// DataWriter unable to create root data folder
        /// </summary>
        DataRootFolderFail = 7001,
        /// <summary>
        /// DataWriter unable to create folder for current Plugin Data Item
        /// </summary>
        DataFolderFail = 7002,
        /// <summary>
        /// Error writing file to disk
        /// </summary>
        FileWriteError = 9999,
        /// <summary>
        /// Error Minimum Version Not Met
        /// </summary>
        MinVersionError = 1099,
        /// <summary>
        /// All is fine and a Reload is required
        /// </summary>
        /// <remarks>
        /// Reload required is set when there are no errors but there are changes
        /// that require the main AutoHotkey Scrip to be reloaded.
        /// </remarks>
        ReloadRequired = -100,

    }
}
