using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using Con = System.Console;
using EndC = BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys.EndCharsSelectedOptions;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ReloadRequired = false;
            Enums.ExitCodeEnum ExitReason = Enums.ExitCodeEnum.NoError;

            SnippitInstal si = ReadWrite.ReadCurrentInstall();
            if (si == null)
            {
                ExitReason = Enums.ExitCodeEnum.NoInstall;
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                if (si.profile == null)
                {
                    ExitReason = Enums.ExitCodeEnum.NoProfile;
                }
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var ProfileHashExitReason = WriteProfileHash(si);
                if (ProfileHashExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (ProfileHashExitReason != Enums.ExitCodeEnum.NoError)
                {
                    ExitReason = ProfileHashExitReason;
                }
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var EndCharExitReason = WriteEndChars(si);
                if (EndCharExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (EndCharExitReason != Enums.ExitCodeEnum.NoError)
                {
                    ExitReason = EndCharExitReason;
                }
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var HotStringExitReason = WriteHotStrings(si);
                if (HotStringExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (HotStringExitReason == Enums.ExitCodeEnum.NoPlugins)
                {
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (HotStringExitReason != Enums.ExitCodeEnum.NoError)
                {
                    ExitReason = HotStringExitReason;
                }
            }
                      

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var HotKeyExitReason = WriteHotKeys(si);
                if (HotKeyExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (HotKeyExitReason == Enums.ExitCodeEnum.NoPlugins)
                {
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if (HotKeyExitReason > Enums.ExitCodeEnum.NoError)
                {
                    ExitReason = HotKeyExitReason;
                }
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var IncExitReason = WriteIncludePlugins(si);
                if (IncExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if ((IncExitReason > Enums.ExitCodeEnum.NoError) && (IncExitReason != Enums.ExitCodeEnum.NoPlugins))
                {
                    ExitReason = IncExitReason;
                }
            }

            if (ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                var DataExitReason = WriteDataFiles(si);
                if (DataExitReason == Enums.ExitCodeEnum.ReloadRequired)
                {
                    ReloadRequired = true;
                    ExitReason = Enums.ExitCodeEnum.NoError;
                }
                else if ((DataExitReason > Enums.ExitCodeEnum.NoError) && (DataExitReason != Enums.ExitCodeEnum.NoPlugins))
                {
                    ExitReason = DataExitReason;
                }
            }

            if ( ReloadRequired == true && ExitReason <= Enums.ExitCodeEnum.NoError)
            {
                IniHelper.SetWriterExitReason((int)Enums.ExitCodeEnum.ReloadRequired);
            }
            else
            {
                IniHelper.SetWriterExitReason((int)ExitReason);
            }
            
            Environment.Exit((int)ExitReason);
        }

        private static Enums.ExitCodeEnum WriteProfileHash(SnippitInstal si)
        {
            Profiles.ProfilesHashWriter pw = new Profiles.ProfilesHashWriter(si);
            pw.Write();
            string sHashFile = Path.Combine(AppCommon.Instance.PathData, "profilehash.txt");
            Enums.ExitCodeEnum retval = pw.ExitReason;
            string pwHashText = pw.ToString();
            bool WriteHsFiles = true;

            if (File.Exists(sHashFile))
            {
                string ExistingText = File.ReadAllText(sHashFile);
                
                if (ExistingText == pwHashText)
                {
                    WriteHsFiles = false;
                }
            }
          
            if (WriteHsFiles == true)
            {
                try
                {
                    File.WriteAllText(sHashFile, pwHashText, Encoding.UTF8);
                    retval = Enums.ExitCodeEnum.ReloadRequired;
                }
                catch (Exception)
                {
                    return Enums.ExitCodeEnum.FileWriteError;
                }
            }

            return retval;
        }

        private static Enums.ExitCodeEnum WriteIncludePlugins(SnippitInstal si)
        {
            Include.IncludeWriter ic = new Include.IncludeWriter(si);
            ic.Write();
            bool WriteHsFiles = true;
            Enums.ExitCodeEnum retval = ic.ExitReason;
            if (retval == Enums.ExitCodeEnum.NoError)
            {
                string icFile = ic.PluginIncludFile;
                string icText = ic.ToString();
                if (File.Exists(icFile))
                {
                    string ExistingText = File.ReadAllText(icFile);
                    string icwHash = Util.GetHashString(icText);
                    string ExistingTextHash = Util.GetHashString(ExistingText);
                    if (icwHash == ExistingTextHash)
                    {
                        WriteHsFiles = false;
                    }
                }
                if (WriteHsFiles == true)
                {
                    try
                    {
                        File.WriteAllText(icFile, icText, Encoding.UTF8);
                    }
                    catch (Exception)
                    {
                        return Enums.ExitCodeEnum.FileWriteError;
                    }
                    ic.SaveIncludePlugins();
                    retval = ic.ExitReason;
                    if (retval == Enums.ExitCodeEnum.NoError)
                    {
                        retval = Enums.ExitCodeEnum.ReloadRequired;
                    }
                }
            }
            return retval;
        }

        private static Enums.ExitCodeEnum WriteEndChars(SnippitInstal si)
        {
            if (si.profile == null)
            {
                return Enums.ExitCodeEnum.NoPlugins;
            }

            EndChars.EndCharsWriter ecw = new EndChars.EndCharsWriter(si.profile);
            ecw.Write();
            if (ecw.ExitReason == Enums.ExitCodeEnum.NoError)
            {
                string ecFile = ecw.EndCharsIncludeFile;
                string ecText = ecw.ToString();
                bool WriteHsFiles = true;
                if (File.Exists(ecFile))
                {
                    string ExistingText = File.ReadAllText(ecFile);
                    string ecwHash = Util.GetHashString(ecText);
                    string ExistingTextHash = Util.GetHashString(ExistingText);
                    if (ecwHash == ExistingTextHash)
                    {
                        WriteHsFiles = false;
                    }
                }
                if (WriteHsFiles == true)
                {
                    try
                    {
                        File.WriteAllText(ecFile, ecText, Encoding.UTF8);
                    }
                    catch (Exception)
                    {
                        return Enums.ExitCodeEnum.FileWriteError;
                    }
                    
                }
            }
            return ecw.ExitReason;
        }
        
        private static Enums.ExitCodeEnum WriteHotStrings(SnippitInstal si)
        {
            
            Hotstrings.HotstringWriter hsw = new Hotstrings.HotstringWriter(si);
            hsw.Write();
            Enums.ExitCodeEnum retval = hsw.ExitReason;
            if (hsw.ExitReason == Enums.ExitCodeEnum.NoError)
            {
                string PluginHsFile = hsw.HsIncludeFile;
                string hswText = hsw.ToString();
                bool WriteHsFiles = true;
                if (File.Exists(PluginHsFile))
                {
                    string ExistingText = File.ReadAllText(PluginHsFile);
                    string hswHash = Util.GetHashString(hswText);
                    string ExistingTextHash = Util.GetHashString(ExistingText);
                    if (hswHash == ExistingTextHash)
                    {
                        WriteHsFiles = false;
                    }
                }
                if (WriteHsFiles == true)
                {
                    try
                    {
                        File.WriteAllText(PluginHsFile, hswText, Encoding.UTF8);
                    }
                    catch (Exception)
                    {
                        return Enums.ExitCodeEnum.FileWriteError;
                    }
                   
                    hsw.SaveSnippitToFile();
                    retval = hsw.ExitReason;
                    if (retval == Enums.ExitCodeEnum.NoError)
                    {
                        retval = Enums.ExitCodeEnum.ReloadRequired;
                    }
                }
            }
            return retval;
        }

        private static Enums.ExitCodeEnum WriteHotKeys(SnippitInstal si)
        {

            HotKeys.HotKeyWriter hkw = new HotKeys.HotKeyWriter(si);
            hkw.Write();
            Enums.ExitCodeEnum retval = hkw.ExitReason;
            if (hkw.ExitReason == Enums.ExitCodeEnum.NoError)
            {
                string PluginKeyFile = hkw.PluginKeyFile;
                string hkwText = hkw.ToString();
                bool WriteHsFiles = true;
                if (File.Exists(PluginKeyFile))
                {


                    string ExistingText = File.ReadAllText(PluginKeyFile);
                    string hkwHash = Util.GetHashString(hkwText);
                    string ExistingTextHash = Util.GetHashString(ExistingText);
                    if (hkwHash == ExistingTextHash)
                    {
                        WriteHsFiles = false;
                    }

                }
                if (WriteHsFiles == true)
                {

                    File.WriteAllText(PluginKeyFile, hkwText, Encoding.UTF8);
                    hkw.SaveCodeToFile();
                    retval = hkw.ExitReason;
                    if (retval == Enums.ExitCodeEnum.NoError)
                    {
                        retval = Enums.ExitCodeEnum.ReloadRequired;
                    }
                }

            }

            return retval;
        }

        private static Enums.ExitCodeEnum WriteDataFiles(SnippitInstal si)
        {
            Data.DataWriter dw = new Data.DataWriter(si);
            dw.Write();
            Enums.ExitCodeEnum retval = dw.ExitReason;
            if ((dw.ExitReason == Enums.ExitCodeEnum.NoError) || (dw.ExitReason == Enums.ExitCodeEnum.NoPlugins))
            {
                string DataIncFile = dw.DataIncludeFile;
                string DataText = dw.ToString();
                bool WriteFiles = true;
                if (File.Exists(DataIncFile))
                {
                    string ExistingTextHash = File.ReadAllText(DataIncFile);
                    string dwHash = Util.GetHashString(DataText);
                    if (dwHash == ExistingTextHash)
                    {
                        WriteFiles = false;
                    }
                }
                if (WriteFiles == true)
                {
                    try
                    {
                        File.WriteAllText(DataIncFile, DataText);
                    }
                    catch (Exception)
                    {
                        return Enums.ExitCodeEnum.FileWriteError;
                    }

                    dw.WriteDataFiles();
                    retval = dw.ExitReason;
                    if (retval == Enums.ExitCodeEnum.NoError)
                    {
                        retval = Enums.ExitCodeEnum.ReloadRequired;
                    }
                }
            }
            return retval;
        }
    }

    
}
