namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    partial class frmInputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInputDialog));
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnClose = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSave = new System.Windows.Forms.ToolStripButton();
            this.UcInitalValue = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.UcDialogTitle = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.UcPlaceholder = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.UcDialogText = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.hlpProvider = new System.Windows.Forms.HelpProvider();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdMgr
            // 
            this.cmdMgr.IdleAction = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.Behavior.None;
            this.cmdMgr.IdleTime = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.TickInterval = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.WarnSetting = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.WarnSettings.Off;
            this.cmdMgr.WarnTime = System.TimeSpan.Parse("00:00:01");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
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
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnClose,
            this.tsBtnSave});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
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
            // UcInitalValue
            // 
            resources.ApplyResources(this.UcInitalValue, "UcInitalValue");
            this.UcInitalValue.FieldText = "";
            this.UcInitalValue.Label = "Initial Value:";
            this.UcInitalValue.MaxLength = 1024;
            this.UcInitalValue.Name = "UcInitalValue";
            this.UcInitalValue.RegularExpression = "";
            this.UcInitalValue.Required = false;
            this.UcInitalValue.ToolTip = "Optional - Initial Value of the Dialog";
            // 
            // UcDialogTitle
            // 
            resources.ApplyResources(this.UcDialogTitle, "UcDialogTitle");
            this.UcDialogTitle.FieldText = "";
            this.UcDialogTitle.Label = "Dialog Title:";
            this.UcDialogTitle.MaxLength = 256;
            this.UcDialogTitle.Name = "UcDialogTitle";
            this.UcDialogTitle.RegularExpression = "";
            this.UcDialogTitle.ToolTip = "Dialog Title";
            // 
            // UcPlaceholder
            // 
            resources.ApplyResources(this.UcPlaceholder, "UcPlaceholder");
            this.UcPlaceholder.FieldText = "";
            this.UcPlaceholder.Label = "Placeholder:";
            this.UcPlaceholder.MaxLength = 256;
            this.UcPlaceholder.Name = "UcPlaceholder";
            this.UcPlaceholder.RegularExpression = "";
            this.UcPlaceholder.ToolTip = "Placeholder such as {0}";
            // 
            // UcDialogText
            // 
            resources.ApplyResources(this.UcDialogText, "UcDialogText");
            this.UcDialogText.FieldText = "";
            this.UcDialogText.Label = "Dialog Text";
            this.UcDialogText.MaxLength = 1024;
            this.UcDialogText.MultiLine = true;
            this.UcDialogText.Name = "UcDialogText";
            this.UcDialogText.RegularExpression = "";
            // 
            // frmInputDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UcDialogText);
            this.Controls.Add(this.UcInitalValue);
            this.Controls.Add(this.UcDialogTitle);
            this.Controls.Add(this.UcPlaceholder);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.hlpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmInputDialog";
            this.hlpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.Load += new System.EventHandler(this.frmInputFixedList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInputDialog_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripButton tsBtnClose;
        private System.Windows.Forms.ToolStripButton tsBtnSave;
        private UC.UcField UcPlaceholder;
        private UC.UcField UcDialogTitle;
        private UC.UcField UcInitalValue;
        private UC.UcField UcDialogText;
        private System.Windows.Forms.HelpProvider hlpProvider;
    }
}