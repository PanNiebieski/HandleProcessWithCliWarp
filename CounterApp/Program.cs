using static System.Runtime.InteropServices.JavaScript.JSType;

var counter = 0;
var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
while (max == -1 || counter < max)
{
    Console.WriteLine($"X     : {++counter}");
    Console.WriteLine($"X * 2 : {counter * 2}");
    Console.WriteLine($"X ^ 2 : {counter * counter}");

    int fact = 1;
    for (int i = 1; i <= counter; i++)
    {
        fact = fact * i;
    }
    Console.WriteLine($"X!    : {fact}");

    await Task.Delay(TimeSpan.FromMilliseconds(1_000));
}