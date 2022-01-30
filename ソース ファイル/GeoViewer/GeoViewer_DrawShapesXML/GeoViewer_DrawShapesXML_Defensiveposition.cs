//
// GeoViewer_DrawShapesXML_DefensivePosition.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using static DSF_NET_TacticalDrawing.CDefensivePosition;
using static DSF_NET_TacticalDrawing.StickerPrimitive;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	// 防御陣地の定義をXMLノードから読み込み描画する。
	void GeoViewerDrawDefensivePositionsXML(CGeoViewer viewer, in XmlNode map_drawing_group_xml_node)
	{
		var defensive_positions = ReadDefensivePositions(map_drawing_group_xml_node);

		foreach(var defensive_position in defensive_positions)
		{
			// ◆防御陣地一つが不正で例外を出すべきか？
			if(defensive_position.BorderNodes   .Count < 2) throw new Exception("defensive position border nodes must be more than 2");
			if(defensive_position.UnitLevelNodes.Count < 2) throw new Exception("defensive position unit level nodes must be more than 2");

			var color = Color.FromArgb(defensive_position.Color);

			//--------------------------------------------------
			// 外縁を描画する。
		
			var dp_line = new CGeoPolyline()
				.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
				.SetLineWidth(defensive_position.LineWidth);

			viewer.AddPrimitive(dp_line);

			// 高さが設定されていないので設定する。
			foreach(var node in defensive_position.BorderNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			var stripped_nodes = MakeStickerLineStripNodesWP(PolygonZoomLevel, defensive_position.BorderNodes, 20);

			dp_line.AddNodes(stripped_nodes);
		
			//--------------------------------------------------
			// 部隊規模標示を描画する。
		
			var unit_level_line_p1 = defensive_position.UnitLevelNodes[0];
			var unit_level_line_p2 = defensive_position.UnitLevelNodes[1];

			unit_level_line_p1.Altitude.Set(20, DAltitudeBase.AGL);
			unit_level_line_p2.Altitude.Set(20, DAltitudeBase.AGL);

			viewer.AddPrimitive
				(new CGeoLine(unit_level_line_p1, unit_level_line_p2)
					.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
					.SetLineWidth(defensive_position.LineWidth));
		}
	}
}
//---------------------------------------------------------------------------
}
