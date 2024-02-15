using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrepareStella.Scripts.Preparing
{
	internal static class DownloadFpsUnlockerCfg
	{
		private const string JsonFileUrl = "https://cdn.sefinek.net/resources/v3/genshin-stella-mod/unlocker.config.json";

		public static async Task RunAsync()
		{
			try
			{
				string unlockerFolderPath = Path.Combine(Start.AppPath, "data", "unlocker");
				string fpsUnlockerConfigPath = Path.Combine(unlockerFolderPath, "unlocker.config.json");

				Console.Write($@"Downloading {Path.GetFileName(fpsUnlockerConfigPath)} ");

				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("User-Agent", Start.UserAgent);

					using (HttpResponseMessage headResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, JsonFileUrl)))
					{
						if (headResponse.IsSuccessStatusCode)
						{
							long? contentLength = headResponse.Content.Headers.ContentLength;
							Console.WriteLine($@"({contentLength} bytes)...");
						}
						else
						{
							Console.WriteLine(@"(Unknown file size)...");
						}
					}

					string fpsUnlockerConfig = await httpClient.GetStringAsync(JsonFileUrl);
					string fpsUnlockerConfigContent = fpsUnlockerConfig.Replace("{GamePath}", Program.SavedGamePath?.Replace("\\", "\\\\") ?? string.Empty);

					await WriteToFileAsync(fpsUnlockerConfigPath, fpsUnlockerConfigContent);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static async Task WriteToFileAsync(string filePath, string content)
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				await writer.WriteAsync(content);
			}
		}
	}
}
