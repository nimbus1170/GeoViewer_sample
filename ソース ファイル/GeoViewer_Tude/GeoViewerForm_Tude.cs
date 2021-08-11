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
			$"ìååo {ct_lg_dms.Deg:000}ìx{ct_lg_dms.Min:00}ï™{ct_lg_dms.Sec:00.000}ïb ({ct_lg_deg:000.00000})\n" +
			$"ñkà‹  {ct_lt_dms.Deg:00}ìx{ct_lt_dms.Min:00}ï™{ct_lt_dms.Sec:00.000}ïb ( {ct_lt_deg:00.00000})\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {GetMGRSID(ct_utm):00} {GetMGRSCoordEW(ct_utm):00000} {GetMGRSCoordNS(ct_utm):00000}\n" +
			$"ïWçÇ {ct.Altitude.Value(DAltitudeBase.AboveSeaLevel):0.0}m";
	}
}
//---------------------------------------------------------------------------
}
