//
//
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_Geography.Convert_LgLt_UTM;
using static DSF_NET_Geography.Convert_LgLt_WP;

using System;
using System.Reflection.Emit;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewViewerForm_Tile : PlaneViewerForm
{
	public override void DispObjInfo()
	{
		var ct = ToLgLt(((CGeoViewer_WP)Viewer).Center);

		var ct_lg_deg = ct.Lg.DecimalDeg;
		var ct_lt_deg = ct.Lt.DecimalDeg;

		var ct_lg_dms = new CDMS(ct_lg_deg);
		var ct_lt_dms = new CDMS(ct_lt_deg);

		var ct_utm = ToUTM(ct);

		ObjInfoLabel.Text = 
			$"ìååo {ct_lg_dms.Deg:000}ìx{ct_lg_dms.Min:00}ï™{ct_lg_dms.Sec:00.000}ïb ({ct_lg_deg:000.00000})\n" +
			$"ñkà‹  {ct_lt_dms.Deg:00}ìx{ct_lt_dms.Min:00}ï™{ct_lt_dms.Sec:00.000}ïb ( {ct_lt_deg:00.00000})\n" +
			$"UTM {ct_utm.LgBand:00} {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
			$"UTM {ct_utm.LgBand:00}{GetLtBand(ToLgLt(ct_utm).Lt):0} {GetMGR(ct_utm):00} {GetMGRSUTM_EW(ct_utm):00000} {GetMGRSUTM_NS(ct_utm):00000}\n" +
			$"ïWçÇ {ct.Altitude.Value(DAltitudeBase.AMSL):0.0}m";
	}
}
//---------------------------------------------------------------------------
}
