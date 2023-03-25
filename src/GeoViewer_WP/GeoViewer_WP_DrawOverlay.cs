//
// GeoViewer_WP.cs
// 地形ビューア(ワールドピクセル) - オーバレイ描画
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;

using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawOverlayGeoViewer_WP(in CGeoViewer_WP viewer)
	{
		//--------------------------------------------------
		// 1 地図を半透明にして重ねてみる。
	//	viewer.AddOverlay("test_map_ol", img_map_data, 1000.0, 0.5f);

		//--------------------------------------------------
		// 2 グリッドをオーバレイに描画する。

		if(GridOverlayCfg != null)
		{
			// オーバレイのサイズの基準(小さい辺をこのサイズにする。)
			var ol_size = ToInt32(GridOverlayCfg.Attributes["Size"].InnerText);
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// ◆テクスチャサイズは任意だが、フォントサイズ等がテクスチャサイズに引きずられる。GraphicsUnit.Pixel以外でも。
			var grid_map_img = new Bitmap(ol_w, ol_h);

			DrawLgLtGrid(grid_map_img, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (grid_map_img, StartLgLt, EndLgLt, GridFontSize);

			// 地表面からの高さ
		 	var ol_offset = ToDouble(GridOverlayCfg.Attributes["Offset"].InnerText);

			// 地図画像データのズームレベルにするので新規インスタンスにする。
			var img_s_wp = new CWPInt(StartWP, ImageZoomLevel);
			var img_e_wp = new CWPInt(EndWP	 , ImageZoomLevel);

			viewer.AddOverlay
				("grid",
				 new CImageMapData_WP(grid_map_img, img_s_wp, img_e_wp),
				 ol_offset,
				 1.0f); // 透明度
		}

		//--------------------------------------------------
		// 3 部分的にテクスチャオーバレイを重ねてみる。
		// ◆オーバレイが範囲外にあると当然エラーになるが、ならない場合もある。
		if(true)
		{
			// オーバレイのサイズの基準(小さい辺をこのサイズにする。)
			int ol_size = 1000;
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// 可也山
		//	var ol_s_lg = new CLg(new CDMS(130,  9, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS( 33, 34, 0.0).DecimalDeg);
		//	var ol_e_lg = new CLg(new CDMS(130, 10, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS( 33, 35, 0.0).DecimalDeg);

			// 香椎沖
			var ol_s_lg = new CLg(new CDMS(130, 24, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS(33, 42, 0.0).DecimalDeg);
			var ol_e_lg = new CLg(new CDMS(130, 25, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS(33, 43, 0.0).DecimalDeg);

			// ◆オーバレイの範囲は南北逆転
			var ol = viewer.MakeOverlay
				(ToWPInt(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_e_lt)),
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_s_lt)),
				 ol_w, ol_h);

			var p_from = ol.ToPointOnOverlay(ToWP(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_s_lt)));
			var p_to   = ol.ToPointOnOverlay(ToWP(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_e_lt)));

			var g = ol.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 5.0f), 0, 0, ol_w, ol_h);

			g.DrawLine(new Pen(Color.Red, 5.0f), p_from, p_to);

			g.Dispose();
		
			// ◆オフセットやアルファ値は動的に変更することを予期してCOverlayのメンバにしない。
			viewer.AddOverlay
				("test_ol",
				 ol,
				 200.0, // 地表面からの高さ
				 0.5f); // 透明度
		}

		//--------------------------------------------------
		// 4 部分的に三角ポリゴンオーバレイを重ねてみる。
		// ◆XMLにしても単なるオーバレイに何の意味があるか分からないが、取り敢えず。
		if(true)
		{
			// 可也山
		//	var ol_s_lg = new CLg(new CDMS(130,  9, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS( 33, 34, 0.0).DecimalDeg);
		//	var ol_e_lg = new CLg(new CDMS(130, 10, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS( 33, 35, 0.0).DecimalDeg);

			// 富士山
		//	var ol_s_lg = new CLg(138.70); var ol_s_lt = new CLt(35.33);
		//	var ol_e_lg = new CLg(138.75); var ol_e_lt = new CLt(35.36);

			// 大津市
			// ◆範囲を外側境界に近づけるとインデックスがオーバーする。
			var ol_s_lg = new CLg(new CDMS(135, 53, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS(34, 57, 30.0).DecimalDeg);
			var ol_e_lg = new CLg(new CDMS(135, 55, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS(34, 59, 30.0).DecimalDeg);

			viewer.AddOverlay
				("test_ol_2",
				 DOverlayBase.OnGeoid,
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_s_lt)),
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_e_lt)),
				 100.0, // Elevation→地表面からの高さ、Geoid→標高
				 new CColorF(0.1f, 0.1f, 0.8f, 0.5f)); // RGBA
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
