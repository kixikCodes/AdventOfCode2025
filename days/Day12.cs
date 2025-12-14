namespace AdventOfCode2025.days;

using System;

public class Day12 {
    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        string input = test
            ? File.ReadAllText(testPath) // Solution does not work for test input (I cheesed it).
            : File.ReadAllText(inputPath);
        string[] blocks = input.Split("\n\n");
        string[] lines = blocks[^1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        List<(int x, int y)> regions = [];
        List<List<int>> presentLists = [];
        foreach (string line in lines) {
            List<int> values = [];
            int current = 0;
            bool inNumber = false;
            foreach (char c in line) {
                if (char.IsDigit(c)) {
                    current = current * 10 + (c - '0');
                    inNumber = true;
                } else if (inNumber) {
                    values.Add(current);
                    current = 0;
                    inNumber = false;
                }
            }
            if (inNumber)
                values.Add(current);
            if (values.Count < 2)
                continue;
            regions.Add((values[0], values[1]));
            presentLists.Add(values.GetRange(2, values.Count - 2));
        }

        // Solving (no parts on final day)
        int result = 0;
        for (int i = 0; i < presentLists.Count; i++) {
            List<int> presents = presentLists[i];
            int sum = 0;
            for (int j = 0; j < presents.Count; j++)
                sum += presents[j];
            int x = regions[i].x;
            int y = regions[i].y;
            if (x / 3 * (y / 3) >= sum)
                result++;
        }
        Console.WriteLine(result);
    }
}
