//
// GeoViewer_DrawShapesXML_MineField.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;

using static DSF_NET_TacticalDrawing.StickerShape;
using static DSF_NET_TacticalDrawing.XMLReader;

using System.Xml;
using DSF_NET_Color;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// �n�����̒�`��XML�m�[�h����ǂݍ��ݕ`�悷��B
	void DrawShapesXML_MineField(in XmlNode drawing_group)
	{
		var mfs = ReadMineFields(drawing_group);

		foreach(var mf in mfs)
		{
			// ���n�����ЂƂ��s���ŗ�O���o���ׂ����H
			if(mf.FrontEdgeNodes.Count < 2) throw new Exception("minefield front edge nodes must be more than 2");
			if(mf.BackEdgeNodes .Count < 2) throw new Exception("minefield back edge nodes must be more than 2");

			var name = mf.Name;

			var color_b = Color.FromArgb(mf.Color);

			var color_f = new CColorF
				(color_b.R / 255.0f,
				 color_b.G / 255.0f,
				 color_b.B / 255.0f,
				 color_b.A / 255.0f);
			
			var line_width = mf.LineWidth;

			//--------------------------------------------------
			// �O����`�悷��B

			// Polyline�ɂȂ��邽�߂ɋt���ɂ���B
			mf.BackEdgeNodes.Reverse();

			// ��FrontEdgeNodes���g���񂷁B
			mf.FrontEdgeNodes.AddRange(mf.BackEdgeNodes);

			Viewer.AddShape
				(name,
				 new CGeoPolyline()
					.SetColor(color_f)
					.SetLineWidth(line_width)
					.AddNodes(MakeGridCrossPointsWP(mf.FrontEdgeNodes, MeshZoomLevel))
					.AddNode(mf.FrontEdgeNodes[0])); // �t�^������B���Ō�̐����͒n�ʂɉ���Ȃ��̂ł́H

			//--------------------------------------------------
			// �n������ʃV���{����`�悷��B
		 
			// �V���{���̒��a
			// ���ڕ���(m)
			const int type_symbol_r = 20;

			//--------------------------------------------------
			// �n������ʃV���{��(AT)��`�悷��B
		 
			foreach(var type_symbol_ct in mf.TypeSymbolPos_AT)
			{
				Viewer.AddShape
					(name,
					 new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(color_f)
						.SetFill(true));
			}

			//--------------------------------------------------
			// �n������ʃV���{��(AP)��`�悷��B
		 
			var type_symbol_i = mf.TypeSymbolPos_AP.GetEnumerator();

			while(type_symbol_i.MoveNext())
			{
				var type_symbol_ct = type_symbol_i.Current;

				Viewer.AddShape
					(name,
					 new CGeoCircle(12, type_symbol_ct, type_symbol_r)
						.SetColor(color_f)
						.SetFill(true));

				// AP�̃q�Q
				type_symbol_i.MoveNext(); var ap_top1 = type_symbol_i.Current;
				type_symbol_i.MoveNext(); var ap_top2 = type_symbol_i.Current;

				Viewer.AddShape
					(name,
					 new CGeoPolyline()
						.SetColor(color_f)
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
