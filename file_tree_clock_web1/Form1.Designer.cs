namespace file_tree_clock_web1
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && ( components != null )) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Form1 ) );
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.timeNow = new System.Windows.Forms.Label();
			this.secTimer = new System.Windows.Forms.Timer( this.components );
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.fileTree = new System.Windows.Forms.TreeView();
			this.fileTreeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.フォルダ作成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名称変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.カットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.コピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ペーストToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.元に戻す = new System.Windows.Forms.ToolStripMenuItem();
			this.他のアプリケーションで開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileinfo = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lastWriteTime = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.fileLength = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.creationTime = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lastAccessTime = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.rExtension = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList( this.components );
			this.passNameLabel = new System.Windows.Forms.Label();
			this.fileNameLabel = new System.Windows.Forms.Label();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainerLeftTop = new System.Windows.Forms.SplitContainer();
			this.splitContainerCenter = new System.Windows.Forms.SplitContainer();
			this.mineType = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.typeName = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.fileTreeContextMenuStrip.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainer1 ) ).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainerLeftTop ) ).BeginInit();
			this.splitContainerLeftTop.Panel1.SuspendLayout();
			this.splitContainerLeftTop.Panel2.SuspendLayout();
			this.splitContainerLeftTop.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainerCenter ) ).BeginInit();
			this.splitContainerCenter.Panel1.SuspendLayout();
			this.splitContainerCenter.Panel2.SuspendLayout();
			this.splitContainerCenter.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point( 5, 102 );
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size( 400, 40 );
			this.listBox1.TabIndex = 2;
			this.listBox1.SelectedIndexChanged += new System.EventHandler( this.listBox1_SelectedIndexChanged );
			// 
			// timeNow
			// 
			this.timeNow.AutoSize = true;
			this.timeNow.Font = new System.Drawing.Font( "MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 128 ) ) );
			this.timeNow.Location = new System.Drawing.Point( 161, 76 );
			this.timeNow.Name = "timeNow";
			this.timeNow.Size = new System.Drawing.Size( 109, 16 );
			this.timeNow.TabIndex = 4;
			this.timeNow.Text = "12時34分 56秒";
			this.timeNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.timeNow.UseMnemonic = false;
			// 
			// secTimer
			// 
			this.secTimer.Enabled = true;
			this.secTimer.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// comboBox1
			// 
			this.comboBox1.Font = new System.Drawing.Font( "MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 128 ) ) );
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point( 89, 73 );
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.comboBox1.Size = new System.Drawing.Size( 61, 23 );
			this.comboBox1.TabIndex = 5;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler( this.comboBox1_SelectedIndexChanged );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font( "MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 128 ) ) );
			this.label1.Location = new System.Drawing.Point( 3, 76 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 80, 15 );
			this.label1.TabIndex = 6;
			this.label1.Text = "ドライブ選択";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// fileTree
			// 
			this.fileTree.AllowDrop = true;
			this.fileTree.ContextMenuStrip = this.fileTreeContextMenuStrip;
			this.fileTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileTree.Location = new System.Drawing.Point( 0, 0 );
			this.fileTree.Name = "fileTree";
			this.fileTree.Size = new System.Drawing.Size( 410, 275 );
			this.fileTree.TabIndex = 7;
			this.fileTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler( this.TreeView1_ItemDrag );
			this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.treeView1_AfterSelect );
			this.fileTree.DragDrop += new System.Windows.Forms.DragEventHandler( this.TreeView1_DragDrop );
			this.fileTree.DragOver += new System.Windows.Forms.DragEventHandler( this.TreeView1_DragOver );
			// 
			// fileTreeContextMenuStrip
			// 
			this.fileTreeContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
			this.フォルダ作成ToolStripMenuItem,
			this.名称変更ToolStripMenuItem,
			this.カットToolStripMenuItem,
			this.コピーToolStripMenuItem,
			this.ペーストToolStripMenuItem,
			this.削除ToolStripMenuItem,
			this.元に戻す,
			this.他のアプリケーションで開くToolStripMenuItem} );
			this.fileTreeContextMenuStrip.Name = "contextMenuStrip1";
			this.fileTreeContextMenuStrip.Size = new System.Drawing.Size( 195, 180 );
			this.fileTreeContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler( this.contextMenuStrip1_ItemClicked );
			// 
			// フォルダ作成ToolStripMenuItem
			// 
			this.フォルダ作成ToolStripMenuItem.Name = "フォルダ作成ToolStripMenuItem";
			this.フォルダ作成ToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.フォルダ作成ToolStripMenuItem.Text = "フォルダ作成";
			// 
			// 名称変更ToolStripMenuItem
			// 
			this.名称変更ToolStripMenuItem.Name = "名称変更ToolStripMenuItem";
			this.名称変更ToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.名称変更ToolStripMenuItem.Text = "名称変更";
			// 
			// カットToolStripMenuItem
			// 
			this.カットToolStripMenuItem.Name = "カットToolStripMenuItem";
			this.カットToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.カットToolStripMenuItem.Text = "カット";
			// 
			// コピーToolStripMenuItem
			// 
			this.コピーToolStripMenuItem.Name = "コピーToolStripMenuItem";
			this.コピーToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.コピーToolStripMenuItem.Text = "コピー";
			// 
			// ペーストToolStripMenuItem
			// 
			this.ペーストToolStripMenuItem.Name = "ペーストToolStripMenuItem";
			this.ペーストToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.ペーストToolStripMenuItem.Text = "ペースト";
			// 
			// 削除ToolStripMenuItem
			// 
			this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
			this.削除ToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.削除ToolStripMenuItem.Text = "削除";
			// 
			// 元に戻す
			// 
			this.元に戻す.Name = "元に戻す";
			this.元に戻す.Size = new System.Drawing.Size( 194, 22 );
			this.元に戻す.Text = "元に戻す";
			this.元に戻す.Visible = false;
			// 
			// 他のアプリケーションで開くToolStripMenuItem
			// 
			this.他のアプリケーションで開くToolStripMenuItem.Name = "他のアプリケーションで開くToolStripMenuItem";
			this.他のアプリケーションで開くToolStripMenuItem.Size = new System.Drawing.Size( 194, 22 );
			this.他のアプリケーションで開くToolStripMenuItem.Text = "他のアプリケーションで開く";
			// 
			// fileinfo
			// 
			this.fileinfo.Location = new System.Drawing.Point( 3, 145 );
			this.fileinfo.MaximumSize = new System.Drawing.Size( 100, 30 );
			this.fileinfo.MinimumSize = new System.Drawing.Size( 100, 30 );
			this.fileinfo.Name = "fileinfo";
			this.fileinfo.Size = new System.Drawing.Size( 100, 30 );
			this.fileinfo.TabIndex = 8;
			this.fileinfo.Text = "ファイル情報";
			this.fileinfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 3, 11 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 41, 12 );
			this.label2.TabIndex = 11;
			this.label2.Text = "更新日";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lastWriteTime
			// 
			this.lastWriteTime.AutoSize = true;
			this.lastWriteTime.Location = new System.Drawing.Point( 50, 11 );
			this.lastWriteTime.Name = "lastWriteTime";
			this.lastWriteTime.Size = new System.Drawing.Size( 119, 12 );
			this.lastWriteTime.TabIndex = 12;
			this.lastWriteTime.Text = "2999年12月31日 23:59";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 176, 11 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 68, 12 );
			this.label3.TabIndex = 13;
			this.label3.Text = "ファイルサイズ";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// fileLength
			// 
			this.fileLength.AutoSize = true;
			this.fileLength.Location = new System.Drawing.Point( 256, 11 );
			this.fileLength.Name = "fileLength";
			this.fileLength.Size = new System.Drawing.Size( 95, 12 );
			this.fileLength.TabIndex = 14;
			this.fileLength.Text = "999999999999999";
			this.fileLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 4, 32 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 41, 12 );
			this.label4.TabIndex = 15;
			this.label4.Text = "作成日";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// creationTime
			// 
			this.creationTime.AutoSize = true;
			this.creationTime.Location = new System.Drawing.Point( 47, 32 );
			this.creationTime.Name = "creationTime";
			this.creationTime.Size = new System.Drawing.Size( 119, 12 );
			this.creationTime.TabIndex = 16;
			this.creationTime.Text = "2999年12月31日 23:59";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point( 176, 32 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 65, 12 );
			this.label5.TabIndex = 17;
			this.label5.Text = "最終アクセス";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lastAccessTime
			// 
			this.lastAccessTime.AutoSize = true;
			this.lastAccessTime.Location = new System.Drawing.Point( 243, 32 );
			this.lastAccessTime.Name = "lastAccessTime";
			this.lastAccessTime.Size = new System.Drawing.Size( 119, 12 );
			this.lastAccessTime.TabIndex = 18;
			this.lastAccessTime.Text = "2999年12月31日 23:59";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point( 3, 54 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 41, 12 );
			this.label6.TabIndex = 19;
			this.label6.Text = "拡張子";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// rExtension
			// 
			this.rExtension.AutoSize = true;
			this.rExtension.Location = new System.Drawing.Point( 50, 54 );
			this.rExtension.Name = "rExtension";
			this.rExtension.Size = new System.Drawing.Size( 25, 12 );
			this.rExtension.TabIndex = 20;
			this.rExtension.Text = ".xxx";
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ( ( System.Windows.Forms.ImageListStreamer ) ( resources.GetObject( "imageList1.ImageStream" ) ) );
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName( 0, "hd_icon.png" );
			this.imageList1.Images.SetKeyName( 1, "folder_close_icon.png" );
			this.imageList1.Images.SetKeyName( 2, "docment_icon.png" );
			this.imageList1.Images.SetKeyName( 3, "move_icon.png" );
			this.imageList1.Images.SetKeyName( 4, "pict_icon.png" );
			this.imageList1.Images.SetKeyName( 5, "music_icon.png" );
			this.imageList1.Images.SetKeyName( 6, "desktop_icon.png" );
			this.imageList1.Images.SetKeyName( 7, "pc_icon.png" );
			this.imageList1.Images.SetKeyName( 8, "folder_close_icon.png" );
			this.imageList1.Images.SetKeyName( 9, "phone_icon.png" );
			this.imageList1.Images.SetKeyName( 10, "hd_sys_icon.png" );
			// 
			// passNameLabel
			// 
			this.passNameLabel.AutoSize = true;
			this.passNameLabel.Location = new System.Drawing.Point( 5, 5 );
			this.passNameLabel.Name = "passNameLabel";
			this.passNameLabel.Size = new System.Drawing.Size( 0, 12 );
			this.passNameLabel.TabIndex = 21;
			// 
			// fileNameLabel
			// 
			this.fileNameLabel.AutoSize = true;
			this.fileNameLabel.Location = new System.Drawing.Point( 5, 25 );
			this.fileNameLabel.Name = "fileNameLabel";
			this.fileNameLabel.Size = new System.Drawing.Size( 41, 12 );
			this.fileNameLabel.TabIndex = 22;
			this.fileNameLabel.Text = "未選択";
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point( 0, 0 );
			this.webBrowser1.MinimumSize = new System.Drawing.Size( 20, 20 );
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size( 625, 505 );
			this.webBrowser1.TabIndex = 25;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.splitContainerLeftTop );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AutoScroll = true;
			this.splitContainer1.Panel2.Controls.Add( this.webBrowser1 );
			this.splitContainer1.Size = new System.Drawing.Size( 1039, 505 );
			this.splitContainer1.SplitterDistance = 410;
			this.splitContainer1.TabIndex = 27;
			// 
			// splitContainerLeftTop
			// 
			this.splitContainerLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerLeftTop.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainerLeftTop.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainerLeftTop.Name = "splitContainerLeftTop";
			this.splitContainerLeftTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerLeftTop.Panel1
			// 
			this.splitContainerLeftTop.Panel1.Controls.Add( this.fileNameLabel );
			this.splitContainerLeftTop.Panel1.Controls.Add( this.passNameLabel );
			// 
			// splitContainerLeftTop.Panel2
			// 
			this.splitContainerLeftTop.Panel2.Controls.Add( this.splitContainerCenter );
			this.splitContainerLeftTop.Size = new System.Drawing.Size( 410, 505 );
			this.splitContainerLeftTop.SplitterDistance = 42;
			this.splitContainerLeftTop.TabIndex = 0;
			// 
			// splitContainerCenter
			// 
			this.splitContainerCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerCenter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainerCenter.IsSplitterFixed = true;
			this.splitContainerCenter.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainerCenter.Name = "splitContainerCenter";
			this.splitContainerCenter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerCenter.Panel1
			// 
			this.splitContainerCenter.Panel1.AutoScroll = true;
			this.splitContainerCenter.Panel1.Controls.Add( this.fileTree );
			// 
			// splitContainerCenter.Panel2
			// 
			this.splitContainerCenter.Panel2.Controls.Add( this.mineType );
			this.splitContainerCenter.Panel2.Controls.Add( this.label8 );
			this.splitContainerCenter.Panel2.Controls.Add( this.typeName );
			this.splitContainerCenter.Panel2.Controls.Add( this.label7 );
			this.splitContainerCenter.Panel2.Controls.Add( this.lastAccessTime );
			this.splitContainerCenter.Panel2.Controls.Add( this.rExtension );
			this.splitContainerCenter.Panel2.Controls.Add( this.label5 );
			this.splitContainerCenter.Panel2.Controls.Add( this.listBox1 );
			this.splitContainerCenter.Panel2.Controls.Add( this.fileLength );
			this.splitContainerCenter.Panel2.Controls.Add( this.creationTime );
			this.splitContainerCenter.Panel2.Controls.Add( this.label3 );
			this.splitContainerCenter.Panel2.Controls.Add( this.label6 );
			this.splitContainerCenter.Panel2.Controls.Add( this.lastWriteTime );
			this.splitContainerCenter.Panel2.Controls.Add( this.label4 );
			this.splitContainerCenter.Panel2.Controls.Add( this.label2 );
			this.splitContainerCenter.Panel2.Controls.Add( this.timeNow );
			this.splitContainerCenter.Panel2.Controls.Add( this.fileinfo );
			this.splitContainerCenter.Panel2.Controls.Add( this.comboBox1 );
			this.splitContainerCenter.Panel2.Controls.Add( this.label1 );
			this.splitContainerCenter.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
			this.splitContainerCenter.Size = new System.Drawing.Size( 410, 459 );
			this.splitContainerCenter.SplitterDistance = 275;
			this.splitContainerCenter.TabIndex = 0;
			// 
			// mineType
			// 
			this.mineType.AutoSize = true;
			this.mineType.Location = new System.Drawing.Point( 229, 53 );
			this.mineType.Name = "mineType";
			this.mineType.Size = new System.Drawing.Size( 62, 12 );
			this.mineType.TabIndex = 24;
			this.mineType.Text = "MIME Type";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point( 189, 53 );
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size( 33, 12 );
			this.label8.TabIndex = 23;
			this.label8.Text = "MIME";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// typeName
			// 
			this.typeName.AutoSize = true;
			this.typeName.Location = new System.Drawing.Point( 125, 53 );
			this.typeName.Name = "typeName";
			this.typeName.Size = new System.Drawing.Size( 60, 12 );
			this.typeName.TabIndex = 22;
			this.typeName.Text = "application";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point( 89, 53 );
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size( 29, 12 );
			this.label7.TabIndex = 21;
			this.label7.Text = "分類";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size( 1039, 505 );
			this.Controls.Add( this.splitContainer1 );
			this.MinimumSize = new System.Drawing.Size( 1055, 39 );
			this.Name = "Form1";
			this.Text = "After watching the contents(中身を見てから)";
			this.Load += new System.EventHandler( this.Form1_Load );
			this.fileTreeContextMenuStrip.ResumeLayout( false );
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainer1 ) ).EndInit();
			this.splitContainer1.ResumeLayout( false );
			this.splitContainerLeftTop.Panel1.ResumeLayout( false );
			this.splitContainerLeftTop.Panel1.PerformLayout();
			this.splitContainerLeftTop.Panel2.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainerLeftTop ) ).EndInit();
			this.splitContainerLeftTop.ResumeLayout( false );
			this.splitContainerCenter.Panel1.ResumeLayout( false );
			this.splitContainerCenter.Panel2.ResumeLayout( false );
			this.splitContainerCenter.Panel2.PerformLayout();
			( ( System.ComponentModel.ISupportInitialize ) ( this.splitContainerCenter ) ).EndInit();
			this.splitContainerCenter.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label timeNow;
		private System.Windows.Forms.Timer secTimer;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView fileTree;
		private System.Windows.Forms.Label fileinfo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lastWriteTime;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label fileLength;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label creationTime;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lastAccessTime;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label rExtension;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label passNameLabel;
		private System.Windows.Forms.Label fileNameLabel;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainerLeftTop;
		private System.Windows.Forms.SplitContainer splitContainerCenter;
		private System.Windows.Forms.Label mineType;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label typeName;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ContextMenuStrip fileTreeContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem 削除ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem フォルダ作成ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 他のアプリケーションで開くToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 元に戻す;
		private System.Windows.Forms.ToolStripMenuItem コピーToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ペーストToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem カットToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 名称変更ToolStripMenuItem;
	}
}

