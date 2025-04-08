//
// PlanViewerMainForm_ParseCommand.cs
//
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		int ShapesN = 0;

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

					Viewer.Draw();
				
					break;

				case "hideshapes":

					if(options.Length == 0)
						Viewer.HideShapes();
					else
						foreach(var shape_name in options)
							Viewer.HideShapes(shape_name);

					Viewer.Draw();
				
					break;

				case "showlayers":
	
					if(options.Length == 0)
						Viewer.ShowOverlays();
					else
						foreach(var ol_name in options)
							Viewer.ShowOverlays(ol_name);

					Viewer.Draw();
				
					break;

				case "hidelayers":

					if(options.Length == 0)
						Viewer.HideOverlays();
					else
						foreach(var ol_name in options)
							Viewer.HideOverlays(ol_name);

					Viewer.Draw();
				
					break;

				case "reloadshapes":

					if(Cfg.DrawingFileName == null) break;
				
					Viewer.DeleteShapes();

					DrawShapesXML();

					Viewer.Draw();

					break;

				case "loadlas":
					LoadLAS();
					break;

				case "loadshp":
					LoadShape();
					break;

				case "listshp":

					var shape_names = Viewer.ShapeNames();

					foreach(var shape_name in shape_names)
						ret	+= $"{shape_name}\r\n";
	
					break;

				case "countobj":

					var gl_objs_count = Viewer.GLObjectCount();	

					foreach(var gl_objs_count_i in gl_objs_count)
						ret	+= $"{gl_objs_count_i.Key, -12} : {gl_objs_count_i.Value:#,0}\r\n";
	
					break;

				case "ptsize":
					
					if(options.Length == 0) break;

					Viewer.SetPointSize_m(float.Parse(options[0])).Draw();
					
					break;


				case "help":
					ShowLog
						("showshapes 図形名 … 図形を表示する。図形名を省略するとすべて表示する。\r\n" + 
						 "hideshapes 図形名 … 図形を非表示にする。図形名を省略するとすべて非表示にする。\r\n" +
						 "showlayers レイヤー名 … レイヤーを表示する。レイヤー名を省略するとすべて表示する。\r\n" +
						 "hidelayers レイヤー名 … レイヤーを非表示にする。レイヤー名を省略するとすべて非表示にする。\r\n" +
						 "loadlas … LASファイル読み込みダイアログを表示する。\r\n" +
						 "loadshp … 図形ファイル読み込みダイアログを表示する。\r\n" +
						 "listshp … 読み込まれている図形名を表示する。\r\n" +
						 "countobj … OpenGLオブジェクトの数を表示する。\r\n" +
						 "ptsize 点サイズ … 点のサイズを設定する。\r\n" +
						 "help … ヘルプを表示する。\r\n");
					break;

				default:
					ret = "unknown command\r\n";
					break;
			}

			return ret;
		}
	}
}
