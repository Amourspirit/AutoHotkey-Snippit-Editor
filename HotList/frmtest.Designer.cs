namespace BigByteTechnologies.Windows.AHKSnipit.HotList
{
    partial class frmtest
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
            this.ucHotstring1 = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcHotstring();
            this.SuspendLayout();
            // 
            // ucHotstring1
            // 
            this.ucHotstring1.Location = new System.Drawing.Point(34, 27);
            this.ucHotstring1.Name = "ucHotstring1";
            this.ucHotstring1.Size = new System.Drawing.Size(367, 99);
            this.ucHotstring1.TabIndex = 0;
            // 
            // frmtest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(558, 261);
            this.Controls.Add(this.ucHotstring1);
            this.Name = "frmtest";
            this.Text = "frmtest";
            this.ResumeLayout(false);

        }

        #endregion

        private UC.UcHotstring ucHotstring1;
    }
}