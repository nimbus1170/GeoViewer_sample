//
// GeoViewer_DrawShapesXML.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;

using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{

	void GeoViewerDrawShapesXML(CGeoViewer viewer, in XmlNode drawing_xml_node)
	{
		var map_drawing_group_xml_nodes = drawing_xml_node.SelectNodes("MapDrawings/MapDrawingGroup");

		// XML�t�@�C�����̐}�`�`��O���[�v�̃m�[�h�𒀎��ɓn���ď�������B
		foreach(XmlNode map_drawing_group_xml_node in map_drawing_group_xml_nodes)
		{
			//--------------------------------------------------
			// �n�����̒�`��XML�m�[�h����ǂݍ��݁A�`�悷��B

			GeoViewerDrawMineFieldsXML(viewer, map_drawing_group_xml_node);

			//--------------------------------------------------
			// �h��w�n��XML�m�[�h����ǂݍ��݁A�}�`�`�惌�C���ɒǉ�����B

			GeoViewerDrawDefensivePositionsXML(viewer, map_drawing_group_xml_node);
		}
	}
}
//---------------------------------------------------------------------------
}
