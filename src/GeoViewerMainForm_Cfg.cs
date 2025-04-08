//
// GeoViewerMainForm_Cfg.cs
// �n�`�r���[�A - �ݒ�t�@�C��
//
// ���{���͕ʂɂ���K�v�͂Ȃ���������Ȃ����A�����LgLt��WP��Tile�ɕ�����Ă���A�������̂������Ă���̂œZ�߂�B
// ��XML��n�[�h�R�[�h���̃o���G�[�V����������̂ňˑ����Ȃ��悤�ɕʂɂ���B
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		//--------------------------------------------------
		// �n�}�f�[�^�t�H���_���ݒ�

		public readonly CCfg Cfg = new ();

		//--------------------------------------------------
		// �ʐݒ�
		// ��TreeCounter�̂悤��Cfg�Ƃ��Ă܂Ƃ߂邩�B

		public string Title;

		public CLgLt StartLgLt_0;
		public CLgLt EndLgLt_0;

		// ���ꉞ�����l��^���Ă������B
		public int MeshZoomLevel  = 14;
		public int ImageZoomLevel = 14;
		public int DEMZoomLevel	  = 14;

		//---------------------------------------------------------------------------

		void ReadCfgFromFile(in string cfg_fname)
		{
			Cfg.ReadFromFile(cfg_fname);

			if(Cfg.ToSelectMapCfg)
			{
				// �ʒu�ݒ�t�@�C���I�����[�h

				var of_dialog = new OpenFileDialog()
					{ Title  = "�n��ݒ�t�@�C�����J��",
					  Filter = "�n��ݒ�t�@�C��|*.cfg.xml",
					  Multiselect = false };

				// ���t�@�C����I�����Ȃ�������I������B
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ���I�����Ȃ��B
					Close();

				var fname = of_dialog.FileName;

				// ���ċN�Ăяo���������v���H
				ReadCfgFromFile(fname);

				of_dialog.Dispose();
			}
			else if(Cfg.ToSelectLAS)
			{
				// �_�Q�t�@�C���I�����[�h

				var of_dialog = new OpenFileDialog()
					{ Title  = "�_�Q�t�@�C�����J��",
					  Filter = "�_�Q�t�@�C��|*.las;*.laz;*.csv;*.txt",
					  Multiselect = true }; // �������I��������

				// ���t�@�C����I�����Ȃ�������I������B
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ���I�����Ȃ��B
					Close();

				var fnames = of_dialog.FileNames;

				Title =
				Cfg.LASFile = fnames[0]; // ��������1�����ݒ�ł��Ȃ����A�ǂ��Ŏg�p���Ă���H

				if(fnames.Length > 1) Title += " ��";

				foreach(var fname in fnames)
				{
					// ���͈͎w��̏ꍇ�͓_�Q��ǂ܂Ȃ��ƒn�}�͈͂����܂�Ȃ����A�n�}�͈͂����߂邽�߂ɂ���(Cfg)�œǂނ̂����������̂ŁA�n�}�͈͂͑S�̈�Ƃ���B
					SetLgLtAreaFromLASFile(fname);

					LASFnames.Add(fname);
				}

				of_dialog.Dispose();
			}
			else if(Cfg.ToSelectShape)
			{
				// �}�`�t�@�C���I�����[�h
				// �������ł̓t�@�C�����݂̂�ǂނ悤�ɂ��āA�ǂݍ���ŕ`�悷��͕̂ʂ̏ꏊ�Ŕ񓯊��ɂ���B
				// ���R�}���h�ł�KML�����ǂ߂�B��������B

				var of_dialog = new OpenFileDialog()
					{ Title  = "�}�`�t�@�C�����J��",
					  Filter = "�}�`�t�@�C��|*.shp",
					  Multiselect = false };

				// ���t�@�C����I�����Ȃ�������I������B
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ���I�����Ȃ��B
					Close();

				var fname = of_dialog.FileName;

				Title = fname;

StopWatch.Lap("before ReadShapefileFromFiles");
MemWatch .Lap("before ReadShapefileFromFiles");
				(Shapefile, ReadShapefileMsg) = ReadShapefileFromFile(fname);
StopWatch.Lap("after  ReadShapefileFromFile");
MemWatch .Lap("after  ReadShapefileFromFile");

				of_dialog.Dispose();
			}
			else
			{
				Title = Cfg.Title;

				StartLgLt_0 = Cfg.StartLgLt_0;
				EndLgLt_0   = Cfg.EndLgLt_0;

				// �����̐ݒ�͂����ł悢���H
				Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[Cfg.DefaultOrigin];
			}

			MeshZoomLevel  = Cfg.MeshZoomLevel;
			ImageZoomLevel = Cfg.ImageZoomLevel;
			DEMZoomLevel   = Cfg.DEMZoomLevel;
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
	}
}
