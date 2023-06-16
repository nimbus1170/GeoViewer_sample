//
// GeoViewer_CfgForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;

using System;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
    //---------------------------------------------------------------------------
    public partial class GeoViewerCfgForm : Form
    {
        private readonly CGeoViewer Viewer = null;

        public GeoViewerCfgForm(in CGeoViewer viewer)
        {
            InitializeComponent();

            Viewer = viewer;

            ShininessTrackBar.Value = ToInt32(Viewer.Shininess());
            AmbientTrackBar  .Value = ToInt32(Viewer.Ambient  () * 100);
            SpecularTrackBar .Value = ToInt32(Viewer.Specular () * 100);

            ShadingModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            switch (Viewer.ShadingMode())
            {
                case DShadingMode.TEXTURE_IMAGE: ShadingModeComboBox.SelectedIndex = 0; break;
                case DShadingMode.TEXTURE_PHOTO: ShadingModeComboBox.SelectedIndex = 1; break;
                case DShadingMode.SMOOTH       : ShadingModeComboBox.SelectedIndex = 2; break;
                case DShadingMode.FLAT         : ShadingModeComboBox.SelectedIndex = 3; break;
                case DShadingMode.WIRE         : ShadingModeComboBox.SelectedIndex = 4; break;
            }

            PolygonModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            switch (Viewer.PolygonMode())
            {
                case DPolygonMode.QUAD5   : PolygonModeComboBox.SelectedIndex = 0; break;
                case DPolygonMode.QUAD4   : PolygonModeComboBox.SelectedIndex = 1; break;
                case DPolygonMode.TRIANGLE: PolygonModeComboBox.SelectedIndex = 2; break;
            }

            FogModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            switch (Viewer.FogMode())
            {
                case DFogMode.CLEAR: FogModeComboBox.SelectedIndex = 0; break;
                case DFogMode.FOG  : FogModeComboBox.SelectedIndex = 1; break;
                case DFogMode.DARK : FogModeComboBox.SelectedIndex = 2; break;
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
                .SetAmbient(ToSingle(AmbientTrackBar.Value) / 100.0f)
                .DrawScene();
        }

        // 鏡面光を設定する。
        private void SpecularTrackBar_Scroll(object sender, EventArgs e)
        {
            // [0,1]
            Viewer?
                .SetSpecular(ToSingle(SpecularTrackBar.Value) / 100.0f)
                .DrawScene();
        }

        // 標高拡大率を設定する。
        private void ElevationMagnifyTrackBar_Scroll(object sender, EventArgs e)
        {
            Viewer?
                .MagnifyElevation(ElevationMagnifyTrackBar.Value)
                .DrawScene();
        }

        //	視程を設定する。
        private void VisibilityNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Viewer?
                .SetVisibility(ToInt32(VisibilityNumericUpDown.Value))
                .DrawScene();
        }

        // 鏡面光視点を有効／無効にする。
        private void LocalViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Viewer?
                .SetLocalView(LocalViewCheckBox.Checked)
                .DrawScene();
        }

        // マーカーを表示/非表示する。
        private void MarkerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Viewer?
                .SetMarkerMode(MarkerCheckBox.Checked)
                .DrawScene();
        }

        private void ShadingModeComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (ShadingModeComboBox.SelectedIndex)
            {
                case 0:
                    Viewer?.SetShadingMode(DShadingMode.TEXTURE_IMAGE);
                    PolygonModeComboBox.SelectedIndex = 1; // ◆テクスチャは四角ポリゴンのみ対応
                    PolygonModeComboBox.Enabled = false;
                    break;

                case 1:
                    Viewer?.SetShadingMode(DShadingMode.TEXTURE_PHOTO);
                    PolygonModeComboBox.SelectedIndex = 1; // ◆テクスチャは四角ポリゴンのみ対応
                    PolygonModeComboBox.Enabled = false;
                    break;

                case 2:
                    Viewer?.SetShadingMode(DShadingMode.SMOOTH);
                    PolygonModeComboBox.Enabled = true;
                    break;

                case 3:
                    Viewer?.SetShadingMode(DShadingMode.FLAT);
                    PolygonModeComboBox.Enabled = true;
                    break;

                case 4:
                    Viewer?.SetShadingMode(DShadingMode.WIRE);
                    PolygonModeComboBox.Enabled = true;
                    break;
            }

            Viewer?.DrawScene();
        }

        private void PolygonModeComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (PolygonModeComboBox.SelectedIndex)
            {
                case 0: Viewer?.SetPolygonMode(DPolygonMode.QUAD5)   ; break;
                case 1: Viewer?.SetPolygonMode(DPolygonMode.QUAD4)   ; break;
                case 2: Viewer?.SetPolygonMode(DPolygonMode.TRIANGLE); break;
            }

            Viewer?.DrawScene();
        }

        private void VisibilityComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (FogModeComboBox.SelectedIndex)
            {
                case 0: Viewer?.SetFogMode(DFogMode.CLEAR); VisibilityNumericUpDown.Enabled = false; break;
                case 1: Viewer?.SetFogMode(DFogMode.FOG)  ; VisibilityNumericUpDown.Enabled = true ; break;
                case 2: Viewer?.SetFogMode(DFogMode.DARK) ; VisibilityNumericUpDown.Enabled = true ; break;
            }

            Viewer?.DrawScene();
        }
    }
    //---------------------------------------------------------------------------
}
