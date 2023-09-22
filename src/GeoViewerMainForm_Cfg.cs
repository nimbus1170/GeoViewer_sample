//
// GeoViewerMainForm_Cfg.cs
// �n�`�r���[�A - �ݒ�t�@�C��
//
// ���{���͕ʂɂ���K�v�͂Ȃ���������Ȃ����A�����LgLt��WP��Tile�ɕ�����Ă���A�������̂������Ă���̂œZ�߂�B
// ��XML��n�[�h�R�[�h���̃o���G�[�V����������̂ňˑ����Ȃ��悤�ɕʂɂ���B
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using System.Xml;

using static DSF_NET_TacticalDrawing.XMLReader;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	//--------------------------------------------------
	// �n�}�f�[�^�t�H���_���ݒ�

	CMapDataCfg MapDataCfg = new ();

	//--------------------------------------------------

	CShapeCfg ShapeCfg = new ();

	//--------------------------------------------------
	// �ʐݒ�

	bool ToShowDebugInfo;

	string PlaneMode;

	string Title;

	double LgLtMargin = 0.001;

	int MeshZoomLevel;
	int ImageZoomLevel;

	int MeshSize;

	double NearPlane;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	bool ToDrawGrid = false;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	string LASFname = "";

	//---------------------------------------------------------------------------

	void ReadCfgFromFile(in string cfg_fname)
	{
		// ���ʂ̐ݒ�t�@�C����ǂݍ���ŕK�v�Ȑݒ���㏑�����邽�߂ɂ��g�p���邽�߁A��`���Ȃ��Ă��G���[�ɂ͂��Ȃ��B
		// �����ݒ�R����`�F�b�N����K�v����B

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_fname);

		//--------------------------------------------------

		var geoviewer_cfg = cfg_doc.SelectSingleNode("CfgRoot")?? throw new Exception("CfgRoot not found (" + cfg_fname + ")");

		//--------------------------------------------------
		// ���ʐݒ�

		MapDataCfg.ReadFromFile(cfg_fname);

		//--------------------------------------------------
		// �}�`�ݒ�

		ShapeCfg.ReadFromFile(cfg_fname);

		// ��MapCfg�ł��ݒ肵�Ă��邪�A������̂��g�����Ƃ�����H
		LgLtMargin	   = ShapeCfg.LgLtMargin;
		MeshZoomLevel  = ShapeCfg.MeshZoomLevel;
		ImageZoomLevel = ShapeCfg.ImageZoomLevel;
		NearPlane	   = ShapeCfg.NearPlane;
		
		//--------------------------------------------------
		// �O���b�h�ݒ�

		var grid_cfg = geoviewer_cfg.SelectSingleNode("Grid");

		if(grid_cfg != null)
		{
			ToDrawGrid = true;		
			GridFontSize = ToInt32(grid_cfg.Attributes["FontSize"].Value);
			GridOverlayCfg = grid_cfg.SelectSingleNode("GridOverlay");
		}
		
		//--------------------------------------------------

		ToShowDebugInfo = (geoviewer_cfg.SelectSingleNode("ToShowDebugInfo") != null)? true: false;;

		//--------------------------------------------------

		var plane_mode_cfg = geoviewer_cfg.SelectSingleNode("PlaneMode");

		if(plane_mode_cfg != null)
			PlaneMode = plane_mode_cfg.Attributes["Mode"].Value;

		//--------------------------------------------------
		// �t�@�C���I�����[�h
		// ���n�}�f�[�^�ݒ蓙�́A���̑O�Ɋ������Ă����K�v������B

		var to_select_map_cfg = geoviewer_cfg.SelectSingleNode("ToSelectMapCfgFile");
		var to_select_las	  = geoviewer_cfg.SelectSingleNode("ToSelectLASFile");
		var to_select_shape	  = geoviewer_cfg.SelectSingleNode("ToSelectShapeFile");

		if((to_select_map_cfg != null) ||
		   (to_select_las	  != null) ||
		   (to_select_shape	  != null))
		{
			// �t�@�C���I�����[�h

			OpenFileDialog of_dialog = new ()
				{ Title  = "�t�@�C�����J��",
				  Filter = "�n��ݒ�t�@�C��(*.mapcfg.xml)|*.mapcfg.xml|" + 
						   "�_�Q�t�@�C��(*.las;*.csv;*.txt)|*.las;*.csv;*.txt|" +
						   "�}�`�t�@�C��(*.shp)|*.shp",
				  FilterIndex =
					(to_select_map_cfg != null)? 1:
					(to_select_las	   != null)? 2:
					(to_select_shape   != null)? 3: 1 }; // ������������1���H

			// ���t�@�C���I�����[�h�Ńt�@�C����I�����Ȃ�������I������B
			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ���I�����Ȃ��B
				Close();

			var fname = of_dialog.FileName;

			switch(of_dialog.FilterIndex)
			{
				case 1: // �n��ݒ�
				{
					// ���ċN�Ăяo���������v���H
					ReadCfgFromFile(fname);
					break;
				}

				case 2: // �_�Q�t�@�C��
				{
					Title = fname; 

StopWatch.Lap("before ReadLASFromFiles");
MemWatch .Lap("before ReadLASFromFiles");
					LAS = ReadLASFromLASFile(fname);
StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");

					break;
				}

				case 3: // �}�`�t�@�C��
				{
					Title = fname;

StopWatch.Lap("before ReadShapefileFromFiles");
MemWatch .Lap("before ReadShapefileFromFiles");
					(ShapeFile, ReadShapefileMsg) = ReadShapefileFromFile(fname);
StopWatch.Lap("after  ReadShapefileFromFile");
MemWatch .Lap("after  ReadShapefileFromFile");

					break;
				}
			}

			of_dialog.Dispose();
		}
		else
		{
			// �n��ݒ�����̃t�@�C������ǂށB

			var map_cfg = geoviewer_cfg.SelectSingleNode("MapCfg"); // ?? throw new Exception("tag MapCfg not found (" + cfg_fname + ")");

			// ���t�@�C���I�����[�h������̂ŁA��`����Ă��Ȃ��Ă��ԈႢ�ł͂Ȃ��H
			if(map_cfg != null)
			{
				Title =	map_cfg.Attributes["Title"].Value;

				MeshSize = ToInt32(map_cfg.Attributes["MeshSize"].Value);

				MeshZoomLevel  = ToInt32(map_cfg.Attributes["MeshZoomLevel" ].Value);
				ImageZoomLevel = ToInt32(map_cfg.Attributes["ImageZoomLevel"].Value);

				if(PlaneMode == "Tile") ImageZoomLevel = MeshZoomLevel;

				NearPlane = ToDouble(map_cfg.Attributes["NearPlane"].Value);

				// �N�����v�O�̊J�n�E�I���o�ܓx���W
				// ���v���[����WP�P�ʂɃN�����v����̂ŁA�N�����v�O�̂��̂��g�p���Ȃ��悤�ɂ���B
				StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
				EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));

				DrawingFileName = map_cfg.SelectSingleNode("Drawings")?.Attributes["File"]?.Value;
			}
		}
	}

	//---------------------------------------------------------------------------

	void ReadCfgFromParam(in string param)
	{
		// ��params�͗\���	
		string[] prms = param.Split(" ");

		// ���Ƃ肠�����B
		Title = "DENKOKU-ROBO2"; 

		StartLgLt_0.Set(new CLg(ToDouble(prms[0])), new CLt(ToDouble(prms[1])));
		EndLgLt_0  .Set(new CLg(ToDouble(prms[2])), new CLt(ToDouble(prms[3])));

		MeshZoomLevel  = ToInt32(prms[4]);
		ImageZoomLevel = ToInt32(prms[5]);
	}

	//---------------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
