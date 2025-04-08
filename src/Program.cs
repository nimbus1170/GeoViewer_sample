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
			// DLLにPATHを通す。
			// プロセスが終了したら環境変数は元に戻る。
			// ◆(なぜか)メインフォーム作成前に参照されるのでここで処置する。
			Environment.SetEnvironmentVariable("PATH", Path.GetFullPath("./DLL") + ";" + Environment.GetEnvironmentVariable("PATH"));

			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GeoViewerMainForm(args));
		}
	}
}
