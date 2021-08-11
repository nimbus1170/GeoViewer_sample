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
		var ct = ((CGeoViewer_Tude)Viewer).Center;

		var ct_lg_deg = ct.Longitude.Value;
		var ct_lt_deg = ct.Latitude .Value;

		var ct_lg_dms = new CDMS(ct_lg_deg);
		var ct_lt_dms = new CDMS(ct_lt_deg);

		var ct_utm = ToUTM(ct);

		ObjInfoLabel.Text = 
			$"���o {ct_lg_dms.Deg:000}�x{ct_lg_dms.Min:00}��{ct_lg_dms.Sec:00.000}�b ({ct_lg_deg:000.00000})\n" +
			$"�k��  {ct_lt_dms.Deg:00}�x{ct_lt_dms.Min:00}��{ct_lt_dms.Sec:00.000}�b ( {ct_lt_deg:00.00000})\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {GetMGRSID(ct_utm):00} {GetMGRSCoordEW(ct_utm):00000} {GetMGRSCoordNS(ct_utm):00000}\n" +
			$"�W�� {ct.Altitude.Value(DAltitudeBase.AboveSeaLevel):0.0}m";
	}
}
//---------------------------------------------------------------------------
}
