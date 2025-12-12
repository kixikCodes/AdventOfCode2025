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

        // Part 2 (again, unviable bcs they made the path count ridiculous)
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
        int result;
        void DFS(string node, bool seendac, bool see) {
            currentPath.Add(node);
            if (node == "out")
                paths.Add([.. currentPath]);
            else if (nodes.TryGetValue(node, out List<string>? nextNodes)) {
                foreach (var next in nextNodes)
                    DFS(next, currentPath);
            }
            currentPath.RemoveAt(currentPath.Count - 1);
        }
        DFS("svr", []);
        result = paths.Count(path => path.Contains("dac") && path.Contains("fft"));
        Console.WriteLine(result);
    }
}
