//
// GeoViewerMainForm_LoadLAS.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using DSF_NET_Geometry;
using DSF_NET_Color;
using DSF_NET_Scene;
using DSF_NET_LAS;

using System.Runtime.Versioning;

using static DSF_CS_Profiler.CProfilerLog;
using static DSF_NET_Geography.DAltitudeBase;
using static DSF_NET_LAS.CLAS;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	CLASzip LASzipData = null;

	string ReadLASMsg = "";

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

			var (laszip_data, read_las_msg) = ReadLASFromFile(las_fname);

StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");

			if(laszip_data != null)
			{
				DrawLAS("las" + (++ShapesN), laszip_data);

				ShowLASLog("las" + ShapesN, laszip_data, read_las_msg);
				DialogTextBox.AppendText("\r\n");
			}
			else
				DialogTextBox.AppendText($"reading LAS error : {read_las_msg}\r\n");

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

/*	CXYOrigin ReadLASOrigin(in string vlrs_data)
	{
		CXYOrigin origin;

		if(vlrs_data.StartsWith("PROJCS"))
		{
			// ◆とりあえず原点座標をヘッダから読む。

			var origin_cm_s = vlrs_data.IndexOf("PARAMETER[\"Central_Meridian\"") + 29;
			var origin_cm_e = vlrs_data.IndexOf(']', origin_cm_s);

			var origin_cm = ToDouble(vlrs_data.Substring(origin_cm_s, origin_cm_e - origin_cm_s));

			var origin_lt_s = vlrs_data.IndexOf("PARAMETER[\"Latitude_Of_Origin\"") + 31;
			var origin_lt_e = vlrs_data.IndexOf(']', origin_lt_s);

			var origin_lt = ToDouble(vlrs_data.Substring(origin_lt_s, origin_lt_e - origin_lt_s));

			origin = new CXYOrigin("", new CLgLt(new CLg(origin_cm), new CLt(origin_lt), AGL));
		}
		else if((1 <= DefaultOrigin) && (DefaultOrigin <= 19))
		{ 
			origin = Convert_LgLt_XY.Origins[DefaultOrigin];
		}
		else
			throw new Exception("unknown vlrs_data and appropriate DefaultOrigin not defined");

		return origin;*/
