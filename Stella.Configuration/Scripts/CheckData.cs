using ConfigurationNC.Properties;
using Microsoft.Win32;
using StellaTelemetry;
using StellaUtils;

namespace ConfigurationNC.Scripts;

internal static class CheckData
{
	public static bool IsAStellaPlusSubscriber()
	{
		return TryGetRegistryValue("Secret", out string? data) && !string.IsNullOrEmpty(data);
	}

	public static bool ResourcesPath()
	{
		return TryGetRegistryValue("ResourcesPath", out string? data) && Directory.Exists(data);
	}

	private static bool TryGetRegistryValue(string keyName, out string? value)
	{
		try
		{
			using RegistryKey? key = Registry.CurrentUser.OpenSubKey(Variables.RegistryPath);
			if (key is not null)
			{
				value = key.GetValue(keyName) as string;
				bool data = !string.IsNullOrEmpty(value);
				Program.Logger.Info($"{keyName}: {data}");

				return data;
			}
		}
		catch (Exception ex)
		{
			Program.Logger.Error(ex);
			MessageBox.Show(string.Format(Resources.CheckData_SorryButSomethingWentWrong, ex.Message), Resources.CheckData_FatalError, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		value = null;
		return false;
	}
}
