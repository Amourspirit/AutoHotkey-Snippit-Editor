using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Hotstrings
{
    /// <summary>
    /// Writes the PluginHs.Ahk file that is included in the Main AutoHotkey script.
    /// </summary>
    internal class HotstringWriter : WriterBase
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        internal HotstringWriter() : base()
        {
            this.HsIncludeFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.PluginHsFile);
           
            if (CurrentInstal == null)
            {
                ExitReason = ExitCodeEnum.NoInstall;
                return;
            }
            if (CurrentInstal.plugin == null || CurrentInstal.plugin.Length == 0)
            {
                ExitReason = ExitCodeEnum.NoPlugins;
                return;
            }
            this.HotstringSet = new HashSet<string>();
                     
            baseTextWriter = new System.IO.StringWriter();
            w = new IndentedTextWriter(baseTextWriter);
        }

        internal HotstringWriter(SnippitInstal si) : this()
        {
            this.m_CurrentInstal = si;
        }

        #endregion

        #region Fields/Members
        private SnippitInstal m_CurrentInstal;
        private SnippitInstal CurrentInstal
        {
            get
            {
                if (this.m_CurrentInstal == null)
                {
                    this.m_CurrentInstal = ReadWrite.ReadCurrentInstall();
                }
                return this.m_CurrentInstal;
            }
        }

        HashSet<string> HotstringSet;

        #endregion


        #region Properties

        /// <summary>
        /// Specifies the Include file for the Hot string Includes in HotHotkey.
        /// </summary>
        public string HsIncludeFile { get; set; }
        #endregion

        #region WriteMethods
        protected internal override void Write()
        {
            try
            {
                if (this.CurrentInstal == null)
                {
                    this.ExitReason = ExitCodeEnum.NoInstall;
                    return;
                }
                if (this.m_CurrentInstal.profile == null)
                {
                    this.ExitReason = ExitCodeEnum.NoProfile;
                    return;
                }
                if (this.m_CurrentInstal.plugin == null)
                {
                    this.ExitReason = ExitCodeEnum.NoPlugins;
                    return;
                }

                if (this.ScriptMinVersion < this.CurrentInstal.profile.FullMinVersion)
                {
                    this.ExitReason = ExitCodeEnum.MinVersionError;
                    return;
                }

                w.WriteLine(AppCommon.Instance.CodeAutoGenMsg);
                w.WriteLine(AppCommon.Instance.CodeLicenseHeader);
                w.Write("; Profile Hash:");
                w.WriteLine(this.CurrentInstal.profile.UniqueName);

                this.WritePlugins();
                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.NoError;
                }
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.PlugingHotstringWriteGeneralFail);
                return;
            }
         
        }

        private void WritePlugins()
        {
            try
            {
                foreach (var p in this.CurrentInstal.plugin)
                {
                    if (p.enabled == false)
                    {
                        continue;
                    }

                    if (this.ScriptMinVersion < p.FullMinVersion)
                    {
                        // The Script version is not high enough to run this plugin.
                        continue;
                    }

                   // HashBuilder.AppendLine(p.UniqueName);

                    this.WritePlugin(p);
                }

                if (HashBuilder.Length > 0)
                {
                    w.WriteLine();
                    w.Write("; Hotstrings Hash Values:");
                    w.WriteLine(this.GeHashBuildertHash());
                }
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.PlugingHotstringWriteGeneralFail);
                return;
            }
           
        }

        private void WritePlugin(plugin p)
        {
            if (p.hotstrings == null)
            {
                return;
            }
            try
            {
                foreach (var hs in p.hotstrings)
                {
                    if (hs.enabled == false)
                    {
                        continue;
                    }
                    switch (hs.type)
                    {
                       
                        case hotstringType.Paste:
                            HashBuilder.AppendLine(hs.UniqueName);
                            this.WriteHotstringPaste(hs);
                            break;
                        case hotstringType.Inline:
                            HashBuilder.AppendLine(hs.UniqueName);
                            this.WriteHotstringInline(hs);
                            break;
                        case hotstringType.Code:
                            HashBuilder.AppendLine(hs.UniqueName);
                            this.WriteHotstringCode(hs);
                            break;
                        default:
                            break;
                    }
                   
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        #region Hot String type Code related
        private void WriteHotstringCode(hotstring hs)
        {
            try
            {
                if (this.HotstringSet.Contains(hs.trigger) == true)
                {
                    // hotstring has already been used, most likely in another plugin., will continue with little notice
                    w.WriteLine();
                    w.Write("; Hotstring '");
                    w.Write(hs.trigger);
                    w.WriteLine("' was used previously and can not be used again");
                    w.WriteLine();
                    return;
                }
                this.HotstringSet.Add(hs.trigger);

                w.Write(":");
                // w.Write(hs.Options.ToAutoHotkeyString());

                var opt = hs.Options;
                if (opt.HasKey[HotStringOptionsEnum.CaseSensitive.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.CaseSensitive.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.OmitEndChar.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.OmitEndChar.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.TriggerInside.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.TriggerInside.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.AutomaticBackSpaceOff.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.AutomaticBackSpaceOff.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.ResetRecognizer.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.ResetRecognizer.GetMapValue());
                }
                w.Write(":");
                w.Write(hs.trigger);
                w.WriteLine("::");
                w.WriteLine(bo);
                w.Indent++;
                // string code = hs.code.CleanNewLine();
                // w.WriteLine(code);
                string aLine;
                StringReader strReader = new StringReader(hs.code);
                while (true)
                {
                    aLine = strReader.ReadLine();
                    if (aLine != null)
                    {
                        w.WriteLine(aLine);
                    }
                    else
                    {
                       break;
                    }
                }
                strReader = null;
                w.WriteLine("return");
                w.Indent--;
                w.Write(bc);
                w.WriteLine();
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NoError || this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.HsWriteHotstringCode;
                }
                throw;
            }
        }
        #endregion

        #region Hot string type Inline Related
        private void WriteHotstringInline(hotstring hs)
        {
            try
            {
                if (this.HotstringSet.Contains(hs.trigger) == true)
                {
                    // hotstring has already been used, most likely in another plugin., will continue with little notice
                    w.WriteLine();
                    w.Write("; Hotstring '");
                    w.Write(hs.trigger);
                    w.WriteLine("' was used previously and can not be used again");
                    w.WriteLine();
                    return;
                }
                this.HotstringSet.Add(hs.trigger);

                bool WriteSingleLine = true;
                WriteSingleLine &= string.IsNullOrEmpty(hs.sendkeys);
                WriteSingleLine &= (hs.replacements == null || hs.replacements.Length == 0);

                string sCode = hs.code.CleanNewLine().Trim();

                WriteSingleLine &= sCode.IndexOf(Environment.NewLine) == -1;


              
                if (WriteSingleLine == true)
                {
                    w.Write(":");
                    w.Write(hs.Options.ToAutoHotkeyString());
                    w.Write(":");
                    w.Write(hs.trigger);
                    w.Write("::");
                                      
                    w.WriteLine(sCode);
                    return;
                }

                w.Write(":");
                var opt = hs.Options;
                if (opt.HasKey[HotStringOptionsEnum.CaseSensitive.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.CaseSensitive.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.OmitEndChar.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.OmitEndChar.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.TriggerInside.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.TriggerInside.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.AutomaticBackSpaceOff.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.AutomaticBackSpaceOff.GetMapValue());

                }
                if (opt.HasKey[HotStringOptionsEnum.ResetRecognizer.ToString()] == true)
                {
                    w.Write(HotStringOptionsEnum.ResetRecognizer.GetMapValue());
                }
                w.Write(":");
                w.Write(hs.trigger);
                w.WriteLine("::");
                w.WriteLine(bo);
                w.Indent++;

                this.WriteHotstringSnippCode(hs);
                this.WriteReplacements(hs);
                this.WriteSendKeyWord(hs);


                w.Write("%");
                w.Write(@"varCode");
                w.WriteLine("%");
                if (string.IsNullOrEmpty(hs.sendkeys) == false)
                {
                    this.WriteSendKeyWord(hs);
                    w.WriteLine(hs.sendkeys);
                }
                w.WriteLine(@"return");
                w.Indent--;
                w.WriteLine(bc);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Writes Send, SendInput, SendPlay or SendEvent followed by a space
        /// </summary>
        /// <param name="hs">The current hotstring</param>
        private void WriteSendKeyWord(hotstring hs)
        {
            switch (hs.Options.SendMethod)
            {
                case Library.AHKSnipit.AHKSnipitLib.Enums.HotStringSendEnum.SendInput:
                    w.Write(@"SendInput ");
                    break;
                case Library.AHKSnipit.AHKSnipitLib.Enums.HotStringSendEnum.SendPlay:
                    w.Write(@"SendPlay ");
                    break;
                case Library.AHKSnipit.AHKSnipitLib.Enums.HotStringSendEnum.SendEvent:
                    w.Write(@"SendEvent ");
                    break;
                case Library.AHKSnipit.AHKSnipitLib.Enums.HotStringSendEnum.None:
                case Library.AHKSnipit.AHKSnipitLib.Enums.HotStringSendEnum.Send:
                default:
                    w.Write(@"Send ");
                    break;
            }
        }
        #endregion

        #region Hot string type Paste related
        private void WriteHotstringPaste(hotstring hs)
        {
            try
            {
                if (this.HotstringSet.Contains(hs.trigger) == true)
                {
                    // hotstring has already been used, most likely in another plugin., will continue with little notice
                    w.WriteLine();
                    w.Write("; Hotstring '");
                    w.Write(hs.trigger);
                    w.WriteLine("' was used previously and can not be used again");
                    w.WriteLine();
                    return;
                }
                this.HotstringSet.Add(hs.trigger);

                w.Write(":");
                w.Write(hs.Options.ToAutoHotkeyString());
                w.Write(":");
                w.Write(hs.trigger);
                w.WriteLine("::");
                w.WriteLine(bo);
                w.Indent++;
                
                w.WriteLine(@"try");
                w.WriteLine(bo);
                w.Indent++;
                
                this.WriteHotstringSnippCode(hs);
                this.WriteReplacements(hs);

                w.Write("varTabify := ");
                w.WriteLine(hs.tabify.ToString().ToLower());

                w.Write("varForceClear := ");
                w.WriteLine(hs.forceclear.ToString().ToLower());

                w.Write("PasteSnipText(varCode, \"");
                w.Write(hs.trigger);
                w.Write("\",");
                w.Write("varTabify, ");
                w.WriteLine("varForceClear)");

                if (string.IsNullOrEmpty(hs.sendkeys) == false)
                {
                    w.WriteLine(@"Sleep, 200");
                    w.Write(@"send, ");
                    w.WriteLine(hs.sendkeys);
                }

                w.WriteLine("return");

                w.Indent--;
                w.WriteLine(bc);
                w.WriteLine(@"catch e");
                w.WriteLine(bo);
                w.Indent++;
                w.WriteLine(@"DisplayError(e)");
                w.Indent--;
                w.WriteLine(bc);
                w.WriteLine("return");
                w.Indent--;
                w.WriteLine(bc);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        #region Write Hot string Item Code
        private void WriteHotstringSnippCode(hotstring hs)
        {
            try
            {
                if (string.IsNullOrEmpty(hs.code))
                {
                    return;
                }
                string strFileName = hs.UniqueName + AppCommon.Instance.DefaultSnippitExt;
                w.Write(@"varCode");
                w.Write(" := GetSnippitInlineText(\"");
                w.Write(strFileName);
                w.WriteLine("\")");

                w.Write("if (MfString.IsNullOrEmpty(");
                w.Write("varCode");
                w.WriteLine("))");
                w.WriteLine(bo);
                w.Indent++;

                w.WriteLine(@"return");

                w.Indent--;
                w.WriteLine(bc);
            }
            catch (Exception)
            {

                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.HsWriteHotstrinPasteCode;
                }
                throw;
            }
           
           
        }
        #endregion

        #region Replacement Methods
        private void WriteReplacements(hotstring hs)
        {
            try
            {
                if (hs.replacements == null || hs.replacements.Length == 0)
                {
                    return;
                }

                foreach (var r in hs.replacements)
                {
                    if (r is inputFixedList)
                    {
                        this.WriteReplacementInputFixedList(r as inputFixedList);
                    }
                    else if (r is inputReplacement)
                    {
                        this.WriteReplacementInputReplacement(r as inputReplacement);
                    }

                }
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.HsWriteReplacements;
                }
                throw;
            }
           
        }

        private void WriteReplacementInputFixedList(inputFixedList rep)
        {
            try
            {
                if (rep.listValues == null || rep.listValues.Length == 0)
                {
                    return;
                }
                string strList = string.Empty;
                foreach (var li in rep.listValues)
                {
                    if (li.defaultSpecified == true && li.@default == defaultType.@true)
                    {
                        strList = strList + li.Value + "||"; // Double || makes the this item be selected

                    }
                    else {
                        strList = strList + li.Value + "|";
                    }

                }

                w.Write(@"varList");
                w.Write(" := \"");
                w.Write(strList);
                w.WriteLine("\"");

                w.Write(@"varListResult");
                w.Write(" := ");
                w.Write("cInputListLong(\"");
                w.Write(rep.dialogtitle);
                w.Write("\", \"");
                w.Write(rep.dialogtext);
                w.Write("\", ");
                w.Write(@"varList");
                w.WriteLine(")");

                w.Write(@"if(MfString.IsNullOrEmpty(");
                w.Write(@"varListResult");
                w.WriteLine("))");
                w.WriteLine(bo);
                w.Indent++;
                w.WriteLine("return");
                w.Indent--;
                w.WriteLine(bc);

                w.Write("varCode");
                w.Write(" := ");
                w.Write(@"Mfunc.StringReplace(");
                w.Write(@"varCode");
                w.Write(", \"");
                w.Write(rep.placeholder);
                w.Write("\", ");
                w.Write(@"varListResult");
                w.WriteLine(", 1)");
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.HsWriteReplacementInputFixedList;
                }
                throw;
            }
            

        }

        private void WriteReplacementInputReplacement(inputReplacement rep)
        {
            try
            {
                w.Write("varInput");
                w.Write(" := ");
                w.Write("cInputBox(\"");
                w.Write(rep.dialogtitle);
                w.Write("\", \"");
                w.Write(rep.dialogtext);
                w.Write("\"");
                if (string.IsNullOrEmpty(rep.dialoginitialvalue) == false)
                {
                    if (rep.dialoginitialvalue.ToLower() == "%clipboard%")
                    {

                        w.Write(@", ");
                        w.Write(@"Public.GetFirstLineOnly(");
                        w.Write(@"Clipboard)");
                    }
                    else
                    {
                        w.Write(", \"");
                        w.Write(rep.dialoginitialvalue);
                        w.Write("\"");
                    }

                }

                w.WriteLine(@")");
                w.Write(@"if(MfString.IsNullOrEmpty(");
                w.Write(@"varInput");
                w.WriteLine(@"))");
                w.WriteLine(bo);
                w.Indent++;
                w.WriteLine(@"return");
                w.Indent--;
                w.WriteLine(bc);

                w.Write(@"varCode");
                w.Write(@" := ");
                w.Write(@"Mfunc.StringReplace(");
                w.Write(@"varCode");
                w.Write(", \"");
                w.Write(rep.placeholder);
                w.Write("\", ");
                w.Write(@"varInput");
                w.WriteLine(", 1)");
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet)
                {
                    this.ExitReason = ExitCodeEnum.HsWriteReplacementInputReplacement;
                }
                throw;
            }
           
        }
        #endregion
        #endregion

        #endregion

        #region Save Snippit to disk
        internal void SaveSnippitToFile()
        {
            try
            {
                string sPath = this.CurrentInstal.profile.codeLanguage.paths.SnippitInlinePath;
                if (Directory.Exists(sPath))
                {
                    string searchP = "*" + AppCommon.Instance.DefaultSnippitExt;
                    var files = Directory.GetFiles(sPath, searchP);
                    foreach (string f in files)
                    {
                        File.Delete(f);
                    }
                }
                else
                {
                    Directory.CreateDirectory(sPath);
                }
               
                foreach (var p in CurrentInstal.plugin)
                {

                    if (p.enabled == false)
                    {
                        continue;
                    }
                    if (p.hotstrings != null)
                    {
                        try
                        {
                            Parallel.ForEach(p.hotstrings, hs =>
                            {
                                if (hs.enabled == true)
                                {
                                    string sFile = hs.UniqueName + AppCommon.Instance.DefaultSnippitExt;
                                    string sFilePath = Path.Combine(sPath, sFile);
                                    string code = hs.code.CleanNewLine();
                                    File.WriteAllText(sFilePath, code);
                                }
                               
                            });
                        }
                        catch (Exception)
                        {
                            if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                            {
                                this.ExitReason = ExitCodeEnum.FileWriteError;
                            }
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.SetExitReason(ExitCodeEnum.HsInlineSaveSnippitToFile);
                return;
            }
        }
             
        #endregion


    }
}
