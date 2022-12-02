using CliWrap;
using CliWrap.Buffered;
using CliWrap.EventStream;
using System.Diagnostics;

Console.WriteLine("\n=======================================================");
Console.WriteLine("==================Update docker image==================");
Console.WriteLine("=======================================================\n");

var cmd0 = Cli.Wrap(@"docker")
    .WithArguments("build -t counter-image -f Dockerfile .")
    .WithWorkingDirectory(@"C:\Users\PanNiebieski\source\repos\HandleProcess\CounterApp")
    .WithValidation(CommandResultValidation.None)
    .WithStandardErrorPipe(PipeTarget.ToDelegate(ErrorHandler))
    .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler));

var r = await cmd0.ExecuteBufferedAsync();

if (r.ExitCode != 0)
    return;

Console.WriteLine("\n===================================================");
Console.WriteLine("==================Run docker image=================");
Console.WriteLine("===================================================\n");

var cmd1 = Cli.Wrap(@"docker")
    .WithArguments(args =>
        args.Add("run")
        //.Add("-it")
        .Add("--rm")
        .Add("counter-image")
        .Add("4")
    );

await foreach (var cmdEvent in cmd1.ListenAsync())
{
    switch (cmdEvent)
    {
        case StartedCommandEvent started:
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"Process started {started.ProcessId}");
            Console.ResetColor();
            break;
        case StandardOutputCommandEvent standardOutput:
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Out -->{standardOutput.Text}");
            Console.WriteLine($"");
            Console.ResetColor();
            break;
        case StandardErrorCommandEvent standardError:
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Error -->{standardError.Text}");
            Console.ResetColor();
            break;

        case ExitedCommandEvent exited:
            Console.WriteLine($"Proces exited with {exited.ExitCode}");
            break;

    }
}

Console.WriteLine("\n===================================================");
Console.WriteLine("==================Run docker image Again ==========");
Console.WriteLine("===================================================\n");


var cmd2 = Cli.Wrap(@"docker")
    .WithArguments(args =>
        args.Add("run")
        //.Add("-it")
        .Add("--rm")
        .Add("counter-image")
        .Add("4")
    ) | (Console.WriteLine, Console.Error.WriteLine);

await cmd2.ExecuteAsync();




void ErrorHandler(string v)
{
    if (v != null)
        Console.WriteLine($"==> {v}");
}

void OutPutHandler(string v)
{
    if (v != null)
        Console.WriteLine($"Output -> {v}");
}