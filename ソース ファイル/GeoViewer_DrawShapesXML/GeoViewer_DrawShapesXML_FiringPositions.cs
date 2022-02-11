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
	// ���Ȑw�n�̒�`��XML�m�[�h����ǂݍ��ݕ`�悷��B
	void GeoViewer_DrawShapesXML_FiringPosition(in XmlNode map_drawing_group_xml_node)
	{
		var firing_positions = ReadFiringPositions(map_drawing_group_xml_node);

		foreach(var firing_position in firing_positions)
		{
			// ���ˌ��w�n�ЂƂ��s���ŗ�O���o���ׂ����H
			if(firing_position.BorderNodes.Count  <  2) throw new Exception("firing position border edge nodes must be more than 2");
			if(firing_position.InnerSymbol.Length != 6) throw new Exception("firing position inner symbol nodes must be 6");

			var color = Color.FromArgb(firing_position.Color);

			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;

			var line_width = firing_position.LineWidth;

			//--------------------------------------------------
			// �O����`�悷��B
			
			// �������ݒ肳��Ă��Ȃ��̂Őݒ肷��B
			foreach(var node in firing_position.BorderNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			// ��CGeoPolygon���K�v���B
			Viewer.AddPrimitive
				(new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, firing_position.BorderNodes, 20))
					.AddNode(firing_position.BorderNodes[0])); // �t�^������B���Ō�̐����͒n�ʂɉ���Ȃ��̂ł́H

			//--------------------------------------------------
			// �����́u>-<�v��`�悷��B
		 
			var inner_symbol = firing_position.InnerSymbol;

			// �������ݒ肳��Ă��Ȃ��̂Őݒ肷��B
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
