using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StellaLauncher.Forms;
using StellaLauncher.Properties;

namespace StellaLauncher.Scripts.Download
{
    /// <summary>
    ///     Class responsible for downloading and updating the FPS unlocker config file.
    /// </summary>
    internal static class FpsUnlockerCfg
    {
        /// <summary>
        ///     Starts the process of downloading and updating the FPS unlocker config file.
        /// </summary>
        public static async Task RunAsync()
        {
            Default._status_Label.Text += $"[i] {Resources.Default_DownloadingConfigFileForFPSUnlocker}\n";

            Log.Output("Downloading config file for FPS Unlocker...");

            await StartDownload();
        }

        private static async Task StartDownload()
        {
            try
            {
                HttpResponseMessage response = await Program.SefinWebClient.GetAsync("https://cdn.sefinek.net/resources/v3/genshin-stella-mod/unlocker.config.json");
                if (response.IsSuccessStatusCode)
                {
                    Stream contentStream = await response.Content.ReadAsStreamAsync();
                    StreamReader reader = new StreamReader(contentStream);
                    string json = await reader.ReadToEndAsync();
                    contentStream.Close();

                    // Parse the JSON
                    dynamic config = JsonConvert.DeserializeObject(json);

                    // Replace the placeholder with the actual game path
                    string gamePath = await Utils.GetGame("giExe");
                    config.GamePath = gamePath;

                    // Serialize the updated JSON back to a string
                    string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);

                    // Write the updated JSON to the config file
                    File.WriteAllText(Program.FpsUnlockerCfgPath, updatedJson);

                    // Update the status label to indicate successful completion
                    Default._status_Label.Text += $"[✓] {Resources.Default_Success}\n";
                    Log.Output("Done.");
                }
                else
                {
                    Default._status_Label.Text += $"[x] Download failed: {response.ReasonPhrase}\n";
                    Log.SaveError($"Failed to download {Path.GetFileName(Program.FpsUnlockerCfgPath)} in {Path.GetDirectoryName(Program.FpsUnlockerCfgPath)}.\n\n{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Default._status_Label.Text += $"[x] {ex.Message}\n";
                Log.SaveError($"Failed to download {Path.GetFileName(Program.FpsUnlockerCfgPath)} in {Path.GetDirectoryName(Program.FpsUnlockerCfgPath)}.\n\n{ex}");
            }
        }
    }
}
