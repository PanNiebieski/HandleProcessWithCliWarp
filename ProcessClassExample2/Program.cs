using System;
using System.Diagnostics;

Console.WriteLine("\n=======================================================");
Console.WriteLine("==================Update docker image==================");
Console.WriteLine("=======================================================\n");

List<string> argList = new List<string>();
argList.Add("docker build -t counter-image -f Dockerfile .");

var psi = new ProcessStartInfo();
psi.FileName = @"C:\Windows\system32\cmd.exe";
psi.RedirectStandardInput = true;
psi.UseShellExecute = false;
psi.RedirectStandardInput = true;
psi.RedirectStandardOutput = true;
psi.RedirectStandardError = true;
psi.UseShellExecute = false;
psi.WorkingDirectory = @"C:\Users\PanNiebieski\source\repos\HandleProcess\CounterApp";

using (var process = new Process())
{
    process.StartInfo = psi;
    process.OutputDataReceived += (sender, e) => TestHandler(sender, e, "Output");
    process.ErrorDataReceived += (sender, e) => TestHandler(sender, e, "Error");

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();


    using (StreamWriter sw = process.StandardInput)
    {
        if (sw.BaseStream.CanWrite)
        {
            foreach (var item in argList)
            {
                sw.WriteLine(item);
            }
        }
    }

    process.WaitForExit(25000);



    if (!process.HasExited)
    {
        process.Kill();
    }
    process.Close();
}

Console.WriteLine("\n===================================================");
Console.WriteLine("==================Run docker image=================");
Console.WriteLine("===================================================\n");

var processInfo = new ProcessStartInfo();

//processInfo.FileName = @"C:\Windows\system32\cmd.exe";
//processInfo.Arguments = "docker run -it --rm counter-image 1";
processInfo.FileName = @"C:\Program Files\Docker\Docker\resources\bin\docker.exe";
processInfo.Arguments = "run -it --rm counter-image 4";
processInfo.CreateNoWindow = true;
processInfo.UseShellExecute = false;
processInfo.RedirectStandardOutput = true;
processInfo.RedirectStandardError = true;

int exitCode;
using (var process = new Process())
{
    process.StartInfo = processInfo;
    process.OutputDataReceived += (sender, e) => TestHandler(sender, e, "Output");
    process.ErrorDataReceived += (sender, e) => TestHandler(sender, e, "Error");

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    process.WaitForExit(12000);
    if (!process.HasExited)
    {
        process.Kill();
    }

    exitCode = process.ExitCode;
    process.Close();
}

void TestHandler(object sender, DataReceivedEventArgs e, string v)
{
    if (e.Data != null)
        Console.WriteLine($"{v} {e.Data}");
}