/*
vlrs_dataの例
WKTか。

BLの場合
"GEOGCS[
	"GCS_WGS_1984",
	DATUM[
		"D_WGS_1984",
		SPHEROID[
			"WGS_1984",
			6378137.0,
			298.257223563],
		TOWGS84[0,0,0,0,0,0,0]],
	PRIMEM["Greenwich",0.0],
	UNIT["Degree",0.0174532925199433]]"

XYの場合
"PROJCS[
	"JGD2011_Japan_Zone_9",
	GEOGCS[
		"GCS_JGD_2011",
		DATUM[
			"D_JGD_2011",
			SPHEROID[
				"GRS_1980",
				6378137.0,
				298.257222101],
			TOWGS84[0,0,0,0,0,0,0]],
		PRIMEM["Greenwich",0.0],
		UNIT["Degree",0.0174532925199433]],
	PROJECTION["Transverse_Mercator"],
	PARAMETER["False_Easting",0.0],
	PARAMETER["False_Northing",0.0],
	PARAMETER["Central_Meridian",139.833333333333],
	PARAMETER["Scale_Factor",0.9999],
	PARAMETER["Latitude_Of_Origin",36],
	UNIT["Meter",1.0]]"

vlrs_dataがない場合もある。
*/
//	}

	//-----------------------------------------------------------------------

	(CLASzip laszip_data, string msg) ReadLASFromFile(in string las_fname)
	{
		CLASzip laszip_data;

		//--------------------------------------------------
		// ローカルな設定をファイルから読み込む。
		// ◆最初に読み込みを試行し、原点定義等が存在していればそれでDefaultOrigin等を上書きする。

		var local_cfg_fname = Path.GetDirectoryName(las_fname) + "\\GeoViewerCfg.local.xml";

		if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

		//--------------------------------------------------

		var las_file_ext = Path.GetExtension(las_fname).ToLower();

		//--------------------------------------------------
		// ◆アプリがファイル選択モードで、複数の点群データを読むと、開始・終了座標が最後の点群データのものになってしまう。
		// →◆なので、取り敢えずファイル選択モードではMultiselectはfalseにしてある。

		//--------------------------------------------------
		// LASファイルの場合

		if(las_file_ext == ".las")
		{
			laszip_data = new CLASzip(las_fname);

			var laszip_header = laszip_data.Header;

			if(ReadStart != -1) // ReadEndが-1なら最後まで読む。
			{
				if(ReadStep == -1)
					laszip_data.ReadPoints(ReadStart, ReadEnd);
				else
					laszip_data.ReadPoints(ReadStart, ReadEnd, ReadStep);
			}
			else if(ReadStep == -1)
				laszip_data.ReadPoints();
//				laszip_data.ReadPointsRange(0.001); ◆うまくいかない。
			else
				laszip_data.ReadPoints(ReadStep);

			var vlrs_data = laszip_header.vlrs_data;

			// ◆WKTで読め。
			if(vlrs_data.StartsWith("GEOGCS"))
			{
				// 経緯度

				StartLgLt_0 = new CLgLt(new CLg(laszip_header.min_x - LgLtMargin), new CLt(laszip_header.min_y - LgLtMargin), AGL);
				EndLgLt_0   = new CLgLt(new CLg(laszip_header.max_x + LgLtMargin), new CLt(laszip_header.max_y + LgLtMargin), AGL);
			}
			else
			{
				// 平面直角座標

				Convert_LgLt_XY.Origin = ReadLASOrigin(vlrs_data, DefaultOrigin);

				// ◆LASデータはXが東西のようだ。
				var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.min_y, laszip_header.min_x), AGL);
				var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.max_y, laszip_header.max_x), AGL);

				StartLgLt_0 = new CLgLt(new CLg(min_lglt.Lg.DecimalDeg - LgLtMargin), new CLt(min_lglt.Lt.DecimalDeg - LgLtMargin), AGL);
				EndLgLt_0   = new CLgLt(new CLg(max_lglt.Lg.DecimalDeg + LgLtMargin), new CLt(max_lglt.Lt.DecimalDeg + LgLtMargin), AGL);
			}

			return (laszip_data, "");
		}

		//--------------------------------------------------
		// テキストファイルの場合

		Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[DefaultOrigin];

		if((DefaultOrigin < 1) || (19 < DefaultOrigin)) throw new Exception("DefaultOrigin not defined");

		laszip_data = new CLASzip();

		//--------------------------------------------------
		// フォーマット定義からフォーマットを特定する。
		// ◆何とかならんか？

		string[] row_format = TXTFormat.Split(' ');

		// ◆当初は無効値として999を入れておいて、定義されていなかったらデフォルト値を設定する。
		var row_i_X = 999;
		var row_i_Y = 999;
		var row_i_Z = 999;
		var row_i_R = 999;
		var row_i_G = 999;
		var row_i_B = 999;
		var row_i_INTENSITY = 999;
		var row_i_CLASS = 999;

		double x_scale_factor = 1.0;
		double y_scale_factor = 1.0;
		double z_scale_factor = 1.0;

		for(var row_i = 0; row_i < row_format.Length; ++row_i)
		{
			var row_format_elems = row_format[row_i].Split(".");

			switch(row_format_elems[0])
			{
				case "X": row_i_X = row_i; x_scale_factor = 1.0 / Math.Pow(10, ToDouble(row_format_elems[1])); break;
				case "Y": row_i_Y = row_i; y_scale_factor = 1.0 / Math.Pow(10, ToDouble(row_format_elems[1])); break;
				case "Z": row_i_Z = row_i; z_scale_factor = 1.0 / Math.Pow(10, ToDouble(row_format_elems[1])); break;
				case "R": row_i_R = row_i; break;
				case "G": row_i_G = row_i; break;
				case "B": row_i_B = row_i; break;
				case "INTENSITY": row_i_INTENSITY = row_i; break;
				case "CLASS": row_i_CLASS = row_i; break;
			}
		}

		if((row_i_X == 999) || (row_i_Y == 999) || (row_i_Z == 999)) return (null, "フォーマット定義が不足しています。");

		//--------------------------------------------------

		var sr = new StreamReader(las_fname);

		// タイトル行を読み飛ばす。
		if(TXTTitleLine == "true") sr.ReadLine();

		double min_intensity = double.MaxValue;
		double max_intensity = double.MinValue;

		bool has_negative_intensity = false;

		while(!sr.EndOfStream)
		{
			string[] values = sr.ReadLine().Split(',');

			double _intensity = 0.0;

			if(row_i_INTENSITY != 999)
			{ 
				_intensity = ToDouble(values[row_i_INTENSITY]);

				if(_intensity < min_intensity) min_intensity = _intensity; 
				if(_intensity > max_intensity) max_intensity = _intensity; 
			
				// ◆受光強度に負値が入っていたのがあったので、便宜的に符合を反転させる。(R57のデータで負数かつ小数になっている。)
				// →◆正負両方入っていたらどうする？
				if(_intensity < 0)
				{
					_intensity = -_intensity;
					has_negative_intensity = true;
				}
			}

			var laszip_pt = new CLASzipPoint
				{ X = ToInt32(ToDouble(values[row_i_X]) / x_scale_factor),
				　Y = ToInt32(ToDouble(values[row_i_Y]) / y_scale_factor),
				  Z = ToInt32(ToDouble(values[row_i_Z]) / z_scale_factor),
				  intensity = (UInt16)_intensity, 
				  classification = (row_i_CLASS == 999)? (Byte)0: ToByte(values[row_i_CLASS]),
				  R = (row_i_R == 999)? (UInt16)32767: (UInt16)(ToUInt16(values[row_i_R]) * (UInt16)256),
				  G = (row_i_G == 999)? (UInt16)32767: (UInt16)(ToUInt16(values[row_i_G]) * (UInt16)256),
				  B = (row_i_B == 999)? (UInt16)32767: (UInt16)(ToUInt16(values[row_i_B]) * (UInt16)256) };
 
			laszip_data.Points.Add(laszip_pt);
		}

		string msg = "";

		if(row_i_R == 999) msg += "色情報がなかったため32767(灰色)で設定\r\n";
		
		if(row_i_INTENSITY != 999)
		{
			msg += $"受光強度(輝度) : {min_intensity} ～ {max_intensity}";
			msg += (has_negative_intensity)? " (負値のものは符号を反転)\r\n": "\r\n";
		}

		//--------------------------------------------------

		// ◆同名の変数が下位スコープ内にあるためブロックに入れる。
		{ 
			var min_x_int = Int32.MaxValue;
			var max_x_int = Int32.MinValue;
			var min_y_int = Int32.MaxValue;
			var max_y_int = Int32.MinValue;
			var min_z_int = Int32.MaxValue;
			var max_z_int = Int32.MinValue;

			foreach(var laszip_pt in laszip_data.Points)
			{
				if(laszip_pt.X < min_x_int) min_x_int = laszip_pt.X;
				if(laszip_pt.X > max_x_int) max_x_int = laszip_pt.X;
				if(laszip_pt.Y < min_y_int) min_y_int = laszip_pt.Y;
				if(laszip_pt.Y > max_y_int) max_y_int = laszip_pt.Y;
				if(laszip_pt.Z < min_z_int) min_z_int = laszip_pt.Z;
				if(laszip_pt.Z > max_z_int) max_z_int = laszip_pt.Z;
			}

			var min_x = min_x_int * x_scale_factor;
			var min_y = min_y_int * y_scale_factor;
			var min_z = min_z_int * z_scale_factor;
			var max_x = max_x_int * x_scale_factor;
			var max_y = max_y_int * y_scale_factor;
			var max_z = max_z_int * z_scale_factor;

			// ◆LASデータはXが東西のようだ。
			var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(min_y, min_x), AGL);
			var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(max_y, max_x), AGL);

			var lg_s_value = min_lglt.Lg.DecimalDeg - LgLtMargin;
			var lg_e_value = max_lglt.Lg.DecimalDeg + LgLtMargin;
			var lt_s_value = min_lglt.Lt.DecimalDeg - LgLtMargin;
			var lt_e_value = max_lglt.Lt.DecimalDeg + LgLtMargin;

			StartLgLt_0 = new CLgLt(new CLg(lg_s_value), new CLt(lt_s_value), AGL);
			EndLgLt_0   = new CLgLt(new CLg(lg_e_value), new CLt(lt_e_value), AGL);

			//--------------------------------------------------

			laszip_data.Header = new CLASzipHeader
				{ version_major = 0,
				  version_minor = 0,
				  point_data_format = 0,
				  number_of_point_records = laszip_data.Points.Count,
				  x_scale_factor = x_scale_factor,
				  y_scale_factor = y_scale_factor,
				  z_scale_factor = z_scale_factor,
				  x_offset = 0.0f,
				  y_offset = 0.0f,
				  z_offset = 0.0f,
				  max_x = max_x,
				  min_x = min_x,
				  max_y = max_y,
				  min_y = min_y,
				  max_z = max_z,
				  min_z = min_z,
				  vlrs_data = "" };
		}

		//--------------------------------------------------

		return (laszip_data, msg);
	}

	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	void DrawLAS(in string name, in CLASzip laszip_data)
	{
		if(ToCheckDataOnly) return;

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

				var dst_pts = new CGeoPoints(PointSize);

				var pt_lglt = new CLgLt();

				var pt_color = new CColorF{ A = 1.0f }; // A:透明度(アルファ値)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.X * x_scale_factor;
					y = y_offset + pt.Y * y_scale_factor;
					z = z_offset + pt.Z * z_scale_factor;

					pt_lglt.SetLg(x).SetLt(y).SetAltitude(AMSL, z);

					//--------------------------------------------------

					intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

					r = pt.R;
					g = pt.G;
					b = pt.B;

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
				Viewer.AddShape(name, laszip_data.MakeGeoPointsBL(PointSize));
		}
		else
		{
			// 平面直角座標

			Convert_LgLt_XY.Origin = ReadLASOrigin(vlrs_data, DefaultOrigin);

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

				var dst_pts = new CGeoPoints(PointSize);

				var pt_xy = new CCoord();

			//	var pt_lglt = new CLgLt(new CLg(), new CLt(), AMSL);

				var pt_color = new CColorF{ A = 1.0f }; // A:透明度(アルファ値)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.X * x_scale_factor;
					y = y_offset + pt.Y * y_scale_factor;
					z = z_offset + pt.Z * z_scale_factor;

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

					r = pt.R;
					g = pt.G;
					b = pt.B;

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
				Viewer.AddShape(name, laszip_data.MakeGeoPointsXY(PointSize));
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
		var laszip_header = laszip_data.Header;

		DialogTextBox.AppendText($"[{las_name}]\r\n");
		DialogTextBox.AppendText($"        バージョン : {laszip_header.version_major}.{laszip_header.version_minor}\r\n");
		DialogTextBox.AppendText($"      フォーマット : {laszip_header.point_data_format}\r\n");
		DialogTextBox.AppendText($"        ポイント数 : {(int)laszip_header.number_of_point_records:#,0}\r\n");

		var laszip_points = laszip_data.Points;

		var has_intensity = false;
		var has_color = false;
		var has_class = false;
			
		foreach(var pt in laszip_points)
		{
			if(!(has_intensity) && ( pt.intensity != 0)						  ) has_intensity = true;
			if(!(has_color    ) && ((pt.R != 0) || (pt.G != 0) || (pt.B != 0))) has_color	  = true;　// ◆実際は32767を入れている。
			if(!(has_class	  ) && ( pt.classification != 0)				  ) has_class	  = true;
		}

		DialogTextBox.AppendText($"  輝度付きポイント : {(has_intensity? "あり":"なし")}\r\n");
		DialogTextBox.AppendText($"    色付きポイント : {(has_color?	 "あり":"なし")}\r\n");
		DialogTextBox.AppendText($"クラス付きポイント : {(has_class?	 "あり":"なし")}\r\n");

		if(!string.IsNullOrEmpty(msg)) DialogTextBox.AppendText(msg);

		DialogTextBox.AppendText($"\r\n");
	}

	//-----------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
