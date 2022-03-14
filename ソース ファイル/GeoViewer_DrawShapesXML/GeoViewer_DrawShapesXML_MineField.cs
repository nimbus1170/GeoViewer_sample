//
// GeoViewer_DrawShapesXML_MineField.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.CMineField;
using static DSF_NET_TacticalDrawing.StickerShape;
using static DSF_NET_TacticalDrawing.XMLReader;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// 地雷原の定義をXMLノードから読み込み描画する。
	void DrawShapesXML_MineField(in XmlNode map_drawing_group_xml_node)
	{
		var minefields = ReadMineFields(map_drawing_group_xml_node);

		foreach(var mf in minefields)
		{
			// ◆地雷原ひとつが不正で例外を出すべきか？
			if(mf.FrontEdgeNodes.Count < 2) throw new Exception("minefield front edge nodes must be more than 2");
			if(mf.BackEdgeNodes .Count < 2) throw new Exception("minefield back edge nodes must be more than 2");

			var name = mf.Name;

			var color = Color.FromArgb(mf.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;
			
			var line_width = mf.LineWidth;

			//--------------------------------------------------
			// 外縁を描画する。

			// Polylineにつなげるために逆順にする。
			mf.BackEdgeNodes.Reverse();

			// ◆FrontEdgeNodesを使い回す。
			mf.FrontEdgeNodes.AddRange(mf.BackEdgeNodes);

			// 高さが設定されていないので設定する。
			foreach(var node in mf.FrontEdgeNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, mf.FrontEdgeNodes, 20))
					.AddNode(mf.FrontEdgeNodes[0]));	// フタをする。◆最後の線分は地面に沿わないのでは？

			//--------------------------------------------------
			// 地雷原種別シンボルを描画する。
		 
			// シンボルの直径
			// ◆目分量(m)
			int type_symbol_r = 20;

			//--------------------------------------------------
			// 地雷原種別シンボル(AT)を描画する。
		 
			foreach(var type_symbol_ct in mf.TypeSymbolPos_AT)
			{
				type_symbol_ct.Altitude.Set(20, DAltitudeBase.AGL);

				Viewer.AddShape
					(name,
					 new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(r, g, b, a)
						.SetFill(true));
			}

			//--------------------------------------------------
			// 地雷原種別シンボル(AP)を描画する。
		 
			var type_symbol_i = mf.TypeSymbolPos_AP.GetEnumerator();

			while(type_symbol_i.MoveNext())
			{
				var type_symbol_ct = type_symbol_i.Current;

				type_symbol_ct.Altitude.Set(20, DAltitudeBase.AGL);

				Viewer.AddShape
					(name,
					 new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(r, g, b, a)
						.SetFill(true));

				// APのヒゲ
				type_symbol_i.MoveNext(); var ap_top1 = type_symbol_i.Current;
				type_symbol_i.MoveNext(); var ap_top2 = type_symbol_i.Current;

				ap_top1.Altitude.Set(20, DAltitudeBase.AGL);
				ap_top2.Altitude.Set(20, DAltitudeBase.AGL);

				Viewer.AddShape
					(name,
					 new CGeoPolyline()
						.SetColor(r, g, b, a)
						.SetLineWidth(line_width)
						.AddNode(ap_top1)
						.AddNode(type_symbol_ct)
						.AddNode(ap_top2));
			}
		}
	}
}
//---------------------------------------------------------------------------
}
