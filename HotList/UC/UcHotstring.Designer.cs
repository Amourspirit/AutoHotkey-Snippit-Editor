namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    partial class UcHotstring
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcHotstring));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTriggerInside = new System.Windows.Forms.Label();
            this.lblOmitEndChar = new System.Windows.Forms.Label();
            this.lblAutoBsOff = new System.Windows.Forms.Label();
            this.lblResetReconizer = new System.Windows.Forms.Label();
            this.chkTriggerInside = new System.Windows.Forms.CheckBox();
            this.chkOmitEndChar = new System.Windows.Forms.CheckBox();
            this.chkAutoBsOff = new System.Windows.Forms.CheckBox();
            this.chkResetReconizer = new System.Windows.Forms.CheckBox();
            this.lblSend = new System.Windows.Forms.Label();
            this.ddlSend = new System.Windows.Forms.ComboBox();
            this.lblHotstring = new System.Windows.Forms.Label();
            this.txtHotstring = new System.Windows.Forms.TextBox();
            this.lblCaseSensitive = new System.Windows.Forms.Label();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.lblSendRaw = new System.Windows.Forms.Label();
            this.chkSendRaw = new System.Windows.Forms.CheckBox();
            this.ttHs = new System.Windows.Forms.ToolTip(this.components);
            this.epHs = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epHs)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.lblTriggerInside, 0, 1);
            this.tlpMain.Controls.Add(this.lblOmitEndChar, 0, 2);
            this.tlpMain.Controls.Add(this.lblAutoBsOff, 0, 3);
            this.tlpMain.Controls.Add(this.lblResetReconizer, 3, 1);
            this.tlpMain.Controls.Add(this.chkTriggerInside, 1, 1);
            this.tlpMain.Controls.Add(this.chkOmitEndChar, 1, 2);
            this.tlpMain.Controls.Add(this.chkAutoBsOff, 1, 3);
            this.tlpMain.Controls.Add(this.chkResetReconizer, 4, 1);
            this.tlpMain.Controls.Add(this.lblSend, 3, 2);
            this.tlpMain.Controls.Add(this.ddlSend, 4, 2);
            this.tlpMain.Controls.Add(this.lblHotstring, 3, 3);
            this.tlpMain.Controls.Add(this.txtHotstring, 4, 3);
            this.tlpMain.Controls.Add(this.lblCaseSensitive, 0, 0);
            this.tlpMain.Controls.Add(this.chkCaseSensitive, 1, 0);
            this.tlpMain.Controls.Add(this.lblSendRaw, 3, 0);
            this.tlpMain.Controls.Add(this.chkSendRaw, 4, 0);
            this.tlpMain.Name = "tlpMain";
            // 
            // lblTriggerInside
            // 
            resources.ApplyResources(this.lblTriggerInside, "lblTriggerInside");
            this.lblTriggerInside.Name = "lblTriggerInside";
            // 
            // lblOmitEndChar
            // 
            resources.ApplyResources(this.lblOmitEndChar, "lblOmitEndChar");
            this.lblOmitEndChar.Name = "lblOmitEndChar";
            // 
            // lblAutoBsOff
            // 
            resources.ApplyResources(this.lblAutoBsOff, "lblAutoBsOff");
            this.lblAutoBsOff.Name = "lblAutoBsOff";
            // 
            // lblResetReconizer
            // 
            resources.ApplyResources(this.lblResetReconizer, "lblResetReconizer");
            this.lblResetReconizer.Name = "lblResetReconizer";
            // 
            // chkTriggerInside
            // 
            resources.ApplyResources(this.chkTriggerInside, "chkTriggerInside");
            this.chkTriggerInside.Name = "chkTriggerInside";
            this.ttHs.SetToolTip(this.chkTriggerInside, resources.GetString("chkTriggerInside.ToolTip"));
            this.chkTriggerInside.UseVisualStyleBackColor = true;
            this.chkTriggerInside.CheckedChanged += new System.EventHandler(this.chkTriggerInside_CheckedChanged);
            // 
            // chkOmitEndChar
            // 
            resources.ApplyResources(this.chkOmitEndChar, "chkOmitEndChar");
            this.chkOmitEndChar.Name = "chkOmitEndChar";
            this.ttHs.SetToolTip(this.chkOmitEndChar, resources.GetString("chkOmitEndChar.ToolTip"));
            this.chkOmitEndChar.UseVisualStyleBackColor = true;
            this.chkOmitEndChar.CheckedChanged += new System.EventHandler(this.chkOmitEndChar_CheckedChanged);
            // 
            // chkAutoBsOff
            // 
            resources.ApplyResources(this.chkAutoBsOff, "chkAutoBsOff");
            this.chkAutoBsOff.Name = "chkAutoBsOff";
            this.ttHs.SetToolTip(this.chkAutoBsOff, resources.GetString("chkAutoBsOff.ToolTip"));
            this.chkAutoBsOff.UseVisualStyleBackColor = true;
            this.chkAutoBsOff.CheckedChanged += new System.EventHandler(this.chkAutoBsOff_CheckedChanged);
            // 
            // chkResetReconizer
            // 
            resources.ApplyResources(this.chkResetReconizer, "chkResetReconizer");
            this.chkResetReconizer.Name = "chkResetReconizer";
            this.ttHs.SetToolTip(this.chkResetReconizer, resources.GetString("chkResetReconizer.ToolTip"));
            this.chkResetReconizer.UseVisualStyleBackColor = true;
            this.chkResetReconizer.CheckedChanged += new System.EventHandler(this.chkResetReconizer_CheckedChanged);
            // 
            // lblSend
            // 
            resources.ApplyResources(this.lblSend, "lblSend");
            this.lblSend.Name = "lblSend";
            // 
            // ddlSend
            // 
            resources.ApplyResources(this.ddlSend, "ddlSend");
            this.ddlSend.DisplayMember = "trigger";
            this.ddlSend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSend.FormattingEnabled = true;
            this.ddlSend.Name = "ddlSend";
            this.ttHs.SetToolTip(this.ddlSend, resources.GetString("ddlSend.ToolTip"));
            this.ddlSend.SelectedIndexChanged += new System.EventHandler(this.ddlSend_SelectedIndexChanged);
            // 
            // lblHotstring
            // 
            resources.ApplyResources(this.lblHotstring, "lblHotstring");
            this.lblHotstring.Name = "lblHotstring";
            // 
            // txtHotstring
            // 
            resources.ApplyResources(this.txtHotstring, "txtHotstring");
            this.txtHotstring.Name = "txtHotstring";
            this.ttHs.SetToolTip(this.txtHotstring, resources.GetString("txtHotstring.ToolTip"));
            this.txtHotstring.Validating += new System.ComponentModel.CancelEventHandler(this.txtHotstring_Validating);
            this.txtHotstring.Validated += new System.EventHandler(this.txtHotstring_Validated);
            // 
            // lblCaseSensitive
            // 
            resources.ApplyResources(this.lblCaseSensitive, "lblCaseSensitive");
            this.lblCaseSensitive.Name = "lblCaseSensitive";
            // 
            // chkCaseSensitive
            // 
            resources.ApplyResources(this.chkCaseSensitive, "chkCaseSensitive");
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.ttHs.SetToolTip(this.chkCaseSensitive, resources.GetString("chkCaseSensitive.ToolTip"));
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            this.chkCaseSensitive.CheckedChanged += new System.EventHandler(this.chkCaseSensitive_CheckedChanged);
            // 
            // lblSendRaw
            // 
            resources.ApplyResources(this.lblSendRaw, "lblSendRaw");
            this.lblSendRaw.Name = "lblSendRaw";
            // 
            // chkSendRaw
            // 
            resources.ApplyResources(this.chkSendRaw, "chkSendRaw");
            this.chkSendRaw.Name = "chkSendRaw";
            this.ttHs.SetToolTip(this.chkSendRaw, resources.GetString("chkSendRaw.ToolTip"));
            this.chkSendRaw.UseVisualStyleBackColor = true;
            this.chkSendRaw.CheckedChanged += new System.EventHandler(this.chkSendRaw_CheckedChanged);
            // 
            // ttHs
            // 
            this.ttHs.AutoPopDelay = 15000;
            this.ttHs.InitialDelay = 500;
            this.ttHs.IsBalloon = true;
            this.ttHs.ReshowDelay = 100;
            // 
            // epHs
            // 
            this.epHs.ContainerControl = this;
            // 
            // cmdMgr
            // 
            this.cmdMgr.IdleAction = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.Behavior.None;
            this.cmdMgr.IdleTime = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.TickInterval = System.TimeSpan.Parse("00:00:01");
            this.cmdMgr.WarnSetting = BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData.WarnSettings.Off;
            this.cmdMgr.WarnTime = System.TimeSpan.Parse("00:00:01");
            // 
            // UcHotstring
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "UcHotstring";
            this.Load += new System.EventHandler(this.UcHotstring_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epHs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblCaseSensitive;
        private System.Windows.Forms.Label lblTriggerInside;
        private System.Windows.Forms.Label lblOmitEndChar;
        private System.Windows.Forms.Label lblAutoBsOff;
        private System.Windows.Forms.Label lblSendRaw;
        private System.Windows.Forms.Label lblResetReconizer;
        private System.Windows.Forms.Label lblSend;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.CheckBox chkTriggerInside;
        private System.Windows.Forms.CheckBox chkOmitEndChar;
        private System.Windows.Forms.CheckBox chkAutoBsOff;
        private System.Windows.Forms.CheckBox chkSendRaw;
        private System.Windows.Forms.CheckBox chkResetReconizer;
        private System.Windows.Forms.ComboBox ddlSend;
        private System.Windows.Forms.ToolTip ttHs;
        private System.Windows.Forms.Label lblHotstring;
        private System.Windows.Forms.TextBox txtHotstring;
        private System.Windows.Forms.ErrorProvider epHs;
        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
    }
}
