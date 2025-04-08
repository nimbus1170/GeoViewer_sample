//
// GeoViewer_DrawShapesXML.cs
//
//---------------------------------------------------------------------------
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		void DrawShapesXML()
		{
			if(Cfg.DrawingFileName == "") return;

			var drawing_doc = new XmlDocument();

			drawing_doc.Load(Cfg.DrawingFileName);

			var drawing_groups = drawing_doc.SelectNodes("DrawingGroups/DrawingGroup");

			// XMLファイル内の図形描画グループのノードを逐次に渡して処理する。
			foreach(XmlNode drawing_group in drawing_groups)
			{
				// 地雷原の定義をXMLノードから読み込み、描画する。
				DrawShapesXML_MineField(drawing_group);

				// 防御陣地をXMLノードから読み込み、描画する。
				DrawShapesXML_DefensivePosition(drawing_group);

				// 特科陣地をXMLノードから読み込み、描画する。
				DrawShapesXML_FiringPosition(drawing_group);
			}
		}
	}
}
