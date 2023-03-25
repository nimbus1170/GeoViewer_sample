//
// GeoViewer_WP.cs
// �n�`�r���[�A(���[���h�s�N�Z��) - �I�[�o���C�`��
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;

using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawOverlayGeoViewer_WP(in CGeoViewer_WP viewer)
	{
		//--------------------------------------------------
		// 1 �n�}�𔼓����ɂ��ďd�˂Ă݂�B
	//	viewer.AddOverlay("test_map_ol", img_map_data, 1000.0, 0.5f);

		//--------------------------------------------------
		// 2 �O���b�h���I�[�o���C�ɕ`�悷��B

		if(GridOverlayCfg != null)
		{
			// �I�[�o���C�̃T�C�Y�̊(�������ӂ����̃T�C�Y�ɂ���B)
			var ol_size = ToInt32(GridOverlayCfg.Attributes["Size"].InnerText);
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// ���e�N�X�`���T�C�Y�͔C�ӂ����A�t�H���g�T�C�Y�����e�N�X�`���T�C�Y�Ɉ���������BGraphicsUnit.Pixel�ȊO�ł��B
			var grid_map_img = new Bitmap(ol_w, ol_h);

			DrawLgLtGrid(grid_map_img, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (grid_map_img, StartLgLt, EndLgLt, GridFontSize);

			// �n�\�ʂ���̍���
		 	var ol_offset = ToDouble(GridOverlayCfg.Attributes["Offset"].InnerText);

			// �n�}�摜�f�[�^�̃Y�[�����x���ɂ���̂ŐV�K�C���X�^���X�ɂ���B
			var img_s_wp = new CWPInt(StartWP, ImageZoomLevel);
			var img_e_wp = new CWPInt(EndWP	 , ImageZoomLevel);

			viewer.AddOverlay
				("grid",
				 new CImageMapData_WP(grid_map_img, img_s_wp, img_e_wp),
				 ol_offset,
				 1.0f); // �����x
		}

		//--------------------------------------------------
		// 3 �����I�Ƀe�N�X�`���I�[�o���C���d�˂Ă݂�B
		// ���I�[�o���C���͈͊O�ɂ���Ɠ��R�G���[�ɂȂ邪�A�Ȃ�Ȃ��ꍇ������B
		if(true)
		{
			// �I�[�o���C�̃T�C�Y�̊(�������ӂ����̃T�C�Y�ɂ���B)
			int ol_size = 1000;
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// ��R
		//	var ol_s_lg = new CLg(new CDMS(130,  9, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS( 33, 34, 0.0).DecimalDeg);
		//	var ol_e_lg = new CLg(new CDMS(130, 10, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS( 33, 35, 0.0).DecimalDeg);

			// ���ŉ�
			var ol_s_lg = new CLg(new CDMS(130, 24, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS(33, 42, 0.0).DecimalDeg);
			var ol_e_lg = new CLg(new CDMS(130, 25, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS(33, 43, 0.0).DecimalDeg);

			// ���I�[�o���C�͈͓̔͂�k�t�]
			var ol = viewer.MakeOverlay
				(ToWPInt(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_e_lt)),
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_s_lt)),
				 ol_w, ol_h);

			var p_from = ol.ToPointOnOverlay(ToWP(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_s_lt)));
			var p_to   = ol.ToPointOnOverlay(ToWP(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_e_lt)));

			var g = ol.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 5.0f), 0, 0, ol_w, ol_h);

			g.DrawLine(new Pen(Color.Red, 5.0f), p_from, p_to);

			g.Dispose();
		
			// ���I�t�Z�b�g��A���t�@�l�͓��I�ɕύX���邱�Ƃ�\������COverlay�̃����o�ɂ��Ȃ��B
			viewer.AddOverlay
				("test_ol",
				 ol,
				 200.0, // �n�\�ʂ���̍���
				 0.5f); // �����x
		}

		//--------------------------------------------------
		// 4 �����I�ɎO�p�|���S���I�[�o���C���d�˂Ă݂�B
		// ��XML�ɂ��Ă��P�Ȃ�I�[�o���C�ɉ��̈Ӗ������邩������Ȃ����A��芸�����B
		if(true)
		{
			// ��R
		//	var ol_s_lg = new CLg(new CDMS(130,  9, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS( 33, 34, 0.0).DecimalDeg);
		//	var ol_e_lg = new CLg(new CDMS(130, 10, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS( 33, 35, 0.0).DecimalDeg);

			// �x�m�R
		//	var ol_s_lg = new CLg(138.70); var ol_s_lt = new CLt(35.33);
		//	var ol_e_lg = new CLg(138.75); var ol_e_lt = new CLt(35.36);

			// ��Îs
			// ���͈͂��O�����E�ɋ߂Â���ƃC���f�b�N�X���I�[�o�[����B
			var ol_s_lg = new CLg(new CDMS(135, 53, 0.0).DecimalDeg); var ol_s_lt = new CLt(new CDMS(34, 57, 30.0).DecimalDeg);
			var ol_e_lg = new CLg(new CDMS(135, 55, 0.0).DecimalDeg); var ol_e_lt = new CLt(new CDMS(34, 59, 30.0).DecimalDeg);

			viewer.AddOverlay
				("test_ol_2",
				 DOverlayBase.OnGeoid,
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_s_lg, ol_s_lt)),
				 ToWPInt(PolygonZoomLevel, new CLgLt(ol_e_lg, ol_e_lt)),
				 100.0, // Elevation���n�\�ʂ���̍����AGeoid���W��
				 new CColorF(0.1f, 0.1f, 0.8f, 0.5f)); // RGBA
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
