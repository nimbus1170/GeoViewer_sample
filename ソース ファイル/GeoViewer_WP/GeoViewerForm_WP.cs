//
//
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_MGRS_UTM;
using static DSF_NET_Geography.Convert_Tude_UTM;
using static DSF_NET_Geography.Convert_Tude_WorldPixel;

using System;
using System.Reflection.Emit;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewViewerForm_WP : PlaneViewerForm
{
	public override void DispObjInfo()
	{
		var ct = ToTude(((CGeoViewer_WP)Viewer).Center);

		double ct_lg_deg = ct.Longitude.Value;
		double ct_lt_deg = ct.Latitude .Value;

		var ct_lg_dms = new CDMS(ct_lg_deg);
		var ct_lt_dms = new CDMS(ct_lt_deg);

		var ct_utm = ToUTM(ct);

		ObjInfoLabel.Text = 
			$"“ŒŒo {ct_lg_dms.Deg:000}“x{ct_lg_dms.Min:00}•ª{ct_lg_dms.Sec:00.000}•b ({ct_lg_deg:000.00000})\n" +
			$"–kˆÜ  {ct_lt_dms.Deg:00}“x{ct_lt_dms.Min:00}•ª{ct_lt_dms.Sec:00.000}•b ( {ct_lt_deg:00.00000})\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {ct_utm.EW:00000} {ct_utm.NS:00000}\n" +
			$"UTM {ct_utm.LongitudinalZone:00}{ct_utm.LatitudinalZone:0} {GetMGRSID(ct_utm):00} {GetMGRSCoordEW(ct_utm):00000} {GetMGRSCoordNS(ct_utm):00000}\n" +
			$"•W‚ {ct.Altitude.Value(DAltitudeBase.AboveSeaLevel):0}m";
			// œ•W‚‚É‚Í”{—¦‚ªŠ|‚©‚Á‚Ä‚¢‚é‚Ì‚Å‚±‚±‚Å³‚µ‚¢’l‚É’¼‚·B
			// ¨ŸValue‚Å”{—¦‚ªŠ|‚©‚Á‚½’l‚ğ•Ô‚·‚×‚«‚Å‚Í‚È‚¢‚ªA•W‚”{—¦‚ÌŠT”O‚ª‚ ‚éˆÈãAˆêŠÑ‚µ‚Ä•W‚”{—¦‚ªŠ|‚©‚Á‚½’l‚ğ•Ô‚·•û‚ª•Ö—˜‚È‚Ì‚Å‚±‚¤‚µ‚Ä‚¢‚éB
	}
}
//---------------------------------------------------------------------------
}
