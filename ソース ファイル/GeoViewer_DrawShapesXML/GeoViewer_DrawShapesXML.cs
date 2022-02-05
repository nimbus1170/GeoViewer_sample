//
// GeoViewer_DrawShapesXML.cs
//
//---------------------------------------------------------------------------
using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// ��������viewer��n���K�v�͂Ȃ��Ǝv�����APlaneViewerXYZ�����邽�߂ɁAPlaneViewerMainForm��GeoViewer�ł͂Ȃ�PlaneViewer�����悤�ɂȂ��Ă��܂��Ă���BXYZ�͂�͂蕪����B
	void GeoViewer_DrawShapesXML(in XmlNode drawing_xml_node)
	{
		var map_drawing_group_xml_nodes = drawing_xml_node.SelectNodes("MapDrawings/MapDrawingGroup");

		// XML�t�@�C�����̐}�`�`��O���[�v�̃m�[�h�𒀎��ɓn���ď�������B
		foreach(XmlNode map_drawing_group_xml_node in map_drawing_group_xml_nodes)
		{
			// �n�����̒�`��XML�m�[�h����ǂݍ��݁A�`�悷��B
			GeoViewer_DrawShapesXML_MineField(map_drawing_group_xml_node);

			// �h��w�n��XML�m�[�h����ǂݍ��݁A�}�`�`�惌�C���ɒǉ�����B
			GeoViewer_DrawShapesXML_DefensivePosition(map_drawing_group_xml_node);
		}
	}
}
//---------------------------------------------------------------------------
}
