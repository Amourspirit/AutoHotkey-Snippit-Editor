namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    partial class UcHotkey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcHotkey));
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkWin = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.tblLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblWin = new System.Windows.Forms.Label();
            this.lblShift = new System.Windows.Forms.Label();
            this.lblAlt = new System.Windows.Forms.Label();
            this.ddlKeys2 = new System.Windows.Forms.ComboBox();
            this.lblKey2 = new System.Windows.Forms.Label();
            this.lblKey1 = new System.Windows.Forms.Label();
            this.ddlKeys1 = new System.Windows.Forms.ComboBox();
            this.chkLeftMod = new System.Windows.Forms.CheckBox();
            this.lblLeftMod = new System.Windows.Forms.Label();
            this.chkRightMod = new System.Windows.Forms.CheckBox();
            this.lblRightMod = new System.Windows.Forms.Label();
            this.chkWildcard = new System.Windows.Forms.CheckBox();
            this.lblWildcard = new System.Windows.Forms.Label();
            this.chkDisableNative = new System.Windows.Forms.CheckBox();
            this.lblDisableNative = new System.Windows.Forms.Label();
            this.chkInstallHook = new System.Windows.Forms.CheckBox();
            this.lblInstallHook = new System.Windows.Forms.Label();
            this.chkUp = new System.Windows.Forms.CheckBox();
            this.lblUp = new System.Windows.Forms.Label();
            this.lblCtrl = new System.Windows.Forms.Label();
            this.tblLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShift
            // 
            resources.ApplyResources(this.chkShift, "chkShift");
            this.chkShift.Name = "chkShift";
            this.chkShift.UseVisualStyleBackColor = true;
            this.chkShift.CheckedChanged += new System.EventHandler(this.chkShift_CheckedChanged);
            // 
            // chkCtrl
            // 
            resources.ApplyResources(this.chkCtrl, "chkCtrl");
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.UseVisualStyleBackColor = true;
            this.chkCtrl.CheckedChanged += new System.EventHandler(this.chkCtrl_CheckedChanged);
            // 
            // chkWin
            // 
            resources.ApplyResources(this.chkWin, "chkWin");
            this.chkWin.Name = "chkWin";
            this.chkWin.UseVisualStyleBackColor = true;
            this.chkWin.CheckedChanged += new System.EventHandler(this.chkWin_CheckedChanged);
            // 
            // chkAlt
            // 
            resources.ApplyResources(this.chkAlt, "chkAlt");
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.UseVisualStyleBackColor = true;
            this.chkAlt.CheckedChanged += new System.EventHandler(this.chkAlt_CheckedChanged);
            this.chkAlt.Validated += new System.EventHandler(this.chkAlt_Validated);
            // 
            // tblLayoutMain
            // 
            resources.ApplyResources(this.tblLayoutMain, "tblLayoutMain");
            this.tblLayoutMain.Controls.Add(this.lblWin, 0, 3);
            this.tblLayoutMain.Controls.Add(this.lblShift, 0, 2);
            this.tblLayoutMain.Controls.Add(this.lblAlt, 0, 1);
            this.tblLayoutMain.Controls.Add(this.ddlKeys2, 1, 5);
            this.tblLayoutMain.Controls.Add(this.lblKey2, 0, 5);
            this.tblLayoutMain.Controls.Add(this.lblKey1, 0, 4);
            this.tblLayoutMain.Controls.Add(this.ddlKeys1, 1, 4);
            this.tblLayoutMain.Controls.Add(this.chkCtrl, 1, 0);
            this.tblLayoutMain.Controls.Add(this.chkAlt, 1, 1);
            this.tblLayoutMain.Controls.Add(this.chkShift, 1, 2);
            this.tblLayoutMain.Controls.Add(this.chkWin, 1, 3);
            this.tblLayoutMain.Controls.Add(this.chkLeftMod, 3, 0);
            this.tblLayoutMain.Controls.Add(this.lblLeftMod, 2, 0);
            this.tblLayoutMain.Controls.Add(this.chkRightMod, 3, 1);
            this.tblLayoutMain.Controls.Add(this.lblRightMod, 2, 1);
            this.tblLayoutMain.Controls.Add(this.chkWildcard, 3, 2);
            this.tblLayoutMain.Controls.Add(this.lblWildcard, 2, 2);
            this.tblLayoutMain.Controls.Add(this.chkDisableNative, 3, 3);
            this.tblLayoutMain.Controls.Add(this.lblDisableNative, 2, 3);
            this.tblLayoutMain.Controls.Add(this.chkInstallHook, 3, 4);
            this.tblLayoutMain.Controls.Add(this.lblInstallHook, 2, 4);
            this.tblLayoutMain.Controls.Add(this.chkUp, 3, 5);
            this.tblLayoutMain.Controls.Add(this.lblUp, 2, 5);
            this.tblLayoutMain.Controls.Add(this.lblCtrl, 0, 0);
            this.tblLayoutMain.Name = "tblLayoutMain";
            // 
            // lblWin
            // 
            resources.ApplyResources(this.lblWin, "lblWin");
            this.lblWin.CausesValidation = false;
            this.lblWin.Name = "lblWin";
            // 
            // lblShift
            // 
            resources.ApplyResources(this.lblShift, "lblShift");
            this.lblShift.CausesValidation = false;
            this.lblShift.Name = "lblShift";
            // 
            // lblAlt
            // 
            resources.ApplyResources(this.lblAlt, "lblAlt");
            this.lblAlt.CausesValidation = false;
            this.lblAlt.Name = "lblAlt";
            // 
            // ddlKeys2
            // 
            resources.ApplyResources(this.ddlKeys2, "ddlKeys2");
            this.ddlKeys2.FormattingEnabled = true;
            this.ddlKeys2.Name = "ddlKeys2";
            this.ddlKeys2.SelectedIndexChanged += new System.EventHandler(this.ddlKeys2_SelectedIndexChanged);
            // 
            // lblKey2
            // 
            resources.ApplyResources(this.lblKey2, "lblKey2");
            this.lblKey2.CausesValidation = false;
            this.lblKey2.Name = "lblKey2";
            // 
            // lblKey1
            // 
            resources.ApplyResources(this.lblKey1, "lblKey1");
            this.lblKey1.CausesValidation = false;
            this.lblKey1.Name = "lblKey1";
            // 
            // ddlKeys1
            // 
            resources.ApplyResources(this.ddlKeys1, "ddlKeys1");
            this.ddlKeys1.FormattingEnabled = true;
            this.ddlKeys1.Name = "ddlKeys1";
            this.ddlKeys1.SelectedIndexChanged += new System.EventHandler(this.ddlKeys1_SelectedIndexChanged);
            // 
            // chkLeftMod
            // 
            resources.ApplyResources(this.chkLeftMod, "chkLeftMod");
            this.chkLeftMod.Name = "chkLeftMod";
            this.chkLeftMod.UseVisualStyleBackColor = true;
            this.chkLeftMod.CheckedChanged += new System.EventHandler(this.chkLeftMod_CheckedChanged);
            // 
            // lblLeftMod
            // 
            resources.ApplyResources(this.lblLeftMod, "lblLeftMod");
            this.lblLeftMod.CausesValidation = false;
            this.lblLeftMod.Name = "lblLeftMod";
            // 
            // chkRightMod
            // 
            resources.ApplyResources(this.chkRightMod, "chkRightMod");
            this.chkRightMod.Name = "chkRightMod";
            this.chkRightMod.UseVisualStyleBackColor = true;
            this.chkRightMod.CheckedChanged += new System.EventHandler(this.chkRightMod_CheckedChanged);
            // 
            // lblRightMod
            // 
            resources.ApplyResources(this.lblRightMod, "lblRightMod");
            this.lblRightMod.CausesValidation = false;
            this.lblRightMod.Name = "lblRightMod";
            // 
            // chkWildcard
            // 
            resources.ApplyResources(this.chkWildcard, "chkWildcard");
            this.chkWildcard.Name = "chkWildcard";
            this.chkWildcard.UseVisualStyleBackColor = true;
            this.chkWildcard.CheckedChanged += new System.EventHandler(this.chkWildcard_CheckedChanged);
            // 
            // lblWildcard
            // 
            resources.ApplyResources(this.lblWildcard, "lblWildcard");
            this.lblWildcard.CausesValidation = false;
            this.lblWildcard.Name = "lblWildcard";
            // 
            // chkDisableNative
            // 
            resources.ApplyResources(this.chkDisableNative, "chkDisableNative");
            this.chkDisableNative.Name = "chkDisableNative";
            this.chkDisableNative.UseVisualStyleBackColor = true;
            this.chkDisableNative.CheckedChanged += new System.EventHandler(this.chkDisableNative_CheckedChanged);
            // 
            // lblDisableNative
            // 
            resources.ApplyResources(this.lblDisableNative, "lblDisableNative");
            this.lblDisableNative.CausesValidation = false;
            this.lblDisableNative.Name = "lblDisableNative";
            // 
            // chkInstallHook
            // 
            resources.ApplyResources(this.chkInstallHook, "chkInstallHook");
            this.chkInstallHook.Name = "chkInstallHook";
            this.chkInstallHook.UseVisualStyleBackColor = true;
            this.chkInstallHook.CheckedChanged += new System.EventHandler(this.chkInstallHook_CheckedChanged);
            // 
            // lblInstallHook
            // 
            resources.ApplyResources(this.lblInstallHook, "lblInstallHook");
            this.lblInstallHook.CausesValidation = false;
            this.lblInstallHook.Name = "lblInstallHook";
            // 
            // chkUp
            // 
            resources.ApplyResources(this.chkUp, "chkUp");
            this.chkUp.Name = "chkUp";
            this.chkUp.UseVisualStyleBackColor = true;
            this.chkUp.CheckedChanged += new System.EventHandler(this.chkUp_CheckedChanged);
            // 
            // lblUp
            // 
            resources.ApplyResources(this.lblUp, "lblUp");
            this.lblUp.Name = "lblUp";
            // 
            // lblCtrl
            // 
            resources.ApplyResources(this.lblCtrl, "lblCtrl");
            this.lblCtrl.CausesValidation = false;
            this.lblCtrl.Name = "lblCtrl";
            // 
            // UcHotkey
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblLayoutMain);
            this.Name = "UcHotkey";
            this.Load += new System.EventHandler(this.UcHotkey_Load);
            this.tblLayoutMain.ResumeLayout(false);
            this.tblLayoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tblLayoutMain;
        protected internal System.Windows.Forms.CheckBox chkShift;
        protected internal System.Windows.Forms.CheckBox chkCtrl;
        protected internal System.Windows.Forms.CheckBox chkWin;
        protected internal System.Windows.Forms.CheckBox chkAlt;
        protected internal System.Windows.Forms.Label lblKey1;
        protected internal System.Windows.Forms.ComboBox ddlKeys1;
        protected internal System.Windows.Forms.Label lblKey2;
        protected internal System.Windows.Forms.ComboBox ddlKeys2;
        protected internal System.Windows.Forms.CheckBox chkLeftMod;
        private System.Windows.Forms.Label lblLeftMod;
        protected internal System.Windows.Forms.CheckBox chkRightMod;
        private System.Windows.Forms.Label lblRightMod;
        protected internal System.Windows.Forms.CheckBox chkWildcard;
        private System.Windows.Forms.Label lblWildcard;
        protected internal System.Windows.Forms.CheckBox chkDisableNative;
        private System.Windows.Forms.Label lblDisableNative;
        protected internal System.Windows.Forms.CheckBox chkInstallHook;
        private System.Windows.Forms.Label lblInstallHook;
        protected internal System.Windows.Forms.CheckBox chkUp;
        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label lblAlt;
        private System.Windows.Forms.Label lblCtrl;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.Label lblWin;
    }
}
