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
	// ◆ここにviewerを渡す必要はないと思うが、PlaneViewerXYZがあるために、PlaneViewerMainFormがGeoViewerではなくPlaneViewerを持つようになってしまっている。XYZはやはり分ける。
	void GeoViewer_DrawShapesXML(in XmlNode drawing_xml_node)
	{
		var map_drawing_group_xml_nodes = drawing_xml_node.SelectNodes("MapDrawings/MapDrawingGroup");

		// XMLファイル内の図形描画グループのノードを逐次に渡して処理する。
		foreach(XmlNode map_drawing_group_xml_node in map_drawing_group_xml_nodes)
		{
			// 地雷原の定義をXMLノードから読み込み、描画する。
			GeoViewer_DrawShapesXML_MineField(map_drawing_group_xml_node);

			// 防御陣地をXMLノードから読み込み、図形描画レイヤに追加する。
			GeoViewer_DrawShapesXML_DefensivePosition(map_drawing_group_xml_node);
		}
	}
}
//---------------------------------------------------------------------------
}
