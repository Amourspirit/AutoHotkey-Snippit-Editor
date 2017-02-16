using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Exceptions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using CM = BigByteTechnologies.Library.Windows.CommandManagement;
using System.ComponentModel;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.IO;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms
{

    public partial class SnippitList : Form
    {
        #region Fields
        bool IsInitialized = false;
        //bool DataDirty = false;
        Plugins CurrentPlugins;
        Color m_ColorItemForeColorDisabled = Color.Gray;
        Color m_ColorItemforecolorDefault = Color.Black;
        Sort.ListViewItemSorter oSorter;
        #endregion

        #region Properties
        /// <summary>
        /// Gets/Sets the current Profile
        /// </summary>
        /// <remarks>
        /// This initially will be the profile set in the ini file.
        /// </remarks>
        protected profile CurrentProfile { get; set; }

        private profile m_IniProfile;
        /// <summary>
        /// Gets the current profile from the ini file
        /// </summary>
        protected profile IniProfile
        {
            get
            {
                if (this.m_IniProfile == null)
                {
                    string sPath = Path.Combine(AppCommon.Instance.PathProfiles, IniHelper.GetCurrentProfile());
                    this.m_IniProfile = this.GetProfile(sPath);
                }
                return this.m_IniProfile;
            }
        }

        private Version m_ScriptVersion;

        /// <summary>
        /// Gets the version of the current script
        /// </summary>
        public Version ScriptVersion
        {
            get
            {
                if (this.m_ScriptVersion == null)
                {
                    this.m_ScriptVersion = this.GetScriptVersion();
                }
                return m_ScriptVersion;
            }
        }


        #endregion

        #region constructor
        public SnippitList()
        {
            this.InitializeComponent();
            this.InitializeCommandManager();
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;
            this.oSorter = new Sort.ListViewItemSorter();
            this.lvMain.ListViewItemSorter = null;
            this.SetLvMainColumnIconDefault();
            this.Init();

        }
        #endregion

        #region Init
        internal void Init()
        {
            this.m_IniProfile = null;
            this.EnsureValidProfile();
            this.PopulateProfiles();
            this.SetCurrentProfile();
            this.SetCurrentPlugins();
            this.PopulateDataFilesList();
            this.TogglePluginEnabledControls();
            this.ToggleItemEnabledControls();
        }
        #endregion

        #region Ensure Valid Profile
        /// <summary>
        /// Checks to see if there is a valid profile installed.
        /// If there is no valid profile installed then a sample profile will be installed and activated.
        /// </summary>
        /// <remarks>
        /// This method will ensure a working profile, even if the user deletes all the profiles that are installed.
        /// Deleting the sample profile with no other profile installed will result in the sample profile being 
        /// created again.
        /// </remarks>
        private void EnsureValidProfile()
        {
            string sProfile = this.GetCurrentProfile();
            bool bValid = true;
            if (string.IsNullOrEmpty(sProfile))
            {
                // search for the first available profile
                bValid = false;
            }

            if (bValid == true)
            {
                bValid = false;
                string sProfilePath = Path.Combine(AppCommon.Instance.PathProfiles, sProfile);
                if (File.Exists(sProfilePath))
                {
                    var result = ValidateXml.ValidateProfileFile(sProfilePath);
                    bValid = !result.HasErrors;
                }
            }

            if (bValid == true)
            {
                return;
            }
            bValid = true;

            profile p = this.GetFirstProfile();
            if (p == null)
            {
                bValid = false;
            }
            if (bValid == true)
            {
                SetCurrentProfile(Path.GetFileName(p.File));
                return;
            }
            try
            {
                this.WriteDefaultProfile();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            bValid = true;
            p = this.GetFirstProfile();
            if (p == null)
            {
                bValid = false;
            }
            if (bValid == true)
            {
                SetCurrentProfile(Path.GetFileName(p.File));
                return;
            }
            MessageBox.Show(this
                , Properties.Resources.ErrorNoProfileText
                , Properties.Resources.ErrorNoProfileTitle
                , MessageBoxButtons.OK
                , MessageBoxIcon.Error);
            this.Close();

        }

        /// <summary>
        /// Reads default profile data from resources and writes to disk
        /// </summary>
        private void WriteDefaultProfile()
        {
            SnippitInstal si;
            si = SnippitInstal.FromXml(Properties.Settings.Default.DefaultInstalXml);
            this.SaveProfile(si.profile);
            ReadWrite.SavePlugin(si, true);
        }

        /// <summary>
        /// Sets the ini value of the current Profile
        /// </summary>
        /// <param name="currentProfile">the file name of the current profile to set</param>
        private void SetCurrentProfile(string currentProfile)
        {
            IniHelper.SetCurrentProfile(currentProfile);

        }

        /// <summary>
        /// Gets the first valid profile from the profiles folder
        /// </summary>
        /// <returns>
        /// A profile instance representing the first found profile in the profiles folder; If no
        /// valid profile is found then null
        /// </returns>
        private profile GetFirstProfile()
        {
            string[] files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");
            if (files.GetLength(0) <= 0)
            {
                return null;
            }
            foreach (string file in files)
            {
                var result = ValidateXml.ValidateProfileFile(file);
                if (result.HasErrors == false)
                {
                    profile p = profile.FromFile(file);
                    return p;
                }
            }
            return null;
        }
        #endregion

        #region Commands

        #region Command Enable Methods
        /// <summary>
        /// Gets if the currently selected main listview item can be toggle to enabled or disabled
        /// </summary>
        /// <remarks>
        /// The current toggle state is not taken into consideration. Only if the item can be toggled to enabled or disabled.
        /// </remarks>
        /// <returns>
        /// True if There is a selected Listview Item and that item can be disable or enabled. Otherwise false.
        /// </returns>
        private bool EnabledLvToggleItem()
        {
            bool b = true;
            b &= this.IsLvItemSelected;
            if (b == true)
            {
                var tag = this.GetLvItemFirstSelectedTag();
                if (tag == null)
                {
                    return false;
                }
                b = false;
                if ((tag.PluginType == PluginEnum.HotKey) || (tag.PluginType == PluginEnum.HotString))
                {
                    b = true;
                }
            }
            return b;
        }
        #endregion

        #region Command Properties
        private TriEnum m_IsEnableSwapProflie = TriEnum.NotSet;
        /// <summary>
        /// Gets if Swap Profile should be enabled
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsEnableSwapProflie
        {
            get
            {
                if (m_IsEnableSwapProflie == TriEnum.NotSet)
                {
                    m_IsEnableSwapProflie = TriEnum.False;
                    bool b = true;
                    b &= !this.IsCurrentProfileIniProfile;
                    if (b == true)
                    {
                        b &= !this.CurrentPlugins.HasChangedData;

                    }
                    if (b == false)
                    {
                        return false;
                    }
                    this.m_IsEnableSwapProflie = TriEnum.True;

                }
                return m_IsEnableSwapProflie == TriEnum.True;
            }
        }

        private TriEnum m_IsEnableSave = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Save Commands should be enabled
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsEnableSave
        {
            get
            {
                if (this.m_IsEnableSave == TriEnum.NotSet)
                {
                    this.m_IsEnableSave = TriEnum.False;
                    if (this.CurrentPlugins == null)
                        return false;
                    if (this.CurrentPlugins.HasChangedData == true)
                        this.m_IsEnableSave = TriEnum.True;
                }
                return this.m_IsEnableSave == TriEnum.True;
            }
        }

        private TriEnum m_IsCurrentProfileValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Current Selected Profile is valid
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsCurrentProfileValid
        {
            get
            {
                if (m_IsCurrentProfileValid == TriEnum.NotSet)
                {
                    m_IsCurrentProfileValid = TriEnum.False;
                    if (this.CurrentProfile == null)
                    {
                        return false;
                    }
                    bool b = this.CurrentProfile.GetIsValid();
                    if (b == true)
                    {
                        this.m_IsCurrentProfileValid = TriEnum.True;
                    }

                }
                return m_IsCurrentProfileValid == TriEnum.True;
            }
        }

        private TriEnum m_IsCurrentProfileIniProfile = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Current Selected Profile is the same as the Profile set in the script ini file.
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsCurrentProfileIniProfile
        {
            get
            {
                if (this.m_IsCurrentProfileIniProfile == TriEnum.NotSet)
                {
                    this.m_IsCurrentProfileIniProfile = TriEnum.False;
                    if (this.IsCurrentProfileValid == false)
                    {
                        return false;
                    }
                    if (string.Equals(this.IniProfile.name, this.CurrentProfile.name, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        this.m_IsCurrentProfileIniProfile = TriEnum.True;
                    }
                }
                return this.m_IsCurrentProfileIniProfile == TriEnum.True;
            }
        }



        /// <summary>
        /// Gets if the if the conditions are met to enable Plugin Move or Copy to another profile
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsPluginMoveCopy
        {
            get
            {
                bool b = true;
                b &= this.IsCleanPluginSelected;
                b &= ddlProfiles.Items.Count > 1;
                return b;
            }
        }

        private TriEnum m_IsCleanPluginSelected = TriEnum.NotSet;
        /// <summary>
        /// Gets if the if the Current Profile is set, Plugin is selected and Plugin as no Changed Data
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsCleanPluginSelected
        {
            get
            {
                if (this.m_IsCleanPluginSelected == TriEnum.NotSet)
                {
                    this.m_IsCleanPluginSelected = TriEnum.False;
                    if (this.CurrentProfile == null)
                    {
                        return false;
                    }
                    if (this.CurrentPlugins == null)
                    {
                        return false;
                    }
                    bool b = true;
                    b &= this.CurrentPlugins.IsCurrentKeyValid;
                    b &= !this.CurrentPlugins.HasChangedData;
                    if (b == true)
                    {
                        this.m_IsCleanPluginSelected = TriEnum.True;
                    }
                }
                return this.m_IsCleanPluginSelected == TriEnum.True;
            }
        }

        private TriEnum m_IsPluginsValidClean = TriEnum.NotSet;
        /// <summary>
        /// Gets if the if the Current Profile is set, Plugin has data and Plugin as no Changed Data
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsPluginsValidClean
        {
            get
            {
                if (this.m_IsPluginsValidClean == TriEnum.NotSet)
                {
                    this.m_IsPluginsValidClean = TriEnum.False;
                    if (this.CurrentProfile == null)
                    {
                        return false;
                    }
                    if (this.CurrentPlugins == null)
                    {
                        return false;
                    }
                    bool b = true;
                    b &= !this.CurrentPlugins.HasChangedData;
                    if (b == true)
                    {
                        this.m_IsPluginsValidClean = TriEnum.True;
                    }
                }
                return this.m_IsPluginsValidClean == TriEnum.True;
            }
        }

        private TriEnum m_IsLvItemSelected = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Main Listview has at least one selected Item
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// </remarks>
        private bool IsLvItemSelected
        {
            get
            {
                if (this.m_IsLvItemSelected == TriEnum.NotSet)
                {
                    this.m_IsLvItemSelected = TriEnum.False;
                    if (this.GetLvItemFirstSelectedTag() != null)
                    {
                        this.m_IsLvItemSelected = TriEnum.True;
                    }

                }
                return this.m_IsLvItemSelected == TriEnum.True;
            }
        }

        private TriEnum m_IsEnabledLvToggleItem = TriEnum.NotSet;
        /// <summary>
        /// Gets if the currently selected main listview item can be toggle to enabled or disabled
        /// </summary>
        /// <remarks>
        /// Property is re-evaluated on each command idle event.
        /// The current toggle state is not taken into consideration. Only if the item can be toggled to enabled or disabled.
        /// </remarks>
        private bool IsEnabledLvToggleItem
        {
            get
            {
                if (this.m_IsEnabledLvToggleItem == TriEnum.NotSet)
                {
                    this.m_IsEnabledLvToggleItem = TriEnum.False;
                    if (EnabledLvToggleItem() == true)
                    {
                        this.m_IsEnabledLvToggleItem = TriEnum.True;
                    }
                }
                return this.m_IsEnabledLvToggleItem == TriEnum.True;
            }
        }
        #endregion

        #region Events
        private void CommandPrePorcess(object sender, CancelEventArgs e)
        {
            // use pre-process event to trigger setting of value only once per idle event
            this.m_IsEnableSwapProflie = TriEnum.NotSet;
            this.m_IsCurrentProfileValid = TriEnum.NotSet;
            this.m_IsCurrentProfileIniProfile = TriEnum.NotSet;
            this.m_IsEnableSave = TriEnum.NotSet;
            this.m_IsCleanPluginSelected = TriEnum.NotSet;
            this.m_IsLvItemSelected = TriEnum.NotSet;
            this.m_IsEnabledLvToggleItem = TriEnum.NotSet;
            this.m_IsPluginsValidClean = TriEnum.NotSet;
        }
        #endregion

        #region Command Management Init
        /// <summary>
        /// Initializes the Command Manager
        /// </summary>
        private void InitializeCommandManager()
        {
            cmdMgr.PreCommandProcess += CommandPrePorcess;

            cmdMgr.Commands.Add(new CM.Command("CmdExit", new CM.Command.ExecuteHandler(CmdExit), new CM.Command.UpdateHandler(CmdExitHandler)));
            cmdMgr.Commands["CmdExit"].CommandInstances.AddRange(new object[] { mnuExit, tsBtnExit });

            cmdMgr.Commands.Add(new CM.Command("CmdSave", new CM.Command.ExecuteHandler(CmdSave), new CM.Command.UpdateHandler(CmdSaveHandler)));
            cmdMgr.Commands["CmdSave"].CommandInstances.AddRange(new object[] { mnuSave, tsBtnSave });

            cmdMgr.Commands.Add(new CM.Command("CmdPluginToggle", new CM.Command.ExecuteHandler(CmdPluginToggle), new CM.Command.UpdateHandler(CmdPluginToggleHandler)));
            cmdMgr.Commands["CmdPluginToggle"].CommandInstances.AddRange(new object[] { mnuPluginToggle, tsBtnPluginToggle });

            cmdMgr.Commands.Add(new CM.Command("CmdLvToggleItem", new CM.Command.ExecuteHandler(CmdLvToggleItem), new CM.Command.UpdateHandler(CmdLvToggleItemHandler)));
            cmdMgr.Commands["CmdLvToggleItem"].CommandInstances.AddRange(new object[] { mnuItemToggle, mnuLvItemToggle });

            cmdMgr.Commands.Add(new CM.Command("CmdValidatePlugin", new CM.Command.ExecuteHandler(CmdValidatePlugin), new CM.Command.UpdateHandler(CmdValidatePluginHandler)));
            cmdMgr.Commands["CmdValidatePlugin"].CommandInstances.Add(mnuValidatePlugin);

            cmdMgr.Commands.Add(new CM.Command("CmdValidateInstall", new CM.Command.ExecuteHandler(CmdValidateInstall), new CM.Command.UpdateHandler(CmdValidatePluginHandler)));
            cmdMgr.Commands["CmdValidateInstall"].CommandInstances.Add(mnuValidateInstall);

            cmdMgr.Commands.Add(new CM.Command("CmdExportProfile", new CM.Command.ExecuteHandler(CmdExportEntireProfile), new CM.Command.UpdateHandler(CmdImportExportHandler)));
            cmdMgr.Commands["CmdExportProfile"].CommandInstances.Add(mnuFileExportPluginsProfile);

           

            // CmdExportPluginOnlyHandler
            cmdMgr.Commands.Add(new CM.Command("CmdExportPluginOnly", new CM.Command.ExecuteHandler(CmdExportPluginOnly), new CM.Command.UpdateHandler(CmdExportPluginOnlyHandler)));
            cmdMgr.Commands["CmdExportPluginOnly"].CommandInstances.Add(mnuFileExportSelectePlugin);

            cmdMgr.Commands.Add(new CM.Command("CmdImportProfile", new CM.Command.ExecuteHandler(CmdImport), new CM.Command.UpdateHandler(CmdImportExportHandler)));
            cmdMgr.Commands["CmdImportProfile"].CommandInstances.Add(mnuImport);

            cmdMgr.Commands.Add(new CM.Command("CmdDeletePlugin", new CM.Command.ExecuteHandler(CmdDeletePlugin), new CM.Command.UpdateHandler(CmdPluginToggleHandler)));
            cmdMgr.Commands["CmdDeletePlugin"].CommandInstances.Add(mnuPluginDelete);

            cmdMgr.Commands.Add(new CM.Command("CmdDeleteProfile", new CM.Command.ExecuteHandler(CmdDeleteProfile), new CM.Command.UpdateHandler(CmdDeleteProfileHandler)));
            cmdMgr.Commands["CmdDeleteProfile"].CommandInstances.Add(mnuProfileDelete);

            cmdMgr.Commands.Add(new CM.Command("CmdSwapProfile", new CM.Command.ExecuteHandler(CmdSwapProfile), new CM.Command.UpdateHandler(CmdSwapProfileHandler)));
            cmdMgr.Commands["CmdSwapProfile"].CommandInstances.AddRange(new object[] { mnuProfileSwap, tsBtnSwap });

            cmdMgr.Commands.Add(new CM.Command("CmdProfileNew", new CM.Command.ExecuteHandler(CmdProflieNew), new CM.Command.UpdateHandler(CmdProflieNewHandler)));
            cmdMgr.Commands["CmdProfileNew"].CommandInstances.Add(mnuProfileNew);
            cmdMgr.Commands["CmdProfileNew"].CommandInstances.Add(tsProfileNew);

            cmdMgr.Commands.Add(new CM.Command("CmdProfileEdit", new CM.Command.ExecuteHandler(CmdProflieEdit), new CM.Command.UpdateHandler(CmdProflieEditHandler)));
            cmdMgr.Commands["CmdProfileEdit"].CommandInstances.Add(mnuProfileEdit);
            cmdMgr.Commands["CmdProfileEdit"].CommandInstances.Add(tsProfileEdit);

            cmdMgr.Commands.Add(new CM.Command("CmdPluginNew", new CM.Command.ExecuteHandler(CmdPluginNew), new CM.Command.UpdateHandler(CmdPluginNewHandler)));
            cmdMgr.Commands["CmdPluginNew"].CommandInstances.Add(mnuPluginNew);
            cmdMgr.Commands["CmdPluginNew"].CommandInstances.Add(tsPluginNew);

            cmdMgr.Commands.Add(new CM.Command("CmdPluginEdit", new CM.Command.ExecuteHandler(CmdPluginEdit), new CM.Command.UpdateHandler(CmdPluginEditHandler)));
            cmdMgr.Commands["CmdPluginEdit"].CommandInstances.AddRange(new object[] { mnuLvItemEdit, mnuPluginEdit, tsPluginEdit });

            cmdMgr.Commands.Add(new CM.Command("CmdPluginMoveToProfile", new CM.Command.ExecuteHandler(CmdPluginMoveToProfle), new CM.Command.UpdateHandler(CmdPluginMoveToProfleHandler)));
            cmdMgr.Commands["CmdPluginMoveToProfile"].CommandInstances.Add(mnuPluginMoveTo);
            cmdMgr.Commands["CmdPluginMoveToProfile"].CommandInstances.Add(tsPluginMoveTo);

            cmdMgr.Commands.Add(new CM.Command("CmdPluginCopyToProfile", new CM.Command.ExecuteHandler(CmdPluginCopyToProfle), new CM.Command.UpdateHandler(CmdPluginCopyToProfleHandler)));
            cmdMgr.Commands["CmdPluginCopyToProfile"].CommandInstances.Add(mnuPluginCopyTo);
            cmdMgr.Commands["CmdPluginCopyToProfile"].CommandInstances.Add(tsProfileCopyTo);

            cmdMgr.Commands.Add(new CM.Command("CmdAbout", new CM.Command.ExecuteHandler(CmdAbout), new CM.Command.UpdateHandler(CmdNullHandler)));
            cmdMgr.Commands["CmdAbout"].CommandInstances.Add(mnuAbout);

            //mnuHelpAhkSnipitHelp
            cmdMgr.Commands.Add(new CM.Command("CmdHelpAhkSnipitHelp", new CM.Command.ExecuteHandler(CmdHelpAhkSnipitHelp), new CM.Command.UpdateHandler(CmdNullHandler)));
            cmdMgr.Commands["CmdHelpAhkSnipitHelp"].CommandInstances.Add(mnuHelpAhkSnipitHelp);

        }
        #endregion

        #region Command Handlers
        private void CmdNullHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
        }
        private void CmdPluginEditHandler(CM.Command cmd)
        {
            bool b = true;
            b &= !Editor.frmPlugin.IsInstance;
            b &= (this.IsPluginMoveCopy == true || this.IsLvItemSelected == true);
            b &= this.IsPluginsValidClean;
            cmd.Enabled = b;

        }

        private void CmdPluginCopyToProfleHandler(CM.Command cmd)
        {
            cmd.Enabled = this.IsPluginMoveCopy;
        }

        private void CmdPluginMoveToProfleHandler(CM.Command cmd)
        {
            cmd.Enabled = this.IsPluginMoveCopy;
        }

        private void CmdPluginNewHandler(CM.Command cmd)
        {
            if (this.CurrentProfile == null)
            {
                cmd.Enabled = false;
                return;
            }
            bool b = true;
            b &= !Editor.frmPlugin.IsInstance;
            b &= this.CurrentPlugins != null;
            if (b == true)
            {
                b &= !this.CurrentPlugins.HasChangedData;
            }
            cmd.Enabled = b;
        }

        private void CmdProflieEditHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
            cmd.Enabled &= !Editor.frmProfile.IsInstance;
            cmd.Enabled &= this.CurrentProfile != null;
            if (cmd.Enabled == true)
            {
                // if save is enabled then export should be disabled
                CmdSaveHandler(cmd);
                cmd.Enabled = !cmd.Enabled;
            }
        }

        private void CmdProflieNewHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
            cmd.Enabled &= !Editor.frmProfile.IsInstance;
            if (cmd.Enabled == true)
            {
                // if save is enabled then export should be disabled
                CmdSaveHandler(cmd);
                cmd.Enabled = !cmd.Enabled;
            }
        }



        /// <summary>
        /// Command handler to enable or disable menu items for Swap Profile
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdSwapProfile(CM.Command)"/>
        private void CmdSwapProfileHandler(CM.Command cmd)
        {
            cmd.Enabled = this.IsEnableSwapProflie;

        }

        /// <summary>
        /// Command handler to enable or disable menu items for Delete Profile
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdDeletePlugin(CM.Command)"/>
        private void CmdDeleteProfileHandler(CM.Command cmd)
        {
            CmdImportExportHandler(cmd);
        }

        /// <summary>
        /// Command handler to enable or disable menu items for Export
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        private void CmdImportExportHandler(CM.Command cmd)
        {
            bool b = true;
            b &= this.CurrentProfile != null;
            b &= !this.IsEnableSave;
            mnuExport.Enabled = b;
            cmd.Enabled = b;
           
        }

        /// <summary>
        /// Command handler to enable or disable menu items for Export Selected Plugin
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        private void CmdExportPluginOnlyHandler(CM.Command cmd)
        {
            bool b = true;
            b &= this.IsCleanPluginSelected;
            cmd.Enabled = b;
        }

        /// <summary>
        /// Command handler to enable or disable menu items Validate Plugins
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdValidatePlugin(CM.Command)"/>
        private void CmdValidatePluginHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
        }
        /// <summary>
        /// Command handler to enable or disable menu items for enable and disable on list view items.
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdLvToggleItem(CM.Command)"/>
        private void CmdLvToggleItemHandler(CM.Command cmd)
        {

            cmd.Enabled = this.IsEnabledLvToggleItem;

        }


        /// <summary>
        /// Command handler to enable or disable commands for the select all text
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdExit(CM.Command)"/>
        private void CmdExitHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
        }

        /// <summary>
        /// Command handler to enable or disable commands for Save
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdSave(CM.Command)"/>
        private void CmdSaveHandler(CM.Command cmd)
        {
            cmd.Enabled = this.IsEnableSave;
            ddlProfiles.Enabled = !this.IsEnableSave;
        }


        /// <summary>
        /// Command handler to allow Disable option for plugin
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdPluginEnable(CM.Command)"/>
        private void CmdPluginToggleHandler(CM.Command cmd)
        {
            cmd.Enabled = true;
            cmd.Enabled &= (ddlFiles.SelectedIndex > 0);
        }


        #endregion

        #region Command Methods
        private void CmdAbout(CM.Command cmd)
        {
            var frm = new About.frmAbout();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void CmdHelpAhkSnipitHelp(CM.Command cmd)
        {
            Help.ShowHelp(this, @"./" + Properties.Resources.FileHelpName);
        }

        private void CmdPluginNew(CM.Command cmd)
        {
            Editor.frmPlugin frm;
            frm = new Editor.frmPlugin();
            frm.Plugin.profileName = this.CurrentProfile.name;
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                string sFile = frm.Plugin.name.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase) == true ? frm.Plugin.name : frm.Plugin.name + ".xml";
                frm.Plugin.File = Path.Combine(AppCommon.Instance.PathXml, this.CurrentProfile.name, sFile);
                frm.Plugin.schemaVersion = AppCommon.Instance.MAxAllowSchemaVersion;
                SavePlugin(frm.Plugin);
#if RELOAD
                // run the script to load the plugin writher
                this.StartScript();
#endif
            }
            frm.Dispose();

        }

        private void CmdPluginEdit(CM.Command cmd)
        {
            Editor.frmPlugin frm;
            frm = new Editor.frmPlugin();
            plugin plg;
            PluginTagInfo tag = this.GetLvItemFirstSelectedTag();
            if (tag != null)
            {
                plg = plugin.FromFile(this.CurrentPlugins[tag.PluginKey].PluginFile);
            }
            else
            {
                plg = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
            }

            frm.Plugin = plg;

            if (tag != null)
            {
                switch (tag.PluginType)
                {
                    case PluginEnum.HotString:
                        frm.InitialHotString = this.CurrentPlugins[tag.PluginKey].ItemHotStrings[tag.ItemKey].Trigger;
                        break;
                    case PluginEnum.HotKey:
                        frm.InitailCommand = this.CurrentPlugins[tag.PluginKey].ItemHotKeys[tag.ItemKey].Hotkey;
                        break;
                    case PluginEnum.IncludeHotString:
                        frm.InitialInclude = this.CurrentPlugins[tag.PluginKey].ItemIncludeHotStrings[tag.ItemKey].ParentInclude.name;
                        break;
                    case PluginEnum.IncludeHotKey:
                        frm.InitialInclude = this.CurrentPlugins[tag.PluginKey].ItemIncludeHotkeys[tag.ItemKey].ParentInclude.name;
                        break;
                    default:
                        break;
                }
            }
            try
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    SavePlugin(frm.Plugin);
#if RELOAD
                    // restart the script if it is running
                    this.StartScript();
#endif
                    SelectPlugin(frm.Plugin);

                }
            }
            catch (Exception e)
            {

                this.DisplayGeneralError(e.Message);
            }
           
            frm.Dispose();
        }

        private void CmdPluginCopyToProfle(CM.Command cmd)
        {
            Editor.Dialog.frmProflieSelection frm;
            frm = new Editor.Dialog.frmProflieSelection();
            frm.CurrentProfleFile = this.CurrentProfile.File;

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var ToProfile = profile.FromFile(frm.ProfileResult);
                    var Plg = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
                    Plg.ParentProfile = ToProfile;
                    Plg.profileName = ToProfile.name;
                    Plg.File = string.Empty;
                    ReadWrite.SavePlugin(Plg, true);
#if RELOAD
                    // if destination profile is the ini profile then we need to reload the script if it is running
                    if (string.Equals(this.IniProfile.name, ToProfile.name, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        this.StartScript();
                    }
#endif
                    MessageBox.Show(this
                        , string.Format(Properties.Resources.PluginCopySuccessText, Plg.name)
                        , Properties.Resources.PluginCopySuccessTitle
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information);

                }
                catch (Exception e)
                {

                    this.DisplayGeneralError(e.Message);
                }
            }
            frm.Dispose();
        }

        private void CmdPluginMoveToProfle(CM.Command cmd)
        {
            Editor.Dialog.frmProflieSelection frm;
            frm = new Editor.Dialog.frmProflieSelection();
            frm.CurrentProfleFile = this.CurrentProfile.File;

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var ToProfile = profile.FromFile(frm.ProfileResult);
                    var Plg = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
                    Plg.ParentProfile = ToProfile;
                    Plg.profileName = ToProfile.name;
                    Plg.File = string.Empty;
                    ReadWrite.SavePlugin(Plg, true);

                    // reload from current plugin so we can now delete the this actual plugin.
                    Plg = null;
                    Plg = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
                    Plg.ParentProfile = ToProfile;

                    ReadWrite.DeletePlugin(Plg);
#if RELOAD
                    // if destination profile is the ini profile then we need to reload the script if it is running
                    if (string.Equals(this.IniProfile.name, ToProfile.name, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        this.StartScript();
                    }
#endif
                    KeyValuePair<string, string> kv = (KeyValuePair<string, string>)ddlProfiles.SelectedItem;

                    this.Init();
                    this.ddlProfiles.SelectedItem = kv;

                    MessageBox.Show(this
                        , string.Format(Properties.Resources.PluginMoveSuccessText, Plg.name)
                        , Properties.Resources.PluginMoveSuccessTitle
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information);

                }
                catch (Exception e)
                {

                    this.DisplayGeneralError(e.Message);
                }
            }
            frm.Dispose();
        }

        private void CmdProflieEdit(CM.Command cmd)
        {
            Editor.frmProfile frm = new Editor.frmProfile();
            frm.FromProfile(this.CurrentProfile);

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                this.Init();
#if RELOAD
                this.StartScript();
#endif
            }
            frm.Dispose();
        }

        private void CmdProflieNew(CM.Command cmd)
        {

            Editor.frmProfile frm = new Editor.frmProfile();

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                this.Init();
            }
            frm.Dispose();

        }

        /// <summary>
        /// Performs the command that takes action to swap the profile
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CCmdSwapProfiletHandler(CM.Command)"/>
        private void CmdSwapProfile(CM.Command cmd)
        {
            if (this.CurrentProfile.GetIsValid() == true)
            {
                if (!string.IsNullOrEmpty(this.CurrentProfile.File))
                {

                    this.SetCurrentProfile(Path.GetFileName(this.CurrentProfile.File));
                    this.m_IniProfile = null; // reset the ini profile so it refreshes
#if RELOAD
                    this.StartScript();
#endif
                    this.Text = string.Format("{0} - {1}", Properties.Resources.FormHotListTitle, this.CurrentProfile.name);
                }
                else
                {
                    MessageBox.Show(this
                     , Properties.Resources.ErrorSettingIniProfileNoFileText
                     , Properties.Resources.ErrorSettingIniProfileNoFileTitle
                     , MessageBoxButtons.OK
                     , MessageBoxIcon.Error);
                }


            }
        }

        /// <summary>
        /// Performs the command that Deletes the Profile
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdDeleteProfileHandler(CM.Command)"/>
        private void CmdDeleteProfile(CM.Command cmd)
        {
            var result = MessageBox.Show(this
               , string.Format(Properties.Resources.DeleteProfileConfirmText, Environment.NewLine, this.CurrentProfile.name)
               , Properties.Resources.DeleteConfirmTitle
               , MessageBoxButtons.OKCancel
               , MessageBoxIcon.Question
               , MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                // set the current key to empty to get the delete method to
                // delete all the plugins in the profile
                CurrentPlugins.CurrentKey = string.Empty;
                this.Delete();
            }
        }

        /// <summary>
        /// Performs the command that Deletes the Plugin
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CCmdDeletePlugintHandler(CM.Command)"/>
        private void CmdDeletePlugin(CM.Command cmd)
        {
            var result = MessageBox.Show(this
                , string.Format(Properties.Resources.DeletePluginConfirmText, Environment.NewLine, this.CurrentPlugins.CurrentPlugin.Name)
                , Properties.Resources.DeleteConfirmTitle
                , MessageBoxButtons.OKCancel
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                this.Delete();
            }

        }
        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdImportExportHandler(CM.Command)"/>
        private void CmdImport(CM.Command cmd)
        {
            this.Import();
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdImportExportHandler(CM.Command)"/>
        private void CmdExportEntireProfile(CM.Command cmd)
        {
            this.ExportSelected(false);
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdImportExportHandler(CM.Command)"/>
        private void CmdExportPluginOnly(CM.Command cmd)
        {
            this.ExportSelected(true);
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdExitHandler(CM.Command)"/>
        private void CmdValidatePlugin(CM.Command cmd)
        {
            var frm = new Validation.ValidatePluginForm(XmlKindEnum.Plugin);
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdExitHandler(CM.Command)"/>
        private void CmdValidateInstall(CM.Command cmd)
        {
            var frm = new Validation.ValidatePluginForm(XmlKindEnum.SnippitInstl);
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Command to Enable or disable List view item.
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdLvToggleItemHandler(CM.Command)"/>
        private void CmdLvToggleItem(CM.Command cmd)
        {
            ListView.SelectedIndexCollection indexes = this.lvMain.SelectedIndices;
            if (indexes.Count == 1)
            {
                var lvItm = this.lvMain.Items[indexes[0]];
                PluginTagInfo tag = (PluginTagInfo)lvItm.Tag;
                if ((tag.PluginType == PluginEnum.HotKey) || (tag.PluginType == PluginEnum.HotString))
                {
                    bool bEnabled = !tag.Enabled;
                    if (this.CurrentPlugins.ContainsKey(tag.PluginKey))
                    {

                        if (tag.PluginType == PluginEnum.HotKey)
                        {
                            if (this.CurrentPlugins[tag.PluginKey].ContainsHotkey(tag.ItemKey))
                            {
                                this.CurrentPlugins[tag.PluginKey].ItemHotKeys[tag.ItemKey].Enabled = bEnabled;
                                tag.Enabled = bEnabled;
                            }
                        }
                        if (tag.PluginType == PluginEnum.HotString)
                        {
                            if (this.CurrentPlugins[tag.PluginKey].ContainsHotString(tag.ItemKey))
                            {
                                this.CurrentPlugins[tag.PluginKey].ItemHotStrings[tag.ItemKey].Enabled = bEnabled;
                                tag.Enabled = bEnabled;
                            }
                        }
                    }
                    this.UpdateLvItemToggled();
                    this.ToggleMenuLvItm();
                }
            }

        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdExitHandler(CM.Command)"/>
        private void CmdExit(CM.Command cmd)
        {
            this.Close();
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdSaveHandler(CM.Command)"/>
        private void CmdSave(CM.Command cmd)
        {
            bool Saved = this.CurrentPlugins.Save();
#if RELOAD
            // if we are editing the plugins of a current running script the restart the script
            if (this.CurrentProfileIsSelected() == true)
            {
                this.StartScript();
            }
#endif
        }

        /// <summary>
        /// Performs the command that disables the current plugin
        /// </summary>
        /// <param name="cmd">Instance of <see cref="CM.Command"/> to manage the control state</param>
        /// <seealso cref="CmdPluginEnabledHandler(CM.Command)"/>
        private void CmdPluginToggle(CM.Command cmd)
        {
            if (this.CurrentPlugins.IsCurrentKeyValid == true)
            {
                this.CurrentPlugins.CurrentPlugin.Enabled = !this.CurrentPlugins.CurrentPlugin.Enabled;
                this.TogglePluginEnabledControls();
            }

        }

#endregion

#endregion

        #region Controls
        /// <summary>
        /// Toggles the Control Image and Text related to Plugin Enable and disable
        /// </summary>
        private void TogglePluginEnabledControls()
        {
            bool setDefault = true;
            if (this.CurrentPlugins.IsCurrentKeyValid == true)
            {
                //this.DataDirty = true;
                if (this.CurrentPlugins.CurrentPlugin.Enabled == false)
                {
                    setDefault = false;
                    this.mnuPluginToggle.Image = Properties.Resources.red_light16;
                    // add menu key activator only if is not part of the resource string
                    if (Properties.Resources.EnableText.IndexOf('&') >= 0)
                    {
                        this.mnuPluginToggle.Text = Properties.Resources.EnableText;
                    } else
                    {
                        this.mnuPluginToggle.Text = @"&" + Properties.Resources.EnableText;
                    }


                    this.tsBtnPluginToggle.Image = Properties.Resources.red_light16;
                    this.tsBtnPluginToggle.Text = Properties.Resources.EnableText;
                    this.tsBtnPluginToggle.ToolTipText = Properties.Resources.PluginEnableTooltip;
                }

            }
            if (setDefault == true)
            {
                this.mnuPluginToggle.Image = Properties.Resources.green_light16;
                // add menu key activator only if is not part of the resource string
                if (Properties.Resources.DisableText.IndexOf('&') >= 0)
                {
                    this.mnuPluginToggle.Text = Properties.Resources.DisableText;
                } else
                {
                    this.mnuPluginToggle.Text = @"&" + Properties.Resources.DisableText;
                }

                this.tsBtnPluginToggle.Image = Properties.Resources.green_light16;
                this.tsBtnPluginToggle.Text = Properties.Resources.DisableText;
                this.tsBtnPluginToggle.ToolTipText = Properties.Resources.PluginDisableTooltip;
            }
        }

        /// <summary>
        /// Toggles the menu Items Text and image to Enable or Disable plugin items
        /// </summary>
        /// <remarks>Used to Set menu Items when Listview menu is opened</remarks>
        private void ToggleMenuLvItm()
        {
            bool setDefault = true;
            ListView.SelectedIndexCollection indexes = this.lvMain.SelectedIndices;
            if (indexes.Count == 1)
            {
                var lvItm = this.lvMain.Items[indexes[0]];
                PluginTagInfo tag = (PluginTagInfo)lvItm.Tag;
                if (tag.Enabled == false)
                {
                    setDefault = false;
                    this.mnuItemToggle.Image = Properties.Resources.flag_red_16;
                    this.mnuLvItemToggle.Image = Properties.Resources.flag_red_16;
                    if (Properties.Resources.DisableText.IndexOf('&') >= 0)
                    {
                        this.mnuItemToggle.Text = Properties.Resources.EnableText;

                    }
                    else
                    {
                        this.mnuItemToggle.Text = @"&" + Properties.Resources.EnableText;

                    }
                    this.mnuLvItemToggle.Text = Properties.Resources.EnableText;
                    this.mnuLvItemToggle.Text = Properties.Resources.EnableText;
                }

            }

            if (setDefault == true)
            {
                this.mnuItemToggle.Image = Properties.Resources.flag_green_16;
                this.mnuLvItemToggle.Image = Properties.Resources.flag_green_16;
                if (Properties.Resources.DisableText.IndexOf('&') >= 0)
                {
                    this.mnuItemToggle.Text = Properties.Resources.DisableText;

                }
                else
                {
                    this.mnuItemToggle.Text = @"&" + Properties.Resources.DisableText;

                }
                this.mnuLvItemToggle.Text = Properties.Resources.DisableText;
                this.mnuLvItemToggle.Text = Properties.Resources.DisableText;
            }
        }

        private void ToggleItemEnabledControls()
        {
            bool setDefault = true;

            if (setDefault == true)
            {
                this.mnuItemToggle.Image = Properties.Resources.flag_green_16;
                this.mnuLvItemToggle.Image = Properties.Resources.flag_green_16;
                // add menu key activator only if is not part of the resource string
                if (Properties.Resources.DisableText.IndexOf('&') >= 0)
                {
                    this.mnuItemToggle.Text = Properties.Resources.DisableText;
                }
                else
                {
                    this.mnuItemToggle.Text = @"&" + Properties.Resources.DisableText;
                }
                this.mnuLvItemToggle.Text = Properties.Resources.DisableText;

            }
        }

#endregion

        #region ListView

        #region Event Handlers

        private void lvMenu_Opening(object sender, CancelEventArgs e)
        {
            if (lvMain.SelectedIndices.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            this.mnuLvItemToggle.Enabled = this.EnabledLvToggleItem();
        }

        private void lvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ToggleMenuLvItm();
        }

        private void lvMain_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvMain.View != View.Details)
            {
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                this.oSorter.Column = e.Column;
                this.oSorter.ColumnType = oSorter.DetectColumnType(lvMain, e.Column);
                oSorter.InvertSortOrder();

                if (lvMain.ListViewItemSorter == null)
                {
                    lvMain.ListViewItemSorter = oSorter;
                }
                else
                {
                    lvMain.Sort();
                }
                for (int i = 0; i < lvMain.Columns.Count; i++)
                {
                    if (i == e.Column)
                        switch (oSorter.SortOrder)
                        {
                            case SortOrder.Ascending:
                                lvMain.Columns[i].ImageKey = "Down";
                                break;
                            case SortOrder.Descending:
                                lvMain.Columns[i].ImageKey = "Up";
                                break;
                            default:
                                lvMain.Columns[i].ImageKey = "Empty";
                                break;
                        }
                    else
                        lvMain.Columns[i].ImageKey = "Empty";

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            


        }

        private void lvMain_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hit = lvMain.HitTest(e.Location);
                if (hit.Item != null)
                {
                    hit.Item.Selected = true;
                }
                else
                {
                    lvMain.SelectedItems.Clear();
                }
            }
        }
#endregion

        /// <summary>
        ///  Sets the default transparent image for each column.
        ///  This acts as a placeholder for the sort image when it is displayed
        /// </summary>
        private void SetLvMainColumnIconDefault()
        {
            for (int i = 0; i < lvMain.Columns.Count; i++)
            {
                lvMain.Columns[i].ImageKey = "Empty";
            }
        }

        /// <summary>
        /// Gets the Tag for the first selected list item
        /// </summary>
        /// <returns>
        /// <see cref="PluginTagInfo"/> for the first selected list item. Or Null if no Item is selected
        /// </returns>
        private PluginTagInfo GetLvItemFirstSelectedTag()
        {
            ListView.SelectedIndexCollection indexes = this.lvMain.SelectedIndices;
            if (indexes.Count >= 1)
            {
                var lvItm = this.lvMain.Items[indexes[0]];
                PluginTagInfo tag = (PluginTagInfo)lvItm.Tag;
                return tag;
            }
            return default(PluginTagInfo);
        }

        /// <summary>
        /// Updates image on listview if enabled or disabled
        /// </summary>
        private void UpdateLvItemToggled()
        {
            ListView.SelectedIndexCollection indexes = this.lvMain.SelectedIndices;
            if (indexes.Count == 1)
            {
                var lvItm = this.lvMain.Items[indexes[0]];
                PluginTagInfo tag = (PluginTagInfo)lvItm.Tag;
                switch (tag.PluginType)
                {
                    case PluginEnum.HotString:
                        if (tag.Enabled == true)
                        {
                            lvItm.ImageIndex = 0;
                            lvItm.ForeColor = this.m_ColorItemforecolorDefault;
                        }
                        else
                        {
                            lvItm.ImageIndex = 2;
                            lvItm.ForeColor = this.m_ColorItemForeColorDisabled;
                        }
                        break;
                    case PluginEnum.HotKey:
                        if (tag.Enabled == true)
                        {
                            lvItm.ImageIndex = 1;
                            lvItm.ForeColor = this.m_ColorItemforecolorDefault;
                        }
                        else
                        {
                            lvItm.ImageIndex = 3;
                            lvItm.ForeColor = this.m_ColorItemForeColorDisabled;
                        }
                        break;
                    case PluginEnum.IncludeHotString:
                    case PluginEnum.IncludeHotKey:
                    default:
                        break;
                }

            }

        }

        /// <summary>
        /// Clears and Populates the Listview with the Current Hotkeys and Hotstrings
        /// </summary>
        private void PopulateListView()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            this.lvMain.BeginUpdate();
            try
            {
                

                this.lvMain.ListViewItemSorter = null;
                this.lvMain.Items.Clear();

                string FilterFile = string.Empty;
                string FilterType = string.Empty;
                string FilterCategory = string.Empty;
                HotTypeEnum HotType = HotTypeEnum.None;

                // Check to see if we have a valid plugin file or All Plugin files selected
                if (ddlFiles.SelectedIndex >= 0)
                {
                    KeyValuePair<string, string> SelectedFile = (KeyValuePair<string, string>)ddlFiles.SelectedItem;
                    if (string.IsNullOrEmpty(SelectedFile.Value))
                    {
                        // No valid entry return at this point
                        return;
                    }
                    FilterFile = SelectedFile.Value;
                }



                // check to see if the type is set all or hotkey or hotstring
                if (ddlType.SelectedIndex >= 0)
                {
                    KeyValuePair<string, string> SelectedType = (KeyValuePair<string, string>)ddlType.SelectedItem;
                    switch (SelectedType.Key)
                    {
                        case "HotKey":
                            HotType = HotTypeEnum.Hotkey;
                            break;
                        case "HotString":
                            HotType = HotTypeEnum.HotString;
                            break;
                        case "All":
                            HotType = HotTypeEnum.HotString | HotTypeEnum.Hotkey;
                            break;
                        default:
                            HotType = HotTypeEnum.None;
                            break;
                    }
                }
                else
                {
                    // by off chance that not a valid Type selection then default to all
                    HotType = HotTypeEnum.HotString | HotTypeEnum.Hotkey;
                }


                // check to see if category is set to all or specific category
                if (ddlCategory.SelectedIndex >= 0)
                {
                    KeyValuePair<string, string> SelectedCat = (KeyValuePair<string, string>)ddlCategory.SelectedItem;
                    if (string.IsNullOrEmpty(SelectedCat.Value))
                    {
                        // if by some unlikely chance the filter is null or empty assign as all
                        FilterCategory = AppCommon.All;
                    }
                    else
                    {
                        FilterCategory = SelectedCat.Value;
                    }
                }
                else
                {
                    // if by off chance not a valid Category Selection default to all
                    FilterCategory = AppCommon.All;
                }

                List<ListViewItem> cols = new List<ListViewItem>();

                // Gets the list of Display items. Method will handle 'All' filters as well
                var dlst = this.CurrentPlugins.ToDisplayList(FilterFile, HotType, FilterCategory);
                foreach (var dl in dlst)
                {
                    ListViewItem lvi;
                    if (dl.PluginType == PluginEnum.HotKey || dl.PluginType == PluginEnum.IncludeHotKey)
                    {
                        HotkeyKeys hks;
                        if (HotkeyKeys.TryParse(dl.Text, out hks) == false)
                        {
                            lvi = new ListViewItem(dl.Text);
                            lvi.SubItems.Add(string.Empty);
                        }
                        else
                        {
                            lvi = new ListViewItem(hks.ToReeadableKeys());
                            lvi.SubItems.Add(hks.ToReadableMainModiferString());
                        }


                    }
                    else
                    {
                        lvi = new ListViewItem(dl.Text);
                        lvi.SubItems.Add(string.Empty);
                    }



                    lvi.SubItems.Add(dl.Name);
                    lvi.SubItems.Add(dl.Description);
                    lvi.SubItems.Add(dl.Category);
                    PluginTagInfo tag = new PluginTagInfo();
                    tag.PluginType = dl.PluginType;
                    tag.Enabled = dl.Enabled;
                    tag.ItemKey = dl.Key;
                    tag.PluginKey = dl.PluginFile;

                    lvi.Tag = tag;

                    if (tag.Enabled)
                    {
                        lvi.ForeColor = this.m_ColorItemforecolorDefault;
                    }
                    else
                    {
                        lvi.ForeColor = this.m_ColorItemForeColorDisabled;
                    }

                    switch (dl.PluginType)
                    {
                        case PluginEnum.HotString:
                            if (dl.Enabled == true)
                            {
                                lvi.ImageIndex = 0;
                            }
                            else
                            {
                                lvi.ImageIndex = 2;
                            }
                            break;
                        case PluginEnum.HotKey:
                            if (dl.Enabled == true)
                            {
                                lvi.ImageIndex = 1;
                            }
                            else
                            {
                                lvi.ImageIndex = 3;
                            }
                            break;
                        case PluginEnum.IncludeHotString:
                            lvi.ImageIndex = 0;
                            break;
                        case PluginEnum.IncludeHotKey:
                            lvi.ImageIndex = 1;
                            break;
                        default:
                            break;
                    }
                    cols.Add(lvi);
                }
                cols.Sort(Sort.ListViewSort.CompareItemText);
                for (int i = 0; i < cols.Count; i++)
                {

                    this.lvMain.Items.Add(cols[i]);
                }
                this.lvMain.ListViewItemSorter = oSorter;
                this.lvMain.Sort();
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                this.lvMain.EndUpdate();
            }
            

        }

        
#endregion

        #region Drop Down List

        private void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.IsInitialized == true)
            {
                this.SetDdlTypeData();
                string sFile = this.GetSelectedPluginFile();
                this.CurrentPlugins.CurrentKey = sFile;
                this.TogglePluginEnabledControls();


            }

        }

        private void ddlProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsInitialized == true)
            {
                this.SetCurrentProfile();
                this.SetCurrentPlugins();
                this.PopulateDataFilesList();
                this.TogglePluginEnabledControls();
                this.ToggleItemEnabledControls();
            }
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsInitialized == true)
            {
                this.PopulateCategoryList();
            }
        }

        private void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsInitialized == true)
            {
                this.PopulateListView();
            }

        }
        // draw the items manually for plugin files
        private void ddlFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            KeyValuePair<string, string> kv = (KeyValuePair<string, string>)ddlFiles.Items[e.Index];

            SolidBrush b = null;
            bool useDefaultColor = true;
            if (this.CurrentPlugins.ContainsKey(kv.Value))
            {
                if (this.CurrentPlugins[kv.Value].Enabled == false)
                {
                    useDefaultColor = false;
                    b = (SolidBrush)new SolidBrush(this.m_ColorItemForeColorDisabled);
                }
            }
            if (useDefaultColor == true)
            {
                b = (SolidBrush)new SolidBrush(SystemColors.ControlText);
            }
            e.Graphics.DrawString(kv.Key, e.Font, b, e.Bounds);
            e.DrawFocusRectangle();

        }

