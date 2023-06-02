//
//
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_UTM;
using static DSF_NET_Geography.Convert_MGRS_UTM;

using System;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	//---------------------------------------------------------------------------
	public abstract partial class GeoViewerForm :Form
	{
		// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
		public CGeoViewer Viewer = null;

		public GeoViewerForm()
		{
			InitializeComponent();

			// マウスホイールの回転で近づく・遠ざかるプロシージャを追加する。
			// コーディングでの追加が必要
			PictureBox.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
		}

		private void PictureBox_Resize(object sender, EventArgs e)
		{
			Viewer?.Resize(PictureBox.Width, PictureBox.Height);
		}

		private void PictureBox_Paint(object sender, PaintEventArgs e)
		{
			Viewer?.DrawScene();
		}

		private void PictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			switch(e.Button)
			{
				case MouseButtons.Middle:
					contextMenuStrip1.Show(Left + e.X, Top + e.Y);
					break;

				default:
					Viewer?.MouseDown(e);
					break;
			}
		}

		private void PictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			Viewer?.MouseMove(e).DrawScene();

			ShowObjInfo();
		}

		private void PictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			Viewer?.MouseUp(e);
		}

		private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
		{
			Viewer?
				.DistFB(-e.Delta * SystemInformation.MouseWheelScrollLines / 60) // ◆移動量は目分量	
				.DrawScene();
		}

		private void PlaneViewerForm_LgLt_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(Viewer == null) return;

			switch(e.KeyChar)
			{ 
				case 'w': Viewer.MoveFB( 10); break;
				case 's': Viewer.MoveFB(-10); break;
				case 'a': Viewer.MoveRL( 10); break;
				case 'd': Viewer.MoveRL(-10); break;
			}

			Viewer.DrawScene();

/*			Viewer?
				.MoveFB
					((e.KeyChar == 'w')?  10:
					 (e.KeyChar == 's')? -10:
										   0) // ◆ムダ
				.MoveRL					   
					((e.KeyChar == 'a')?  10:
					 (e.KeyChar == 'd')? -10:
										   0)
				.DrawScene();
*/
			ShowObjInfo();
		}

		private void PlaneViewerForm_LgLt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(Viewer == null) return;

			switch(e.KeyCode)
			{
				case Keys.Up   : Viewer.LookUD( 10); break;
				case Keys.Down : Viewer.LookUD(-10); break;
				case Keys.Right: Viewer.TurnRL(-10); break;
				case Keys.Left : Viewer.TurnRL( 10); break;
			}

			Viewer.DrawScene();
		}

		private void MarkerToolStripMenuItem_Click(Object sender, EventArgs e)
		{
			if(Viewer == null) return;

			var item = (ToolStripMenuItem)sender;

			item.Checked = !(item.Checked);

			Viewer
				.SetMarkerMode(item.Checked)
				.DrawScene();
		}

		public abstract void ShowObjInfo();

		protected void ShowObjInfoImpl(CLgLt ct)
		{
			var ct_lg_deg = ct.Lg.DecimalDeg;
			var ct_lt_deg = ct.Lt.DecimalDeg;

			var ct_lg_dms = new CDMS(ct_lg_deg);
			var ct_lt_dms = new CDMS(ct_lt_deg);

			var ct_utm = ToUTM(ct);

			var (x, y, z) = BLHToXYZ(ct_lt_deg, ct_lg_deg, ct.GetAltitude(DAltitudeBase.AE));

			ObjInfoLabel.Text =
				$"東経 {ct_lg_dms.Deg:000}度{ct_lg_dms.Min:00}分{ct_lg_dms.Sec:00.000}秒 ({ct_lg_deg:000.00000}度)\n" +
				$"北緯  {ct_lt_dms.Deg:00}度{ct_lt_dms.Min:00}分{ct_lt_dms.Sec:00.000}秒 ( {ct_lt_deg:00.00000}度)\n" +
				$"UTM  {ct_utm.LgBand:00}{((ct_utm.Hemi == DHemi.N) ? "n" : "s")}   {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
				$"MGRS {ct_utm.LgBand:00}{GetLtBand(ToLgLt(ct_utm).Lt):0} {GetMGRS_ID(ct_utm):00} {GetMGRS_EW(ct_utm):00000}   {GetMGRS_NS(ct_utm):00000}\n" +
				$"標高 {ct.GetAltitude(DAltitudeBase.AMSL):0.0}m　ジオイド高 {ct.GetAltitude(DAltitudeBase.AE) - ct.GetAltitude(DAltitudeBase.AMSL):0.000}m\n" +
				$"地心直交座標 X:{(int)x:#,0}m Y:{(int)y:#,0}m Z:{(int)z:#,0}m";
		}
	}
	//---------------------------------------------------------------------------
}
