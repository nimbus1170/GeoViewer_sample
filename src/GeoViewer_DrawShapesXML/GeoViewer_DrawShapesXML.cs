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
	void DrawShapesXML()
	{
		var map_drawing_xml = new XmlDocument();

		map_drawing_xml.Load(MapDrawingFileName);

		var map_drawing_group_xml_nodes = map_drawing_xml.SelectNodes("MapDrawings/MapDrawingGroup");

		// XML�t�@�C�����̐}�`�`��O���[�v�̃m�[�h�𒀎��ɓn���ď�������B
		foreach(XmlNode map_drawing_group_xml_node in map_drawing_group_xml_nodes)
		{
			// �n�����̒�`��XML�m�[�h����ǂݍ��݁A�`�悷��B
			DrawShapesXML_MineField(map_drawing_group_xml_node);

			// �h��w�n��XML�m�[�h����ǂݍ��݁A�`�悷��B
			DrawShapesXML_DefensivePosition(map_drawing_group_xml_node);

			// ���Ȑw�n��XML�m�[�h����ǂݍ��݁A�`�悷��B
			DrawShapesXML_FiringPosition(map_drawing_group_xml_node);
		}
	}
}
//---------------------------------------------------------------------------
}
