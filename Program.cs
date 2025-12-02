namespace AdventOfCode2025;

using System;
using System.Reflection;

class Program {
    public static void Main(string[] args) {
        if (args.Length < 1 || args.Length > 2) {
            Console.WriteLine("Usage: ./aoc <day> or ./aoc test <day>");
            return;
        }
        bool test = args.Length == 2 && args[0] == "test";
        string dayArg = test ? args[1] : args[0];
        if (!int.TryParse(dayArg, out int dayToRun) || dayToRun <= 0) {
            Console.WriteLine("Invalid day. Must be a positive number.");
            return;
        }
        string day = dayToRun.ToString("D2");
        string typeName = $"AdventOfCode2025.days.Day{day}";
        Type? dayType = Type.GetType(typeName);
        if (dayType == null) {
            Console.WriteLine($"Day {day} does not exist.");
            return;
        }
        MethodInfo? runMethod = dayType.GetMethod("RunDay", BindingFlags.Public | BindingFlags.Static);
        if (runMethod == null) {
            Console.WriteLine($"Day {day} not implemented.");
            return;
        }
        try {
            string inputPath = $"inputs/Day{day}.txt";
            string testPath = $"tests/Day{day}.txt";
            runMethod.Invoke(null, [inputPath, testPath, test]);
        } catch (Exception e) {
            Console.WriteLine($"Error running day {day}:");
            Console.WriteLine(e.InnerException?.Message ?? e.Message);
        }
    }
}
