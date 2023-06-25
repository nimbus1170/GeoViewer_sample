//
// GeoViewerMainForm_LAS.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using DSF_NET_Geometry;
using DSF_NET_Color;
using DSF_NET_Scene;

using System.Runtime.Versioning;

using static DSF_CS_Profiler.CProfilerLog;
using static DSF_NET_Geography.DAltitudeBase;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
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

			StartLgLt_0 = new CLgLt(new CLg(lg_s_value), new CLt(lt_s_value), AGL);
			EndLgLt_0   = new CLgLt(new CLg(lg_e_value), new CLt(lt_e_value), AGL);
		}
		else
		{
			// ���ʒ��p���W

			var origin_lglt = ReadLASOrigin(vlrs_data);

			Convert_LgLt_XY.Origin = origin_lglt;

			// ��LAS�f�[�^��X�������̂悤���B
			var min_xy = new CCoord(laszip_header.min_y, laszip_header.min_x);
			var max_xy = new CCoord(laszip_header.max_y, laszip_header.max_x);

			var min_lglt = Convert_LgLt_XY.ToLgLt(min_xy, AGL);
			var max_lglt = Convert_LgLt_XY.ToLgLt(max_xy, AGL);

			var lg_s_value = min_lglt.Lg.DecimalDeg - LASMargin;
			var lg_e_value = max_lglt.Lg.DecimalDeg + LASMargin;
			var lt_s_value = min_lglt.Lt.DecimalDeg - LASMargin;
			var lt_e_value = max_lglt.Lt.DecimalDeg + LASMargin;

			StartLgLt_0 = new CLgLt(new CLg(lg_s_value), new CLt(lt_s_value), AGL);
			EndLgLt_0   = new CLgLt(new CLg(lg_e_value), new CLt(lt_e_value), AGL);
		}
	}

	CXYOrigin ReadLASOrigin(in string vlrs_data)
	{
		CXYOrigin origin = null;

		if(vlrs_data.StartsWith("PROJCS"))
		{
			// ���Ƃ肠�������_���W���w�b�_����ǂށB

			var origin_cm_s = vlrs_data.IndexOf("PARAMETER[\"Central_Meridian\"") + 29;
			var origin_cm_e = vlrs_data.IndexOf(']', origin_cm_s);

			var origin_cm = Convert.ToDouble(vlrs_data.Substring(origin_cm_s, origin_cm_e - origin_cm_s));

			var origin_lt_s = vlrs_data.IndexOf("PARAMETER[\"Latitude_Of_Origin\"") + 31;
			var origin_lt_e = vlrs_data.IndexOf(']', origin_lt_s);

			var origin_lt = Convert.ToDouble(vlrs_data.Substring(origin_lt_s, origin_lt_e - origin_lt_s));

			origin = new CXYOrigin("", new CLgLt(new CLg(origin_cm), new CLt(origin_lt), AGL));
		}
		else if((1 <= DefaultOrigin) &&(DefaultOrigin <= 19))
		{ 
			origin = Convert_LgLt_XY.Origins[DefaultOrigin];
		}
		else
			throw new Exception("unknown vlrs_data and appropriate DefaultOrigin not defined");

		return origin;
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

vlrs_data���Ȃ��ꍇ������B
*/
	}

	[SupportedOSPlatform("windows")]
	void DrawLAS()
	{
		if(LASzipData == null) return;

		if(ToCheckLASDataOnly) return;

		//--------------------------------------------------

		var laszip_data = LASzipData;

		var laszip_header = laszip_data.Header;

		var x_scale_factor = laszip_header.x_scale_factor;
		var y_scale_factor = laszip_header.y_scale_factor;
		var z_scale_factor = laszip_header.z_scale_factor;

		var x_offset = laszip_header.x_offset;
		var y_offset = laszip_header.y_offset;
		var z_offset = laszip_header.z_offset;

		var laszip_points = laszip_data.Points;

		double x, y, z;

		ushort r, g, b;

		float intensity;

		//--------------------------------------------------

		var vlrs_data = laszip_header.vlrs_data; //�����Ƀt�@�C����񂪓����Ă���B

StopWatch.Lap("before SetLASPoints");

		// ���Ƃ肠�����������邪�AHString�Ƃ��Ő؂蕪����B
		if(vlrs_data.StartsWith("GEOGCS"))
		{
			// �o�ܓx

			if(false)
			{
				// C#�̕��Ŋe�_���쐬����B
				// C#�̕��ŐF������������ꍇ�͂�����
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

				var dst_pts = new CGeoPoints(4.0f);

				var pt_lglt = new CLgLt();

				var pt_color = new CColorF{ A = 1.0f }; // A:�����x(�A���t�@�l)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.X * x_scale_factor;
					y = y_offset + pt.Y * y_scale_factor;
					z = z_offset + pt.Z * z_scale_factor;

					pt_lglt.SetLg(x).SetLt(y).SetAltitude(AMSL, z);

					//--------------------------------------------------

					intensity = (has_intensity)? ((pt.intensity - min_intensity) / intensity_range): 1.0f;

					r = pt.R;
					g = pt.G;
					b = pt.B;

					if((r != 0) || (g != 0) || (b != 0))
					{
						// �F�t��
						pt_color.R = r / 255.0f * intensity;
						pt_color.G = g / 255.0f * intensity;
						pt_color.B = b / 255.0f * intensity;
					}
					else
					{
						// �F�Ȃ�
						pt_color.R =
						pt_color.G =
						pt_color.B = intensity;
					}

					//--------------------------------------------------

					dst_pts.AddPoint(pt_lglt, pt_color);
				}

				Viewer.AddShape("laspoints", dst_pts);
			}
			else
				// DLL���Ŋe�_���쐬����B
				Viewer.AddShape("laspoints", LASzipData.MakeGeoPointsBL().SetPointSize(4.0f));
		}
		else
		{
			// ���ʒ��p���W

			var origin_lglt = ReadLASOrigin(vlrs_data);
			
			Convert_LgLt_XY.Origin = origin_lglt;

			if(true)
			{
				// C#�̕��Ŋe�_���쐬����B
				// C#�̕��ŐF������������ꍇ�͂�����
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

				var dst_pts = new CGeoPoints(4.0f);

				var pt_xy = new CCoord();

			//	var pt_lglt = new CLgLt(new CLg(), new CLt(), AMSL);

				var pt_color = new CColorF{ A = 1.0f }; // A:�����x(�A���t�@�l)

				foreach(var pt in laszip_points)
				{
					//--------------------------------------------------

					x = x_offset + pt.X * x_scale_factor;
					y = y_offset + pt.Y * y_scale_factor;
					z = z_offset + pt.Z * z_scale_factor;

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

					r = pt.R;
					g = pt.G;
					b = pt.B;

					if((r != 0) || (g != 0) || (b != 0))
					{
						// �F�t��
						pt_color.R = r / 255.0f * intensity;
						pt_color.G = g / 255.0f * intensity;
						pt_color.B = b / 255.0f * intensity;
					}
					else
					{
						// �F�Ȃ�
						pt_color.R =
						pt_color.G =
						pt_color.B = intensity;
					}

					//--------------------------------------------------

					dst_pts.AddPoint(pt_lglt, pt_color);
				}

				Viewer.AddShape("laspoints", dst_pts);
			}
			else
				Viewer.AddShape("laspoints", LASzipData.MakeGeoPointsXY().SetPointSize(4.0f));
		}

