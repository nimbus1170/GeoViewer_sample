//
// GeoViewerMainForm_LoadShapefile.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.Convert_LgLt_XY;
using static DSF_NET_Geography.DAltitudeBase;

using DSF_NET_Color;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using DSF_CS_Geography;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		CShapefile Shapefile = null;

		string ReadShapefileMsg = "";

		//-----------------------------------------------------------------------

		(CShapefile shp_file, string msg) ReadShapefileFromFile(in string shp_fname)
		{
			// ローカルな設定をファイルから読み込む。
			// ◆最初に読み込みを試行し、原点定義等が存在していればそれでDefaultOrigin等を上書きする。

			var local_cfg_fname = Path.GetDirectoryName(shp_fname) + "\\default.cfg.xml";

			if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

			//--------------------------------------------------

			var shp_file = new CShapefile(shp_fname);

			//--------------------------------------------------

			var prj_fname = shp_fname.Replace(".shp", ".prj");

			if(File.Exists(prj_fname))
			{
				var sr = new StreamReader(prj_fname);

				string prj_wkt_txt = sr.ReadToEnd();

				var prj_wkt = new CWKT(prj_wkt_txt);

				switch(prj_wkt.Name)
				{ 
					case "PROJCS":
					{
						// 平面直角座標系
						// ◆PROJCSの有無で判断するのか？
			
						shp_file.IsXY = true;

						var origin_txt = prj_wkt.Items[0];

						// ◆WKTに経緯度でも定義されているのでは？(→ LoadLAS)
						var default_origin = 
							origin_txt.EndsWith("Japan_Zone_1" )?  1:
							origin_txt.EndsWith("Japan_Zone_2" )?  2:
							origin_txt.EndsWith("Japan_Zone_3" )?  3:
							origin_txt.EndsWith("Japan_Zone_4" )?  4:
							origin_txt.EndsWith("Japan_Zone_5" )?  5:
							origin_txt.EndsWith("Japan_Zone_6" )?  6:
							origin_txt.EndsWith("Japan_Zone_7" )?  7:
							origin_txt.EndsWith("Japan_Zone_8" )?  8:
							origin_txt.EndsWith("Japan_Zone_9" )?  9:
							origin_txt.EndsWith("Japan_Zone_10")? 10:
							origin_txt.EndsWith("Japan_Zone_11")? 11:
							origin_txt.EndsWith("Japan_Zone_12")? 12:
							origin_txt.EndsWith("Japan_Zone_13")? 13:
							origin_txt.EndsWith("Japan_Zone_14")? 14:
							origin_txt.EndsWith("Japan_Zone_15")? 15:
							origin_txt.EndsWith("Japan_Zone_16")? 16:
							origin_txt.EndsWith("Japan_Zone_17")? 17:
							origin_txt.EndsWith("Japan_Zone_18")? 18:
							origin_txt.EndsWith("Japan_Zone_19")? 19: Cfg.DefaultOrigin;

						Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[default_origin];

						// ◆シェープファイルはXが東西のようだ。
						var min_lglt = ToLgLt(new CCoord(shp_file.MinBound[1], shp_file.MinBound[0]), AE);
						var max_lglt = ToLgLt(new CCoord(shp_file.MaxBound[1], shp_file.MaxBound[0]), AE);

						StartLgLt_0 = new CLgLt(new CLg(min_lglt.Lg.DecimalDeg - Cfg.LgLtMargin), new CLt(min_lglt.Lt.DecimalDeg - Cfg.LgLtMargin));
						EndLgLt_0   = new CLgLt(new CLg(max_lglt.Lg.DecimalDeg + Cfg.LgLtMargin), new CLt(max_lglt.Lt.DecimalDeg + Cfg.LgLtMargin));

						break;
					}
	
					case "GEOGCS":
					{ 
						// 経緯度座標系
						// ◆GEOGCSの有無で判断するのか？

						StartLgLt_0 = new CLgLt(new CLg(shp_file.MinBound[0] - Cfg.LgLtMargin), new CLt(shp_file.MinBound[1] - Cfg.LgLtMargin));
						EndLgLt_0   = new CLgLt(new CLg(shp_file.MaxBound[0] + Cfg.LgLtMargin), new CLt(shp_file.MaxBound[1] + Cfg.LgLtMargin));

						break;
					}

					default:
						break;
				}
			}
			else
			{
				// ◆prjファイルが無かったら経緯度座標系とする。
				StartLgLt_0 = new CLgLt(new CLg(shp_file.MinBound[0] - Cfg.LgLtMargin), new CLt(shp_file.MinBound[1] - Cfg.LgLtMargin));
				EndLgLt_0   = new CLgLt(new CLg(shp_file.MaxBound[0] + Cfg.LgLtMargin), new CLt(shp_file.MaxBound[1] + Cfg.LgLtMargin));
			}

			//--------------------------------------------------

			return (shp_file, "");
		}

		//-----------------------------------------------------------------------

		// ◆取り敢えず16色ほど準備しておく。

		readonly Color[] colors =
		[
			Color.Red,
			Color.Blue,
			Color.DarkGreen,
			Color.Yellow,
			Color.Cyan,
			Color.Magenta,
			Color.Orange,
			Color.Purple,
			Color.Brown,
			Color.Pink,
			Color.Green,
			Color.LightBlue,
			Color.LightYellow,
			Color.LightCyan,
			Color.LightGray,
			Color.LightPink,
		];

		int curr_color_i = 0;

		Color GetNextColor()
		{
			return colors[curr_color_i++ % colors.Length];
		}

		readonly CColorF[] colorfs =
		[
			new (1.0f, 0.0f, 0.0f, 1.0f),
			new (0.0f, 0.0f, 1.0f, 1.0f),
			new (0.0f, 0.5f, 0.0f, 1.0f),
			new (1.0f, 1.0f, 0.0f, 1.0f),
			new (0.0f, 1.0f, 1.0f, 1.0f),
			new (1.0f, 0.0f, 1.0f, 1.0f),
			new (1.0f, 0.5f, 0.0f, 1.0f),
			new (0.5f, 0.0f, 1.0f, 1.0f),
			new (0.5f, 0.5f, 0.5f, 1.0f),
			new (1.0f, 0.5f, 1.0f, 1.0f),
			new (0.5f, 1.0f, 0.5f, 1.0f),
			new (0.5f, 0.5f, 1.0f, 1.0f),
			new (1.0f, 1.0f, 0.5f, 1.0f),
			new (0.5f, 1.0f, 1.0f, 1.0f),
			new (1.0f, 0.5f, 0.5f, 1.0f),
			new (0.5f, 0.5f, 0.5f, 1.0f),
		];

		int curr_colorf_i = 0;

		CColorF GetNextColorF()
		{
			return colorfs[curr_colorf_i++ % colorfs.Length];
		}

		void DrawShapefile(in string name, in CShapefile shp_file)
		{
			if(Cfg.ToCheckDataOnly) return;

			switch(shp_file.ShapeType)
			{
				case "NullShape": ShowLog("NullShape\r\n" ); return;

				case "Point" :
				case "PointZ": // ◆取り急ぎZ値は無視、ではない。
				case "PointM": // ◆取り急ぎM値は無視
				{
					var dst_pts = new CGeoPoints((float)Cfg.PointSize);

					var pt_lglt = new CLgLt();

				//	var pt_color = GetNextColorF();
					var pt_colorf = GetNextColorF();

					var pt_color = new CColorB
						{ R = (byte)(pt_colorf.R * 255),
						  G = (byte)(pt_colorf.G * 255),
						  B = (byte)(pt_colorf.B * 255),
						  A = (byte)(pt_colorf.A * 255) };

					// ◆エンティティ単位で描画する。
					// ◆複数種のエンティティが混在することはないか？
					foreach(var entity in shp_file.Entities)
					{
						foreach(var pt in entity.Parts)
						{
							if(shp_file.IsXY)
								// ◆シェープファイルはXが東西のようだ。
								// ◆高度は取り敢えず。
								pt_lglt = ToLgLt(new CCoord(pt.Y, pt.X), AGL).SetAltitude(AGL, 100);
							else
							//	pt_lglt.SetLg(pt.X).SetLt(pt.Y).SetAltitude(AGL, 100);
								pt_lglt.SetLg(pt.X).SetLt(pt.Y).SetAltitude(AMSL, pt.Z);

							dst_pts.AddPoint(pt_lglt, pt_color);
						}
					}

					Viewer.AddShape(name, dst_pts);

					break;
				}

				case "Arc" :
				case "ArcZ": // ◆取り急ぎZ値は無視、ではない。
				case "ArcM": // ◆取り急ぎM値は無視
				{
					var pt_lglt = new CLgLt();

					var color = GetNextColorF();

					int entity_i = 0;

					// ◆エンティティ単位で描画する。
					// ◆複数種のエンティティが混在することはないか？
					foreach(var entity in shp_file.Entities)
					{
						var polyline = new CGeoPolyline()
							.SetColor(color)
							.SetLineWidth(2.0f);

						foreach(var pt in entity.Parts)
						{
							if(shp_file.IsXY)
								// ◆シェープファイルはXが東西のようだ。
								// ◆高度は取り敢えず。
								pt_lglt = ToLgLt(new CCoord(pt.Y, pt.X), AGL).SetAltitude(AGL, 100);
							else
							//	pt_lglt.SetLg(pt.X).SetLt(pt.Y).SetAltitude(AGL, 100);
								pt_lglt.SetLg(pt.X).SetLt(pt.Y).SetAltitude(AMSL, pt.Z);

							polyline.AddNode(pt_lglt);
						}

						Viewer.AddShape(name + "_" + (++entity_i), polyline);
					}

					break;
				}

				case "Polygon" :
				case "PolygonZ": // ◆取り急ぎZ値は無視、ではない。
				case "PolygonM": // ◆取り急ぎM値は無視
				{
					if(Cfg.ToDrawShapeAsTIN)
					{
						// 三角形分割する。
						// ◆このポリゴンは地表面に沿っていない。テクスチャの方なら沿ってはいるが…。

						int entity_i = 0;

						// ◆エンティティ単位でポリゴンにする。
						foreach(var entity in shp_file.Entities)
						{
							// ◆複数種のエンティティが混在することはないか？

							var src_lglts = new List<CLgLt>();

							foreach(var pt in entity.Parts)
							{
								var pt_lglt = new CLgLt();

								pt_lglt.SetLg(pt.X).SetLt(pt.Y); // ◆高度は取り敢えずなし。

								src_lglts.Add(pt_lglt);
							}

							var polygon_name = name + "_" + (++entity_i);

							try
							{ 
								var dst_lglts = CTIN.ToTriangles(src_lglts);

								var dst_tri_n = dst_lglts.Count / 3;

								int dst_lglt_i = 0;

	//							var color = new CColorF(1.0f, 0.0f, 0.0f, 0.5f);
								var color = GetNextColorF();

								for(var dst_tri_i = 0; dst_tri_i < dst_tri_n; ++dst_tri_i)
								{
									var pt1_lglt = dst_lglts[dst_lglt_i++].SetAltitude(AGL, 100.0);
									var pt2_lglt = dst_lglts[dst_lglt_i++].SetAltitude(AGL, 100.0);
									var pt3_lglt = dst_lglts[dst_lglt_i++].SetAltitude(AGL, 100.0);
					
									Viewer.AddShape(polygon_name, new CGeoLine(pt1_lglt, pt2_lglt).SetColor(color).SetLineWidth(2.0f));
									Viewer.AddShape(polygon_name, new CGeoLine(pt2_lglt, pt3_lglt).SetColor(color).SetLineWidth(2.0f));
									Viewer.AddShape(polygon_name, new CGeoLine(pt3_lglt, pt1_lglt).SetColor(color).SetLineWidth(2.0f));
								}
							}
							catch(Exception)
							{
								ShowLog($"unable to process {polygon_name}\r\n");
								return;
							}
						}
					}
					else if(Cfg.ToDrawShapeAsLayer)
					{
						// テクスチャで貼りつける。

						// ◆正しくない？というか、何？
	/*					var x_min = shp_file.MinBound[0];
						var y_min = shp_file.MinBound[1];
						var x_max = shp_file.MaxBound[0];
						var y_max = shp_file.MaxBound[1];
	*/
						double x_min = double.MaxValue;
						double y_min = double.MaxValue;
						double x_max = 0.0;
						double y_max = 0.0;

						int pt_n = 0;

						foreach(var entity in shp_file.Entities)
						{
							foreach(var pt in entity.Parts)
							{
								if(pt.X < x_min) x_min = pt.X;
								if(pt.Y < y_min) y_min = pt.Y;
								if(pt.X > x_max) x_max = pt.X;
								if(pt.Y > y_max) y_max = pt.Y;
	
								++pt_n;	
							}
						}

						var h = x_max - x_min;
						var w = y_max - y_min;

						// オーバレイのサイズの基準(短辺のサイズ)
						// ◆これで表示されるポリゴンのピクセルが決まるので、ポリゴンの範囲が広いと粗くカクカクになる。
						int ol_size = 10000;
						int ol_w = (h > w)? ol_size: (int)(ol_size * w / h);
						int ol_h = (w > h)? ol_size: (int)(ol_size * h / w);

						var ol_s_lg = new CLg(x_min); var ol_s_lt = new CLt(y_min);
						var ol_e_lg = new CLg(x_max); var ol_e_lt = new CLt(y_max);

						var pt_lglt = new CLgLt();

						int entity_i = 0;

						// ◆エンティティ単位でレイヤーにする。
						// →◆レイヤーのサイズはそれぞれのエンティティの範囲にとどめるべき。
						foreach(var entity in shp_file.Entities)
						{
							// ◆複数種のエンティティが混在することはないか？

							var pts = new PointF[entity.Parts.Count];

							int pt_i = 0;

							// ◆オーバレイの範囲は南北逆転
							var ol = ((CGeoViewer_WP)Viewer).MakeOverlay
								(ToWPInt(MeshZoomLevel, new CLgLt(ol_s_lg, ol_e_lt)),
								 ToWPInt(MeshZoomLevel, new CLgLt(ol_e_lg, ol_s_lt)),
								 ol_w, ol_h);

							foreach(var pt in entity.Parts)
							{
								pt_lglt.SetLg(pt.X).SetLt(pt.Y); // ◆オーバレイなので高度は関係ない。

								pts[pt_i++] = ol.ToPointOnOverlay(ToWP(MeshZoomLevel, pt_lglt));
							}

							var polygon = new System.Drawing.Drawing2D.GraphicsPath();

							polygon.AddPolygon(pts);

							var g = ol.GetGraphics();

	//						g.FillPath(Brushes.Red, polygon);
							g.FillPath(new SolidBrush(GetNextColor()), polygon);

							g.Dispose();

							// ◆オフセットやアルファ値は動的に変更することを予期してCOverlayのメンバにしない。
							((CGeoViewer_WP)Viewer).AddOverlay
								(name + "_" + (++entity_i),
								 ol,
								 100.0, // 地表面からの高さ
								 0.8f); // 透明度
						}
					}
					else
					{
						// ポリゴンで描画する。

						var pt_lglt = new CLgLt();

						var color = GetNextColorF();

						int entity_i = 0;

						// ◆エンティティ単位で描画する。
						// ◆複数種のエンティティが混在することはないか？
						foreach(var entity in shp_file.Entities)
						{
							// ◆ポリゴンではないのでは？しかし、現状では閉じたポリゴンでは表示できないから？
							var polyline = new CGeoPolyline()
								.SetColor(color)
								.SetLineWidth(2.0f);

							foreach(var pt in entity.Parts)
							{
								if(shp_file.IsXY)
									// ◆シェープファイルはXが東西のようだ。
									// ◆高度は取り敢えず。
									pt_lglt = ToLgLt(new CCoord(pt.Y, pt.X), AGL).SetAltitude(AGL, 100);
								else
									pt_lglt.SetLg(pt.X).SetLt(pt.Y).SetAltitude(AGL, 100);

								polyline.AddNode(pt_lglt);
							}

							Viewer.AddShape(name + "_" + (++entity_i), polyline);
						}
					}

					break;
				}

				case "MultiPoint" :
				case "MultiPointZ":
				case "MultiPointM":
				case "MultiPatch" :
					ShowLog($"未対応({shp_file.ShapeType})\r\n" );
					return;

				case "UnknownShapeType":
					ShowLog("UnknownShapeType\r\n");
					return;

				default:
					ShowLog("shape type unknown\r\n");
					return;
			}
		
	StopWatch.Lap("after  make shapes");
	MemWatch .Lap("after  make shapes");

			Viewer.Draw();

	StopWatch.Lap("after  DrawScene");
	MemWatch .Lap("after  DrawScene");
		}

		//-----------------------------------------------------------------------

		private void ShowShapefileLog(in string shp_name, in CShapefile shp_file, in string msg)
		{
			ShowLog($"[{shp_name}]\r\n");
			ShowLog($"シェープタイプ : {shp_file.ShapeType}\r\n");
			ShowLog($"エンティティ数 : {shp_file.EntitiesCount}\r\n");
			ShowLog($"  MinBound.XYZ : {shp_file.MinBound[0]:0.00000} {shp_file.MinBound[1]:0.00000} {shp_file.MinBound[2]:0.00000}\r\n");
			ShowLog($"  MaxBound.XYZ : {shp_file.MaxBound[0]:0.00000} {shp_file.MaxBound[1]:0.00000} {shp_file.MaxBound[2]:0.00000}\r\n");

	/*		var entities_i = 0;

			foreach(var entity in shp_file.Entities)
			{
				ShowLog($"  [{shp_name}_{++entities_i}]\r\n");
				ShowLog($"  エンティティ #{entities_i}\r\n");
				ShowLog($"    シェープタイプ : {entity.ShapeType}\r\n");
				ShowLog($"          パート数 : {((entity.PartsCount == 0)? 1: entity.PartsCount)}\r\n");
				ShowLog($"            頂点数 : {entity.VerticesCount}\r\n");
				ShowLog($"          Min.XYZM : {entity.XMin:0.00000} {entity.YMin:0.00000} {entity.ZMin:0.00000} {entity.MMin:0.00000}\r\n");
				ShowLog($"          Max.XYZM : {entity.XMax:0.00000} {entity.YMax:0.00000} {entity.ZMax:0.00000} {entity.MMax:0.00000}\r\n");

				var parts_i = 0;

				foreach(var part in entity.Parts)
				{
					ShowLog($"    パート #{++parts_i}\r\n");
					ShowLog($"      シェープタイプ : {part.ShapeType}\r\n");
					ShowLog($"                XYZM : {part.X:0.00000} {part.Y:0.00000} {part.Z:0.00000} {part.M:0.00000}\r\n");
				}
			}
	*/
			if(!string.IsNullOrEmpty(msg)) ShowLog(msg);

			ShowLog($"\r\n");
		}

		//-----------------------------------------------------------------------
	}
}
