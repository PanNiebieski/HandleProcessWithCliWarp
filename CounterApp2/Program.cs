using static System.Runtime.InteropServices.JavaScript.JSType;

var counter = 20;
var min = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
while (min >= 20 || counter >= min)
{
    Console.WriteLine($"Y     : {--counter}");
    Console.WriteLine($"Y * 2 : {counter * 2}");
    Console.WriteLine($"Y ^ 2 : {counter * counter}");

    int fact = 1;
    for (int i = 1; i <= counter; i++)
    {
        fact = fact * i;
    }
    Console.WriteLine($"Y!    : {fact}");

    await Task.Delay(TimeSpan.FromMilliseconds(1_000));
}