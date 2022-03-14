//
// GeoViewer_DrawShapesXML_DefensivePosition.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.CDefensivePosition;
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
	// �h��w�n�̒�`��XML�m�[�h����ǂݍ��ݕ`�悷��B
	void DrawShapesXML_DefensivePosition(in XmlNode map_drawing_group_xml_node)
	{
		var defensive_positions = ReadDefensivePositions(map_drawing_group_xml_node);

		foreach(var dp in defensive_positions)
		{
			// ���h��w�n�ЂƂ��s���ŗ�O���o���ׂ����H
			if(dp.BorderNodes   .Count < 2) throw new Exception("defensive position border nodes must be more than 2");
			if(dp.UnitLevelNodes.Count < 2) throw new Exception("defensive position unit level nodes must be more than 2");

			var name = dp.Name;

			var color = Color.FromArgb(dp.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;

			var line_width = dp.LineWidth;

			//--------------------------------------------------
			// �O����`�悷��B
		
			// �������ݒ肳��Ă��Ȃ��̂Őݒ肷��B
			foreach(var node in dp.BorderNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, dp.BorderNodes, 20)));
		
			//--------------------------------------------------
			// �����K�͕W����`�悷��B
			// ����芸���������̂݁B
		
			var unit_level_line_p1 = dp.UnitLevelNodes[0];
			var unit_level_line_p2 = dp.UnitLevelNodes[1];

			unit_level_line_p1.Altitude.Set(20, DAltitudeBase.AGL);
			unit_level_line_p2.Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddShape
				(name,
				 new CGeoLine(unit_level_line_p1, unit_level_line_p2)
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width));
		}
	}
}
//---------------------------------------------------------------------------
}
