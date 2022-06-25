//
// PlanViewerMainForm_ParseCommand.cs
//
//---------------------------------------------------------------------------
using System;
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
		
		// ◆複数行の文字列が返ることもあるので改行も含める。
		string ret = "";

		switch(cmd_lines[0])
		{
			case "showshapes":
	
				if(cmd_lines.Length == 1)
					Viewer.ShowShapes();
				else
					for(var names_i = 1; names_i < cmd_lines.Length; ++names_i)
						Viewer.ShowShapes(cmd_lines[names_i]);

				Viewer.DrawScene();
				
				break;

			case "hideshapes":

				if(cmd_lines.Length == 1)
					Viewer.HideShapes();
				else
					for(var names_i = 1; names_i < cmd_lines.Length; ++names_i)
						Viewer.HideShapes(cmd_lines[names_i]);

				Viewer.DrawScene();
				
				break;

			case "showoverlays":
	
				if(cmd_lines.Length == 1)
					Viewer.ShowOverlays();
				else
					for(var names_i = 1; names_i < cmd_lines.Length; ++names_i)
						Viewer.ShowOverlays(cmd_lines[names_i]);

				Viewer.DrawScene();
				
				break;

			case "hideoverlays":

				if(cmd_lines.Length == 1)
					Viewer.HideOverlays();
				else
					for(var names_i = 1; names_i < cmd_lines.Length; ++names_i)
						Viewer.HideOverlays(cmd_lines[names_i]);

				Viewer.DrawScene();
				
				break;

			case "countobj":

				var gl_objs_count = Viewer.GLObjectCount();	

				foreach(var gl_objs_count_i in gl_objs_count)
					ret	+= $"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value}\r\n";
	
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
