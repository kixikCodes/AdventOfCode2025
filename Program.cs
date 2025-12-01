namespace AdventOfCode2025;

using System;
using System.Reflection;

class Program {
    public static void Main(string[] args) {
        if (args.Length != 1) {
            Console.WriteLine("Usage: ./aoc <day>");
            return;
        }
        if (!int.TryParse(args[0], out int dayToRun) || dayToRun <= 0) {
            Console.WriteLine("Invalid day. Must be a positive number.");
            return;
        }
        string typeName = $"AdventOfCode2025.days.Day{dayToRun}";
        Type? dayType = Type.GetType(typeName);
        if (dayType == null) {
            Console.WriteLine($"Day {dayToRun} does not exist.");
            return;
        }
        MethodInfo? runMethod = dayType.GetMethod("RunDay", BindingFlags.Public | BindingFlags.Static);
        if (runMethod == null) {
            Console.WriteLine($"Day {dayToRun} not implemented.");
            return;
        }
        try {
            runMethod.Invoke(null, null);
        }
        catch (Exception e) {
            Console.WriteLine($"Error running day {dayToRun}:");
            Console.WriteLine(e.InnerException?.Message ?? e.Message);
        }
    }
}
