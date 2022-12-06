using System.Text.RegularExpressions;
using System.IO;


var (inputString, errorString) = GetInputString();

if (String.IsNullOrEmpty(inputString))
{
    Console.WriteLine($"Failed to get the input string, error: {errorString}");
    Console.WriteLine($"Press any key to continue. ");
    Console.ReadKey();
    return;
}


var orderedTotals = Regex.Replace(inputString, @"((\r)+)?(\n)+((\r)+)?", ",")
    .Split(",,", StringSplitOptions.None)
    .Select(s => s.Split(",")
        .Select(i =>
        {
            try { return Int32.Parse(i); }
            catch { return 0; }
        })
        .Sum()
    )
    .OrderBy(s => s);


Console.WriteLine($"Max calories: {orderedTotals.Last()}, Min calories: {orderedTotals.First()}");

var top3Tot = orderedTotals.Reverse().Take(3).Sum();

Console.WriteLine($"Top 3 total: {top3Tot}");


static (string, string) GetInputString()
{
    try
    {
        var resStr = File.ReadAllText("C:\\Development\\AoC\\2022\\Day1\\ElfCalories\\Data.txt");

        return (resStr ?? "", "");
    }
    catch (Exception ex)
    {
        return ("", ex.Message);
    }
}
