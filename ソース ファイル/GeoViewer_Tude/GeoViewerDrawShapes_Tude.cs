//
//
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_Geography.Convert_Tude_UTM;

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
	void GeoViewerDrawShapes_Tude(CGeoViewer viewer)
	{
		//--------------------------------------------------
		// シンボル１

		var symbol_1_pos = ToTude(ToUTM(52, 'S', "FC0980011950", new CAltitude(10)));

		var symbol_1 = new CGeoSymbol(100, 100, symbol_1_pos)
			// ◆ビットマップストリームだと正しく表示されない。
//			.SetTex(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.gif"), 8);
//			.SetTex(new Bitmap("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp"), 8);
//			.SetTex("C:/DSF/SharedData/Symbols/Unit/inf/blue/plt.bmp", 8);
			.SetTex("./Symbols/plt.bmp", 8);

		viewer.AddPrimitive(symbol_1);

		//--------------------------------------------------
		// テキスト

		// OpenGLバージョン
		// ◆作ってはみたが日本語表示できない。
/*
		var text_1_pos = ToTude(new CUTM(52, "FC1012010000", new CAltitude(500)));

		var text_1 = new CString()
			.SetString("可也山")
			.SetPos(text_1_pos);
*/
		// ◆サイズは目分量
		var text_1 = new CGeoSymbol(200.0, 32.0, symbol_1_pos, new CCoord(50.0 + 100.0, 0.0, 0.0));

		{ 
			// インデックス付きピクセル形式のイメージからはグラフィックスオブジェクトが作成できないので32ビットARGBで作成する。
			// 背景がうまい具合に透明になっている。
			var text_canvas = new Bitmap(256, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			
			var g = Graphics.FromImage(text_canvas);

			var font = new Font("ＭＳ ゴシック", 24, GraphicsUnit.Pixel);

			// ◆背景が透明の場合はどれがきれいか？
			g.SmoothingMode = SmoothingMode.HighSpeed;

			g.DrawString("これはテストです。", font, Brushes.Blue, 0, 0);

			font.Dispose();
			g.Dispose();

			text_1.SetTex(text_canvas, 0);
		}

		viewer.AddPrimitive(text_1);

		//--------------------------------------------------
		// シンボル２

		var symbol_2_pos = ToTude(ToUTM(52, 'S', "FC2000020000", new CAltitude(2500)));

		var symbol_2 = new CGeoSymbol(100.0, 100.0, symbol_2_pos)
//			.SetTex("C:/DSF/SharedData/Symbols/Person/Red/UnKnown.bmp", 0);
			.SetTex("./Symbols/Unknown.bmp", 0);

		viewer.AddPrimitive(symbol_2);

		//--------------------------------------------------
		// 円

		// 分割数10
		// ◆DSF_NET_Geometryに同名のクラスがある。以下同じ。
		var circle_1 = new DSF_NET_Scene.CGeoCircle(10, symbol_1_pos, 1000)
			.SetLineWidth(5.0f)
			.SetColor(1.0f, 0.0f, 0.0f, 0.5f) // R,G,B,アルファ(透明度)
			.SetFill(false);

		viewer.AddPrimitive(circle_1);

		//--------------------------------------------------
		// 扇形

		// 分割数10
		var fan_1 = new DSF_NET_Scene.CGeoFan(10, symbol_2_pos, 1000, new CMil(5600), new CMil( 800))
			.SetColor(1.0f, 1.0f, 0.0f, 0.5f)
			.SetFill(true);

		viewer.AddPrimitive(fan_1);

		//--------------------------------------------------
		// 直線

		var line_1 = new DSF_NET_Scene.CGeoLine(symbol_1_pos, symbol_2_pos)
			.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
			.SetLineWidth(2.0f);

		viewer.AddPrimitive(line_1);

		//--------------------------------------------------
		// 放物線

		// 分割数10、射角1200ミル
		var parabola_1 = new CGeoParabola(10, symbol_1_pos, symbol_2_pos, new CMil(1200.0))
			.SetColor(1.0f, 0.0f, 0.0f, 0.5f)
			.SetLineWidth(2.0f);

		viewer.AddPrimitive(parabola_1);
	}
}
//---------------------------------------------------------------------------
}
