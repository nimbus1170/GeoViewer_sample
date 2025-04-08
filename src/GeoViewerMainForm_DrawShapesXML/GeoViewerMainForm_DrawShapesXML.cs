//
// GeoViewer_DrawShapesXML.cs
//
//---------------------------------------------------------------------------
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		void DrawShapesXML()
		{
			if(Cfg.DrawingFileName == "") return;

			var drawing_doc = new XmlDocument();

			drawing_doc.Load(Cfg.DrawingFileName);

			var drawing_groups = drawing_doc.SelectNodes("DrawingGroups/DrawingGroup");

			// XML�t�@�C�����̐}�`�`��O���[�v�̃m�[�h�𒀎��ɓn���ď�������B
			foreach(XmlNode drawing_group in drawing_groups)
			{
				// �n�����̒�`��XML�m�[�h����ǂݍ��݁A�`�悷��B
				DrawShapesXML_MineField(drawing_group);

				// �h��w�n��XML�m�[�h����ǂݍ��݁A�`�悷��B
				DrawShapesXML_DefensivePosition(drawing_group);

				// ���Ȑw�n��XML�m�[�h����ǂݍ��݁A�`�悷��B
				DrawShapesXML_FiringPosition(drawing_group);
			}
		}
	}
}
