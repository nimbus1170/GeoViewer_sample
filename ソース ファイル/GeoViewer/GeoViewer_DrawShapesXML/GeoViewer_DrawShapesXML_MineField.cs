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
	// �n�����̒�`��XML�m�[�h����ǂݍ��ݕ`�悷��B
	void GeoViewer_DrawShapesXML_MineField(CGeoViewer viewer, in XmlNode map_drawing_group_xml_node)
	{
		var minefields = ReadMineFields(map_drawing_group_xml_node);

		foreach(var minefield in minefields)
		{
			// ���n�����ЂƂ��s���ŗ�O���o���ׂ����H
			if(minefield.FrontEdgeNodes.Count < 2) throw new Exception("minefield front edge nodes must be more than 2");
			if(minefield.BackEdgeNodes .Count < 2) throw new Exception("minefield back edge nodes must be more than 2");

			var color = Color.FromArgb(minefield.Color);

			//--------------------------------------------------
			// �n�����̊O�g��`�悷��B

			var mf_line = new CGeoPolyline()
				.SetColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f)
				.SetLineWidth(minefield.LineWidth);

			viewer.AddPrimitive(mf_line);

			// Polyline�ɂȂ��邽�߂ɋt���ɂ���B
			minefield.BackEdgeNodes.Reverse();

			// ��FrontEdgeNodes���g���񂷁B
			minefield.FrontEdgeNodes.AddRange(minefield.BackEdgeNodes);

			// �������ݒ肳��Ă��Ȃ��̂Őݒ肷��B
			foreach(var node in minefield.FrontEdgeNodes)
				node.Altitude.Set(20, DAltitudeBase.AGL);

			var stripped_nodes = MakeStickerLineStripNodesWP(PolygonZoomLevel, minefield.FrontEdgeNodes, 20);

			mf_line.AddNodes(stripped_nodes);

			// �Ō�ɊW������B
			mf_line.AddNode(minefield.FrontEdgeNodes[0]);

			//--------------------------------------------------
			// �n������ʃV���{����`�悷��B
		 
			// �V���{���̒��a
			// ���ڕ���(m)
			int type_symbol_r = 20;

			//--------------------------------------------------
			// �n������ʃV���{��(AT)��`�悷��B
		 
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
			// �n������ʃV���{��(AP)��`�悷��B
		 
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

				// AP�̃q�Q
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
