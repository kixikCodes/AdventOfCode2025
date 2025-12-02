namespace AdventOfCode2025.days;

using System;

public class Day01 {
    private record Rotation(char Direction, int Distance);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Rotation> rotations = [];
        foreach (string line in lines) {
            char direction = line[0];
            int distance = Convert.ToInt32(line[1..]);
            rotations.Add(new(direction, distance));
        }

        // Part 1
        Part1(rotations);

        // Part 2
        Part2(rotations);
    }

    private static void Part1(List<Rotation> rotations) {
        int result = 0;
        int currentPos = 50;
        foreach (var rotation in rotations) {
            if (rotation.Direction == 'R') {
                currentPos += rotation.Distance;
                currentPos %= 100;
            }
            else if (rotation.Direction == 'L') {
                currentPos -= rotation.Distance;
                currentPos %= 100;
                if (currentPos < 0)
                    currentPos += 100;
            }
            if (currentPos == 0)
                result++;
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<Rotation> rotations) {
        int result = 0;
        int currentPos = 50;
        foreach (var rotation in rotations) {
            int totalSteps = rotation.Direction == 'R'
                ? rotation.Distance
                : -rotation.Distance;
            int fullLaps = Math.Abs(totalSteps) / 100;
            result += fullLaps;
            int remainingSteps = Math.Abs(totalSteps) % 100;
            if (remainingSteps == 0) {
                currentPos = (currentPos + totalSteps) % 100;
                if (currentPos < 0)
                    currentPos += 100;
                continue;
            }
            int previousPos = currentPos;
            if (totalSteps > 0) {
                int stepsToZero = (100 - previousPos) % 100;
                if (stepsToZero > 0 && remainingSteps >= stepsToZero)
                    result++;
            } else {
                int stepsToZero = previousPos % 100;
                if (stepsToZero > 0 && remainingSteps >= stepsToZero)
                    result++;
            }
            int newPos = (currentPos + totalSteps) % 100;
            if (newPos < 0)
                newPos += 100;
            currentPos = newPos;
        }
        Console.WriteLine(result);
    }
}
