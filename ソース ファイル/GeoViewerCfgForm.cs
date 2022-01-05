//
// GeoViewerCfgForm.cs
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
public partial class GeoViewerCfgForm : Form
{
	private CGeoViewer Viewer = null;

	public GeoViewerCfgForm(CGeoViewer viewer)
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
		// [0,128]
		Viewer?
			.SetShininess(ShininessTrackBar.Value)
			.DrawScene();
	}

	// 環境光を設定する。
	private void AmbientTrackBar_Scroll(object sender, EventArgs e)
	{
		// [0,1]
		Viewer?
			.SetAmbient(System.Convert.ToSingle(AmbientTrackBar.Value) / 100.0f)
			.DrawScene();
	}

	// 鏡面光を設定する。
	private void SpecularTrackBar_Scroll(object sender, EventArgs e)
	{
		// [0,1]
		Viewer?
			.SetSpecular(Convert.ToSingle(SpecularTrackBar.Value) / 100.0f)
			.DrawScene();
	}

	// 標高拡大率を設定する。
	private void ElevationMagnifyTrackBar_Scroll(object sender, EventArgs e)
	{
		Viewer?
			.MagnifyElevation(ElevationMagnifyTrackBar.Value)
			.DrawScene();
	}

	// シェーディングモードをワイヤフレームに設定する。
	private void WireRadioButton_CheckedChanged(Object sender, EventArgs e)
	{
		Viewer?
			.SetShadingMode(DShadingMode.SHADING_WIRE)
			.DrawScene();
	}

	// シェーディングモードをフラットシェーディングに設定する。
	private void FlatRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetShadingMode(DShadingMode.SHADING_FLAT)
			.DrawScene();
	}

	// シェーディングモードをスムースシェーディングに設定する。
	private void SmoothRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetShadingMode(DShadingMode.SHADING_SMOOTH)
			.DrawScene();
	}

	// シェーディングモードをマッピングに設定する。
	private void MappingRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetShadingMode(DShadingMode.SHADING_MAPPING)
			.DrawScene();
	}

	// 視程モードを無限遠に設定する。
	private void FogNoRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetFogMode(DFogMode.FOG_NO)
			.DrawScene();
	}

	// 視程モード霧に設定する。
	private void FogFogRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetFogMode(DFogMode.FOG_FOG)
			.DrawScene();
	}

	// 視程モード夜暗に設定する。
	private void FogDarkRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetFogMode(DFogMode.FOG_DARK)
			.DrawScene();
	}

	//	視程を設定する。
	private void VisibilityNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetVisibility(Convert.ToInt32(VisibilityNumericUpDown.Value))
			.DrawScene();
	}

	// 鏡面光視点を有効／無効にする。
	private void LocalViewCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetLocalView(LocalViewCheckBox.Checked? true: false)
			.DrawScene();
	}

	// マーカーを表示／非表示にする。
	private void MarkerCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		Viewer?
			.SetMarkerMode(MarkerCheckBox.Checked? true: false)
			.DrawScene();
	}
	}
//---------------------------------------------------------------------------
}