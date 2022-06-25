//
// GeoViewer_DrawShapes.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning; // for SupportedOSPlatform
using System.Windows.Forms;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_UTM;
using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_TacticalDrawing.StickerShape;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")] // Windows固有API(Graphics)が使用されていることを宣言する。
	void DrawShapes()
	{
		//--------------------------------------------------
		// シンボル１

		var symbol_1_p = ToLgLt(ToUTM(52, 'S', "FC", 09800, 11950, new CAltitude(10)));

		Viewer.AddShape
			("symbol_1",
			 new CGeoSymbol(100, 100, symbol_1_p)
				// ◆ビットマップストリームだと正しく表示されない。
//				.SetTexture(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.gif"), 8);
//				.SetTexture(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp"), 8);
//				.SetTexture("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp", 8);
				.SetTexture("./Symbols/plt.bmp", 8));

		//--------------------------------------------------
		// テキスト１
		// OpenGLバージョン
		// ◆日本語表示はできない。
		
		Viewer.AddShape
			("text_1",
			 new CGeoString("Kayasan", ToLgLt(ToUTM(52, 'S', "FC", 07760, 15320, new CAltitude(50))))
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

		Viewer.AddShape
			("text_2",
			 new CGeoSymbol(200.0, 32.0, symbol_1_p, new CCoord(50.0 + 100.0, 0.0, 0.0)) // ◆サイズは目分量
				.SetTexture(text_canvas, 0));

		//--------------------------------------------------
		// シンボル２

		var symbol_2_p = ToLgLt(ToUTM(52, 'S', "FC", 20000, 20000, new CAltitude(2500)));

		Viewer.AddShape
			("symbol_2",
			 new CGeoSymbol(100.0, 100.0, symbol_2_p)
//				.SetTexture("C:/DSF/SharedData/Symbols/Person/Red/UnKnown.bmp", 0));
				.SetTexture("./Symbols/Unknown.bmp", 0));

		//--------------------------------------------------
		// 円

		// ◆DSF_NET_Geometryに同名のクラスがある。以下同じ。
		Viewer.AddShape
			("circle_1",
			 new CGeoCircle(12, symbol_1_p, 1000)	// 頂点数12
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(5.0f)
				.SetFill(false));

		//--------------------------------------------------
		// 扇形

		Viewer.AddShape
			("fan_1",
			 new CGeoFan(10, symbol_2_p, 1000, new CMil(5600), new CMil(800))	// 分割数10
				.SetColor(1.0f, 1.0f, 0.0f, 0.5f)
				.SetFill(true));

		//--------------------------------------------------
		// 直線

/*		Viewer.AddShape
			("line_1",
			 new CGeoLine(symbol_1_p, symbol_2_p)
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f));
*/
		//--------------------------------------------------
		// 放物線

		Viewer.AddShape
			("parabola_1",
			 new CGeoParabola(10, symbol_1_p, symbol_2_p, new CMil(1200.0)) // 分割数10、射角1200ミル
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f));

		//--------------------------------------------------
		// 連続線

		Viewer.AddShape
			("lines_1",
			 new CGeoPolyline()
				.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
				.SetLineWidth(2.0f)
				.AddNode(ToLgLt(ToUTM(52, 'S', "FC", 07100, 14000, new CAltitude(50))))
				.AddNode(ToLgLt(ToUTM(52, 'S', "FC", 07200, 14100, new CAltitude(50))))
				.AddNode(ToLgLt(ToUTM(52, 'S', "FC", 07300, 14000, new CAltitude(50))))
				.AddNode(ToLgLt(ToUTM(52, 'S', "FC", 07400, 14100, new CAltitude(50)))));

		//--------------------------------------------------
		// 地表面に沿う線分
		// ◆動作を確認出来たらDLLに括り出したい。
		// ◆四角ポリゴンが鞍形のように中が盛り上がっていると線分が隠れる場合がある。三角ポリゴンなら隠れないようにできそうだが。
		// ◆他の図形にも適用したいが、べた書きするか？

		// 糸島
	//	var sticker_line_s = symbol_1_p;
	//	var sticker_line_e = symbol_2_p;
		var sticker_line_s = ToLgLt(ToUTM(52, 'S', "FC", 07000, 14000, new CAltitude(50)));
		var sticker_line_e = ToLgLt(ToUTM(52, 'S', "FC", 08500, 16000, new CAltitude(50)));

		// 富士山
	//	var sticker_line_s_pos = ToLgLt(ToUTM(54, 'S', "TE", 88000, 10000, new CAltitude(50)));
	//	var sticker_line_e_pos = ToLgLt(ToUTM(54, 'S', "TE", 96000, 18000, new CAltitude(50)));

		// ◆WPの要素であるPolygonZoomLevelが出てきている。標高データがタイルなので仕方ないか。標高地図データを指定するようにできないか？
		var nodes = MakeStickerLineStripNodesWP(PolygonZoomLevel, sticker_line_s, sticker_line_e);
		
		var sticker_line = new CGeoPolyline()
			.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
			.SetLineWidth(2.0f);

		// ●内容を後で設定してもOK。参照だからか。
		Viewer.AddShape("lines_2", sticker_line);

		foreach(var node in nodes)
		{
			sticker_line.AddNode(node.Value);

			// 確認のため●を描画する。
			Viewer.AddShape
				("",
				 new CGeoCircle(8, node.Value, 10)
					.SetLineWidth(2.0f)
					.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
					.SetFill(true));
		}
	}
}
//---------------------------------------------------------------------------
}
