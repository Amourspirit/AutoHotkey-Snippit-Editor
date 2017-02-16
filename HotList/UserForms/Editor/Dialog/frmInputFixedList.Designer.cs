namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    partial class frmInputFixedList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInputFixedList));
            this.cmdMgr = new BigByteTechnologies.Library.Windows.CommandManagement.CommandManager();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnClose = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSave = new System.Windows.Forms.ToolStripButton();
            this.listValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgList = new System.Windows.Forms.DataGridView();
            this.hlpProvider = new System.Windows.Forms.HelpProvider();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DelCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.UcDialogText = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.UcDialogTitle = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.UcPlaceholder = new BigByteTechnologies.Windows.AHKSnipit.HotList.UC.UcField();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listValuesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).BeginInit();
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
            this.hlpProvider.SetHelpKeyword(this.menuStrip1, resources.GetString("menuStrip1.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this.menuStrip1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("menuStrip1.HelpNavigator"))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.hlpProvider.SetShowHelp(this.menuStrip1, ((bool)(resources.GetObject("menuStrip1.ShowHelp"))));
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
            this.hlpProvider.SetShowHelp(this.toolStrip1, ((bool)(resources.GetObject("toolStrip1.ShowHelp"))));
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
            // listValuesBindingSource
            // 
            this.listValuesBindingSource.DataSource = typeof(BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.itemType);
            // 
            // dgList
            // 
            resources.ApplyResources(this.dgList, "dgList");
            this.dgList.AutoGenerateColumns = false;
            this.dgList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.DelCol});
            this.dgList.DataSource = this.listValuesBindingSource;
            this.hlpProvider.SetHelpKeyword(this.dgList, resources.GetString("dgList.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this.dgList, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("dgList.HelpNavigator"))));
            this.dgList.Name = "dgList";
            this.hlpProvider.SetShowHelp(this.dgList, ((bool)(resources.GetObject("dgList.ShowHelp"))));
            this.dgList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgList_CellClick);
            this.dgList.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgList_DefaultValuesNeeded);
            this.dgList.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgList_UserDeletingRow);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "defaultSpecified";
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Value";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // DelCol
            // 
            this.DelCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.DelCol, "DelCol");
            this.DelCol.Name = "DelCol";
            this.DelCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DelCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UcDialogText
            // 
            resources.ApplyResources(this.UcDialogText, "UcDialogText");
            this.UcDialogText.FieldText = "";
            this.UcDialogText.Label = "Dialog Text:";
            this.UcDialogText.MaxLength = 1024;
            this.UcDialogText.MultiLine = true;
            this.UcDialogText.Name = "UcDialogText";
            this.UcDialogText.RegularExpression = "";
            this.hlpProvider.SetShowHelp(this.UcDialogText, ((bool)(resources.GetObject("UcDialogText.ShowHelp"))));
            // 
            // UcDialogTitle
            // 
            this.UcDialogTitle.FieldText = "";
            this.UcDialogTitle.Label = "Dialog Title:";
            resources.ApplyResources(this.UcDialogTitle, "UcDialogTitle");
            this.UcDialogTitle.MaxLength = 256;
            this.UcDialogTitle.Name = "UcDialogTitle";
            this.UcDialogTitle.RegularExpression = "";
            this.hlpProvider.SetShowHelp(this.UcDialogTitle, ((bool)(resources.GetObject("UcDialogTitle.ShowHelp"))));
            this.UcDialogTitle.ToolTip = "Dialog Title";
            // 
            // UcPlaceholder
            // 
            this.UcPlaceholder.FieldText = "";
            this.UcPlaceholder.Label = "Placeholder:";
            resources.ApplyResources(this.UcPlaceholder, "UcPlaceholder");
            this.UcPlaceholder.MaxLength = 256;
            this.UcPlaceholder.Name = "UcPlaceholder";
            this.UcPlaceholder.RegularExpression = "";
            this.hlpProvider.SetShowHelp(this.UcPlaceholder, ((bool)(resources.GetObject("UcPlaceholder.ShowHelp"))));
            this.UcPlaceholder.ToolTip = "Placeholder such as {0}";
            // 
            // frmInputFixedList
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UcDialogText);
            this.Controls.Add(this.UcDialogTitle);
            this.Controls.Add(this.UcPlaceholder);
            this.Controls.Add(this.dgList);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.hlpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.hlpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmInputFixedList";
            this.hlpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.Load += new System.EventHandler(this.frmInputFixedList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInputFixedList_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listValuesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BigByteTechnologies.Library.Windows.CommandManagement.CommandManager cmdMgr;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.BindingSource listValuesBindingSource;
        private System.Windows.Forms.DataGridView dgList;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripButton tsBtnClose;
        private System.Windows.Forms.ToolStripButton tsBtnSave;
        private UC.UcField UcPlaceholder;
        private UC.UcField UcDialogTitle;
        private UC.UcField UcDialogText;
        private System.Windows.Forms.HelpProvider hlpProvider;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewImageColumn DelCol;
    }
}