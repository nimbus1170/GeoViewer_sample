//
// GeoViewer_WP.cs
// �n�`�r���[�A(���[���h�s�N�Z��) - �I�[�o���C�`��
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geometry.CDMS;
using static DSF_NET_TacticalDrawing.Observer;
using static DSF_NET_Scene.Observer;

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
		//	var ol_s_lg = new CLg(ToDecimalDeg(130,  9, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 34, 0.0));
		//	var ol_e_lg = new CLg(ToDecimalDeg(130, 10, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 35, 0.0));

			// ���ŉ�
			var ol_s_lg = new CLg(ToDecimalDeg(130, 24, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 42, 0.0));
			var ol_e_lg = new CLg(ToDecimalDeg(130, 25, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 43, 0.0));

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
		// �����i�΂̐��~�߂��납�V�~�����[�V����
		// ���P�Ȃ�P�F�I�[�o���C�ɈӖ������邩������Ȃ����A��芸�����B
		if(Title == "���i�΁i��Îs�j")
		{
			// ���͈͂��O�����E�ɋ߂Â���ƃC���f�b�N�X���I�[�o�[����B
			var ol_s_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 53, 0.0)), new CLt(ToDecimalDeg(34, 57, 30.0)));
			var ol_e_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 55, 0.0)), new CLt(ToDecimalDeg(34, 59, 30.0)));

			viewer.AddOverlay
				("���i�΂̐���",
				 ToWPInt(PolygonZoomLevel, ol_s_lglt),
				 ToWPInt(PolygonZoomLevel, ol_e_lglt),
				 DOverlayAltitudeOffsetBase.AMSL, // AGL:�n�\�ʂ���̍��x�AAMSL:�C�ʂ���̍��x()
				 100.0,
				 new CColorF(0.1f, 0.1f, 0.8f, 0.5f)); // RGBA
		}

		//--------------------------------------------------
		// ���E�}
		if(true)
		{ 
			// OP�ʒu�F���������̋{�n�x����
			var op_lglt = new CLgLt(new CLg(130.18127), new CLt(33.54134), new CAltitude(10));

			Viewer.AddShape
				("",
				 new CGeoCircle(8, op_lglt, 10)
					.SetLineWidth(2.0f)
					.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 1.0f))
					.SetFill(true));

			// �Ď��͈́FOP���琼������
			var ol_s_lg = new CLg(130.15); var ol_s_lt = new CLt(33.53);
			var ol_e_lg = new CLg(130.18); var ol_e_lt = new CLt(33.55);

			// ��WP���W�͓�k�t�]
			var observe_wp_sx = ToWPIntX(PolygonZoomLevel, ol_s_lg);
			var observe_wp_sy = ToWPIntY(PolygonZoomLevel, ol_e_lt);
			var observe_wp_ex = ToWPIntX(PolygonZoomLevel, ol_e_lg);
			var observe_wp_ey = ToWPIntY(PolygonZoomLevel, ol_s_lt);

			// �I�[�o���C�̃T�C�Y
			// ����荇����WP�Ԋu��100����
			int ol_w = (observe_wp_ex.Value - observe_wp_sx.Value) * 100;
			int ol_h = (observe_wp_ey.Value - observe_wp_sy.Value) * 100;

			var ol = viewer.MakeOverlay
				(new CWPInt(observe_wp_sx, observe_wp_sy),
				 new CWPInt(observe_wp_ex, observe_wp_ey),
				 ol_w, ol_h);

			var g = ol.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 10.0f), 0, 0, ol_w, ol_h);

			var red_brush = new SolidBrush(Color.Red);

			var dot_size   = 50;
			var dot_size_2 = dot_size / 2;

			for(var obj_y = 0; obj_y <= ol_h; obj_y += dot_size)
				for(var obj_x = 0; obj_x <= ol_w; obj_x += dot_size)
				{
					var obj_wp_x_value = observe_wp_sx.Value + ((double)obj_x) / 100.0;
					var obj_wp_y_value = observe_wp_sy.Value + ((double)obj_y) / 100.0;

					var obj_lglt = ToLgLt(new CWP(new CWPX(PolygonZoomLevel, obj_wp_x_value), new CWPY(PolygonZoomLevel, obj_wp_y_value)));

					obj_lglt.SetAltitude(10, DAltitudeBase.AGL);

					StopWatch.Lap("before IsObserve");

				//	var is_observe = IsObserve(PolygonZoomLevel, op_lglt, obj_lglt);
					var is_observe = IsObserve(op_lglt, obj_lglt, PolygonZoomLevel);
				
					StopWatch.Lap("after IsObserve");

				//	if(!IsObserve(PolygonZoomLevel, op_lglt, obj_lglt))
					if(!is_observe)
						g.FillRectangle(red_brush, obj_x - dot_size_2, obj_y - dot_size_2, dot_size, dot_size);
				}

			g.Dispose();
		
			// ���I�t�Z�b�g��A���t�@�l�͓��I�ɕύX���邱�Ƃ�\������COverlay�̃����o�ɂ��Ȃ��B
			viewer.AddOverlay
				("test_ol",
				 ol,
				 10.0, // �n�\�ʂ���̍���
				 0.5f); // �����x
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
