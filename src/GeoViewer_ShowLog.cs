//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;

using static DSF_CS_Profiler.CProfilerLog;

using System.Windows.Forms;

using static System.Math;
using DSF_NET_Scene;
using System.Drawing;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	private void ShowLog()
	{
		//--------------------------------------------------
		// �v���[���T�C�Y���v�Z����B

		var lglt_00 = StartLgLt;
		var lglt_10 = new CLgLt(EndLgLt  .Lg, StartLgLt.Lt);
		var lglt_01 = new CLgLt(StartLgLt.Lg, EndLgLt  .Lt);

		var coord_00 = ToGeoCentricCoord(lglt_00);
		var coord_10 = ToGeoCentricCoord(lglt_10);
		var coord_01 = ToGeoCentricCoord(lglt_01);

		var dx_00_10 = coord_10.X - coord_00.X;
		var dy_00_10 = coord_10.Y - coord_00.Y;
		var dz_00_10 = coord_10.Z - coord_00.Z;

		// ��C#�ɂ�Hypot���Ȃ��B
		var plane_size_EW = (int)(Sqrt(dx_00_10 * dx_00_10 + dy_00_10 * dy_00_10 + dz_00_10 * dz_00_10));

		var dx_00_01 = coord_01.X - coord_00.X;
		var dy_00_01 = coord_01.Y - coord_00.Y;
		var dz_00_01 = coord_01.Z - coord_00.Z;

		var plane_size_NS = (int)(Sqrt(dx_00_01 * dx_00_01 + dy_00_01 * dy_00_01 + dz_00_01 * dz_00_01));

		//--------------------------------------------------

		var log = Log.ToDictionary();

		var vert_nx = log["VertexNX"];
		var vert_ny = log["VertexNY"];

		DialogTextBox.AppendText($"              ���_�� : {vert_nx:#,0} x {vert_ny:#,0}\r\n");
		DialogTextBox.AppendText($"          �|���S���� : {(vert_nx - 1) * (vert_ny - 1) * 2:#,0}\r\n");
		DialogTextBox.AppendText($"    �e�N�X�`���T�C�Y : {log["TexW"]}px x {log["TexH"]}px\r\n");
		DialogTextBox.AppendText($"      �e�N�X�`������ : {log["TexN"]}\r\n");
		DialogTextBox.AppendText($"      �\���n��T�C�Y : {plane_size_EW:#,0}m x {plane_size_NS:#,0}m\r\n");
		DialogTextBox.AppendText($"      �|���S���T�C�Y : {plane_size_EW / (vert_nx - 1)}m x {plane_size_NS / (vert_ny - 1)}m\r\n");
		DialogTextBox.AppendText($"    �摜�Y�[�����x�� : {ImageZoomLevel}\r\n");
		DialogTextBox.AppendText($"�|���S���Y�[�����x�� : {PolygonZoomLevel}\r\n");
		DialogTextBox.AppendText($"\r\n");

		// �\���̗L���ɂ�����炸�X�g�b�v����B
		// �������ɗ���O��Stop����ׂ��ł́H
		StopWatch.Stop();
		MemWatch .Stop();

		if(ToShowDebugInfo)
		{
			DialogTextBox.AppendText($"polygons count from planes\r\n");
			DialogTextBox.AppendText($"gnd-sea polygons : {log["gnd_sea_polygons_count"]:#,0}\r\n");
			DialogTextBox.AppendText($"texture polygons : {log["texture_polygons_count"]:#,0}\r\n");
			DialogTextBox.AppendText($"\r\n");
			DialogTextBox.AppendText($"polygons count from GLObjects\r\n");

			var gl_objs_count = Viewer.GLObjectCount();	

			foreach(var gl_objs_count_i in gl_objs_count)
				DialogTextBox.AppendText($"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value:#,0}\r\n");

			DialogTextBox.AppendText($"\r\n");

			DialogTextBox.AppendText(MakeStopWatchLog(StopWatch) + $"\r\n");
			DialogTextBox.AppendText(MakeMemWatchLog (MemWatch ) + $"\r\n");
		}

		//--------------------------------------------------

		if(LASzipData != null)
		{ 
			var laszip_header = LASzipData.Header;

			DialogTextBox.AppendText($"[LAS�f�[�^]\r\n");
			DialogTextBox.AppendText($"        �o�[�W���� : {laszip_header.version_major}.{laszip_header.version_minor}\r\n");
			DialogTextBox.AppendText($"      �t�H�[�}�b�g : {laszip_header.point_data_format}\r\n");
			DialogTextBox.AppendText($"        �|�C���g�� : {(int)laszip_header.number_of_point_records:#,0}\r\n");

			var laszip_points = LASzipData.Points;

			var is_color_exists = false;
			var is_class_exists = false;
			
			foreach(var pt in laszip_points)
			{
				if((pt.R != 0) || (pt.G != 0) || (pt.B != 0)) is_color_exists = true;

				if(pt.classification != 0) is_class_exists = true;
			}

			DialogTextBox.AppendText($"    �F�t���|�C���g : {(is_color_exists? "����":"�Ȃ�")}\r\n");
			DialogTextBox.AppendText($"�N���X�t���|�C���g : {(is_class_exists? "����":"�Ȃ�")}\r\n");

			DialogTextBox.AppendText($"\r\n");

		}

		//--------------------------------------------------
		// ���v�����v�g�����̂悤�ɖY�ꂸ�\�����Ȃ��Ă͂Ȃ�Ȃ��̂��H
		DialogTextBox.AppendText(">");
	}
}
//---------------------------------------------------------------------------
}
