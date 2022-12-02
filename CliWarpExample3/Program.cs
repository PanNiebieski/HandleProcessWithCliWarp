using CliWrap;
using CliWrap.Buffered;



Console.WriteLine("\n=======================================================");
Console.WriteLine("==================Update docker image==================");
Console.WriteLine("=======================================================\n");

var cmd0 = Cli.Wrap(@"docker")
    .WithArguments("build -t counter-image -f Dockerfile .")
    .WithWorkingDirectory(@"C:\Users\PanNiebieski\source\repos\HandleProcess\CounterApp")
    .WithValidation(CommandResultValidation.None)
    .WithStandardErrorPipe(PipeTarget.ToDelegate(ErrorHandler))
    .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler))
    |
    Cli.Wrap(@"docker")
    .WithArguments("build -t counter-image-2 -f Dockerfile .")
    .WithWorkingDirectory(@"C:\Users\PanNiebieski\source\repos\HandleProcess\CounterApp2")
    .WithValidation(CommandResultValidation.None)
    .WithStandardErrorPipe(PipeTarget.ToDelegate(ErrorHandler2))
    .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler2))
    ;

var r = await cmd0.ExecuteBufferedAsync();

if (r.ExitCode != 0)
    return;











Console.WriteLine("\n===================================================");
Console.WriteLine("==================Run many docker images==========");
Console.WriteLine("===================================================\n");




var cmd3 = Cli.Wrap(@"docker")
    .WithArguments(args =>
        args.Add("run")
        //.Add("-it")
        .Add("--rm")
        .Add("counter-image")
        .Add("10")
    ).WithStandardErrorPipe(PipeTarget.ToDelegate(ErrorHandler))
    .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler))
    |
     Cli.Wrap("ping")
        .WithArguments("cezarywalenciuk.pl")
        .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler3))
    |
    Cli.Wrap(@"docker")
    .WithArguments(args =>
        args.Add("run")
        //.Add("-it")
        .Add("--rm")
        .Add("counter-image-2")
        .Add("18")
    ).WithStandardErrorPipe(PipeTarget.ToDelegate(ErrorHandler2))
    .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler2));



var r2 = await cmd3.ExecuteBufferedAsync();

Console.WriteLine("\n===================================================");
Console.WriteLine("==================Run many pings ==========");
Console.WriteLine("===================================================\n");

var cmd4 = Cli.Wrap("ping")
        .WithArguments("google.pl")
        .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler))
    |
     Cli.Wrap("ping")
        .WithArguments("cezarywalenciuk.pl")
        .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler3))
    |
    Cli.Wrap("ping")
        .WithArguments("onet.pl")
        .WithStandardOutputPipe(PipeTarget.ToDelegate(OutPutHandler2));

var r3 = await cmd4.ExecuteBufferedAsync();


void ErrorHandler(string v)
{
    Console.ForegroundColor = ConsoleColor.Magenta;

    if (v != null)
        Console.WriteLine($"==> {v}");

    Console.ResetColor();
}

void OutPutHandler(string v)
{
    Console.ForegroundColor = ConsoleColor.Magenta;

    if (v != null)
        Console.WriteLine($"Output -> {v}");


    Console.ResetColor();
}

void ErrorHandler2(string v)
{
    Console.ForegroundColor = ConsoleColor.Cyan;

    if (v != null)
        Console.WriteLine($"==> {v}");

    Console.ResetColor();
}

void OutPutHandler2(string v)
{
    Console.ForegroundColor = ConsoleColor.Cyan;

    if (v != null)
        Console.WriteLine($"Output -> {v}");

    Console.ResetColor();
}

void OutPutHandler3(string v)
{
    Console.ForegroundColor = ConsoleColor.Green;

    if (v != null)
        Console.WriteLine($"Output -> {v}");

    Console.ResetColor();
}