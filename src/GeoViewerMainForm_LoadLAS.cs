//
// GeoViewerMainForm_LoadLAS.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Color;
using DSF_NET_Scene;

using static DSF_NET_Geography.DAltitudeBase;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		//-----------------------------------------------------------------------
		// �_�Q

		// CLAS�̔z��
		readonly List<CLAS> LASs = []; // ���폜���������A�܂��g�p���Ă���Ƃ��낪����B
		readonly List<string> LASFnames = [];

		//-----------------------------------------------------------------------

		// �R�}���h����Ăяo�����B
		void LoadLAS()
		{
			var of_dialog = new OpenFileDialog()
				{ Title  = "�_�Q�t�@�C�����J��",
				  Filter = "�_�Q�t�@�C��(*.las;*.csv;*.txt)|*.las;*.csv;*.txt",
				  Multiselect = true };

			if(of_dialog.ShowDialog() == DialogResult.Cancel) return;

			var las_fnames = of_dialog.FileNames;

			of_dialog.Dispose();

			// ���񓯊������ɂ���B

StopWatch.Start();
MemWatch .Lap("before ReadLASFromFiles");
			foreach(var las_fname in las_fnames)
			{
				ShowLog($"loading {las_fname}\r\n");

				var las = new CLAS(Cfg).ReadLASFile(las_fname, (float)Cfg.PointSize, IsLASDataRaw, (Cfg.AltitudeBase == "AGL")? AGL: AMSL);

StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");

				if(las.LASzip is not null)
				{
					DrawLAS("las" + (++ShapesN), las);

					ShowLog(las.Log + "\r\n");
				}
				else
					ShowLog($"reading LAS error : {""}\r\n"); // ���G���[�ł͂Ȃ���O���o���Ă���B
			}
StopWatch.Stop();
MemWatch .Stop();

			if(Cfg.ToShowDebugInfo)
			{
				ShowLog(StopWatch.ResultLog);
				ShowLog(MemWatch .ResultLog);
				ShowLog("\r\n");
			}
		}

		//-----------------------------------------------------------------------

		void SetLgLtAreaFromLASFile(in string las_fname)
		{
			var las = new CLAS(Cfg).ReadLASHeader(las_fname);

			var laszip_header = las.LASzip.Header;

			if(las.LASType == DLASType.LAS_LgLt)
			{
				// �o�ܓx

				var s_lg = new CLg(laszip_header.min_x - Cfg.LgLtMargin);
				var s_lt = new CLt(laszip_header.min_y - Cfg.LgLtMargin);

				var e_lg = new CLg(laszip_header.max_x + Cfg.LgLtMargin);
				var e_lt = new CLt(laszip_header.max_y + Cfg.LgLtMargin);

				StartLgLt_0	= new CLgLt(s_lg, s_lt);
				EndLgLt_0	= new CLgLt(e_lg, e_lt);
			}
			else
			{
				// ���ʒ��p���W

				Convert_LgLt_XY.Origin =  las.XYOrigin;

				// ��LAS�f�[�^��X�������̂悤���B
				var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.min_y, laszip_header.min_x), AGL);
				var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.max_y, laszip_header.max_x), AGL);

				var s_lg = new CLg(min_lglt.Lg.DecimalDeg - Cfg.LgLtMargin);
				var s_lt = new CLt(min_lglt.Lt.DecimalDeg - Cfg.LgLtMargin);

				var e_lg = new CLg(max_lglt.Lg.DecimalDeg + Cfg.LgLtMargin);
				var e_lt = new CLt(max_lglt.Lt.DecimalDeg + Cfg.LgLtMargin);

				if(StartLgLt_0 is null)
				{
					StartLgLt_0	= new CLgLt(s_lg, s_lt);
				}
				else
				{
					var s_lg_0 = StartLgLt_0.Lg;
					if(s_lg.DecimalDeg < s_lg_0.DecimalDeg) s_lg_0 = s_lg;

					var s_lt_0 = StartLgLt_0.Lt;
					if(s_lt.DecimalDeg < s_lt_0.DecimalDeg) s_lt_0 = s_lt;

					StartLgLt_0 = new CLgLt(s_lg_0, s_lt_0);
				}

				if(EndLgLt_0 is null)
				{
					EndLgLt_0	= new CLgLt(e_lg, e_lt);
				}
				else
				{
					var e_lg_0 = EndLgLt_0.Lg;
					if(e_lg_0.DecimalDeg < e_lg.DecimalDeg) e_lg_0 = e_lg;

					var e_lt_0 = EndLgLt_0.Lt;
					if(e_lt_0.DecimalDeg < e_lt.DecimalDeg) e_lt_0 = e_lt;

					EndLgLt_0 = new CLgLt(e_lg_0, e_lt_0);
				}
			}
		}

		//-----------------------------------------------------------------------

		CLAS ReadLASFromCfgFile(in string cfg_fname)
		{
			var las = new CLAS(Cfg).ReadFromCfgFile(cfg_fname, (float)Cfg.PointSize, IsLASDataRaw, (Cfg.AltitudeBase == "AGL")? AGL: AMSL);

			var laszip_header = las.LASzip.Header;

			if(las.LASType == DLASType.LAS_LgLt)
			{
				// �o�ܓx

				StartLgLt_0 = new CLgLt(new CLg(laszip_header.min_x - Cfg.LgLtMargin), new CLt(laszip_header.min_y - Cfg.LgLtMargin));
				EndLgLt_0   = new CLgLt(new CLg(laszip_header.max_x + Cfg.LgLtMargin), new CLt(laszip_header.max_y + Cfg.LgLtMargin));
			}
			else
			{
				// ���ʒ��p���W

				Convert_LgLt_XY.Origin =  las.XYOrigin;

				// ��LAS�f�[�^��X�������̂悤���B
				var min_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.min_y, laszip_header.min_x), AGL);
				var max_lglt = Convert_LgLt_XY.ToLgLt(new CCoord(laszip_header.max_y, laszip_header.max_x), AGL);

				StartLgLt_0 = new CLgLt(new CLg(min_lglt.Lg.DecimalDeg - Cfg.LgLtMargin), new CLt(min_lglt.Lt.DecimalDeg - Cfg.LgLtMargin));
				EndLgLt_0   = new CLgLt(new CLg(max_lglt.Lg.DecimalDeg + Cfg.LgLtMargin), new CLt(max_lglt.Lt.DecimalDeg + Cfg.LgLtMargin));
			}

			return las;
		}

		//-----------------------------------------------------------------------

		// �񓯊������I�����̏��������C���X���b�h�Ŏ��s���邽�߂̃R���e�L�X�g
		static SynchronizationContext mainthread_context;

		void DrawLAS(string name, CLAS las)
		{
			// ��ShapeCfg�H
			if(Cfg.ToCheckDataOnly) return;

			var laszip_data = las.LASzip;

			var laszip_header = laszip_data.Header;

			var x_scale_factor = laszip_header.x_scale_factor;
			var y_scale_factor = laszip_header.y_scale_factor;
			var z_scale_factor = laszip_header.z_scale_factor;

			var x_offset = laszip_header.x_offset;
			var y_offset = laszip_header.y_offset;
			var z_offset = laszip_header.z_offset;

			var laszip_points = laszip_data.Pts;

			double x, y, z;

			ushort r, g, b;

			float intensity;

			var vlrs_data = laszip_header.vlrs_data; //�����Ƀt�@�C����񂪓����Ă���B

			// ���Ƃ肠�����������邪�AWKT�ł́H
			if(vlrs_data.StartsWith("GEOGCS"))
			{
				// �o�ܓx

				if(false)
				{
					// C#�Ŋe�_���쐬����B
					// C#�ŐF������������ꍇ�͂�����
					// ���K�v���͂���̂ō������͕K�v

					//--------------------------------------------------
					// ���x�ŐF�������邽�߁A���x�͈̔͂����߂�B
					// ��LAS�f�[�^�ł͋��x�̊�͌��܂��Ă��Ȃ��̂ŁA�ő�l�E�ŏ��l���炻�͈̔͂����߂�B

					ushort min_intensity = ushort.MaxValue;
					ushort max_intensity = ushort.MinValue;

					foreach(var pt in laszip_points)
					{
						if(pt.intensity < min_intensity) min_intensity = pt.intensity;
						if(pt.intensity > max_intensity) max_intensity = pt.intensity;
					}

					float intensity_range = max_intensity - min_intensity;  

					bool has_intensity = (intensity_range > 0.0f)? true: false;

					//--------------------------------------------------

					var dst_pts = new CGeoPoints((float)Cfg.PointSize);

					var pt_lglt = new CLgLt();

				//	var pt_color = new CColorF{ A = 1.0f }; // A:�����x(�A���t�@�l)
					var pt_color = new CColorB{ A = 255 }; // A:�����x(�A���t�@�l)

					foreach(var pt in laszip_points)
					{
						//--------------------------------------------------

						x = x_offset + pt.x * x_scale_factor;
						y = y_offset + pt.y * y_scale_factor;
						z = z_offset + pt.z * z_scale_factor;

						pt_lglt.SetLg(x).SetLt(y).SetAltitude(AMSL, z);

						//--------------------------------------------------

						intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

						r = pt.r;
						g = pt.g;
						b = pt.b;

						if((r != 0) || (g != 0) || (b != 0))
						{
							// �F�t��
/*							pt_color.R = r / 65535.0f * intensity;
							pt_color.G = g / 65535.0f * intensity;
							pt_color.B = b / 65535.0f * intensity;
*/							pt_color.R = (byte)(r / 255 * intensity);
							pt_color.G = (byte)(g / 255 * intensity);
							pt_color.B = (byte)(b / 255 * intensity);
						}
						else
						{
							// �F�Ȃ�
							pt_color.R =
							pt_color.G =
							pt_color.B = (byte)intensity;
						}

						//--------------------------------------------------

						dst_pts.AddPoint(pt_lglt, pt_color);
					}

					Viewer.AddShape(name, dst_pts);
				}
				else
					// DLL���Ŋe�_���쐬����B
					// C#�ŏ����͂ł��Ȃ�������
					Viewer.AddShape(name, laszip_data.MakeGeoPtsBL((float)Cfg.PointSize)).Draw();
			}
			else
			{
				// ���ʒ��p���W

	//			Convert_LgLt_XY.Origin = ToXYOrigin(vlrs_data, DefaultOrigin);
	//			Convert_LgLt_XY.Origin = LASs[0].XYOrigin;
				Convert_LgLt_XY.Origin = las.XYOrigin;

				if(false)
				{
					// C#�Ŋe�_���쐬����B
					// C#�ŐF������������ꍇ�͂�����

					//--------------------------------------------------
					// ���x�ŐF�������邽�߁A���x�͈̔͂����߂�B
					// ��LAS�f�[�^�ł͋��x�̊�͌��܂��Ă��Ȃ��̂ŁA�ő�l�E�ŏ��l���炻�͈̔͂����߂�B

					ushort min_intensity = ushort.MaxValue;
					ushort max_intensity = ushort.MinValue;

					foreach(var pt in laszip_points)
					{
						if(pt.intensity < min_intensity) min_intensity = pt.intensity;
						if(pt.intensity > max_intensity) max_intensity = pt.intensity;
					}

					float intensity_range = max_intensity - min_intensity;  

					bool has_intensity = (intensity_range > 0.0f)? true: false;

					//--------------------------------------------------

					var dst_pts = new CGeoPoints((float)Cfg.PointSize);

					var pt_xy = new CCoord();

				//	var pt_lglt = new CLgLt(new CLg(), new CLt(), AMSL);

				//	var pt_color = new CColorF{ A = 1.0f }; // A:�����x(�A���t�@�l)
					var pt_color = new CColorB{ A = 255 }; // A:�����x(�A���t�@�l)

					foreach(var pt in laszip_points)
					{
						//--------------------------------------------------

						x = x_offset + pt.x * x_scale_factor;
						y = y_offset + pt.y * y_scale_factor;
						z = z_offset + pt.z * z_scale_factor;

	//StopWatch.Lap("(C#)before XYToLgLt");
						// �������Ɏ��Ԃ��������Ă���B
						// ��LAS�f�[�^��X�������̂悤���B
						var pt_lglt = Convert_LgLt_XY.ToLgLt(pt_xy.Set(y, x), AMSL).SetAltitude(AMSL, z);
						// ���n���悤�ɂ���Ƒ����H�x���H
						// ���X�g�b�v�E�H�b�`�Ŏ��ԋ���Ă�B
					//	Convert_LgLt_XY.ToLgLt(pt_xy.Set(y, x), ref pt_lglt).SetAltitude(AMSL, z);
	//StopWatch.Lap("(C#)after  XYToLgLt");

						//--------------------------------------------------

						intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

						r = pt.r;
						g = pt.g;
						b = pt.b;

						if((r != 0) || (g != 0) || (b != 0))
						{
							// �F�t��
						/*	pt_color.R = r / 65535.0f * intensity;
							pt_color.G = g / 65535.0f * intensity;
							pt_color.B = b / 65535.0f * intensity;
						*/	pt_color.R = (byte)(r / 255.0f * intensity);
							pt_color.G = (byte)(g / 255.0f * intensity);
							pt_color.B = (byte)(b / 255.0f * intensity);
						}
						else
						{
							// �F�Ȃ�
							pt_color.R =
							pt_color.G =
							pt_color.B = (byte)intensity;
						}

						//--------------------------------------------------

						dst_pts.AddPoint(pt_lglt, pt_color);
					}

					Viewer.AddShape(name, dst_pts);
				}
				else
				{
					// DLL���Ŋe�_���쐬����B
					// C#�ŏ����͂ł��Ȃ�������

					// �񓯊��Ŏ��s����B

				//	Viewer.AddShape(name, laszip_data.MakeGeoPointsXY((float)Cfg.PointSize));

					// ��OpenGL�̓��C���X���b�h�Ŏ��s���Ȃ��ƃ������A�N�Z�X�ᔽ����������B
					// �@MakeGeoPoints��CGeoPoints���쐬���Ă���AOpenGL����������̂Ŕ񓯊������ł��Ȃ��B
				//	laszip_data.MakeGeoPoints((float)Cfg.PointSize);

					// �񓯊��Ŏ��s����LAS�ǂݍ��ݏI�����Draw���邽�߂̃��C���X���b�h�̃R���e�L�X�g���擾����B
					mainthread_context = SynchronizationContext.Current;

					Task.Run(() => 
					{
					//	laszip_data.LoadGeoPointsXY((las.Cfg.AltitudeBase == "AGL")? AGL: AMSL);
						Viewer.AddShape(name, laszip_data.GeoPts);
					})
					.ContinueWith(t => mainthread_context.Post(_ => 
						{ Viewer.Draw();
						  ViewerForm.AdditionalInfo.Remove("LAS");
						  ViewerForm.ShowInfo();

						  laszip_data.Dispose();

					/*	  var weak_ref = new WeakReference(laszip_data);

						  GC.Collect();
						  GC.WaitForPendingFinalizers();

						  if(weak_ref.IsAlive)
						  {
							ShowLog("LAS�ǂݍ��݃G���[\r\n");
						  }
						  */
						}, null));
				}
			}

			Viewer.UpdatePointSize_px();

StopWatch.Lap("after  MakeLASPoints");
MemWatch .Lap("after  MakeLASPoints");
		}
	}
}
