//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;
using DSF_NET_Profiler;
using DSF_NET_Utility;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	CGeoViewer_WP	GeoViewer_WP;
	CGeoViewer_LgLt	GeoViewer_LgLt; 
	CXYZPlaneViewer XYZPlaneViewer; 

	CPlaneViewer Viewer = null;

	readonly string CfgFileName;

	CInfoMap  Info	   = new CInfoMap ();
	CProfiler Profiler = new CProfiler();

	public PlaneViewerMainForm(string[] args)
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
			//Run_XYZPlaneViewer(); // 単純なXYZプレーン
		}
		catch(System.Xml.XPath.XPathException ex)
		{
			MessageListBox.Items.Add("XPath Error > " + ex.Message);
		}
		catch(System.NullReferenceException ex)
		{
			MessageListBox.Items.Add("Error > " + ex.Message);
		}
		catch(Exception ex)
		{
		//	MessageListBox.Items.Add("Error > " + ex.StackTrace);
			MessageListBox.Items.Add("Error > " + ex.Message);
		}
	}

	private void DisplayLog(in CLgLt s_lglt, in CLgLt e_lglt)
	{
		//--------------------------------------------------
		// プレーンサイズを計算する。

		var lglt_00 = s_lglt;
		var lglt_10 = new CLgLt(e_lglt.Lg, s_lglt.Lt);
		var lglt_01 = new CLgLt(s_lglt.Lg, e_lglt.Lt);

		var coord_00 = ToGeoCentricCoord(lglt_00);
		var coord_10 = ToGeoCentricCoord(lglt_10);
		var coord_01 = ToGeoCentricCoord(lglt_01);

		var dx_00_10 = coord_10.X - coord_00.X;
		var dy_00_10 = coord_10.Y - coord_00.Y;
		var dz_00_10 = coord_10.Z - coord_00.Z;

		var plane_size_EW = (int)(Math.Sqrt(dx_00_10 * dx_00_10 + dy_00_10 * dy_00_10 + dz_00_10 * dz_00_10));

		var dx_00_01 = coord_01.X - coord_00.X;
		var dy_00_01 = coord_01.Y - coord_00.Y;
		var dz_00_01 = coord_01.Z - coord_00.Z;

		var plane_size_NS = (int)(Math.Sqrt(dx_00_01 * dx_00_01 + dy_00_01 * dy_00_01 + dz_00_01 * dz_00_01));

		//--------------------------------------------------

		var info_dictionary = Info.ToDictionary();

		var vert_nx = info_dictionary["VertexNX"];
		var vert_ny = info_dictionary["VertexNY"];

		MessageListBox.Items.Add($"          頂点数 : {vert_nx} x {vert_ny}");
		MessageListBox.Items.Add($"      ポリゴン数 : {(vert_nx - 1) * (vert_ny - 1) * 2}");
		MessageListBox.Items.Add($"テクスチャサイズ : {info_dictionary["TexW"]}pix x {info_dictionary["TexH"]}pix");
		MessageListBox.Items.Add($"  テクスチャ枚数 : {info_dictionary["TexN"]}");
		MessageListBox.Items.Add($"  表示地域サイズ : {plane_size_EW}m x {plane_size_NS}m");
		MessageListBox.Items.Add($"  ポリゴンサイズ : {plane_size_EW / (vert_nx - 1)}m x {plane_size_NS / (vert_ny - 1)}m");
		MessageListBox.Items.Add($"");

		//--------------------------------------------------

		// ◆このメソッドに入る前にStopするべきでは？
		Profiler.Stop();

		MessageListBox.Items.Add("elapsed time   memory delta");

		var total_time = Profiler.TotalTime;

		foreach(var profile in Profiler.Profiles)
		{
			var laptime = profile.Value.LapTime;

			var laptime_percentage = ToDouble(laptime) / total_time * 100;

			MessageListBox.Items.Add($"{laptime, 5}ms ({laptime_percentage, 4:0.0}%) {profile.Value.MemDelta.PhysMem / 1000.0, 9:#,###,###}KB : {profile.Key}");
		}

		MessageListBox.Items.Add($"total {total_time}ms   {Profiler.TotalMem.PhysMem / 1000.0, 9:#,###,###}KB");
	}
}
//---------------------------------------------------------------------------
}
