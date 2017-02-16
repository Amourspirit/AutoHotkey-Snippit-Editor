namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    partial class UcVersion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcVersion));
            this.lblVersion = new System.Windows.Forms.Label();
            this.TbField = new System.Windows.Forms.TextBox();
            this.ttVersion = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tlpVersion = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tlpVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // TbField
            // 
            resources.ApplyResources(this.TbField, "TbField");
            this.TbField.Name = "TbField";
            this.ttVersion.SetToolTip(this.TbField, resources.GetString("TbField.ToolTip"));
            this.TbField.Validating += new System.ComponentModel.CancelEventHandler(this.txtVersion_Validating);
            this.TbField.Validated += new System.EventHandler(this.txtVersion_Validated);
            // 
            // ttVersion
            // 
            this.ttVersion.AutoPopDelay = 15000;
            this.ttVersion.InitialDelay = 500;
            this.ttVersion.IsBalloon = true;
            this.ttVersion.ReshowDelay = 100;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tlpVersion
            // 
            resources.ApplyResources(this.tlpVersion, "tlpVersion");
            this.tlpVersion.Controls.Add(this.lblVersion, 0, 0);
            this.tlpVersion.Controls.Add(this.TbField, 1, 0);
            this.tlpVersion.Name = "tlpVersion";
            // 
            // UcVersion
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpVersion);
            this.Name = "UcVersion";
            this.Load += new System.EventHandler(this.UcVersion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tlpVersion.ResumeLayout(false);
            this.tlpVersion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ToolTip ttVersion;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        protected internal System.Windows.Forms.TextBox TbField;
        private System.Windows.Forms.TableLayoutPanel tlpVersion;
    }
}
