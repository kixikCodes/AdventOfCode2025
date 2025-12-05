namespace AdventOfCode2025.days;

using System;

public class Day05 {
    private record Range(long FirstId, long LastId);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Range> freshIdRanges = [];
        List<long> ids = [];

        bool parsedAllRanges = false;
        foreach (string line in lines) {
            if (string.IsNullOrEmpty(line)) {
                parsedAllRanges = true;
                continue;
            }
            if (!parsedAllRanges) {
                freshIdRanges.Add(new(
                    Convert.ToInt64(line.Split('-')[0]),
                    Convert.ToInt64(line.Split('-')[1])
                ));
            } else
                ids.Add(Convert.ToInt64(line));
        }


        // Part 1
        Part1(freshIdRanges, ids);

        // Part 2
        Part2(freshIdRanges);
    }

    private static void Part1(List<Range> freshIdRanges, List<long> ids) {
        int result = 0;

        foreach (long id in ids) {
            foreach (Range idRange in freshIdRanges) {
                if (id >= idRange.FirstId && id <= idRange.LastId) {
                    result++;
                    break;
                }
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<Range> freshIdRanges) {
        long result = 0;
        freshIdRanges.Sort((a, b) => a.FirstId.CompareTo(b.FirstId));

        int i = 0;
        while (i < freshIdRanges.Count - 1) {
            Range current = freshIdRanges[i];
            Range next = freshIdRanges[i + 1];
            if (current.LastId >= next.FirstId) {
                Range merged = new(
                    current.FirstId,
                    Math.Max(current.LastId, next.LastId)
                );
                freshIdRanges[i + 1] = merged;
                freshIdRanges.RemoveAt(i);
            } else
                i++;
        }

        foreach (Range idRange in freshIdRanges)
            result += idRange.LastId - idRange.FirstId + 1;
        Console.WriteLine(result);
    }
}
