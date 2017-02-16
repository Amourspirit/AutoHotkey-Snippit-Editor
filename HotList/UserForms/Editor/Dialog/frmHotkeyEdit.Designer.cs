namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    partial class frmHotkeyEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHotkeyEdit));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.UcHotkeyMain = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcHotkey();
            this.hlpProvider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmdMgr
            // 
            this.cmdMgr.IdleAction = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.Behavior.None;
            this.cmdMgr.IdleTime = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.TickInterval = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.WarnSetting = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.WarnSettings.Off;
            this.cmdMgr.WarnTime = System.TimeSpan.Parse("00:00:01");
            // 
            // UcHotkeyMain
            // 
            this.hlpProvider.SetHelpKeyword(this.UcHotkeyMain, resources.GetString("UcHotkeyMain.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this.UcHotkeyMain, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("UcHotkeyMain.HelpNavigator"))));
            this.hlpProvider.SetHelpString(this.UcHotkeyMain, resources.GetString("UcHotkeyMain.HelpString"));
            resources.ApplyResources(this.UcHotkeyMain, "UcHotkeyMain");
            this.UcHotkeyMain.Name = "UcHotkeyMain";
            this.hlpProvider.SetShowHelp(this.UcHotkeyMain, ((bool)(resources.GetObject("UcHotkeyMain.ShowHelp"))));
            // 
            // frmHotkeyEdit
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.UcHotkeyMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.hlpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHotkeyEdit";
            this.hlpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.frmHotkeyEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UC.UcHotkey UcHotkeyMain;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
        private System.Windows.Forms.HelpProvider hlpProvider;
    }
}