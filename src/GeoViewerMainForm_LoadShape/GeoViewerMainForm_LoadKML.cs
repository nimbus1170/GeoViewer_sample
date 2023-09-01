//
// GeoViewerMainForm_LoadKML.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using DSF_NET_Color;
using DSF_NET_Scene;

using System.Runtime.Versioning;

//using static DSF_CS_Profiler.CProfilerLog;

using static DSF_NET_Color.CColor;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.DAltitudeBase;

using System.Drawing.Drawing2D;
using System.Xml;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	//-----------------------------------------------------------------------

	public class CIconStyle
	{
		public string Icon;
		public double Scale;
	}

	public class CLabelStyle
	{
		public Color  Color;
		public double Scale;
	}

	public class CLineStyle
	{
		public Color  Color;
		public double Width;
	}

	public class CPolyStyle
	{
		public Color  Color;
		public bool	  Outline;
	}

	public class CStyle
	{
		public CIconStyle  IconStyle ;
		public CLabelStyle LabelStyle;
		public CLineStyle  LineStyle ;
		public CPolyStyle  PolyStyle ;
	}

	readonly Dictionary<string, CStyle> Styles = new ();

	//-----------------------------------------------------------------------

	public class CKMLCoord
	{
		public double Longitude;
		public double Latitude ;
		public double Altitude  = 0.0;
	}

	static List<CKMLCoord> ToCoordList(in string coords_str)
	{
		var coord_list = new List<CKMLCoord>();

		var coord_elem = coords_str.Trim().Split(' ');

		foreach(var coord in coord_elem)
		{
			var coord_elems	= coord.Split(',');
			
			coord_list.Add(new CKMLCoord()
				{ Longitude = ToDouble(coord_elems[0]),
				  Latitude  = ToDouble(coord_elems[1]),
				  Altitude  = (coord_elems.Length == 3)? ToDouble(coord_elems[2]): 0.0});
		}

		return coord_list;
	}

	//-----------------------------------------------------------------------

	[SupportedOSPlatform("windows")]
	void ReadDrawKMLFromFile(in string kml_fname)
	{
		//--------------------------------------------------
		// ローカルな設定をファイルから読み込む。
		// ◆最初に読み込みを試行し、原点定義等が存在していればそれでDefaultOrigin等を上書きする。

		var local_cfg_fname = Path.GetDirectoryName(kml_fname) + "\\GeoViewerCfg.local.xml";

		if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

		//--------------------------------------------------
		// KMLを読み込む。
		// https://developers.google.com/kml/documentation/kmlreference?hl=ja

		var name = "shp" + (++ShapesN);

		var kml_doc = new XmlDocument();

		kml_doc.Load(kml_fname);

		var xml_root = kml_doc.DocumentElement;

		if((xml_root == null) || (xml_root.Name != "kml")) throw new Exception("not a kml document");

		//--------------------------------------------------

		var kml_styles = kml_doc.GetElementsByTagName("Style");

		foreach(XmlElement kml_style in kml_styles)
		{
			var id = kml_style.GetAttribute("id");

			var style = new CStyle();

			//--------------------------------------------------

			var kml_icon_styles = kml_style.GetElementsByTagName("IconStyle");

			foreach(XmlElement kml_icon_style in kml_icon_styles)
			{
				var icon	  = kml_icon_style.GetElementsByTagName("Icon" )[0]?.InnerText;
				var scale_txt = kml_icon_style.GetElementsByTagName("scale")[0]?.InnerText;

				style.IconStyle = new CIconStyle(){ Icon = icon, Scale = ToDouble(scale_txt) };
			}

			//--------------------------------------------------

			var kml_label_styles = kml_style.GetElementsByTagName("LabelStyle");

			foreach(XmlElement kml_label_style in kml_label_styles)
			{
				var abgr_txt  = kml_label_style.GetElementsByTagName("color")[0]?.InnerText;
				var scale_txt = kml_label_style.GetElementsByTagName("scale")[0]?.InnerText;

				style.LabelStyle = new CLabelStyle(){ Color = FromABGR(ToInt32(abgr_txt, 16)), Scale = ToDouble(scale_txt) }; 
			}

			//--------------------------------------------------

			var kml_line_styles = kml_style.GetElementsByTagName("LineStyle");

			foreach(XmlElement kml_line_style in kml_line_styles)
			{
				var abgr_txt  = kml_line_style.GetElementsByTagName("color")[0]?.InnerText;
				var width_txt = kml_line_style.GetElementsByTagName("width")[0]?.InnerText;

				style.LineStyle = new CLineStyle(){ Color = FromABGR(ToInt32(abgr_txt, 16)), Width = ToDouble(width_txt) };
			}

			//--------------------------------------------------

			var kml_poly_styles = kml_style.GetElementsByTagName("PolyStyle");

			foreach(XmlElement kml_poly_style in kml_poly_styles)
			{
				var abgr_txt	= kml_poly_style.GetElementsByTagName("color"  )[0]?.InnerText;
				var outline_txt	= kml_poly_style.GetElementsByTagName("outline")[0]?.InnerText;

				style.PolyStyle = new CPolyStyle(){ Color = FromABGR(ToInt32(abgr_txt, 16)), Outline = (outline_txt == "1")? true: false };
			}

			//--------------------------------------------------

			Styles.Add(id, style);
		}

		//--------------------------------------------------

		var points_n   = 0;
		var lines_n	   = 0;
		var polygons_n = 0;


		// ◆これで良いが、SelectSingleNodde("kml")とかで読めない。
		var folders = kml_doc.GetElementsByTagName("Folder");

		foreach(XmlElement folder in folders)
		{
			var placemarks = folder.GetElementsByTagName("Placemark");

			foreach(XmlElement placemark in placemarks)
			{
				// ◆placemark単位で扱えると良いが、速度を稼ぐため保留する。
//				var name = name0 + "_" + placemark.Attributes["id"]?.Value;

				//--------------------------------------------------

				var style_url = placemark.GetElementsByTagName("styleUrl")[0]?.InnerText;

				var style = Styles[style_url[1..]];

				//--------------------------------------------------
				// ◆placemark.GetElementsByTagNameでも、MultiGeometryの中まで検索してしまうので、どちらかにする。
				// →◆MultiGeometryとそうでないものが混在しないか？何とかならないか？

				var kml_geoms = placemark.GetElementsByTagName("MultiGeometry");

				if(kml_geoms.Count != 0)
				{
					foreach(XmlElement kml_geom in kml_geoms)
					{
						points_n   += DrawPoints	 (name, kml_geom.GetElementsByTagName("Point"	  ), style);
						lines_n	   += DrawLineStrings(name, kml_geom.GetElementsByTagName("LineString"), style);
						polygons_n += DrawPolygons   (name, kml_geom.GetElementsByTagName("Polygon"	  ), style);
					}
				}
				else
				{
					points_n   += DrawPoints	 (name, placemark.GetElementsByTagName("Point"	   ), style);
					lines_n	   += DrawLineStrings(name, placemark.GetElementsByTagName("LineString"), style);
					polygons_n += DrawPolygons   (name, placemark.GetElementsByTagName("Polygon"   ), style);
				}

				//--------------------------------------------------

				Viewer.DrawScene();
			}
		}
		
		//--------------------------------------------------

		DialogTextBox.AppendText($"{name}\r\n");
		DialogTextBox.AppendText($"  ポイント数 : {points_n  }\r\n");
		DialogTextBox.AppendText($"    ライン数 : {lines_n   }\r\n");
		DialogTextBox.AppendText($"  ポリゴン数 : {polygons_n}\r\n");
	}

	//-----------------------------------------------------------------------
	// Point

	[SupportedOSPlatform("windows")]
	int DrawPoints(in string name, in XmlNodeList kml_points, in CStyle style)
	{
		if(kml_points.Count == 0) return 0;

StopWatch.Lap("before DrawPoints");

		var pts = new CGeoPoints(PointSize);

		var pt_lglt = new CLgLt();

		// ◆KMLではアイコンで示されるが、取り敢えず。
		var pt_color = new CColorF{ R = 1.0f, G = 0.0f, B = 0.0f, A = 1.0f };

		foreach(XmlElement kml_point in kml_points)
		{
			var kml_coords = kml_point.GetElementsByTagName("coordinates");

			foreach(XmlElement kml_coord in kml_coords)
			{
				var coords = ToCoordList(kml_coord.InnerText);

				foreach(var coord in coords)
				{ 
					// ◆Icon(画像)を反映しろ。PointSpriteか。
					// ◆高度は取り敢えず。
					pts.AddPoint(pt_lglt.SetLg(coord.Longitude).SetLt(coord.Latitude).SetAltitude(AGL, 150), pt_color);
				}
			}
		}

		Viewer.AddShape(name, pts);

StopWatch.Lap("after  DrawPoints");

		return kml_points.Count;
	}

	//-----------------------------------------------------------------------
	// LineString

	[SupportedOSPlatform("windows")]
	int DrawLineStrings(in string name, in XmlNodeList kml_lines, in CStyle style)
	{
		if(kml_lines.Count == 0) return 0;

StopWatch.Lap("before DrawLineStrings");

		var node_lglt = new CLgLt();

		var color = new CColorF(style.LineStyle.Color);

		var line_w = (float)style.LineStyle.Width;

		foreach(XmlElement kml_line in kml_lines)
		{
			var polyline = new CGeoPolyline()
				.SetColor(color)
				.SetLineWidth(line_w);

			var kml_coords = kml_line.GetElementsByTagName("coordinates");

			foreach(XmlElement kml_coord in kml_coords)
			{
				var coords = ToCoordList(kml_coord.InnerText);

				foreach(var coord in coords)
				{
					// ◆高度は取り敢えず。
					polyline.AddNode(node_lglt.SetLg(coord.Longitude).SetLt(coord.Latitude).SetAltitude(AGL, 150));
				}
			}

			Viewer.AddShape(name, polyline);
		}

StopWatch.Lap("after  DrawLineStrings");

		return kml_lines.Count;
	}

	//-----------------------------------------------------------------------
	// Polygon

