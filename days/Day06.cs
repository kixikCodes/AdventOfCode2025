namespace AdventOfCode2025.days;

using System;
using System.Linq;
using System.Text;

public class Day06 {
    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<long[]> operands = [];
        List<char> operators = [];
        var valueGrid = lines.SkipLast(1)
            .Select(line =>
            line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray())
        .ToArray();
        operands = [.. Enumerable.Range(0, valueGrid[0].Length)
                .Select(col => valueGrid.Select(row => row[col]).ToArray())];
        operators = [.. lines[^1].Where(c => c is '+' or '*')];

        // Part 1
        Part1(operands, operators);

        // Part 2
        Part2(lines);
    }

    private static void Part1(List<long[]> operands, List<char> operators) {
        long result = 0;
        for (int i = 0; i < operands.Count; i++) {
            long[] values = operands[i];
            char op = operators[i];

            long accumulator = operands[i][0];
            for (int j = 1; j < values.Length; j++) {
                accumulator = (op == '+')
                    ? accumulator += values[j]
                    : accumulator *= values[j]; // Only other operator is '*'.
            }
            result += accumulator;
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<string> lines) {
        int h = lines.Count;
        int w = lines[0].Length;

        List<string> rotatedLines = new(w);
        for (int x = w - 1; x >= 0; x--) {
            var sb = new StringBuilder(h);
            for (int y = 0; y < h; y++)
                sb.Append(lines[y][x]);
            rotatedLines.Add(sb.ToString());
        }

        List<long[]> operands = [];
        List<char> operators = [];
        List<string> currentBlock = [];

        foreach (var line in rotatedLines) {
            if (string.IsNullOrWhiteSpace(line)) {
                if (currentBlock.Count > 0) {
                    List<long> values = [];

                    for (int i = 0; i < currentBlock.Count - 1; i++)
                        values.Add(long.Parse(currentBlock[i].Trim()));
                    string last = currentBlock[^1].TrimEnd();
                    char op = last[^1];
                    string numberPart = last[..^1].Trim();
                    operators.Add(op);
                    values.Add(long.Parse(numberPart));
                    operands.Add([.. values]);
                    currentBlock.Clear();
                }
            } else
                currentBlock.Add(line);
        }
        if (currentBlock.Count > 0) {
            List<long> values = [];

            for (int i = 0; i < currentBlock.Count - 1; i++)
                values.Add(long.Parse(currentBlock[i].Trim()));
            string last = currentBlock[^1].TrimEnd();
            char op = last[^1];
            string numberPart = last[..^1].Trim();
            operators.Add(op);
            values.Add(long.Parse(numberPart));
            operands.Add([.. values]);
        }

        Part1(operands, operators); // lmfao
    }
}
