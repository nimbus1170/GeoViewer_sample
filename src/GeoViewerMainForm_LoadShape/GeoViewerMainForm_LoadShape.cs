//
// GeoViewerMainForm_LoadShape.cs
//
//---------------------------------------------------------------------------
using System.Runtime.Versioning;

using static DSF_CS_Profiler.CProfilerLog;

using System.DirectoryServices;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	void LoadShape()
	{
		OpenFileDialog of_dialog = new ()
			{ Title  = "図形ファイルを開く",
			  Filter = "図形ファイル(*.shp;*.geojson;*.json;*.kml)|*.shp;*.geojson;*.json;*.kml",
			  Multiselect = true };

		if(of_dialog.ShowDialog() == DialogResult.Cancel) return;

		var shape_fnames = of_dialog.FileNames;

		of_dialog.Dispose();

StopWatch.Start();
StopWatch.Lap("before load shapes");
MemWatch .Lap("before load shapes");

		foreach(var shape_fname in shape_fnames)
		{
			DialogTextBox.AppendText($"loading {shape_fname}\r\n");

			// ◆取り敢えず、未対応の図形は例外で返す。
			try
			{
				switch(Path.GetExtension(shape_fname))
				{
					case ".shp":
					{
						var (shp_file, read_shp_msg) = ReadShapefileFromFile(shape_fname);

						if(shp_file != null)
						{
							var shp_name = "shp" + (++ShapesN);

							DrawShapefile(shp_name, shp_file);

							ShowShapefileLog(shp_name, shp_file, read_shp_msg);
						}
						else
							DialogTextBox.AppendText($"reading SHP error : {read_shp_msg}\r\n");

						break;
					}
				
					case ".geojson":
					case ".json":
					{
						ReadDrawGeoJSONFromFile(shape_fname);
						break;
					}

					case ".kml":
					{
						ReadDrawKMLFromFile(shape_fname);
						break;
					}

					default:
						DialogTextBox.AppendText($"unknown shape file type : {shape_fname}\r\n");
						break;
					
				}
			}
			catch(Exception ex)
			{
				DialogTextBox.AppendText(ex.Message + "\r\n");
			}

		}

StopWatch.Lap("after  load shapes");
MemWatch .Lap("after  load shapes");
StopWatch.Stop();
MemWatch .Stop();

		if(ToShowDebugInfo)
		{
			DialogTextBox.AppendText(MakeStopWatchLog(StopWatch));
			DialogTextBox.AppendText(MakeMemWatchLog (MemWatch ));
			DialogTextBox.AppendText("\r\n");
		}
	}

	//-----------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
