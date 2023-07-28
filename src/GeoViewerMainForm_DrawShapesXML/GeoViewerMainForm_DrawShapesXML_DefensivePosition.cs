//
// GeoViewerMainForm_DrawShapesXML_DefensivePosition.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.StickerShape;
using static DSF_NET_TacticalDrawing.XMLReader;

using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// 防御陣地の定義をXMLノードから読み込み描画する。
	void DrawShapesXML_DefensivePosition(in XmlNode drawing_group)
	{
		var dps = ReadDefensivePositions(drawing_group);

		foreach(var dp in dps)
		{
			// ◆防御陣地ひとつが不正で例外を出すべきか？
			if(dp.BorderNodes   .Count < 2) throw new Exception("defensive position border nodes must be more than 2");
			if(dp.UnitLevelNodes.Count < 2) throw new Exception("defensive position unit level nodes must be more than 2");

			var name = dp.Name;

			var color_b = Color.FromArgb(dp.Color);

			var color_f = new CColorF
				(color_b.R / 255.0f,
				 color_b.G / 255.0f,
				 color_b.B / 255.0f,
				 color_b.A / 255.0f);

			var line_width = dp.LineWidth;

			//--------------------------------------------------
			// 外縁を描画する。
		
			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(color_f)
					.SetLineWidth(line_width)
					.AddNodes(MakeGridCrossPointsWP(dp.BorderNodes, MeshZoomLevel)));
		
			//--------------------------------------------------
			// 部隊規模標示を描画する。
			// ◆取り敢えず中隊のみ。
		
			var unit_level_line_p1 = dp.UnitLevelNodes[0];
			var unit_level_line_p2 = dp.UnitLevelNodes[1];

			Viewer.AddShape
				(name,
				 new CGeoLine(unit_level_line_p1, unit_level_line_p2)
					.SetColor(color_f)
					.SetLineWidth(line_width));
		}
	}
}
//---------------------------------------------------------------------------
}
