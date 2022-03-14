//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
using DSF_NET_Profiler;
using DSF_NET_Utility;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
	public CGeoViewer Viewer = null;

	readonly string CfgFileName;

	string MapDrawingFileName = null;

	CInfoMap  Info	   = new CInfoMap ();
	CProfiler Profiler = new CProfiler();

	List<string> cmd_history = new List<string>();

	int curr_cmd_history_pos;

	public GeoViewerMainForm(string[] args)
	{
		InitializeComponent();

		CfgFileName = args[0];
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		// ◆Form_Loadだと、このフォームは最後まで表示されない
		// 　非同期にできないか？すべきか？

		try
		{
			// ◆どれか一つ選んで実行
			Run_GeoViewer_WP(); // ワールドピクセル単位の地形表示
			//Run_GeoViewer_Tile(); // タイル単位の地形表示
			//Run_GeoViewer_LgLt(); // 経緯度単位の地形表示
		}
		catch(System.Xml.XPath.XPathException ex)
		{
			DialogTextBox.AppendText("XPath Error : " + ex.Message + "\r\n");
		}
		catch(System.NullReferenceException ex)
		{
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
		catch(Exception ex)
		{
		//	InputTextBox.AppendText("Error : " + ex.StackTrace + "\r\n");
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
	}

	private void InputTextBox_KeyPress(Object sender, KeyPressEventArgs e)
	{
		// ◆プロンプト(>)の後で改行しないよう、Enterを打ち消してプロンプトに置き換える必要があるが、これができるのはなぜかKeyPressイベントだけ。

		var tb = sender as TextBox;
			
		switch(e.KeyChar)
		{
			case '\r': // Enter

				tb.AppendText("\r\n");

				var cmd_line = tb.Lines[tb.Lines.Length - 2].Substring(1);

				if(cmd_line != "")
				{
					if((cmd_history.Count == 0) || (cmd_history[cmd_history.Count - 1] != cmd_line))
						cmd_history.Add(cmd_line);

					curr_cmd_history_pos = cmd_history.Count - 1;
						
					var ret = ParseCommand(cmd_line);

					// ◆複数行の文字列が返ることもあるので改行も含めておく。
					if(ret != "") tb.AppendText(ret);
				}

				// ◆プロンプト(>)の後で改行しないようEnterを打ち消してプロンプトに置き換える。
				e.KeyChar = '>';

				break;

			case '\b': // Backspace

				// ◆プロンプト(>)を残すようにする。
				// 　何もしないので空白('')を設定したいが、設定できないので'\0'を設定する。
				if(tb.Lines[tb.Lines.Length - 1].Length == 1) e.KeyChar = '\0';

				break;
		}
	}

	private void InputTextBox_KeyDown(Object sender, KeyEventArgs e)
	{
		//--------------------------------------------------

		if(e.KeyCode == Keys.Escape) Application.Exit();		

		//--------------------------------------------------
		// 矢印キーでカーソルがテキストの末尾以外のところに行かないように止める。
		// ◆KeyPressイベントは矢印キー等では発生しない。
		if((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left)) e.SuppressKeyPress = true;

		//--------------------------------------------------
		// コマンド履歴を入力する。

		if(cmd_history.Count == 0) return;

		if((e.KeyCode != Keys.Up) && (e.KeyCode != Keys.Down)) return;

		var tb = sender as TextBox;

		tb.Text = tb.Text.Remove(tb.Text.LastIndexOf('>'));

		tb.AppendText(">" + cmd_history[curr_cmd_history_pos]);

		switch(e.KeyCode)
		{ 
			case Keys.Up:
				if(curr_cmd_history_pos > 0) curr_cmd_history_pos--;
				break;

			case Keys.Down:
				if(curr_cmd_history_pos < (cmd_history.Count - 1)) curr_cmd_history_pos++;
				break;
		} 
	}

	private void InputTextBox_MouseDown(Object sender, MouseEventArgs e)
	{
		// マウスクリックでカーソルがテキストの末尾以外のところに行かないようにする。
		((TextBox)sender).AppendText("\0");
	}

	private void InputTextBox_MouseMove(Object sender, MouseEventArgs e)
	{
		// マウスによる範囲選択でカーソルがテキストの末尾以外のところに行かないようにする。範囲選択も解除される。
		if(e.Button == MouseButtons.Left) ((TextBox)sender).AppendText("\0");
	}
}
//---------------------------------------------------------------------------
}
