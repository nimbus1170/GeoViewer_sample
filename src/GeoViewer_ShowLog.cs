//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;

using static DSF_CS_Profiler.CProfilerLog;

using System.Windows.Forms;

using static System.Math;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	private void ShowLog()
	{
		//--------------------------------------------------
		// プレーンサイズを計算する。

		var lglt_00 = StartLgLt;
		var lglt_10 = new CLgLt(EndLgLt  .Lg, StartLgLt.Lt);
		var lglt_01 = new CLgLt(StartLgLt.Lg, EndLgLt  .Lt);

		var coord_00 = ToGeoCentricCoord(lglt_00);
		var coord_10 = ToGeoCentricCoord(lglt_10);
		var coord_01 = ToGeoCentricCoord(lglt_01);

		var dx_00_10 = coord_10.X - coord_00.X;
		var dy_00_10 = coord_10.Y - coord_00.Y;
		var dz_00_10 = coord_10.Z - coord_00.Z;

		// ◆C#にはHypotがない。
		var plane_size_EW = (int)(Sqrt(dx_00_10 * dx_00_10 + dy_00_10 * dy_00_10 + dz_00_10 * dz_00_10));

		var dx_00_01 = coord_01.X - coord_00.X;
		var dy_00_01 = coord_01.Y - coord_00.Y;
		var dz_00_01 = coord_01.Z - coord_00.Z;

		var plane_size_NS = (int)(Sqrt(dx_00_01 * dx_00_01 + dy_00_01 * dy_00_01 + dz_00_01 * dz_00_01));

		//--------------------------------------------------

		var info_dictionary = Info.ToDictionary();

		var vert_nx = info_dictionary["VertexNX"];
		var vert_ny = info_dictionary["VertexNY"];

		DialogTextBox.AppendText($"          頂点数 : {vert_nx} x {vert_ny}\r\n");
		DialogTextBox.AppendText($"      ポリゴン数 : {(vert_nx - 1) * (vert_ny - 1) * 2}\r\n");
		DialogTextBox.AppendText($"テクスチャサイズ : {info_dictionary["TexW"]}pix x {info_dictionary["TexH"]}pix\r\n");
		DialogTextBox.AppendText($"  テクスチャ枚数 : {info_dictionary["TexN"]}\r\n");
		DialogTextBox.AppendText($"  表示地域サイズ : {plane_size_EW}m x {plane_size_NS}m\r\n");
		DialogTextBox.AppendText($"  ポリゴンサイズ : {plane_size_EW / (vert_nx - 1)}m x {plane_size_NS / (vert_ny - 1)}m\r\n");
		DialogTextBox.AppendText($"\r\n");

		DialogTextBox.AppendText($"polygons count from planes\r\n");
		DialogTextBox.AppendText($"gnd-sea polygons count : {info_dictionary["gnd_sea_polygons_count"]}\r\n");
		DialogTextBox.AppendText($"texture polygons count : {info_dictionary["texture_polygons_count"]}\r\n");
		DialogTextBox.AppendText($"\r\n");

		DialogTextBox.AppendText($"polygons count from GLObjects\r\n");

		var gl_objs_count = Viewer.GLObjectCount();	

		foreach(var gl_objs_count_i in gl_objs_count)
			DialogTextBox.AppendText($"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value}\r\n");

		DialogTextBox.AppendText($"\r\n");

		//--------------------------------------------------

		// ◆ここに来る前にStopするべきでは？
		StopWatch.Stop();

		DialogTextBox.AppendText(MakeStopWatchLog(StopWatch));
		DialogTextBox.AppendText($"\r\n");

		//--------------------------------------------------

		// ◆ここに来る前にStopするべきでは？
		MemWatch.Stop();

		DialogTextBox.AppendText(MakeMemWatchLog(MemWatch));
		DialogTextBox.AppendText($"\r\n");

		//--------------------------------------------------

		// ◆プロンプトをこのように忘れず表示しなくてはならないのか？
		DialogTextBox.AppendText(">");
	}
}
//---------------------------------------------------------------------------
}
