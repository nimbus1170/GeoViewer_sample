//
// GeoViewer_DrawShapesXML_FiringPositions.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.StickerPrimitive;
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
	void GeoViewer_DrawShapesXML_FiringPosition(in XmlNode map_drawing_group_xml_node)
	{
		var firing_positions = ReadFiringPositions(map_drawing_group_xml_node);

		foreach(var firing_position in firing_positions)
		{
			// ◆射撃陣地ひとつが不正で例外を出すべきか？
			if(firing_position.BorderNodes.Count  <  2) throw new Exception("firing position border edge nodes must be more than 2");
			if(firing_position.InnerSymbol.Length != 6) throw new Exception("firing position inner symbol nodes must be 6");

			var color = Color.FromArgb(firing_position.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;

			var line_width = firing_position.LineWidth;

			//--------------------------------------------------
			// 外縁を描画する。
			
			// 高さが設定されていないので設定する。
			foreach(var node in firing_position.BorderNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			// ◆CGeoPolygonが必要か。
			Viewer.AddPrimitive
				(new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, firing_position.BorderNodes, 20))
					.AddNode(firing_position.BorderNodes[0])); // フタをする。◆最後の線分は地面に沿わないのでは？

			//--------------------------------------------------
			// 内部の「>-<」を描画する。
		 
			var inner_symbol = firing_position.InnerSymbol;

			// 高さが設定されていないので設定する。
			for(var i = 0; i < 6; ++i)
				inner_symbol[i].Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddPrimitive(new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[0], inner_symbol[1]}, 20)));
			Viewer.AddPrimitive(new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[2], inner_symbol[1]}, 20)));
			Viewer.AddPrimitive(new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[1], inner_symbol[4]}, 20)));
			Viewer.AddPrimitive(new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[3], inner_symbol[4]}, 20)));
			Viewer.AddPrimitive(new CGeoPolyline().SetColor(r, g, b, a).SetLineWidth(line_width).AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, new List<CLgLt>(){inner_symbol[5], inner_symbol[4]}, 20)));
		}
	}
}
//---------------------------------------------------------------------------
}
