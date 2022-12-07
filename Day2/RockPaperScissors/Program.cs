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

var score = Regex.Replace(inputString, @"((\r)+)?(\n)+((\r)+)?", ",")
    .Split(",", StringSplitOptions.None)
    .Select(s => new Round(s));

Console.WriteLine($"(Orig) Round score is: {score.Select(s => s.MyScore()).Sum()}");
Console.WriteLine($"(Orig) Round score is: {score.Select(s => s.NewScore()).Sum()}");

static (string, string) GetInputString()
{
    try
    {
        var resStr = File.ReadAllText("C:\\Development\\AoC\\2022\\Day2\\RockPaperScissors\\Data.txt");

        return (resStr ?? "", "");
    }
    catch (Exception ex)
    {
        return ("", ex.Message);
    }
}

enum Shape
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}

enum Result
{
    Loose,
    Draw,
    Win
}

static class ShapeMethods
{
    public static Result ResFromString(string i)
    {
        switch (i)
        {
            case "X": return Result.Loose;
            case "Y": return Result.Draw;
            default: return Result.Win;
        }
    }

    public static Shape FromString(string i)
    {
        switch (i) 
        {
            case "A": return Shape.Rock;
            case "B": return Shape.Paper;
            case "C": return Shape.Scissors;
            case "X": return Shape.Rock;
            case "Y": return Shape.Paper;
            default: return Shape.Scissors;
        }
    }

    public static int HighestWin(this Shape shape) => (shape == Shape.Scissors ? 1 : (int)shape + 1) + 6;

    public static int HighestDraw(this Shape shape) => (int)shape + 3;

    public static int HighestLoss(this Shape shape) => (shape == Shape.Rock ? 3 : (int)shape - 1);

    public static int ScoreOver(this Shape shape, Shape other)
    {
        if (shape == other) return (int)shape + 3;
        else if (((shape == Shape.Rock && other == Shape.Scissors) ||
            (int)shape - (int)other > 0) && 
            !(shape == Shape.Scissors && other == Shape.Rock)) return (int)shape + 6;
        return (int)shape;
    }
}

class Round
{
    public Round(string str)
    {
        string[] splitStr = str.Split(' ');
        Opponent = ShapeMethods.FromString(splitStr.FirstOrDefault(""));
        Mine = ShapeMethods.FromString(splitStr.LastOrDefault(""));
        Result = ShapeMethods.ResFromString(splitStr.LastOrDefault(""));
    }
    public int MyScore() => Mine.ScoreOver(Opponent);
    public int NewScore()
    {
        switch (Result)
        {
            case Result.Loose: return Opponent.HighestLoss();
            case Result.Draw: return Opponent.HighestDraw();
            default: return Opponent.HighestWin();
        }
    }
    public Shape Opponent { get; set; } = Shape.Rock;
    public Shape Mine { get; set; } = Shape.Rock;
    public Result Result { get; set; } = Result.Draw;
}
