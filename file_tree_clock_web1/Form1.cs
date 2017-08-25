using Microsoft.Win32;          ///WebBrowserコントロールを配置すると、IEのバージョン 7をIE11の Edgeモードに変更///
using System;
using System.Text.RegularExpressions;         ///WebBrowserコントロールを配置すると、IEのバージョン 7をIE11の Edgeモードに変更///
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;    // 参照設定に追加を忘れずに
							//using System.Object;
							//using System.MarshalByRefObject;
							//using System.ComponentModel.Component;
							//using System.Management.ManagementBaseObject;
							//using System.Management.ManagementObject;
using Microsoft.VisualBasic.FileIO; //DelFiles,MoveFolderのFileSystem
using System.Diagnostics;

namespace file_tree_clock_web1
{
	public partial class Form1 : Form
	{
		Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( FEATURE_BROWSER_EMULATION );
		const string FEATURE_BROWSER_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
		string process_name = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
		string process_dbg_name = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".vshost.exe";

		string[] systemFiles = new string[] { "RECYCLE", ".bak", ".bdmv", ".blf", ".BIN", ".cab",  ".cfg",  ".cmd",".css",  ".dat",".dll",
												".inf",  ".inf", ".ini", ".lsi", ".iso",  ".lst", ".jar",  ".log", ".lock",".mis",
												".mni",".MARKER",  ".mbr", ".manifest",
											  ".properties",".pnf" ,  ".prx", ".scr", ".settings",  ".so",  ".sys",  ".xml", ".exe"};
		string[] videoFiles = new string[] { ".mov", ".qt", ".mpg",".mpeg",  ".mp4",  ".m1v", ".mp2", ".mpa",".mpe",".webm",  ".ogg",  ".3gp",  ".3g2",  ".asf",  ".asx",
												".m2ts",".dvr-ms",".ivf",".wax",".wmv", ".wvx",  ".wm",  ".wmx",  ".wmz",  ".swf", ".flv", };
		string[] imageFiles = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".tif", ".ico", ".bmp" };
		string[] audioFiles = new string[] { ".adt",  ".adts", ".aif",  ".aifc", ".aiff", ".au", ".snd", ".cda",
												".mp3", ".m4a", ".aac", ".ogg", ".mid", ".midi", ".rmi", ".ra", ".flac", ".wax", ".wma", ".wav" };
		string[] textFiles = new string[] { ".txt", ".html", ".htm", ".xhtml", ".xml", ".rss", ".xml", ".css", ".js", ".vbs", ".cgi", ".php" };
		string[] applicationFiles = new string[] { ".zip", ".pdf", ".doc", ".m3u", ".xls", ".wpl", ".wmd", ".wms", ".wmz", ".wmd" };
		string copySouce = "";      //コピーするアイテムのurl
		string cutSouce = "";       //カットするアイテムのurl

		public Form1()
		{
			InitializeComponent();
			///WebBrowserコントロールを配置すると、IEのバージョン 7をIE11の Edgeモードに変更//http://blog.livedoor.jp/tkarasuma/archives/1036522520.html
			regkey.SetValue( process_name, 11001, Microsoft.Win32.RegistryValueKind.DWord );
			regkey.SetValue( process_dbg_name, 11001, Microsoft.Win32.RegistryValueKind.DWord );

			fileTree.LabelEdit = true;         //ツリーノードをユーザーが編集できるようにする

			//イベントハンドラの追加
			fileTree.BeforeLabelEdit += new NodeLabelEditEventHandler( fileTree_BeforeLabelEdit );
			fileTree.AfterLabelEdit += new NodeLabelEditEventHandler( fileTree1_AfterLabelEdit );
			fileTree.KeyUp += new KeyEventHandler( fileTree_KeyUp );

			元に戻す.Visible = false;
			ペーストToolStripMenuItem.Visible = false;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			regkey.DeleteValue( process_name );
			regkey.DeleteValue( process_dbg_name );
			regkey.Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string TAG = "[Form1_Load]";
			string dbMsg = TAG;

			fileTree.ImageList = this.imageList1;   //☆treeView1では設定できなかった
			MakeDriveList();

			/*	//イベントハンドラを追加する
				fileTree.ItemDrag += new ItemDragEventHandler( TreeView1_ItemDrag );
				fileTree.DragOver += new DragEventHandler( TreeView1_DragOver );
				fileTree.DragDrop += new DragEventHandler( TreeView1_DragDrop );*/
			MyLog( dbMsg );
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			Application.ApplicationExit -= new EventHandler( Application_ApplicationExit );         //ApplicationExitイベントハンドラを削除
		}       //ApplicationExitイベントハンドラ

		/*		private void Form1_ResizeEnd(object sender, EventArgs e)
				{
					string TAG = "[Form1_ResizeEnd]";
					string dbMsg = TAG;

					ReSizeViews();
					MyLog( dbMsg );
				}*/
		/// <summary>
		/// /////////////////////////////////////////////////////////////////////////////////////////////////////Formイベント/////
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			string TAG = "[treeView1_BeforeExpand]";
			string dbMsg = TAG;
			//	dbMsg += "sender=" + sender;
			//	dbMsg += "e=" + e;
			try {
				TreeNode tn = e.Node;//, tn2;
				string sarchDir = tn.Text;//展開するノードのフルパスを取得		FullPath だとM:\\DL
				dbMsg += ",sarchDir=" + sarchDir;
				/*		string motoPass = passNameLabel.Text + "";
						dbMsg += ",motoPass=" + motoPass;
						if (motoPass != "") {
							sarchDir = motoPass + sarchDir;// + Path.DirectorySeparatorChar
						} else if (0 < motoPass.IndexOf( ":", StringComparison.OrdinalIgnoreCase )) {
							sarchDir = tn.Text;
						}
						dbMsg += ">sarchDir>" + sarchDir;
						passNameLabel.Text = sarchDir;
						*/
				tn.Nodes.Clear();
				//	FolderItemListUp( sarchDir, tn );

				/*
								tn.Nodes.Clear();
								di = new DirectoryInfo( sarchDir );//ディレクトリ一覧を取得
								//string sarchDir = di.Name;
								MyLog( dbMsg );
								foreach (FileInfo fi in di.GetFiles(  )) {
									tn2 = new TreeNode( fi.Name, 3, 3 );
									string rfileName = fi.Name;
									rfileName = rfileName.Replace( sarchDir,"" );
									dbMsg += ",rfileName=" + rfileName;
									tn.Nodes.Add( rfileName );
								}
								MyLog( dbMsg );
								foreach (DirectoryInfo d2 in di.GetDirectories(  )) {
									tn2 = new TreeNode( d2.Name, 1, 2 );
									string rfolereName = d2.Name;
									 rfolereName = rfolereName.Replace( sarchDir + Path.DirectorySeparatorChar, "" );
									dbMsg += ",rfolereName=" + rfolereName;
									tn.Nodes.Add( rfolereName );
									FolderItemListUp( d2.Name, tn2 );
									//	tn2.Nodes.Add( "..." );
								}
								*/
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}       //ノードを展開しようとしているときに発生するイベント

