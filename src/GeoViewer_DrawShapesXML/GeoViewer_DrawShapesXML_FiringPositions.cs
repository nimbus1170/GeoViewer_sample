//
// GeoViewer_DrawShapesXML_FiringPositions.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.StickerShape;
using static DSF_NET_TacticalDrawing.XMLReader;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// 特科陣地の定義をXMLノードから読み込み描画する。
	void DrawShapesXML_FiringPosition(in XmlNode drawing_group)
	{
		var fps = ReadFiringPositions(drawing_group);

		foreach(var fp in fps)
		{
			// ◆射撃陣地ひとつが不正で例外を出すべきか？
			if(fp.BorderNodes.Count  <  2) throw new Exception("firing position border edge nodes must be more than 2");
			if(fp.InnerSymbol.Length != 6) throw new Exception("firing position inner symbol nodes must be 6");

			var name = fp.Name;

			var color = Color.FromArgb(fp.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;

			var line_width = fp.LineWidth;

			//--------------------------------------------------
			// 外縁を描画する。
			
			// ◆CGeoPolygonが必要か。
			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, fp.BorderNodes))
					.AddNode(fp.BorderNodes[0])); // フタをする。◆最後の線分は地面に沿わないのでは？

			//--------------------------------------------------
			// 内部の「>-<」を描画する。
		 
			var inner_symbol = fp.InnerSymbol;

			Viewer.AddShape(name, new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[0], inner_symbol[1]})));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[2], inner_symbol[1]})));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[1], inner_symbol[4]})));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[3], inner_symbol[4]})));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[5], inner_symbol[4]})));
		}
	}
}
//---------------------------------------------------------------------------
}
