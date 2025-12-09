namespace AdventOfCode2025.days;

using System;

public class Day09 {
    record Point(int X, int Y);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Point> points = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(',');
            int x = int.Parse(splitLine[0]);
            int y = int.Parse(splitLine[1]);
            points.Add(new Point(x, y));
        }

        // Part 1
        Part1(points);

        // Part 2
        Part2();
    }

    private static void Part1(List<Point> points) {
        long result = 0;
        for (int i = 0; i < points.Count; i++) {
            for (int j = i + 1; j < points.Count; j++) {
                long width = Math.Abs(points[i].X - points[j].X) + 1;
                long height = Math.Abs(points[i].Y - points[j].Y) + 1;
                long area = width * height;
                if (area > result)
                    result = area;
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2() {
        
    }
}
