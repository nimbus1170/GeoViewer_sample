//
// GeoViewerMainForm_DrawOverlayOnGeoViewer_WP.cs
// �n�`�r���[�A(���[���h�s�N�Z��) - �I�[�o���C�`��
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.GeoObserver;
using static DSF_NET_Geometry.CCoord;
using static DSF_NET_Geometry.CDMS;
using static DSF_NET_TacticalDrawing.GeoObserver;
using static DSF_NET_Scene.DOverlayAltitudeOffsetBase;

using System;
using System.Runtime.Versioning;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawOverlayOnGeoViewer_WP(in CGeoViewer_WP viewer)
	{
		//--------------------------------------------------
		// 1 �n�}�𔼓����ɂ��ďd�˂Ă݂�B
	//	viewer.AddOverlay("test_map_ol", img_map_data, 1000.0, 0.5f);

		//--------------------------------------------------
		// 2 �O���b�h���I�[�o���C�ɕ`�悷��B

		if(ToDrawGrid && (GridOverlayCfg != null))
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
		if(Title == "��������")
		{
			// �I�[�o���C�̃T�C�Y�̊(�������ӂ����̃T�C�Y�ɂ���B)
			int ol_size = 1000;
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			// ��R
			var ol_s_lg = new CLg(ToDecimalDeg(130,  9, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 34, 0.0));
			var ol_e_lg = new CLg(ToDecimalDeg(130, 10, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 35, 0.0));

			// ���ŉ�
		//	var ol_s_lg = new CLg(ToDecimalDeg(130, 24, 0.0)); var ol_s_lt = new CLt(ToDecimalDeg(33, 42, 0.0));
		//	var ol_e_lg = new CLg(ToDecimalDeg(130, 25, 0.0)); var ol_e_lt = new CLt(ToDecimalDeg(33, 43, 0.0));

			// ���I�[�o���C�͈͓̔͂�k�t�]
			var ol = viewer.MakeOverlay
				(ToWPInt(MeshZoomLevel, new CLgLt(ol_s_lg, ol_e_lt, DAltitudeBase.AGL)),
				 ToWPInt(MeshZoomLevel, new CLgLt(ol_e_lg, ol_s_lt, DAltitudeBase.AGL)),
				 ol_w, ol_h);

			var p_from = ol.ToPointOnOverlay(ToWP(MeshZoomLevel, new CLgLt(ol_s_lg, ol_s_lt, DAltitudeBase.AGL)));
			var p_to   = ol.ToPointOnOverlay(ToWP(MeshZoomLevel, new CLgLt(ol_e_lg, ol_e_lt, DAltitudeBase.AGL)));

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
		// 3.1 �����I�Ƀe�N�X�`���I�[�o���C���d�˂Ă݂�B
		// ���I�[�o���C���͈͊O�ɂ���Ɠ��R�G���[�ɂȂ邪�A�Ȃ�Ȃ��ꍇ������B
		if(Title == "�ɓ��R�y�Η�")
		{
			// �I�[�o���C�̃T�C�Y�̊(�Z�ӂ̃T�C�Y)
			int ol_size = 1000;
			int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
			int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

			var ol_s_lg = new CLg(139.0682); var ol_s_lt = new CLt(35.1093);
			var ol_e_lg = new CLg(139.0917); var ol_e_lt = new CLt(35.1226);

			// ���I�[�o���C�͈͓̔͂�k�t�]
			var ol = viewer.MakeOverlay
				(ToWPInt(MeshZoomLevel, new CLgLt(ol_s_lg, ol_e_lt, DAltitudeBase.AGL)),
				 ToWPInt(MeshZoomLevel, new CLgLt(ol_e_lg, ol_s_lt, DAltitudeBase.AGL)),
				 ol_w, ol_h);

			var g = ol.GetGraphics();

			// �ɓ��R�y�Η��ЊQ
			// �����̉摜�͌o�ܓx�ɑ΂��Ď΂߂ɂȂ��Ă���B�A�t�B���ϊ��ŕ␳�ł��Ȃ����H
			// ���������A�o�ܓx�Œ���(�H)���Ă��Ȃ��v���[��(���C���[)����邩�B
			g.DrawImage(Image.FromFile("2021_July_6,_Japan_Standard_Time_morning_09_o'clock_48_minutes_shooting._Aerial_photograph_of_Atami_debris_flow_disaster.jpg"), 0, 0, ol_w, ol_h);

			g.Dispose();
		
			// ���I�t�Z�b�g��A���t�@�l�͓��I�ɕύX���邱�Ƃ�\������COverlay�̃����o�ɂ��Ȃ��B
			viewer.AddOverlay
				("test_ol",
				 ol,
				 10.0, // �n�\�ʂ���̍���
				 0.8f); // �����x
		}

		//--------------------------------------------------
		// 4 �����I�ɎO�p���b�V���I�[�o���C���d�˂Ă݂�B
		// ���u���i�΂̐��~�߂��납�v�V�~�����[�V����(�Z���V�~�����[�V����)
		if(Title == "���i�΂̐��~�߂��납")
		{
			// ���͈͂��O�����E�ɋ߂Â���ƃC���f�b�N�X���I�[�o�[����B
			var ol_s_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 53, 0.0)), new CLt(ToDecimalDeg(34, 57, 30.0)), DAltitudeBase.AGL);
			var ol_e_lglt = new CLgLt(new CLg(ToDecimalDeg(135, 55, 0.0)), new CLt(ToDecimalDeg(34, 59, 30.0)), DAltitudeBase.AGL);

			viewer.AddOverlay
				("���i�΂̐���",
				 ToWPInt(MeshZoomLevel, ol_s_lglt),
				 ToWPInt(MeshZoomLevel, ol_e_lglt),
				 AMSL,  // AMSL:�C��(�W�I�C�h��)�ɑ΂��郌�C���[(�e���_�͊C�����x�����)�AAGL:�n�\�ʂɑ΂��郌�C���[(�e���_�͑Βn���x�����)
				 100.0, // ���������̃I�t�Z�b�g(m)�c���i�΂̐��ʂ����̊C�����x�ɂȂ����B
				 new CColorF(0.1f, 0.1f, 0.8f, 0.5f)); // RGBA

			// �������ƒn�\�ʂɋ߂��I�[�o���C�\�ʂ��܂���ɂȂ�B�f�v�X�o�b�t�@�̃r�b�g��(�[�x)������ĂȂ����Ƃ��l�����邪�AChoosePixelFormat�ł�24bit�ȊO�ɐݒ�ł��Ȃ��B
			// �@�}�V���Ŏg�p�\�ȃf�v�X�o�b�t�@�[�x��24bit�ƍl�����邪�A�P����24bit(2^24=16,777,216)����16km������1m�̌덷���o�Ă��܂��B(���ۂ͂��߂��Ă��܂���ɂȂ��Ă���B)
			// �����ߕ��ʂ�傫��(���_���牓��)����Ɗɘa���ꂽ�B�v�׋�
		}

		//--------------------------------------------------
		// 5 ���E�}��\������B
	//	if(Title == "��������")
		if(false)
		{ 
			//--------------------------------------------------
			// �ȉ��A�e�X�g

			var lglt0 = new CLgLt(new CLg(139.0), new CLt(36), DAltitudeBase.AGL);
			var lglt1 = new CLgLt(new CLg(140.0), new CLt(37), DAltitudeBase.AGL);
			var lglt2 = new CLgLt(new CLg(140.0), new CLt(35), DAltitudeBase.AGL);
			var lglt3 = new CLgLt(new CLg(138.0), new CLt(35), DAltitudeBase.AGL);
			var lglt4 = new CLgLt(new CLg(138.0), new CLt(37), DAltitudeBase.AGL);

			// ���ʊp
			var az1 = Azimuth(lglt0, lglt1).DecimalDeg;
			var az2 = Azimuth(lglt0, lglt2).DecimalDeg;
			var az3 = Azimuth(lglt0, lglt3).DecimalDeg;
			var az4 = Azimuth(lglt0, lglt4).DecimalDeg;

			// ���n����(�北����)
			var a1 = GeodesicLength(lglt1, lglt2);

			// ��������
			var a2 = Distance3D(ToGeoCentricCoord(lglt1), ToGeoCentricCoord(lglt2));

			// �e�X�g�����܂ŁB
			//--------------------------------------------------

			// OP�ʒu�F���������̋{�n�x����
			var op_lglt = new CLgLt(new CLg(130.18127), new CLt(33.54134), new CAltitude(DAltitudeBase.AGL, 10));

			Viewer.AddShape
				("",
				 new CGeoCircle(8, op_lglt, 10)
					.SetLineWidth(2.0f)
					.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 1.0f))
					.SetFill(true));

			// �Ď��͈́FOP���琼������
			var obj_s_lg = new CLg(130.15); var obj_s_lt = new CLt(33.53);
			var obj_e_lg = new CLg(130.18); var obj_e_lt = new CLt(33.55);

