//
// GeoViewer_LgLt.cs
// 地形ビューア(経緯度) - オーバレイ描画
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.DAltitudeBase;

using System.Runtime.Versioning;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawOverlayOnGeoViewer_LgLt(in CGeoViewer_LgLt viewer)
	{
		//--------------------------------------------------
		// 1 地図を半透明にして重ねてみる。

		// ◆下のテクスチャを隠してしまう。何かあったはず。
	//	viewer.AddOverlay("test_map_ol", img_map_data, 1000.0, 0.5f);

		//--------------------------------------------------
		// 2 グリッドをオーバレイに描画する。

		if(ToDrawGrid && (GridOverlayCfg != null))
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

			viewer.AddOverlay
				("grid",
				 new CImageMapData_LgLt(grid_map_img, StartLgLt, EndLgLt),
				 ol_offset,
				 1.0f); // 透明度
		}

		//--------------------------------------------------
		// 3 部分的にオーバレイを重ねてみる。
		// ◆オーバレイが範囲外にあると当然エラーになる。(たまたまならない場合もある。)
		if(true)
		{
			// オーバレイのサイズの基準(小さい辺をこのサイズにする。)
			int ol_size = 500;
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// 可也山
			var (ol_s_lglt, ol_e_lglt) = ExtendToPolygonSize
				(new CLgLt(new CLg(new CDMS(130,  9, 0.0).DecimalDeg), new CLt(new CDMS(33, 34, 0.0).DecimalDeg), AGL),
				 new CLgLt(new CLg(new CDMS(130, 10, 0.0).DecimalDeg), new CLt(new CDMS(33, 35, 0.0).DecimalDeg), AGL),
				 PolygonSize);

			var ol = viewer.MakeOverlay(ol_s_lglt, ol_e_lglt, ol_w, ol_h);

			// オーバレイ拡張前の対角線
			var p_from = ol.ToPointOnOverlay(ol_s_lglt);
			var p_to   = ol.ToPointOnOverlay(ol_e_lglt);

			var g = ol.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 5.0f), 0, 0, ol_w, ol_h);
			
			g.DrawLine(new Pen(Color.Red, 5.0f), p_from, p_to);
			
			g.Dispose();

			viewer.AddOverlay
				("test_ol",
				 ol,
				 200.0, // 地表面からの高さ
				 1.0f); // 透明度
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
