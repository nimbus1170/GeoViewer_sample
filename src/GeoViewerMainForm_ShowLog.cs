//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.DAltitudeBase;

using DSF_NET_Scene;
using static DSF_NET_Scene.CGLObject;

using static DSF_CS_Profiler.CProfilerLog;

using static System.Math;
using System.Runtime.Versioning;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
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

		var log = Log.ToDictionary();

		var vert_nx = log["VertexNX"];
		var vert_ny = log["VertexNY"];

		DialogTextBox.AppendText($"                  頂点数 : {vert_nx:#,0} x {vert_ny:#,0}\r\n");
		DialogTextBox.AppendText($"メッシュ(三角ポリゴン)数 : {(vert_nx - 1) * (vert_ny - 1) * 2:#,0}\r\n");
		DialogTextBox.AppendText($"        テクスチャサイズ : {log["TexW"]}px x {log["TexH"]}px\r\n");
		DialogTextBox.AppendText($"          テクスチャ枚数 : {log["TexN"]}\r\n");
		DialogTextBox.AppendText($"          表示地域サイズ : {plane_size_EW:#,0}m x {plane_size_NS:#,0}m\r\n");
		DialogTextBox.AppendText($"          メッシュサイズ : {plane_size_EW / (vert_nx - 1)}m x {plane_size_NS / (vert_ny - 1)}m\r\n");
		DialogTextBox.AppendText($"        画像ズームレベル : {ImageZoomLevel}\r\n");
		DialogTextBox.AppendText($"    メッシュズームレベル : {MeshZoomLevel}\r\n");
		DialogTextBox.AppendText($"              OpenGL精度 : {GetGLPrecision()}\r\n");
		DialogTextBox.AppendText($"\r\n");

		// 表示の有無にかかわらずストップする。
		// ◆ここに来る前にStopするべきでは？
		StopWatch.Stop();
		MemWatch .Stop();

		if(ToShowDebugInfo)
		{
			DialogTextBox.AppendText($"mesh count from planes\r\n");
			DialogTextBox.AppendText($"gnd-sea mesh : {log["gnd_sea_mesh_count"]:#,0}\r\n");
			DialogTextBox.AppendText($"texture mesh : {log["texture_mesh_count"]:#,0}\r\n");
			DialogTextBox.AppendText($"\r\n");
			DialogTextBox.AppendText($"mesh count from GLObjects\r\n");

			var gl_objs_count = Viewer.GLObjectCount();	

			foreach(var gl_objs_count_i in gl_objs_count)
				DialogTextBox.AppendText($"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value:#,0}\r\n");

			DialogTextBox.AppendText($"\r\n");

			DialogTextBox.AppendText(MakeStopWatchLog(StopWatch) + $"\r\n");
			DialogTextBox.AppendText(MakeMemWatchLog (MemWatch ) + $"\r\n");
		}

		//--------------------------------------------------
		
		if(LASzipData != null) ShowLASLog("las" + ShapesN, LASzipData, ReadLASMsg);

		// ◆なぜかプロンプトを受け付けなくなったが、表示量を削減したら大丈夫だった。単に表示量が多いから？
		if(ShapeFile != null) ShowShapefileLog("shp" + ShapesN, ShapeFile, ReadShapefileMsg);

		//--------------------------------------------------

		// ◆うまくプロンプトまでスクロールしない。
//		Activate();
//		TopMost = true;

//		DialogTextBox.Focus();

		// ◆プロンプトをこのように忘れず表示しなくてはならないのか？
		DialogTextBox.AppendText(">");

//		DialogTextBox.ScrollToCaret();
	}
}
//---------------------------------------------------------------------------
}