/* 例
<Polygon id="ID">
	<!-- specific to Polygon -->
	<extrude>0</extrude>        <!-- boolean -->
	<tessellate>0</tessellate>  <!-- boolean -->
	<altitudeMode>clampToGround</altitudeMode>
		<!-- kml:altitudeModeEnum: clampToGround, relativeToGround, or absolute -->
		<!-- or, substitute gx:altitudeMode: clampToSeaFloor, relativeToSeaFloor -->
	<outerBoundaryIs>
		<LinearRing>
			<coordinates>...</coordinates>  <!-- lon,lat[,alt] -->
		</LinearRing>
	</outerBoundaryIs>
	<innerBoundaryIs>
		<LinearRing>
			<coordinates>...</coordinates>  <!-- lon,lat[,alt] -->
		</LinearRing>
	</innerBoundaryIs>
</Polygon>
*/
	[SupportedOSPlatform("windows")]
	int DrawPolygons(in string name, in XmlNodeList kml_polygons, in CStyle style)
	{
		if(kml_polygons.Count == 0) return 0;

		// ◆取り敢えずオーバレイに描画する。
		// ◆大きなサイズでオーバレイを作成するとなぜか表示されないのでポリゴンごとにオーバレイにする。
		// →◆Placemark単位でオーバレイにすると、AddOverlayに時間がかかっているので遅いが、Placemark単位で表示非表示を切り替えられるのも良い。
		// 　→◆現状ではnameでは区別していないので個別には扱えない。それなら1枚のオーバレイへの描画を地球しろ。

StopWatch.Lap("before DrawPolygons");

		double lg_min = double.MaxValue;
		double lt_min = double.MaxValue;
		double lg_max = 0.0;
		double lt_max = 0.0;

		foreach(XmlElement kml_polygon in kml_polygons)
		{
			var kml_coords = kml_polygon.GetElementsByTagName("coordinates");

			foreach(XmlElement kml_coord in kml_coords)
			{
				var coords = ToCoordList(kml_coord.InnerText);

				foreach(var coord in coords)
				{
					if(coord.Longitude < lg_min) lg_min = coord.Longitude;
					if(coord.Latitude  < lt_min) lt_min = coord.Latitude ;
					if(coord.Longitude > lg_max) lg_max = coord.Longitude;
					if(coord.Latitude  > lt_max) lt_max = coord.Latitude ;
				}
			}
		}

StopWatch.Lap("after  set min_max");

		var h = lt_max - lt_min;
		var w = lg_max - lg_min;

		// オーバレイのサイズの基準(短辺のサイズ)
		int ol_size = 2000;
		int ol_w = (h > w)? ol_size: (int)(ol_size * w / h);
		int ol_h = (w > h)? ol_size: (int)(ol_size * h / w);

		var ol_s_lg = new CLg(lg_min); var ol_s_lt = new CLt(lt_min);
		var ol_e_lg = new CLg(lg_max); var ol_e_lt = new CLt(lt_max);

		// ◆オーバレイの範囲は南北逆転
		var ol = ((CGeoViewer_WP)Viewer).MakeOverlay
			(ToWPInt(MeshZoomLevel, new CLgLt(ol_s_lg, ol_e_lt)),
			 ToWPInt(MeshZoomLevel, new CLgLt(ol_e_lg, ol_s_lt)),
			 ol_w, ol_h);

StopWatch.Lap("after  MakeOverlay");

		foreach(XmlElement kml_polygon in kml_polygons)
		{
			//--------------------------------------------------

			var kml_outer_boundaries = kml_polygon.GetElementsByTagName("outerBoundaryIs");

			foreach(XmlElement kml_outer_boundary in kml_outer_boundaries)
				DrawLinearRings(kml_outer_boundary.GetElementsByTagName("LinearRing"), style, ol, false);

			//--------------------------------------------------

			var kml_inner_boundaries = kml_polygon.GetElementsByTagName("innerBoundaryIs");

			foreach(XmlElement kml_inner_boundary in kml_inner_boundaries)
				DrawLinearRings(kml_inner_boundary.GetElementsByTagName("LinearRing"), style, ol, true);
		}

StopWatch.Lap("after  DrawPolygons");

		// ◆オフセットやアルファ値は動的に変更することを予期してCOverlayのメンバにしない。
		((CGeoViewer_WP)Viewer).AddOverlay
			(name,
			 ol,
			 100.0, // 地表面からの高さ
			 0.8f); // 透明度→◆ポリゴンの透明度との関係は？↓
//			 style.Polygon.Color.Value.Alpha / 255.0f);

StopWatch.Lap("after  AddOverlay");

		return kml_polygons.Count;
	}

	//-----------------------------------------------------------------------
	// LinearRing

	[SupportedOSPlatform("windows")]
	void DrawLinearRings(in XmlNodeList kml_linear_rings, in CStyle style, in COverlay_WP ol, in bool is_inner_boundary)
	{
		if(kml_linear_rings.Count == 0) return;

		// ◆パスでまとめると速くならないか？1個のOuterBoundery/InnerBoundaryにLinearRingやcoordinatesは高々1個？

		var node_lglt = new CLgLt();

		// InnerBoundaryの場合は透明度を0(透明)にしてGraphicsを上書きモードにして中抜きする。
		var brush = new SolidBrush
			(Color.FromArgb
				((is_inner_boundary? 0: style.PolyStyle.Color.A),
				 style.PolyStyle.Color.R,
				 style.PolyStyle.Color.G,
				 style.PolyStyle.Color.B));

		var g = ol.GetGraphics();

		g.CompositingMode = is_inner_boundary? CompositingMode.SourceCopy: CompositingMode.SourceOver;

		foreach(XmlElement kml_linear_ring in kml_linear_rings)
		{
			var kml_coords = kml_linear_ring.GetElementsByTagName("coordinates");

			foreach(XmlElement kml_coord in kml_coords)
			{
				var coords = ToCoordList(kml_coord.InnerText);

				var nodes_ol = new PointF[coords.Count];

				int node_i = 0;

StopWatch.Lap("before ToPointOnOverlay");
				foreach(var coord in coords)
				{
					// ◆高度は取り敢えず。
					nodes_ol[node_i++] = ol.ToPointOnOverlay(ToWP(MeshZoomLevel, node_lglt.SetLg(coord.Longitude).SetLt(coord.Latitude).SetAltitude(AE, 0)));
				}
StopWatch.Lap("after  ToPointOnOverlay");

				g.FillPolygon
					(brush,
					 nodes_ol);
				 //, System.Drawing.Drawing2D.FillMode.Winding);
StopWatch.Lap("after  g.FillPolygon");
			}
		}

		g.Dispose();
	}

	//-----------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
