//
// GeoViewerDrawShapes_LgLt.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;
using DSF_NET_TacticalDrawing;

using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_Geography.Convert_LgLt_UTM;
using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_TacticalDrawing.StickerPrimitive;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	void GeoViewerDrawShapes_LgLt(CGeoViewer viewer)
	{
		//--------------------------------------------------
		// シンボル１

		var symbol_1_pos = ToLgLt(ToUTM(52, 'S', "FC", 09800, 11950, new CAltitude(10)));

		viewer.AddPrimitive
			(new CGeoSymbol(100, 100, symbol_1_pos)
				// ◆ビットマップストリームだと正しく表示されない。
//				.SetTex(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.gif"), 8);
//				.SetTex(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp"), 8);
//				.SetTex("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp", 8);
				.SetTex("./Symbols/plt.bmp", 8));

		//--------------------------------------------------
		// テキスト１
		// OpenGLバージョン
		// ◆日本語表示はできない。
		
		viewer.AddPrimitive
			(new CGeoString("Kayasan", ToLgLt(ToUTM(52, 'S', "FC", 07760, 15320, new CAltitude(50))))
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)); // ◆色が設定できない。

		//--------------------------------------------------
		// テキスト２

		// インデックス付きピクセル形式のイメージからはグラフィックスオブジェクトが作成できないので32ビットARGBで作成する。
		// ◆背景がうまい具合に透明になっている。
		// ◆サイズが目分量なので文字列に応じて取得するようにしろ。
		var text_canvas = new Bitmap(256, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			
		{
			var g = Graphics.FromImage(text_canvas);

			// ◆どれがきれいか？
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.DrawString("これはテストです。", new Font("ＭＳ ゴシック", 24, GraphicsUnit.Pixel), Brushes.Blue, 0, 0);

			g.Dispose();
		}

		viewer.AddPrimitive
			(new CGeoSymbol(200.0, 32.0, symbol_1_pos, new CCoord(50.0 + 100.0, 0.0, 0.0)) // ◆サイズは目分量
				.SetTex(text_canvas, 0));

		//--------------------------------------------------
		// シンボル２

		var symbol_2_pos = ToLgLt(ToUTM(52, 'S', "FC", 20000, 20000, new CAltitude(2500)));

		viewer.AddPrimitive
			(new CGeoSymbol(100.0, 100.0, symbol_2_pos)
//				.SetTex("C:/DSF/SharedData/Symbols/Person/Red/UnKnown.bmp", 0));
				.SetTex("./Symbols/Unknown.bmp", 0));

		//--------------------------------------------------
		// 円

		// ◆DSF_NET_Geometryに同名のクラスがある。以下同じ。
		viewer.AddPrimitive
			(new CGeoCircle(12, symbol_1_pos, 1000)	// 頂点数12
				.SetLineWidth(5.0f)
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetFill(false));

		//--------------------------------------------------
		// 扇形

		viewer.AddPrimitive
			(new CGeoFan(10, symbol_2_pos, 1000, new CMil(5600), new CMil(800))	// 分割数10
				.SetColor(1.0f, 1.0f, 0.0f, 0.5f)
				.SetFill(true));

		//--------------------------------------------------
		// 直線

		viewer.AddPrimitive
			(new CGeoLine(symbol_1_pos, symbol_2_pos)
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f));

		//--------------------------------------------------
		// 放物線

		viewer.AddPrimitive
			(new CGeoParabola(10, symbol_1_pos, symbol_2_pos, new CMil(1200.0)) // 分割数10、射角1200ミル
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f));

		//--------------------------------------------------
		// 地表面に沿う線分
		// ◆動作を確認出来たらDLLに括り出したいが、WPとLgLtで処理が異なる。
		// ◆四角ポリゴンが鞍形のように中が盛り上がっていると線分が隠れる場合がある。三角ポリゴンなら隠れないようにできそうだが。
		// ◆他の図形にも適用したいが、べた書きするか？

		// 糸島
	//	var sticker_line_s_pos = symbol_1_pos;
	//	var sticker_line_e_pos = symbol_2_pos;

		// 富士山
		var sticker_line_s_pos = ToLgLt(ToUTM(54, 'S', "TE", 88000, 10000, new CAltitude(50)));
		var sticker_line_e_pos = ToLgLt(ToUTM(54, 'S', "TE", 96000, 18000, new CAltitude(50)));

		// ◆WPの要素であるPolygonZoomLevelが出てきている。標高データがタイルなので仕方ないか。
		var nodes = MakeStickerLineStripNodesWP(PolygonZoomLevel, sticker_line_s_pos, sticker_line_e_pos, 20);
		
		var last_node = sticker_line_s_pos;

		foreach(var node in nodes)
		{
			var curr_node = node.Value; 

			viewer.AddPrimitive
				(new CGeoLine(last_node, curr_node)
					.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
					.SetLineWidth(2.0f));

			last_node = curr_node;

			// 確認用
			viewer.AddPrimitive
				(new CGeoCircle(8, curr_node, 10)
					.SetLineWidth(2.0f)
					.SetColor(1.0f, 0.0f, 0.0f, 0.5f) // R,G,B,アルファ(透明度)
					.SetFill(true));
		}

		viewer.AddPrimitive
			(new CGeoLine(last_node, sticker_line_e_pos)
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f));
	}
}
//---------------------------------------------------------------------------
}
