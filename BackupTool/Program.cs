using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteError("Expected path to a config file!");
                return;
            }

            if (!File.Exists(args[0]))
            {
                WriteError("Unable to find config file!");
                return;
            }

            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(args[0]));

            // backup:
            var psi = new ProcessStartInfo(config.MongoDumpCmd.Replace("{DestinationPath}", config.DestinationPath));
          
        }

        private static void WriteLine(string str, ConsoleColor color = ConsoleColor.Gray)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = originalColor;
        }

        private static void WriteError(string str)
        {
            WriteLine(str, ConsoleColor.Red);
        }
    }
}
