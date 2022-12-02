

using CliWrap;
using System.Text;

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));

StringBuilder sb = new StringBuilder();

try
{
    await Cli.Wrap("ping")
        .WithArguments("-t cezarywalenciuk.pl")
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(sb))
        .ExecuteAsync(cts.Token);
}
catch (OperationCanceledException ex)
{
    Console.WriteLine("TimeOut for this Task");
}
finally
{
    Console.WriteLine(sb.ToString());
}