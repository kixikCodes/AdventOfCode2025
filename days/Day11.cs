namespace AdventOfCode2025.days;

using System;

public class Day11 {
    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        Dictionary<string, List<string>> nodes = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(": ");
            nodes[splitLine[0]] = [.. splitLine[1].Split(' ')];
        }

        // Part 1
        Part1(nodes);

        // Part 2 (Thanks to @HyperNeutrino)
        Part2(nodes);
    }

    private static void Part1(Dictionary<string, List<string>> nodes) {
        int DFS(string node) {
            if (node == "out")
                return 1;
            if (!nodes.TryGetValue(node, out List<string>? value) || value.Count == 0)
                return 0;
            int pathCount = 0;
            foreach (var next in nodes[node])
                pathCount += DFS(next);
            return pathCount;
        }
        Console.WriteLine(DFS("you"));
    }

    private static void Part2(Dictionary<string, List<string>> nodes) {
        Dictionary<(string start, string end), long> memo = [];

        long DFS(string start, string end) {
            if (memo.TryGetValue((start, end), out var value))
                return value;
            long pathCount = 0;
            if (start == end)
                pathCount = 1;
            else if (!nodes.TryGetValue(start, out var nextNodes))
                pathCount = 0;
            else {
                long sum = 0;
                foreach (var next in nextNodes)
                    sum += DFS(next, end);
                pathCount = sum;
            }
            memo[(start, end)] = pathCount;
            return pathCount;
        }
        Console.WriteLine(DFS("svr", "dac") * DFS("dac", "fft") * DFS("fft", "out")
                        + DFS("svr", "fft") * DFS("fft", "dac") * DFS("dac", "out"));
    }
}
