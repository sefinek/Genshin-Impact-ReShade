using System;
using System.IO;
using System.Reflection;

namespace Inject_Mods.Scripts
{
    /// <summary>
    ///     Provides logging functionality for the launcher.
    /// </summary>
    internal static class Log
    {
        private static readonly string Folder = Path.Combine(Program.AppData, "logs");
        private static readonly string OutputFile = Path.Combine(Folder, "inject.output.log");


        /// <summary>
        ///     Initializes the necessary directories for logging.
        /// </summary>
        private static void InitDirs()
        {
            if (!Directory.Exists(Program.AppData)) Directory.CreateDirectory(Program.AppData);
            if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);
        }

        /// <summary>
        ///     Logs the provided information to the output log file asynchronously.
        /// </summary>
        /// <param name="log">The log message to be written to the log file.</param>
        public static async void Output(string log)
        {
            InitDirs();

            try
            {
                using (StreamWriter sw = File.AppendText(OutputFile))
                {
                    await sw.WriteLineAsync($"[{Program.AppVersion}: {DateTime.Now}]: {log}");
                }
            }
            catch
            {
                // ...
            }
        }

        /// <summary>
        ///     Logs the provided error information to the output log file asynchronously.
        /// </summary>
        /// <param name="log">The error log message to be written to the log file.</param>
        public static async void SaveError(string log)
        {
            InitDirs();

            try
            {
                using (StreamWriter sw = File.AppendText(OutputFile))
                {
                    await sw.WriteLineAsync($"[{Program.AppVersion}: {DateTime.Now}]: SaveError() • {Assembly.GetExecutingAssembly().GetName().Name}\n{log}\n");
                }
            }
            catch
            {
                // ...
            }
        }

        /// <summary>
        ///     Logs the provided exception and shows an error dialog.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        public static void ThrowError(Exception ex)
        {
            Console.WriteLine($"{ex.Message}\n");

            SaveError(ex.ToString());
        }

        /// <summary>
        ///     Logs the provided exception, sends telemetry data, shows an error dialog, and exits the application.
        /// </summary>
        /// <param name="ex">The exception to be logged and reported.</param>
        public static void ErrorAndExit(Exception ex)
        {
            // Log the exception and show the error dialog
            ThrowError(ex);

            // Exit the application with a specific exit code
            Environment.Exit(999991000);
        }

        /// <summary>
        ///     Helper method to display a toast notification.
        /// </summary>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="message">The message of the toast notification.</param>
        private static void ShowToastNotification(string title, string message)
        {
            try
            {
                // new ToastContentBuilder()
                //    .AddText(title)
                //    .AddText(message)
                //    .Show();
            }
            catch (Exception ex)
            {
                ErrorAndExit(ex);
            }
        }
    }
}