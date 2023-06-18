//
// GeoViewer_Cfg.cs
// �n�`�r���[�A�̐ݒ�t�@�C���̓��e
//
// ���{���͕ʂɂ���K�v�͂Ȃ���������Ȃ����A�����LgLt��WP��Tile�ɕ�����Ă���A�������̂������Ă���̂œZ�߂�B
// ��XML��n�[�h�R�[�h���̃o���G�[�V����������̂ňˑ����Ȃ��悤�ɕʂɂ���B
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using System.Xml;

using static DSF_NET_TacticalDrawing.XMLReader;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	bool ToShowDebugInfo;

	string PlaneMode;

	string Title;

	int PolygonZoomLevel;
	int ImageZoomLevel;

	int PolygonSize;

	double NearPlane;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	string MapDataFolder;

	// �����y�n���@�f�[�^�Œ�

	double LASMargin;

	// �����y�n���@�f�[�^�Œ�

	string GSIImageTileFolder;
	string GSIImageTileExt;
	
	string GSIPhotoTileFolder;
	string GSIPhotoTileExt;
	
	string GSIElevationTileFolder;

	string GSIGeoidModelFile;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	void ReadCfgFromFile(in string cfg_fname)
	{
		// ���ݒ薢��`�ɂ��Ă͗�O���o��̂ŁA�ݒ���ɑ΂��Ă͏ڍׂȃ��b�Z�[�W��\������悤�ɂ������B

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_fname);

		//--------------------------------------------------

		var geoviewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg")?? throw new Exception("tag GeoViewerCfg not found (" + cfg_fname + ")");

		//--------------------------------------------------

		ToShowDebugInfo = (geoviewer_cfg.SelectSingleNode("ToShowDebugInfo") != null)? true: false;;

		//--------------------------------------------------

		if(geoviewer_cfg.SelectSingleNode("PlaneMode") != null)
			PlaneMode = geoviewer_cfg.SelectSingleNode("PlaneMode").Attributes["Mode"].InnerText;

		//--------------------------------------------------
		// �n��ݒ�

		var to_select_map_cfg = geoviewer_cfg.SelectSingleNode("ToSelectMapCfgFile");

		if(to_select_map_cfg != null)
		{
			// �n��ݒ��ʃt�@�C������ǂށB

			OpenFileDialog of_dialog = new ()
				{ Title  = "�n��ݒ�t�@�C�����J��",
				  Filter = "�n��ݒ�t�@�C��(*.mapcfg.xml)|*.mapcfg.xml" };

			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ���I�����Ȃ��B
				Close();

			// ���ċN�Ăяo���������v���H
			ReadCfgFromFile(of_dialog.FileName);

			of_dialog.Dispose();
		}
		else
		{
			// �n��ݒ�����̃t�@�C������ǂށB

			var map_cfg = geoviewer_cfg.SelectSingleNode("MapCfg")?? throw new Exception("tag MapCfg not found (" + cfg_fname + ")");

			Title =	map_cfg.Attributes["Title"].InnerText;

			PolygonSize = ToInt32(map_cfg.Attributes["PolygonSize"].InnerText);

			PolygonZoomLevel = ToInt32(map_cfg.Attributes["PolygonZoomLevel"].InnerText);
			ImageZoomLevel	 = ToInt32(map_cfg.Attributes["ImageZoomLevel"  ].InnerText);

			NearPlane = ToDouble(map_cfg.Attributes["NearPlane"].InnerText);

			// �N�����v�O�̊J�n�E�I���o�ܓx���W
			// ���v���[����WP�P�ʂɃN�����v����̂ŁA�N�����v�O�̂��̂��g�p���Ȃ��悤�ɂ���B
			StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
			EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));

			DrawingFileName = map_cfg.SelectSingleNode("Drawings")?.Attributes["File"]?.InnerText;
		}

		//--------------------------------------------------
		// LAS�t�@�C����ǂށB
		// ���_�u���Đݒ肳�ꂽ��ǂ�����H

		var lasview_cfg = geoviewer_cfg.SelectSingleNode("ToSelectLASFile");
		
		if(lasview_cfg != null)
		{
			// LASView�_�C�A���O

			LASMargin = ToDouble(lasview_cfg.Attributes["Margin"].InnerText);

			PolygonZoomLevel = ToInt32(lasview_cfg.Attributes["PolygonZoomLevel"].InnerText);
			ImageZoomLevel	 = ToInt32(lasview_cfg.Attributes["ImageZoomLevel"  ].InnerText);

			NearPlane = ToDouble(lasview_cfg.Attributes["NearPlane"].InnerText);

			OpenFileDialog of_dialog = new ()
				{ Title  = "LAS�t�@�C�����J��",
				  Filter = "LAS�t�@�C��(*.las)|*.las" };

			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ���I�����Ȃ��B
				Close();

			ReadLASFromFile(of_dialog.FileName);

			of_dialog.Dispose();
		}
		
		//--------------------------------------------------
		// �n�}�f�[�^�ݒ�
		// ��XML�ǂݍ��݂̐��ۂ́A�X��NULL���肷��̂ł͂Ȃ��A��芸������O���o������B

		var map_data_cfg = geoviewer_cfg.SelectSingleNode("MapDataCfg");

		if(map_data_cfg != null)
		{
			MapDataFolder = map_data_cfg.SelectSingleNode("MapData").Attributes["Folder"].InnerText;

			GSIImageTileFolder = map_data_cfg.SelectSingleNode("GSIImageTiles").Attributes["Folder"].InnerText;
			GSIImageTileExt	   = map_data_cfg.SelectSingleNode("GSIImageTiles").Attributes["Ext"   ].InnerText;

			GSIPhotoTileFolder = map_data_cfg.SelectSingleNode("GSIPhotoTiles").Attributes["Folder"].InnerText;
			GSIPhotoTileExt	   = map_data_cfg.SelectSingleNode("GSIPhotoTiles").Attributes["Ext"   ].InnerText;

			GSIElevationTileFolder = map_data_cfg.SelectSingleNode("GSIElevationTiles").Attributes["Folder"].InnerText;

			GSIGeoidModelFile = map_data_cfg.SelectSingleNode("GSIGeoidModel").Attributes["File"].InnerText;
		}

		//--------------------------------------------------
		// �O���b�h�ݒ�

		var grid_cfg = geoviewer_cfg.SelectSingleNode("Grid");

		if(grid_cfg != null)
		{
			GridFontSize = ToInt32(grid_cfg.Attributes["FontSize"].InnerText);
			GridOverlayCfg = grid_cfg.SelectSingleNode("GridOverlay");
		}
	}

	void ReadCfgFromParam(in string param)
	{
		// ��params�͗\���	
		string[] prms = param.Split(" ");

		// ���Ƃ肠�����B
		Title = "DENKOKU-ROBO2"; 

		StartLgLt_0.Set(new CLg(ToDouble(prms[0])), new CLt(ToDouble(prms[1])));
		EndLgLt_0  .Set(new CLg(ToDouble(prms[2])), new CLt(ToDouble(prms[3])));

		PolygonZoomLevel = ToInt32(prms[4]);
		ImageZoomLevel	 = ToInt32(prms[5]);
	}

	CLASzip LASzipData = null;

	void ReadLASFromFile(in string las_fname)
	{
		Title = las_fname; 

		LASzipData = new CLASzip(las_fname);

		var laszip_header = LASzipData.Header;

		var vlrs_data = laszip_header.vlrs_data; //�����Ƀt�@�C����񂪓����Ă���B

		// ���Ƃ肠�����������邪�AHString�Ƃ��Ő؂蕪����B
		if(vlrs_data.StartsWith("GEOGCS"))
		{
			// �o�ܓx

			// ���Ƃ肠����
			var lg_s_value = laszip_header.min_x - LASMargin;
			var lg_e_value = laszip_header.max_x + LASMargin;
			var lt_s_value = laszip_header.min_y - LASMargin;
			var lt_e_value = laszip_header.max_y + LASMargin;

			StartLgLt_0 = new CLgLt(new CLg(lg_s_value), new CLt(lt_s_value));
			EndLgLt_0   = new CLgLt(new CLg(lg_e_value), new CLt(lt_e_value));
		}
		else if(vlrs_data.StartsWith("PROJCS"))
		{
			// ���ʒ��p���W

			var origin_s = vlrs_data.Substring(8);

		}
		else
			throw new Exception("unknown vlrs_data");
/*
BL�̏ꍇ
"GEOGCS[
	\"GCS_WGS_1984\",
	DATUM[
		\"D_WGS_1984\",
		SPHEROID[
			\"WGS_1984\",
			6378137.0,
			298.257223563
		],
		TOWGS84[0,0,0,0,0,0,0]
	],
	PRIMEM[\"Greenwich\",0.0],
	UNIT[\"Degree\",0.0174532925199433]
]"

XY�̏ꍇ
"PROJCS[
	\"JGD2011_Japan_Zone_9\",
	GEOGCS[
		\"GCS_JGD_2011\",
		DATUM[
			\"D_JGD_2011\",
			SPHEROID[
				\"GRS_1980\",
				6378137.0,
				298.257222101
			],
			TOWGS84[0,0,0,0,0,0,0]
		],
		PRIMEM[\"Greenwich\",0.0],
		UNIT[\"Degree\",0.0174532925199433]
	],
	PROJECTION[\"Transverse_Mercator\"],
	PARAMETER[\"False_Easting\",0.0],
	PARAMETER[\"False_Northing\",0.0],
	PARAMETER[\"Central_Meridian\",139.833333333333],
	PARAMETER[\"Scale_Factor\",0.9999],
	PARAMETER[\"Latitude_Of_Origin\",36],
	UNIT[\"Meter\",1.0]
]"
*/



	}
}
//---------------------------------------------------------------------------
}
