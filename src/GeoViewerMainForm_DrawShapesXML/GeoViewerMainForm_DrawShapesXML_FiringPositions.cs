//
// GeoViewer_DrawShapesXML_FiringPositions.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
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

			var color_b = Color.FromArgb(fp.Color);

			var color_f = new CColorF
				(color_b.R / 255.0f,
				 color_b.G / 255.0f,
				 color_b.B / 255.0f,
				 color_b.A / 255.0f);

			var line_width = fp.LineWidth;

			//--------------------------------------------------
			// �O����`�悷��B
			
			// ��CGeoPolygon���K�v���B
			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(color_f)
					.SetLineWidth(line_width)
					.AddNodes(MakeGridCrossPointsWP(fp.BorderNodes, PolygonZoomLevel))
					.AddNode(fp.BorderNodes[0])); // �t�^������B���Ō�̐����͒n�ʂɉ���Ȃ��̂ł́H

			//--------------------------------------------------
			// �����́u>-<�v��`�悷��B
		 
			var inner_symbol = fp.InnerSymbol;

			Viewer.AddShape(name, new CGeoPolyline().SetColor(color_f).SetLineWidth(line_width).AddNodes(MakeGridCrossPointsWP(new List<CLgLt>(){inner_symbol[0], inner_symbol[1]}, PolygonZoomLevel)));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(color_f).SetLineWidth(line_width).AddNodes(MakeGridCrossPointsWP(new List<CLgLt>(){inner_symbol[2], inner_symbol[1]}, PolygonZoomLevel)));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(color_f).SetLineWidth(line_width).AddNodes(MakeGridCrossPointsWP(new List<CLgLt>(){inner_symbol[1], inner_symbol[4]}, PolygonZoomLevel)));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(color_f).SetLineWidth(line_width).AddNodes(MakeGridCrossPointsWP(new List<CLgLt>(){inner_symbol[3], inner_symbol[4]}, PolygonZoomLevel)));
			Viewer.AddShape(name, new CGeoPolyline().SetColor(color_f).SetLineWidth(line_width).AddNodes(MakeGridCrossPointsWP(new List<CLgLt>(){inner_symbol[5], inner_symbol[4]}, PolygonZoomLevel)));
		}
	}
}
//---------------------------------------------------------------------------
}