#endregion

        #region Types
        /// <summary>
        /// Sets the data source for the Type drop down list (ddlTypes)
        /// </summary>
        private void SetDdlTypeData()
        {
            // Populate the Type with the available type for the current Profile
            ddlType.DataSource = null;
            ddlType.Items.Clear();
            ddlType.DisplayMember = "Value";
            string sFile = string.Empty;
            if (ddlFiles.SelectedIndex >= 0)
            {
                KeyValuePair<string, string> kv = (KeyValuePair<string, string>)ddlFiles.SelectedItem;
                sFile = kv.Value;
            }
            List<KeyValuePair<string, string>> Types = this.GetTypeList(sFile);
            // if there is only one the we will not add ALL
            // if there are none we will add All rather then leaving the list empty
            if (Types.Count != 1)
            {
                Types.Sort(Library.AHKSnipit.AHKSnipitLib.ListHelper.Compare.KeyValuePairCompareStringValue);
                KeyValuePair<string, string> all = new KeyValuePair<string, string>(AppCommon.All, Properties.Resources.All);
                Types.Insert(0, all);
            }

            ddlType.DataSource = Types;
        }
        /// <summary>
        /// Gets a List of KeyValus that represents the Type for the currently selected File
        /// </summary>
        /// <returns>
        /// List of KeyValuePair
        /// </returns>
        private List<KeyValuePair<string, string>> GetTypeList(string DataFileName)
        {
            List<KeyValuePair<string, string>> types = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrEmpty(DataFileName))
            {
                return types;
            }

            bool All = false;
            bool HasHotKeys = false;
            bool HasHotstrings = false;

            if (string.Equals(DataFileName, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                All = true;
            }
            if (All == true)
            {
                if (this.CurrentPlugins.HasHotkey == true)
                {
                    HasHotKeys = true;
                }
                if (this.CurrentPlugins.HasHotString == true)
                {
                    HasHotstrings = true;
                }
            }
            else
            {
                if (this.CurrentPlugins.ContainsKey(DataFileName))
                {
                    var itm = this.CurrentPlugins[DataFileName];

                    if (itm.HasHotkey == true)
                    {
                        HasHotKeys = true;
                    }
                    if (itm.HasHotString == true)
                    {
                        HasHotstrings = true;
                    }
                }

            }
            if (HasHotKeys == true)
            {
                KeyValuePair<string, string> kvCmd = new KeyValuePair<string, string>("HotKey", Properties.Resources.HotKey);
                types.Add(kvCmd);
            }
            if (HasHotstrings == true)
            {
                KeyValuePair<string, string> kvhs = new KeyValuePair<string, string>("HotString", Properties.Resources.HotString);
                types.Add(kvhs);
            }

            return types;

        }
