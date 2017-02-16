#region Using
using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.Dialog;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.AHKSnipit.HotList.Executor;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData;
using BigByteTechnologies.Windows.AHKSnipit.HotList.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Library.Windows.CommandManagement;
using BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;
#endregion

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor
{
    public partial class frmPlugin : Form
    {
        #region Instance
        private static frmPlugin _instance;

        public static frmPlugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new frmPlugin();
                }
                return _instance;
            }
        }

        public static bool IsInstance
        {
            get
            {
                return frmPlugin._instance != null;
            }
        }

        #endregion

        #region Constructor
        public frmPlugin()
        {
            InitializeComponent();
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode == true)
            {
                return;
            }
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;

            InitializeCommandManager();
            this.IsNew = true;

            ErrorCounterIncHs = new ErrorCounter();
            ErrorCounterIncHk = new ErrorCounter();
           

            var HsTypeMapEnumRule = new EnumRule<hotstringType>();
            HsTypeMapEnumRule.Add(hotstringType.UnKnown);

            var HsTypeMapValue = HotstringTypeMap.Instance.ToBindingList(HsTypeMapEnumRule);
            cboHsType.DisplayMember = "Value";
            cboHsType.ValueMember = "Key";
            cboHsType.DataSource = HsTypeMapValue;
       


            plg = new plugin();

            CommandBindingList = new BindingList<command>();
            CommandBindingList.AddingNew += (s, e) =>
            {
                e.NewObject = new command { Parent = CommandBindingList };
            };
  
            HotstringBindingList = new BindingList<hotstring>();
            HotstringBindingList.AddingNew += (s, e) =>
            {
                e.NewObject = new hotstring { Parent = HotstringBindingList };
            };

            IncludeBindingList = new BindingList<include>();
            IncludeBindingList.AddingNew += (s, e) =>
            {
                e.NewObject = new include { Parent = IncludeBindingList };
            };
           

            IncHsBindingList = new BindingList<includeHotstring>();
            //IncHsBindingList.AddingNew += (s, e) =>
            //{
            //    e.NewObject = new includeHotstring { Parent = IncHsBindingList };
            //};


            IncHkBindingList = new BindingList<includeCommand>();
            //IncHkBindingList.AddingNew += (s, e) =>
            //{
            //    e.NewObject = new includeCommand { Parent = IncHkBindingList };
            //};

            DataItemBindingList = new BindingList<dataItem>();
            DataItemBindingList.AddingNew += (s, e) =>
            {
                e.NewObject = new dataItem { Parent = DataItemBindingList };
            };

            HsReplacements = new BindingList<IReplacement>();
            bnHotkeys.Visible = false;
            bnHotstrings.Visible = false;
            bnIncludes.Visible = false;
            mnuHsReplacement.Visible = false;
            tsHsReplacements.Visible = false;
            DgReplacementsAddImgColumns();
            DgIncludeHotstringsAddImgColumns();
            DgIncludeHotKeyAddImgColumns();

            // bind the multiline textboxes seperate so we can contol formating for \n and \r\n
            BindMultiLineTextboxes();

            // wire up one control from each section to have a way to know when the controls have been created for the first time.
            txtHsName.HandleCreated += HotStringControl_HandleCreated;
            txtHkName.HandleCreated += HoteyControl_HandleCreated;
            txtIncName.HandleCreated += IncludeControl_HandleCreated;
            dgIncHk.HandleCreated += IncludeControlHk_HandleCreated;
            dgIncHs.HandleCreated += IncludeControlHs_HandleCreated;
            dgHsReplacements.HandleCreated += HotStringControlReplacement_HandleCreated;
            txtDataFileName.HandleCreated += DataControl_HandleCreated;

            UcHk.ItemChanged += this.UcHk_ItemChanged;

            
        }
        #endregion

        #region Format Line Breaks

        /// <summary>
        /// Binds all multi line textboxes to format line breaks
        /// </summary>
        private void BindMultiLineTextboxes()
        {
            Binding bindtxtHsCode = new Binding("Text", hotstringsBindingSource, "code");
            bindtxtHsCode.Format += NewLineFormat_binding;
            txtHsCode.DataBindings.Clear();
            txtHsCode.DataBindings.Add(bindtxtHsCode);

            Binding bindtxtHsDescription = new Binding("Text", hotstringsBindingSource, "description");
            bindtxtHsDescription.Format += NewLineFormat_binding;
            txtHsDescription.DataBindings.Clear();
            txtHsDescription.DataBindings.Add(bindtxtHsDescription);

            Binding bindtxtHkCode = new Binding("Text", commandsBindingSource, "code");
            bindtxtHkCode.Format += NewLineFormat_binding;
            txtHkCode.DataBindings.Clear();
            txtHkCode.DataBindings.Add(bindtxtHkCode);

            Binding bindtxtHkDescription = new Binding("Text", commandsBindingSource, "description");
            bindtxtHkDescription.Format += NewLineFormat_binding;
            txtHkDescription.DataBindings.Clear();
            txtHkDescription.DataBindings.Add(bindtxtHkDescription);

            Binding bindtxtIncCode = new Binding("Text", includeBindingSource, "code");
            bindtxtIncCode.Format += NewLineFormat_binding;
            txtIncCode.DataBindings.Clear();
            txtIncCode.DataBindings.Add(bindtxtIncCode);

            //txtIncDescription
            Binding bindtxtIncDescription = new Binding("Text", includeBindingSource, "description");
            bindtxtIncDescription.Format += NewLineFormat_binding;
            txtIncDescription.DataBindings.Clear();
            txtIncDescription.DataBindings.Add(bindtxtIncDescription);

            Binding bindtxtDataValue = new Binding("Text", dataItemBindingSource, "dataValue");
            bindtxtDataValue.Format += NewLineFormat_binding;
            txtDataValue.DataBindings.Clear();
            txtDataValue.DataBindings.Add(bindtxtDataValue);
        }

        private void NewLineFormat_binding(object sender, ConvertEventArgs cevent)
        {
           // The method convers \n to \r\n for textex boxes to display line breaks

           if (cevent.DesiredType != typeof(string)) return;
           if (cevent.Value == null) return;
            

            // replace /n with /r/n
            // just in case there instance of \r\n will replace with \n first
            // in most cases this will not replace anthing but jsut in case
            string result = ((string)cevent.Value).Replace(Environment.NewLine, "\n");

            // Replace all the line terminators with \r\n
            result = result.Replace("\n", Environment.NewLine);
            cevent.Value = result;
            
        }

        #endregion

        #region Fields
        private plugin plg;
        BindingList<command> CommandBindingList;
        BindingList<hotstring> HotstringBindingList;
        BindingList<IReplacement> HsReplacements;
        BindingList<include> IncludeBindingList;
        BindingList<includeHotstring> IncHsBindingList;
        BindingList<includeCommand> IncHkBindingList;
        BindingList<dataItem> DataItemBindingList;

        private BindingList<hotstringType> HsTypeMap;

        private bool IsInitialized = false;
        private const int IndexTabMain = 0;
        private const int IndexTabHk = 1;
        private const int IndexTabHs = 2;
        private const int IndexTabInclude = 3;
        private const int IndexTabData = 4;

        private const int IndexTabIncludeMain = 0;
        private const int IndexTabIncludeHk = 2;
        private const int IndexTabIncludeHs = 3;

        private const int IndexTabHsRpMain = 0;
        private const int IndexTabHsRpReplace = 1;
        private bool CancelToolStripTextBoxExCommandExecute = false;
        private int IncludeCurrentIndex = -1;
        private int CommandCurrentIndex = -1;
        private int DataCurrentIndex = -1;
        /// <summary>
        /// Flag that is set to true while an include is being deleted
        /// </summary>
        private bool IncludingDeleting = false;

        /// <summary>
        /// The last added Include Command item
        /// </summary>
        private includeCommand IncHkLastAdded = null;

        /// <summary>
        /// The last added Include Command Index
        /// </summary>
        private int IncludHkLastAddedIndex = -1;

        /// <summary>
        /// The last added Include Hotstring
        /// </summary>
        private includeHotstring IncHsLastAdded = null;

        /// <summary>
        /// The last added Include Hotstring Index
        /// </summary>
        private int IncludHsLastAddedIndex = -1;

        /// <summary>
        /// The Row index of the first error that occurred on for Hotstring replacements Grid
        /// </summary>
       // private int GridErrorIndexHsReplacements = -1;
        HotkeyKeys hsk;

        ErrorCounter ErrorCounterIncHs;
        ErrorCounter ErrorCounterIncHk;

        /// <summary>
        /// Will contain which textbox is current Focused. Will be null if no textbox has focus
        /// </summary>
        /// <remarks>
        /// <see cref="tBox_Enter(object, EventArgs)"/> 
        /// <see cref="tBox_Leave(object, EventArgs)"/>
        /// </remarks>
        private TextBox FocusedTextbox = null;
        
        #endregion

        #region Controls Created
        // Databound controls wil not have any data until after they are created. Controls on tabs that have never been clicked
        // on will most likely not be created until the tab as been clicked on at least one time. When a control is created for the 
        // first time it will fire a HandleCreated Event. We will use this handle to grab a contol on the tab we want to verfiy that
        // the control has been created. We will need this for validation purporses of the entire form.
        // See Also: https://msdn.microsoft.com/en-us/library/system.windows.forms.control.handlecreated(v=vs.110).aspx
        // See Also: http://stackoverflow.com/questions/9828153/cannot-data-bind-to-a-control-when-control-visible-false
        //
        // One Data control from each tab should do the trick. Events are wired up in the constructor

        /// <summary>
        /// Gets if the Hotstring Controls have been created. Does not include Replacement Controls
        /// </summary>
        private bool HotStringControlsCreated = false;
        /// <summary>
        /// Gets if the HotString Replacement Controls have been created
        /// </summary>
        private bool HotStringControlsReplacementCreated = false;
        /// <summary>
        /// Gets if the HotKey controls have been created
        /// </summary>
        private bool HotkeyControlsCreated = false;
        /// <summary>
        /// Gets if the Include controls have been created. Does not include Hotstring and Hotkey child controls
        /// </summary>
        private bool IncludeControlsCreated = false;
        /// <summary>
        /// Gets if the Include Hotkey controls have been created
        /// </summary>
        private bool IncludeControlsHkCreated = false;
        /// <summary>
        /// Gets if the Include HotString controls have been created
        /// </summary>
        private bool IncludeControlsHsCreated = false;
        /// <summary>
        /// Gets if the Data controls have been created
        /// </summary>
        private bool DataControlsCreated = false;

        private void HotStringControl_HandleCreated(Object sender, EventArgs e)
        {
            HotStringControlsCreated = true;
        }
        private void HotStringControlReplacement_HandleCreated(Object sender, EventArgs e)
        {
            HotStringControlsReplacementCreated = true;
        }

        private void HoteyControl_HandleCreated(Object sender, EventArgs e)
        {
            HotkeyControlsCreated = true;
        }
        private void IncludeControl_HandleCreated(Object sender, EventArgs e)
        {
            IncludeControlsCreated = true;
        }
        private void DataControl_HandleCreated(Object sender, EventArgs e)
        {
            DataControlsCreated = true;
        }
        private void IncludeControlHk_HandleCreated(Object sender, EventArgs e)
        {
            IncludeControlsHkCreated = true;
        }

        private void IncludeControlHs_HandleCreated(Object sender, EventArgs e)
        {
            IncludeControlsHsCreated = true;
        }
        //dgHsReplacements

        #endregion

        #region Properties

        #region General Properties
        /// <summary>
        /// Gets/Sets the Plugin Being Edited
        /// </summary>
        public plugin Plugin
        {
            get { return plg; }
            set { plg = value; }
        }
        /// <summary>
        /// The Initial Command HotKey. If set the Matching Hotkey Command will be selected on startup
        /// </summary>
        public string InitailCommand { get; set; }

        /// <summary>
        /// The Initial Include Name. If set the Matching Include will be selected on startup
        /// </summary>
        public string InitialInclude { get; set; }

        /// <summary>
        /// The Initial Hotstring Trigger. If Set the Matching Hotstring will be selected on startup
        /// </summary>
        public string InitialHotString { get; set; }

        /// <summary>
        /// Gets/Sets the main window
        /// </summary>
        public SnippitList MainWindow { get; set; }

        /// <summary>
        /// Specifies if this a new profile or an existing profile
        /// </summary>
        public bool IsNew { get; set; }
        #endregion

        #region Command Related Properties

        #region Active Tab
        private bool IsTabMainActive
        {
            get
            {
                return this.tcMain.SelectedIndex == IndexTabMain;
            }
        }
        private bool IsTabHotKeysActive
        {
            get
            {
                return this.tcMain.SelectedIndex == IndexTabHk;
            }
        }
        private bool IsTabHotStringActive
        {
            get
            {
                return this.tcMain.SelectedIndex == IndexTabHs;
            }
        }
        private bool IsTabHotStringMainTabActive
        {
            get
            {
                return (this.IsTabHotStringActive == true && this.tcHs.SelectedIndex == IndexTabHsRpMain);
            }
        }

        private bool IsTabHotStringReplacementsTabActive
        {
            get
            {
                return (this.IsTabHotStringActive == true && this.tcHs.SelectedIndex == IndexTabHsRpReplace);
            }
        }

        private bool IsTabIncludesActive
        {
            get
            {
                return this.tcMain.SelectedIndex == IndexTabInclude;
            }
        }
        private bool IsTabIncludesTabMainActive
        {
            get
            {
                return (IsTabIncludesActive == true && tcIncludes.SelectedIndex == IndexTabIncludeMain);
            }
        }
        private bool IsTabIncludesTabHotKeysActive
        {
            get
            {
                return (IsTabIncludesActive == true && tcIncludes.SelectedIndex == IndexTabIncludeHk);
            }
        }
        private bool IsTabIncludesTabHotStringsActive
        {
            get
            {
                return (IsTabIncludesActive == true && tcIncludes.SelectedIndex == IndexTabIncludeHs);
            }
        }

        private bool IsTabDataActive
        {
            get
            {
                return this.tcMain.SelectedIndex == IndexTabData;
            }
        }
        #endregion

        #region Tab Specific Properties
        #region HotString Tab Main Subtab
        hotstringType? m_CurrentHotStringType = null;
        /// <summary>
        /// Gets the Current Hotstring Type. Returns Null if Hotstring Main Tab is not active
        /// </summary>
        private hotstringType? CurrentHotStringType
        {
            get
            {
                if (m_CurrentHotStringType.HasValue == false)
                {
                    if (this.IsTabHotStringMainTabActive == false)
                    {
                        return null;
                    }
                    if (cboHsType.SelectedItem != null)
                    {
                        SortedMapItem sType = (SortedMapItem)cboHsType.SelectedItem;
                        m_CurrentHotStringType = (hotstringType)Enum.Parse(typeof(hotstringType), sType.Key);
                        
                    }
                    else
                    {
                        return null;
                    }
                }
                return m_CurrentHotStringType;
            }
        }

       /// <summary>
       /// Gets if the HotString Type has Paste Option Selected. False if Hotstring Main tab is not selected
       /// </summary>
        private bool IsTypePasteSelected
        {
            get
            {
                if (CurrentHotStringType.HasValue == false)
                {
                    return false;
                }
                return CurrentHotStringType == hotstringType.Paste;
            }
        }
        /// <summary>
        /// Gets if the HotString Type has Inline Option Selected. False if Hotstring Main tab is not selected
        /// </summary>
        private bool IsTypeInlineSelected
        {
            get
            {
                if (CurrentHotStringType.HasValue == false)
                {
                    return false;
                }
                return CurrentHotStringType == hotstringType.Inline;
            }
        }
        #endregion
        #endregion

        #region IsValid Properties
        TriEnum m_IsClipboardText = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Clipboard contains text
        /// </summary>
        /// <remarks>
        /// This property is reset on each CommandPrePorcess event
        /// </remarks>
        private bool IsClipboardText
        {
            get
            {
                if (m_IsClipboardText == TriEnum.NotSet)
                {
                    if (Clipboard.GetDataObject().GetDataPresent(typeof(string)) == true)
                    {
                        m_IsClipboardText = TriEnum.True;
                    }
                    else
                    {
                        m_IsClipboardText = TriEnum.False;
                    }
                }
                return m_IsClipboardText == TriEnum.True;
            }
        }
        TriEnum m_MainControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Main Controls have any data Errors. True if there are no Data errors; Otherwise false
        /// </summary>
        private bool IsMainControlsValid
        {
            get
            {
                if (m_MainControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidMainControls() == true)
                    {
                        m_MainControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_MainControlsValid = TriEnum.False;
                    }
                }
                return m_MainControlsValid == TriEnum.True;
            }
        }

        TriEnum m_hkControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Hotkey Controls have any data Errors. True if there are no Data errors; Otherwise false
        /// </summary>
        private bool IsHotKeyControlsValid
        {
            get
            {
                if (m_hkControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidHkControls() == true && this.IsHotKeyUserControlsValid == true)
                    {
                        m_hkControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_hkControlsValid = TriEnum.False;
                    }
                }
                return m_hkControlsValid == TriEnum.True;
            }
        }

        TriEnum m_IsDataControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Data Controls have any data Errors. True if there are no Data errors; Otherwise false
        /// </summary>
        private bool IsDataControlsValid
        {
            get
            {
                if (m_IsDataControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidDataControls() == true)
                    {
                        m_IsDataControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_IsDataControlsValid = TriEnum.False;
                    }
                }
                return m_IsDataControlsValid == TriEnum.True;
            }
        }

        TriEnum m_IsHotKeyUserControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Hotkey User Controls have any data Errors. True if there are no Data errors; Otherwise false
        /// </summary>
        private bool IsHotKeyUserControlsValid
        {
            get
            {
                if (m_IsHotKeyUserControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidUcHkControls() == true)
                    {
                        m_IsHotKeyUserControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_IsHotKeyUserControlsValid = TriEnum.False;
                    }
                }
                return m_IsHotKeyUserControlsValid == TriEnum.True;
            }
        }

        TriEnum m_hsControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Hotstring Controls have data errors. True if there are no data errors. Does no include Replacements Grid
        /// </summary>
        private bool IsHotStringControlsValid
        {
            get
            {
                if (m_hsControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidHsControls() == true && (UcHs.IsValid == true || HotstringBindingList.Count == 0))
                    {
                        m_hsControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_hsControlsValid = TriEnum.False;
                    }
                }
                return m_hsControlsValid == TriEnum.True;
            }
        }

        TriEnum m_IsHotstringReplacementGridValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if Hotstring Replacement Grid is valid. True if Valid; Otherwise false
        /// </summary>
        private bool IsHotstringReplacementGridValid
        {
            get
            {
                if (m_IsHotstringReplacementGridValid == TriEnum.NotSet)
                {
                    m_IsHotstringReplacementGridValid = TriEnum.True;
                }
                return m_IsHotstringReplacementGridValid == TriEnum.True;
            }
        }

        TriEnum m_incControlsValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if Include Controls are Valid. True if there are no data errors. Does not include Grids
        /// </summary>
        private bool IsIncludeControlsValid
        {
            get
            {
                if (m_incControlsValid == TriEnum.NotSet)
                {
                    if (this.IsValidIncControls() == true)
                    {
                        m_incControlsValid = TriEnum.True;
                    }
                    else
                    {
                        m_incControlsValid = TriEnum.False;
                    }
                }
                return m_incControlsValid == TriEnum.True;
            }
        }

        TriEnum m_IsIncludeHkGridValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Include Hotkey grid is Valid. True if Valid; Otherwise false
        /// </summary>
        private bool IsIncludeHkGridValid
        {
            get
            {
                if (m_IsIncludeHkGridValid == TriEnum.NotSet)
                {
                    //Debug.WriteLine(string.Format("ErrorCounterIncHk Error count:{0}", ErrorCounterIncHk.ErrorCount));
                    if (this.ErrorCounterIncHk.ErrorCount > 0)
                    {
                        m_IsIncludeHkGridValid = TriEnum.False;
                    }
                    else
                    {
                        m_IsIncludeHkGridValid = TriEnum.True;
                    }

             
                }
                return m_IsIncludeHkGridValid == TriEnum.True;
            }
        }

        TriEnum m_IsIncludeHsGridValid = TriEnum.NotSet;
        /// <summary>
        /// Gets if Include Hotstring Grid is valid. True if Valid; Otherwise false
        /// </summary>
        private bool IsIncludeHsGridValid
        {
            get
            {
                if (m_IsIncludeHsGridValid == TriEnum.NotSet)
                {
                    //Debug.WriteLine(string.Format("ErrorCounterIncHs Error count:{0}", ErrorCounterIncHs.ErrorCount));
                    if (this.ErrorCounterIncHs.ErrorCount > 0)
                    {
                        m_IsIncludeHsGridValid = TriEnum.False;
                    }
                    else
                    {
                        m_IsIncludeHsGridValid = TriEnum.True;
                    }
                }
                // validation has occurred, do not validate again until flag has been
                // reset in CommandPrePorcess event
               // this.IncHotstringGridNeedsValidation = false;
                return m_IsIncludeHsGridValid == TriEnum.True;
            }
        }

        /// <summary>
        /// Gets if all Include controls are valid including grids
        /// </summary>
        private bool IsIncludeValid
        {
            get
            {
                bool b = true;
                b &= this.IsIncludeControlsValid;
                b &= this.IsIncludeHkGridValid;
                b &= this.IsIncludeHsGridValid;
                return b;                
            }
        }

        /// <summary>
        /// Gets if all Hotstrings are valid including grids
        /// </summary>
        private bool IsHotStringValid
        {
            get
            {
                bool b = true;
                b &= this.IsHotStringControlsValid;
                b &= this.IsHotstringReplacementGridValid;
                return b;
            }
        }
        #endregion

        #region DataGrid Properties

        #region Includes Hotstring Grid
        /// <summary>
        /// Specifies the Previous state of the Includes Hotstring Grid before validation
        /// </summary>
        //private bool IncHotstringGridNeedsValidationPrev { get; set; } = true;
        /// <summary>
        /// Gets if Anything has changed on the Includes Hotstring Grid that requires Validation
        /// </summary>
       // private bool IncHotstringGridNeedsValidation { get; set; } = true;
        #endregion

        #endregion

        #endregion

        #endregion

        #region Commands

        #region Command Events
        private void CommandPrePorcess(object sender, CancelEventArgs e)
        {
            // use pre process event to trigger setting of value only once per idle event
            m_IsClipboardText = TriEnum.NotSet;
            m_MainControlsValid = TriEnum.NotSet;
            m_hkControlsValid = TriEnum.NotSet;
            m_hsControlsValid = TriEnum.NotSet;
            m_incControlsValid = TriEnum.NotSet;
            m_IsIncludeHkGridValid = TriEnum.NotSet;
            m_IsDataControlsValid = TriEnum.NotSet;
           
            m_IsHotstringReplacementGridValid = TriEnum.NotSet;
            m_IsHotKeyUserControlsValid = TriEnum.NotSet;
            m_CurrentHotStringType = null;

            // only change the grid valid state if the grid has been changed since last command check
            //if (this.IncHotstringGridNeedsValidation == true)
            //{
            //    m_IsIncludeHsGridValid = TriEnum.NotSet;
            //}
            m_IsIncludeHsGridValid = TriEnum.NotSet;
        }

        private void ToolStripTextBoxEx_CommandExecute(object sender, CancelCommandEventArgs args )
        {
            args.Cancel = CancelToolStripTextBoxExCommandExecute;
        }
        #endregion

        #region Command Management Init
        private void InitializeCommandManager()
        {
            cmdMgr.PreCommandProcess += CommandPrePorcess;

            cmdMgr.RegisterCommandExecutor(new DataGridViewEx());
            cmdMgr.RegisterCommandExecutor(new ToolStripEx());
            ToolStripTextBoxEx ToolStripTextBoxExItm = new ToolStripTextBoxEx();
            cmdMgr.RegisterCommandExecutor(ToolStripTextBoxExItm);
            ToolStripTextBoxExItm.CommandExecute += ToolStripTextBoxEx_CommandExecute;

            Command.UpdateHandler NullUpDateHandler = new Command.UpdateHandler(CmdNullHandler);
            Command.ExecuteHandler NullExecuteHandler = new Command.ExecuteHandler(CmdNullUpdate);

            #region Cut / copy / Paste / Select / Delete

            cmdMgr.Commands.Add(new Command("CmdEditCopy", new Command.ExecuteHandler(CmdEditCopy), new Command.UpdateHandler(CmdEditCopyHandler)));
            cmdMgr.Commands["CmdEditCopy"].CommandInstances.Add(mnuEditCopy);
            cmdMgr.Commands["CmdEditCopy"].CommandInstances.Add(cMnuEditCopy);

            cmdMgr.Commands.Add(new Command("CmdMainCut", new Command.ExecuteHandler(CmdEditCut), new Command.UpdateHandler(CmdEditCutHandler)));
            cmdMgr.Commands["CmdMainCut"].CommandInstances.Add(mnuEditCut);
            cmdMgr.Commands["CmdMainCut"].CommandInstances.Add(cMnuEditCut);

            cmdMgr.Commands.Add(new Command("CmdMainPaste", new Command.ExecuteHandler(CmdEditPaste), new Command.UpdateHandler(CmdEditPasteHandler)));
            cmdMgr.Commands["CmdMainPaste"].CommandInstances.Add(mnuEditPaste);
            cmdMgr.Commands["CmdMainPaste"].CommandInstances.Add(cMnuEditPaste);

            cmdMgr.Commands.Add(new Command("CmdMainSelectAll", new Command.ExecuteHandler(CmdEditSelect), new Command.UpdateHandler(CmdEditSelectHandler)));
            cmdMgr.Commands["CmdMainSelectAll"].CommandInstances.Add(mnuEditSelectAll);
            cmdMgr.Commands["CmdMainSelectAll"].CommandInstances.Add(cMnuEditSelectAll);

            cmdMgr.Commands.Add(new Command("CmdMainDelete", new Command.ExecuteHandler(CmdEditDelete), new Command.UpdateHandler(CmdEditCopyHandler)));
            cmdMgr.Commands["CmdMainDelete"].CommandInstances.Add(mnuEditDelete);
            cmdMgr.Commands["CmdMainDelete"].CommandInstances.Add(cMnuEditDelete);

            cmdMgr.Commands.Add(new Command("CmdMainUndo", new Command.ExecuteHandler(CmdEditUndo), new Command.UpdateHandler(CmdEditUndoHandler)));
            cmdMgr.Commands["CmdMainUndo"].CommandInstances.Add(mnuEditUndo);
            cmdMgr.Commands["CmdMainUndo"].CommandInstances.Add(cMnuEditUndo);
            #endregion

            #region Main
            cmdMgr.Commands.Add(new Command("CmdClose", new Command.ExecuteHandler(CmdClose), new Command.UpdateHandler(CmdCloseHandler)));
            cmdMgr.Commands["CmdClose"].CommandInstances.AddRange(new object[] { mnuClose, tsBtnClose });

            cmdMgr.Commands.Add(new Command("CmdSave", new Command.ExecuteHandler(CmdSave), new Command.UpdateHandler(CmdSaveHandler)));
            cmdMgr.Commands["CmdSave"].CommandInstances.Add(mnuSave);
            cmdMgr.Commands["CmdSave"].CommandInstances.Add(tsBtnSave);

            #endregion

            #region HotString
            cmdMgr.Commands.Add(new Command("CmdBnHs", NullExecuteHandler, new Command.UpdateHandler(CmdBnHsHandler)));
            cmdMgr.Commands.Add(new Command("CmdUcHotkey", NullExecuteHandler, new Command.UpdateHandler(CmdUcHotkey)));
            cmdMgr.Commands.Add(new Command("CmdUcHostring", NullExecuteHandler, new Command.UpdateHandler(CmdUcHostring)));

            cmdMgr.Commands.Add(new Command("CmdTabifyState", NullExecuteHandler, new Command.UpdateHandler(CmdHsTabifyHandler)));
            cmdMgr.Commands.Add(new Command("CmdForceClearState", NullExecuteHandler, new Command.UpdateHandler(CmdHsForcClearHandler)));
            cmdMgr.Commands.Add(new Command("CmdSendKeysState", NullExecuteHandler, new Command.UpdateHandler(CmdHsSendKeysHandler)));


            cmdMgr.Commands.Add(new Command("CmdHotStringData", NullExecuteHandler, new Command.UpdateHandler(CmdHotStringControlsHandler)));
            cmdMgr.Commands["CmdHotStringData"].CommandInstances.AddRange(new object[] {
                txtHsName
                ,txtHsCategory
                ,cbHsEnabled
                ,txtHsDescription
                ,cboHsType
                ,txtHsCode
                ,dgHsReplacements});

            cmdMgr.Commands.Add(new Command("CmdHsReplacementAdd", NullExecuteHandler, new Command.UpdateHandler(CmdHsReplacementAddHandler)));
            cmdMgr.Commands["CmdHsReplacementAdd"].CommandInstances.AddRange(new object[] { mnuHsReplacement, tsHsReplacements });

            cmdMgr.Commands.Add(new Command("CmdHsReplacementAddInput", new Command.ExecuteHandler(CmdHsReplacementAddInput), new Command.UpdateHandler(CmdHsReplacementAddHandler)));
            cmdMgr.Commands["CmdHsReplacementAddInput"].CommandInstances.Add(mnuHsReplacementAddInput);
            cmdMgr.Commands["CmdHsReplacementAddInput"].CommandInstances.Add(tsBtnHsReplacementsAddInput);

            cmdMgr.Commands.Add(new Command("CmdHsReplacementAddFixedList", new Command.ExecuteHandler(CmdHsReplacementAddFixedList), new Command.UpdateHandler(CmdHsReplacementAddHandler)));
            cmdMgr.Commands["CmdHsReplacementAddFixedList"].CommandInstances.Add(mnuHsReplacementAddFixedist);
            cmdMgr.Commands["CmdHsReplacementAddFixedList"].CommandInstances.Add(tsBtnHsReplacementsAddFixed);

            #region Binding Nav HotStrings
            cmdMgr.Commands.Add(new Command("CmdHsDelItem", new Command.ExecuteHandler(CmdHsDeleteNew), NullUpDateHandler));
            cmdMgr.Commands["CmdHsDelItem"].CommandInstances.Add(bnHsBtnDelete);

            cmdMgr.Commands.Add(new Command("CmdHsAddNew", NullExecuteHandler, new Command.UpdateHandler(CmdHsAddNewHandler)));
            cmdMgr.Commands["CmdHsAddNew"].CommandInstances.Add(bnHsBtnAddNew);

            cmdMgr.Commands.Add(new Command("CmdHsMoveFirst", new Command.ExecuteHandler(CmdHsMoveFirst), new Command.UpdateHandler(CmdHsMoveFirstHandler)));
            cmdMgr.Commands["CmdHsMoveFirst"].CommandInstances.Add(bnHsBtnMoveFirstItem);

            cmdMgr.Commands.Add(new Command("CmdHsMovePrevious", new Command.ExecuteHandler(CmdHsMovePrevious), new Command.UpdateHandler(CmdHsMovePreviousHandler)));
            cmdMgr.Commands["CmdHsMovePrevious"].CommandInstances.Add(bnHsBtnMovePrevItem);

            cmdMgr.Commands.Add(new Command("CmdHsMoveNext", new Command.ExecuteHandler(CmdHsMoveNext), new Command.UpdateHandler(CmdHsMoveNextHandler)));
            cmdMgr.Commands["CmdHsMoveNext"].CommandInstances.Add(btHsBtnMoveNextItem);

            cmdMgr.Commands.Add(new Command("CmdHsMoveLast", new Command.ExecuteHandler(CmdHsMoveLast), new Command.UpdateHandler(CmdHsMoveLastHandler)));
            cmdMgr.Commands["CmdHsMoveLast"].CommandInstances.Add(bnHsBtnMoveLastItem);

            cmdMgr.Commands.Add(new Command("CmdHsTsTextbox", new Command.ExecuteHandler(CmdHsTsTextbox), new Command.UpdateHandler(CmdHsTsTextboxHandler)));
            cmdMgr.Commands["CmdHsTsTextbox"].CommandInstances.Add(bnHsPosition);

            #endregion

            #endregion

            #region HotKey
            cmdMgr.Commands.Add(new Command("CmdBnHk", NullExecuteHandler, new Command.UpdateHandler(CmdBnHkHandler)));

            cmdMgr.Commands.Add(new Command("CmdHotKeyData", NullExecuteHandler, new Command.UpdateHandler(CmdHotKeyControlsHandler)));
            cmdMgr.Commands["CmdHotKeyData"].CommandInstances.AddRange(new object[] {
                txtHkName
                ,txtHkCategory
                ,cbHkEnabled
                ,txtHkLabel
                ,txtHkDescription
                ,txtHkCode});

            #region Binding Nav Hotkeys
            cmdMgr.Commands.Add(new Command("CmdHkDelItem", new Command.ExecuteHandler(CmdHkDeleteNew), NullUpDateHandler));
            cmdMgr.Commands["CmdHkDelItem"].CommandInstances.Add(bnHkBtnDelete);

            cmdMgr.Commands.Add(new Command("CmdHkAddNew", NullExecuteHandler, new Command.UpdateHandler(CmdHkAddNewHandler)));
            cmdMgr.Commands["CmdHkAddNew"].CommandInstances.Add(bnHkBtnAddNew);

            cmdMgr.Commands.Add(new Command("CmdHkMoveFirst", new Command.ExecuteHandler(CmdHkMoveFirst), new Command.UpdateHandler(CmdHkMoveFirstHandler)));
            cmdMgr.Commands["CmdHkMoveFirst"].CommandInstances.Add(bnHkBtnMoveFirstItem);

            cmdMgr.Commands.Add(new Command("CmdHkMovePrevious", new Command.ExecuteHandler(CmdHkMovePrevious), new Command.UpdateHandler(CmdHkMovePreviousHandler)));
            cmdMgr.Commands["CmdHkMovePrevious"].CommandInstances.Add(bnHkBtnMovePrevItem);

            cmdMgr.Commands.Add(new Command("CmdHkMoveNext", new Command.ExecuteHandler(CmdHkMoveNext), new Command.UpdateHandler(CmdHkMoveNextHandler)));
            cmdMgr.Commands["CmdHkMoveNext"].CommandInstances.Add(btHkBtnMoveNextItem);

            cmdMgr.Commands.Add(new Command("CmdHkMoveLast", new Command.ExecuteHandler(CmdHkMoveLast), new Command.UpdateHandler(CmdHkMoveLastHandler)));
            cmdMgr.Commands["CmdHkMoveLast"].CommandInstances.Add(bnHkBtnMoveLastItem);

            cmdMgr.Commands.Add(new Command("CmdHkTsTextbox", new Command.ExecuteHandler(CmdHkTsTextbox), new Command.UpdateHandler(CmdHkTsTextboxHandler)));
            cmdMgr.Commands["CmdHkTsTextbox"].CommandInstances.Add(bnHkPosition);

            #endregion

            #endregion

            #region Includes
            cmdMgr.Commands.Add(new Command("CmdBnInclude", NullExecuteHandler, new Command.UpdateHandler(CmdBnIncludeHandler)));

            cmdMgr.Commands.Add(new Command("CmdIncludeData", NullExecuteHandler, new Command.UpdateHandler(CmdIncludeControlsHandler)));
            cmdMgr.Commands["CmdIncludeData"].CommandInstances.AddRange(new object[] {
                txtIncName
                , chkIncEnabled
                , txtIncDescription
                , txtIncCode});

            cmdMgr.Commands.Add(new Command("CmdIncGrid", new Command.ExecuteHandler(CmdNullUpdate), new Command.UpdateHandler(CmdIncludeGrid)));

            #region Binding Nav Includes

            cmdMgr.Commands.Add(new Command("CmdIncDelItem", new Command.ExecuteHandler(CmdIncDeleteNew), new Command.UpdateHandler(CmdIncDeleteNewHandler)));
            cmdMgr.Commands["CmdIncDelItem"].CommandInstances.Add(bnIncBtnDelete);

            cmdMgr.Commands.Add(new Command("CmdIncAddNew", NullExecuteHandler, new Command.UpdateHandler(CmdIncAddNewHandler)));
            cmdMgr.Commands["CmdIncAddNew"].CommandInstances.Add(bnIncBtnAddNew);

            cmdMgr.Commands.Add(new Command("CmdIncMoveFirst", new Command.ExecuteHandler(CmdIncMoveFirst), new Command.UpdateHandler(CmdIncMoveFirstHandler)));
            cmdMgr.Commands["CmdIncMoveFirst"].CommandInstances.Add(bnIncBtnMoveFirstItem);

            cmdMgr.Commands.Add(new Command("CmdIncMovePrevious", new Command.ExecuteHandler(CmdIncMovePrevious), new Command.UpdateHandler(CmdIncMovePreviousHandler)));
            cmdMgr.Commands["CmdIncMovePrevious"].CommandInstances.Add(bnIncBtnMovePrevItem);

            cmdMgr.Commands.Add(new Command("CmdIncMoveNext", new Command.ExecuteHandler(CmdIncMoveNext), new Command.UpdateHandler(CmdIncMoveNextHandler)));
            cmdMgr.Commands["CmdIncMoveNext"].CommandInstances.Add(btIncBtnMoveNextItem);

            cmdMgr.Commands.Add(new Command("CmdIncMoveLast", new Command.ExecuteHandler(CmdIncMoveLast), new Command.UpdateHandler(CmdIncMoveLastHandler)));
            cmdMgr.Commands["CmdIncMoveLast"].CommandInstances.Add(bnIncBtnMoveLastItem);

            cmdMgr.Commands.Add(new Command("CmdIncTsTextbox", new Command.ExecuteHandler(CmdIncTsTextbox), new Command.UpdateHandler(CmdIncTsTextboxHandler)));
            cmdMgr.Commands["CmdIncTsTextbox"].CommandInstances.Add(bnIncPosition);

            #endregion

            #endregion

            #region DataItems
            cmdMgr.Commands.Add(new Command("CmdBnDataHandler", NullExecuteHandler, new Command.UpdateHandler(CmdBnDataHandler)));

            cmdMgr.Commands.Add(new Command("CmdDataControlsData", NullExecuteHandler, new Command.UpdateHandler(CmdDataControlsHandler)));
            cmdMgr.Commands["CmdDataControlsData"].CommandInstances.AddRange(new object[] {
                txtDataFileName
                , txtDataName
                , txtDataValue
                , chkDataEncoded
                , chkDataOverwrite});

            #region Binding Nav Includes

            cmdMgr.Commands.Add(new Command("CmdDataDelItem", new Command.ExecuteHandler(CmdDataDeleteNew), new Command.UpdateHandler(CmdDataDeleteNewHandler)));
            cmdMgr.Commands["CmdDataDelItem"].CommandInstances.Add(bnDataBtnDelete);

            cmdMgr.Commands.Add(new Command("CmdDataAddNew", NullExecuteHandler, new Command.UpdateHandler(CmdDataAddNewHandler)));
            cmdMgr.Commands["CmdDataAddNew"].CommandInstances.Add(bnDataBtnAddNew);

            cmdMgr.Commands.Add(new Command("CmdDataMoveFirst", new Command.ExecuteHandler(CmdDataMoveFirst), new Command.UpdateHandler(CmdDataMoveFirstHandler)));
            cmdMgr.Commands["CmdDataMoveFirst"].CommandInstances.Add(bnDataBtnMoveFirstItem);

            cmdMgr.Commands.Add(new Command("CmdDataMovePrevious", new Command.ExecuteHandler(CmdDataMovePrevious), new Command.UpdateHandler(CmdDataMovePreviousHandler)));
            cmdMgr.Commands["CmdDataMovePrevious"].CommandInstances.Add(bnDataBtnMovePrevItem);

            cmdMgr.Commands.Add(new Command("CmdDataMoveNext", new Command.ExecuteHandler(CmdDataMoveNext), new Command.UpdateHandler(CmdDataMoveNextHandler)));
            cmdMgr.Commands["CmdDataMoveNext"].CommandInstances.Add(btDataBtnMoveNextItem);

            cmdMgr.Commands.Add(new Command("CmdDataMoveLast", new Command.ExecuteHandler(CmdDataMoveLast), new Command.UpdateHandler(CmdDataMoveLastHandler)));
            cmdMgr.Commands["CmdDataMoveLast"].CommandInstances.Add(bnDataBtnMoveLastItem);

            cmdMgr.Commands.Add(new Command("CmdDataTsTextbox", new Command.ExecuteHandler(CmdDataTsTextbox), new Command.UpdateHandler(CmdDataTsTextboxHandler)));
            cmdMgr.Commands["CmdDataTsTextbox"].CommandInstances.Add(bnDataPosition);

            #endregion

            #endregion

            #region Import menu
            //CmdDataImportText
            cmdMgr.Commands.Add(new Command("CmdImportMenu", NullExecuteHandler, new Command.UpdateHandler(CmdImporMenuHandler)));
            cmdMgr.Commands["CmdImportMenu"].CommandInstances.AddRange(new object[] { mnuFileImport });

            cmdMgr.Commands.Add(new Command("CmdImportDataText", new Command.ExecuteHandler(CmdDataImportText), new Command.UpdateHandler(CmdImporMenuHandler)));
            cmdMgr.Commands["CmdImportDataText"].CommandInstances.Add(mnuFileImportText);
            cmdMgr.Commands["CmdImportDataText"].CommandInstances.Add(tsBtnDataImportText);

            cmdMgr.Commands.Add(new Command("CmdImportDataBinary", new Command.ExecuteHandler(CmdDataImportBinary), new Command.UpdateHandler(CmdImporMenuHandler)));
            cmdMgr.Commands["CmdImportDataBinary"].CommandInstances.Add(mnuFileImportBinary);
            cmdMgr.Commands["CmdImportDataBinary"].CommandInstances.Add(tsBtnDataImportBinary);
            #endregion

            #region Clone
            cmdMgr.Commands.Add(new Command("CmdCloneObject", new Command.ExecuteHandler(CmdClone), new Command.UpdateHandler(CmdCloneHandler)));
            cmdMgr.Commands["CmdCloneObject"].CommandInstances.Add(tsBtnConeObj);
            cmdMgr.Commands["CmdCloneObject"].CommandInstances.Add(mnuEditCloneObj);
            #endregion


        }
        #endregion

        #region Command Handlers

        #region Clone Handler
        private void CmdCloneHandler(Command Cmd)
        {
            bool b = false;
            b |= IsTabHotKeysActive;
            b |= IsTabHotStringActive;
            b |= IsTabIncludesActive;
            b |= IsTabDataActive;
            if (b == true)
            {
                tsClone.Visible = true;
            }
            else
            {
                tsClone.Visible = false;
                Cmd.Enabled = false;
                return;
            }
            if (IsTabHotKeysActive == true)
            {
                this.CmdCloneHotKeysHandler(Cmd);
            }
            else if (IsTabHotStringActive == true)
            {
                this.CmdCloneHotStringsHandler(Cmd);
            }
            else if (IsTabIncludesActive == true)
            {
                this.CmdCloneIncludesHandler(Cmd);
            }
            else if (IsTabDataActive == true)
            {
                this.CmdCloneDataHandler(Cmd);
            }
        }
        private void CmdCloneHotKeysHandler(Command Cmd)
        {
            bool b = true;
            b &= commandsBindingSource.Count > 0;
            b &= bnHkBtnAddNew.Enabled;
            Cmd.Enabled = b;
        }
        private void CmdCloneHotStringsHandler(Command Cmd)
        {
            bool b = true;
            b &= hotstringsBindingSource.Count > 0;
            b &= bnHsBtnAddNew.Enabled;
            Cmd.Enabled = b;
        }
        private void CmdCloneIncludesHandler(Command Cmd)
        {
            bool b = true;
            b &= includeBindingSource.Count > 0;
            b &= bnIncBtnAddNew.Enabled;
            Cmd.Enabled = b;
        }
        private void CmdCloneDataHandler(Command Cmd)
        {
            bool b = true;
            b &= dataItemBindingSource.Count > 0;
            b &= bnDataBtnAddNew.Enabled;
            Cmd.Enabled = b;
        }
        #endregion

        #region Cut / Copy / Paste / Select / Handlers
        private void CmdEditUndoHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.Enabled
                && this.FocusedTextbox.CanUndo);
        }
        private void CmdEditCutHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.Enabled
                && this.FocusedTextbox.SelectionLength > 0);
        }

        private void CmdEditCopyHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.SelectionLength > 0);
        }

        private void CmdEditSelectHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.CanSelect);
        }

        private void CmdEditPasteHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null && this.IsClipboardText == true);
        }
        #endregion

        #region Binding Nav Hotkeys
        private void CmdHkTsTextboxHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            b &= commandsBindingSource.Count > 0;
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= this.IsHotKeyControlsValid;
            Cmd.Enabled = b;
        }

        private void CmdHkMovePreviousHandler(Command Cmd)
        {
            // bnHkBtnMovePrevItem
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            b &= commandsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= commandsBindingSource.Position > 0; // zero based index
            b &= this.IsHotKeyControlsValid;
            Cmd.Enabled = b;
        }
        private void CmdHkMoveFirstHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            b &= commandsBindingSource.Count > 0;
            
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= commandsBindingSource.Position > 0; // zero based index
            b &= this.IsHotKeyControlsValid;
            Cmd.Enabled = b;
        }
        //bnHkBtnMoveLastItem
        private void CmdHkMoveLastHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            b &= commandsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= commandsBindingSource.Position < (commandsBindingSource.Count - 1); // zero based index
            b &= this.IsHotKeyControlsValid;
            Cmd.Enabled = b;
        }

        private void CmdHkMoveNextHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            b &= commandsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= commandsBindingSource.Position < (commandsBindingSource.Count - 1); // zero based index
            b &= this.IsHotKeyControlsValid;
            Cmd.Enabled = b;
        }
        private void CmdHkAddNewHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHk;
            if (b == true)
            {
                b &= this.IsHotKeyControlsValid;
            }
            Cmd.Enabled = b;
        }

        private void CmdBnHkHandler(Command Cmd)
        {
            bool bVisible = true;
            bVisible &= tcMain.SelectedIndex == IndexTabHk;
            bnHotkeys.Visible = bVisible;

        }
        #endregion

        #region Binding Nav HotStrings
        private void CmdHsTsTextboxHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            b &= hotstringsBindingSource.Count > 0;
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= this.IsHotStringValid;
            Cmd.Enabled = b;
        }

        private void CmdHsMovePreviousHandler(Command Cmd)
        {
            // bnHkBtnMovePrevItem
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            b &= hotstringsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= hotstringsBindingSource.Position > 0; // zero based index
            b &= this.IsHotStringValid;
            Cmd.Enabled = b;
        }
        private void CmdHsMoveFirstHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            b &= hotstringsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= hotstringsBindingSource.Position > 0; // zero based index
            b &= this.IsHotStringValid;
            Cmd.Enabled = b;
        }
        //bnHkBtnMoveLastItem
        private void CmdHsMoveLastHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            b &= hotstringsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= hotstringsBindingSource.Position < (hotstringsBindingSource.Count - 1); // zero based index
            b &= this.IsHotStringValid;
            Cmd.Enabled = b;
        }

        private void CmdHsMoveNextHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            b &= hotstringsBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= hotstringsBindingSource.Position < (hotstringsBindingSource.Count - 1); // zero based index
            b &= this.IsHotStringValid;
            Cmd.Enabled = b;
        }
        private void CmdHsAddNewHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabHs;
            if (b == true)
            {
                b &= this.IsHotStringValid;
            }
            Cmd.Enabled = b;
        }

        private void CmdBnHsHandler(Command Cmd)
        {
            bool bVisible = true;
            bVisible &= tcMain.SelectedIndex == IndexTabHs;
            bnHotstrings.Visible = bVisible;

        }

        private void CmdUcHotkey(Command Cmd)
        {
            UcHk.Enabled = CommandBindingList.Count > 0;
        }

        private void CmdUcHostring(Command Cmd)
        {
            UcHs.Enabled = HotstringBindingList.Count > 0;
        }


        #endregion

        #region Binding Nav Includes

        private void CmdIncTsTextboxHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= this.IsIncludeValid;
            Cmd.Enabled = b;
        }

        private void CmdIncMovePreviousHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            // bnHkBtnMovePrevItem
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= includeBindingSource.Position > 0; // zero based index
            b &= this.IsIncludeValid;
            Cmd.Enabled = b;
        }
        private void CmdIncMoveFirstHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= includeBindingSource.Position > 0; // zero based index
            b &= this.IsIncludeValid;
            Cmd.Enabled = b;
        }
        //bnHkBtnMoveLastItem
        private void CmdIncMoveLastHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= includeBindingSource.Position < (includeBindingSource.Count - 1); // zero based index
            b &= this.IsIncludeValid;
            Cmd.Enabled = b;
        }

        private void CmdIncMoveNextHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= includeBindingSource.Position < (includeBindingSource.Count - 1); // zero based index
            b &= this.IsIncludeValid;
            Cmd.Enabled = b;
        }
        private void CmdIncAddNewHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            if (b == true)
            {
                b &= this.IsIncludeValid;
            }
            Cmd.Enabled = b;
        }

        private void CmdIncDeleteNewHandler(Command Cmd)
        {
            if (this.IsTabIncludesActive == false)
            {
                return;
            }
            Cmd.Enabled = includeBindingSource.Current != null;

        }
        
        private void CmdBnIncludeHandler(Command Cmd)
        {
            bool bVisible = true;
            bVisible &= tcMain.SelectedIndex == IndexTabInclude;
            bnIncludes.Visible = bVisible;
            //Cmd.Visible = bVisible;

        }
        #endregion

        #region Hotstring Tab Handlers

        #region Hotstrnig Replacement Handlers
        private void CmdHsReplacementAddHandler(Command Cmd)
        {
            bool bVisible = true;
            bVisible &= tcMain.SelectedIndex == IndexTabHs;
            bVisible &= tcHs.SelectedIndex == IndexTabHsRpReplace;
            bVisible &= HotstringBindingList.Count > 0;
            Cmd.Visible = bVisible;

        }
        #endregion
        #endregion

        #region Include Hotstring / Hotkeys
        private void CmdIncludeGrid(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;
            
            b &= this.IsIncludeControlsValid;

            dgIncHk.Enabled = b;
            dgIncHs.Enabled = b;
            
        }
        private void CmdIncludeControlsHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabInclude;
            b &= includeBindingSource.Count > 0;
            Cmd.Enabled = b;
        }
        #endregion

        #region Binding Nav Data
          
        private void CmdDataTsTextboxHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }

        private void CmdDataMovePreviousHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            // bnHkBtnMovePrevItem
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= dataItemBindingSource.Position > 0; // zero based index
            b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }
        private void CmdDataMoveFirstHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= dataItemBindingSource.Position > 0; // zero based index
            b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }
        //bnHkBtnMoveLastItem
        private void CmdDataMoveLastHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= dataItemBindingSource.Position < (dataItemBindingSource.Count - 1); // zero based index
            b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }

        private void CmdDataMoveNextHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= dataItemBindingSource.Position < (dataItemBindingSource.Count - 1); // zero based index
            b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }
        private void CmdDataAddNewHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            if (b == true)
            {
                b &= this.IsDataControlsValid;
            }
            Cmd.Enabled = b;
        }

        private void CmdDataDeleteNewHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                return;
            }
            Cmd.Enabled = dataItemBindingSource.Current != null;

        }

        private void CmdBnDataHandler(Command Cmd)
        {
            bool bVisible = true;
            bVisible &= tcMain.SelectedIndex == IndexTabData;
            bnData.Visible = bVisible;
            tsDataImport.Visible = bVisible;
            //Cmd.Visible = bVisible;

        }

        private void CmdDataControlsHandler(Command Cmd)
        {
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;
            Cmd.Enabled = b;
        }
        #endregion

        #region Import Menu Command Handlers
        private void CmdImporMenuHandler(Command Cmd)
        {
            if (this.IsTabDataActive == false)
            {
                Cmd.Enabled = false;
                return;
            }
            bool b = true;
            b &= tcMain.SelectedIndex == IndexTabData;
            b &= dataItemBindingSource.Count > 0;
            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            //b &= this.IsDataControlsValid;
            Cmd.Enabled = b;
        }
        #endregion

        #region Other Command Handlers
        private void CmdNullHandler(Command Cmd)
        {
            // do nothing 
        }
        

        private void CmdHsTabifyHandler(Command Cmd)
        {
            if (IsTabHotStringMainTabActive == false)
            {
                return;
            }
            bool b = true;
            b &= HotstringBindingList.Count > 0;
            b &= this.IsTypePasteSelected;

           
            cbHsTabify.Enabled = b;
            lblHsTabify.Enabled = b;
        }

        private void CmdHsForcClearHandler(Command Cmd)
        {
            if (IsTabHotStringMainTabActive == false)
            {
                return;
            }
            bool b = true;
            b &= HotstringBindingList.Count > 0;
            b &= this.IsTypePasteSelected;


            cbHsForceClear.Enabled = b;
            lblHsForceClear.Enabled = b;
        }

        private void CmdHsSendKeysHandler(Command Cmd)
        {
            if (IsTabHotStringMainTabActive == false)
            {
                return;
            }
            bool b = true;
            b &= HotstringBindingList.Count > 0;
            b &= (this.IsTypePasteSelected || this.IsTypeInlineSelected);

            txtHsSendKeys.Enabled = b;
            lblHsSendKeys.Enabled = b;
        }

        private void CmdHotKeyControlsHandler(Command Cmd)
        {
            Cmd.Enabled = CommandBindingList.Count > 0;
        }
        private void CmdHotStringControlsHandler(Command Cmd)
        {
            Cmd.Enabled = HotstringBindingList.Count > 0;
        }

        
        private void CmdSaveHandler(Command Cmd)
        {
            bool b = true;
            b &= IsMainControlsValid;
            b &= IsHotKeyControlsValid;
            b &= IsHotStringValid;
            b &= IsIncludeValid;
            b &= IsDataControlsValid;
            
            Cmd.Enabled = b;
           
        }
        
        private void CmdCloseHandler(Command Cmd)
        {
            Cmd.Enabled = true;
        }
        #endregion

        #endregion

        #region Commnad Methods

        #region Clone Method
        private void CmdClone(Command Cmd)
        {
            if (IsTabHotKeysActive == true)
            {
                this.CloneHotKey();
            }
            else if (IsTabHotStringActive == true)
            {
                this.CloneHotString();
            }
            else if (IsTabIncludesActive == true)
            {
                this.CloneInclude();
            }
            else if (IsTabDataActive == true)
            {
                this.CloneData();
            }
        }
        #endregion

        #region Cut / Copy / Paste / Select / Delete Handlers
        private void CmdEditCopy(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                Clipboard.SetText(this.FocusedTextbox.SelectedText);
            }
        }

        private void CmdEditCut(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                Clipboard.SetText(FocusedTextbox.SelectedText);

                FocusedTextbox.SelectedText = string.Empty;
            }
        }

        private void CmdEditPaste(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                string str = Clipboard.GetText();
              
                FocusedTextbox.Paste(str);
            }
        }

        private void CmdEditSelect(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.SelectAll();
            }
        }

        private void CmdEditDelete(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.SelectedText = "";
            }
        }

        private void CmdEditUndo(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.Undo();
            }
        }

        #endregion

        #region Binding Nav Hotkeys
        private void CmdHkDeleteNew(Command Cmd)
        {
            // Reset Hotkey Validataion errors when record has been deleted
            ResetHkValidationErrors();
   
        }

        private void CmdHkTsTextbox(Command Cmd)
        {
            int i = -1;
            bool PosChanged = false;
            if( int.TryParse(bnHkPosition.Text, out i))
            {
                if (i > 0 && commandsBindingSource.Count >= i)
                {
                    int pos = i - 1;
                    if (commandsBindingSource.Position != pos)
                    {
                        commandsBindingSource.Position = i - 1;
                        PosChanged = true;
                    }
                   
                }
            }
            if (PosChanged == false)
            {
                i = commandsBindingSource.Position + 1;
                bnHkPosition.Text = i.ToString();
            }

        }
        private void CmdHkMovePrevious(Command Cmd)
        {
            commandsBindingSource.MovePrevious();
        }
        private void CmdHkMoveFirst(Command Cmd)
        {
            commandsBindingSource.MoveFirst();
        }
        private void CmdHkMoveLast(Command Cmd)
        {
            commandsBindingSource.MoveLast();
        }

        private void CmdHkMoveNext(Command Cmd)
        {
            commandsBindingSource.MoveNext();
        }
        #endregion

        #region Binding Nav HotStrings
        private void CmdHsDeleteNew(Command Cmd)
        {
            // Reset Hotstring Validation errors when record has been deleted
            ResetHsValidationErrors();
            HsReplacements.Clear();
        }

        private void CmdHsTsTextbox(Command Cmd)
        {
            int i = -1;
            bool PosChanged = false;
            if (int.TryParse(bnHsPosition.Text, out i))
            {
                if (i > 0 && hotstringsBindingSource.Count >= i)
                {
                    int pos = i - 1;
                    if (hotstringsBindingSource.Position != pos)
                    {
                        hotstringsBindingSource.Position = i - 1;
                        PosChanged = true;
                    }

                }
            }
            if (PosChanged == false)
            {
                i = hotstringsBindingSource.Position + 1;
                bnHsPosition.Text = i.ToString();
            }

        }
        private void CmdHsMovePrevious(Command Cmd)
        {
            hotstringsBindingSource.MovePrevious();
        }
        private void CmdHsMoveFirst(Command Cmd)
        {
            hotstringsBindingSource.MoveFirst();
        }
        private void CmdHsMoveLast(Command Cmd)
        {
            hotstringsBindingSource.MoveLast();
        }

        private void CmdHsMoveNext(Command Cmd)
        {
            hotstringsBindingSource.MoveNext();
        }
        #endregion

        #region Binding Nav Includes
        private void CmdIncDeleteNew(Command Cmd)
        {
            this.IncludingDeleting = true;
            IncHkBindingList.Clear();
            IncHsBindingList.Clear();
            includeBindingSource.Remove(includeBindingSource.Current);
            // Reset Include Validation errors when record has been deleted
            ResetIncValidationErrors();
            this.IncludingDeleting = false;


        }
        private void CmdIncTsTextbox(Command Cmd)
        {
            int i = -1;
            bool PosChanged = false;
            if (int.TryParse(bnIncPosition.Text, out i))
            {
                if (i > 0 && includeBindingSource.Count >= i)
                {
                    int pos = i - 1;
                    if (includeBindingSource.Position != pos)
                    {
                        includeBindingSource.Position = i - 1;
                        PosChanged = true;
                    }

                }
            }
            if (PosChanged == false)
            {
                i = includeBindingSource.Position + 1;
                bnIncPosition.Text = i.ToString();
            }

        }
        private void CmdIncMovePrevious(Command Cmd)
        {
            includeBindingSource.MovePrevious();
        }
        private void CmdIncMoveFirst(Command Cmd)
        {
            includeBindingSource.MoveFirst();
        }
        private void CmdIncMoveLast(Command Cmd)
        {
            includeBindingSource.MoveLast();
        }

        private void CmdIncMoveNext(Command Cmd)
        {
            includeBindingSource.MoveNext();
        }
        #endregion

        #region Binding Nav Data
        private void CmdDataDeleteNew(Command Cmd)
        {
            this.ResetDataValidationErrors();
        }
        private void CmdDataTsTextbox(Command Cmd)
        {
            int i = -1;
            bool PosChanged = false;
            if (int.TryParse(bnDataPosition.Text, out i))
            {
                if (i > 0 && dataItemBindingSource.Count >= i)
                {
                    int pos = i - 1;
                    if (dataItemBindingSource.Position != pos)
                    {
                        dataItemBindingSource.Position = i - 1;
                        PosChanged = true;
                    }
                }
            }
            if (PosChanged == false)
            {
                i = dataItemBindingSource.Position + 1;
                bnDataPosition.Text = i.ToString();
            }

        }
        private void CmdDataMovePrevious(Command Cmd)
        {
            dataItemBindingSource.MovePrevious();
        }
        private void CmdDataMoveFirst(Command Cmd)
        {
            dataItemBindingSource.MoveFirst();
        }
        private void CmdDataMoveLast(Command Cmd)
        {
            dataItemBindingSource.MoveLast();
        }

        private void CmdDataMoveNext(Command Cmd)
        {
            dataItemBindingSource.MoveNext();
        }
        #endregion

        #region Hotstring Tab Commands
        private void CmdHsReplacementAddInput(Command Cmd)
        {
            using (Editor.Dialog.frmInputDialog inputDialog = new Editor.Dialog.frmInputDialog())
            {
                if (inputDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.HsReplacements.Add(inputDialog.DialogData);
                    var hs = (hotstring)hotstringsBindingSource.Current;
                    if (hs != null)
                    {
                        hs.replacements = this.HsReplacements.ToArray();
                    }
                }
            }
          
        }
        private void CmdHsReplacementAddFixedList(Command Cmd)
        {
            using (Editor.Dialog.frmInputFixedList inputFixedList = new Editor.Dialog.frmInputFixedList())
            {
                if (inputFixedList.ShowDialog(this) == DialogResult.OK)
                {
                    this.HsReplacements.Add(inputFixedList.DialogData);
                    var hs = (hotstring)hotstringsBindingSource.Current;
                    if (hs != null)
                    {
                        hs.replacements = this.HsReplacements.ToArray();
                    }
                }
            }

        }
        #endregion

        #region Data Import Commands
        private void CmdDataImportText(Command Cmd)
        {
            this.DataImportText();
        }

        private void CmdDataImportBinary(Command Cmd)
        {
            this.DataImportBinary();
        }
        #endregion

        #region Null Commands
        private void CmdNullUpdate(Command Cmd)
        { }
        #endregion

        #region Save / Close
        private void CmdSave(Command Cmd)
        {
            if (this.ValidateChildren() == true)
            {
                this.SaveCurrentIncludeArrays();

                this.SetHotkeySnippit();
                
                this.SetIncludeSnippit();

                this.plg.commands = CommandBindingList.ToArray();
                this.plg.hotstrings = HotstringBindingList.ToArray();
                this.plg.includes = IncludeBindingList.ToArray();
                this.plg.dataItems = DataItemBindingList.ToArray();
                if (frmPlugin.IsInstance == true)
                {
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
               

            }
        }
        private void CmdClose(Command Cmd)
        {
            if (frmPlugin.IsInstance == true)
            {
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            
        }
        #endregion
        #endregion

        #endregion

        #region Form Events

        #region Form Loading Events
        private void frmPlugin_Load(object sender, EventArgs e)
        {
            if (this.DesignMode == true)
            {
                return;
            }
            this.IsNew = string.IsNullOrEmpty(this.plg.File);
            int iCommand = -1;
            int iHotString = -1;
            int iInclude = -1;
            if (string.IsNullOrEmpty(this.plg.minVersion) == true)
            {
                this.plg.minVersion = AppCommon.Instance.DefaultMinVersion.ToString();
            }
            if (plg.commands != null)
            {
                bool DoInitialCommand = !string.IsNullOrEmpty(this.InitailCommand);
                for (int i = 0; i < plg.commands.Length; i++)
                {
                    var cmd = plg.commands[i];
                    command newCmd = CommandBindingList.AddNew();
                    newCmd.Populate(cmd);
                    if (DoInitialCommand == true && iCommand == -1 && newCmd.hotkey == this.InitailCommand)
                    {
                        iCommand = i;
                    }
                }
               
            }

            if (plg.hotstrings != null)
            {
                bool DoInitialHotstring = !string.IsNullOrEmpty(this.InitialHotString);
                for (int i = 0; i < plg.hotstrings.Length; i++)
                {
                    var hs = plg.hotstrings[i];
                    hotstring newHs = HotstringBindingList.AddNew();
                    newHs.Populate(hs, false);
                    if (DoInitialHotstring == true && iHotString == -1 && newHs.trigger == this.InitialHotString)
                    {
                        iHotString = i;
                    }
                }
            }

            if (plg.includes != null)
            {
                bool DoInitalInclude = !string.IsNullOrEmpty(this.InitialInclude);
                for (int i = 0; i < plg.includes.Length; i++)
                {
                    var inc = plg.includes[i];
                    include newInc = IncludeBindingList.AddNew();
                    inc.PoplulateOther(newInc, false);
                    if (DoInitalInclude == true && iInclude == -1 && newInc.name == this.InitialInclude)
                    {
                        iInclude = i;
                    }
                }
            }

            if (plg.dataItems != null)
            {
                for (int i = 0; i < plg.dataItems.Length; i++)
                {
                    dataItem di = plg.dataItems[i];
                    dataItem newDi = DataItemBindingList.AddNew();
                    di.PoplulateOther(newDi, false);
                }
            }

            // if the version is not supplied then add a default of version 1
            if (string.IsNullOrEmpty(plg.version) == true)
            {
                plg.version = @"1.0.0.0";
            }

            pluginBindingSource.DataSource = this.plg;
            includeBindingSource.DataSource = IncludeBindingList;
            if (iInclude > -1)
            {
                includeBindingSource.Position = iInclude;
            }

            commandsBindingSource.DataSource = CommandBindingList;
            if (iCommand > -1)
            {
                commandsBindingSource.Position = iCommand;
            }
            hotstringsBindingSource.DataSource = HotstringBindingList;
            if (iHotString > -1)
            {
                hotstringsBindingSource.Position = iHotString;
            }
            
            //AddEditHsReplacementGrid();
            HsReplacementsBiindingSource.DataSource = HsReplacements;

            hkIncBindingSource.DataSource = IncHkBindingList;
            hsIncBindingSource.DataSource = IncHsBindingList;

            dataItemBindingSource.DataSource = DataItemBindingList;


            tsBtnHsReplacementsAddInput.Image = Z.IconLibrary.Silk.Icon.ApplicationAdd.GetImage();
            tsBtnHsReplacementsAddFixed.Image = Z.IconLibrary.Silk.Icon.ApplicationFormAdd.GetImage();
            mnuHsReplacementAddInput.Image = Z.IconLibrary.Silk.Icon.ApplicationAdd.GetImage();
            mnuHsReplacementAddFixedist.Image = Z.IconLibrary.Silk.Icon.ApplicationFormAdd.GetImage();

            mnuEditCut.Image = Z.IconLibrary.Silk.Icon.Cut.GetImage();
            mnuEditCopy.Image = Z.IconLibrary.Silk.Icon.PageCopy.GetImage();
            mnuEditPaste.Image = Z.IconLibrary.Silk.Icon.PagePaste.GetImage();
            mnuEditDelete.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            mnuEditUndo.Image = Z.IconLibrary.Silk.Icon.ArrowUndo.GetImage();

            cMnuEditCut.Image = Z.IconLibrary.Silk.Icon.Cut.GetImage();
            cMnuEditCopy.Image = Z.IconLibrary.Silk.Icon.PageCopy.GetImage();
            cMnuEditPaste.Image = Z.IconLibrary.Silk.Icon.PagePaste.GetImage();
            cMnuEditDelete.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            cMnuEditUndo.Image = Z.IconLibrary.Silk.Icon.ArrowUndo.GetImage();

            this.IsInitialized = true;
            // update the various Databinding Sources to proper display indexes and what not.
            hotstringsBindingSource_CurrentChanged(null, new EventArgs());
            commandsBindingSource_CurrentChanged(null, new EventArgs());
            includeBindingSource_CurrentChanged(null, new EventArgs());
            dataItemBindingSource_CurrentChanged(null, new EventArgs());
            if (iHotString > -1)
            {
                tcMain.SelectedIndex = IndexTabHs;
            }
            else if (iCommand > -1)
            {
                tcMain.SelectedIndex = IndexTabHk;
            }
            else if (iInclude > -1)
            {
                tcMain.SelectedIndex = IndexTabInclude;
            }

            if (Properties.Settings.Default.PlgWindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.PlgLocation;
                Size = Properties.Settings.Default.PlgSize;
            }
            else if (Properties.Settings.Default.PlgWindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.PlgLocation;
                Size = Properties.Settings.Default.PlgSize;
            }
            else
            {
                Location = Properties.Settings.Default.PlgLocation;
                Size = Properties.Settings.Default.PlgSize;
            }

            this.FocusedTextbox = null;
            this.SetTextBoxGeneralEvents();
        }
        #endregion

        #region Form Closing Evnets
        private void frmPlugin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.PlgLocation = RestoreBounds.Location;
                Properties.Settings.Default.PlgWindowState = WindowState;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.PlgLocation = Location;
                Properties.Settings.Default.PlgSize = Size;
                Properties.Settings.Default.PlgWindowState = WindowState;
            }
            else
            {
                Properties.Settings.Default.PlgLocation = RestoreBounds.Location;
                Properties.Settings.Default.PlgSize = RestoreBounds.Size;
                Properties.Settings.Default.PlgWindowState = WindowState;
            }
        }

        private void frmPlugin_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
        #endregion

        #endregion

        #region Validation

        #region Main Validation
        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidMainControls()
        {
            bool retval = true;
            CancelEventArgs ce = new CancelEventArgs();
            txtMainName_Validating(null, ce);
            retval &= !ce.Cancel;
            if (retval == true)
            {
                txtMainVersion_Validating(null, ce);
                retval &= !ce.Cancel;
            }
         
            return retval;
        }
        private void txtMainName_Validating(object sender, CancelEventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtMainName.Text))
            {
                e.Cancel = true;
                //txtFileName.Focus();
                if (sender != null)
                    EpMain.SetError(txtMainName, string.Format(Properties.Resources.ValidationRequired, lblMainName.Text));
                return;
            }
            if (txtMainName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtMainName, string.Format(Properties.Resources.ValidationFormat, lblMainName.Text));
                return;
            }
            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtMainName, null);
            }


        }

        private void txtMainVersion_Validating(object sender, CancelEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtMainVersion.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtMainVersion, string.Format(Properties.Resources.ValidationRequired, lblMainVersion.Text));
                return;
            }
            bool IsMatch = RegularExpressions.VersionRegex.IsMatch(txtMainVersion.Text);
            if (IsMatch == true)
            {
                e.Cancel = false;
                if (sender != null)
                    EpMain.SetError(txtMainVersion, null);
                return;
            }
            else
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtMainVersion, string.Format(Properties.Resources.ValidationFormat, lblMainVersion.Text));
                return;
            }
        }


        #endregion

        #region HotStrings Validation
        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidHsControls()
        {
            bool retval = true;
            if (HotstringBindingList.Count == 0)
            {
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            txtHsName_Validating(null, ce);
            retval &= !ce.Cancel;
           
            if (retval == true)
            {
                txtHsCode_Validating(null, ce);
                retval &= !ce.Cancel;
            }
            if (retval == true)
            {
                txtHsCategory_Validating(null, ce);
                retval &= !ce.Cancel;
            }
           
            return retval;

        }

        private void txtHsName_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotstringData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHsName.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHsName, string.Format(Properties.Resources.ValidationRequired, lblHsName.Text));
                return;
            }

            //if (txtHsName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            //{
            //    e.Cancel = true;
            //    if (sender != null)
            //        EpMain.SetError(txtHsName, string.Format(Properties.Resources.ValidationFormat, lblHsName.Text));
            //    return;
            //}

            hotstring hs = HotstringBindingList[hotstringsBindingSource.Position];
            if (hs != null)
            {
                if (HotstringBindingList.Any(
                     x => string.Equals(x.name, txtHsName.Text, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, hs)))
                {
                    e.Cancel = true;
                    if (sender != null)
                        EpMain.SetError(txtHsName, string.Format(Properties.Resources.ValidationDublicateNotAllowed, lblHsName.Text));
                    return;
                }
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtHsName, null);
            }
        }

        private void txtHsCategory_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotstringData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHsCategory.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHsCategory, string.Format(Properties.Resources.ValidationRequired, lblHsCategroy.Text));
                return;
            }
        }

        private void txtHsCode_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotstringData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHsCode.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHsCode, string.Format(Properties.Resources.ValidationRequired, lblCode.Text));
                return;
            }
            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtHsCode, null);
                return;
            }
        }

        
        private void cboHsType_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotstringData(sender, e) == false) return;
           
        }

        /// <summary>
        /// Checks to see if the Data source for hotstrings has any data. If there is not data return value is false
        /// </summary>
        /// <param name="sender">The Object Sender</param>
        /// <param name="e">The args</param>
        private bool Text_ValidatingHotstringData(object sender, CancelEventArgs e)
        {
            if (HotStringControlsCreated == false) return false;

            if (hotstringsBindingSource.Count == 0)
            {
                if (sender != null)
                    EpMain.SetError((Control)sender, null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Resets all Error Messages for HotString Controls
        /// </summary>
        private void ResetHsValidationErrors()
        {
            EpMain.SetError(txtHsName, null);
            EpMain.SetError(txtHsCode, null);
            EpMain.SetError(txtHsCategory, null);
        }
        #endregion

        #region Hotkeys Validation
        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidHkControls()
        {
            bool retval = true;
            if (CommandBindingList.Count == 0)
            {
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            txtHkName_Validating(null, ce);
            retval &= !ce.Cancel;
            if (retval == true)
            {
                txtHkLabel_Validating(null, ce);
                retval &= !ce.Cancel;
            }
          
            if (retval == true)
            {
                txtHkCode_Validating(null, ce);
                retval &= !ce.Cancel;
            }

            if (retval == true)
            {
                txtHkCategory_Validating(null, ce);
                retval &= !ce.Cancel;
            }
           
            return retval;
            
        }
        private void txtHkName_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotkeyData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHkName.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHkName, string.Format(Properties.Resources.ValidationRequired, lblHkName.Text));
                return;
            }

            //if (txtHkName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            //{
            //    e.Cancel = true;
            //    if (sender != null)
            //        EpMain.SetError(txtHkName, string.Format(Properties.Resources.ValidationFormat, lblHkName.Text));
            //    return;
            //}

            command hk = CommandBindingList[commandsBindingSource.Position];
            if (hk != null)
            {
                if (CommandBindingList.Any(
                     x => string.Equals(x.name, txtHkName.Text, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, hk)))
                {
                    e.Cancel = true;
                    if (sender != null)
                        EpMain.SetError(txtHkName, string.Format(Properties.Resources.ValidationDublicateNotAllowed, lblHkName.Text));
                    return;
                }
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtHkName, null);
            }
        }


        private void txtHkCategory_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotkeyData(sender, e) == false) return;
            if (string.IsNullOrWhiteSpace(txtHkCategory.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHkCategory, string.Format(Properties.Resources.ValidationRequired, lblHkCategory.Text));
                return;
            }
        }
        private void txtHkLabel_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotkeyData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHkLabel.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHkLabel, string.Format(Properties.Resources.ValidationRequired, lblHkLabel.Text));
                return;
            }
            bool IsMatch = RegularExpressions.LetternNummberUnderscoreRegex.IsMatch(txtHkLabel.Text);
            if (IsMatch == true)
            {
                e.Cancel = false;
                if (sender != null)
                    EpMain.SetError(txtHkLabel, null);
                return;
            }
            else
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHkLabel, string.Format(Properties.Resources.ValidationNumberLettersUnserscore, lblHkLabel.Text));
                return;
            }
        }

       

        private void txtHkCode_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotkeyData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtHkCode.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtHkCode, string.Format(Properties.Resources.ValidationRequired, gbCode.Text));
                return;
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtHkCode, null);
            }

        }

      
        /// <summary>
        /// Checks to see if the Data source for hotkeys has any data. If there is not data e.cancel is set to true
        /// </summary>
        /// <param name="sender">The Object Sender</param>
        /// <param name="e">The args</param>
        private bool Text_ValidatingHotkeyData(object sender, CancelEventArgs e)
        {
            if (HotkeyControlsCreated == false) return false;

            if (commandsBindingSource.Count == 0)
            {
                if (sender != null)
                    EpMain.SetError((Control)sender, null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Resets all Error Messages for Hotstring Controls
        /// </summary>
        private void ResetHkValidationErrors()
        {
            EpMain.SetError(txtHkLabel, null);
            EpMain.SetError(txtHkName, null);
            EpMain.SetError(txtHkCategory, null);
        }

       
        #endregion

        #region Include Validation

        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidIncControls()
        {
            bool retval = true;
            if (includeBindingSource.Count == 0)
            {
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            txtIncName_Validating(null, ce);
            retval &= !ce.Cancel;
           
            if (retval == true)
            {
                txtIncCode_Validating(null, ce);
                retval &= !ce.Cancel;
            }
            return retval;

        }

        private void txtIncName_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingIncData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtIncName.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtIncName, string.Format(Properties.Resources.ValidationRequired, lblIncName.Text));
                return;
            }

            //if (txtIncName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            //{
            //    e.Cancel = true;
            //    if (sender != null)
            //        EpMain.SetError(txtIncName, string.Format(Properties.Resources.ValidationFormat, lblIncName.Text));
            //    return;
            //}

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtIncName, null);
            }
        }

       

        private void txtIncCode_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingIncData(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtIncCode.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtIncCode, string.Format(Properties.Resources.ValidationRequired, lblIncCode.Text));
                return;
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtIncCode, null);
            }
        }

        /// <summary>
        /// Checks to see if the Data source for hotkeys has any data. If there is not data e.cancel is set to true
        /// </summary>
        /// <param name="sender">The Object Sender</param>
        /// <param name="e">The args</param>
        private bool Text_ValidatingIncData(object sender, CancelEventArgs e)
        {
            if (IncludeControlsCreated == false) return false;

            if (includeBindingSource.Count == 0)
            {
                if (sender != null)
                    EpMain.SetError((Control)sender, null);
                return false;
            }
            return true;
        }
        private void ResetIncValidationErrors()
        {
            EpMain.SetError(txtIncName, null);
            EpMain.SetError(txtIncCode, null);
            ErrorCounterIncHs.ErrorCount = 0;
            ErrorCounterIncHk.ErrorCount = 0;
        }
        #endregion

        #region Hotkey Validated
        private void txtHkName_Validated(object sender, EventArgs e)
        {
            if (this.IsNew == true)
            {
                var hk = (command)this.commandsBindingSource.Current;
                
            }
        }

        #endregion

        #region User Control Hotkey Validation 
        /// <summary>
        /// Validates the controls for hotkeys User Control without setting errors
        /// </summary>
        private bool IsValidUcHkControls()
        {
            bool retval = true;
            if (commandsBindingSource.Count == 0)
            {
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            UcHk_Validating(null, ce);
            retval &= !ce.Cancel;
            
            return retval;

        }
        private void UcHk_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingHotkeyData(sender, e) == false) return;
            if (UcHk.HotKey.IsValid == false)
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(gbHotkey, string.Format(Properties.Resources.ValidationInvalidKeyCombination, gbHotkey.Text));
                return;
            }

            e.Cancel = false;
            if (sender != null)
                EpMain.SetError(gbHotkey, null);
            return;
        }
        #endregion
        
        #region Data Item Validation

        private void txtDataName_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingDataCtls(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtDataName.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtDataName, string.Format(Properties.Resources.ValidationRequired, lblDataName.Text));
                return;
            }
            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtIncName, null);
            }
        }


        private void txtDataFileName_Validating(object sender, CancelEventArgs e)
        {
            if (Text_ValidatingDataCtls(sender, e) == false) return;

            if (string.IsNullOrWhiteSpace(txtDataFileName.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtDataFileName, string.Format(Properties.Resources.ValidationRequired, lblDataFileName.Text));
                return;
            }

            if (txtDataFileName.Text.Length > 200)
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtDataFileName, string.Format(Properties.Resources.ValidationFieldToLong, lblDataFileName.Text, "200"));
                return;
            }
            if (txtDataFileName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                e.Cancel = true;
                if (sender != null)
                    EpMain.SetError(txtDataFileName, string.Format(Properties.Resources.ValidationFormat, lblDataFileName.Text));
                return;
            }

            
            dataItem di = DataItemBindingList[dataItemBindingSource.Position];
            if (di != null)
            {
                if (DataItemBindingList.Any(
                     x => string.Equals(x.dataFileName, txtDataFileName.Text, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, di)))
                {
                    e.Cancel = true;
                    if (sender != null)
                        EpMain.SetError(txtDataFileName, string.Format(Properties.Resources.ValidationDublicateNotAllowed, lblDataFileName.Text));
                    return;
                }
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(txtDataFileName, null);
            }
        }

        /// <summary>
        /// Checks to see if the Data source for Data Tabs page has any data. If there is not data e.cancel is set to true
        /// </summary>
        /// <param name="sender">The Object Sender</param>
        /// <param name="e">The args</param>
        private bool Text_ValidatingDataCtls(object sender, CancelEventArgs e)
        {
            Debug.WriteLine(String.Format("DataControlsCreated: {0}", DataControlsCreated.ToString()));
            if (DataControlsCreated == false) return false;

            if (dataItemBindingSource.Count == 0)
            {
                if (sender != null)
                    EpMain.SetError((Control)sender, null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidDataControls()
        {
            bool retval = true;
            if (dataItemBindingSource.Count == 0)
            {
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            txtDataName_Validating(null, ce);
            retval &= !ce.Cancel;

            if (retval == true)
            {
                txtDataFileName_Validating(null, ce);
                retval &= !ce.Cancel;
            }
            return retval;

        }

        /// <summary>
        /// Resets all Error Messages for Hotstring Controls
        /// </summary>
        private void ResetDataValidationErrors()
        {
            EpMain.SetError(txtDataName, null);
            EpMain.SetError(txtDataFileName, null);
        }
        #endregion

        #endregion

        #region Disposing
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                cmdMgr.PreCommandProcess -= CommandPrePorcess;
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Binding Source Events

        #region Hotstring Binding Source
        private void hotstringsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }

            try
            {
                var hs = (hotstring)hotstringsBindingSource.Current;
                if (hs == null)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                
                // tabify will default to false in the constructor for hotstring
                // make sure tabify Specified is set to true to ensure the element is written into the xml
                // if tabifySpecified is false the element may not be saved even if the value of tabify is true.
                // this seems to be due to the fact that the element is optional and with a default of false.
                hs.tabifySpecified = true; 

                this.UcHs.HotStringValue = hs;
                HsReplacements.Clear();
                if (hs != null && hs.replacements != null)
                {
                    foreach (var r in hs.replacements)
                    {
                        if (r is IReplacement)
                        {
                            HsReplacements.Add((IReplacement)r);
                        }
                    }
                }
                int i = hotstringsBindingSource.Position + 1;
                bnHsPosition.SetText(i.ToString());

                ResetHsValidationErrors();

                UcHs.IsNew = hs == null;
                if (UcHs.IsNew == false)
                {
                    string strHsType = hs.type.ToString();
                    for (int j = 0; j < cboHsType.Items.Count; j++)
                    {
                        SortedMapItem itm = (SortedMapItem)cboHsType.Items[j];
                        if (itm.Key == strHsType)
                        {
                            cboHsType.SelectedIndex = j;
                            break;
                        }
                    }
                }
                else
                {
                    string strHsType = HotStringSendEnum.Send.ToString();
                    for (int j = 0; j < cboHsType.Items.Count; j++)
                    {
                        SortedMapItem itm = (SortedMapItem)cboHsType.Items[j];
                        if (itm.Key == strHsType)
                        {
                            cboHsType.SelectedIndex = j;
                            break;
                        }
                    }
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
        #endregion

        #region Command Binding Source
        private void commandsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }
            try
            {
                command c = (command)commandsBindingSource.Current;
                if (c == null)
                    return;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                int prevIndex = CommandCurrentIndex;
                CommandCurrentIndex = commandsBindingSource.Position;

                int i = commandsBindingSource.Position + 1;
                bnHkPosition.SetText(i.ToString());
                
                if (HotkeyKeys.TryParse(c.hotkey, out hsk))
                {
                    UcHk.HotKey = hsk;
                }
                else
                {
                    hsk = new HotkeyKeys();
                    UcHk.HotKey = hsk;
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
           
            ResetHkValidationErrors();

        }

        #endregion

        #region Data Binding Source
        private void dataItemBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }
            try
            {
                dataItem d = (dataItem)dataItemBindingSource.Current;
                if (d == null)
                    return;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                int prevIndex = DataCurrentIndex;
                DataCurrentIndex = dataItemBindingSource.Position;

                int i = dataItemBindingSource.Position + 1;
                bnDataPosition.SetText(i.ToString());

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            ResetDataValidationErrors();
        }
        #endregion

        #region Misc Methods

        /// <summary>
        /// Saves any include Hotkeys or Hotstring to the plugin current selected includes
        /// </summary>
        /// <remarks>
        /// The Include hotkey and hotstrings are loosly bound to the include binding list. This method
        /// saves any unsaved hotkey and hotstrings to the plugin includes. Other changes are saved when the
        /// inclduesBindingList changes. This method should be called on save to ensure any changes on the
        /// currently selected includes are save back to the plugin
        /// </remarks>
        private void SaveCurrentIncludeArrays()
        {
            if (IncludeCurrentIndex > -1 && IncludeCurrentIndex < includeBindingSource.Count)
            {
                var Inc = (include)includeBindingSource[IncludeCurrentIndex];
                if (Inc != null)
                {
                    Inc.commands = IncHkBindingList.ToArray();
                    Inc.hotstrings = IncHsBindingList.ToArray();
                }
            }
        }
        #endregion

        #region Include Binding Source
        private void includeBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }
            try
            {
                var inc = (include)includeBindingSource.Current;
                if (inc == null)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                // Setting the previous item commands and hotstring here is needed to update
                // the commands and hotstring as we can not bind an array to the grid. Instead
                // we read the array into a binding list and then save the binding lists value back
                // to the include as an array again.
                //
                // However this method is being called because an include has just been deleted
                // from the includes binding list then we would not want to assign the previous
                // include the include hotstring or include command values

                if (this.IncludingDeleting == false)
                {
                    int prevIndex = IncludeCurrentIndex;
                    IncludeCurrentIndex = includeBindingSource.Position;

                    if (prevIndex > -1 && prevIndex < includeBindingSource.Count)
                    {
                        var prevInc = (include)includeBindingSource[prevIndex];
                        if (prevInc != null)
                        {
                            prevInc.commands = IncHkBindingList.ToArray();
                            prevInc.hotstrings = IncHsBindingList.ToArray();
                        }
                    }
                }

                IncHkBindingList.Clear();
                ErrorCounterIncHk.ErrorCount = 0;
                if (inc.commands != null)
                {
                    foreach (var c in inc.commands)
                    {
                        var cNew = IncHkBindingList.AddNew();
                        c.PoplulateOther(cNew);
                        cNew.Errors = ErrorCounterIncHk;
                    }
                }

                IncHsBindingList.Clear();
                // reset the error count for hotstring includes
                ErrorCounterIncHs.ErrorCount = 0;
                if (inc.hotstrings != null)
                {
                    foreach (var h in inc.hotstrings)
                    {
                        var hNew = IncHsBindingList.AddNew();
                        h.PoplulateOther(hNew);
                        hNew.Errors = this.ErrorCounterIncHs;
                    }
                }

                int i = includeBindingSource.Position + 1;
                bnIncPosition.SetText(i.ToString());

                ResetIncValidationErrors();
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
        #endregion

        #region Include Hotstring Binding Source
        private void hsIncBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            this.IncHsLastAdded = new includeHotstring();
            this.IncHsLastAdded.Parent = this.IncHsBindingList;
            this.IncHsLastAdded.Errors = this.ErrorCounterIncHs;
            e.NewObject = this.IncHsLastAdded;
            this.IncludHsLastAddedIndex = dgIncHs.NewRowIndex;
        }
        
        #endregion

        #region Include Hotkey Binding Source
        private void hkIncBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            this.IncHkLastAdded = new includeCommand();
            this.IncHkLastAdded.Parent = this.IncHkBindingList;
            this.IncHkLastAdded.Errors = this.ErrorCounterIncHk;
            e.NewObject = this.IncHkLastAdded;
            this.IncludHkLastAddedIndex = dgIncHk.NewRowIndex;
        }
        #endregion
        #endregion

        #region Datagrid

        #region Datagrid Replacements
        private void DgReplacementsAddImgColumns()
        {
            DataGridViewImageColumn edit = new DataGridViewImageColumn();
            edit.Image = Z.IconLibrary.Silk.Icon.Pencil.GetImage();
            edit.Width = 20;
            edit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgHsReplacements.Columns.Add(edit);

            DataGridViewImageColumn del = new DataGridViewImageColumn();
            del.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            del.Width = 20;
            del.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgHsReplacements.Columns.Add(del);
        }

        private void dgHsReplacements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgHsReplacements.NewRowIndex != e.RowIndex)
            {
                if (e.ColumnIndex == 2) // edit currnet
                {
                    IReplacement itm = HsReplacements[e.RowIndex];
                    if (itm.ReplacementType == ReplacementEnum.InputDialog)
                    {
                        Editor.Dialog.frmInputDialog inputDialog = new Editor.Dialog.frmInputDialog();
                        inputDialog.DialogData = (inputReplacement)itm;
                        if (inputDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            this.HsReplacements[e.RowIndex] = inputDialog.DialogData;
                            var hs = (hotstring)hotstringsBindingSource.Current;
                            if (hs != null)
                            {
                                hs.replacements = this.HsReplacements.ToArray();
                            }
                        }
                        inputDialog.Dispose();
                    }
                    else if (itm.ReplacementType == ReplacementEnum.InputFixedList)
                    {
                        Editor.Dialog.frmInputFixedList inputFixedList = new Editor.Dialog.frmInputFixedList();
                        inputFixedList.DialogData = (inputFixedList)itm;
                        if (inputFixedList.ShowDialog(this) == DialogResult.OK)
                        {
                            this.HsReplacements[e.RowIndex] = inputFixedList.DialogData;
                            var hs = (hotstring)hotstringsBindingSource.Current;
                            if (hs != null)
                            {
                                hs.replacements = this.HsReplacements.ToArray();
                            }
                        }
                        inputFixedList.Dispose();
                    }

                }
                if (e.ColumnIndex == 3) // delete
                {
                    var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        this.HsReplacements.RemoveAt(e.RowIndex);
                        var hs = (hotstring)hotstringsBindingSource.Current;
                        if (hs != null)
                        {
                            hs.replacements = this.HsReplacements.ToArray();
                        }
                    }
                }

            }
        }

        private void dgHsReplacements_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // add delete image to new rows
            int delColIndex = dgHsReplacements.ColumnCount - 1;
            int editColIndex = delColIndex - 1;
            e.Row.Cells[delColIndex].Value = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            e.Row.Cells[editColIndex].Value = Z.IconLibrary.Silk.Icon.Pencil.GetImage();
        }

        private void dgHsReplacements_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                      , MessageBoxButtons.YesNo
                      , MessageBoxIcon.Question
                      , MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region dgIncHk
        private void DgIncludeHotKeyAddImgColumns()
        {
            DataGridViewImageColumn del = new DataGridViewImageColumn();
            del.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            del.Width = 20;
            del.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgIncHk.Columns.Add(del);
        }

        private void dgIncHk_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                e.Cancel = true;
                HotkeyKeys hks;
                string sHotKey = string.Empty;
                if (dgIncHk.NewRowIndex > e.RowIndex && dgIncHk.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    sHotKey = dgIncHk.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                }

                if (string.IsNullOrEmpty(sHotKey))
                {
                    hks = new HotkeyKeys();
                }
                else
                {
                    if (!HotkeyKeys.TryParse(sHotKey, out hks))
                    {
                        hks = new HotkeyKeys();
                    }
                }
                frmHotkeyEdit frm = new frmHotkeyEdit();
                frm.Hotkey = hks;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    dgIncHk.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hks.ToString();
                    dgIncHk.NotifyCurrentCellDirty(true);
                    e.Cancel = false;
                }

            }
        }

        private void dgIncHk_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                      , MessageBoxButtons.YesNo
                      , MessageBoxIcon.Question
                      , MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }

            includeCommand icmd;
            icmd = IncHkBindingList[e.Row.Index];
            // clear any errors the row has before deleting
            icmd.ClearErrors();
        }

        private void dgIncHk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgIncHk.NewRowIndex != e.RowIndex)
            {
                int delColIndex = dgIncHk.ColumnCount - 1;
                if (e.ColumnIndex == delColIndex) // delete
                {
                    var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        this.IncHkBindingList[e.RowIndex].ClearErrors();
                        this.IncHkBindingList.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void dgIncHk_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // add delete image to new rows
            int colIndex = dgIncHk.ColumnCount - 1;
            e.Row.Cells[colIndex].Value = Z.IconLibrary.Silk.Icon.Delete.GetImage();
        }

        private void dgIncHk_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (this.IsInitialized == false)
            {
                return;
            }
            // when a new hotkey is added to the grid and the user hits the
            // escape key the row is never committed but is deleted.
            // here we check to see if the user is deleting an un-committed row
            // and reseting any errors that row may have had.
            if (e.RowCount == 1 && e.RowIndex == this.IncludHkLastAddedIndex)
            {
                if (this.IncHkLastAdded != null)
                {
                    this.IncHkLastAdded.ClearErrors();
                    this.IncHkLastAdded = null;
                    this.IncludHkLastAddedIndex = -1;
                }
            }
        }
        #endregion

        #region dgIncHs
        private void DgIncludeHotstringsAddImgColumns()
        {
             DataGridViewImageColumn del = new DataGridViewImageColumn();
            del.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            del.Width = 20;
            del.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgIncHs.Columns.Add(del);
        }

        private void dgIncHs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                       , MessageBoxButtons.YesNo
                       , MessageBoxIcon.Question
                       , MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            includeHotstring ihs;
            ihs = IncHsBindingList[e.Row.Index];
            // clear any errors the row has before deleting
            ihs.ClearErrors();
        }

        private void dgIncHs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgIncHs.NewRowIndex != e.RowIndex)
            {
                int delColIndex = dgIncHs.ColumnCount - 1;
                if (e.ColumnIndex == delColIndex) // delete
                {
                    var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        this.IncHsBindingList[e.RowIndex].ClearErrors();
                        this.IncHsBindingList.RemoveAt(e.RowIndex);
                        
                    }
                }

            }
           
            
        }

        private void dgIncHs_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // add delete image to new rows
            int colIndex = dgIncHs.ColumnCount - 1;
            e.Row.Cells[colIndex].Value = Z.IconLibrary.Silk.Icon.Delete.GetImage();
        }

        private void dgIncHs_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (this.IsInitialized == false)
            {
                return;
            }
            // when a new hotstring is added to the grid and the user hits the
            // escape key the row is never committed but is deleted.
            // here we check to see if the user is deleting an un-committed row
            // and reseting any errors that row may have had.
            if (e.RowCount == 1 && e.RowIndex == this.IncludHsLastAddedIndex)
            {
                if (this.IncHsLastAdded != null)
                {
                    this.IncHsLastAdded.ClearErrors();
                    this.IncHsLastAdded = null;
                    this.IncludHsLastAddedIndex = -1;
                }
            }
        }
        #endregion

        #region Grid Validation
        /// <summary>
        /// Gets if a Grid contains Data Errors
        /// </summary>
        /// <param name="grid">The grid to check</param>
        /// <returns>
        /// -1 if no Errors are found; Otherwise the row index the error occurred on.
        /// </returns>
        /// <remarks>
        /// This method is fine for grid with little data but slows after a few thousand cell a lot.
        /// Recommend using implementation such as done with <see cref="includeHotstring"/>.
        /// </remarks>
        private int GridHasError(DataGridView grid)
        {
            if (grid.NewRowIndex <= 0)
            {
                return -1;
            }
            int hasErrorText = -1;
            //replace this.dataGridView1 with the name of your datagridview control
            foreach (DataGridViewRow row in grid.Rows)
            {

              
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.ErrorText))
                    {
                        hasErrorText = cell.RowIndex;
                        break;
                    }
                }
                if (hasErrorText > -1)
                    break;
            }

            return hasErrorText;
        }

        #endregion

        #endregion

        #region Save Related Methods
        /// <summary>
        /// Write the Snippit file value for any hotkey that is include type and does not have a name applied
        /// </summary>
        private void SetHotkeySnippit()
        {
            foreach (var hk in CommandBindingList)
            {
                // if this is an include and the snippit file name as not been provided then add it here
                if (hk.type == commandType.include && string.IsNullOrEmpty(hk.snippit) == true)
                {
                    string SnipptName = plg.name + @"_" + hk.name;
                    if (SnipptName.EndsWith(@".ahk", StringComparison.CurrentCultureIgnoreCase) == false)
                    {
                        SnipptName += @".ahk";
                    }
                    hk.snippit = SnipptName.Replace(' ', '_');
                }
            }
        }

        /// <summary>
        /// Write the snippit file value for any Include that does not yet have a snippit value assigned
        /// </summary>
        private void SetIncludeSnippit()
        {
            foreach (var inc in IncludeBindingList)
            {
                // if this is an include and the snippit file name as not been provided then add it here

                if (string.IsNullOrEmpty(inc.snippit) == true)
                {
                    string SnipptName = plg.name + @"_" + inc.name;
                    if (SnipptName.EndsWith(@".ahk", StringComparison.CurrentCultureIgnoreCase) == false)
                    {
                        SnipptName += @".ahk";
                    }
                    inc.snippit = SnipptName.Replace(' ', '_');
                }
            }
        }

        #endregion

        #region Combo Hotstring Events
        private void cboHsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsInitialized == false)
            {
                return;
            }
            if (cboHsType.SelectedItem != null)
            {

                SortedMapItem sType = (SortedMapItem)cboHsType.SelectedItem;
                var HsType = (hotstringType)Enum.Parse(typeof(hotstringType), sType.Key);
                if (hotstringsBindingSource.Current == null)
                {
                    return;
                }
                var hs = (hotstring)hotstringsBindingSource.Current;
                if (hs.type != HsType)
                {
                    hs.type = HsType;
                }

            }
        }

        private void cboHsType_Validated(object sender, EventArgs e)
        {
            UcHs.BindSendType(); // bind the send type drop down to reflect new options
        }
        #endregion

        #region User Control HotKey
        private void UcHk_ItemChanged(object sender, EventArgs e)
        {
            var currentCmd = (command)commandsBindingSource.Current;
            if (currentCmd != null)
            {
                currentCmd.hotkey = UcHk.HotKey.ToString();
                Debug.WriteLine(UcHk.HotKey.ToReadableString());
                this.UcHk_Validating(UcHk, new CancelEventArgs(false));
            }

        }




        #endregion

        #region Data Import Methods

        private void DataImportText()
        {
            OpenFileDialog fOpen = new OpenFileDialog();
            fOpen.AddExtension = true;
            fOpen.RestoreDirectory = true;
            //LastDataTextImportLocation
            if ((string.IsNullOrEmpty(Properties.Settings.Default.LastDataTextImportLocation) == false)
                && (Directory.Exists(Properties.Settings.Default.LastDataTextImportLocation) == true))
            {
                fOpen.InitialDirectory = Properties.Settings.Default.LastDataTextImportLocation;
            }
            else
            {
                fOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            fOpen.DefaultExt = "txt";
            fOpen.Filter = "TXT|*.txt|ini|*.ini|All Files|*.*";
            if (fOpen.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.LastDataTextImportLocation = Path.GetDirectoryName(fOpen.FileName);
                if (txtDataValue.Text.Length > 0)
                {
                    //ImportDataReplaceValue
                    string msg = string.Format(Properties.Resources.ReplaceValue, lblDataValue.Text);
                    string title = string.Format(Properties.Resources.ReplaceValueTitle, lblDataValue.Text);
                    if (MessageBox.Show(this, msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                dataItem di = (dataItem)dataItemBindingSource.Current;

                if (string.IsNullOrEmpty(di.name))
                {
                    di.name = Path.GetFileNameWithoutExtension(fOpen.FileName);
                }
                if (string.IsNullOrEmpty(di.dataFileName))
                {
                    di.dataFileName = Path.GetFileName(fOpen.FileName);
                }

                di.encoded = false;
                di.dataValue = File.ReadAllText(fOpen.FileName);
                dataItemBindingSource.ResetBindings(false);
                Application.DoEvents();

                // Validate only the affected forms to get any errors to show up
                txtDataName_Validating(txtDataName, new CancelEventArgs());
                txtDataFileName_Validating(txtDataFileName, new CancelEventArgs());
            }
        }

        private void DataImportBinary()
        {
            OpenFileDialog fOpen = new OpenFileDialog();
            fOpen.AddExtension = true;
            fOpen.RestoreDirectory = true;
            if ((string.IsNullOrEmpty(Properties.Settings.Default.LastDataBinaryImportLocation) == false)
                && (Directory.Exists(Properties.Settings.Default.LastDataBinaryImportLocation) == true))
            {
                fOpen.InitialDirectory = Properties.Settings.Default.LastDataBinaryImportLocation;
            }
            else
            {
                fOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            // fOpen.DefaultExt = "txt";
            fOpen.Filter = "All Files|*.*";
            if (fOpen.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.LastDataBinaryImportLocation = Path.GetDirectoryName(fOpen.FileName);
                if (txtDataValue.Text.Length > 0)
                {
                    //ImportDataReplaceValue
                    string msg = string.Format(Properties.Resources.ReplaceValue, lblDataValue.Text);
                    string title = string.Format(Properties.Resources.ReplaceValueTitle, lblDataValue.Text);
                    if (MessageBox.Show(this, msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                dataItem di = (dataItem)dataItemBindingSource.Current;

                if (string.IsNullOrEmpty(di.name))
                {
                    di.name = Path.GetFileNameWithoutExtension(fOpen.FileName);
                }
                if (string.IsNullOrEmpty(di.dataFileName))
                {
                    di.dataFileName = Path.GetFileName(fOpen.FileName);
                }
                di.encoded = true;
                di.dataValue = Util.FileReadToBase64(fOpen.FileName);
                dataItemBindingSource.ResetBindings(false);
                Application.DoEvents();

                // Validate only the affected forms to get any errors to show up
                txtDataName_Validating(txtDataName, new CancelEventArgs());
                txtDataFileName_Validating(txtDataFileName, new CancelEventArgs());
            }
        }





        #endregion

        #region Event General Event Handlers for Textboxes
        private void tBox_Enter(object sender, EventArgs e)
        {
            this.FocusedTextbox = (TextBox)sender;
        }

        private void tBox_Leave(object sender, EventArgs e)
        {
            this.FocusedTextbox = null;
        }
        #endregion

        /// <summary>
        /// Capture Enter and leave for all textboxes that will be acted upon by the edit menus
        /// </summary>
        /// <remarks>
        /// All Edit menus are set to act upon <see cref="FocusedTextbox"/>.
        /// This method ensure that  <see cref="FocusedTextbox"/> will always be the current selected textbox.
        /// </remarks>
        private void SetTextBoxGeneralEvents()
        {
            #region Main
            txtMainName.Enter += tBox_Enter;
            txtMainName.Leave += tBox_Leave;

            txtMainVersion.Enter += tBox_Enter;
            txtMainVersion.Leave += tBox_Leave;
            #endregion

            #region Hotkeys
            txtHkName.Enter += tBox_Enter;
            txtHkName.Leave += tBox_Leave;

            txtHkCategory.Enter += tBox_Enter;
            txtHkCategory.Leave += tBox_Leave;

            txtHkLabel.Enter += tBox_Enter;
            txtHkLabel.Leave += tBox_Leave;

            txtHkDescription.Enter += tBox_Enter;
            txtHkDescription.Leave += tBox_Leave;

            txtHkCode.Enter += tBox_Enter;
            txtHkCode.Leave += tBox_Leave;
            #endregion

            #region HotStrings
            txtHsName.Enter += tBox_Enter;
            txtHsName.Leave += tBox_Leave;

            txtHsDescription.Enter += tBox_Enter;
            txtHsDescription.Leave += tBox_Leave;

            txtHsCategory.Enter += tBox_Enter;
            txtHsCategory.Leave += tBox_Leave;

            txtHsSendKeys.Enter += tBox_Enter;
            txtHsSendKeys.Leave += tBox_Leave;

            txtHsCode.Enter += tBox_Enter;
            txtHsCode.Leave += tBox_Leave;
            #endregion

            #region Includes
            txtIncName.Enter += tBox_Enter;
            txtIncName.Leave += tBox_Leave;

            txtIncDescription.Enter += tBox_Enter;
            txtIncDescription.Leave += tBox_Leave;

            txtIncCode.Enter += tBox_Enter;
            txtIncCode.Leave += tBox_Leave;
            #endregion

            #region Data
            txtDataName.Enter += tBox_Enter;
            txtDataName.Leave += tBox_Leave;

            txtDataFileName.Enter += tBox_Enter;
            txtDataFileName.Leave += tBox_Leave;

            txtDataValue.Enter += tBox_Enter;
            txtDataValue.Leave += tBox_Leave;
            #endregion
        }
        /// <summary>
        /// Sets focus on the current textbox the menu is being display for if the textbox does not already have focus.
        /// </summary>
        /// <param name="sender">ContextMenuStrip</param>
        /// <param name="e">CancelEventArgs</param>
        /// <remarks>
        /// If focus is not set on the textbox before the popup menu is displayed then the popup menu will be acting on any other textbox
        /// that may have focus.
        /// </remarks>
        private void cMnuEdit_Opening(object sender, CancelEventArgs e)
        {
            Control ctl = ((ContextMenuStrip)sender).SourceControl;
            if (ctl.Focused == false)
            {
                ctl.Focus();
            }
        }

        #region Clone Object
        private void CloneHotKey()
        {
            command c = (command)commandsBindingSource.Current;
            if (c == null)
                return;
            command cNew = new command();
            cNew = c.Poplulate(false);
            cNew.enabled = false;
            cNew.name = cNew.name + "1";
            commandsBindingSource.Add(cNew);
            commandsBindingSource.MoveLast();
        }

        private void CloneHotString()
        {
            hotstring hs = (hotstring)hotstringsBindingSource.Current;
            if (hs == null)
                return;
            hotstring hsNew = new hotstring();
            hsNew = hs.Poplulate(false);
            hsNew.enabled = false;
            hsNew.name = hsNew.name + "1";
            hotstringsBindingSource.Add(hsNew);
            hotstringsBindingSource.MoveLast();
        }

        private void CloneInclude()
        {
            include Inc = (include)includeBindingSource.Current;
            if (Inc == null)
                return;
            include IncNew = new include();
            IncNew = Inc.Poplulate(false);
            IncNew.enabled = false;
            IncNew.name = IncNew.name + "1";
            includeBindingSource.Add(IncNew);
            includeBindingSource.MoveLast();
        }

        private void CloneData()
        {
            dataItem di = (dataItem)dataItemBindingSource.Current;
            if (di == null)
                return;
            dataItem diNew = new dataItem();
            diNew = di.Poplulate(false);
            diNew.name = diNew.name + "1";
            diNew.overwrite = false;
            string Ext = Path.GetExtension(diNew.dataFileName);
            string FileName = Path.GetFileNameWithoutExtension(diNew.dataFileName);
            diNew.dataFileName = string.Format("{0}{1}{2}", FileName, "1", Ext);
            dataItemBindingSource.Add(diNew);
            dataItemBindingSource.MoveLast();
        }
        #endregion
    }
}
