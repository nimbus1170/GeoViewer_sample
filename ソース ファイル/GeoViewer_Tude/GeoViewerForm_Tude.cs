//
// GeoViewerForm_Tude.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_Geography.Convert_Tude_UTM;

using System;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerForm_Tude : PlaneViewerForm
{
	public override void DispObjInfo()
	{
		CTude ct = ((CGeoViewer_Tude)Viewer).Center;

		double ct_lg_deg = ct.Longitude.Value;
		double ct_lt_deg = ct.Latitude .Value;

		var ct_lg_dms = new CDMS(ct_lg_deg);
		var ct_lt_dms = new CDMS(ct_lt_deg);

		var ct_utm = ToUTM(ct);

		ObjInfoLabel.Text = 
			$"東経 {ct_lg_dms.Deg:000}度{ct_lg_dms.Min:00}分{ct_lg_dms.Sec:00.000}秒 ({ct_lg_deg:000.00000})\n" +
			$"北緯  {ct_lt_dms.Deg:00}度{ct_lt_dms.Min:00}分{ct_lt_dms.Sec:00.000}秒 ( {ct_lt_deg:00.00000})\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {GetMGRSID(ct_utm):00} {GetMGRSCoordEW(ct_utm):00000} {GetMGRSCoordNS(ct_utm):00000}\n" +
			$"標高 {ct.Altitude.Value(DAltitudeBase.AboveSeaLevel):0}m";
			// ●標高には倍率が掛かっているのでここで正しい値に直す。
			// →◆Valueで倍率が掛かった値を返すべきではないが、標高倍率の概念がある以上、一貫して標高倍率が掛かった値を返す方が便利なのでこうしている。
			// →◆標高倍率をToGeoCentricCoordで入れる方法もあるが、CCoordMakerを挟んでいるので単純ではない。後回し。
	}
}
//---------------------------------------------------------------------------
}
