using System;
using System.Diagnostics;
using System.IO;
using System.Net;

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupTool
{
    internal class Program
    {
        private static readonly TraceSource Trace = new TraceSource("Trace");

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Trace.TraceEvent(TraceEventType.Error, 0, "Expected path to a config file!");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Trace.TraceEvent(TraceEventType.Error, 0, "Unable to find config file!");
                return;
            }

            var config = Config.Parse(args[0]);

            Trace.TraceInformation("Parsed config:");
            Trace.TraceInformation(JsonConvert.SerializeObject(config, Formatting.Indented));

            // Create destination directory
            Directory.CreateDirectory(config.DestinationPath);

            Trace.TraceInformation("Tasks started!");

            // start dump and download tasks
            Task.WaitAll(new [] { GetDbDumpTask(config), GetDownloadFilesTask(config) });

            Trace.TraceInformation("Tasks completed!");
        }

        private static Task GetDbDumpTask(Config config)
        {
            return Task.Factory.StartNew(() =>
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(config.MongoDumpPath, config.MongoDumpArgs)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };
                process.OutputDataReceived += (a, e) => Trace.TraceInformation(e.Data);
                process.ErrorDataReceived += (a, e) => Trace.TraceEvent(TraceEventType.Error, 0, e.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                Trace.TraceInformation("Dump completed!");
            });
        }

        private static Task GetDownloadFilesTask(Config config)
        {
            return Task.Factory.StartNew(() =>
            {
                // download file
                new WebClient().DownloadFile(new Uri(config.DownloadFilesUrl), config.FilesPath);
                Trace.TraceInformation("Download Completed!");
            });
        }
    }
}
