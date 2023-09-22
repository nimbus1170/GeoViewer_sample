//
// GeoViewerMainForm_LoadLAS.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using DSF_NET_Geometry;
using DSF_NET_Color;
using DSF_NET_LAS;
using DSF_NET_Scene;

using System.Runtime.Versioning;

using static DSF_CS_Profiler.CProfilerLog;
using static DSF_NET_Geography.DAltitudeBase;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	//-----------------------------------------------------------------------
	// 点群

	CLAS LAS = null;

	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	void LoadLAS()
	{
		OpenFileDialog of_dialog = new ()
			{ Title  = "点群ファイルを開く",
			  Filter = "点群ファイル(*.las;*.csv;*.txt)|*.las;*.csv;*.txt",
			  Multiselect = true };

		if(of_dialog.ShowDialog() == DialogResult.Cancel) return;

		var las_fnames = of_dialog.FileNames;

		of_dialog.Dispose();

StopWatch.Start();
MemWatch .Lap("before ReadLASFromFiles");
		foreach(var las_fname in las_fnames)
		{
			DialogTextBox.AppendText($"loading {las_fname}\r\n");

			var las = ReadLASFromLASFile(las_fname);

StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");

			if(las.LASzip != null)
			{
				DrawLAS("las" + (++ShapesN), las.LASzip);

				ShowLASLog("las" + ShapesN, las.LASzip,"");
				DialogTextBox.AppendText("\r\n");
			}
			else
				DialogTextBox.AppendText($"reading LAS error : {""}\r\n"); // ◆エラーではなく例外を出している。
		}
StopWatch.Stop();
MemWatch .Stop();

		if(ToShowDebugInfo)
		{
			DialogTextBox.AppendText(MakeStopWatchLog(StopWatch));
			DialogTextBox.AppendText(MakeMemWatchLog (MemWatch ));
			DialogTextBox.AppendText("\r\n");
		}
	}

	//-----------------------------------------------------------------------

	CLAS ReadLASFromLASFile(in string las_fname)
	{
		LAS = new CLAS().ReadFromLASFile(las_fname);

		var laszip_header = LAS.LASzip.Header;

		if(LAS.LASType == DLASType.LAS_LgLt)
		{
			// 経緯度

			StartLgLt_0 = new CLgLt(new CLg(laszip_header.min_x - LgLtMargin), new CLt(laszip_header.min_y - LgLtMargin));
			EndLgLt_0   = new CLgLt(new CLg(laszip_header.max_x + LgLtMargin), new CLt(laszip_header.max_y + LgLtMargin));
		}
		else
		{
			// 平面直角座標

			Convert_LgLt_XY.Origin =  LAS.XYOrigin;

			// ◆LASデータはXが東西のようだ。
			var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.min_y, laszip_header.min_x), AGL);
			var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.max_y, laszip_header.max_x), AGL);

			StartLgLt_0 = new CLgLt(new CLg(min_lglt.Lg.DecimalDeg - LgLtMargin), new CLt(min_lglt.Lt.DecimalDeg - LgLtMargin));
			EndLgLt_0   = new CLgLt(new CLg(max_lglt.Lg.DecimalDeg + LgLtMargin), new CLt(max_lglt.Lt.DecimalDeg + LgLtMargin));
		}

		return LAS;
	}

	CLAS ReadLASFromCfgFile(in string cfg_fname)
	{
		LAS = new CLAS().ReadFromCfgFile(cfg_fname);

		var laszip_header = LAS.LASzip.Header;

		if(LAS.LASType == DLASType.LAS_LgLt)
		{
			// 経緯度

			StartLgLt_0 = new CLgLt(new CLg(laszip_header.min_x - LgLtMargin), new CLt(laszip_header.min_y - LgLtMargin));
			EndLgLt_0   = new CLgLt(new CLg(laszip_header.max_x + LgLtMargin), new CLt(laszip_header.max_y + LgLtMargin));
		}
		else
		{
			// 平面直角座標

			Convert_LgLt_XY.Origin =  LAS.XYOrigin;

			// ◆LASデータはXが東西のようだ。
			var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.min_y, laszip_header.min_x), AGL);
			var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.max_y, laszip_header.max_x), AGL);

			StartLgLt_0 = new CLgLt(new CLg(min_lglt.Lg.DecimalDeg - LgLtMargin), new CLt(min_lglt.Lt.DecimalDeg - LgLtMargin));
			EndLgLt_0   = new CLgLt(new CLg(max_lglt.Lg.DecimalDeg + LgLtMargin), new CLt(max_lglt.Lt.DecimalDeg + LgLtMargin));
		}

		return LAS;
	}

	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	void DrawLAS(in string name, in CLASzip laszip_data)
	{
		// ◆ShapeCfg？
		if(ShapeCfg.ToCheckDataOnly) return;

		var laszip_header = laszip_data.Header;

		var x_scale_factor = laszip_header.x_scale_factor;
		var y_scale_factor = laszip_header.y_scale_factor;
		var z_scale_factor = laszip_header.z_scale_factor;

		var x_offset = laszip_header.x_offset;
		var y_offset = laszip_header.y_offset;
		var z_offset = laszip_header.z_offset;

		var laszip_points = laszip_data.Points;

		double x, y, z;

		ushort r, g, b;

		float intensity;

		var vlrs_data = laszip_header.vlrs_data; //ここにファイル情報が入っている。

		// ◆とりあえずこうするが、WKTでは？
		if(vlrs_data.StartsWith("GEOGCS"))
		{
			// 経緯度

			if(false)
			{
				// C#で各点を作成する。
				// C#で色等を処理する場合はこちら
				// ◆必要性はあるので高速化は必要

				//--------------------------------------------------
				// 強度で色分けするため、強度の範囲を求める。
				// ◆LASデータでは強度の基準は決まっていないので、最大値・最小値からその範囲を求める。

				ushort min_intensity = ushort.MaxValue;
				ushort max_intensity = ushort.MinValue;

				foreach(var pt in laszip_points)
				{
					if(pt.intensity < min_intensity) min_intensity = pt.intensity;
					if(pt.intensity > max_intensity) max_intensity = pt.intensity;
				}

				float intensity_range = max_intensity - min_intensity;  

				bool has_intensity = (intensity_range > 0.0f)? true: false;

				//--------------------------------------------------

				var dst_pts = new CGeoPoints((float)ShapeCfg.PointSize);

				var pt_lglt = new CLgLt();

				var pt_color = new CColorF{ A = 1.0f }; // A:透明度(アルファ値)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.x * x_scale_factor;
					y = y_offset + pt.y * y_scale_factor;
					z = z_offset + pt.z * z_scale_factor;

					pt_lglt.SetLg(x).SetLt(y).SetAltitude(AMSL, z);

					//--------------------------------------------------

					intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

					r = pt.r;
					g = pt.g;
					b = pt.b;

					if((r != 0) || (g != 0) || (b != 0))
					{
						// 色付き
						pt_color.R = r / 65535.0f * intensity;
						pt_color.G = g / 65535.0f * intensity;
						pt_color.B = b / 65535.0f * intensity;
					}
					else
					{
						// 色なし
						pt_color.R =
						pt_color.G =
						pt_color.B = intensity;
					}

					//--------------------------------------------------

					dst_pts.AddPoint(pt_lglt, pt_color);
				}

				Viewer.AddShape(name, dst_pts);
			}
			else
				// DLL内で各点を作成する。
				// C#で処理はできないが高速
				Viewer.AddShape(name, laszip_data.MakeGeoPointsBL((float)ShapeCfg.PointSize));
		}
		else
		{
			// 平面直角座標

//			Convert_LgLt_XY.Origin = ReadLASOrigin(vlrs_data, DefaultOrigin);
			Convert_LgLt_XY.Origin = LAS.XYOrigin;

			if(false)
			{
				// C#で各点を作成する。
				// C#で色等を処理する場合はこちら

				//--------------------------------------------------
				// 強度で色分けするため、強度の範囲を求める。
				// ◆LASデータでは強度の基準は決まっていないので、最大値・最小値からその範囲を求める。

				ushort min_intensity = ushort.MaxValue;
				ushort max_intensity = ushort.MinValue;

				foreach(var pt in laszip_points)
				{
					if(pt.intensity < min_intensity) min_intensity = pt.intensity;
					if(pt.intensity > max_intensity) max_intensity = pt.intensity;
				}

				float intensity_range = max_intensity - min_intensity;  

				bool has_intensity = (intensity_range > 0.0f)? true: false;

				//--------------------------------------------------

				var dst_pts = new CGeoPoints((float)ShapeCfg.PointSize);

				var pt_xy = new CCoord();

			//	var pt_lglt = new CLgLt(new CLg(), new CLt(), AMSL);

				var pt_color = new CColorF{ A = 1.0f }; // A:透明度(アルファ値)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.x * x_scale_factor;
					y = y_offset + pt.y * y_scale_factor;
					z = z_offset + pt.z * z_scale_factor;

//StopWatch.Lap("(C#)before XYToLgLt");
					// ◆ここに時間がかかっている。
					// ◆LASデータはXが東西のようだ。
					var pt_lglt = Convert_LgLt_XY.ToLgLt(pt_xy.Set(y, x), AMSL).SetAltitude(AMSL, z);
					// ◆渡すようにすると速い？遅い？
					// ◆ストップウォッチで時間喰ってる。
				//	Convert_LgLt_XY.ToLgLt(pt_xy.Set(y, x), ref pt_lglt).SetAltitude(AMSL, z);
//StopWatch.Lap("(C#)after  XYToLgLt");

					//--------------------------------------------------

					intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

					r = pt.r;
					g = pt.g;
					b = pt.b;

					if((r != 0) || (g != 0) || (b != 0))
					{
						// 色付き
						pt_color.R = r / 65535.0f * intensity;
						pt_color.G = g / 65535.0f * intensity;
						pt_color.B = b / 65535.0f * intensity;
					}
					else
					{
						// 色なし
						pt_color.R =
						pt_color.G =
						pt_color.B = intensity;
					}

					//--------------------------------------------------

					dst_pts.AddPoint(pt_lglt, pt_color);
				}

				Viewer.AddShape(name, dst_pts);
			}
			else
				// DLL内で各点を作成する。
				// C#で処理はできないが高速
				Viewer.AddShape(name, laszip_data.MakeGeoPointsXY((float)ShapeCfg.PointSize));
		}

StopWatch.Lap("after  MakeLASPoints");
MemWatch .Lap("after  MakeLASPoints");

		Viewer.DrawScene();

StopWatch.Lap("after  DrawScene");
MemWatch .Lap("after  DrawScene");
	}

	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	private void ShowLASLog(in string las_name, in CLASzip laszip_data, in string msg)
	{
		DialogTextBox.AppendText(LAS.Log);

		if(!string.IsNullOrEmpty(msg)) DialogTextBox.AppendText(msg);

		DialogTextBox.AppendText($"\r\n");
	}

	//-----------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
