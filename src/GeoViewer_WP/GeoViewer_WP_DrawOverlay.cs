//
// GeoViewer_WP.cs
// 地形ビューア(ワールドピクセル) - オーバレイ描画
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geometry.CDMS;
using static DSF_NET_TacticalDrawing.Observer;
using static DSF_NET_Scene.Observer;

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
		//	var ol_s_lg = new CLg(ToDecimalDeg(130,  9, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 34, 0.0));
		//	var ol_e_lg = new CLg(ToDecimalDeg(130, 10, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 35, 0.0));

			// 香椎沖
			var ol_s_lg = new CLg(ToDecimalDeg(130, 24, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 42, 0.0));
			var ol_e_lg = new CLg(ToDecimalDeg(130, 25, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 43, 0.0));

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
		// ●琵琶湖の水止めたろかシミュレーション
		// ◆単なる単色オーバレイに意味があるか分からないが、取り敢えず。
		if(Title == "琵琶湖（大津市）")
		{
			// ◆範囲を外側境界に近づけるとインデックスがオーバーする。
			var ol_s_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 53, 0.0)), new CLt(ToDecimalDeg(34, 57, 30.0)));
			var ol_e_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 55, 0.0)), new CLt(ToDecimalDeg(34, 59, 30.0)));

			viewer.AddOverlay
				("琵琶湖の水面",
				 ToWPInt(PolygonZoomLevel, ol_s_lglt),
				 ToWPInt(PolygonZoomLevel, ol_e_lglt),
				 DOverlayAltitudeOffsetBase.AMSL, // AGL:地表面からの高度、AMSL:海面からの高度()
				 100.0,
				 new CColorF(0.1f, 0.1f, 0.8f, 0.5f)); // RGBA
		}

		//--------------------------------------------------
		// 視界図
		if(true)
		{ 
			// OP位置：糸島半島の宮地岳西側
			var op_lglt = new CLgLt(new CLg(130.18127), new CLt(33.54134), new CAltitude(10));

			Viewer.AddShape
				("",
				 new CGeoCircle(8, op_lglt, 10)
					.SetLineWidth(2.0f)
					.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 1.0f))
					.SetFill(true));

			// 監視範囲：OPから西方向へ
			var ol_s_lg = new CLg(130.15); var ol_s_lt = new CLt(33.53);
			var ol_e_lg = new CLg(130.18); var ol_e_lt = new CLt(33.55);

			// ◆WP座標は南北逆転
			var observe_wp_sx = ToWPIntX(PolygonZoomLevel, ol_s_lg);
			var observe_wp_sy = ToWPIntY(PolygonZoomLevel, ol_e_lt);
			var observe_wp_ex = ToWPIntX(PolygonZoomLevel, ol_e_lg);
			var observe_wp_ey = ToWPIntY(PolygonZoomLevel, ol_s_lt);

			// オーバレイのサイズ
			// ◆取り合えずWP間隔を100分割
			int ol_w = (observe_wp_ex.Value - observe_wp_sx.Value) * 100;
			int ol_h = (observe_wp_ey.Value - observe_wp_sy.Value) * 100;

			var ol = viewer.MakeOverlay
				(new CWPInt(observe_wp_sx, observe_wp_sy),
				 new CWPInt(observe_wp_ex, observe_wp_ey),
				 ol_w, ol_h);

			var g = ol.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 10.0f), 0, 0, ol_w, ol_h);

			var red_brush = new SolidBrush(Color.Red);

			var dot_size   = 50;
			var dot_size_2 = dot_size / 2;

			for(var obj_y = 0; obj_y <= ol_h; obj_y += dot_size)
				for(var obj_x = 0; obj_x <= ol_w; obj_x += dot_size)
				{
					var obj_wp_x_value = observe_wp_sx.Value + ((double)obj_x) / 100.0;
					var obj_wp_y_value = observe_wp_sy.Value + ((double)obj_y) / 100.0;

					var obj_lglt = ToLgLt(new CWP(new CWPX(PolygonZoomLevel, obj_wp_x_value), new CWPY(PolygonZoomLevel, obj_wp_y_value)));

					obj_lglt.SetAltitude(10, DAltitudeBase.AGL);

					StopWatch.Lap("before IsObserve");

				//	var is_observe = IsObserve(PolygonZoomLevel, op_lglt, obj_lglt);
					var is_observe = IsObserve(op_lglt, obj_lglt, PolygonZoomLevel);
				
					StopWatch.Lap("after IsObserve");

				//	if(!IsObserve(PolygonZoomLevel, op_lglt, obj_lglt))
					if(!is_observe)
						g.FillRectangle(red_brush, obj_x - dot_size_2, obj_y - dot_size_2, dot_size, dot_size);
				}

			g.Dispose();
		
			// ◆オフセットやアルファ値は動的に変更することを予期してCOverlayのメンバにしない。
			viewer.AddOverlay
				("test_ol",
				 ol,
				 10.0, // 地表面からの高さ
				 0.5f); // 透明度
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
