namespace AdventOfCode2025.days;

using System;

public class Day03 {
    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<int[]> banks = [];
        foreach (string line in lines)
            banks.Add([.. line.Select(c => c - '0')]);

        // Part 1
        Part1(banks);

        // Part 2
        Part2(banks);
    }

    private static void Part1(List<int[]> banks) {
        int result = 0;
        foreach (int[] batteries in banks) {
            int first = Array.IndexOf(batteries, batteries.Max());
            if (first == batteries.Length - 1)
                first = Array.IndexOf(batteries, batteries.Take(batteries.Length - 1).Max());
            int second = batteries
                .Skip(first + 1)
                .Select((v, i) => i + first + 1)
                .MaxBy(i => batteries[i]);
            int joltage = int.Parse(batteries[first].ToString() + batteries[second].ToString());
            result += joltage;
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<int[]> banks) {
        long result = 0;
        foreach (int[] batteries in banks) {
            long joltage = 0;
            List<int> bank = [.. batteries];
            for (int i = 0; i < 11; i++) {
                int digit = bank.Take(bank.Count + i - 11).Max();
                int index = bank.IndexOf(digit);
                bank = [.. bank.Skip(index + 1)];
                joltage = (joltage * 10) + digit;
            }
            joltage = (joltage * 10) + bank.Max();
            result += joltage;
        }
        Console.WriteLine(result);
    }
}
