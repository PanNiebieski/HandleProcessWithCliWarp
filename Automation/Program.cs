using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Docker.Automation
{
    class Program
    {
        static string containerRunCommand = null;
        static string ip;
        static void Main(string[] args)
        {
            // In this case I am using azurite container, you can use container of your choice  
            Console.WriteLine("Spinning up azurite container...");
            containerRunCommand = "run --name azurite -p 10000:10000 mcr.microsoft.com/azure-storage/azurite azurite-blob --blobHost 0.0.0.0";
            RunCommand(containerRunCommand, false);
            Console.WriteLine("Getting Container IP...");
            string inspectCommand = string.Concat("inspect -f ", "\"{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}\"", " azurite");
            RunCommand(inspectCommand, true);
            if (ip != null) ip = ""
            Console.WriteLine("Please press any key to exit");
            Console.ReadLine();
            Environment.Exit(1);
        }
        /// <summary>  
        /// This method runs cmd commands, here in this case will run the container and also inspect the container and get the ipaddress  
        /// </summary>  
        /// <param name="command"></param>  
        /// <param name="stdoutput"></param>  
        private static void RunCommand(string command, bool stdoutput)
        {
            var processInfo = new ProcessStartInfo("docker", $"{command}");
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            int exitCode;
            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                var started = process.Start();
                if (stdoutput)
                {
                    StreamReader reader = process.StandardOutput;
                    ip = Regex.Replace(reader.ReadToEnd(), @"\t|\n|\r", "");
                    if (string.IsNullOrEmpty(ip))
                    {
                        Console.WriteLine($"Unable to get ip of the container");
                        Environment.Exit(1);
                    }
                    Console.WriteLine($"Azurite conatainer is listening @ {ip}");
                }
                process.WaitForExit(12000);
                if (!process.HasExited)
                {
                    process.Kill();
                }
                exitCode = process.ExitCode;
                process.Close();
                Console.WriteLine("Azurite is up and running");
            }
        }

    }
}