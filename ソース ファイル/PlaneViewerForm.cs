//
//
//
//---------------------------------------------------------------------------
using DSF_NET_SceneGraph;

using System;
using System.Drawing;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public abstract partial class PlaneViewerForm : Form
{
	public CPlaneViewer Viewer = null;

	public PlaneViewerForm()
	{
		InitializeComponent();

		// マウスホイールの回転で近づく・遠ざかるプロシージャを追加する。
		// コーディングでの追加が必要
		PictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(PictureBox_MouseWheel);
	}

	private void PictureBox_Resize(object sender, EventArgs e)
	{
		if(Viewer == null) return;
		Viewer.Resize(PictureBox.Width, PictureBox.Height);
	}

	private void PictureBox_Paint(object sender, PaintEventArgs e)
	{
		if(Viewer == null) return;
		Viewer.DrawScene();
	}

	private void PictureBox_MouseDown(object sender, MouseEventArgs e)
	{
		if(Viewer == null) return;
		Viewer.MouseDown(e);
	}

	private void PictureBox_MouseMove(object sender, MouseEventArgs e)
	{
		if(Viewer == null) return;
		Viewer.MouseMove(e);

		DispObjInfo();
	}

	private void PictureBox_MouseUp(object sender, MouseEventArgs e)
	{
		if(Viewer == null) return;
		Viewer.MouseUp(e);
	}

	private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
	{
		if(Viewer == null) return;
		Viewer.DistFB(-e.Delta * SystemInformation.MouseWheelScrollLines / 60); // ◆移動量は目分量	
	}

	private void PlaneViewerForm_Tude_KeyPress(object sender, KeyPressEventArgs e)
	{
		if(Viewer == null) return;

		if(e.KeyChar == 'w')
			Viewer.MoveFB(10);
		else if(e.KeyChar == 's')
			Viewer.MoveFB(-10);
		else
			return;
	}

	private void PlaneViewerForm_Tude_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		switch(e.KeyCode)
		{
			case Keys.Up   : Viewer.LookUD( 10); break;
			case Keys.Down : Viewer.LookUD(-10); break;
			case Keys.Right: Viewer.TurnRL(-10); break;
			case Keys.Left : Viewer.TurnRL( 10); break;
		}
	}

	public abstract void DispObjInfo();
}
//---------------------------------------------------------------------------
}