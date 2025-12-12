namespace AdventOfCode2025.days;

using System;

public class Day10 {
    record Machine(List<bool> IndicatorLights, List<List<int>> Buttons, List<int> JoltageRequirements);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Machine> machineManuals = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(' ');
            List<bool> indicatorLights = [];
            List<List<int>> buttons = [];
            List<int> joltageRequirements = [];
            foreach (string token in splitLine) {
                if (token.StartsWith('[') && token.EndsWith(']'))
                    indicatorLights = [.. token[1..^1].Select(c => c == '#')];
                else if (token.StartsWith('(') && token.EndsWith(')'))
                    buttons.Add([.. token[1..^1].Split(',').Select(int.Parse)]);
                else if (token.StartsWith('{') && token.EndsWith('}'))
                    joltageRequirements = [.. token[1..^1].Split(',').Select(int.Parse)];
            }
            machineManuals.Add(new(indicatorLights, buttons, joltageRequirements));
        }
        //Debug(machineManuals);
        
        // Part 1
        Part1(machineManuals);

        // Part 2 (correct but unviable, BFS is too slow for input)
        Part2(machineManuals);
    }

    private static void Part1(List<Machine> machineManuals) {
        int result = 0;
        foreach (Machine manual in machineManuals) {
            int lightCount = manual.IndicatorLights.Count;
            int buttonCount = manual.Buttons.Count;
            int target = 0;
            for (int i = 0; i < lightCount; i++)
                if (manual.IndicatorLights[i])
                    target |= 1 << i;
            int[] buttonMasks = new int[buttonCount];
            for (int i = 0; i < buttonCount; i++) {
                int mask = 0;
                foreach (int lightIndex in manual.Buttons[i])
                    mask |= 1 << lightIndex;
                buttonMasks[i] = mask;
            }
            Queue<(int state, int steps)> q = new();
            q.Enqueue((0, 0));
            HashSet<int> visited = [0];
            while (q.Count > 0) {
                var (state, presses) = q.Dequeue();
                if (state == target) {
                    result += presses;
                    break;
                }
                foreach (int mask in buttonMasks) {
                    int next = state ^ mask;
                    if (visited.Add(next))
                        q.Enqueue((next, presses + 1));
                }
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<Machine> machineManuals) {
        int result = 0;
        foreach (Machine manual in machineManuals) {
            int joltagesCount = manual.JoltageRequirements.Count;
            int buttonCount = manual.Buttons.Count;
            int[] target = [.. manual.JoltageRequirements];
            int[] init = new int[joltagesCount];
            int[][] buttonIndexes = new int[buttonCount][];
            for (int i = 0; i < buttonCount; i++)
                buttonIndexes[i] = [.. manual.Buttons[i]];
            Queue<(int[] state, int steps)> q = new();
            q.Enqueue((init, 0));
            HashSet<string> visited = [];
            static string Key(int[] arr) => string.Join(',', arr);
            visited.Add(Key(init));
            while (q.Count > 0) {
                var (state, presses) = q.Dequeue();
                bool finished = true;
                for (int i = 0; i < joltagesCount; i++) {
                    if (state[i] != target[i]) {
                        finished = false;
                        break;
                    }
                }
                if (finished) {
                    result += presses;
                    break;
                }
                for (int i = 0; i < buttonCount; i++) {
                    int[] next = (int[])state.Clone();
                    foreach (int index in buttonIndexes[i])
                        next[index]++;
                    bool valid = true;
                    for (int j = 0; j < joltagesCount; j++) {
                        if (next[j] > target[j]) {
                            valid = false;
                            break;
                        }
                    }
                    if (!valid)
                        continue;
                    string key = Key(next);
                    if (!visited.Add(key))
                        continue;
                    q.Enqueue((next, presses + 1));
                }
            }
        }
        Console.WriteLine(result);
    }

    private static void Debug(List<Machine> machineManuals) {
        foreach (Machine manual in machineManuals) {
            Console.Write("[");
            Console.Write(string.Join(' ', manual.IndicatorLights));
            Console.Write("] ");
            foreach (List<int> button in manual.Buttons) {
                Console.Write("(");
                Console.Write(string.Join(' ', button));
                Console.Write(") ");
            }
            Console.Write("{");
            Console.Write(string.Join(' ', manual.JoltageRequirements));
            Console.WriteLine("}");
        }
    }
}