#endregion

        #region Category

        /// <summary>
        /// Populates the Category Drop-down list
        /// </summary>
        private void PopulateCategoryList()
        {
            ddlCategory.DataSource = null;
            ddlCategory.Items.Clear();
            ddlCategory.DisplayMember = "Key";
            List<KeyValuePair<string, string>> lst;
            string sFile = string.Empty;
            if (ddlFiles.SelectedIndex >= 0)
            {
                KeyValuePair<string, string> kv = (KeyValuePair<string, string>)ddlFiles.SelectedItem;
                sFile = kv.Value;
            }
            HotTypeEnum HotType = HotTypeEnum.None;
            KeyValuePair<string, string> kvType;
            if (ddlType.SelectedIndex >= -0)
            {
                kvType = (KeyValuePair<string, string>)ddlType.SelectedItem;

            } else
            {
                kvType = new KeyValuePair<string, string>("null", "null");
            }
            switch (kvType.Key)
            {
                case "HotKey":
                    HotType = HotTypeEnum.Hotkey;
                    break;
                case "HotString":
                    HotType = HotTypeEnum.HotString;
                    break;
                case "All":
                    HotType = HotTypeEnum.HotString | HotTypeEnum.Hotkey;
                    break;
                default:
                    HotType = HotTypeEnum.None;
                    break;
            }
            lst = this.GetCategories(sFile, HotType);
            lst.Sort(Library.AHKSnipit.AHKSnipitLib.ListHelper.Compare.KeyValuePairCompareStringKey);
            KeyValuePair<string, string> all = new KeyValuePair<string, string>(Properties.Resources.All, AppCommon.All);
            lst.Insert(0, all);
            ddlCategory.DataSource = lst;

        }

        /// <summary>
        /// Gets a list of Unique Categories as KeyValuePairs with Key as Uppercase category and value as normal case category
        /// </summary>
        /// <param name="DataFileName">The Name of the File to get the Categories for. or ALL for all files</param>
        /// <param name="HotType">The HotTypes to get the Categories for. Can be one or more flags set</param>
        /// <returns>
        /// A List of unique KeyValuePair with Key as Uppercase category and value as normal case category
        /// </returns>
        private List<KeyValuePair<string, string>> GetCategories(string DataFileName, HotTypeEnum HotType)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();

            if (HotType == HotTypeEnum.None)
            {
                return lst;
            }
            lst = this.CurrentPlugins.GetCategories(DataFileName, true, HotType).ToList();
            lst.Sort(Library.AHKSnipit.AHKSnipitLib.ListHelper.Compare.KeyValuePairCompareStringKey);
            return lst;

        }
