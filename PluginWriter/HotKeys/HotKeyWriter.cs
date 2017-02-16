using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.Base;

namespace BigByteTechnologies.Windows.Console.AHKSnipit.PluginWriter.HotKeys
{
    internal class HotKeyWriter : WriterBase
    {
        #region Constructor
        internal HotKeyWriter() : base()
        {
            this.m_PluginKeyFile = Path.Combine(AppCommon.Instance.PathData, Properties.Settings.Default.PluginKeysFile);
            this.MultiKeys = new Dictionary<string, List<command>>();
            this.LabelSet = new HashSet<string>();
            this.RegularKeySet = new HashSet<string>();
            this.MultiKeySet = new HashSet<string>();

        }
        internal HotKeyWriter(SnippitInstal si) : this()
        {
            this.m_CurrentInstal = si;
        }
        #endregion;

        #region Fields/Members
        Dictionary<string, List<command>> MultiKeys;
        private HashSet<string> LabelSet;
        private HashSet<string> RegularKeySet;
        private HashSet<string> MultiKeySet;
        #endregion



        #region Properties

        private string m_PluginKeyFile;
        /// <summary>
        /// Specifies the Plugin Key include file for the Plugin Keys in HotHotkey.
        /// </summary>
        public string PluginKeyFile
        {
            get
            {
                return m_PluginKeyFile;
            }
        }
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
        #endregion

        #region Write Methods
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


                foreach (var p in this.m_CurrentInstal.plugin)
                {
                    if (p.enabled == false)
                    {
                        continue;
                    }
                    if (p.commands == null)
                    {
                        continue;
                    }

                   // HashBuilder.AppendLine(p.UniqueName);

                    if (this.ScriptMinVersion < p.FullMinVersion)
                    {
                        // The Script version is not high enough to run this plugin.
                        continue;
                    }
                    foreach (var cmd in p.commands)
                    {
                        if (cmd.enabled == false)
                        {
                            continue;
                        }
                        if (cmd.KeyData.IsValid == false)
                        {
                            continue;
                        }
                        HashBuilder.AppendLine(cmd.UniqueName);
                        if (cmd.KeyData.IsMultiKey == true)
                        {
                            string PrefixMain = cmd.KeyData.GetPrefix() + cmd.KeyData.Key1.ToString();
                            if (this.MultiKeys.ContainsKey(PrefixMain) == true)
                            {
                                this.MultiKeys[PrefixMain].Add(cmd);
                            }
                            else
                            {
                                List<command> lst = new List<command>();
                                lst.Add(cmd);
                                this.MultiKeys.Add(PrefixMain, lst);
                            }
                            continue;
                        }
                        this.WriteRegular(cmd);

                    }
                }
                this.WriteMultiKeys();

                if (HashBuilder.Length > 0)
                {
                    w.WriteLine();
                    w.Write("; HotKeys Hash Values:");
                    w.WriteLine(this.GeHashBuildertHash());
                }
            }
            catch (Exception)
            {

                if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                {
                    this.ExitReason = ExitCodeEnum.HkGeneralError;
                }
                return;
            }

