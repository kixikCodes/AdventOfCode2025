namespace AdventOfCode2025.days;

using System;

public class Day04 {
    static readonly int[,] vectors = {
		{-1, -1}, {0, -1}, {1, -1},
		{ -1, 0}, /* @ */  { 1, 0},
		{ -1, 1}, { 0, 1}, { 1, 1}
	};

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        int rows = lines.Count;
        int cols = lines[0].Length;
        char[,] grid = new char[rows, cols];
        for (int r = 0; r < rows; r++) {
            string line = lines[r];
            for (int c = 0; c < cols; c++)
                grid[r, c] = line[c];
        }

        // Part 1
        Part1(grid);

        // Part 2
        Part2(grid);
    }

    private static void Part1(char[,] grid) {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int result = 0;

        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                if (grid[r, c] == '@') {
                    int adjacentRolls = 0;
                    for (int v = 0; v < vectors.GetLength(0); v++) {
                        int nr = r + vectors[v, 0];
                        int nc = c + vectors[v, 1];
                        if (nr < 0 || nr >= rows)
                            continue;
                        if (nc < 0 || nc >= cols)
                            continue;
                        if (grid[nr, nc] == '@')
                            adjacentRolls++;
                    }
                    if (adjacentRolls < 4)
                        result++;
                } else
                    continue;
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2(char[,] grid) {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int result = 0;

        while (true) {
            int removedRolls = 0;
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    if (grid[r, c] == '@') {
                        int adjacentRolls = 0;
                        for (int v = 0; v < vectors.GetLength(0); v++) {
                            int nr = r + vectors[v, 0];
                            int nc = c + vectors[v, 1];
                            if (nr < 0 || nr >= rows)
                                continue;
                            if (nc < 0 || nc >= cols)
                                continue;
                            if (grid[nr, nc] == '@')
                                adjacentRolls++;
                        }
                        if (adjacentRolls < 4) {
                            grid[r, c] = '.';
                            removedRolls++;
                        }
                    } else
                        continue;
                }
            }
            result += removedRolls;
            //PrintGrid(grid);
            if (removedRolls == 0)
                break;
        }
        Console.WriteLine(result);
    }

    // Debug printing method
    private static void PrintGrid(char[,] grid) {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                Console.Write(grid[r, c]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
