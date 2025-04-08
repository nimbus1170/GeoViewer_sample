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
			// ���[�J���Ȑݒ���t�@�C������ǂݍ��ށB
			// ���ŏ��ɓǂݍ��݂����s���A���_��`�������݂��Ă���΂����DefaultOrigin�����㏑������B

			var local_cfg_fname = Path.GetDirectoryName(geojson_fname) + "\\default.cfg.xml";

			if(File.Exists(local_cfg_fname)) ReadCfgFromFile(local_cfg_fname);

			var sr = new StreamReader(geojson_fname);

			string json_str = sr.ReadToEnd();
		
			var json_doc = JsonDocument.Parse(json_str);

			if(json_doc.RootElement.ValueKind != JsonValueKind.Object) throw new Exception("root must be Object");

			// ����芸�������ɕ`�悷�邪�A���H�����l������ƁA�V�F�[�v�t�@�C���̂悤�Ɉ�U�f�[�^�Ƃ��ēǂނׂ����B
			// ��FeatureCollection���Ȃ���΃G���[���o���悤�ɁB
			// �����̑��̏��������Ă���̂ŕ\���ł���悤�ɁB

			var name = "shape" + (++ShapesN);

			foreach(var root_child in json_doc.RootElement.EnumerateObject())
			{
				switch(root_child.Name)
				{
					case "type":
					case "name":
					case "crs": // ���W�Q�ƌn���(Coordinate Reference System)
						// ����芸�����ǂݔ�΂��B
						// �������ʒ��p���W�n(�Ɋւ�����)������̂ł́H
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
										// ����芸�����ǂݔ�΂��B
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

													// ��coordinates����Œ�`����Ă��Ă����Ȃ��悤�ɁA��U�f�[�^�ɓ���邩�B
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

															// ��������Ȃ����A�����q���g�������Ȃ��̂��H�B
															foreach(var coord_elem in geom_elem.Value.EnumerateArray())
															{
																// �����x�͎�芸�����B
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
																// �����x�͎�芸�����B
																polyline.AddNode(pt_lglt.SetLg(coord_elem[0].GetDouble()).SetLt(coord_elem[1].GetDouble()).SetAltitude(AGL, 100));
															}

															Viewer.AddShape(name, polyline);

															break;
														}

														case "Polygon":
														{
															/* "coordinates": [
																	[[30, 10], [40, 40], [20, 40], [10, 20], [30, 10]],
																	[[20, 30], [35, 35], [30, 20], [20, 30]] // ���h�[�i�c��̏ꍇ��2�ȏ�̃|���S���ɂȂ�B
																] */
	
															// ����芸�����A(Multi)LineString�Ɠ����B

															var pt_lglt = new CLgLt();

															var color = new CColorF(1.0f, 0.0f, 0.0f, 1.0f);

															foreach(var coord_elem in geom_elem.Value.EnumerateArray())
															{
																var polyline = new CGeoPolyline()
																	.SetColor(color)
																	.SetLineWidth(2.0f);

																foreach(var sub_coord_elem in coord_elem.EnumerateArray())
																{
																	// �����x�͎�芸�����B
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
