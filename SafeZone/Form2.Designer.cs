namespace SafeZone
{
    partial class Form2
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
            this.logListView = new BrightIdeasSoftware.ObjectListView();
            this.dateColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.levelColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.loggerColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.messageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.timeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.logListView)).BeginInit();
            this.SuspendLayout();
            // 
            // logListView
            // 
            this.logListView.AllColumns.Add(this.dateColumn);
            this.logListView.AllColumns.Add(this.timeColumn);
            this.logListView.AllColumns.Add(this.levelColumn);
            this.logListView.AllColumns.Add(this.loggerColumn);
            this.logListView.AllColumns.Add(this.messageColumn);
            this.logListView.CellEditUseWholeCell = false;
            this.logListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dateColumn,
            this.timeColumn,
            this.levelColumn,
            this.loggerColumn,
            this.messageColumn});
            this.logListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.logListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListView.FullRowSelect = true;
            this.logListView.Location = new System.Drawing.Point(0, 0);
            this.logListView.MultiSelect = false;
            this.logListView.Name = "logListView";
            this.logListView.Size = new System.Drawing.Size(975, 394);
            this.logListView.TabIndex = 0;
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // dateColumn
            // 
            this.dateColumn.AspectName = "date";
            this.dateColumn.Text = "Tarih";
            this.dateColumn.Width = 90;
            // 
            // levelColumn
            // 
            this.levelColumn.AspectName = "level";
            this.levelColumn.Text = "Seviye";
            // 
            // loggerColumn
            // 
            this.loggerColumn.AspectName = "logger";
            this.loggerColumn.Text = "Kayıtçı";
            this.loggerColumn.Width = 150;
            // 
            // messageColumn
            // 
            this.messageColumn.AspectName = "message";
            this.messageColumn.Text = "Mesaj";
            this.messageColumn.Width = 580;
            // 
            // timeColumn
            // 
            this.timeColumn.AspectName = "time";
            this.timeColumn.Text = "Saat";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 394);
            this.Controls.Add(this.logListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log";
            ((System.ComponentModel.ISupportInitialize)(this.logListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView logListView;
        private BrightIdeasSoftware.OLVColumn dateColumn;
        private BrightIdeasSoftware.OLVColumn levelColumn;
        private BrightIdeasSoftware.OLVColumn loggerColumn;
        private BrightIdeasSoftware.OLVColumn messageColumn;
        private BrightIdeasSoftware.OLVColumn timeColumn;
    }
}