#endregion

        #region Plugins
        /// <summary>
        /// Saves a plugin for the current profile. For saving new or edited profiles
        /// </summary>
        /// <param name="p">The Profile to save</param>
        /// <remarks>
        /// If saving an existing profile then the profile resources files will be deleted before saving.
        /// </remarks>
        private void SavePlugin(plugin p)
        {
            if (p.ParentProfile == null)
            {
                if (this.CurrentProfile == null)
                {
                    return;
                }
                p.ParentProfile = this.CurrentProfile;
            }
            ReadWrite.SavePlugin(p, true);

            // call init to reset fields
            this.Init();
        }

        /// <summary>
        /// Selects the Plugin file in the plugin file list
        /// </summary>
        /// <param name="p">The Plugin to select</param>
        private void SelectPlugin(plugin p)
        {
            KeyValuePair<string, string> kv;
            kv = new KeyValuePair<string, string>(p.name, p.File);

            ddlFiles.SelectedItem = kv;
        }

        /// <summary>
        /// Gets the File Name from the currently selected Plugin File
        /// </summary>
        /// <returns>
        /// String of the file or Empty string if no value is selected
        /// </returns>
        private string GetSelectedPluginFile()
        {
            if (ddlFiles.SelectedIndex > -1)
            {
                KeyValuePair<string, string> kv = new KeyValuePair<string, string>();
                kv = (KeyValuePair<string, string>)ddlFiles.SelectedItem;
                return kv.Value;
            }
            return string.Empty;
        }



        /// <summary>
        /// Populates the Drop Down Files list (ddlFiles) with the data XML files for the current selected profile. Key is Name and Value is file.
        /// </summary>
        private void PopulateDataFilesList()
        {
            this.ddlFiles.DataSource = null;
            Application.DoEvents();
            this.ddlFiles.Items.Clear();
            this.ddlFiles.DisplayMember = "Key";
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();// this.GetProfileDataFiles();
            foreach (var item in this.CurrentPlugins)
            {
                lst.Add(new KeyValuePair<string, string>(item.Name, item.PluginFile));
            }

            lst.Sort(Library.AHKSnipit.AHKSnipitLib.ListHelper.Compare.KeyValuePairCompareStringKey);
            KeyValuePair<string, string> all = new KeyValuePair<string, string>(Properties.Resources.All, AppCommon.All);
            lst.Insert(0, all);
            this.ddlFiles.DataSource = lst;
        }

        /// <summary>
        /// Gets a List of all the XML data files for the current profile
        /// </summary>
        /// <returns>
        /// List of KevValuePair with Key being the display name and value being the full file path
        /// </returns>
        [Obsolete]
        private List<KeyValuePair<string, string>> GetProfileDataFiles()
        {
            List<KeyValuePair<string, string>> fileList = new List<KeyValuePair<string, string>>();
            if (this.CurrentProfile == null)
            {
                return fileList;
            }
            string sPath = this.CurrentProfile.codeLanguage.paths.mainData;
            if (string.IsNullOrWhiteSpace(sPath))
            {
                return fileList;
            }
            if (!Directory.Exists(sPath))
            {
                sPath = Path.Combine(AppCommon.Instance.PathXml, sPath);
            }
            if (!Directory.Exists(sPath))
            {
                return fileList;
            }
            var files = Directory.GetFiles(sPath, "*.xml");
            foreach (string file in files)
            {
                KeyValuePair<string, string> kv = new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(file), file);
                fileList.Add(kv);
            }
            return fileList;
        }