            if (this.ExitReason == ExitCodeEnum.NotSet)
            {
                this.ExitReason = ExitCodeEnum.NoError;
            }
        }

        private void WriteRegular(command cmd)
        {
            try
            {
                if (cmd.enabled == false)
                {
                    return;
                }
                if (string.IsNullOrEmpty(cmd.label) == true)
                {
                    // label can not be null
                    w.WriteLine();
                    w.Write("; Label for command '");
                    w.Write(cmd.name);
                    w.WriteLine("' was empty. Unable to write this command");
                    w.WriteLine();
                    return;
                }
                if (this.LabelSet.Contains(cmd.label))
                {
                    // label has already been used, will continue with little notice
                    w.WriteLine();
                    w.Write("; Label '");
                    w.Write(cmd.label);
                    w.WriteLine("' was used previously and can not be used again");
                    w.WriteLine();
                    return;
                }
                this.LabelSet.Add(cmd.label);

                string sKey = cmd.KeyData.ToString();
                if (this.RegularKeySet.Contains(sKey))
                {
                    // key combination has already been used, will continue with little notice
                    w.WriteLine();
                    w.Write("; Keys '");
                    w.Write(sKey);
                    w.WriteLine("' was used previously and can not be used again");
                    w.WriteLine();
                    return;
                }
                this.RegularKeySet.Add(sKey);

                w.Write(sKey);
                w.WriteLine(@"::");
                w.WriteLine(bo);
                w.Indent++;

                w.Write("__mylbl := \"");
                w.Write(cmd.label);
                w.WriteLine("\"");

                w.WriteLine(@"if (IsLabel(__mylbl))");
                w.WriteLine(bo);
                w.Indent++;
                w.WriteLine(@"gosub, %__mylbl%");
                w.Indent--;
                w.WriteLine(bc);
                w.WriteLine("return");
                w.Indent--;
                w.WriteLine(bc);
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                {
                    this.ExitReason = ExitCodeEnum.HkWriteRegular;
                }
                throw;
            }

        }

        private void WriteMultiKeys()
        {
            try
            {
                foreach (var kv in this.MultiKeys)
                {
                    string sKey = kv.Key;
                    if (this.RegularKeySet.Contains(sKey))
                    {
                        // key has already been used, will continue with little notice
                        w.WriteLine();
                        w.Write("; Key '");
                        w.Write(sKey);
                        w.WriteLine("' was used previously and can not be used again");
                        w.WriteLine();
                        return;
                    }
                    this.RegularKeySet.Add(sKey);

                    w.Write(sKey);
                    w.WriteLine(@"::");
                    w.WriteLine(bo);
                    w.Indent++;

                    w.WriteLine(@"Input userKey, L1 T2 I");

                    w.WriteLine("if (ErrorLevel = \"Timeout\")");
                    w.WriteLine(bo);
                    w.Indent++;
                    w.WriteLine(@"return");
                    w.Indent--;
                    w.WriteLine(bc);
                    w.WriteLine("if (ErrorLevel <> \"Max\")");
                    w.WriteLine(bo);
                    w.Indent++;
                    w.WriteLine(@"return");
                    w.Indent--;
                    w.WriteLine(bc);

                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        command cmd = kv.Value[i];
                        if (string.IsNullOrEmpty(cmd.label) == true)
                        {
                            // label can not be null
                            w.WriteLine();
                            w.Write("; Label for command '");
                            w.Write(cmd.name);
                            w.WriteLine("' was empty. Unable to write this command");
                            w.WriteLine();
                            continue;
                        }
                        if (this.LabelSet.Contains(cmd.label))
                        {
                            // label has already been used, will continue with little notice
                            w.WriteLine();
                            w.Write("; Label '");
                            w.Write(cmd.label);
                            w.WriteLine("' was used previously and can not be used again");
                            w.WriteLine();
                            continue;
                        }
                        this.LabelSet.Add(cmd.label);
                        string MultiKeyString = sKey + " - " + cmd.KeyData.Key2.ToString();
                        if (this.MultiKeySet.Contains(MultiKeyString))
                        {
                            // Key Par has already been used, will continue with little notice
                            w.WriteLine();
                            w.Write("; Key Pair '");
                            w.Write(MultiKeyString);
                            w.WriteLine("' was used previously and can not be used again");
                            w.WriteLine();
                            continue;
                        }
                        this.MultiKeySet.Add(MultiKeyString);

                        if (i == 0)
                        {
                            w.Write(@"If (userKey = ");
                        }
                        else
                        {
                            w.Write(@"else If (userKey = ");
                        }
                        w.Write("\"");
                        // Multi-key value use a different method then single key method
                        // There is a different maping value then the default HotKeysEnum value
                        // Check an see if there is a map to remap the value and if not write the default.
                        // It is most likely that if there is no maping value then the multi-key will not work.
                        string keyName;
                        if (cmd.KeyData.Key2.Has2MapValue())
                        {
                            keyName = cmd.KeyData.Key2.GetKey2MapValue();
                        }
                        else
                        {
                            keyName = cmd.KeyData.Key2.ToString();
                        }
                        w.Write(keyName);
                        w.WriteLine("\")");
                        w.WriteLine(bo);
                        w.Indent++;
                       
                        w.Write("__mylbl := \"");
                        w.Write(cmd.label);
                        w.WriteLine("\"");

                        w.WriteLine(@"if (IsLabel(__mylbl))");
                        w.WriteLine(bo);
                        w.Indent++;

                        w.Write(@"KeyWait, ");
                        w.Write(keyName);
                        w.WriteLine(@", T1"); // continue after 1 second of waiting
                        // could write if errorlevel here but we are going to continue if keywait is not finished

                        w.WriteLine(@"gosub, %__mylbl%");
                        w.Indent--;
                        w.WriteLine(bc);

                        w.Indent--;
                        w.WriteLine(bc);
                    }
                    w.WriteLine(@"__mylbl := Null");
                    w.WriteLine(@"userKey := Null");
                    w.WriteLine("return");
                    w.Indent--;
                    w.WriteLine(bc);
                } // foreach (var kv in this.MultiKeys)
            }
            catch (Exception)
            {
                if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                {
                    this.ExitReason = ExitCodeEnum.HkWriteMultiKeys;
                }
                throw;
            }

        }
        #endregion

        #region Save Snippit to disk
        internal void SaveCodeToFile()
        {
            try
            {
                string sPath = this.CurrentInstal.profile.codeLanguage.paths.PluginFullPath;
                if (Directory.Exists(sPath))
                {
                    string searchP = "*.ahk";
                    var files = Directory.GetFiles(sPath, searchP);
                    Parallel.ForEach(files, f => {
                        File.Delete(f);
                    });
                }
                else
                {
                    Directory.CreateDirectory(sPath);
                }
                foreach (var cmd in this.CurrentInstal.plugin)
                {
                    if (cmd.enabled == false)
                    {
                        continue;
                    }
                    if (cmd.commands != null)
                    {
                        try
                        {
                            Parallel.ForEach(cmd.commands, c =>
                            {
                                if (c.enabled == true)
                                {
                                    string sFile = c.UniqueName + ".ahk";
                                    string sFilePath = Path.Combine(sPath, sFile);
                                    string code = c.code.CleanNewLine();
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine(AppCommon.Instance.CodeAutoGenMsg);
                                    sb.AppendLine(AppCommon.Instance.CodeLicenseHeader);
                                    sb.Append(code);
                                    File.WriteAllText(sFilePath, sb.ToString(), Encoding.UTF8);
                                }
                            });
                        }
                        catch (Exception)
                        {
                            if (this.ExitReason == ExitCodeEnum.NoError || this.ExitReason == ExitCodeEnum.NotSet)
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
                if (this.ExitReason == ExitCodeEnum.NotSet || this.ExitReason == ExitCodeEnum.NoError)
                {
                    this.ExitReason = ExitCodeEnum.HkSaveCodeToFile;
                }
                return;
            }
        }
        #endregion
    }
}
