//
// GeoViewer_DrawShapesXML_MineField.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using static DSF_NET_TacticalDrawing.CMineField;
using static DSF_NET_TacticalDrawing.StickerPrimitive;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	// 地雷原の定義をXMLノードから読み込み描画する。
	void GeoViewer_DrawShapesXML_MineField(CGeoViewer viewer, in XmlNode map_drawing_group_xml_node)
	{
		var minefields = ReadMineFields(map_drawing_group_xml_node);

		foreach(var minefield in minefields)
		{
			// ◆地雷原ひとつが不正で例外を出すべきか？
			if(minefield.FrontEdgeNodes.Count < 2) throw new Exception("minefield front edge nodes must be more than 2");
			if(minefield.BackEdgeNodes .Count < 2) throw new Exception("minefield back edge nodes must be more than 2");

			var color = Color.FromArgb(minefield.Color);

			//--------------------------------------------------
			// 地雷原の外枠を描画する。

			var mf_line = new CGeoPolyline()
				.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
				.SetLineWidth(minefield.LineWidth);

			viewer.AddPrimitive(mf_line);

			// Polylineにつなげるために逆順にする。
			minefield.BackEdgeNodes.Reverse();

			// ◆FrontEdgeNodesを使い回す。
			minefield.FrontEdgeNodes.AddRange(minefield.BackEdgeNodes);

			// 高さが設定されていないので設定する。
			foreach(var node in minefield.FrontEdgeNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			var stripped_nodes = MakeStickerLineStripNodesWP(PolygonZoomLevel, minefield.FrontEdgeNodes, 20);

			mf_line.AddNodes(stripped_nodes);

			// 最後に蓋をする。
			mf_line.AddNode(minefield.FrontEdgeNodes[0]);

			//--------------------------------------------------
			// 地雷原種別シンボルを描画する。
		 
			// シンボルの直径
			// ◆目分量(m)
			int type_symbol_r = 20;

			//--------------------------------------------------
			// 地雷原種別シンボル(AT)を描画する。
		 
			foreach(var type_symbol_ct in minefield.TypeSymbolPos_AT)
			{
				type_symbol_ct.Altitude.Set(20, DAltitudeBase.AGL);

				viewer.AddPrimitive
					(new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
						.SetLineWidth(1.0f)
						.SetFill(true));
			}

			//--------------------------------------------------
			// 地雷原種別シンボル(AP)を描画する。
		 
			var type_symbol_i = minefield.TypeSymbolPos_AP.GetEnumerator();

			while(type_symbol_i.MoveNext())
			{
				var type_symbol_ct = type_symbol_i.Current;

				type_symbol_ct.Altitude.Set(20, DAltitudeBase.AGL);

				viewer.AddPrimitive
					(new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
						.SetLineWidth(1.0f)
						.SetFill(true));

				// APのヒゲ
				type_symbol_i.MoveNext(); var ap_top1 = type_symbol_i.Current;
				type_symbol_i.MoveNext(); var ap_top2 = type_symbol_i.Current;

				ap_top1.Altitude.Set(20, DAltitudeBase.AGL);
				ap_top2.Altitude.Set(20, DAltitudeBase.AGL);

				viewer.AddPrimitive
					(new CGeoPolyline()
						.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
						.SetLineWidth(minefield.LineWidth)
						.AddNode(ap_top1)
						.AddNode(type_symbol_ct)
						.AddNode(ap_top2));
			}
		}
	}
}
//---------------------------------------------------------------------------
}
