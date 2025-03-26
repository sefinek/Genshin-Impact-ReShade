using ConfigurationNC.Forms;
using ConfigurationNC.Properties;
using NLog;
using StellaUtils;
using System.Globalization;

namespace ConfigurationNC;

internal static class Program
{
	internal static Logger Logger = null!;
	private static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stella Mod Launcher");
	private static readonly IniFile Settings = new(Path.Combine(AppData, "settings.ini"));

	[STAThread]
	private static void Main()
	{
		LogManagerHelper.Initialize(Path.Combine(Window.AppPath, "NLog.config"), "Configuration window", Window.AppVersion);
		Logger = LogManagerHelper.GetLogger();

		// Set the correct language
		string currentLang = Settings.ReadString("Language", "UI");
		Logger.Info($"Loaded language from settings: {currentLang}");
		if (!Variables.SupportedLangs.Contains(currentLang))
		{
			string sysLang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
			currentLang = Array.Find(Variables.SupportedLangs, lang => lang == sysLang) ?? "en";
			Logger.Info($"System language detected: {sysLang}. Using: {currentLang}");
			Settings.WriteString("Language", "UI", currentLang);
		}
		else
		{
			Logger.Info($"Using supported language: {currentLang}");
		}

		try
		{
			CultureInfo culture = new(currentLang);
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = culture;
			Logger.Info($"Application culture set to: {culture.Name}");
		}
		catch (CultureNotFoundException)
		{
			Logger.Error($"CultureNotFoundException! Invalid language detected: {currentLang}; Falling back to English...");
			CultureInfo fallback = new("en");
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = fallback;
		}

		// Init
		ApplicationConfiguration.Initialize();
		Application.ThreadException += (_, e) => Logger.Error($"ThreadException: {e.Exception.Message}");
		AppDomain.CurrentDomain.UnhandledException += (_, e) => Logger.Error($"UnhandledException: {((Exception)e.ExceptionObject).Message}");

		try
		{
			Application.Run(new Window { Icon = Resources.cat_white_52x52 });

			Logger.Info("Application.Run(): new Window");
		}
		catch (Exception ex)
		{
			Logger.Error(ex);
			MessageBox.Show(ex.Message, Window.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
