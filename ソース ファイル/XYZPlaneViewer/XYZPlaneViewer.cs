//
// XYZPlaneViewer.cs
// XYZ平面ビューア
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using System;
using System.Drawing;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	void Run_XYZPlaneViewer()
	{
		//--------------------------------------------------

		var title = "XYZ";

		//--------------------------------------------------

		Text = title;

		//--------------------------------------------------

		var s_coord = new CCoord(    0,     0);
		var e_coord = new CCoord(10000, 10000);

		//--------------------------------------------------
		// 1 地表面画像データを作成する。

		Stopwatch.Lap("build map image");

		var img_map_data = 
//			new CImageMapData_XYZ(new Bitmap("C:/DSF/SharedData/Images/地面.bmp"), s_coord, e_coord);
			new CImageMapData_XYZ(new Bitmap("./Images/地面.bmp"), s_coord, e_coord);

		Stopwatch.Lap("- map image built");

		//--------------------------------------------------
		// 2 ビューアフォームを作成する。

		Stopwatch.Lap("build viewer form");

		var viewer_form = new XYZPlaneViewerForm();

		Stopwatch.Lap("- viewer form built");

		viewer_form.Text = title;

		viewer_form.Show();

		//--------------------------------------------------
		// 3 ビューアパラメータを作成する。← 1,2

		var viewer_params = new CXYZPlaneViewerParameters();

		{ 
			viewer_params.viewer_control = viewer_form.PictureBox;
			viewer_params.s_coord = s_coord;
			viewer_params.e_coord = e_coord;
			viewer_params.polygon_size = 400; // ◆不要？
			viewer_params.img_map_data = img_map_data;
			viewer_params.options = "";
		}

		//--------------------------------------------------
		// 4 コントローラフォームを作成する。

		Stopwatch.Lap("build controller form");

		var controller_form = new XYZPlaneViewerControllerForm(XYZPlaneViewer);

		Stopwatch.Lap("- controller form built");

		controller_form.Text = title;

		controller_form.Show();

		//--------------------------------------------------
		// 5 コントローラパラメータを作成する。← 4

		var controller_parts = new CControllerParts();

		{ 
			controller_parts.ObjXTextBox   = controller_form.ObjXTextBox;
			controller_parts.ObjXScrollBar = controller_form.ObjXScrollBar;
			controller_parts.MaxObjXLabel  = controller_form.MaxObjXLabel;
			controller_parts.MinObjXLabel  = controller_form.MinObjXLabel;

			controller_parts.ObjYTextBox   = controller_form.ObjYTextBox;
			controller_parts.ObjYScrollBar = controller_form.ObjYScrollBar;
			controller_parts.MaxObjYLabel  = controller_form.MaxObjYLabel;
			controller_parts.MinObjYLabel  = controller_form.MinObjYLabel;

			controller_parts.DirScrollBar = controller_form.DirScrollBar;
			controller_parts.DirTextBox   = controller_form.DirTextBox;

			controller_parts.DistanceScrollBar = controller_form.DistanceScrollBar;
			controller_parts.DistanceTextBox   = controller_form.DistanceTextBox;

			controller_parts.AngleScrollBar = controller_form.AngleScrollBar;
			controller_parts.AngleTextBox   = controller_form.AngleTextBox;

			controller_parts.ObserverXScrollBar = controller_form.ObserverXScrollBar;
			controller_parts.ObserverXTextBox   = controller_form.ObserverXTextBox;
			controller_parts.MaxObserverXLabel  = controller_form.MaxObserverXLabel;
			controller_parts.MinObserverXLabel  = controller_form.MinObserverXLabel;

			controller_parts.ObserverYScrollBar = controller_form.ObserverYScrollBar;
			controller_parts.ObserverYTextBox   = controller_form.ObserverYTextBox;
			controller_parts.MaxObserverYLabel  = controller_form.MaxObserverYLabel;
			controller_parts.MinObserverYLabel  = controller_form.MinObserverYLabel;

			controller_parts.ObserverAltitudeScrollBar = controller_form.ObserverAltitudeScrollBar;
			controller_parts.ObserverAltitudeTextBox   = controller_form.ObserverAltitudeTextBox;
		}

		//--------------------------------------------------
		// 6 表示設定を作成する。

		var scene_config = new CSceneConfig
			(0.8f, // 環境光反射係数 [0,1]
			 1.0f, // 鏡面反射係数   [0,1]
			 128,  // ハイライト     [0,128]
			 DShadingMode.SHADING_MAPPING,
			 DFogMode.FOG_NO,
			 3000); // 視程

		//--------------------------------------------------
		// 7 ビューアを作成する。← 3,5,6,7

		Stopwatch.Lap("build viewer");

		XYZPlaneViewer = new CXYZPlaneViewer(viewer_params, scene_config, controller_parts, Info, Stopwatch);

		Stopwatch.Lap("- viewer built");

		//--------------------------------------------------
		// 8 ビューアフォームとコントローラフォームにビューアを設定する。← 7

		viewer_form	   .Viewer = XYZPlaneViewer;
		controller_form.Viewer = XYZPlaneViewer;

		//--------------------------------------------------
		// 9 表示設定フォームを作成する。← 7

		var config_form = new PlaneViewerConfigForm(XYZPlaneViewer);

		config_form.Text = title;

		config_form.Show();

		//--------------------------------------------------
		// 10 シーンを描画する。← 7

		Stopwatch.Lap("create scene");

		XYZPlaneViewer.CreateScene();

		Stopwatch.Lap("- scene created");

		//--------------------------------------------------
		// 11 図形を描画する。← 7

		Stopwatch.Lap("draw shapes");

		XYZPlaneViewerDrawShapes();

		Stopwatch.Lap("- shapes drawn");

		//--------------------------------------------------

		Viewer = XYZPlaneViewer;
	}
}
//---------------------------------------------------------------------------
}
