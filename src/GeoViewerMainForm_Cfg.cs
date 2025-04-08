//
// GeoViewerMainForm_Cfg.cs
// 地形ビューア - 設定ファイル
//
// ◆本来は別にする必要はないかもしれないが、現状はLgLtとWPとTileに分かれており、同じものを書いているので纏める。
// ◆XMLやハードコード等のバリエーションがあるので依存しないように別にする。
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		//--------------------------------------------------
		// 地図データフォルダ等設定

		public readonly CCfg Cfg = new ();

		//--------------------------------------------------
		// 個別設定
		// ◆TreeCounterのようにCfgとしてまとめるか。

		public string Title;

		public CLgLt StartLgLt_0;
		public CLgLt EndLgLt_0;

		// ◆一応初期値を与えておくが。
		public int MeshZoomLevel  = 14;
		public int ImageZoomLevel = 14;
		public int DEMZoomLevel	  = 14;

		//---------------------------------------------------------------------------

		void ReadCfgFromFile(in string cfg_fname)
		{
			Cfg.ReadFromFile(cfg_fname);

			if(Cfg.ToSelectMapCfg)
			{
				// 位置設定ファイル選択モード

				var of_dialog = new OpenFileDialog()
					{ Title  = "地域設定ファイルを開く",
					  Filter = "地域設定ファイル|*.cfg.xml",
					  Multiselect = false };

				// ◆ファイルを選択しなかったら終了する。
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ◆終了しない。
					Close();

				var fname = of_dialog.FileName;

				// ◆再起呼び出しだが大丈夫か？
				ReadCfgFromFile(fname);

				of_dialog.Dispose();
			}
			else if(Cfg.ToSelectLAS)
			{
				// 点群ファイル選択モード

				var of_dialog = new OpenFileDialog()
					{ Title  = "点群ファイルを開く",
					  Filter = "点群ファイル|*.las;*.laz;*.csv;*.txt",
					  Multiselect = true }; // ◆複数選択実装中

				// ◆ファイルを選択しなかったら終了する。
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ◆終了しない。
					Close();

				var fnames = of_dialog.FileNames;

				Title =
				Cfg.LASFile = fnames[0]; // ◆ここは1個しか設定できないが、どこで使用している？

				if(fnames.Length > 1) Title += " 他";

				foreach(var fname in fnames)
				{
					// ◆範囲指定の場合は点群を読まないと地図範囲が決まらないが、地図範囲を決めるためにここ(Cfg)で読むのもおかしいので、地図範囲は全領域とする。
					SetLgLtAreaFromLASFile(fname);

					LASFnames.Add(fname);
				}

				of_dialog.Dispose();
			}
			else if(Cfg.ToSelectShape)
			{
				// 図形ファイル選択モード
				// ◆ここではファイル名のみを読むようにして、読み込んで描画するのは別の場所で非同期にしろ。
				// ◆コマンドではKML等も読める。整理しろ。

				var of_dialog = new OpenFileDialog()
					{ Title  = "図形ファイルを開く",
					  Filter = "図形ファイル|*.shp",
					  Multiselect = false };

				// ◆ファイルを選択しなかったら終了する。
				if(of_dialog.ShowDialog() == DialogResult.Cancel)
				//	Application.Exit(); // ◆終了しない。
					Close();

				var fname = of_dialog.FileName;

				Title = fname;

StopWatch.Lap("before ReadShapefileFromFiles");
MemWatch .Lap("before ReadShapefileFromFiles");
				(Shapefile, ReadShapefileMsg) = ReadShapefileFromFile(fname);
StopWatch.Lap("after  ReadShapefileFromFile");
MemWatch .Lap("after  ReadShapefileFromFile");

				of_dialog.Dispose();
			}
			else
			{
				Title = Cfg.Title;

				StartLgLt_0 = Cfg.StartLgLt_0;
				EndLgLt_0   = Cfg.EndLgLt_0;

				// ◆この設定はここでよいか？
				Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[Cfg.DefaultOrigin];
			}

			MeshZoomLevel  = Cfg.MeshZoomLevel;
			ImageZoomLevel = Cfg.ImageZoomLevel;
			DEMZoomLevel   = Cfg.DEMZoomLevel;
		}

		//---------------------------------------------------------------------------

		void ReadCfgFromParam(in string param)
		{
			// ◆paramsは予約語	
			string[] prms = param.Split(" ");

			// ◆とりあえず。
			Title = "DENKOKU-ROBO2"; 

			StartLgLt_0.Set(new CLg(ToDouble(prms[0])), new CLt(ToDouble(prms[1])));
			EndLgLt_0  .Set(new CLg(ToDouble(prms[2])), new CLt(ToDouble(prms[3])));

			MeshZoomLevel  = ToInt32(prms[4]);
			ImageZoomLevel = ToInt32(prms[5]);
		}
	}
}
