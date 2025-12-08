namespace AdventOfCode2025.days;

using System;

public class Day07 {
    record Pos(int R, int C);
    static readonly Dictionary<(int r, int c), long> memo = [];

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        int rows = lines.Count;
        int cols = lines[0].Length;
        char[,] labMap = new char[rows, cols];
        Pos startPos = new(0, 0);
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                labMap[r, c] = lines[r][c];
                if (lines[r][c] == 'S')
                    startPos = new(r, c);
            }
        }

        // Part 1
        Part1(labMap, startPos);

        // Part 2 (recursive)
        Console.WriteLine(Part2(labMap, startPos.R, startPos.C));
    }

    private static void Part1(char[,] labMap, Pos startPos) {
        int result = 0;
        int rows = labMap.GetLength(0);
        int cols = labMap.GetLength(1);

        List<Pos> activeBeams = [startPos];
        HashSet<(int r, int c)> visited = [];
        while (activeBeams.Count > 0) {
            List<Pos> nextBeams = [];
            foreach (Pos beam in activeBeams) {
                int newR = beam.R + 1;
                if (newR >= rows)
                    continue;
                if (labMap[newR, beam.C] == '.' && visited.Add((newR, beam.C))) {
                    nextBeams.Add(new Pos(newR, beam.C));
                    // labMap[newR, beam.C] = '|';
                } else if (labMap[newR, beam.C] == '^') {
                    result++;
                    int leftC = beam.C - 1;
                    int rightC = beam.C + 1;
                    if (leftC >= 0 && leftC < cols && labMap[newR, leftC] == '.'
                    && visited.Add((newR, leftC))) {
                        nextBeams.Add(new Pos(newR, leftC));
                        // labMap[newR, leftC] = '|';
                    }
                    if (rightC >= 0 && rightC < cols && labMap[newR, rightC] == '.' 
                    && visited.Add((newR, rightC))) {
                        nextBeams.Add(new Pos(newR, rightC));
                        // labMap[newR, rightC] = '|';
                    }
                }
            }
            activeBeams = nextBeams;
            // PrintLabMap(labMap);
        }
        Console.WriteLine(result);
    }

    private static long Part2(char[,] labMap, int r, int c) {
        int rows = labMap.GetLength(0);
        int cols = labMap.GetLength(1);
        if (c < 0 || c >= cols)
            return 0;
        if (r >= rows)
            return 1;
        if (memo.TryGetValue((r, c), out long cached))
            return cached;
        long result;
        char current = labMap[r, c];
        if (current == '.' || current == 'S')
            result = Part2(labMap, r + 1, c);
        else if (current == '^') {
            result = Part2(labMap, r, c - 1) + Part2(labMap, r, c + 1);
        } else
            result = 0;
        memo[(r, c)] = result;
        return result;
    }

    // Debug printing function (not for part 2, would implode CPU)
    private static void PrintLabMap(char[,] labMap) {
        int rows = labMap.GetLength(0);
        int cols = labMap.GetLength(1);
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++)
                Console.Write(labMap[r, c]);
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