		/// <summary>
		/// ファイルクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>	
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)//NodeMouseClickが利かなかった
		{
			string TAG = "[treeView1_AfterSelect]";
			string dbMsg = TAG;
			try {
				//		dbMsg += "sender=" + sender;                    //sender=	常にSystem.Windows.Forms.TreeView, Nodes.Count: 5, Nodes[0]: TreeNode: C:\,
				//		dbMsg += ",e=" + e;                             //e=		常にSystem.Windows.Forms.TreeViewEventArgs,
				typeName.Text = "";
				mineType.Text = "";
				TreeNode selectNode = e.Node;
				string selectItem = selectNode.Text;
				dbMsg += ",selectItem=" + selectItem;
				string fullPathName = selectNode.FullPath;  //selectItem;
															/*		TreeNode selectParent = selectNode.Parent;
																	while (selectParent != null) {                                  //親ノードが無くなるまで
																		string parentText = selectParent.Text;                      //
																		if (0 < parentText.IndexOf( Path.DirectorySeparatorChar + "", StringComparison.OrdinalIgnoreCase )) {
																			fullPathName = parentText + fullPathName;
																		} else {
																			fullPathName = parentText + Path.DirectorySeparatorChar + fullPathName;
																		}
																		selectParent = selectParent.Parent;
																	}*/
				dbMsg += ",fullPathName=" + fullPathName;
				FileInfo fi = new FileInfo( fullPathName );
				String infoStr = ",Exists;";
				infoStr += fi.Exists;
				fileinfo.Text = infoStr;
				string fullName = fi.FullName;
				infoStr += ",絶対パス;" + fullName;
				infoStr += ",親ディレクトリ;" + fi.Directory;// 
				string passNameStr = fi.DirectoryName + "";    //親ディレクトリ名
				if (passNameStr == "") {
					passNameStr = fullName;
				}
				passNameLabel.Text = passNameStr;    //親ディレクトリ名
				string fileNameStr = fi.Name + "";//ファイル名= selectItem;
				if (fileNameStr == "") {
					fileNameStr = fullName;
				}
				fileNameLabel.Text = fileNameStr;//ファイル名= selectItem;
				lastWriteTime.Text = fi.LastWriteTime.ToString();//更新
				creationTime.Text = fi.CreationTime.ToString();//作成
				lastAccessTime.Text = fi.LastAccessTime.ToString();//アクセス
				rExtension.Text = fi.Extension.ToString();//拡張子
														  //		int32 fileLength = fi.Length*1;
				dbMsg += ",infoStr=" + infoStr;                             //infoStr=,Exists;False,拡張子;作成;2012/11/04 3:56:33,アクセス;2012/11/04 3:56:33,絶対パス;I:\Dtop,親ディレクトリ;I:\

				string fileAttributes = fi.Attributes.ToString();
				dbMsg += ",Attributes=" + fileAttributes;
				dbMsg += ",Directory.Exists=" + Directory.Exists( fullName );                             //infoStr=,Exists;False,拡張子;作成;2012/11/04 3:56:33,アクセス;2012/11/04 3:56:33,絶対パス;I:\Dtop,親ディレクトリ;I:\
				名称変更ToolStripMenuItem.Visible = true;
				if (copySouce != "" || cutSouce != "") {
					ペーストToolStripMenuItem.Visible = true;
					コピーToolStripMenuItem.Visible = false;
					if (cutSouce != "") {
						カットToolStripMenuItem.Visible = false;
					}
				} else {
					ペーストToolStripMenuItem.Visible = false;
					コピーToolStripMenuItem.Visible = true;
					カットToolStripMenuItem.Visible = true;
				}
				削除ToolStripMenuItem.Visible = true;
				if (fi.Exists) {                //Attributes=Archive
					if (rExtension.Text != "") {
						fileLength.Text = fi.Length.ToString();//ファイルサイズ
						他のアプリケーションで開くToolStripMenuItem.Visible = true;
						MakeWebSouce( fullName );
					}
				} else if (Directory.Exists( fullName )) {                               //フォルダの場合	(		fileAttributes == "Directory"
					dbMsg += ",Directoryを選択";
					fileLength.Text = "";//ファイルサイズ
										 //			TreeNode tNode = e.Node.Nodes;//new TreeNode( selectItem, selectIindex, 0 );
					FolderItemListUp( fullName, e.Node );
					フォルダ作成ToolStripMenuItem.Visible = true;
					他のアプリケーションで開くToolStripMenuItem.Visible = false;
					if (fileNameLabel.Text == passNameLabel.Text) {
						dbMsg += ",ドライブを選択";
						名称変更ToolStripMenuItem.Visible = false;
						コピーToolStripMenuItem.Visible = false;
						カットToolStripMenuItem.Visible = false;
						削除ToolStripMenuItem.Visible = false;
						元に戻す.Visible = false;
					}
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}

		private void FolderItemListUp(string sarchDir, TreeNode tNode)//, string sarchTyp
		{
			string TAG = "[FolderItemListUp]";
			string dbMsg = TAG;
			try {
				dbMsg += "sarchDir=" + sarchDir;                    //sender=System.Windows.Forms.TreeView, Nodes.Count: 5, Nodes[0]: TreeNode: C:\,
				dbMsg += ",tNode=" + tNode;                             //e=System.Windows.Forms.TreeViewEventArgs,
				dbMsg += ",Nodes=" + tNode.Nodes.ToString();
				tNode.Nodes.Clear();

				string[] files = Directory.GetFiles( sarchDir );        //		sarchDir	"C:\\\\マイナンバー.pdf"	string	☆sarchDir = "\\2013.m3u"でフルパスになっていない
				if (files != null) {
					foreach (string fileName in files) {
						string[] extStrs = fileName.Split( '.' );
						string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
						dbMsg += "\n拡張子=" + extentionStr;
						if (-1 < Array.IndexOf( systemFiles, extentionStr ) ||
							0 < fileName.IndexOf( "BOOTNXT", StringComparison.OrdinalIgnoreCase ) ||
							0 < fileName.IndexOf( "-ms", StringComparison.OrdinalIgnoreCase ) ||
							0 < fileName.IndexOf( "RECYCLE", StringComparison.OrdinalIgnoreCase )
							) {
						} else {
							int iconType = 2;
							if (-1 < Array.IndexOf( videoFiles, extentionStr )) {
								iconType = 3;
							} else if (-1 < Array.IndexOf( imageFiles, extentionStr )) {
								iconType = 4;
							} else if (-1 < Array.IndexOf( audioFiles, extentionStr )) {
								iconType = 5;
							} else if (-1 < Array.IndexOf( textFiles, extentionStr )) {
								iconType = 2;
							}
							dbMsg += ",iconType=" + iconType;
							string rfileName = fileName.Replace( sarchDir, "" );
							rfileName = rfileName.Replace( Path.DirectorySeparatorChar + "", "" );
							dbMsg += ",file=" + rfileName;
							tNode.Nodes.Add( fileName, rfileName, iconType, iconType );
						}
					}
				}
				string[] foleres = Directory.GetDirectories( sarchDir );//
				if (foleres != null) {
					foreach (string folereName in foleres) {
						if (-1 < folereName.IndexOf( "RECYCLE", StringComparison.OrdinalIgnoreCase ) ||
							-1 < folereName.IndexOf( "System Vol", StringComparison.OrdinalIgnoreCase )) {
						} else {
							string rfolereName = folereName.Replace( sarchDir, "" );// + 
							rfolereName = rfolereName.Replace( Path.DirectorySeparatorChar + "", "" );
							dbMsg += ",foler=" + rfolereName;
							tNode.Nodes.Add( folereName, rfolereName, 1, 1 );
						}
					}           //ListBox1に結果を表示する
				}
				//		MyLog( dbMsg );
			} catch (UnauthorizedAccessException UAEx) {
				Console.WriteLine( TAG + "で" + UAEx.Message + "発生;" + dbMsg );
			} catch (PathTooLongException PathEx) {
				Console.WriteLine( TAG + "で" + PathEx.Message + "発生;" + dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}       //フォルダの中身をリストアップ


		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectItem = listBox1.SelectedItem.ToString();
			FileInfo fi = new FileInfo( selectItem );
			String infoStr = ",Exists;";
			infoStr += fi.Exists;
			infoStr += ",拡張子;";
			infoStr += fi.Extension;
			infoStr += "作成;";
			infoStr += fi.CreationTime;
			infoStr += ",アクセス;";
			infoStr += fi.LastAccessTime;
			infoStr += ",更新;";
			infoStr += fi.LastWriteTime;
			if (fi.Exists) {
				infoStr += ",ファイルサイズ;";
				infoStr += fi.Length;
			} else {
				MakeFolderList( selectItem );
			}
			infoStr += ",絶対パス;";
			infoStr += fi.FullName;//       
			infoStr += ",ファイル名;";
			infoStr += fi.Name;
			infoStr += ",親ディレクトリ;";
			infoStr += fi.Directory;//     
			infoStr += ",親ディレクトリ名;";
			infoStr += fi.DirectoryName;
			fileinfo.Text = infoStr;
		}           //リストアイテムのクリック

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			string TAG = "[webBrowser1_DocumentCompleted]";
			string dbMsg = TAG;
			//	ReSizeViews();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectDrive = comboBox1.SelectedItem.ToString();
			listBox1.Items.Clear();
			MakeFolderList( selectDrive );
		}           //ドライブセレクト

		private void MakeFolderList(string sarchDir)//, string sarchTyp
		{
			try {
				string[] files = Directory.GetFiles( sarchDir );
				if (files != null) {
					foreach (string fileName in files) {
						if (-1 < fileName.IndexOf( "RECYCLE.BIN", StringComparison.OrdinalIgnoreCase )) {
						} else {

							string rfileName = fileName.Replace( sarchDir, "" );
							listBox1.Items.Add( rfileName );      //ListBox1に結果を表示する
						}
					}
				}
				string[] foleres = Directory.GetDirectories( sarchDir );//
				if (foleres != null) {
					foreach (string folereName in foleres) {
						if (-1 < folereName.IndexOf( "RECYCLE", StringComparison.OrdinalIgnoreCase ) ||
							-1 < folereName.IndexOf( "System Vol", StringComparison.OrdinalIgnoreCase )
							) { } else {
							listBox1.Items.Add( folereName );
							//        MakeFolderList(folereName);
						}
					}           //ListBox1に結果を表示する

				}
			} catch (UnauthorizedAccessException UAEx) {
				Console.WriteLine( UAEx.Message );
			} catch (PathTooLongException PathEx) {
				Console.WriteLine( PathEx.Message );
			}

		}       //ファイルリストアップ

		private void MakeFileList(string sarchDir, string sarchType)
		{
			string[] files = Directory.GetFiles( "c:\\" );
			foreach (string fileName in files) {
				listBox1.Items.Add( fileName );
			}           //ListBox1に結果を表示する

			//     System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sarchDir);
			//     System.IO.FileInfo[] files =di.GetFiles(sarchType, System.IO.SearchOption.AllDirectories);
			//        foreach (System.IO.FileInfo f in files)
			//       {
			//           listBox1.Items.Add(f.FullName);
			//       }           //ListBox1に結果を表示する

			//以下2行でも同様      https://dobon.net/vb/dotnet/file/getfiles.html
			//            string[] files = System.IO.Directory.GetFiles( sarchDir, sarchType, System.IO.SearchOption.AllDirectories);           //"C:\test"以下のファイルをすべて取得する
			//         listBox1.Items.AddRange(files);           //ListBox1に結果を表示する
		}       //ファイルリストアップ

		private void MakeDriveList()
		{
			//		ManagementObject mo = new ManagementObject();

			TreeNode tn;
			//	fileTree.ImageIndex =2;
			//	fileTree.SelectedImageIndex = 0;

			foreach (DriveInfo drive in DriveInfo.GetDrives())//http://www.atmarkit.co.jp/fdotnet/dotnettips/557driveinfo/driveinfo.html
			{
				string driveNames = drive.Name; // ドライブ名
				if (drive.IsReady) { // ドライブの準備はOK？
					comboBox1.Items.Add( driveNames ); //comboBoxに結果を表示する
													   //         Console.WriteLine("\t{0}\t{1}\t{2}",
													   //           drive.DriveFormat,  // フォーマット
													   //           drive.DriveType,    // 種類
													   //           drive.VolumeLabel); // ボリュームラベル
													   //	tn = new TreeNode( driveNames, 0, 0 );//ノードにドライブアイコンを設定
					tn = new TreeNode( driveNames, 0, 0 );
					//		tn.ImageIndex = 1;          //folder_close_icon.png
					fileTree.Nodes.Add( tn );//親ノードにドライブを設定
					FolderItemListUp( driveNames, tn );
					tn.ImageIndex = 0;          //hd_icon.png

				}
			}
			comboBox1.SelectedIndex = 3;

		}//使用可能なドライブリスト取得

		////ファイル操作////////////////////////////////////////////////////////////////////
		public string GetFileTypeStr(string checkFileName)
		{
			string TAG = "[GetFileTypeStr]";
			string dbMsg = TAG;
			//	try {
			string retType = "";
			string retMIME = "";
			string[] extStrs = checkFileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
			dbMsg += "\n拡張子=" + extentionStr;
			if (-1 < extentionStr.IndexOf( ".mov", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".qt", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/quicktime";
			} else if (-1 < extentionStr.IndexOf( ".mpg", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".mpeg", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/mpeg";
			} else if (-1 < extentionStr.IndexOf( ".mp4", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/mp4";        //ver12:MP4 ビデオ ファイル 
			} else if (-1 < extentionStr.IndexOf( ".webm", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/webm";
			} else if (-1 < extentionStr.IndexOf( ".ogv", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/ogv";
			} else if (-1 < extentionStr.IndexOf( ".avi", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-msvideo";
			} else if (-1 < extentionStr.IndexOf( ".3gp", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/3gpp";     //audio/3gpp
			} else if (-1 < extentionStr.IndexOf( ".3g2", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/3gpp2";            //audio/3gpp2
			} else if (-1 < extentionStr.IndexOf( ".asf", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-ms-asf";
			} else if (-1 < extentionStr.IndexOf( ".asx", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-ms-asf";   //ver9:Windows Media メタファイル 
			} else if (-1 < extentionStr.IndexOf( ".wax", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";   //ver9:Windows Media メタファイル 
			} else if (-1 < extentionStr.IndexOf( ".wmv", StringComparison.OrdinalIgnoreCase )) {
				retMIME = "video/x-ms-wmv";      //ver9:Windows Media 形式
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".wvx", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-ms-wvx";       //ver9:Windows Media メタファイル 
			} else if (-1 < extentionStr.IndexOf( ".wmx", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-ms-wmx";       //ver9:Windows Media メタファイル 
			} else if (-1 < extentionStr.IndexOf( ".wmz", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "application/x-ms-wmz";
			} else if (-1 < extentionStr.IndexOf( ".wmd", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "application/x-ms-wmd";
			} else if (-1 < extentionStr.IndexOf( ".swf", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "application/x-shockwave-flash";
			} else if (-1 < extentionStr.IndexOf( ".flv", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				retMIME = "video/x-flv";
			} else if (-1 < extentionStr.IndexOf( ".ivf", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";     //ver10:Indeo Video Technology
			} else if (-1 < extentionStr.IndexOf( ".dvr-ms", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";            //ver12:Microsoft デジタル ビデオ録画
			} else if (-1 < extentionStr.IndexOf( ".m2ts", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";           //ver12:MPEG-2 TS ビデオ ファイル 
			} else if (-1 < extentionStr.IndexOf( ".m1v", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".mp2", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".mpa", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".mpe", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".m3u", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".m4v", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
			} else if (-1 < extentionStr.IndexOf( ".mp4v", StringComparison.OrdinalIgnoreCase )) {
				retType = "video";
				//image/////////////////////////////////////////////////////////////////////////
			} else if (-1 < extentionStr.IndexOf( ".jpg", StringComparison.OrdinalIgnoreCase ) ||
					 -1 < extentionStr.IndexOf( ".jpeg", StringComparison.OrdinalIgnoreCase )) {
				retType = "image";
				retMIME = "image/jpeg";
			} else if (-1 < extentionStr.IndexOf( ".gif", StringComparison.OrdinalIgnoreCase )) {
				retType = "image";
				retMIME = "image/gif";
			} else if (-1 < extentionStr.IndexOf( ".png", StringComparison.OrdinalIgnoreCase )) {
				retType = "image";
				retMIME = "image/png";
			} else if (-1 < extentionStr.IndexOf( ".ico", StringComparison.OrdinalIgnoreCase )) {
				retType = "image";
				retMIME = "image/vnd.microsoft.icon";
			} else if (-1 < extentionStr.IndexOf( ".bmp", StringComparison.OrdinalIgnoreCase )) {
				retType = "image";
				retMIME = "image/x-ms-bmp";
				//audio/////////////////////////////////////////////////////////////////////////
			} else if (-1 < extentionStr.IndexOf( ".mp3", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/mpeg";
			} else if (-1 < extentionStr.IndexOf( ".m4a", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".aac", StringComparison.OrdinalIgnoreCase )
				) {
				retType = "audio";
				retMIME = "audio/aac";         //var12;MP4 オーディオ ファイル
			} else if (-1 < extentionStr.IndexOf( ".ogg", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/ogg";
			} else if (-1 < extentionStr.IndexOf( ".midi", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".mid", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".rmi", StringComparison.OrdinalIgnoreCase )
				) {
				retType = "audio";
				retMIME = "audio/midi";          //var9;MIDI 
			} else if (-1 < extentionStr.IndexOf( ".ra", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/vnd.rn-realaudio";
			} else if (-1 < extentionStr.IndexOf( ".flac", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/flac";
			} else if (-1 < extentionStr.IndexOf( ".wma", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/x-ms-wma";
			} else if (-1 < extentionStr.IndexOf( ".wav", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				retMIME = "audio/wav";           //var9;Windows 用オーディオ   
			} else if (-1 < extentionStr.IndexOf( ".aif", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".aifc", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".aiff", StringComparison.OrdinalIgnoreCase )
				) {
				retType = "audio";           //var9;Audio Interchange File FormatI 
			} else if (-1 < extentionStr.IndexOf( ".au", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";          //var9;Sun Microsystems  
			} else if (-1 < extentionStr.IndexOf( ".snd", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";          //var9; NeXT  
			} else if (-1 < extentionStr.IndexOf( ".cda", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";          //var9;CD オーディオ トラック 
			} else if (-1 < extentionStr.IndexOf( ".adt", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";          //var12;Windows オーディオ ファイル 
			} else if (-1 < extentionStr.IndexOf( ".adts", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";           //var12;Windows オーディオ ファイル 
			} else if (-1 < extentionStr.IndexOf( ".asx", StringComparison.OrdinalIgnoreCase )) {
				retType = "audio";
				//text/////////////////////////////////////////////////////////////////////////
			} else if (-1 < extentionStr.IndexOf( ".txt", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "text/plain";
			} else if (-1 < extentionStr.IndexOf( ".html", StringComparison.OrdinalIgnoreCase ) ||
				-1 < extentionStr.IndexOf( ".htm", StringComparison.OrdinalIgnoreCase )
				) {
				retType = "text";
				retMIME = "text/html";
			} else if (-1 < extentionStr.IndexOf( ".xhtml", StringComparison.OrdinalIgnoreCase )) {
				retMIME = "application/xhtml+xml";
			} else if (-1 < extentionStr.IndexOf( ".xml", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "text/xml";
			} else if (-1 < extentionStr.IndexOf( ".rss", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "application/rss+xml";
			} else if (-1 < extentionStr.IndexOf( ".xml", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "application/xml";            //、text/xml
			} else if (-1 < extentionStr.IndexOf( ".css", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "text/css";
			} else if (-1 < extentionStr.IndexOf( ".js", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "text/javascript";
			} else if (-1 < extentionStr.IndexOf( ".vbs", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "text/vbscript";
			} else if (-1 < extentionStr.IndexOf( ".cgi", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "application/x-httpd-cgi";
			} else if (-1 < extentionStr.IndexOf( ".php", StringComparison.OrdinalIgnoreCase )) {
				retType = "text";
				retMIME = "application/x-httpd-php";
				//application/////////////////////////////////////////////////////////////////////////
			} else if (-1 < extentionStr.IndexOf( ".zip", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";
				retMIME = "application/zip";
			} else if (-1 < extentionStr.IndexOf( ".pdf", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";
				retMIME = "application/pdf";
			} else if (-1 < extentionStr.IndexOf( ".doc", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";
				retMIME = "application/msword";
			} else if (-1 < extentionStr.IndexOf( ".xls", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";
				retMIME = "application/msexcel";
			} else if (-1 < extentionStr.IndexOf( ".wmx", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";        //ver9:Windows Media Player スキン 
			} else if (-1 < extentionStr.IndexOf( ".wms", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";       //ver9:Windows Media Player スキン  
			} else if (-1 < extentionStr.IndexOf( ".wmz", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";       //ver9:Windows Media Player スキン  
			} else if (-1 < extentionStr.IndexOf( ".wpl", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";       //ver9:Windows Media Player スキン  
			} else if (-1 < extentionStr.IndexOf( ".wmd", StringComparison.OrdinalIgnoreCase )) {
				retType = "application";       //ver9:Windows Media Download パッケージ   

			} else if (-1 < extentionStr.IndexOf( ".wm", StringComparison.OrdinalIgnoreCase )) {        //以降wmで始まる拡張子が誤動作
				retType = "video";
				retMIME = "video/x-ms-wm";
			}
			//	}
			typeName.Text = retType;
			mineType.Text = retMIME;
			return retType;
			//		MyLog( dbMsg );
			//		} catch (Exception er) {
			//		Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			//	}
		}       //拡張子からタイプとMIMEを返す

		private string ReadTextFile(string fileName, string emCord)
		{
			string TAG = "[ReadTextFile]";
			string dbMsg = TAG;
			string retStr = "";
			try {
				dbMsg += ",fileName=" + fileName + ",emCord=" + emCord;
				StreamReader sr = new StreamReader( fileName, Encoding.GetEncoding( emCord ) );
				retStr = sr.ReadToEnd();
				sr.Close();
			} catch (Exception e) {
				Console.WriteLine( TAG + "でエラー発生" + e.Message + ";" + dbMsg );
			}
			MyLog( dbMsg );
			return retStr;
		}           //テキスト系ファイルの読込み	http://www.atmarkit.co.jp/ait/articles/0306/13/news003.html

		////web/////////////////////////////////////////////////////////////////ファイル操作///
		private string MakeVideoSouce(string fileName, int webWidth, int webHeight)
		{
			string TAG = "[MakeVideoSouce]";
			string dbMsg = TAG;
			string contlolPart = "";
			string comentStr = "このタイプの表示は検討中です。";
			string[] extStrs = fileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
			string[] souceNames = fileName.Split( Path.DirectorySeparatorChar );
			string souceName = souceNames[souceNames.Length - 1];
			string mineTypeStr = mineType.Text;//	"video/x-ms-asf";     //.asf
			string clsId = "";
			string codeBase = "";

			if (extentionStr == ".webm" ||
				extentionStr == ".ogg"
				) {
				contlolPart += "<div class=" + '"' + "video-container" + '"' + ">\n";
				contlolPart += "\t\t\t<video src=" + '"' + "file://" + fileName + '"' +
											" controls autoplay style = " + '"' + "width:100%;height: auto;" + '"' +
												"></video>\n\t\t</div>";          // 
				comentStr = "読み込めないファイルは対策検討中です。。";
			} else if (extentionStr == ".flv" ||
				extentionStr == ".swf"
				) {
				clsId = "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000";
				codeBase = "http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,0,0";
				contlolPart += "<object classid=" + '"' + clsId + '"' +
								" CODEBASE=" + '"' + codeBase + '"' +
								" width = " + '"' + webWidth + '"' + " height = " + '"' + webHeight + '"' + ">\n";
				//	" id = " + '"' + "previewDB2" + '"' +  " type = " + '"' + mineTypeStr + '"' +">\n";
				//			contlolPart += "\t\t\t<param name=" + '"' + "FlashVars" + '"' + " value=" + '"' + "fms_app=&video_file=" +  "file://" + fileName+'"' + "/>\n";
				//&image_file=&link_url=&autoplay=false&mute=false&vol=&controllbar=true&buffertime=5" />
				contlolPart += "\t\t\t<param name =" + '"' + "movie" + '"' + " value = " + '"' + "file://" + fileName + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "bgcolor" + '"' + " value = " + '"' + "#FFFFFF" + '"' + "/>\n";
				//		contlolPart += "\t\t\t<param name= " + '"' + "bgcolor" + '"' + " value=" + '"' + "#fff" + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "LOOP" + '"' + " value = " + '"' + "false" + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "quality" + '"' + " value = " + '"' + "high" + '"' + "/>\n";
				//		contlolPart += "\t\t\t<param name =E=quality VALUE=high/>\n";
				contlolPart += "\t\t\t\t<embed src=" + '"' + "file://" + fileName + '"' +
											" width=" + webWidth + " height= " + webHeight + " bgcolor=#FFFFFF" +
											" LOOP=false quality=high PLUGINSPAGE=" + '"' + "http://www.macromedia.com/shockwave/download/index.cgi?" +
												"P1_Prod_Version=ShockwaveFlash" + '"' + " type=" + '"' + mineTypeStr + '"' + "/>\n";

				//			contlolPart += "\t\t\t<param name= " + '"' + "allowScriptAccess" + '"' + " value=" + '"' + "always" + '"' + "/>\n";
				//			contlolPart += "\t\t\t<param name= " + '"' + "allowFullScreen" + '"' + " value=" + '"' + "true" + '"' + "/>\n";
				//			contlolPart += "\t\t\t<param name= " + '"' + "scale" + '"' + " value=" + '"' + "noscale" + '"' + "/>\n";
				//	contlolPart += "\t\t\t<param name=" + '"' + "FlashVars" + '"' + " value=" + '"' +  '"' + "/>\n";
				//			contlolPart += "\n\t\t\t<param name=" + '"' + "FlashVars" + '"' + " value=" + '"' + "file://" + fileName + '"' + "/>\n";
				/* 
				 * < EMBED SRC="test.swf" WIDTH=300 HEIGHT=300 bgcolor=#FFFFFF LOOP=false QUALITY=high 
PLUGINSPAGE="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash" TYPE="application/x-shockwave-flash" </EMBED>


				 * <param name="flashvars" value="fms_app=&amp;video_file=MVI_7565.flv&amp;image_file=&amp;link_url=&amp;autoplay=false&amp;mute=false&amp;vol=&amp;controllbar=true&amp;buffertime=5">
				contlolPart += "\t\t\t<embed type=" + '"' + mineTypeStr + '"' + " align = " + '"' + "middle" + '"' +
															" width = " + '"' + webWidth + '"' + " height = " + '"' + webHeight + '"' +
															" name = " + '"' + "previewDB2" + '"' + " allowScriptAccess = " + '"' + "always" + '"' +
															" allowFullScreen = " + '"' + "true" + '"' + " scale = " + '"' + "noscale" + '"' +
															" quality = " + '"' + "high" + '"' + " src = " + '"' + "file://" + fileName + '"' +
															" bgcolor = " + '"' + "#fff" + '"' + " FlashVars = " + '"'  + '"' + "/></embed>\n";
			*/


				//		contlolPart += "\n\t\t< param name = " + '"' + "FlashVars" + '"' + "value = " + '"' + "flv= + '"' +fileName + '"' +"&autoplay=1&margin=0" + '"' + "/>\n\t\t\t";
			} else if (extentionStr == ".wmv" ||        //ver9:Windows Media 形式
				extentionStr == ".asf" ||
				extentionStr == ".wm" ||
				extentionStr == ".asx" ||        //ver9:Windows Media メタファイル 
				extentionStr == ".wax" ||        //ver9:Windows Media メタファイル 
				extentionStr == ".wvx" ||        //ver9:Windows Media メタファイル 
				extentionStr == ".wmx" ||        //ver9:Windows Media メタファイル 
				extentionStr == ".ivf" ||        //ver10:Indeo Video Technology
				extentionStr == ".dvr-ms" ||        //ver12:Microsoft デジタル ビデオ録画
				extentionStr == ".m2ts" ||        //ver12:MPEG-2 TS ビデオ ファイル 
				extentionStr == ".mpg" ||
				extentionStr == ".m1v" ||
				extentionStr == ".mp2" ||
				extentionStr == ".mpa" ||
				extentionStr == ".mpe" ||
				extentionStr == ".mp4" ||        //ver12:MP4 ビデオ ファイル 
				extentionStr == ".m4v" ||
				extentionStr == ".mp4" ||
				extentionStr == ".mp4v" ||
				extentionStr == ".mpeg" ||
				extentionStr == ".mpeg" ||
				extentionStr == ".mpeg" ||
				extentionStr == ".3gp" ||
				extentionStr == ".3gpp" ||
				extentionStr == ".qt" ||
				extentionStr == ".mov"       //ver12:QuickTime ムービー ファイル 
				) {
				clsId = "CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6";   //Windows Media Player9
				contlolPart += "\n\t\t<object classid =" + '"' + clsId + '"' + " width = " + '"' + webWidth + '"' + " height = " + '"' + webHeight + '"' + ">\n";
				contlolPart += "\t\t\t<param name =" + '"' + "url" + '"' + "value = " + '"' + "file://" + fileName + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "stretchToFit" + '"' + " value = true />\n";//右クリックして縮小/拡大で200％
				contlolPart += "\t\t\t<param name =" + '"' + "autoStart" + '"' + " value = " + true + "/>\n";
				comentStr = souceName + "\n" + "Windows Media Player読み込めないファイルは対策検討中です。";
				///参照 http://so-zou.jp/web-app/tech/html/sample/embed-video.htm/////
				/////https://support.microsoft.com/ja-jp/help/316992/file-types-supported-by-windows-media-player
			} else {
				comentStr = "この形式は対応確認中です。";
			}
			contlolPart += "\t\t</object>\n";

			contlolPart += "\t\t<div>\n\t\t\t" + comentStr + "\n\t\t</div>\n";
			MyLog( dbMsg );
			return contlolPart;
		}           //Video用のタグを作成

		private string MakeImageSouce(string fileName, int webWidth, int webHeight)
		{
			string TAG = "[MakeImageSouce]";
			string dbMsg = TAG;
			string contlolPart = "";
			string comentStr = "";
			string[] extStrs = fileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
			if (extentionStr == ".jpg" ||
				extentionStr == ".jpeg" ||
				extentionStr == ".png" ||
				extentionStr == ".gif"
				) {
			} else {
				/*	 ".tif", ".ico", ".bmp" };*/
				comentStr = "静止画はimgタグで読めるもののみ対応しています。";
			}
			contlolPart += "\n\t\t<img src = " + '"' + fileName + '"' + " style=" + '"' + "width:100%" + '"' + "/>\n";
			// + '"' + webWidth + '"' + " height = " + '"' + webHeight + '"' +
			contlolPart += "\t\t<div>\n\t\t\t" + comentStr + "\n\t\t</div>\n";
			MyLog( dbMsg );
			return contlolPart;
		}  //静止画用のタグを作成

		private string MakeAudioSouce(string fileName)
		{
			string TAG = "[MakeAudioSouce]";
			string dbMsg = TAG;
			string contlolPart = "";
			string comentStr = "";
			string[] extStrs = fileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();

			if (extentionStr == ".mp3" ||
				extentionStr == ".aac" ||
				extentionStr == ".m4a" ||           //iTurne
				extentionStr == ".ogg"
				) {
				//		contlolPart += "<div class=" + '"' + "video-container" + '"' + ">\n";
				contlolPart += "\t\t\t<audio src=" + '"' + "file://" + fileName + '"' + " controls autoplay style = " + '"' + "width:100%" + '"' + " />\n";
				comentStr = "audioタグで読み込めないファイルは対策検討中です。。";
			} else if (extentionStr == ".wma" ||
				extentionStr == ".wvx" ||
				extentionStr == ".wax" ||
				extentionStr == ".wav" ||
				extentionStr == ".m4a" ||           //var12;MP4 オーディオ ファイル
				extentionStr == ".midi" ||           //var9;MIDI 
				extentionStr == ".mid" ||           //var9;MIDI 
				extentionStr == ".rmi" ||           //var9;MIDI 
				extentionStr == ".aif" ||           //var9;Audio Interchange File FormatI 
				extentionStr == ".aifc" ||           //var9;Audio Interchange File FormatI 
				extentionStr == ".aiff" ||           //var9;Audio Interchange File FormatI 
				extentionStr == ".au" ||           //var9;Sun Microsystems および NeXT  
				extentionStr == ".snd" ||           //var9;Sun Microsystems および NeXT  
				extentionStr == ".wav" ||           //var9;Windows 用オーディオ   
				extentionStr == ".cda" ||           //var9;CD オーディオ トラック 
				extentionStr == ".adt" ||           //var12;Windows オーディオ ファイル 
				extentionStr == ".adts" ||           //var12;Windows オーディオ ファイル 
				extentionStr == ".asx"
				) {
				string clsId = "CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6";   //Windows Media Player9
				contlolPart += "\n\t\t<object classid =" + '"' + clsId + '"' + " style = " + '"' + "width:100%" + '"' + " >\n";
				contlolPart += "\t\t\t<param name =" + '"' + "url" + '"' + "value = " + '"' + "file://" + fileName + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "stretchToFit" + '"' + " value = true />\n";//右クリックして縮小/拡大で200％
				contlolPart += "\t\t\t<param name =" + '"' + "autoStart" + '"' + " value = " + true + "/>\n";
				comentStr = "Windows Media Player9読み込めないファイルは対策検討中です。";

				/*		contlolPart += "<ASX VERSION =" + '"' + "3.0"  + '"' + " >\n";
						contlolPart += "\t\t<ENTRY >\n";
						contlolPart += "\t\t\t<REF HREF =" + '"' +  fileName + '"' + " >\n";//"file://" +
						contlolPart += "\t\t\t</ENTRY >\n";
						contlolPart += "\t\t\t</ASX >\n";
						  comentStr = "ASXタグで確認中です。(Windows Media Player　がサポートしている形式)";*/
			} else {
				/* ".ra", ".flac",  }; */
				comentStr = "このファイルの再生方法は確認中です。";
			}
			contlolPart += "\t\t<div>\n\t\t\t" + comentStr + "\n\t\t</div>\n";
			MyLog( dbMsg );
			return contlolPart;
		}  //静止画用のタグを作成


		private string MakeTextSouce(string fileName, int webWidth, int webHeight)
		{
			string TAG = "[MakeTextSouce]";
			string dbMsg = TAG;
			string contlolPart = "";
			string comentStr = "";
			string rText = ReadTextFile( fileName, "Shift_JIS" );
			string[] extStrs = fileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
			contlolPart += "\t\t<pre>\n";
			if (extentionStr == ".htm" ||
				extentionStr == ".html" ||
				extentionStr == ".xhtml" ||
				extentionStr == ".xml" ||
				extentionStr == ".rss" ||
				extentionStr == ".xml" ||
				extentionStr == ".css" ||
				extentionStr == ".js" ||
				extentionStr == ".vbs" ||
				extentionStr == ".cgi" ||
				extentionStr == ".php"
				) {
				rText = rText.Replace( "<", "&lt;" );
				rText = rText.Replace( ">", "&gt;" );
				contlolPart += rText;
			} else if (extentionStr == ".txt") {
				contlolPart += "\t\t\t" + rText + "\n";
			} else {
				comentStr = "このファイルの表示方法は確認中です。";
			}
			contlolPart += "\t\t</pre>\n";
			contlolPart += "\t\t<div>\n\t\t\t" + comentStr + "\n\t\t</div>\n";
			MyLog( dbMsg );
			return contlolPart;
		}  //Text用のタグを作成		


		private string MakeApplicationeSouce(string fileName, int webWidth, int webHeight)
		{
			string TAG = "[MakeApplicationeSouce]";
			string dbMsg = TAG;
			string contlolPart = "";
			string comentStr = "";
			string[] extStrs = fileName.Split( '.' );
			string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
			if (extentionStr == ".wmx" ||        //ver9:Windows Media Player スキン 
				extentionStr == ".wms" ||        //ver9:Windows Media Player スキン  
				extentionStr == ".wmz" ||     //ver9:Windows Media Player スキン  
				extentionStr == ".wms" ||     //ver9:Windows Media Player スキン  
				extentionStr == ".m3u" ||//MPEGだがrealPlayyerのプレイリスト
				extentionStr == ".wmd"     //ver9:Windows Media Download パッケージ   
				) {
				string clsId = "CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6";   //Windows Media Player9
				contlolPart += "\n\t\t<object classid =" + '"' + clsId + '"' + " style = " + '"' + "width:100%" + '"' + " >\n";
				contlolPart += "\t\t\t<param name =" + '"' + "url" + '"' + "value = " + '"' + "file://" + fileName + '"' + "/>\n";
				contlolPart += "\t\t\t<param name =" + '"' + "stretchToFit" + '"' + " value = true />\n";//右クリックして縮小/拡大で200％
				contlolPart += "\t\t\t<param name =" + '"' + "autoStart" + '"' + " value = " + true + "/>\n";
				comentStr = "Windows Media Player9読み込めないファイルは対策検討中です。";
			} else {
				comentStr = "このファイルの再生方法は確認中です。";
			}
			contlolPart += "\t\t<div>\n\t\t\t" + comentStr + "\n\t\t</div>\n";
			MyLog( dbMsg );
			return contlolPart;
		}  //アプリケーション用のタグを作成

		private void MakeWebSouceBody(string fileName)
		{
			string TAG = "[MakeWebSouceBody]";
			string dbMsg = TAG;
			try {
				dbMsg += ",fileName=" + fileName;
				string urlStr = System.Reflection.Assembly.GetExecutingAssembly().Location;//res://
				urlStr = urlStr.Substring( 0, urlStr.IndexOf( "bin" ) ) + "brows.htm";
				dbMsg += ",url=" + urlStr;
				int webWidth = webBrowser1.Width - 20;
				int webHeight = webBrowser1.Height - 40;
				dbMsg += ",web[" + webWidth + "×" + webHeight + "]";
				string[] extStrs = fileName.Split( '.' );
				string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();

				string contlolPart = @"<!DOCTYPE html>
	<html>
		<head>
			<meta charset = " + '"' + "UTF-8" + '"' + " >";
				contlolPart += "\n\t\t\t<meta http-equiv = " + '"' + "X-UA-Compatible" + '"' + " content =  " + '"' + "requiresActiveX =true" + '"' + " />";
				contlolPart += "\n\t\t\t<link rel = " + '"' + "stylesheet" + '"' + " type = " + '"' + "text/css" + '"' + " href = " + '"' + "brows.css" + '"' + "/>\n";
				contlolPart += "\t</head>\n\t<body>\n\t\t";
				string retType = GetFileTypeStr( fileName );
				dbMsg += ",retType=" + retType;
				if (retType == "video" ||
					 retType == "image" ||
					retType == "audio"
					) {
					contlolPart += "\t</head>\n\t<body style = " + '"' + "background-color: #000000;color:#ffffff;" + '"' + " >\n\t\t";
				} else {
					contlolPart += "\t</head>\n\t<body>\n\t\t";
				}

				if (retType == "video") {
					contlolPart += MakeVideoSouce( fileName, webWidth, webHeight );
				} else if (retType == "image") {
					contlolPart += MakeImageSouce( fileName, webWidth, webHeight );
				} else if (retType == "audio") {
					contlolPart += MakeAudioSouce( fileName );
				} else if (retType == "text") {
					contlolPart += MakeTextSouce( fileName, webWidth, webHeight );
				} else if (retType == "application") {
					contlolPart += MakeApplicationeSouce( fileName, webWidth, webHeight );
				}
				contlolPart += @"	</body>
</html>

";
				dbMsg += ",contlolPart=" + contlolPart;
				if (File.Exists( urlStr )) {
					dbMsg += "既存ファイル有り";
					System.IO.File.Delete( urlStr );                //20170818;ここで停止？
					dbMsg += ">Exists=" + File.Exists( urlStr );
				}
				////UTF-8でテキストファイルを作成する
				System.IO.StreamWriter sw = new System.IO.StreamWriter( urlStr, false, System.Text.Encoding.UTF8 );
				sw.Write( contlolPart );
				sw.Close();
				dbMsg += ">Exists=" + File.Exists( urlStr );
				Uri nextUri = new Uri( "file://" + urlStr );
				dbMsg += ",nextUri=" + nextUri;

				webBrowser1.Navigate( nextUri );
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}//形式に合わせたhtml作成

		private void MakeWebSouce(string fileName)
		{
			string TAG = "[MakeWebSouce]";
			string dbMsg = TAG;
			try {
				dbMsg += ",fileName=" + fileName;
				string urlStr = System.Reflection.Assembly.GetExecutingAssembly().Location;//res://
				urlStr = urlStr.Substring( 0, urlStr.IndexOf( "bin" ) ) + "brows.htm";
				dbMsg += ",url=" + urlStr;
				/*		int webWidth = webBrowser1.Width - 20;
						int webHeight = webBrowser1.Height - 40;
						dbMsg += ",web[" + webWidth + "×" + webHeight + "]";*/
				string[] extStrs = fileName.Split( '.' );
				string extentionStr = "." + extStrs[extStrs.Length - 1].ToLower();
				if (extentionStr == ".htm" ||
					extentionStr == ".html") {
					string titolStr = "webでHTMLを読み込みますか？";
					string msgStr = "組み込んであるScriptなどで異常終了する場合があります\n" +
						"「はい」　web表示\n" +
						"     　　　※異常終了する場合は読み込みを中断します。" +
						"「いいえ」ソースをテキストで表示\n" +
						"「キャンセル」読込み中止";
					DialogResult result = MessageBox.Show( msgStr, titolStr,
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Asterisk,
						MessageBoxDefaultButton.Button1 );                  //メッセージボックスを表示する
					if (result == DialogResult.Yes) {
						//「はい」が選択された時
						urlStr = fileName;
						Uri nextUri = new Uri( "file://" + urlStr );
						dbMsg += ",nextUri=" + nextUri;
						try {
							webBrowser1.ScriptErrorsSuppressed = true;      //
							webBrowser1.Navigate( nextUri );
						} catch (Exception e) {
							Console.WriteLine( TAG + "でエラー発生" + e.Message + ";" + dbMsg );
						}
					} else if (result == DialogResult.No) {
						//「いいえ」が選択された時
						MakeWebSouceBody( fileName );
					} else if (result == DialogResult.Cancel) {
						//「キャンセル」が選択された時
					}
				} else {
					MakeWebSouceBody( fileName );
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}//形式に合わせたhtml作成
		 /*		http://html5-css3.jp/tips/youtube-html5video-window.html
		  *		http://dobon.net/vb/dotnet/string/getencodingobject.html
		  */

		protected override void OnPaint(PaintEventArgs e)
		{
			string TAG = "[OnPaint]";
			string dbMsg = TAG;
			base.OnPaint( e );
			MakeWebSouce( passNameLabel.Text + Path.DirectorySeparatorChar + fileNameLabel.Text );
			MyLog( dbMsg );
		}           //リサイズ時の再描画

		private void ReSizeViews(object sender, EventArgs e)
		{
			string TAG = "[ReSizeViews]";
			string dbMsg = TAG;
			try {
				//		Size size = Form1.ScrollRectangle.Size; //webBrowser1.Document.Bodyだとerror! Body is null;
				//	var leftPWidth = 405;
				dbMsg += "[" + this.Width + "×" + this.Height + "]";
				dbMsg += ",leftTop=" + splitContainerLeftTop.Height + ",Center=" + splitContainerCenter.Height;
				//		splitContainer1.Panel1.Width = leftPWidth;
				//	splitContainerLeftTop.Height = 60;
				//	splitContainerCenter.Panel1.Height = this.Height-(60+80);            //_Panel2.
				//	splitContainerCenter.Panel2.Height = 80;            //_Panel2.
				//		splitContainerCenter.Width = leftPWidth;
				dbMsg += ">>2=" + splitContainerLeftTop.Height + ">>Center=" + splitContainerCenter.Height;
				MakeWebSouce( fileNameLabel.Text );
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}//表示サイズ変更

		//ファイルTree操作/////////////////////////////////////////////////////////////////////////////////
		//		System.IO.FileInfo fCpoy = null;
		//		System.IO.FileInfo fMove = null;

		/// <summary>
		///Nodeを書き直して再び開く ///////////////////////////////////
		/// </summary>
		public void ReExpandNode()
		{
			string TAG = "[ReExpandNode]";
			string dbMsg = TAG;
			try {
				string selectItem = fileTree.SelectedNode.FullPath;
				dbMsg += ",selectItem=" + selectItem;//
				string passNameStr = fileTree.SelectedNode.Parent.FullPath;
				dbMsg += ",passNameStr=" + passNameStr;
				//	if (File.Exists( selectItem )) {
				//		dbMsg += ",ファイルを選択";
				fileTree.SelectedNode.Parent.Collapse();                            //閉じて
				FolderItemListUp( passNameStr, fileTree.SelectedNode );      //TreeNodeを再構築して
				fileTree.SelectedNode.Expand();                             //開く
																			/*			} else if (Directory.Exists( selectItem )) {
																							dbMsg += ",フォルダを選択";
																						}
																						dbMsg += ",reDrowNode=" + reDrowNode.Name + ",passNameStr=" + passNameStr;
																						*/
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// 指定されたものを削除
		/// </summary>
		/// <param name="sourceName">ファイルもしくはフォルダ名</param>
		/// <param name="isTrash">trueでゴミ箱　/　falseで完全削除</param>
		public void DelFiles(string sourceName, bool isTrash)
		{
			string TAG = "[DelFiles]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName + "を削除;isTrash=" + isTrash;
				Microsoft.VisualBasic.FileIO.RecycleOption recycleOption = RecycleOption.DeletePermanently;         //ファイルまたはディレクトリを完全に削除します。 既定モード。
				元に戻す.Visible = false;
				if (isTrash) {
					recycleOption = RecycleOption.SendToRecycleBin;                                                //ファイルまたはディレクトリの送信、 ごみ箱します。
																												   //			元に戻す.Visible = true;
				}
				if (File.Exists( sourceName )) {
					dbMsg += ",ファイルを選択";
					FileSystem.DeleteFile( sourceName, UIOption.AllDialogs, recycleOption, UICancelOption.DoNothing );
					//もしくは	System.IO.File.Delete( sourceName );             //フォルダ"C:\TEST"を削除する
				} else if (Directory.Exists( sourceName )) {
					dbMsg += ",フォルダを選択";
					FileSystem.DeleteDirectory( sourceName, UIOption.AllDialogs, recycleOption, UICancelOption.DoNothing );
					//もしくは	System.IO.Directory.Delete( sourceName, true );   //true;エラーを無視して削除？
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
			//https://dobon.net/vb/dotnet/file/directorycreate.html
		}

		public void MoveMyFile(string sourceName, string destName)
		{
			string TAG = "[MoveMyFile]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName + ",先=" + destName;
				System.IO.FileInfo fi = new System.IO.FileInfo( sourceName );   //変更元のFileInfoのオブジェクトを作成します。 @"C:\files1\sample1.txt" 
				fi.MoveTo( destName );                                           //MoveToメソッドで移動先を指定してファイルを移動します。@"D:\files2\sample2.txt"
																				 // http://www.openreference.org/articles/view/329
																				 //	fi = null;
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
				throw new NotImplementedException();//要求されたメソッドまたは操作が実装されない場合にスローされる例外。
			}
		}

		public void MoveFolder(string sourceName, string destName)
		{
			string TAG = "[MoveFolder]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName + ",先=" + destName;
				//https://dobon.net/vb/dotnet/file/directorycreate.html
				/*			string[] dirs = System.IO.Directory.GetFiles( sourceName, "*", System.IO.SearchOption.AllDirectories );
								dbMsg += ",中身は" + dirs.Length + "フォルダ" ;
								foreach (string dir in dirs) {
									string newSouce = dir;
									dbMsg += "," + newSouce;
								}*/

				//Directoryクラスを使用する方法;中身移動せず
				/*			System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory( destName );   //フォルダ"C:\TEST\SUB"を作成する
							System.IO.Directory.Move( sourceName, destName );               //フォルダ"C:\1"を"C:\2\SUB"に移動（名前を変更）する
							string[] files = System.IO.Directory.GetFiles( di.FullName, "*", System.IO.SearchOption.AllDirectories );
							dbMsg += ",di（" + di.FullName + "）に" + files.Length + "件";//
							System.IO.Directory.Delete( sourceName, true );             //フォルダ"C:\TEST"を削除する
							*/
				//DirectoryInfoクラスを使用する方法;中身移動せず
				/*		System.IO.DirectoryInfo di = new System.IO.DirectoryInfo( sourceName ); //@"C:\TEST\SUB"；DirectoryInfoオブジェクトを作成する
						string[] files = System.IO.Directory.GetFiles( di.FullName, "*", System.IO.SearchOption.AllDirectories );
						dbMsg += ",di（" + di.FullName + "）に" + files.Length + "件";//
						di.Create();                                                           //フォルダ"C:\TEST\SUB"を作成する
						System.IO.DirectoryInfo subDir = di.CreateSubdirectory( "1" );     //サブフォルダを作成する☆subDirには、フォルダ"C:\TEST\SUB\1"のDirectoryInfoオブジェクトが入る
						files = System.IO.Directory.GetFiles( subDir.FullName, "*", System.IO.SearchOption.AllDirectories );
						dbMsg += ",di（" + subDir.FullName + "）に" + files.Length + "件";
						subDir.MoveTo( destName );                                           //フォルダ"C:\TEST\SUB\1"を"C:\TEST\SUB\2"に移動する☆subDirの内容は、"C:\TEST\SUB\2"のものに変わる
						di.Delete( true );                                                  //フォルダ"C:\TEST\SUB"を根こそぎ削除する☆trueにしないと中身が有った場合にエラー発生
						*/

				//FileSystemを使用:参照設定に"Microsoft.VisualBasic.dll"が追加されている必要がある
				FileSystem.CreateDirectory( destName );                     //フォルダdestを作成する
				string[] dirs = System.IO.Directory.GetFiles( destName, "*", System.IO.SearchOption.AllDirectories );
				dbMsg += ",中身は" + dirs.Length + "フォルダ";
				FileSystem.MoveDirectory( sourceName, destName, true );    //sourceをdestに移動する☆第3項にTrueを指定すると、destが存在する時、上書きする
																		   //		FileSystem.MoveDirectory( sourceName, destName, UIOption.AllDialogs, UICancelOption.DoNothing );//sourceをdestに移動する
																		   //進行状況ダイアログとエラーダイアログを表示する☆ユーザーがキャンセルしても例外OperationCanceledExceptionをスローしない
				dirs = System.IO.Directory.GetFiles( destName, "*", System.IO.SearchOption.AllDirectories );
				dbMsg += ">>" + dirs.Length + "件";
				DelFiles( sourceName, false );
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
			//https://dobon.net/vb/dotnet/file/directorycreate.html
		}


		public void TargetReName(string destName)
		{
			string TAG = "[TargetReName]";
			string dbMsg = TAG;
			try {
				dbMsg += " , destName=" + destName;
				//		TreeNode selectNode = fileTree.SelectedNode;
				string selectItem = fileNameLabel.Text + "";
				string passNameStr = passNameLabel.Text + "";
				dbMsg += " , passNameStr=" + passNameStr;
				if (passNameStr != selectItem) {                        //ドライブ選択でなければ
					dbMsg += " , selectItem=" + selectItem;
					if (selectItem != passNameLabel.Text) {
						selectItem = passNameLabel.Text + Path.DirectorySeparatorChar + selectItem;
						dbMsg += ">>" + selectItem;  // selectItem=media2.flv>>M:\sample/media2.flv,選択；ペースト,
					}
					string titolStr = selectItem + "の名称変更";
					string msgStr = "元の名称\n" + selectItem;
					dbMsg += ",titolStr=" + titolStr + ",msgStr=" + msgStr;

					InputDialog f = new InputDialog( msgStr, titolStr, destName );
					if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						destName = f.ResultText;
						dbMsg += ",元=" + selectItem + ",先=" + destName;
						string renewName = passNameLabel.Text + Path.DirectorySeparatorChar + destName;
						if (File.Exists( selectItem )) {
							dbMsg += ">>ファイル名変更>" + renewName;
							MoveMyFile( selectItem, renewName );
						} else if (Directory.Exists( selectItem )) {
							dbMsg += ">>フォルダ名変更>" + renewName;
							MoveFolder( selectItem, renewName );
						}
						//  https://dobon.net/vb/dotnet/control/tvlabeledit.html
					} else {
						dbMsg += ">>Cancel";
					}
				} else {
					string titolStr = selectItem + "の名称は変更できません";
					string msgStr = "ドライブ名称は変更できません";
					DialogResult result = MessageBox.Show( msgStr, titolStr,
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation,
						MessageBoxDefaultButton.Button1 );                  //メッセージボックスを表示する
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// ファイルもしくはフォルダをコピーする
		/// </summary>
		/// <param name="sourceName">コピー元</param>
		/// <param name="destName">コピー先</param>
		public void FilesCopy(string sourceName, string destName)
		{
			string TAG = "[FilesCopy]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName + ",先=" + destName;
				string[] souceNames = sourceName.Split( Path.DirectorySeparatorChar );
				string souceEnd = souceNames[souceNames.Length - 1];
				destName += Path.DirectorySeparatorChar + souceEnd;
				dbMsg += ">>" + destName;
				if (File.Exists( sourceName )) {
					dbMsg += ">>ファイル";
					if (File.Exists( destName )) {
						string[] extStrs = destName.Split( '.' );
						souceEnd = extStrs[extStrs.Length - 2];
						destName = destName + "のコピー." + extStrs[extStrs.Length - 1];
						dbMsg += ">>" + destName;
					}
					//		FileSystem.CopyFile( sourceName, destName );                  //"C:\test\1.txt"を"C:\test\2.txt"にコピーする
					//		FileSystem.CopyFile( sourceName, destName, true );	//"C:\test\2.txt"がすでに存在している場合は、これを上書きする
					//		FileSystem.CopyFile( sourceName, destName, UIOption.OnlyErrorDialogs );                    //エラーの時、ダイアログを表示する
					//		FileSystem.CopyFile( sourceName, destName,UIOption.AllDialogs );                    //進行状況ダイアログと、エラーダイアログを表示する
					FileSystem.CopyFile( sourceName, destName, UIOption.AllDialogs, UICancelOption.DoNothing );
					//進行状況ダイアログやエラーダイアログでキャンセルされても例外をスローしない
					//UICancelOption.DoNothingを指定しないと、例外OperationCanceledExceptionが発生
				} else if (Directory.Exists( sourceName )) {
					dbMsg += ">>フォルダ";
					if (Directory.Exists( destName )) {
						destName = destName + "のコピー";
						dbMsg += ">>" + destName;
					}
					FileSystem.CopyDirectory( sourceName, destName, UIOption.AllDialogs, UICancelOption.DoNothing );
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// ファイルもしくはフォルダをコピーする
		/// </summary>
		/// <param name="sourceName">コピー元</param>
		/// <param name="destName">コピー先</param>
		public void FilesMove(string sourceName, string destName)
		{
			string TAG = "[FilesMove]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName + ",先=" + destName;
				string[] souceNames = sourceName.Split( Path.DirectorySeparatorChar );
				string souceEnd = souceNames[souceNames.Length - 1];
				destName += Path.DirectorySeparatorChar + souceEnd;
				dbMsg += ">>" + destName;
				if (File.Exists( sourceName )) {
					dbMsg += ">>ファイル";
					MoveMyFile( sourceName, destName );
				} else if (Directory.Exists( sourceName )) {
					dbMsg += ">>フォルダ";
					MoveFolder( sourceName, destName );
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		public static void CopyDirectory(string sourceDirName, string destDirName)
		{
			string TAG = "[CopyDirectory]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceDirName + ",先=" + destDirName;
				if (!System.IO.Directory.Exists( destDirName )) {                                                           //コピー先のディレクトリがないときは
					System.IO.Directory.CreateDirectory( destDirName );                                                      //作る
					System.IO.File.SetAttributes( destDirName, System.IO.File.GetAttributes( sourceDirName ) );              //属性もコピー
				}

				if (destDirName[destDirName.Length - 1] != System.IO.Path.DirectorySeparatorChar) {
					destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;                                      //コピー先のディレクトリ名の末尾に"\"をつける
				}

				string[] files = System.IO.Directory.GetFiles( sourceDirName );
				foreach (string file in files) {
					System.IO.File.Copy( file, destDirName + System.IO.Path.GetFileName( file ), true );                     //コピー元のディレクトリにあるファイルをコピー
				}

				string[] dirs = System.IO.Directory.GetDirectories( sourceDirName );
				foreach (string dir in dirs) {
					CopyDirectory( dir, destDirName + System.IO.Path.GetFileName( dir ) );          //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
				}

				//		MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
			}
		}

		/// <summary>
		/// コピーかカットかを判定してペースト動作へ
		/// </summary>
		/// <param name="copySouce"></param>
		/// <param name="cutSouce"></param>
		/// <param name="peastFor"></param>
		public void PeastSelecter(string copySouce, string cutSouce, string peastFor)
		{
			string TAG = "[PeastSelecter]";
			string dbMsg = TAG;
			try {
				dbMsg += ",copy=" + copySouce + ",cut=" + cutSouce + ",先=" + peastFor;
				/*	string moveFolder = passNameLabel.Text + Path.DirectorySeparatorChar;// + Path.AltDirectorySeparatorChar + selectItem;
					dbMsg += ",ペースト先=" + moveFolder;       //ペースト先=M:\sample
																		if (fCpoy != null) {
																			   fileCopyProcess( fCpoy, moveFolder );
																		   } else if (fMove != null) {
																			   fileCutProcess( fMove, moveFolder );
																		   }*/
				MyLog( dbMsg );
				if (copySouce != "") {
					FilesCopy( copySouce, peastFor );
					//		copySouce = "";
				} else if (cutSouce != "") {
					FilesMove( cutSouce, peastFor );
					cutSouce = "";
				}
				コピーToolStripMenuItem.Visible = true;
				カットToolStripMenuItem.Visible = true;
				ペーストToolStripMenuItem.Visible = false;
				ReExpandNode();
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/*		private void fileCopyProcess(System.IO.FileInfo copyFi, string moveFolder)
				{
					string TAG = "[fileCopyProcess]";
					string dbMsg = TAG;
					try {
						string motoName = copyFi.Name;
						dbMsg += ",motoName=" + motoName;     //motoName=media2.flv
						if (-1 < copyFi.Length) {
							string sakiFolder = copyFi.FullName.Replace( motoName, "" );
							dbMsg += ",sakiFolder=" + sakiFolder;   //sakiFolder=M:\sample\
							if (moveFolder == sakiFolder) {
								string[] extStrs = motoName.Split( '.' );
								motoName = extStrs[extStrs.Length - 2];
								string extentionStr = "." + extStrs[extStrs.Length - 1];
								motoName = motoName + "のコピー" + extentionStr;
								dbMsg += ">>" + motoName;
							}
							System.IO.FileInfo copyFile = copyFi.CopyTo( moveFolder + Path.DirectorySeparatorChar + motoName );
							//"C:\test\1.txt"を"@"C:\test\2.txt"にコピーする	copyFileには、コピー先のファイルを表すFileInfoオブジェクトが入る
						} else {
							CopyDirectory( motoName, moveFolder );
						}
						copyFi = null;

						MyLog( dbMsg );
					} catch (Exception er) {
						Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
					}

				}
		*/
		/*		private void fileCutProcess(System.IO.FileInfo fMove, string moveFolder)
				{
					string TAG = "[fileCutProcess]";
					string dbMsg = TAG;
					try {
						string motoName = fMove.Name;
						dbMsg += ",motoName=" + motoName;
						string sakiFolder = fMove.FullName.Replace( motoName, "" );
						dbMsg += ",sakiFolder=" + sakiFolder;
						if (moveFolder == sakiFolder) {
							string titolStr = "同じフォルダです。";
							string msgStr = "移動先のフォルダを選択して下さい。";
							DialogResult result = MessageBox.Show( msgStr, titolStr,
								MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation,
								MessageBoxDefaultButton.Button1 );                  //メッセージボックスを表示する
						} else {
							fMove.MoveTo( moveFolder + Path.DirectorySeparatorChar + motoName );
							//"C:\test\1.txt"を"C:\test\3.txt"に移動する	//fiは、移動先のファイルを表すFileInfoに変わる
							fMove = null;
						}

						MyLog( dbMsg );
					} catch (Exception er) {
						Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
					}
				}
		*/

		/// <summary>
		/// 割り付けられたアプリケーションを起動する
		/// </summary>
		/// <param name="sourceName"></param>
		public void SartApication(string sourceName)
		{
			string TAG = "[SartApication]";
			string dbMsg = TAG;
			try {
				dbMsg += ",元=" + sourceName;
				System.Diagnostics.Process p = System.Diagnostics.Process.Start( sourceName );
				dbMsg += ",MainWindowTitle=" + p.MainWindowTitle;
				dbMsg += ",ModuleName=" + p.MainModule.ModuleName;
				dbMsg += ",ProcessName=" + p.ProcessName;
				MyLog( dbMsg );                                             //何故かここのLogが出ない
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// ファイルリストの右クリックで開くコンテキストメニューの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string TAG = "[contextMenuStrip1_ItemClicked]";
			string dbMsg = TAG;
			try {
				//			dbMsg += "sender=" + sender;                    //sender=	常にSystem.Windows.Forms.TreeView, Nodes.Count: 5, Nodes[0]: TreeNode: C:\,
				//			dbMsg += ",e=" + e;                             //e=		常にSystem.Windows.Forms.TreeViewEventArgs,
				dbMsg += ",ClickedItem=" + e.ClickedItem.Name;                             //e=		常にSystem.Windows.Forms.TreeViewEventArgs,
				string clickedMenuItem = e.ClickedItem.Name.Replace( "ToolStripMenuItem", "" );
				dbMsg += ">>" + clickedMenuItem;               // Name: contextMenuStrip1, Items: 7,e=System.Windows.Forms.ToolStripItemClickedEventArgs,ClickedItem=ペーストToolStripMenuItem>>ペーストToolStripMenuItem ,
				TreeNode selectNode = fileTree.SelectedNode;
				string selectItem = selectNode.Text;
				dbMsg += " , selectItem=" + selectItem;
				if (selectItem != passNameLabel.Text) {
					selectItem = passNameLabel.Text + Path.DirectorySeparatorChar + selectItem;
					dbMsg += ">>" + selectItem;  // selectItem=media2.flv>>M:\sample/media2.flv,選択；ペースト,
				}
				string destDirName = selectItem + Path.DirectorySeparatorChar + "新しいフォルダ";
				string selectFullName = selectNode.FullPath;
				switch (clickedMenuItem) {                                           // クリックされた項目の Name を判定します。 
					case "フォルダ作成":
					dbMsg += ",選択；フォルダ作成=" + destDirName;
					System.IO.Directory.CreateDirectory( destDirName );
					break;

					case "名称変更":
					dbMsg += ",選択；名称変更=" + destDirName;
					TargetReName( selectNode.Text );
					ReExpandNode();
					break;

					case "カット":
					cutSouce = selectItem;
					dbMsg += ",選択；カット" + cutSouce;
					break;

					case "コピー":
					copySouce = selectItem;
					dbMsg += ",選択；コピー" + copySouce;
					break;

					case "ペースト":
					dbMsg += ",選択；ペースト";
					PeastSelecter( copySouce, cutSouce, selectFullName );
					break;

					case "削除":
					dbMsg += ",選択；削除;" + selectFullName;
					DelFiles( selectFullName, true );
					ReExpandNode();
					break;

					case "元に戻す":
					dbMsg += ",選択；元に戻す";
					元に戻す.Visible = false;
					break;

					case "他のアプリケーションで開く":
					dbMsg += ",選択；他のアプリケーションで開く";
					SartApication( selectItem );
					break;
					default:
					break;
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}

		}

		string beforStr = "";
		private void fileTree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			string TAG = "[fileTree_BeforeLabelEdit]";
			string dbMsg = TAG;
			try {
				if (e.Node.Parent == null) {
					e.CancelEdit = true;        //ルートのコードは編集できないようにする
				} else {
					beforStr = e.Node.Text;
					dbMsg += " , beforStr=" + beforStr;
					string[] extStrs = beforStr.Split( '.' );
					dbMsg += " , " + extStrs.Length + "分割";
					if (1 < extStrs.Length) {
						string motoName = extStrs[extStrs.Length - 2];
						int reSelectEnd = motoName.Length;
						dbMsg += " , reSelectEnd=" + reSelectEnd + "まで";
					}
					TargetReName( beforStr );
				}
				ReExpandNode();
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
				//		throw new NotImplementedException();//要求されたメソッドまたは操作が実装されない場合にスローされる例外。
			}
		}

		private void fileTree1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			string TAG = "[fileTree1_AfterLabelEdit]";
			string dbMsg = TAG;
			try {
				if (e.Label != null) {                                                       //ラベルが変更されたか調べる	//e.Labelがnullならば、変更されていない
					if (e.Node.Parent != null) {                                             //同名のノードが同じ親ノード内にあるか調べる
						foreach (TreeNode n in e.Node.Parent.Nodes) {
							if (n != e.Node && n.Text == e.Label) {                         //同名のノードがあるときは
								MessageBox.Show( "同名のノードがすでにあります。" );
								e.CancelEdit = true;                                       //編集をキャンセルして元に戻す
								return;
								/*			} else {
												//		TreeNode selectNode = fileTree.SelectedNode;
												string selectItem = fileNameLabel.Text + "";
												string passNameStr = passNameLabel.Text + "";
												if (passNameStr != selectItem) {                        //ドライブ選択でなければ
													dbMsg += " , selectItem=" + selectItem;
													if (selectItem != passNameLabel.Text) {
														selectItem = passNameLabel.Text + Path.DirectorySeparatorChar + selectItem;
														dbMsg += ">>" + selectItem;  // selectItem=media2.flv>>M:\sample/media2.flv,選択；ペースト,
													}
													string renewName = passNameLabel.Text + Path.DirectorySeparatorChar + e.Label;
													if (File.Exists( selectItem )) {
														dbMsg += ">>ファイル名変更>" + renewName;
														MoveMyFile( selectItem, renewName );
													} else if (Directory.Exists( selectItem )) {
														dbMsg += ">>フォルダ名変更>" + renewName;
														MoveFolder( selectItem, renewName );
													}
													//  https://dobon.net/vb/dotnet/control/tvlabeledit.html
													fileTree.SelectedNode.Collapse();                            //閉じて
													FolderItemListUp( selectItem, fileTree.SelectedNode );      //TreeNodeを再構築して
													fileTree.SelectedNode.Expand();                             //開く
												}*/
							}
						}
					}
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
				throw new NotImplementedException();//要求されたメソッドまたは操作が実装されない場合にスローされる例外。
			}
		}

		private void fileTree_KeyUp(object sender, KeyEventArgs e)
		{
			string TAG = "[fileTree_KeyUp]";
			string dbMsg = TAG;
			try {
				TreeView tv = ( TreeView ) sender;
				if (e.KeyCode == Keys.F2 && tv.SelectedNode != null && tv.LabelEdit) {              //F2キーが離されたときは、フォーカスのあるアイテムの編集を開始
					tv.SelectedNode.BeginEdit();
				}
			} catch (Exception er) {
				Console.WriteLine( TAG + "でエラー発生" + er.Message + ";" + dbMsg );
				throw new NotImplementedException();//要求されたメソッドまたは操作が実装されない場合にスローされる例外。
			}
		}


		/// <summary>
		/// fileTreeのノードがドラッグされた時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeView1_ItemDrag(object sender, ItemDragEventArgs e)
		{
			string TAG = "[TreeView1_ItemDrag]";
			string dbMsg = TAG;
			try {
				cutSouce = "";       //カットするアイテムのurl
				copySouce = "";      //コピーするアイテムのurl
				TreeView tv = ( TreeView ) sender;
				tv.SelectedNode = ( TreeNode ) e.Item;
				dbMsg += " ,SelectedNode=" + tv.SelectedNode.FullPath;
				tv.Focus();
				DragDropEffects dde = tv.DoDragDrop( e.Item, DragDropEffects.All );
				dbMsg += "のドラッグを開始";
				if (( dde & DragDropEffects.Move ) == DragDropEffects.Move) {
					cutSouce = tv.SelectedNode.FullPath;       //カットするアイテムのurl
					dbMsg += " , 移動した時は、ドラッグしたノードを削除";
					tv.Nodes.Remove( ( TreeNode ) e.Item );
				}
				dbMsg += ",copy=" + copySouce + ",cut=" + cutSouce;
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// ドラッグしている時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeView1_DragOver(object sender, DragEventArgs e)
		{
			string TAG = "[TreeView1_DragOver]";
			string dbMsg = TAG;
			try {
				TreeView tv = ( TreeView ) sender;
				dbMsg += " ,SelectedNode=" + tv.SelectedNode.FullPath;
				if (e.Data.GetDataPresent( typeof( TreeNode ) )) {              //ドラッグされているデータがTreeNodeか調べる
					if (( e.KeyState & 8 ) == 8 && ( e.AllowedEffect & DragDropEffects.Copy ) == DragDropEffects.Copy) {
						dbMsg += " , Ctrlキーが押されている>>Copy";//Ctrlキーが押されていればCopy//"8"はCtrlキーを表す
						copySouce = tv.SelectedNode.FullPath;      //コピーするアイテムのurl
						e.Effect = DragDropEffects.Copy;
					} else if (( e.AllowedEffect & DragDropEffects.Move ) == DragDropEffects.Move) {
						dbMsg += " , 何も押されていない>>Move";
						cutSouce = tv.SelectedNode.FullPath;     //カットするアイテムのurl
						e.Effect = DragDropEffects.Move;
					} else {
						cutSouce = tv.SelectedNode.FullPath;     //カットするアイテムのurl
						e.Effect = DragDropEffects.None;
					}
				} else {
					dbMsg += " ,TreeNodeでなければ受け入れない";
					e.Effect = DragDropEffects.None;
					if (e.Effect != DragDropEffects.None) {                 //マウス下のNodeを選択する
						TreeNode target = tv.GetNodeAt( tv.PointToClient( new Point( e.X, e.Y ) ) );             //マウスのあるNodeを取得する
						TreeNode source = ( TreeNode ) e.Data.GetData( typeof( TreeNode ) );                     //ドラッグされているNodeを取得する
						if (target != null && target != source && !IsChildNode( source, target )) {             //マウス下のNodeがドロップ先として適切か調べる
							if (target.IsSelected == false) {                                                   //Nodeを選択する
								tv.SelectedNode = target;
							}
						} else {
							e.Effect = DragDropEffects.None;
						}
					}
				}
				dbMsg += ",copy=" + copySouce + ",cut=" + cutSouce;
				//		MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// ドロップされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeView1_DragDrop(object sender, DragEventArgs e)
		{
			string TAG = "[TreeView1_DragDrop]";
			string dbMsg = TAG;
			try {
				if (e.Data.GetDataPresent( typeof( TreeNode ) )) {                              //ドロップされたデータがTreeNodeか調べる
					TreeView tv = ( TreeView ) sender;
					dbMsg += " Drop先は" + tv.SelectedNode.FullPath;
					TreeNode source = ( TreeNode ) e.Data.GetData( typeof( TreeNode ) );         //ドロップされたデータ(TreeNode)を取得
					TreeNode target = tv.GetNodeAt( tv.PointToClient( new Point( e.X, e.Y ) ) ); //ドロップ先のTreeNodeを取得する
					string dropSouce = target.FullPath;
					if (target != null && target != source && !IsChildNode( source, target )) { //マウス下のNodeがドロップ先として適切か調べる
						dbMsg += ",copy=" + copySouce + ",cut=" + cutSouce + ",peast先=" + dropSouce;
						PeastSelecter( copySouce, cutSouce, dropSouce );
						/*表示だけの書き換えなら
							TreeNode cln = ( TreeNode ) source.Clone();                             //ドロップされたNodeのコピーを作成
							target.Nodes.Add( cln );												//Nodeを追加
							target.Expand();														//ドロップ先のNodeを展開
							tv.SelectedNode = cln;                                                  //追加されたNodeを選択
						*/
					} else {
						e.Effect = DragDropEffects.None;
					}
				} else {
					e.Effect = DragDropEffects.None;
				}
				MyLog( dbMsg );
			} catch (Exception er) {
				dbMsg += "でエラー発生" + er.Message;
				MyLog( dbMsg );
			}
		}

		/// <summary>
		/// あるTreeNodeが別のTreeNodeの子ノードか調べる
		/// </summary>
		/// <param name="parentNode">親ノードか調べるTreeNode</param>
		/// <param name="childNode">子ノードか調べるTreeNode</param>
		/// <returns>子ノードの時はTrue</returns>
		private static bool IsChildNode(TreeNode parentNode, TreeNode childNode)
		{
			string TAG = "[IsChildNode]";
			string dbMsg = TAG;
			//			try {

			if (childNode.Parent == parentNode) {
				return true;
			} else if (childNode.Parent != null) {
				return IsChildNode( parentNode, childNode.Parent );
			} else {
				return false;
			}
			/*	//		MyLog( dbMsg );
					} catch (Exception er) {
						dbMsg += "でエラー発生" + er.Message;
				//		MyLog( dbMsg );
					}*/
		}


		///時計表示////////////////////////////////////////////////////////////////////
		private void timer1_Tick(object sender, EventArgs e)
		{
			SetDisplayTime();
		}

		private void SetDisplayTime()
		{
			timeNow.Text = DateTime.Now.ToString( "HH時mm分 ss秒" );
		}
		//デバッグツール///////////////////////////////////////////////////////////その他//
		Boolean debug_now = true;
		private void MyLog(string msg)
		{
			if (debug_now) {
				Console.WriteLine( msg );
			}
		}
	}
}
