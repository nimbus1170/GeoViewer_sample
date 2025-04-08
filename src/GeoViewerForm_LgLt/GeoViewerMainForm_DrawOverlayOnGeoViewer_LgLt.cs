//
// GeoViewer_LgLt.cs
// �n�`�r���[�A(�o�ܓx) - �I�[�o���C�`��
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		void DrawOverlayOnGeoViewer_LgLt(in CGeoViewer_LgLt viewer)
		{
			//--------------------------------------------------
			// 1 �n�}�𔼓����ɂ��ďd�˂Ă݂�B

			// �����̃e�N�X�`�����B���Ă��܂��B�����������͂��B
		//	viewer.AddOverlay("test_map_ol", img_map_data, 1000.0, 0.5f);

			//--------------------------------------------------
			// 2 �O���b�h���I�[�o���C�ɕ`�悷��B

			if(Cfg.ToDrawGrid && (Cfg.GridOverlayCfg is not null))
			{ 
				// �I�[�o���C�̃T�C�Y�̊(�������ӂ����̃T�C�Y�ɂ���B)
				var ol_size = ToInt32(Cfg.GridOverlayCfg.Attributes["Size"].Value);
				int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
				int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

				// ���e�N�X�`���T�C�Y�͔C�ӂ����A�t�H���g�T�C�Y�����e�N�X�`���T�C�Y�Ɉ���������BGraphicsUnit.Pixel�ȊO�ł��B
				var grid_map_img = new Bitmap(ol_w, ol_h);

				DrawLgLtGrid(grid_map_img, StartLgLt, EndLgLt, Cfg.GridFontSize);
				DrawUTMGrid (grid_map_img, StartLgLt, EndLgLt, Cfg.GridFontSize);

				// �n�\�ʂ���̍���
		 		var ol_offset = ToDouble(Cfg.GridOverlayCfg.Attributes["Offset"].Value);

				viewer.AddOverlay
					("grid",
					 new CImageMapData_LgLt(grid_map_img, StartLgLt, EndLgLt),
					 ol_offset,
					 1.0f); // �����x
			}

			//--------------------------------------------------
			// 3 �����I�ɃI�[�o���C���d�˂Ă݂�B
			// ���I�[�o���C���͈͊O�ɂ���Ɠ��R�G���[�ɂȂ�B(���܂��܂Ȃ�Ȃ��ꍇ������B)
			if(true)
			{
				// �I�[�o���C�̃T�C�Y�̊(�������ӂ����̃T�C�Y�ɂ���B)
				int ol_size = 500;
				int ol_w = (MapImage.Height > MapImage.Width )? ol_size: (ol_size * MapImage.Width  / MapImage.Height);
				int ol_h = (MapImage.Width  > MapImage.Height)? ol_size: (ol_size * MapImage.Height / MapImage.Width ); 

				// ��R
				var (ol_s_lglt, ol_e_lglt) = ExtendToMeshSize
					(new CLgLt(new CLg(new CDMS(130,  9, 0.0).DecimalDeg), new CLt(new CDMS(33, 34, 0.0).DecimalDeg)),
					 new CLgLt(new CLg(new CDMS(130, 10, 0.0).DecimalDeg), new CLt(new CDMS(33, 35, 0.0).DecimalDeg)),
					 Cfg.MeshSize);

				var ol = viewer.MakeOverlay(ol_s_lglt, ol_e_lglt, ol_w, ol_h);

				// �I�[�o���C�g���O�̑Ίp��
				var p_from = ol.ToPointOnOverlay(ol_s_lglt);
				var p_to   = ol.ToPointOnOverlay(ol_e_lglt);

				var g = ol.GetGraphics();

				g.DrawRectangle(new Pen(Color.Red, 5.0f), 0, 0, ol_w, ol_h);
			
				g.DrawLine(new Pen(Color.Red, 5.0f), p_from, p_to);
			
				g.Dispose();

				viewer.AddOverlay
					("test_ol",
					 ol,
					 200.0, // �n�\�ʂ���̍���
					 1.0f); // �����x
			}

			//--------------------------------------------------

			viewer.Draw();
		}
	}
}
