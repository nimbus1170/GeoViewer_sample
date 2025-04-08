//
// GeoViewerForm.cs
// ビューアフォーム
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_UTM;
using static DSF_NET_Geography.Convert_MGRS_UTM;

using DSF_NET_Geometry;
using DSF_NET_Scene;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	// ◆デザイナからイベントが追加されると自動フォーマットされるようなので、規定どおり名前空間でインデントしておく。
	public abstract partial class GeoViewerViewForm : Form
	{
		// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
		internal CGeoViewer Viewer = null;

		internal GeoViewerMainForm MainForm = null;

		public GeoViewerViewForm()
		{
			InitializeComponent();

			// マウスホイールの回転で近づく・遠ざかるプロシージャを追加する。
			// コーディングでの追加が必要
			PictureBox.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
		}

		private void PictureBox_Resize(object sender, EventArgs e)
		{
			Viewer?.Resize();
		}

		private void PictureBox_Paint(object sender, PaintEventArgs e)
		{
			Viewer?.Draw();
		}

		private void PictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if(Viewer == null) return;

			switch(e.Button)
			{
				case MouseButtons.Middle:
					contextMenuStrip1.Show(Left + e.X, Top + e.Y);
					break;

				default:
					if(Viewer.IsCamPosLocked && (e.Button != MouseButtons.Left)) Viewer.IsCamPosLocked = false;
					// ◆マウスボタンとコントロールモードの組み合わせはここで指定できるようにしておくべき。
					Viewer.MouseDown(e);
					break;
			}
		}

		private void PictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			Viewer?.MouseMove(e).Draw();
			ShowInfo();
		}

		private void PictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			Viewer?.MouseUp(e);
		}

		private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
		{
			if(Viewer == null) return;

			if(Viewer.IsCamPosLocked) Viewer.IsCamPosLocked = false;

			// 注視点高度変更
			if(true)
			{
				// 増減量倍率
				// Ctrlキーが押されているときは増減量を10にする。
				int dd = ((Control.ModifierKeys & Keys.Control) == Keys.Control) ? 10 : 1;

				// 1ホイールステップで1m増減するようにする。
				// ◆e.Daltaは1ホイールステップが120のようだ。
				// ◆ここでの100はRESOLUTION_IN_METERだが、ビューアから取れるようにしろ。
				Viewer.MoveUD(-e.Delta / 120 * dd * 100).Draw();
			}

			// 距離増減
			if(false)
			{
				int dd = 120;

				// Ctrlキーが押されているときは、ddを半分にする
				if((Control.ModifierKeys & Keys.Control) == Keys.Control) dd /= 4;

				Viewer
					.DistFB(-e.Delta * SystemInformation.MouseWheelScrollLines / dd) // ◆移動量は目分量	
					.Draw();
			}

			ShowInfo();
		}

		private void GeoViewerForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(Viewer == null) return;

			switch(e.KeyChar)
			{
				case 'w': Viewer.MoveFB( 10); break;
				case 's': Viewer.MoveFB(-10); break;
				case 'a': Viewer.MoveRL( 10); break;
				case 'd': Viewer.MoveRL(-10); break;

				case 'l':
					Viewer.IsCamPosLocked = !Viewer.IsCamPosLocked;
					break;
			}

			Viewer.Draw();

	/*		Viewer
				.MoveFB
					((e.KeyChar == 'w')?  10:
					(e.KeyChar  == 's')? -10:
											0) // ◆ムダ
				.MoveRL					   
					((e.KeyChar == 'a')?  10:
					(e.KeyChar  == 'd')? -10:
											0)
				.Draw();
	*/

			ShowInfo();
		}

		private void GeoViewerForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(Viewer == null) return;

			switch(e.KeyCode)
			{
				case Keys.Up   : Viewer.LookUD( 10); break;
				case Keys.Down : Viewer.LookUD(-10); break;
				case Keys.Right: Viewer.TurnRL(-10); break;
				case Keys.Left : Viewer.TurnRL( 10); break;
			}

			Viewer.Draw();
		}

		private void MarkerToolStripMenuItem_Click(Object sender, EventArgs e)
		{
			if(MainForm == null) return;

			var item = (ToolStripMenuItem)sender;

			item.Checked = !(item.Checked);

			// メインフォームに反映・処理する。

			MainForm.MarkerCheckBox.Checked = !(MainForm.MarkerCheckBox.Checked);

			MainForm.MarkerCheckBox_CheckedChanged(sender, e);
		}

		internal abstract void ShowInfo();

		internal Dictionary<string, string> AdditionalInfo = [];

		protected void ShowInfoImpl(in CLgLt obj, in CLgLt cam)
		{
			if(Viewer == null) return;

			var obj_lg_deg = obj.Lg.DecimalDeg;
			var obj_lt_deg = obj.Lt.DecimalDeg;

			var obj_lg_dms = new CDMS(obj_lg_deg);
			var obj_lt_dms = new CDMS(obj_lt_deg);

			var origin_name = Convert_LgLt_XY.Origin.Name;
			var obj_xy = Convert_LgLt_XY.ToXY(obj);

			var obj_utm = ToUTM(obj);

			var (obj_x, obj_y, obj_z) = BLHToXYZ(obj_lt_deg, obj_lg_deg, obj.AltitudeAE);

			var cam_lg_deg = cam.Lg.DecimalDeg;
			var cam_lt_deg = cam.Lt.DecimalDeg;

			var cam_lg_dms = new CDMS(cam_lg_deg);
			var cam_lt_dms = new CDMS(cam_lt_deg);

			var cam_xy = Convert_LgLt_XY.ToXY(cam);

			var cam_utm = ToUTM(cam);

			var (cam_x, cam_y, cam_z) = BLHToXYZ(cam_lt_deg, cam_lg_deg, cam.AltitudeAE);

			// ◆ここでの100.0はRESOLUTION_IN_METERだが、ビューアから取れるようにしろ。
			var dist_m = Viewer.Dist / 100.0;

			// 注視点での画面に見える縦サイズ(m)
			var h_m = 2.0 * dist_m * Math.Tan(Viewer.Fovy * Math.PI / 180.0 / 2.0);

			// MainForm.ToShowInfoの最初の7個の要素のうち、trueが1つでもあれば、trueを返す。
			var to_show_obj_cam_info = MainForm.ToShowInfo.Take(7).Any(to_show_Info => to_show_Info);

			InfoLabel.Text =
				(to_show_obj_cam_info?	 $"注視点\n": "") +
				(MainForm.ToShowInfo[0]? $"  東経{obj_lg_dms.Deg:000}度{obj_lg_dms.Min:00}分{obj_lg_dms.Sec:00.000}秒 北緯{obj_lt_dms.Deg:00}度{obj_lt_dms.Min:00}分{obj_lt_dms.Sec:00.000}秒\n": "") +
				(MainForm.ToShowInfo[1]? $"  東経{obj_lg_deg:000.00000} 北緯{obj_lt_deg:00.00000}\n": "") +
				(MainForm.ToShowInfo[2]? $"  {origin_name}系 {obj_xy.X:#,0.00} {obj_xy.Y:#,0.00}\n": "") +
				(MainForm.ToShowInfo[3]? $"  UTM  {obj_utm.LgBand:00}{((obj_utm.Hemi == DHemi.N) ? "n" : "s")}   {obj_utm.EW:00000} {obj_utm.NS:00000}\n": "") +
				(MainForm.ToShowInfo[4]? $"  MGRS {obj_utm.LgBand:00}{GetLtBand(ToLgLt(obj_utm).Lt):0} {GetMGRS_ID(obj_utm):00} {GetMGRS_EW(obj_utm):00000}   {GetMGRS_NS(obj_utm):00000}\n": "") +
				(MainForm.ToShowInfo[5]? $"  標高{obj.Elevation:0.0}m　地上高{obj.AltitudeAGL:0.0}m　ジオイド高{obj.AltitudeAE - obj.AltitudeAMSL:0.000}m\n": "") +
				(MainForm.ToShowInfo[6]? $"  地心直交座標 X:{(int)obj_x:#,0}m Y:{(int)obj_y:#,0}m Z:{(int)obj_z:#,0}m\n": "") +
				(to_show_obj_cam_info?	 $"カメラ\n": "") +
				(MainForm.ToShowInfo[0]? $"  東経{cam_lg_dms.Deg:000}度{cam_lg_dms.Min:00}分{cam_lg_dms.Sec:00.000}秒 北緯{cam_lt_dms.Deg:00}度{cam_lt_dms.Min:00}分{cam_lt_dms.Sec:00.000}秒\n": "") +
				(MainForm.ToShowInfo[1]? $"  東経{cam_lg_deg:000.00000} 北緯{cam_lt_deg:00.00000}\n": "") +
				(MainForm.ToShowInfo[2]? $"  {origin_name}系 {cam_xy.X:#,0.00} {cam_xy.Y:#,0.00}\n": "") +
				(MainForm.ToShowInfo[3]? $"  UTM  {cam_utm.LgBand:00}{((cam_utm.Hemi == DHemi.N) ? "n" : "s")}   {cam_utm.EW:00000} {cam_utm.NS:00000}\n": "") +
				(MainForm.ToShowInfo[4]? $"  MGRS {cam_utm.LgBand:00}{GetLtBand(ToLgLt(cam_utm).Lt):0} {GetMGRS_ID(cam_utm):00} {GetMGRS_EW(cam_utm):00000}   {GetMGRS_NS(cam_utm):00000}\n": "") +
				(MainForm.ToShowInfo[5]? $"  標高{cam.Elevation:0.0}m　地上高{cam.AltitudeAGL:0.0}m　ジオイド高{cam.AltitudeAE - cam.AltitudeAMSL:0.000}m\n": "") +
				(MainForm.ToShowInfo[6]? $"  地心直交座標 X:{(int)cam_x:#,0}m Y:{(int)cam_y:#,0}m Z:{(int)cam_z:#,0}m\n": "") +
				(MainForm.ToShowInfo[7]? $"カメラ → 注視点\n": "") +
				(MainForm.ToShowInfo[7]? $"  距離{dist_m:0.}m　方位角{Viewer.Dir:0.0}度　俯角{Viewer.Angle:0.0}度\n": "") +
				(Viewer.IsCamPosLocked ? "カメラ位置固定中\n" : "") +
				$"\n" +
				$"画面サイズ {PictureBox.Width}x{PictureBox.Height}\n" +
				$"視野角(縦) {Viewer.Fovy}\n" +
				$"注視点での画面に見える縦サイズ {h_m:0.0}m\n" +
				$"注視点での1m当たりのピクセル数 {PictureBox.Height / h_m:0.0}px/m";

			foreach(var additional_info in AdditionalInfo)
			{
				InfoLabel.Text += "\r\n" + additional_info.Value + "\r\n";
			}
		}
	}
}
