//
// GeoViewerMainForm_Cfg.cs
// �n�`�r���[�A - �ݒ�t�@�C��
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
	int	   DefaultOrigin;
	string TXTTitleLine;
	string TXTFormat;
	int	   PointSize;
	double LASMargin;
	bool   ToCheckLASDataOnly = false;

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
		// ���ʂ̐ݒ�t�@�C����ǂݍ���ŕK�v�Ȑݒ���㏑�����邽�߂ɂ��g�p���邽�߁A��`���Ȃ��Ă��G���[�ɂ͂��Ȃ��B
		// �����ݒ�R����`�F�b�N����K�v����B

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_fname);

		//--------------------------------------------------

		var geoviewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg")?? throw new Exception("tag GeoViewerCfg not found (" + cfg_fname + ")");

		//--------------------------------------------------

		ToShowDebugInfo = (geoviewer_cfg.SelectSingleNode("ToShowDebugInfo") != null)? true: false;;

		//--------------------------------------------------

		var plane_mode_cfg = geoviewer_cfg.SelectSingleNode("PlaneMode");

		if(plane_mode_cfg != null)
			PlaneMode = plane_mode_cfg.Attributes["Mode"].InnerText;

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

			var map_cfg = geoviewer_cfg.SelectSingleNode("MapCfg"); // ?? throw new Exception("tag MapCfg not found (" + cfg_fname + ")");

			if(map_cfg != null)
			{
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
		}

		//--------------------------------------------------
		// LAS�ݒ��ǂށB
		// ��LAS�t�@�C�����ォ��ǂݍ��ޏꍇ�ɂ��K�v�B

		var las_cfg = geoviewer_cfg.SelectSingleNode("LASCfg");

		if(las_cfg != null)
		{
			DefaultOrigin = ToInt32(las_cfg.Attributes["DefaultOrigin"].InnerText);
			PointSize	  = ToInt32(las_cfg.Attributes["PointSize"	  ].InnerText);

			// ���K�{�ł͂Ȃ��B
			if(las_cfg.Attributes["TXTTitleLine" ] != null) TXTTitleLine = las_cfg.Attributes["TXTTitleLine" ].InnerText;
			if(las_cfg.Attributes["TXTFormat"	 ] != null)	TXTFormat	 = las_cfg.Attributes["TXTFormat"	 ].InnerText;
		}

		//--------------------------------------------------
		// LAS�t�@�C����ǂށB
		// ���_�u���Đݒ肳�ꂽ��ǂ�����H

		var lasview_cfg = geoviewer_cfg.SelectSingleNode("ToSelectLASFile");
		
		if(lasview_cfg != null)
		{
			// �t�@�C���I���_�C�A���O��\������LAS�t�@�C����I������B

			LASMargin = ToDouble(lasview_cfg.Attributes["Margin"].InnerText);

			ToCheckLASDataOnly = (lasview_cfg.Attributes["ToCheckLASDataOnly"]?.InnerText == "true")? true: false;

			PolygonZoomLevel = ToInt32(lasview_cfg.Attributes["PolygonZoomLevel"].InnerText);
			ImageZoomLevel	 = ToInt32(lasview_cfg.Attributes["ImageZoomLevel"  ].InnerText);

			NearPlane = ToDouble(lasview_cfg.Attributes["NearPlane"].InnerText);

			OpenFileDialog of_dialog = new ()
				{ Title  = "LAS�t�@�C�����J��",
				  Filter = "LAS�t�@�C��(*.las;*.csv;*.txt)|*.las;*.csv;*.txt" };

			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ���I�����Ȃ��B
				Close();

			var las_fname = of_dialog.FileName;

			of_dialog.Dispose();

			Title = las_fname; 

StopWatch.Lap("before ReadLASFromFiles");
MemWatch .Lap("before ReadLASFromFiles");
			(LASzipData, ReadLASMsg) = ReadLASFromFile(las_fname);
StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");
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
}
//---------------------------------------------------------------------------
}
