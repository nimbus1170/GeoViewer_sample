//
// SceneViewerConfigFor.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;

using System;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerConfigForm : Form
{
	private CPlaneViewer Viewer = null;

	public PlaneViewerConfigForm(CPlaneViewer viewer)
	{
		InitializeComponent();

		Viewer = viewer;

		ShininessTrackBar.Value = ToInt32(Viewer.Shininess()	  );
		AmbientTrackBar  .Value = ToInt32(Viewer.Ambient  () * 100);
		SpecularTrackBar .Value = ToInt32(Viewer.Specular () * 100);

		switch(Viewer.ShadingMode())
		{
			case DShadingMode.SHADING_FLAT   : FlatRadioButton   .Checked = true; break;
			case DShadingMode.SHADING_SMOOTH : SmoothRadioButton .Checked = true; break;
			case DShadingMode.SHADING_MAPPING: MappingRadioButton.Checked = true; break;
		}

		switch(Viewer.FogMode())
		{
			case DFogMode.FOG_NO  : FogNoRadioButton  .Checked = true; break;
			case DFogMode.FOG_FOG : FogFogRadioButton .Checked = true; break;
			case DFogMode.FOG_DARK: FogDarkRadioButton.Checked = true; break;
		}

		VisibilityNumericUpDown.Value = (decimal)(Viewer.Visibility());
	}

	// ハイライトを設定する。
	private void ShininessTrackBar_Scroll(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		// [0,128]
		Viewer.SetShininess(ShininessTrackBar.Value);

		Viewer.DrawScene();
	}

	// 環境光を設定する。
	private void AmbientTrackBar_Scroll(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		// [0,1]
		Viewer.SetAmbient(System.Convert.ToSingle(AmbientTrackBar.Value) / 100.0f);

		Viewer.DrawScene();
	}

	// 鏡面光を設定する。
	private void SpecularTrackBar_Scroll(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		// [0,1]
		Viewer.SetSpecular(Convert.ToSingle(SpecularTrackBar.Value) / 100.0f);

		Viewer.DrawScene();
	}

	// シェーディングモードをフラットシェーディングに設定する。
	private void FlatRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetShadingMode(DShadingMode.SHADING_FLAT);

		Viewer.DrawScene();
	}

	// シェーディングモードをスムースシェーディングに設定する。
	private void SmoothRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetShadingMode(DShadingMode.SHADING_SMOOTH);

		Viewer.DrawScene();
	}

	// シェーディングモードをマッピングに設定する。
	private void MappingRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetShadingMode(DShadingMode.SHADING_MAPPING);

		Viewer.DrawScene();
	}

	// 視程モードを無限遠に設定する。
	private void FogNoRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetFogMode(DFogMode.FOG_NO);

		Viewer.DrawScene();
	}

	// 視程モード霧に設定する。
	private void FogFogRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetFogMode(DFogMode.FOG_FOG);

		Viewer.DrawScene();
	}

	// 視程モード夜暗に設定する。
	private void FogDarkRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetFogMode(DFogMode.FOG_DARK);

		Viewer.DrawScene();
	}

	//	視程を設定する。
	private void VisibilityNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetVisibility(Convert.ToInt32(VisibilityNumericUpDown.Value));

		Viewer.DrawScene();
	}

	// 鏡面光視点を有効／無効にする。
	private void LocalViewCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetLocalView(LocalViewCheckBox.Checked? true: false);

		Viewer.DrawScene();
	}

	// マーカーを表示／非表示にする。
	private void MarkerCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if(Viewer == null) return;

		Viewer.SetMarkerMode(MarkerCheckBox.Checked? true: false);

		Viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}