#endregion

        #region Profiles
        /// <summary>
        /// Gets if the current profile being display/Edited is the same as the current profile in the settings.ini file
        /// </summary>
        /// <returns>True if the profile is the same; Otherwise false</returns>
        private bool CurrentProfileIsSelected()
        {
            bool retval = false;
            string cProfile = this.GetCurrentProfile();
            if (string.IsNullOrEmpty(cProfile))
            {
                return retval;
            }
            try
            {
                string pName = Path.GetFileName(this.CurrentPlugins.Profile.File);
                retval = string.Equals(cProfile, pName, StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception)
            {
                Debug.WriteLine("Unable to compare current profile and ini profile value");
            }
            return retval;

        }

        /// <summary>
        /// Reads INI file and get the current profile value
        /// </summary>
        /// <returns>String Representing the current profile</returns>
        private string GetCurrentProfile()
        {
            return IniHelper.GetCurrentProfile();

        }

        /// <summary>
        /// Reads INI file and get the current profile value
        /// </summary>
        /// <returns>String Representing the current profile</returns>
        private System.Version GetScriptVersion()
        {
            return IniHelper.GetScriptVersion();
        }

        /// <summary>
        /// Gets a list of Profiles from the Application Profiles folder Key is Name and Value is file
        /// </summary>
        /// <returns>List of CodeLanguage and XML File name</returns>
        private List<KeyValuePair<string, string>> GetProfiles()
        {
            var profiiles = new List<KeyValuePair<string, string>>();
            var files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");

            foreach (string file in files)
            {
                var p = GetProfile(file);

                profiiles.Add(new KeyValuePair<string, string>(p.codeLanguage.codeName, file));
            }
            return profiiles;
        }

        /// <summary>
        /// Gets a Profile from the file Passed In
        /// </summary>
        /// <param name="ProfileFile">The XML Profile to create the profile for</param>
        /// <returns>Profile representing the <paramref name="ProfileFile"/> if it exist; Otherwise Empty Profile is returned</returns>
        private profile GetProfile(string ProfileFile)
        {
            if (!File.Exists(ProfileFile))
            {
                return new profile();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(profile));

            StreamReader reader = new StreamReader(ProfileFile);
            var p = (profile)serializer.Deserialize(reader);
            reader.Close();
            p.File = ProfileFile;
            return p;
        }

        /// <summary>
        /// Gets the Profiles and loads them into ddlProfiles 
        /// </summary>
        private void PopulateProfiles()
        {
            ddlProfiles.Items.Clear();
            ddlProfiles.DisplayMember = "Key";
            try
            {
                this.Text = Properties.Resources.FormHotListTitle;
                var profiles = this.GetProfiles();
                string current = this.GetCurrentProfile();
                current.Trim();
                current = current.ToLower();
                byte match = 0;
                if (!string.IsNullOrWhiteSpace(current))
                {

                    if (current.IndexOf(Path.DirectorySeparatorChar) > -1)
                    {
                        match = 2;
                    }
                    else
                    {
                        match = 1;
                    }

                }

                foreach (var itm in profiles)
                {
                    int index = this.ddlProfiles.Items.Add(itm);
                    if (match > 0)
                    {
                        string strMatch = string.Empty;
                        if (match == 1)
                        {
                            // matching on filename only
                            strMatch = Path.GetFileName(itm.Value.ToLower());

                            if (strMatch == current)
                            {
                                ddlProfiles.SelectedIndex = index;
                                match = 0;
                                this.Text = string.Format("{0} - {1}", Properties.Resources.FormHotListTitle, itm.Key);
                            }
                        }
                        else
                        {
                            // match full path
                            strMatch = itm.Value.ToLower();
                            if (strMatch == current)
                            {
                                ddlProfiles.SelectedIndex = index;
                                match = 0;
                                this.Text = string.Format("{0} - {1}", Properties.Resources.FormHotListTitle, itm.Key);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }
        /// <summary>
        /// Gets the currently selected profile and loads it into the CurrentProfile property
        /// </summary>
        private void SetCurrentProfile()
        {
            if (ddlProfiles.SelectedIndex >= 0)
            {
                KeyValuePair<string, string> kv;
                kv = (KeyValuePair<string, string>)ddlProfiles.SelectedItem;
                try
                {
                    //this.CurrentPlugins = new Plugins(kv.Value);
                    this.CurrentProfile = this.GetProfile(kv.Value);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

            } else
            {
                this.CurrentProfile = null;
            }
        }
        /// <summary>
        /// Set the current Plugins from the value of the current profile
        /// </summary>
        private void SetCurrentPlugins()
        {
            this.CurrentPlugins = new Plugins(this.CurrentProfile.File);
        }


#endregion

        #region Form Events
        private void SnippitList_Load(object sender, EventArgs e)
        {
            this.mnuPluginNew.Image = Z.IconLibrary.Silk.Icon.TableAdd.GetImage();
            this.tsPluginNew.Image = this.mnuPluginNew.Image;
            this.mnuPluginEdit.Image = Z.IconLibrary.Silk.Icon.TabEdit.GetImage();
            this.tsPluginEdit.Image = this.mnuPluginEdit.Image;
            this.mnuLvItemEdit.Image = this.mnuPluginEdit.Image;
            this.mnuProfileNew.Image = Z.IconLibrary.Silk.Icon.PageAdd.GetImage();
            this.tsProfileNew.Image = this.mnuProfileNew.Image;
            this.mnuProfileEdit.Image = Z.IconLibrary.Silk.Icon.PageEdit.GetImage();
            this.tsProfileEdit.Image = this.mnuProfileEdit.Image;
            this.mnuValidateInstall.Image = Z.IconLibrary.Silk.Icon.ReportKey.GetImage();
            this.mnuValidatePlugin.Image = Z.IconLibrary.Silk.Icon.ReportKey.GetImage();
            this.mnuFileExportSelectePlugin.Image = Z.IconLibrary.Silk.Icon.Plugin.GetImage();
            this.mnuFileExportPluginsProfile.Image = Z.IconLibrary.Silk.Icon.ApplicationGo.GetImage();
            this.mnuHelpAhkSnipitHelp.Image = Z.IconLibrary.Silk.Icon.Help.GetImage();
            this.mnuHelpProjectUrl.Image = Z.IconLibrary.Silk.Icon.WorldLink.GetImage();
            this.IsInitialized = true;
            this.SetDdlTypeData();
            this.PopulateListView();

            if (Properties.Settings.Default.SiWindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.SiLocation;
                Size = Properties.Settings.Default.SiSize;
            }
            else if (Properties.Settings.Default.SiWindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.SiLocation;
                Size = Properties.Settings.Default.SiSize;
            }
            else
            {
                Location = Properties.Settings.Default.SiLocation;
                Size = Properties.Settings.Default.SiSize;
            }
            colCategory.Width = Properties.Settings.Default.SiColCategoryWidth;
            colDesc.Width = Properties.Settings.Default.SiColDescWidth;
            colKey.Width = Properties.Settings.Default.SiColKeyWidth;
            colMod.Width = Properties.Settings.Default.SiColModWidth;
            colName.Width = Properties.Settings.Default.SiColNameWidth;
        }

        private void SnippitList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.SiLocation = RestoreBounds.Location;
                Properties.Settings.Default.SiWindowState = WindowState;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.SiLocation = Location;
                Properties.Settings.Default.SiSize = Size;
                Properties.Settings.Default.SiWindowState = WindowState;
            }
            else
            {
                Properties.Settings.Default.SiLocation = RestoreBounds.Location;
                Properties.Settings.Default.SiSize = RestoreBounds.Size;
                Properties.Settings.Default.SiWindowState = WindowState;
            }
            Properties.Settings.Default.SiColCategoryWidth = colCategory.Width;
            Properties.Settings.Default.SiColDescWidth = colDesc.Width;
            Properties.Settings.Default.SiColKeyWidth = colKey.Width;
            Properties.Settings.Default.SiColModWidth = colMod.Width;
            Properties.Settings.Default.SiColNameWidth = colName.Width;
            Properties.Settings.Default.Save();

        }
        #endregion

        #region AutoHotKey Script
#if RELOAD
        /// <summary>
        /// Restarts the main Script if is running or Starts new if not running
        /// </summary>
        /// <remarks>
        /// Reads script location from app.ini
        /// </remarks>
        private void StartScript()
        {
            string scriptPath = IniHelper.GetLauncherScriptPath();

            if (string.IsNullOrEmpty(scriptPath))
            {
                return;
            }
            if (!File.Exists(scriptPath))
            {
                return;
            }
            Process.Start(scriptPath);

        }
#endif
        /// <summary>
        /// Gets if the script is currently running
        /// </summary>
        /// <returns>True if Script is running; Otherwise false</returns>
        /// <remarks>Gets Script running state from app.ini</remarks>
        private bool ScriptIsRunning()
        {
            return IniHelper.GetScriptIsRunning();
        }

#endregion

        #region Export
        /// <summary>
        /// Exports the profile with one ore more plugins
        /// </summary>
        /// <remarks>
        /// If plugin has Select all then all plugins will be exported for the selected profile.
        /// If plugin has a specific plugin selected then only that plugin will be exported.
        /// </remarks>
        private void ExportSelected(bool PluginOnly)
        {
            if (this.CurrentProfile == null)
            {
                return;
            }
            if (PluginOnly == true)
            {
                ExportSelectedPlugin();
                return;
            }
            try
            {
                SnippitInstal instal = new SnippitInstal();
                instal.schemaVersion = AppCommon.Instance.MAxAllowSchemaVersion;

                var cProfile = profile.FromFile(CurrentProfile.File);
                instal.profile = cProfile;

                if (PluginOnly == true && this.CurrentPlugins.IsCurrentKeyValid == true)
                {
                    var cPlugin = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
                    instal.plugin = new plugin[] { cPlugin };
                }
                else if(PluginOnly == false)
                {
                    List<plugin> cPlugins = new List<plugin>();
                    foreach (var item in this.CurrentPlugins)
                    {
                        cPlugins.Add(plugin.FromFile(item.PluginFile));
                    }

                    instal.plugin = cPlugins.ToArray();
                }
                else
                {
                    // Nothing selected to export
                    return;
                }

                XmlSerializer writer = new XmlSerializer(typeof(SnippitInstal));
                SaveFileDialog sd = new SaveFileDialog();
                sd.AddExtension = true;
                sd.OverwritePrompt = true;
                sd.RestoreDirectory = true;
                //LastDataTextImportLocation
                if ((string.IsNullOrEmpty(Properties.Settings.Default.SiLastExportLocationInstall) == false)
                    && (Directory.Exists(Properties.Settings.Default.SiLastExportLocationInstall) == true))
                {
                    sd.InitialDirectory = Properties.Settings.Default.SiLastExportLocationInstall;
                }
                else
                {
                    sd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                sd.DefaultExt = "xml";
                sd.Filter = "XML|*.xml|All Files|*.*";
                if (PluginOnly == true && this.CurrentPlugins.IsCurrentKeyValid == true)
                {
                    sd.FileName = this.CurrentPlugins.CurrentPlugin.Name + ".xml";
                }
                else
                {
                    sd.FileName = instal.profile.name + ".xml";
                }

                if (sd.ShowDialog(this) == DialogResult.OK)
                {
                    Properties.Settings.Default.SiLastExportLocationInstall = Path.GetDirectoryName(sd.FileName);
                    // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SnippitInstal.xml";
                    FileStream file = File.Create(sd.FileName);

                    writer.Serialize(file, instal);
                    file.Close();
                    MessageBox.Show(Properties.Resources.FileExportedMessage, Properties.Resources.FileExportTitle);
                }

            }
            catch (Exception e)
            {
                this.DisplayGeneralError(e.Message);
            }
        }

        private void ExportSelectedPlugin()
        {
            if (this.CurrentProfile == null)
            {
                return;
            }
            try
            {
               
               
                if (this.CurrentPlugins.IsCurrentKeyValid == false)
                {
                    return;
                }
               
                var cPlugin = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);

                XmlSerializer writer = new XmlSerializer(typeof(plugin));
                SaveFileDialog sd = new SaveFileDialog();
                sd.AddExtension = true;
                sd.OverwritePrompt = true;
                sd.RestoreDirectory = true;
                //LastDataTextImportLocation
                if ((string.IsNullOrEmpty(Properties.Settings.Default.SiLastExportLocationPlugin) == false)
                    && (Directory.Exists(Properties.Settings.Default.SiLastExportLocationPlugin) == true))
                {
                    sd.InitialDirectory = Properties.Settings.Default.SiLastExportLocationPlugin;
                }
                else
                {
                    sd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
               
                sd.DefaultExt = "xml";
                sd.Filter = "XML|*.xml|All Files|*.*";
                sd.FileName = this.CurrentPlugins.CurrentPlugin.Name + ".xml";

                if (sd.ShowDialog(this) == DialogResult.OK)
                {
                    Properties.Settings.Default.SiLastExportLocationPlugin = Path.GetDirectoryName(sd.FileName);
                    // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SnippitInstal.xml";
                    FileStream file = File.Create(sd.FileName);

                    writer.Serialize(file, cPlugin);
                    file.Close();
                    MessageBox.Show(Properties.Resources.FileExportedMessage, Properties.Resources.FileExportTitle);
                }

            }
            catch (Exception e)
            {
                this.DisplayGeneralError(e.Message);
            }
        }
#endregion

        #region Import
        /// <summary>
        /// Imports a Install file or a Plugin file and saves to disk
        /// </summary>
        private void Import()
        {
            OpenFileDialog fOpen = new OpenFileDialog();
            fOpen.AddExtension = true;
            fOpen.RestoreDirectory = true;
            //LastDataTextImportLocation
            if ((string.IsNullOrEmpty(Properties.Settings.Default.SiLastImportLocationInstall) == false)
                && (Directory.Exists(Properties.Settings.Default.SiLastImportLocationInstall) == true))
            {
                fOpen.InitialDirectory = Properties.Settings.Default.SiLastImportLocationInstall;
            }
            else
            {
                fOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            fOpen.DefaultExt = "xml";
            fOpen.Filter = "XML|*.xml|All Files|*.*";
            if (fOpen.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.SiLastImportLocationInstall = Path.GetDirectoryName(fOpen.FileName);
                var vr = ValidateXml.ValidateSnipitInstalFile(fOpen.FileName);

                if (vr.HasErrors == false)
                {
                    // snippit install import
                    SnippitInstal si = SnippitInstal.FromFile(fOpen.FileName);
                    profile newP = si.profile;
                    profile matched = this.GetProfile(newP);
                    if (matched == null)
                    {
                        // no existing matching profile
                        string sFile = Path.Combine(AppCommon.Instance.PathProfiles, newP.name + ".xml");
                        newP.File = sFile;
                        if (File.Exists(newP.File))
                        {
                            // A profile file already exist with a different profile name
                            // since there is not a match we will create a new file name
                            // for the profile being installed
                            string newFileName = newP.File;
                            int i = 0;
                            while (File.Exists(newFileName))
                            {
                                i++;
                                newFileName = string.Format("{0}{1}{2}_{3}{4}"
                                    , Path.GetDirectoryName(newP.File)
                                    , Path.DirectorySeparatorChar
                                    , Path.GetFileNameWithoutExtension(newP.File)
                                    , i.ToString()
                                    , Path.GetExtension(newP.File));
                            }
                            newP.File = newFileName;

                        }
                        this.SaveProfile(newP);
                    }
                    else
                    {

                        // there is a matching profile
                        if (matched.FullVersion < newP.FullVersion)
                        {
                            // the imported profile is newer then the exist profile so will update the current profile
                            this.SaveProfile(newP);
                        }
                    }
                    ReadWrite.SavePlugin(si, true);
                    MessageBox.Show(this, Properties.Resources.ImportCompleteText, Properties.Resources.ImportCompleteTitle
                                , MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // call init to reset fields
                    this.Init();
                }
                else
                {
                    // not a valid install XML so lets check for a valid plugin to add to an existing profile
                    var vPlugin = ValidateXml.ValidatePluginFile(fOpen.FileName);
                    if (vPlugin.HasErrors == false)
                    {
                        var plg = plugin.FromFile(fOpen.FileName);
                        // this is a valid plugin, Check for a profile and add the plugin if we have a profile
                        var p = this.GetProfileFromName(plg.profileName);
                        Editor.Dialog.frmProflieSelection frmpSel = new Editor.Dialog.frmProflieSelection();
                        if (p != null)
                        {
                           frmpSel.SelectedProfileFile = p.File;
                        }
                        if (frmpSel.ShowDialog(this) == DialogResult.OK)
                        {
                            var ImportToProfie = profile.FromFile(frmpSel.ProfileResult);

                            SnippitInstal si = new SnippitInstal();
                            si.profile = ImportToProfie;
                            plg.ParentProfile = ImportToProfie;
                            plg.profileName = ImportToProfie.name;
                            plg.File = string.Empty; // clear the file name to to save into selected profile
                            si.plugin = new plugin[] { plg };
                            ReadWrite.SavePlugin(si, true);
                            MessageBox.Show(this, Properties.Resources.ImportCompleteText, Properties.Resources.ImportCompleteTitle
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // call init to reset fields
                            this.Init();
                        }
                      
                    }
                    else
                    {
                        MessageBox.Show(this, Properties.Resources.ImportInvalidFileText, Properties.Resources.ImportInvalidFileTitle
                                 , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }



        /// <summary>
        /// Saves a profile to disk
        /// </summary>
        /// <param name="p">The profile to save</param>
        private void SaveProfile(profile p)
        {
            try
            {
                ReadWrite.SaveProfile(p);
            }
            catch (ProfileVersionException)
            {
                MessageBox.Show(this, Properties.Resources.ImportNoMatchingProfileText, Properties.Resources.ImportNoMatchingProfileTitle
                                , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception e)
            {
                this.DisplayGeneralError(e.Message);
                return;
            }


        }


        /// <summary>
        /// Get a string that represents the fully qualified profile data path ( plugin path )
        /// </summary>
        /// <param name="p">The Profile to get the Data Path from</param>
        /// <returns>
        /// String representing the fully qualified data path.
        /// </returns>
        private string GetProfileFullDataPath(profile p)
        {
            string sPath = Path.Combine(AppCommon.Instance.PathXml, p.name);
            if (!string.IsNullOrEmpty(p.codeLanguage.paths.mainData))
            {
                if (p.codeLanguage.paths.mainData.IndexOf(Path.DirectorySeparatorChar) > -1)
                {
                    if (Directory.Exists(p.codeLanguage.paths.mainData))
                    {
                        sPath = p.codeLanguage.paths.mainData;
                    }
                }
            }
            return sPath;
        }

        /// <summary>
        /// Gets Profile instance from name
        /// </summary>
        /// <param name="ProfileName">The internal name same as <see cref="profile.name"/>, to search for</param>
        /// <returns>
        /// An instance of <see cref="profile"/> if there is a installed profile with matching <see cref="profile.name"/>;
        /// Otherwise null
        /// </returns>
        private profile GetProfileFromName(string ProfileName)
        {
            var files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");
            profile p = default(profile);

            foreach (var file in files)
            {
                try
                {
                    profile current = profile.FromFile(file);
                    if (string.Equals(current.name, ProfileName, StringComparison.CurrentCulture))
                    {
                        p = current;
                        break;
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }
            return p;
        }
        /// <summary>
        /// Search through the installed profiles and looks for a match based upon <see cref="profile.name"/>
        /// </summary>
        /// <param name="match">The Profile to match</param>
        /// <returns>
        /// The matching Profile if it exit; Otherwise Null
        /// </returns>
        private profile GetProfile(profile match)
        {
            var files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");
            profile p = default(profile);

            foreach (var file in files)
            {
                try
                {
                    profile current = profile.FromFile(file);
                    if (string.Equals(current.name, match.name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        p = current;
                        break;
                    }

                }
                catch (Exception)
                {

                    continue;
                }


            }
            return p;
        }
#endregion

        #region Delete
        /// <summary>
        /// Deletes Selected plugin from plugins. If not Select All is chosen then the profile with all its plugins will be deleted.
        /// This delete is permanent
        /// </summary>
        private void Delete()
        {
            if (this.CurrentProfile == null)
            {
                return;
            }

            // get the current profile from the ini to compare
            // if it is the deleted profile then restart the script if it is running
            string iniCurrentScript = IniHelper.GetCurrentProfile();
            string deletedProflie = Path.GetFileName(CurrentProfile.File);
            try
            {


                SnippitInstal instal = new SnippitInstal();

                var cProfile = profile.FromFile(CurrentProfile.File);
                instal.profile = cProfile;

                if (this.CurrentPlugins.IsCurrentKeyValid == true)
                {
                    var cPlugin = plugin.FromFile(this.CurrentPlugins.CurrentPlugin.PluginFile);
                    instal.plugin = new plugin[] { cPlugin };
                }
                else
                {
                    List<plugin> cPlugins = new List<plugin>();

                    foreach (var item in this.CurrentPlugins)
                    {
                        try
                        {
                            var r = plugin.FromFile(item.PluginFile);
                            cPlugins.Add(r);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    instal.plugin = cPlugins.ToArray();
                }
                ReadWrite.DeleteInstal(instal, true);

                MessageBox.Show(this
                    , Properties.Resources.DeleteMsgSuccessText
                    , Properties.Resources.DeleteMsgSuccessTitle
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(this
                    , string.Format(Properties.Resources.DeleteMsgFailReasonText, Environment.NewLine, e.Message)
                    , Properties.Resources.DeleteMsgFailTitle
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);

            }
            // call init to reset fields
            this.Init(); // init will also ensure there is a default profile even if the last profile is deleted here
#if RELOAD
            if (string.Equals(iniCurrentScript, deletedProflie, StringComparison.CurrentCultureIgnoreCase))
            {
                // if the script we just deleted is the same as the running script then
                // we should restart the script
                this.StartScript();
            }
#endif
        }


        /// <summary>
        /// Gets if a a path is an empty directory
        /// </summary>
        /// <param name="path">The Path to search</param>
        /// <returns>
        /// True if the path contains files; Otherwise false.
        /// </returns>
        private bool DirectoryIsEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
#endregion

        #region Error Handling
        /// <summary>
        /// Displays a General Error in a message box
        /// </summary>
        /// <param name="Msg">The Message to display</param>
        private void DisplayGeneralError(string Msg)
        {
            MessageBox.Show(this
                   , Msg
                   , Properties.Resources.ErrorTitleError
                   , MessageBoxButtons.OK
                   , MessageBoxIcon.Error);
            return;
        }


        #endregion

        #region Menu Click Event Handlers
        private void mnuHelpProjectUrl_Click(object sender, EventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(AppCommon.Instance.UrlProjectHome);
        }
        #endregion
    }
}
