namespace AdventOfCode2025.days;

using System;

public class Day10 {
    record Machine(List<char> IndicatorLights, List<List<int>> Buttons, List<int> JoltageRequirements);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Machine> machineManuals = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(' ');
            List<char> indicatorLights = [];
            List<List<int>> buttons = [];
            List<int> joltageRequirements = [];
            foreach (string token in splitLine) {
                if (token.StartsWith('[') && token.EndsWith(']'))
                    indicatorLights = [.. token[1..^1].ToCharArray()];
                else if (token.StartsWith('(') && token.EndsWith(')'))
                    buttons.Add([.. token[1..^1].Split(',').Select(int.Parse)]);
                else if (token.StartsWith('{') && token.EndsWith('}'))
                    joltageRequirements = [.. token[1..^1].Split(',').Select(int.Parse)];
            }
            machineManuals.Add(new(indicatorLights, buttons, joltageRequirements));
        }

        // Part 1
        Part1();

        // Part 2
        Part2();
    }

    private static void Part1() {
        
    }

    private static void Part2() {
        
    }
}
