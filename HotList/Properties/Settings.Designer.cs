﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("snippit")]
        public string DefaultSnippitExt {
            get {
                return ((string)(this["DefaultSnippitExt"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0""?>
<SnippitInstal xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
 xsi:noNamespaceSchemaLocation=""SnippitInstal.xsd"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <profile version=""0.1.0.0"" minVersion=""0.2.0"" name=""sample"">
        <codeLanguage>
            <codeName>Sample</codeName>
            <paths>
                <mainData>sample</mainData>
            </paths>
        </codeLanguage>
        <windows>
            <window>
                <name>Notepad</name>
                <value>ahk_class Notepad</value>
            </window>
        </windows>
    </profile>
    <plugin version=""0.1"" profileName=""sample"" name=""sample"">
        <hotstrings>
            <hotstring trigger=""hw"">
                <name>Hello World</name>
                <description>Hello World</description>
                <category>Sample</category>
                <forceclear>true</forceclear>
                <snippit>hello world</snippit>
                <type>inline</type>
            </hotstring>
        </hotstrings>
    </plugin>
</SnippitInstal>")]
        public string DefaultInstalXml {
            get {
                return ((string)(this["DefaultInstalXml"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastDataBinaryImportLocation {
            get {
                return ((string)(this["LastDataBinaryImportLocation"]));
            }
            set {
                this["LastDataBinaryImportLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastDataTextImportLocation {
            get {
                return ((string)(this["LastDataTextImportLocation"]));
            }
            set {
                this["LastDataTextImportLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState SiWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["SiWindowState"]));
            }
            set {
                this["SiWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point SiLocation {
            get {
                return ((global::System.Drawing.Point)(this["SiLocation"]));
            }
            set {
                this["SiLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("689, 451")]
        public global::System.Drawing.Size SiSize {
            get {
                return ((global::System.Drawing.Size)(this["SiSize"]));
            }
            set {
                this["SiSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point PlgLocation {
            get {
                return ((global::System.Drawing.Point)(this["PlgLocation"]));
            }
            set {
                this["PlgLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("826, 556")]
        public global::System.Drawing.Size PlgSize {
            get {
                return ((global::System.Drawing.Size)(this["PlgSize"]));
            }
            set {
                this["PlgSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState PlgWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["PlgWindowState"]));
            }
            set {
                this["PlgWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState ProWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["ProWindowState"]));
            }
            set {
                this["ProWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point ProLocation {
            get {
                return ((global::System.Drawing.Point)(this["ProLocation"]));
            }
            set {
                this["ProLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("613, 370")]
        public global::System.Drawing.Size ProSize {
            get {
                return ((global::System.Drawing.Size)(this["ProSize"]));
            }
            set {
                this["ProSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SiLastImportLocationInstall {
            get {
                return ((string)(this["SiLastImportLocationInstall"]));
            }
            set {
                this["SiLastImportLocationInstall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SiLastExportLocationInstall {
            get {
                return ((string)(this["SiLastExportLocationInstall"]));
            }
            set {
                this["SiLastExportLocationInstall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SiLastExportLocationPlugin {
            get {
                return ((string)(this["SiLastExportLocationPlugin"]));
            }
            set {
                this["SiLastExportLocationPlugin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ValLastSiLocation {
            get {
                return ((string)(this["ValLastSiLocation"]));
            }
            set {
                this["ValLastSiLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ValLastProLocation {
            get {
                return ((string)(this["ValLastProLocation"]));
            }
            set {
                this["ValLastProLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ValLastPlgLocation {
            get {
                return ((string)(this["ValLastPlgLocation"]));
            }
            set {
                this["ValLastPlgLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState ValWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["ValWindowState"]));
            }
            set {
                this["ValWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point ValLocation {
            get {
                return ((global::System.Drawing.Point)(this["ValLocation"]));
            }
            set {
                this["ValLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("442, 276")]
        public global::System.Drawing.Size ValSize {
            get {
                return ((global::System.Drawing.Size)(this["ValSize"]));
            }
            set {
                this["ValSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("80")]
        public int SiColKeyWidth {
            get {
                return ((int)(this["SiColKeyWidth"]));
            }
            set {
                this["SiColKeyWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("120")]
        public int SiColModWidth {
            get {
                return ((int)(this["SiColModWidth"]));
            }
            set {
                this["SiColModWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("106")]
        public int SiColNameWidth {
            get {
                return ((int)(this["SiColNameWidth"]));
            }
            set {
                this["SiColNameWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("220")]
        public int SiColDescWidth {
            get {
                return ((int)(this["SiColDescWidth"]));
            }
            set {
                this["SiColDescWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int SiColCategoryWidth {
            get {
                return ((int)(this["SiColCategoryWidth"]));
            }
            set {
                this["SiColCategoryWidth"] = value;
            }
        }
    }
}
