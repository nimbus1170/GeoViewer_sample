//
// GeoViewer_WP.cs
// 地形ビューア(ワールドピクセル) - オーバレイ描画
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.CEllipsoid;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.GeoObserver;
using static DSF_NET_Geometry.CCoord;
using static DSF_NET_Geometry.CDMS;
using static DSF_NET_TacticalDrawing.GeoObserver;

using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

using static System.Convert;
using System;
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
		if(Title == "糸島半島")
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
		if(false)
	//	if(Title == "糸島半島")
		{ 
			//--------------------------------------------------
			// 以下、テスト

			var lglt0 = new CLgLt(new CLg(139.0), new CLt(36));
			var lglt1 = new CLgLt(new CLg(140.0), new CLt(37));
			var lglt2 = new CLgLt(new CLg(140.0), new CLt(35));
			var lglt3 = new CLgLt(new CLg(138.0), new CLt(35));
			var lglt4 = new CLgLt(new CLg(138.0), new CLt(37));

			// 方位角
			var az1 = Azimuth(lglt0, lglt1).DecimalDeg;
			var az2 = Azimuth(lglt0, lglt2).DecimalDeg;
			var az3 = Azimuth(lglt0, lglt3).DecimalDeg;
			var az4 = Azimuth(lglt0, lglt4).DecimalDeg;

			// 測地線長(大圏距離)
			var a1 = GeodesicLength(lglt1, lglt2);

			// 直線距離
			var a2 = Distance3D(ToGeoCentricCoord(lglt1), ToGeoCentricCoord(lglt2));

			// テストここまで。
			//--------------------------------------------------

			// OP位置：糸島半島の宮地岳西側
			var op_lglt = new CLgLt(new CLg(130.18127), new CLt(33.54134), new CAltitude(10));

			Viewer.AddShape
				("",
				 new CGeoCircle(8, op_lglt, 10)
					.SetLineWidth(2.0f)
					.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 1.0f))
					.SetFill(true));

			// 監視範囲：OPから西方向へ
			var obj_s_lg = new CLg(130.15); var obj_s_lt = new CLt(33.53);
			var obj_e_lg = new CLg(130.18); var obj_e_lt = new CLt(33.55);

StopWatch.Lap("before IsObserve in C++");
			var ol_1 = COverlay_WP.MakeVisibilityOverlay(op_lglt, new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_e_lg, obj_e_lt), PolygonZoomLevel, 10.0);
StopWatch.Lap("after  IsObserve in C++");

StopWatch.Lap("before IsObserveMP in C++");
			var ol_2 = COverlay_WP.MakeVisibilityOverlayMP(op_lglt, new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_e_lg, obj_e_lt), PolygonZoomLevel, 10.0);
StopWatch.Lap("after  IsObserveMP in C++");
/*
			// ◆WP座標は南北逆転
			var obj_sx_wpi = ToWPIntX(PolygonZoomLevel, obj_s_lg);
			var obj_sy_wpi = ToWPIntY(PolygonZoomLevel, obj_e_lt);
			var obj_ex_wpi = ToWPIntX(PolygonZoomLevel, obj_e_lg);
			var obj_ey_wpi = ToWPIntY(PolygonZoomLevel, obj_s_lt);

			// オーバレイのサイズ(密度)
			// ◆取り敢えずWP間隔を100分割してwprと称する。
			int ol_w_wpr = (obj_ex_wpi.Value - obj_sx_wpi.Value) * 100;
			int ol_h_wpr = (obj_ey_wpi.Value - obj_sy_wpi.Value) * 100;

			var ol_3 = viewer.MakeOverlay
				(new CWPInt(obj_sx_wpi, obj_sy_wpi),
					new CWPInt(obj_ex_wpi, obj_ey_wpi),
					ol_w_wpr, ol_h_wpr);

			var g = ol_3.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 10.0f), 0, 0, ol_w_wpr, ol_h_wpr);

			var red_brush = new SolidBrush(Color.Red);

			// ◆このドットサイズはほとんど意味がないが、ドットサイズに厳密には意味はないものとして、取り敢えずこれでやる。
			var dot_size_wpr   = 50;
			var dot_size_2_wpr = dot_size_wpr / 2;

			var obj_wp = new CWP
				(new CWPX(PolygonZoomLevel, 0.0				),
				 new CWPY(PolygonZoomLevel, obj_sy_wpi.Value));
	
			var dd = (double)(dot_size_wpr) / 100.0;

			for(var obj_y_wp = 0; obj_y_wp <= ol_h_wpr; obj_y_wp += dot_size_wpr)
			{
				obj_wp.X.Value = obj_sx_wpi.Value;
		
				for(var obj_x_wp = 0; obj_x_wp <= ol_w_wpr; obj_x_wp += dot_size_wpr)
				{
					var obj_lglt = ToLgLt(obj_wp).SetAltitude(10.0, DAltitudeBase.AGL);

				//	if(!IsObserve(PolygonZoomLevel, op_lglt, obj_lglt)) // C# 版
					if(!IsObserve(op_lglt, obj_lglt, PolygonZoomLevel)) // C++版
						g.FillRectangle(red_brush, obj_x_wp - dot_size_2_wpr, obj_y_wp - dot_size_2_wpr, dot_size_wpr, dot_size_wpr);
					
					obj_wp.X += dd;
				}

				obj_wp.Y += dd;
			}

StopWatch.Lap("after  IsObserve in C#");
*/
			// ◆オフセットやアルファ値は動的に変更することを予期してCOverlayのメンバにしない。
			// ●                 地表面からの高さ↓   ↓透明度
			viewer.AddOverlay("test_ol_1", ol_1, 10.0, 0.3f);
			viewer.AddOverlay("test_ol_2", ol_2, 20.0, 0.3f);
		//	viewer.AddOverlay("test_ol_3", ol_3, 30.0, 0.3f);

			// 距離
		//	var ol_w_m = (int)GeodesicLength(new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_e_lg, obj_s_lt));
		//	var ol_h_m = (int)GeodesicLength(new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_s_lg, obj_e_lt));

		//	var dot_size_w_m = dot_size_wpr * ol_w_m / ol_w_wpr;
		//	var dot_size_h_m = dot_size_wpr * ol_h_m / ol_h_wpr;
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
