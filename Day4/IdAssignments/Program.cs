using System.Text.RegularExpressions;


var (inputString, errorString) = GetInputString();

if (String.IsNullOrEmpty(inputString))
{
    Console.WriteLine($"Failed to get the input string, error: {errorString}");
    Console.WriteLine($"Press any key to continue. ");
    Console.ReadKey();
    return;
}

var assignPars = Regex.Replace(inputString, @"((\r)+)?(\n)+((\r)+)?", "|")
    .Split("|", StringSplitOptions.None)
    .Select(l => new AssignmentPair(l))
    .ToList();



Console.WriteLine($"Contained pairs {assignPars.Where(p => p.IsContained()).Count()}");
// 785
Console.WriteLine($"Overlapping pairs {assignPars.Where(p => p.Overlaps()).Count()}");



static (string, string) GetInputString()
{
    try
    {
        var resStr = File.ReadAllText("C:\\Development\\AoC\\2022\\Day4\\IdAssignments\\Data.txt");

        return (resStr ?? "", "");
    }
    catch (Exception ex)
    {
        return ("", ex.Message);
    }
}

class AssignmentPair
{
    public AssignmentPair(string line)
    {
        var split = line.Split(',');
        var (left, right) = (split.FirstOrDefault(""), split.LastOrDefault(""));
        Left = RangeFromStr(left);
        Right = RangeFromStr(right);
    }
    private Range RangeFromStr(string s)
    {
        var split = s.Split('-');
        var (from, to) = (split.FirstOrDefault(""), split.LastOrDefault(""));
        return new Range(IntFromStr(from), IntFromStr(to));
    }
    private int IntFromStr(string s)
    {
        try { return Int32.Parse(s); }
        catch { return 0; }
    }
    public bool IsContained() => Left.IsContained(Right);
    public bool Overlaps() => Left.Overlaps(Right);
    public Range Left { get; set; } = new Range(0, 0);
    public Range Right { get; set; } = new Range(0, 0);
}

static class RangeExtensions
{
    public static bool IsContained(this Range range, Range other)
    {
        return (range.Start.Value <= other.Start.Value && range.End.Value >= other.End.Value) ||
            (other.Start.Value <= range.Start.Value && other.End.Value >= range.End.Value);
    }
    public static bool Overlaps(this Range range, Range other)
    {
        return (range.Start.Value >= other.Start.Value && range.Start.Value <= other.End.Value) ||
            (range.Start.Value >= other.Start.Value && range.End.Value <= other.End.Value) ||
            (range.Start.Value <= other.Start.Value && range.End.Value >= other.End.Value) ||
            (range.End.Value >= other.Start.Value && range.End.Value <= other.End.Value);
    }
}
