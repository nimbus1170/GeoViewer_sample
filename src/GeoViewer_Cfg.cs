//
// GeoViewer_Cfg.cs
// �n�`�r���[�A�̐ݒ�t�@�C���̓��e
//
// ���{���͕ʂɂ���K�v�͂Ȃ���������Ȃ����A�����LgLt��WP��Tile�ɕ�����Ă���A�������̂������Ă���̂œZ�߂�B
// ��XML��n�[�h�R�[�h���̃o���G�[�V����������̂ňˑ����Ȃ��悤�ɕʂɂ���B
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

	// �����y�n���@�f�[�^�Œ�

	string GSIImageTileFolder;
	string GSIImageTileExt;
	
	string GSIElevationTileFolder;

	string GSIGeoidModelFile;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	void ReadCfgFromFile(in string cfg_file_name)
	{
		// ���ݒ薢��`�ɂ��Ă͗�O���o��̂ŁA�ݒ���ɑ΂��Ă͏ڍׂȃ��b�Z�[�W��\������悤�ɂ������B

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_file_name);

		var geo_viewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg");

		//--------------------------------------------------
		// �ʂ̒n��ݒ�

		var map_cfg = geo_viewer_cfg.SelectSingleNode("MapCfg");

		Title =	map_cfg.Attributes["Title"].InnerText;

		PolygonSize = ToInt32(map_cfg.Attributes["PolygonSize"	].InnerText);

		PolygonZoomLevel = ToInt32(map_cfg.Attributes["PolygonZoomLevel"].InnerText);
		ImageZoomLevel	 = ToInt32(map_cfg.Attributes["ImageZoomLevel"  ].InnerText);

		// �N�����v�O�̊J�n�E�I���o�ܓx���W
		// ���v���[����WP�P�ʂɃN�����v����̂ŁA�N�����v�O�̂��̂��g�p���Ȃ��悤�ɂ���B
		StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
		EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));
		
		//--------------------------------------------------
		// �S�ʐݒ�
		// ��XML�ǂݍ��݂̐��ۂ́A�X��NULL���肷��̂ł͂Ȃ��A��芸������O���o������B

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
