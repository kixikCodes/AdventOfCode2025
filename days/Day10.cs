namespace AdventOfCode2025.days;

using System;

record Machine(List<bool> IndicatorLights, List<List<int>> Buttons, List<int> JoltageRequirements);

public class Day10 {
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
        
        // Part 1 (BFS with Bit Packing)
        Part1(machineManuals);

        // Part 2 (Algebraic Solution with Gaussian Elimination and DFS)
        // Thanks to @HyperNeutrino and @icub3d for explanation. This was excessive for AoC, in my opinion.
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

        foreach (Machine m in machineManuals) {
            Matrix matrix = new(m);
            int max = m.JoltageRequirements.Max() + 1;
            int best = int.MaxValue;
            int[] free = new int[matrix.Independents.Count];

            static void DFS(Matrix m, int idx, int[] free, ref int best, int max) {
                if (idx == free.Length) {
                    var v = m.Check(free);
                    if (v.HasValue) best = Math.Min(best, v.Value);
                    return;
                }
                int partial = free.Take(idx).Sum();
                for (int i = 0; i < max && partial + i < best; i++) {
                    free[idx] = i;
                    DFS(m, idx + 1, free, ref best, max);
                }
            }

            DFS(matrix, 0, free, ref best, max);
            result += best;
        }
        Console.WriteLine(result);
    }
}

// Part 2 Exclusive Utility Class
class Matrix {
    public double[,] Data;
    public int Rows, Cols;
    public List<int> Dependents = [];
    public List<int> Independents = [];
    public const double Epsilon = 1e-9;

    public Matrix(Machine m) {
        Rows = m.JoltageRequirements.Count;
        Cols = m.Buttons.Count;
        Data = new double[Rows, Cols + 1];
        for (int c = 0; c < Cols; c++)
            foreach (int r in m.Buttons[c])
                Data[r, c] = 1;
        for (int r = 0; r < Rows; r++)
            Data[r, Cols] = m.JoltageRequirements[r];
        GaussianElimination();
    }

    private void GaussianElimination() {
        int pivot = 0;

        for (int col = 0; col < Cols && pivot < Rows; col++) {
            int best = pivot;
            double bestVal = Math.Abs(Data[pivot, col]);
            for (int r = pivot + 1; r < Rows; r++) {
                double v = Math.Abs(Data[r, col]);
                if (v > bestVal) {
                    bestVal = v;
                    best = r;
                }
            }
            if (bestVal < Epsilon) {
                Independents.Add(col);
                continue;
            }
            for (int c = col; c <= Cols; c++)
                (Data[pivot, c], Data[best, c]) = (Data[best, c], Data[pivot, c]);
            Dependents.Add(col);
            double div = Data[pivot, col];
            for (int c = col; c <= Cols; c++)
                Data[pivot, c] /= div;
            for (int r = 0; r < Rows; r++) {
                if (r == pivot) continue;
                double factor = Data[r, col];
                if (Math.Abs(factor) > Epsilon)
                    for (int c = col; c <= Cols; c++)
                        Data[r, c] -= factor * Data[pivot, c];
            }
            pivot++;
        }
        for (int c = Dependents.Count + Independents.Count; c < Cols; c++)
            Independents.Add(c);
    }

    public int? Check(int[] free) {
        int total = free.Sum();

        for (int r = 0; r < Dependents.Count; r++) {
            double v = Data[r, Cols];
            for (int i = 0; i < Independents.Count; i++)
                v -= Data[r, Independents[i]] * free[i];
            if (v < -Epsilon)
                return null;
            double round = Math.Round(v);
            if (Math.Abs(v - round) > Epsilon)
                return null;
            total += (int)round;
        }
        return total;
    }
}
