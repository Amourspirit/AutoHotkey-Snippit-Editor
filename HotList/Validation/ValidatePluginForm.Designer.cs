namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Validation
{
    partial class ValidatePluginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidatePluginForm));
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuValidate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblResults = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.ofdPlugin = new System.Windows.Forms.OpenFileDialog();
            this.hlpProvider = new System.Windows.Forms.HelpProvider();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.mnuFile,
            this.mnuEdit});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuValidate,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            resources.ApplyResources(this.mnuFile, "mnuFile");
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            resources.ApplyResources(this.mnuOpen, "mnuOpen");
            // 
            // mnuValidate
            // 
            this.mnuValidate.Name = "mnuValidate";
            resources.ApplyResources(this.mnuValidate, "mnuValidate");
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            resources.ApplyResources(this.mnuExit, "mnuExit");
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopy,
            this.mnuSelectAll});
            this.mnuEdit.Name = "mnuEdit";
            resources.ApplyResources(this.mnuEdit, "mnuEdit");
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            resources.ApplyResources(this.mnuCopy, "mnuCopy");
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            resources.ApplyResources(this.mnuSelectAll, "mnuSelectAll");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblResults);
            this.panel1.Controls.Add(this.txtResult);
            this.panel1.Controls.Add(this.btnFile);
            this.panel1.Controls.Add(this.txtFile);
            this.panel1.Controls.Add(this.lblFile);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lblResults
            // 
            resources.ApplyResources(this.lblResults, "lblResults");
            this.lblResults.Name = "lblResults";
            // 
            // txtResult
            // 
            resources.ApplyResources(this.txtResult, "txtResult");
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            // 
            // btnFile
            // 
            resources.ApplyResources(this.btnFile, "btnFile");
            this.btnFile.Name = "btnFile";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // txtFile
            // 
            resources.ApplyResources(this.txtFile, "txtFile");
            this.txtFile.Name = "txtFile";
            // 
            // lblFile
            // 
            resources.ApplyResources(this.lblFile, "lblFile");
            this.lblFile.Name = "lblFile";
            // 
            // ofdPlugin
            // 
            this.ofdPlugin.DefaultExt = "xml";
            resources.ApplyResources(this.ofdPlugin, "ofdPlugin");
            // 
            // ValidatePluginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.hlpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ValidatePluginForm";
            this.hlpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ValidatePluginForm_FormClosing);
            this.Load += new System.EventHandler(this.ValidatePluginForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.OpenFileDialog ofdPlugin;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuValidate;
        private System.Windows.Forms.HelpProvider hlpProvider;
    }
}