using System;
using System.Windows.Forms;

namespace GeoViewer_sample
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		
		{
			// ◆普通に実行しても例外は確認できないが、デバッグ時には確認できる。
 			if(args.Length < 1) throw new Exception("arguments number not enough.");

			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GeoViewerMainForm(args));
		}
	}
}
