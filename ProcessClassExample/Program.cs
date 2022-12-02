using System.Diagnostics;

Console.WriteLine("Press any key to run CMD...");
Console.ReadKey();

ProcessStartInfo processStartInfo = new ProcessStartInfo();
processStartInfo.FileName = @"C:\Windows\system32\cmd.exe";
processStartInfo.Arguments = "/c date /t";

processStartInfo.CreateNoWindow = true;
processStartInfo.UseShellExecute = false;
processStartInfo.RedirectStandardOutput = true;

Process process = new Process();
process.StartInfo = processStartInfo;
process.Start();

string output = process.StandardOutput.ReadToEnd();
process.WaitForExit();

Console.WriteLine("Current date (received from CMD):");
Console.Write(output);
