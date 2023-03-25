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
	// ���Ȑw�n�̒�`��XML�m�[�h����ǂݍ��ݕ`�悷��B
	void DrawShapesXML_FiringPosition(in XmlNode drawing_group)
	{
		var fps = ReadFiringPositions(drawing_group);

		foreach(var fp in fps)
		{
			// ���ˌ��w�n�ЂƂ��s���ŗ�O���o���ׂ����H
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
			// �O����`�悷��B
			
			// ��CGeoPolygon���K�v���B
			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(r, g, b, a)
					.SetLineWidth(line_width)
					.AddNodes(MakeStickerLineStripNodesWP(PolygonZoomLevel, fp.BorderNodes))
					.AddNode(fp.BorderNodes[0])); // �t�^������B���Ō�̐����͒n�ʂɉ���Ȃ��̂ł́H

			//--------------------------------------------------
			// �����́u>-<�v��`�悷��B
		 
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