StopWatch.Lap("after  SetLASPoints");

		//--------------------------------------------------

		Viewer.DrawScene();
	}

	[SupportedOSPlatform("windows")]
	private void ShowLASLog()
	{
		if(LASzipData == null) return;

		var laszip_header = LASzipData.Header;

		DialogTextBox.AppendText($"[LAS�f�[�^]\r\n");
		DialogTextBox.AppendText($"        �o�[�W���� : {laszip_header.version_major}.{laszip_header.version_minor}\r\n");
		DialogTextBox.AppendText($"      �t�H�[�}�b�g : {laszip_header.point_data_format}\r\n");
		DialogTextBox.AppendText($"        �|�C���g�� : {(int)laszip_header.number_of_point_records:#,0}\r\n");

		var laszip_points = LASzipData.Points;

		var has_intensity = false;
		var has_color = false;
		var has_class = false;
			
		foreach(var pt in laszip_points)
		{
			if(!(has_intensity) && ( pt.intensity != 0)						  ) has_intensity = true;
			if(!(has_color    ) && ((pt.R != 0) || (pt.G != 0) || (pt.B != 0))) has_color = true;
			if(!(has_class	  ) && ( pt.classification != 0)				  ) has_class = true;
		}

		DialogTextBox.AppendText($"  �P�x�t���|�C���g : {(has_intensity? "����":"�Ȃ�")}\r\n");
		DialogTextBox.AppendText($"    �F�t���|�C���g : {(has_color?	 "����":"�Ȃ�")}\r\n");
		DialogTextBox.AppendText($"�N���X�t���|�C���g : {(has_class?	 "����":"�Ȃ�")}\r\n");

		DialogTextBox.AppendText($"\r\n");
	}
}
//---------------------------------------------------------------------------
}
