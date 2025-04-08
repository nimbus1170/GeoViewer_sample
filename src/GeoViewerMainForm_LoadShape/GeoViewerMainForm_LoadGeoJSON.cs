//
// GeoViewerMainForm_LoadGeoJSON.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using static DSF_NET_Geography.DAltitudeBase;

using DSF_NET_Color;
using DSF_NET_Scene;

using System.Text.Json;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		void ReadDrawGeoJSONFromFile(in string geojson_fname)
		{
			// ローカルな設定をファイルから読み込む。
			// ◆最初に読み込みを試行し、原点定義等が存在していればそれでDefaultOrigin等を上書きする。

			var local_cfg_fname = Path.GetDirectoryName(geojson_fname) + "\\default.cfg.xml";

			if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

			var sr = new StreamReader(geojson_fname);

			string json_str = sr.ReadToEnd();
		
			var json_doc = JsonDocument.Parse(json_str);

			if(json_doc.RootElement.ValueKind != JsonValueKind.Object) throw new Exception("root must be Object");

			// ◆取り敢えず直に描画するが、加工等を考慮すると、シェープファイルのように一旦データとして読むべきか。
			// ◆FeatureCollectionがなければエラーを出すように。
			// ◆その他の情報も入っているので表示できるように。

			var name = "shape" + (++ShapesN);

			foreach(var root_child in json_doc.RootElement.EnumerateObject())
			{
				switch(root_child.Name)
				{
					case "type":
					case "name":
					case "crs": // 座標参照系情報(Coordinate Reference System)
						// ◆取り敢えず読み飛ばす。
						// →◆平面直角座標系(に関する情報)もあるのでは？
						break;

					case "features":
					{
						var features = root_child;

						if(features.Value.ValueKind != JsonValueKind.Array) throw new Exception("features must be Array");

						foreach (var feature in features.Value.EnumerateArray())
						{
							if(feature.ValueKind != JsonValueKind.Object) throw new Exception("feature must be Object");

							foreach(var feature_elem in feature.EnumerateObject())
							{
								switch(feature_elem.Name)
								{
									case "type":
									case "properties":
										// ◆取り敢えず読み飛ばす。
										break;

									case "geometry":
									{
										if(feature_elem.Value.ValueKind != JsonValueKind.Object) throw new Exception("geometry must be Object");
			
										string geom_type = "";

										foreach(var geom_elem in feature_elem.Value.EnumerateObject())
										{
											switch(geom_elem.Name)
											{
												case "type":
													geom_type = geom_elem.Value.ToString();
													break;

												case "coordinates":
												{
													if(geom_elem.Value.ValueKind != JsonValueKind.Array) throw new Exception("geometry coordinates must be Array");

													// ◆coordinatesより後で定義されていても問題ないように、一旦データに入れるか。
													if(string.IsNullOrEmpty(geom_type)) throw new Exception("geometry type must be defined prior to the coordinates definition");

													switch(geom_type)
													{
														case "Point":
														{ 
															/* "coordinates": [30, 10] */

															var pts = new CGeoPoints((float)Cfg.PointSize);

															var pt_lglt = new CLgLt();

														//	var pt_color = new CColorF{ R = 1.0f, G = 0.0f, B = 0.0f, A = 1.0f };
															var pt_color = new CColorB{ R = 255, G = 255, B = 255, A = 255 };

															// ◆一つしかないが、反復子を使うしかないのか？。
															foreach(var coord_elem in geom_elem.Value.EnumerateArray())
															{
																// ◆高度は取り敢えず。
																pts.AddPoint(pt_lglt.SetLg(coord_elem[0].GetDouble()).SetLt(coord_elem[1].GetDouble()).SetAltitude(AGL, 100), pt_color);
															}

															Viewer.AddShape(name, pts);

															break;
														}

														case "LineString":
														{ 
															/* "coordinates": [
																	[30, 10], [10, 30], [40, 40]
																] */

															var polyline = new CGeoPolyline()
																.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 1.0f))
																.SetLineWidth(2.0f);

															var pt_lglt = new CLgLt();

															foreach(var coord_elem in geom_elem.Value.EnumerateArray())
															{
																// ◆高度は取り敢えず。
																polyline.AddNode(pt_lglt.SetLg(coord_elem[0].GetDouble()).SetLt(coord_elem[1].GetDouble()).SetAltitude(AGL, 100));
															}

															Viewer.AddShape(name, polyline);

															break;
														}

														case "Polygon":
														{
															/* "coordinates": [
																	[[30, 10], [40, 40], [20, 40], [10, 20], [30, 10]],
																	[[20, 30], [35, 35], [30, 20], [20, 30]] // ◆ドーナツ状の場合は2個以上のポリゴンになる。
																] */
	
															// ◆取り敢えず、(Multi)LineStringと同じ。

															var pt_lglt = new CLgLt();

															var color = new CColorF(1.0f, 0.0f, 0.0f, 1.0f);

															foreach(var coord_elem in geom_elem.Value.EnumerateArray())
															{
																var polyline = new CGeoPolyline()
																	.SetColor(color)
																	.SetLineWidth(2.0f);

																foreach(var sub_coord_elem in coord_elem.EnumerateArray())
																{
																	// ◆高度は取り敢えず。
																	polyline.AddNode(pt_lglt.SetLg(sub_coord_elem[0].GetDouble()).SetLt(sub_coord_elem[1].GetDouble()).SetAltitude(AGL, 100));
																}

																Viewer.AddShape(name, polyline);
															}

															break;
														}	

														case "MultiPoint":
														case "MultiLineString":
														case "MultiPolygon":
															throw new Exception("multi geometry not supported");
													}

													break;
												}
											}
										}

										break;
									}
								}
							}
						}

						break;
					}
				}
			}

			Viewer.Draw();
		}

		//-----------------------------------------------------------------------
	}
	//---------------------------------------------------------------------------
}
