namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    partial class UcField
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
            this.lblName = new System.Windows.Forms.Label();
            this.TbField = new System.Windows.Forms.TextBox();
            this.Ttip = new System.Windows.Forms.ToolTip(this.components);
            this.EpMain = new System.Windows.Forms.ErrorProvider(this.components);
            this.tlpField = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.EpMain)).BeginInit();
            this.tlpField.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Margin = new System.Windows.Forms.Padding(3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(122, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // TbField
            // 
            this.TbField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbField.Location = new System.Drawing.Point(131, 3);
            this.TbField.Name = "TbField";
            this.TbField.Size = new System.Drawing.Size(186, 20);
            this.TbField.TabIndex = 1;
            this.TbField.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.TbField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            this.TbField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            this.TbField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyUp);
            this.TbField.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
            this.TbField.Validated += new System.EventHandler(this.txtName_Validated);
            // 
            // Ttip
            // 
            this.Ttip.AutoPopDelay = 15000;
            this.Ttip.InitialDelay = 500;
            this.Ttip.IsBalloon = true;
            this.Ttip.ReshowDelay = 100;
            // 
            // EpMain
            // 
            this.EpMain.ContainerControl = this;
            // 
            // tlpField
            // 
            this.tlpField.ColumnCount = 3;
            this.tlpField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpField.Controls.Add(this.lblName, 0, 0);
            this.tlpField.Controls.Add(this.TbField, 1, 0);
            this.tlpField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpField.Location = new System.Drawing.Point(0, 0);
            this.tlpField.Name = "tlpField";
            this.tlpField.RowCount = 1;
            this.tlpField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpField.Size = new System.Drawing.Size(340, 24);
            this.tlpField.TabIndex = 2;
            // 
            // UcField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpField);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UcField";
            this.Size = new System.Drawing.Size(340, 24);
            this.Load += new System.EventHandler(this.UcField_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EpMain)).EndInit();
            this.tlpField.ResumeLayout(false);
            this.tlpField.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ToolTip Ttip;
        private System.Windows.Forms.ErrorProvider EpMain;
        protected internal System.Windows.Forms.TextBox TbField;
        private System.Windows.Forms.TableLayoutPanel tlpField;
    }
}
