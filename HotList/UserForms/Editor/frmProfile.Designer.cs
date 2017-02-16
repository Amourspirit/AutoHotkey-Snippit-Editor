namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor
{
    partial class frmProfile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfile));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowSpy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowSpyLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnClose = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSave = new System.Windows.Forms.ToolStripButton();
            this.tsbWindowSpy = new System.Windows.Forms.ToolStripButton();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.chkGlobal = new System.Windows.Forms.CheckBox();
            this.lblGlobalProfile = new System.Windows.Forms.Label();
            this.gbEndChars = new System.Windows.Forms.GroupBox();
            this.clbEndChars = new System.Windows.Forms.CheckedListBox();
            this.tabWindows = new System.Windows.Forms.TabPage();
            this.dgWindows = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWindow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.windowsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.ttMain = new System.Windows.Forms.ToolTip(this.components);
            this.bsEndChars = new System.Windows.Forms.BindingSource(this.components);
            this.hlpProviderProfile = new System.Windows.Forms.HelpProvider();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cMnuEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMnuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.cMnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cMnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMnuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cMnuEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.cMnuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ProfileName = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcFileName();
            this.ProfileVersion = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcVersion();
            this.ProfileDisplayName = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.ProfileDisplayDescription = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.gbEndChars.SuspendLayout();
            this.tabWindows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWindows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEndChars)).BeginInit();
            this.cMnuEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuEdit,
            this.toolsToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.hlpProviderProfile.SetShowHelp(this.menuStrip1, ((bool)(resources.GetObject("menuStrip1.ShowHelp"))));
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuClose});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // mnuSave
            // 
            this.mnuSave.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.save_16;
            this.mnuSave.Name = "mnuSave";
            resources.ApplyResources(this.mnuSave, "mnuSave");
            // 
            // mnuClose
            // 
            this.mnuClose.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.exit_16;
            this.mnuClose.Name = "mnuClose";
            resources.ApplyResources(this.mnuClose, "mnuClose");
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWindowSpy,
            this.mnuWindowSpyLocation});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // mnuWindowSpy
            // 
            this.mnuWindowSpy.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.windowspy;
            this.mnuWindowSpy.Name = "mnuWindowSpy";
            resources.ApplyResources(this.mnuWindowSpy, "mnuWindowSpy");
            // 
            // mnuWindowSpyLocation
            // 
            this.mnuWindowSpyLocation.Name = "mnuWindowSpyLocation";
            resources.ApplyResources(this.mnuWindowSpyLocation, "mnuWindowSpyLocation");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnClose,
            this.tsBtnSave,
            this.tsbWindowSpy});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.hlpProviderProfile.SetShowHelp(this.toolStrip1, ((bool)(resources.GetObject("toolStrip1.ShowHelp"))));
            // 
            // tsBtnClose
            // 
            this.tsBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnClose.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.exit_16;
            resources.ApplyResources(this.tsBtnClose, "tsBtnClose");
            this.tsBtnClose.Name = "tsBtnClose";
            // 
            // tsBtnSave
            // 
            this.tsBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnSave.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.save_16;
            resources.ApplyResources(this.tsBtnSave, "tsBtnSave");
            this.tsBtnSave.Name = "tsBtnSave";
            // 
            // tsbWindowSpy
            // 
            this.tsbWindowSpy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWindowSpy.Image = global::BigByteTechnologies.Windows.AHKSnipit.HotList.Properties.Resources.windowspy;
            resources.ApplyResources(this.tsbWindowSpy, "tsbWindowSpy");
            this.tsbWindowSpy.Name = "tsbWindowSpy";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabMain);
            this.tcMain.Controls.Add(this.tabWindows);
            resources.ApplyResources(this.tcMain, "tcMain");
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.hlpProviderProfile.SetShowHelp(this.tcMain, ((bool)(resources.GetObject("tcMain.ShowHelp"))));
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tlpMain);
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Name = "tabMain";
            this.hlpProviderProfile.SetShowHelp(this.tabMain, ((bool)(resources.GetObject("tabMain.ShowHelp"))));
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.ProfileName, 0, 0);
            this.tlpMain.Controls.Add(this.chkGlobal, 1, 4);
            this.tlpMain.Controls.Add(this.ProfileVersion, 0, 1);
            this.tlpMain.Controls.Add(this.lblGlobalProfile, 0, 4);
            this.tlpMain.Controls.Add(this.ProfileDisplayName, 0, 2);
            this.tlpMain.Controls.Add(this.ProfileDisplayDescription, 0, 3);
            this.tlpMain.Controls.Add(this.gbEndChars, 3, 0);
            this.tlpMain.Name = "tlpMain";
            this.hlpProviderProfile.SetShowHelp(this.tlpMain, ((bool)(resources.GetObject("tlpMain.ShowHelp"))));
            // 
            // chkGlobal
            // 
            resources.ApplyResources(this.chkGlobal, "chkGlobal");
            this.chkGlobal.Name = "chkGlobal";
            this.hlpProviderProfile.SetShowHelp(this.chkGlobal, ((bool)(resources.GetObject("chkGlobal.ShowHelp"))));
            this.ttMain.SetToolTip(this.chkGlobal, resources.GetString("chkGlobal.ToolTip"));
            this.chkGlobal.UseVisualStyleBackColor = true;
            // 
            // lblGlobalProfile
            // 
            resources.ApplyResources(this.lblGlobalProfile, "lblGlobalProfile");
            this.lblGlobalProfile.Name = "lblGlobalProfile";
            this.hlpProviderProfile.SetShowHelp(this.lblGlobalProfile, ((bool)(resources.GetObject("lblGlobalProfile.ShowHelp"))));
            // 
            // gbEndChars
            // 
            this.gbEndChars.Controls.Add(this.clbEndChars);
            resources.ApplyResources(this.gbEndChars, "gbEndChars");
            this.gbEndChars.Name = "gbEndChars";
            this.tlpMain.SetRowSpan(this.gbEndChars, 5);
            this.hlpProviderProfile.SetShowHelp(this.gbEndChars, ((bool)(resources.GetObject("gbEndChars.ShowHelp"))));
            this.gbEndChars.TabStop = false;
            // 
            // clbEndChars
            // 
            resources.ApplyResources(this.clbEndChars, "clbEndChars");
            this.clbEndChars.FormattingEnabled = true;
            this.clbEndChars.Name = "clbEndChars";
            this.hlpProviderProfile.SetShowHelp(this.clbEndChars, ((bool)(resources.GetObject("clbEndChars.ShowHelp"))));
            this.clbEndChars.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbEndChars_ItemCheck);
            // 
            // tabWindows
            // 
            this.tabWindows.Controls.Add(this.dgWindows);
            resources.ApplyResources(this.tabWindows, "tabWindows");
            this.tabWindows.Name = "tabWindows";
            this.hlpProviderProfile.SetShowHelp(this.tabWindows, ((bool)(resources.GetObject("tabWindows.ShowHelp"))));
            this.tabWindows.UseVisualStyleBackColor = true;
            // 
            // dgWindows
            // 
            this.dgWindows.AutoGenerateColumns = false;
            this.dgWindows.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgWindows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWindows.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColWindow});
            this.dgWindows.DataSource = this.windowsBindingSource;
            resources.ApplyResources(this.dgWindows, "dgWindows");
            this.dgWindows.Name = "dgWindows";
            this.hlpProviderProfile.SetShowHelp(this.dgWindows, ((bool)(resources.GetObject("dgWindows.ShowHelp"))));
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "name";
            resources.ApplyResources(this.ColName, "ColName");
            this.ColName.MaxInputLength = 256;
            this.ColName.Name = "ColName";
            // 
            // ColWindow
            // 
            this.ColWindow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColWindow.DataPropertyName = "value";
            resources.ApplyResources(this.ColWindow, "ColWindow");
            this.ColWindow.MaxInputLength = 1024;
            this.ColWindow.Name = "ColWindow";
            // 
            // cmdMgr
            // 
            this.cmdMgr.IdleAction = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.Behavior.None;
            this.cmdMgr.IdleTime = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.TickInterval = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.WarnSetting = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.WarnSettings.Off;
            this.cmdMgr.WarnTime = System.TimeSpan.Parse("00:00:01");
            // 
            // ttMain
            // 
            this.ttMain.AutoPopDelay = 15000;
            this.ttMain.InitialDelay = 500;
            this.ttMain.IsBalloon = true;
            this.ttMain.ReshowDelay = 100;
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditUndo,
            this.toolStripMenuItem4,
            this.mnuEditCut,
            this.mnuEditCopy,
            this.mnuEditPaste,
            this.mnuEditDelete,
            this.toolStripMenuItem3,
            this.mnuEditSelectAll});
            this.mnuEdit.Name = "mnuEdit";
            resources.ApplyResources(this.mnuEdit, "mnuEdit");
            // 
            // mnuEditUndo
            // 
            this.mnuEditUndo.Name = "mnuEditUndo";
            resources.ApplyResources(this.mnuEditUndo, "mnuEditUndo");
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Name = "mnuEditCut";
            resources.ApplyResources(this.mnuEditCut, "mnuEditCut");
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Name = "mnuEditCopy";
            resources.ApplyResources(this.mnuEditCopy, "mnuEditCopy");
            // 
            // mnuEditPaste
            // 
            this.mnuEditPaste.Name = "mnuEditPaste";
            resources.ApplyResources(this.mnuEditPaste, "mnuEditPaste");
            // 
            // mnuEditDelete
            // 
            this.mnuEditDelete.Name = "mnuEditDelete";
            resources.ApplyResources(this.mnuEditDelete, "mnuEditDelete");
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            // 
            // mnuEditSelectAll
            // 
            this.mnuEditSelectAll.Name = "mnuEditSelectAll";
            resources.ApplyResources(this.mnuEditSelectAll, "mnuEditSelectAll");
            // 
            // cMnuEdit
            // 
            this.cMnuEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMnuEditUndo,
            this.toolStripMenuItem5,
            this.cMnuEditCut,
            this.cMnuEditCopy,
            this.cMnuEditPaste,
            this.cMnuEditDelete,
            this.toolStripMenuItem6,
            this.cMnuEditSelectAll});
            this.cMnuEdit.Name = "cMnuEdit";
            this.hlpProviderProfile.SetShowHelp(this.cMnuEdit, ((bool)(resources.GetObject("cMnuEdit.ShowHelp"))));
            resources.ApplyResources(this.cMnuEdit, "cMnuEdit");
            this.cMnuEdit.Opening += new System.ComponentModel.CancelEventHandler(this.cMnuEdit_Opening);
            // 
            // cMnuEditUndo
            // 
            this.cMnuEditUndo.Name = "cMnuEditUndo";
            resources.ApplyResources(this.cMnuEditUndo, "cMnuEditUndo");
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            // 
            // cMnuEditCut
            // 
            this.cMnuEditCut.Name = "cMnuEditCut";
            resources.ApplyResources(this.cMnuEditCut, "cMnuEditCut");
            // 
            // cMnuEditCopy
            // 
            this.cMnuEditCopy.Name = "cMnuEditCopy";
            resources.ApplyResources(this.cMnuEditCopy, "cMnuEditCopy");
            // 
            // cMnuEditPaste
            // 
            this.cMnuEditPaste.Name = "cMnuEditPaste";
            resources.ApplyResources(this.cMnuEditPaste, "cMnuEditPaste");
            // 
            // cMnuEditDelete
            // 
            this.cMnuEditDelete.Name = "cMnuEditDelete";
            resources.ApplyResources(this.cMnuEditDelete, "cMnuEditDelete");
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            // 
            // cMnuEditSelectAll
            // 
            this.cMnuEditSelectAll.Name = "cMnuEditSelectAll";
            resources.ApplyResources(this.cMnuEditSelectAll, "cMnuEditSelectAll");
            // 
            // ProfileName
            // 
            this.tlpMain.SetColumnSpan(this.ProfileName, 3);
            resources.ApplyResources(this.ProfileName, "ProfileName");
            this.ProfileName.FileName = "";
            this.ProfileName.Label = "*Name:";
            this.ProfileName.Name = "ProfileName";
            this.hlpProviderProfile.SetShowHelp(this.ProfileName, ((bool)(resources.GetObject("ProfileName.ShowHelp"))));
            this.ProfileName.ToolTip = "";
            // 
            // ProfileVersion
            // 
            this.tlpMain.SetColumnSpan(this.ProfileVersion, 3);
            resources.ApplyResources(this.ProfileVersion, "ProfileVersion");
            this.ProfileVersion.Label = "*Version:";
            this.ProfileVersion.Name = "ProfileVersion";
            this.hlpProviderProfile.SetShowHelp(this.ProfileVersion, ((bool)(resources.GetObject("ProfileVersion.ShowHelp"))));
            this.ProfileVersion.ToolTip = "";
            // 
            // ProfileDisplayName
            // 
            this.tlpMain.SetColumnSpan(this.ProfileDisplayName, 3);
            resources.ApplyResources(this.ProfileDisplayName, "ProfileDisplayName");
            this.ProfileDisplayName.FieldText = "";
            this.ProfileDisplayName.Label = "*Display Name:";
            this.ProfileDisplayName.MaxLength = 256;
            this.ProfileDisplayName.Name = "ProfileDisplayName";
            this.ProfileDisplayName.RegularExpression = "";
            this.hlpProviderProfile.SetShowHelp(this.ProfileDisplayName, ((bool)(resources.GetObject("ProfileDisplayName.ShowHelp"))));
            this.ProfileDisplayName.ToolTip = "";
            // 
            // ProfileDisplayDescription
            // 
            this.tlpMain.SetColumnSpan(this.ProfileDisplayDescription, 3);
            resources.ApplyResources(this.ProfileDisplayDescription, "ProfileDisplayDescription");
            this.ProfileDisplayDescription.FieldText = "";
            this.ProfileDisplayDescription.Label = "Description:";
            this.ProfileDisplayDescription.Name = "ProfileDisplayDescription";
            this.ProfileDisplayDescription.RegularExpression = "";
            this.ProfileDisplayDescription.Required = false;
            this.hlpProviderProfile.SetShowHelp(this.ProfileDisplayDescription, ((bool)(resources.GetObject("ProfileDisplayDescription.ShowHelp"))));
            this.ProfileDisplayDescription.ToolTip = "";
            // 
            // frmProfile
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.hlpProviderProfile.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.hlpProviderProfile.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmProfile";
            this.hlpProviderProfile.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProfile_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProfile_FormClosed);
            this.Load += new System.EventHandler(this.frmProfile_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.gbEndChars.ResumeLayout(false);
            this.tabWindows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgWindows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEndChars)).EndInit();
            this.cMnuEdit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnClose;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabMain;
        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
        private UC.UcVersion ProfileVersion;
        private UC.UcFileName ProfileName;
        private UC.UcField ProfileDisplayDescription;
        private UC.UcField ProfileDisplayName;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripButton tsBtnSave;
        private System.Windows.Forms.TabPage tabWindows;
        private System.Windows.Forms.DataGridView dgWindows;
        private System.Windows.Forms.DataGridViewTextBoxColumn WindowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.BindingSource windowsBindingSource;
        private System.Windows.Forms.CheckBox chkGlobal;
        private System.Windows.Forms.Label lblGlobalProfile;
        private System.Windows.Forms.ToolTip ttMain;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbEndChars;
        private System.Windows.Forms.CheckedListBox clbEndChars;
        private System.Windows.Forms.BindingSource bsEndChars;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowSpy;
        private System.Windows.Forms.ToolStripButton tsbWindowSpy;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowSpyLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWindow;
        private System.Windows.Forms.HelpProvider hlpProviderProfile;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuEditUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCut;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSelectAll;
        private System.Windows.Forms.ContextMenuStrip cMnuEdit;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditCut;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem cMnuEditSelectAll;
    }
}