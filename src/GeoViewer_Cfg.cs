//
// GeoViewer_Cfg.cs
// 地形ビューアの設定ファイルの内容
//
// ◆本来は別にする必要はないかもしれないが、現状はLgLtとWPとTileに分かれており、同じものを書いているので纏める。
// ◆XMLやハードコード等のバリエーションがあるので依存しないように別にする。
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using System.Windows.Forms;
using System.Xml;

using static DSF_NET_TacticalDrawing.XMLReader;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	string  Title;

	int PolygonSize;

	int PolygonZoomLevel;
	int ImageZoomLevel;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	string MapDataFolder;

	// ◆国土地理院データ固定

	string GSIImageTileFolder;
	string GSIImageTileExt;
	
	string GSIElevationTileFolder;

	string GSIGeoidModelFile;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	void ReadCfgFromFile(in string cfg_file_name)
	{
		// ▼設定未定義については例外が出るので、設定誤りに対しては詳細なメッセージを表示するようにしたい。

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_file_name);

		var geo_viewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg");

		//--------------------------------------------------
		// 個別の地域設定

		var map_cfg = geo_viewer_cfg.SelectSingleNode("MapCfg");

		Title =	map_cfg.Attributes["Title"].InnerText;

		PolygonSize = ToInt32(map_cfg.Attributes["PolygonSize"	].InnerText);

		PolygonZoomLevel = ToInt32(map_cfg.Attributes["PolygonZoomLevel"].InnerText);
		ImageZoomLevel	 = ToInt32(map_cfg.Attributes["ImageZoomLevel"  ].InnerText);

		// クランプ前の開始・終了経緯度座標
		// ◆プレーンをWP単位にクランプするので、クランプ前のものを使用しないようにする。
		StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
		EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));
		
		//--------------------------------------------------
		// 全般設定
		// ◆XML読み込みの成否は、個々にNULL判定するのではなく、取り敢えず例外を出させる。

		MapDataFolder = geo_viewer_cfg.SelectSingleNode("MapData").Attributes["Folder"].InnerText;

		GSIImageTileFolder = geo_viewer_cfg.SelectSingleNode("GSIImageTiles").Attributes["Folder"].InnerText;
		GSIImageTileExt	   = geo_viewer_cfg.SelectSingleNode("GSIImageTiles").Attributes["Ext"   ].InnerText;

		GSIElevationTileFolder = geo_viewer_cfg.SelectSingleNode("GSIElevationTiles").Attributes["Folder"].InnerText;

		GSIGeoidModelFile = geo_viewer_cfg.SelectSingleNode("GSIGeoidModel").Attributes["File"].InnerText;

		var grid_cfg = geo_viewer_cfg.SelectSingleNode("Grid");

		GridFontSize = ToInt32(grid_cfg.Attributes["FontSize"].InnerText);

		GridOverlayCfg = grid_cfg.SelectSingleNode("GridOverlay");

		DrawingFileName = map_cfg.SelectSingleNode("Drawings")?.Attributes["File"]?.InnerText;
	}
}
//---------------------------------------------------------------------------
}
