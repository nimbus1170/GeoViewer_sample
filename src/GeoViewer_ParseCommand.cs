//
// PlanViewerMainForm_ParseCommand.cs
//
//---------------------------------------------------------------------------
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	private string ParseCommand(in string cmd_line)
	{
		if(Viewer == null) return "no viewer";
			
		var cmd_lines = cmd_line.Split(' ');

		if(cmd_lines.Length == 0) return "no command line";
		
		var cmd		= cmd_lines[0];
		var options = cmd_lines[1..];

		// ◆複数行の文字列が返ることもあるので改行も含める。
		string ret = "";

		// ◆ヘルプと処理を一緒に書けないか？デリゲート(Func/Action)でコンテナに入れるとできそうだが、
		// 　複雑になる。switchだと書きやすいか？switch以外で書けないが。
		switch(cmd)
		{
			case "showshapes":
	
				if(options.Length == 0)
					Viewer.ShowShapes();
				else
					foreach(var shape_name in options)
						Viewer.ShowShapes(shape_name);

				Viewer.DrawScene();
				
				break;

			case "hideshapes":

				if(options.Length == 0)
					Viewer.HideShapes();
				else
					foreach(var shape_name in options)
						Viewer.HideShapes(shape_name);

				Viewer.DrawScene();
				
				break;

			case "showoverlays":
	
				if(options.Length == 0)
					Viewer.ShowOverlays();
				else
					foreach(var ol_name in options)
						Viewer.ShowOverlays(ol_name);

				Viewer.DrawScene();
				
				break;

			case "hideoverlays":

				if(options.Length == 0)
					Viewer.HideOverlays();
				else
					foreach(var ol_name in options)
						Viewer.HideOverlays(ol_name);

				Viewer.DrawScene();
				
				break;

			case "countobj":

				var gl_objs_count = Viewer.GLObjectCount();	

				foreach(var gl_objs_count_i in gl_objs_count)
					ret	+= $"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value:#,0}\r\n";
	
				break;

			case "reloadshapes":

				if(DrawingFileName == null) break;
				
				Viewer.DeleteShapes();

				DrawShapesXML();

				Viewer.DrawScene();

				break;

			case "help":
				break;

			default:
				ret = "unknown command\r\n";
				break;
		}

		return ret;
	}
}
//---------------------------------------------------------------------------
}
