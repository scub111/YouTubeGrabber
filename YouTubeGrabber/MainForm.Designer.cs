using RapidInterface;

namespace YouTubeGrabber
{
    partial class MainForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sprbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.slblCaption = new System.Windows.Forms.ToolStripStatusLabel();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.teDownloadPath = new DevExpress.XtraEditors.TextEdit();
            this.gridBase = new RapidInterface.GridControlEx(this.components);
            this.viewBase = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.columnIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnReference = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnPriority = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnProgressPercentage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repProgressPercentage = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.btnAnalyse = new DevExpress.XtraEditors.SimpleButton();
            this.tdInputUri = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tmrInterfaceUpdate = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teDownloadPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProgressPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tdInputUri.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sprbProgress,
            this.slblCaption});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(734, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sprbProgress
            // 
            this.sprbProgress.Name = "sprbProgress";
            this.sprbProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // slblCaption
            // 
            this.slblCaption.Name = "slblCaption";
            this.slblCaption.Size = new System.Drawing.Size(0, 17);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnDownload);
            this.layoutControl1.Controls.Add(this.teDownloadPath);
            this.layoutControl1.Controls.Add(this.gridBase);
            this.layoutControl1.Controls.Add(this.memoEdit1);
            this.layoutControl1.Controls.Add(this.btnAnalyse);
            this.layoutControl1.Controls.Add(this.tdInputUri);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(734, 544);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(593, 38);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(129, 22);
            this.btnDownload.StyleController = this.layoutControl1;
            this.btnDownload.TabIndex = 9;
            this.btnDownload.Text = "Download";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // teDownloadPath
            // 
            this.teDownloadPath.EditValue = "C:\\Downloads\\";
            this.teDownloadPath.Location = new System.Drawing.Point(41, 38);
            this.teDownloadPath.Name = "teDownloadPath";
            this.teDownloadPath.Size = new System.Drawing.Size(548, 20);
            this.teDownloadPath.StyleController = this.layoutControl1;
            this.teDownloadPath.TabIndex = 8;
            // 
            // gridBase
            // 
            this.gridBase.Location = new System.Drawing.Point(24, 98);
            this.gridBase.MainView = this.viewBase;
            this.gridBase.Name = "gridBase";
            this.gridBase.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repProgressPercentage});
            this.gridBase.ShowOnlyPredefinedDetails = true;
            this.gridBase.Size = new System.Drawing.Size(686, 422);
            this.gridBase.TabIndex = 7;
            this.gridBase.UseEmbeddedNavigator = true;
            this.gridBase.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewBase});
            // 
            // viewBase
            // 
            this.viewBase.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.columnIndex,
            this.columnTitle,
            this.columnReference,
            this.columnPriority,
            this.columnProgressPercentage});
            this.viewBase.GridControl = this.gridBase;
            this.viewBase.Name = "viewBase";
            this.viewBase.OptionsFind.AlwaysVisible = true;
            this.viewBase.OptionsSelection.MultiSelect = true;
            this.viewBase.OptionsView.ShowAutoFilterRow = true;
            this.viewBase.OptionsView.ShowGroupPanel = false;
            this.viewBase.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.viewBase_PopupMenuShowing);
            // 
            // columnIndex
            // 
            this.columnIndex.Caption = "Index";
            this.columnIndex.FieldName = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.Visible = true;
            this.columnIndex.VisibleIndex = 0;
            this.columnIndex.Width = 20;
            // 
            // columnTitle
            // 
            this.columnTitle.Caption = "Title";
            this.columnTitle.FieldName = "Title";
            this.columnTitle.Name = "columnTitle";
            this.columnTitle.Visible = true;
            this.columnTitle.VisibleIndex = 1;
            this.columnTitle.Width = 100;
            // 
            // columnReference
            // 
            this.columnReference.Caption = "Reference";
            this.columnReference.FieldName = "Reference";
            this.columnReference.Name = "columnReference";
            this.columnReference.Visible = true;
            this.columnReference.VisibleIndex = 2;
            this.columnReference.Width = 150;
            // 
            // columnPriority
            // 
            this.columnPriority.Caption = "Priority";
            this.columnPriority.FieldName = "Priority";
            this.columnPriority.Name = "columnPriority";
            this.columnPriority.Visible = true;
            this.columnPriority.VisibleIndex = 3;
            this.columnPriority.Width = 40;
            // 
            // columnProgressPercentage
            // 
            this.columnProgressPercentage.Caption = "Progress";
            this.columnProgressPercentage.ColumnEdit = this.repProgressPercentage;
            this.columnProgressPercentage.FieldName = "ProgressPercentage";
            this.columnProgressPercentage.Name = "columnProgressPercentage";
            this.columnProgressPercentage.Visible = true;
            this.columnProgressPercentage.VisibleIndex = 4;
            // 
            // repProgressPercentage
            // 
            this.repProgressPercentage.DisplayFormat.FormatString = "{0)";
            this.repProgressPercentage.EndColor = System.Drawing.Color.Green;
            this.repProgressPercentage.Name = "repProgressPercentage";
            this.repProgressPercentage.StartColor = System.Drawing.Color.Green;
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(24, 98);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(686, 422);
            this.memoEdit1.StyleController = this.layoutControl1;
            this.memoEdit1.TabIndex = 6;
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Location = new System.Drawing.Point(593, 12);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(129, 22);
            this.btnAnalyse.StyleController = this.layoutControl1;
            this.btnAnalyse.TabIndex = 5;
            this.btnAnalyse.Text = "Analyse";
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // tdInputUri
            // 
            this.tdInputUri.EditValue = "https://www.youtube.com/watch?v=c1NCzfo2_jo&list=PL08903FB7ACA1C2FB&index=132";
            this.tdInputUri.Location = new System.Drawing.Point(41, 12);
            this.tdInputUri.Name = "tdInputUri";
            this.tdInputUri.Size = new System.Drawing.Size(548, 20);
            this.tdInputUri.StyleController = this.layoutControl1;
            this.tdInputUri.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.tabbedControlGroup1,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(734, 544);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.tdInputUri;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(581, 26);
            this.layoutControlItem1.Text = "Uri:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(26, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnAnalyse;
            this.layoutControlItem2.Location = new System.Drawing.Point(581, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(133, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 52);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.layoutControlGroup3;
            this.tabbedControlGroup1.SelectedTabPageIndex = 1;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(714, 472);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3});
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(690, 426);
            this.layoutControlGroup3.Text = "Data";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gridBase;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(690, 426);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(690, 426);
            this.layoutControlGroup2.Text = "HTML";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.memoEdit1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(690, 426);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.teDownloadPath;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(581, 26);
            this.layoutControlItem5.Text = "Path:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(26, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnDownload;
            this.layoutControlItem6.Location = new System.Drawing.Point(581, 26);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(133, 26);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // tmrInterfaceUpdate
            // 
            this.tmrInterfaceUpdate.Enabled = true;
            this.tmrInterfaceUpdate.Interval = 1000;
            this.tmrInterfaceUpdate.Tick += new System.EventHandler(this.tmrInterfaceUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 566);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YouTube Grabber";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teDownloadPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProgressPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tdInputUri.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar sprbProgress;
        private System.Windows.Forms.ToolStripStatusLabel slblCaption;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.SimpleButton btnAnalyse;
        private DevExpress.XtraEditors.TextEdit tdInputUri;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private GridControlEx gridBase;
        private DevExpress.XtraGrid.Views.Grid.GridView viewBase;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.Columns.GridColumn columnIndex;
        private DevExpress.XtraGrid.Columns.GridColumn columnTitle;
        private DevExpress.XtraGrid.Columns.GridColumn columnReference;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.TextEdit teDownloadPath;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn columnProgressPercentage;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repProgressPercentage;
        private System.Windows.Forms.Timer tmrInterfaceUpdate;
        private DevExpress.XtraGrid.Columns.GridColumn columnPriority;
    }
}

