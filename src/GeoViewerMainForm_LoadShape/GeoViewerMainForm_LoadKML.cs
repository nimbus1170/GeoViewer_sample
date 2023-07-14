//
// GeoViewerMainForm_LoadKML.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using DSF_NET_Color;
using DSF_NET_Scene;

using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

using System.Runtime.Versioning;

using static DSF_CS_Profiler.CProfilerLog;

using static DSF_NET_Geography.DAltitudeBase;
using System.IO;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	//-----------------------------------------------------------------------

	public class CIconStyle
	{
		public IconStyle.IconLink? Icon;
		public double? Scale;
	}

	public class CLabelStyle
	{
		public Color32? Color;
		public double? Scale;
	}

	public class CLineStyle
	{
		public Color32? Color;
		public double? Width;
	}

	public class CPolyStyle
	{
		public Color32? Color;
		public bool? Outline;
	}

	public class CStyle
	{
		public CIconStyle  Icon	   = new ();
		public CLabelStyle Label   = new ();
		public CLineStyle  Line	   = new ();
		public CPolyStyle  Polygon = new ();
	}

	//-----------------------------------------------------------------------

	void ReadDrawKMLFromFile(in string kml_fname)
	{
		// ローカルな設定をファイルから読み込む。
		// ◆最初に読み込みを試行し、原点定義等が存在していればそれでDefaultOrigin等を上書きする。

		var local_cfg_fname = Path.GetDirectoryName(kml_fname) + "\\GeoViewerCfg.local.xml";

		if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

		var sr = new StreamReader(kml_fname);

		string json_str = sr.ReadToEnd();
		
		// ◆取り敢えず直に描画するが、加工等を考慮すると、シェープファイルのように一旦データとして読むべきか。
		// ◆FeatureCollectionがなければエラーを出すように。

		var name = "shape" + (++ShapesN);

		FileStream stream = File.Open(kml_fname, FileMode.Open);

		KmlFile? kml_file = KmlFile.Load(stream);
			
		Kml? kml = kml_file.Root as Kml;

		if(kml == null) throw new Exception("not a KML file");
			
		var placemarks = new List<Placemark>();

		ExtractPlacemarks(kml.Feature, placemarks);

		foreach(var placemark in placemarks)
		{
			Console.WriteLine(placemark.Name);

			var style_selector = kml_file.FindStyle(placemark.StyleUrl.ToString().Substring(1));

			var style = new CStyle();

			foreach(var raw_style in style_selector.Flatten())
			{
				switch(raw_style.GetType().Name)
				{
					case "IconStyle":
						style.Icon.Icon  = ((IconStyle?)raw_style)?.Icon;
						style.Icon.Scale = ((IconStyle?)raw_style)?.Scale;
						break;

					case "LabelStyle":
						style.Label.Color = ((LabelStyle?)raw_style)?.Color;
						style.Label.Scale = ((LabelStyle?)raw_style)?.Scale;
						break;

					case "LineStyle":
						style.Line.Color = ((LineStyle?)raw_style)?.Color;
						style.Line.Width = ((LineStyle?)raw_style)?.Width;
						break;

					case "PolygonStyle":
						style.Polygon.Color   = ((PolygonStyle?)raw_style)?.Color;
						style.Polygon.Outline = ((PolygonStyle?)raw_style)?.Outline;
						break;
				}
			}

			switch(placemark.Geometry)
			{
				case SharpKml.Dom.Point:
					break;//throw new Exception("not supported");

				case MultipleGeometry:
				{ 
					foreach(var mult_geom_i in placemark.Geometry.Flatten())
					{ 
						var mult_geom_name = mult_geom_i.GetType().Name;

						switch(mult_geom_name)
						{
							case "LineString":
							{ 
								foreach(var geom_i in mult_geom_i.Flatten())
								{
									var geom_name = geom_i.GetType().Name;

									Console.WriteLine(geom_name);

									if(geom_name == "CoordinateCollection")
									{
										foreach(var coord_i in geom_i.Flatten())
										{
											var coords = coord_i as CoordinateCollection;
										
											foreach(var coord in coords)
											{
												Console.WriteLine
													(coord.Longitude + " " +
													 coord.Latitude  + " " +
													 coord.Altitude);
											}
										}
									}
								}

								break;
							}
						}
					}

					break;
				}

				default:
					break;//throw new Exception("not supported");
			}
		}

		Viewer.DrawScene();
	}

	//-----------------------------------------------------------------------

	private static void ExtractPlacemarks(Feature feature, List<Placemark> placemarks)
	{
		if(feature is Placemark placemark)
		{
			placemarks.Add(placemark);
		}
		else if(feature is Container container)
		{
			foreach (Feature f in container.Features)
			{
				ExtractPlacemarks(f, placemarks);
			}
		}
	}

	//-----------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