StopWatch.Lap("before IsObserve in C++");
			var ol_1 = COverlay_WP.MakeVisibilityOverlay(op_lglt, new CLgLt(obj_s_lg, obj_s_lt, DAltitudeBase.AGL), new CLgLt(obj_e_lg, obj_e_lt, DAltitudeBase.AGL), MeshZoomLevel, 10.0);
StopWatch.Lap("after  IsObserve in C++");

StopWatch.Lap("before IsObserveMP in C++");
			var ol_2 = COverlay_WP.MakeVisibilityOverlayMP(op_lglt, new CLgLt(obj_s_lg, obj_s_lt, DAltitudeBase.AGL), new CLgLt(obj_e_lg, obj_e_lt, DAltitudeBase.AGL), MeshZoomLevel, 10.0);
StopWatch.Lap("after  IsObserveMP in C++");
/*
			// ��WP���W�͓�k�t�]
			var obj_sx_wpi = ToWPIntX(MeshZoomLevel, obj_s_lg);
			var obj_sy_wpi = ToWPIntY(MeshZoomLevel, obj_e_lt);
			var obj_ex_wpi = ToWPIntX(MeshZoomLevel, obj_e_lg);
			var obj_ey_wpi = ToWPIntY(MeshZoomLevel, obj_s_lt);

			// �I�[�o���C�̃T�C�Y(���x)
			// ����芸����WP�Ԋu��100��������wpr�Ə̂���B
			int ol_w_wpr = (obj_ex_wpi.Value - obj_sx_wpi.Value) * 100;
			int ol_h_wpr = (obj_ey_wpi.Value - obj_sy_wpi.Value) * 100;

			var ol_3 = viewer.MakeOverlay
				(new CWPInt(obj_sx_wpi, obj_sy_wpi),
					new CWPInt(obj_ex_wpi, obj_ey_wpi),
					ol_w_wpr, ol_h_wpr);

			var g = ol_3.GetGraphics();

			g.DrawRectangle(new Pen(Color.Red, 10.0f), 0, 0, ol_w_wpr, ol_h_wpr);

			var red_brush = new SolidBrush(Color.Red);

			// �����̃h�b�g�T�C�Y�͂قƂ�ǈӖ����Ȃ����A�h�b�g�T�C�Y�Ɍ����ɂ͈Ӗ��͂Ȃ����̂Ƃ��āA��芸��������ł��B
			var dot_size_wpr   = 50;
			var dot_size_2_wpr = dot_size_wpr / 2;

			var obj_wp = new CWP
				(new CWPX(MeshZoomLevel, 0.0				),
				 new CWPY(MeshZoomLevel, obj_sy_wpi.Value));
	
			var dd = (double)(dot_size_wpr) / 100.0;

			for(var obj_y_wp = 0; obj_y_wp <= ol_h_wpr; obj_y_wp += dot_size_wpr)
			{
				obj_wp.X.Value = obj_sx_wpi.Value;
		
				for(var obj_x_wp = 0; obj_x_wp <= ol_w_wpr; obj_x_wp += dot_size_wpr)
				{
					var obj_lglt = ToLgLt(obj_wp).SetAltitude(10.0, DAltitudeBase.AGL);

				//	if(!IsObserve(MeshZoomLevel, op_lglt, obj_lglt)) // C# ��
					if(!IsObserve(op_lglt, obj_lglt, MeshZoomLevel)) // C++��
						g.FillRectangle(red_brush, obj_x_wp - dot_size_2_wpr, obj_y_wp - dot_size_2_wpr, dot_size_wpr, dot_size_wpr);
					
					obj_wp.X += dd;
				}

				obj_wp.Y += dd;
			}

StopWatch.Lap("after  IsObserve in C#");
*/
			// ���I�t�Z�b�g��A���t�@�l�͓��I�ɕύX���邱�Ƃ�\������COverlay�̃����o�ɂ��Ȃ��B
			// ��                 �n�\�ʂ���̍�����   �������x
			viewer.AddOverlay("test_ol_1", ol_1, 10.0, 0.3f);
			viewer.AddOverlay("test_ol_2", ol_2, 20.0, 0.3f);
		//	viewer.AddOverlay("test_ol_3", ol_3, 30.0, 0.3f);

			// ����
		//	var ol_w_m = (int)GeodesicLength(new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_e_lg, obj_s_lt));
		//	var ol_h_m = (int)GeodesicLength(new CLgLt(obj_s_lg, obj_s_lt), new CLgLt(obj_s_lg, obj_e_lt));

		//	var dot_size_w_m = dot_size_wpr * ol_w_m / ol_w_wpr;
		//	var dot_size_h_m = dot_size_wpr * ol_h_m / ol_h_wpr;
		}

		//--------------------------------------------------

		viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}