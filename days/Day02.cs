namespace AdventOfCode2025.days;

using System;
using System.Text.RegularExpressions;

public class Day02 {
    private record Range(string FirstId, string LastId);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        string[] rangeStrings = test
            ? File.ReadAllText(testPath).Split(',')
            : File.ReadAllText(inputPath).Split(',');
        List<Range> ranges = [];
        foreach (string range in rangeStrings)
            ranges.Add(new(range.Split('-')[0], range.Split('-')[1]));

        // Part 1
        Part1(ranges);

        // Part 2
        Part2(ranges);
    }

    private static void Part1(List<Range> ranges) {
        long result = 0;
        Regex recursivePattern = new(@"^([1-9]\d*)\1$", RegexOptions.Compiled);
        foreach (Range idRange in ranges) {
            long start = Convert.ToInt64(idRange.FirstId);
            long end = Convert.ToInt64(idRange.LastId);
            for (long i = start; i <= end; i++) {
                string idString = i.ToString();
                if (recursivePattern.IsMatch(idString))
                    result += i;
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<Range> ranges) {
        long result = 0;
        Regex recursivePattern = new(@"^(\d+)\1+$", RegexOptions.Compiled);
        foreach (Range idRange in ranges) {
            long start = Convert.ToInt64(idRange.FirstId);
            long end = Convert.ToInt64(idRange.LastId);
            for (long i = start; i <= end; i++) {
                string idString = i.ToString();
                if (recursivePattern.IsMatch(idString))
                    result += i;
            }
        }
        Console.WriteLine(result);
    }
}
