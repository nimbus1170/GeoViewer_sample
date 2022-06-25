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

		// XMLファイル内の図形描画グループのノードを逐次に渡して処理する。
		foreach(XmlNode map_drawing_group_xml_node in map_drawing_group_xml_nodes)
		{
			// 地雷原の定義をXMLノードから読み込み、描画する。
			DrawShapesXML_MineField(map_drawing_group_xml_node);

			// 防御陣地をXMLノードから読み込み、描画する。
			DrawShapesXML_DefensivePosition(map_drawing_group_xml_node);

			// 特科陣地をXMLノードから読み込み、描画する。
			DrawShapesXML_FiringPosition(map_drawing_group_xml_node);
		}
	}
}
//---------------------------------------------------------------------------
}
