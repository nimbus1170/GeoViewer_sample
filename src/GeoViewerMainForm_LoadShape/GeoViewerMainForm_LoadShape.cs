//
// GeoViewerMainForm_LoadShape.cs
//
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
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
				ShowLog($"loading {shape_fname}\r\n");

				// ◆未対応の図形は取り敢えず例外で返す。
				try
				{
					switch(Path.GetExtension(shape_fname))
					{
						case ".shp":
						{
							var (shp_file, read_shp_msg) = ReadShapefileFromFile(shape_fname);

							if(shp_file is not null)
							{
								var shp_name = "shp" + (++ShapesN);

								DrawShapefile(shp_name, shp_file);

								ShowShapefileLog(shp_name, shp_file, read_shp_msg);
							}
							else
								ShowLog($"reading SHP error : {read_shp_msg}\r\n");

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
							ShowLog($"unknown shape file type : {shape_fname}\r\n");
							break;
					
					}
				}
				catch(Exception ex)
				{
					ShowLog(ex.Message + "\r\n");
				}

			}

StopWatch.Lap("after  load shapes");
MemWatch .Lap("after  load shapes");
StopWatch.Stop();
MemWatch .Stop();

			if(Cfg.ToShowDebugInfo)
			{
				ShowLog(StopWatch.ResultLog);
				ShowLog(MemWatch .ResultLog);
				ShowLog("\r\n");
			}
		}
	}
}
