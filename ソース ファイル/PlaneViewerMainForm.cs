//
//
//
//---------------------------------------------------------------------------
using DSF_NET_SceneGraph;

using DSF_NET_Utility;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using static System.Convert;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	CGeoViewer_WP	GeoViewer_WP;
	CGeoViewer_Tude	GeoViewer_Tude; 
	CXYZPlaneViewer XYZPlaneViewer; 

	CPlaneViewer Viewer = null;

	readonly string ConfigFileName;

	CInfoMap   Info		 = new CInfoMap	 ();
	CStopwatch Stopwatch = new CStopwatch();

	int PlaneSizeEW = 0;
	int PlaneSizeNS = 0;

	public PlaneViewerMainForm(string[] args)
	{
		InitializeComponent();

		ConfigFileName = args[0];
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		// ◆Form_Loadだと、このフォームは最後まで表示されない
		// 　非同期にできないか？すべきか？

		try
		{
			// ◆どれか一つ選んで実行
			Run_GeoViewer_WP();
			//Run_GeoViewer_Tile();
			//Run_GeoViewer_Tude();
			//Run_XYZPlaneViewer();

			//--------------------------------------------------

			var info_dictionary = Info.ToDictionary();

			var vert_nx = info_dictionary["VertexNX"];
			var vert_ny = info_dictionary["VertexNY"];

			MessageListBox.Items.Add($"        vertex_nx : {vert_nx}");
			MessageListBox.Items.Add($"        vertex_ny : {vert_ny}");
			MessageListBox.Items.Add($"    tri_polygon_n : {(vert_nx - 1) * (vert_ny - 1) * 2}");
			MessageListBox.Items.Add($"            tex_w : {info_dictionary["TexW"]}");
			MessageListBox.Items.Add($"            tex_h : {info_dictionary["TexH"]}");
			MessageListBox.Items.Add($"            tex_n : {info_dictionary["TexN"]}");
			MessageListBox.Items.Add($"    PlaneSizeEW_m : {PlaneSizeEW}");
			MessageListBox.Items.Add($"    PlaneSizeNS_m : {PlaneSizeNS}");
			MessageListBox.Items.Add($"polygon_size_EW_m : {PlaneSizeEW / (vert_nx - 1)}");
			MessageListBox.Items.Add($"polygon_size_NS_m : {PlaneSizeNS / (vert_ny - 1)}");
			MessageListBox.Items.Add($"");

			//--------------------------------------------------

			Stopwatch.Stop();

			MessageListBox.Items.Add("elapsed times in millisecond");

			var total_time = Stopwatch.TotalTime;

			foreach(var laptimes_i in Stopwatch.LapTimes)
			{
				var laptime = laptimes_i.Value;

				double laptime_percentage = ToDouble(laptime) / total_time * 100;

				MessageListBox.Items.Add($"{laptime, 5} ({laptime_percentage, 4:0.0}%) : {laptimes_i.Key}");
			}

			MessageListBox.Items.Add($"total {total_time} milliseond");
		}
		catch(System.Xml.XPath.XPathException ex)
		{
			MessageListBox.Items.Add("XPath Error > " + ex.Message);
		}
		catch(System.NullReferenceException ex)
		{
			MessageListBox.Items.Add("Error > " + ex.Message);
		}
		catch(Exception ex)
		{
			MessageListBox.Items.Add("Error > " + ex.Message);
		}
	}
}
//---------------------------------------------------------------------------
}
