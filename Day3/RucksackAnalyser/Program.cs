using System.Text.RegularExpressions;


var (inputString, errorString) = GetInputString();

if (String.IsNullOrEmpty(inputString))
{
    Console.WriteLine($"Failed to get the input string, error: {errorString}");
    Console.WriteLine($"Press any key to continue. ");
    Console.ReadKey();
    return;
}

const string WEIGHTS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

var lines = Regex.Replace(inputString, @"((\r)+)?(\n)+((\r)+)?", ",")
    .Split(",", StringSplitOptions.None)
    .ToList();

var sumOfWeights = lines.Select(s => (
        s.Substring(0, (int)(s.Length / 2)), s.Substring((int)(s.Length / 2), (int)(s.Length / 2))
    )).Select(s =>
        s.Item1.Where(c => s.Item2.Contains(c)).FirstOrDefault(' ')
    ).Select(s => WEIGHTS.IndexOf(s) + 1).Sum();


Console.WriteLine($"Sum of common weights {sumOfWeights}");


// 2437
// 2342
var groups = lines.Partition(3)
    .Select(s =>
    {
        var head = s.Take(2);
        var tail = s.LastOrDefault("");
        return tail.Where(c => head.All(s => s.Contains(c))).FirstOrDefault(' ');
    });

Console.WriteLine($"Sum of common group weights {groups.Select(s => WEIGHTS.IndexOf(s) + 1).Sum()}");


static (string, string) GetInputString()
{
    try
    {
        var resStr = File.ReadAllText("C:\\Development\\AoC\\2022\\Day3\\RucksackAnalyser\\Data.txt");

        return (resStr ?? "", "");
    }
    catch (Exception ex)
    {
        return ("", ex.Message);
    }
}

public static class Extensions
{
    public static List<List<T>> Partition<T>(this List<T> values, int chunkSize)
    {
        return values.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}
