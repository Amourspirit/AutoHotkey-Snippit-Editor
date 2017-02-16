namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    partial class UcFileName
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
            this.EpMain = new System.Windows.Forms.ErrorProvider(this.components);
            this.TtMain = new System.Windows.Forms.ToolTip(this.components);
            this.tlpFileName = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.EpMain)).BeginInit();
            this.tlpFileName.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(122, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // TbField
            // 
            this.TbField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TbField.Location = new System.Drawing.Point(131, 3);
            this.TbField.Name = "TbField";
            this.TbField.Size = new System.Drawing.Size(186, 20);
            this.TbField.TabIndex = 1;
            this.TbField.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            this.TbField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFileName_KeyDown);
            this.TbField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFileName_KeyPress);
            this.TbField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFileName_KeyUp);
            this.TbField.Validating += new System.ComponentModel.CancelEventHandler(this.txtFileName_Validating);
            this.TbField.Validated += new System.EventHandler(this.txtFileName_Validated);
            // 
            // EpMain
            // 
            this.EpMain.ContainerControl = this;
            // 
            // TtMain
            // 
            this.TtMain.AutoPopDelay = 15000;
            this.TtMain.InitialDelay = 500;
            this.TtMain.IsBalloon = true;
            this.TtMain.ReshowDelay = 100;
            // 
            // tlpFileName
            // 
            this.tlpFileName.ColumnCount = 3;
            this.tlpFileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpFileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpFileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFileName.Controls.Add(this.lblName, 0, 0);
            this.tlpFileName.Controls.Add(this.TbField, 1, 0);
            this.tlpFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFileName.Location = new System.Drawing.Point(0, 0);
            this.tlpFileName.Name = "tlpFileName";
            this.tlpFileName.RowCount = 1;
            this.tlpFileName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFileName.Size = new System.Drawing.Size(340, 24);
            this.tlpFileName.TabIndex = 2;
            // 
            // UcFileName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFileName);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UcFileName";
            this.Size = new System.Drawing.Size(340, 24);
            this.Load += new System.EventHandler(this.UcFileName_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EpMain)).EndInit();
            this.tlpFileName.ResumeLayout(false);
            this.tlpFileName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ErrorProvider EpMain;
        private System.Windows.Forms.ToolTip TtMain;
        protected internal System.Windows.Forms.TextBox TbField;
        private System.Windows.Forms.TableLayoutPanel tlpFileName;
    }
}
