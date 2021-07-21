//
//
//
//---------------------------------------------------------------------------
using DSF_NET_SceneGraph;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class XYZPlaneViewerControllerForm : PlaneViewerControllerForm
{
	public XYZPlaneViewerControllerForm(CPlaneViewer viewer)
	: base(viewer)
	{
		ObjXLabel.Text = "X";
		ObjYLabel.Text = "Y";

		ObjXScrollBarLabel.Text = "X";
		ObjYScrollBarLabel.Text = "Y";

		ObserverXLabel.Text = "X";
		ObserverYLabel.Text = "Y";

		ObserverXScrollBarLabel.Text = "X";
		ObserverYScrollBarLabel.Text = "Y";
	}
}
//---------------------------------------------------------------------------
}
