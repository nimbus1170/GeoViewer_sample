//
// GeoViewer_DrawShapesXML_DefensivePosition.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.CDefensivePosition;
using static DSF_NET_TacticalDrawing.StickerPrimitive;
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
	void GeoViewer_DrawShapesXML_DefensivePosition(in XmlNode map_drawing_group_xml_node)
	{
		var defensive_positions = ReadDefensivePositions(map_drawing_group_xml_node);

		foreach(var defensive_position in defensive_positions)
		{
			// ���h��w�n�ЂƂ��s���ŗ�O���o���ׂ����H
			if(defensive_position.BorderNodes   .Count < 2) throw new Exception("defensive position border nodes must be more than 2");
			if(defensive_position.UnitLevelNodes.Count < 2) throw new Exception("defensive position unit level nodes must be more than 2");

			var color = Color.FromArgb(defensive_position.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;

			var line_width = defensive_position.LineWidth;

			//--------------------------------------------------
			// �O����`�悷��B
		
			// �������ݒ肳��Ă��Ȃ��̂Őݒ肷��B
			foreach(var node in defensive_position.BorderNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddPrimitive
				(new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, defensive_position.BorderNodes, 20)));
		
			//--------------------------------------------------
			// �����K�͕W����`�悷��B
			// ����芸���������̂݁B
		
			var unit_level_line_p1 = defensive_position.UnitLevelNodes[0];
			var unit_level_line_p2 = defensive_position.UnitLevelNodes[1];

			unit_level_line_p1.Altitude.Set(20, DAltitudeBase.AGL);
			unit_level_line_p2.Altitude.Set(20, DAltitudeBase.AGL);

			Viewer.AddPrimitive
				(new CGeoLine(unit_level_line_p1, unit_level_line_p2)
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width));
		}
	}
}
//---------------------------------------------------------------------------
}
