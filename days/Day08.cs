namespace AdventOfCode2025.days;

using System;

public class Day08 {
    record Point3D(long X, long Y, long Z);

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        int threshold = test ? 10 : 1000;
        List<Point3D> junctionBoxes = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(',');
            int x = int.Parse(splitLine[0]);
            int y = int.Parse(splitLine[1]);
            int z = int.Parse(splitLine[2]);
            junctionBoxes.Add(new Point3D(x, y, z));
        }

        // Part 1
        Part1(junctionBoxes, threshold);

        // Part 2
        Part2(junctionBoxes);
    }

    private static void Part1(List<Point3D> junctionBoxes, int threshold) {
        int result = 0;
        // Test all distances between pairs of points.
        List<(Point3D p, Point3D q, int dist)> connections = [];
        for (int i = 0; i < junctionBoxes.Count; i++) {
            for (int j = i + 1; j < junctionBoxes.Count; j++) {
                Point3D p = junctionBoxes[i];
                Point3D q = junctionBoxes[j];
                double dx = p.X - q.X;
                double dy = p.Y - q.Y;
                double dz = p.Z - q.Z;
                int dist = (int)Math.Sqrt(dx*dx + dy*dy + dz*dz);
                connections.Add((p, q, dist));
            }
        }
        // Combine top [threshold] connections into circuits.
        var closest = connections.OrderBy(e => e.dist).Take(threshold).ToList();
        List<HashSet<Point3D>> circuits = [];
        foreach (var (p, q, _) in closest) {
            HashSet<Point3D>? circuitWithBox = null;
            HashSet<Point3D>? circuitWithTarget = null;
            foreach (HashSet<Point3D> circuit in circuits) {
                if (circuit.Contains(p))
                    circuitWithBox = circuit;
                if (circuit.Contains(q))
                    circuitWithTarget = circuit;
            }
            if (circuitWithBox == null && circuitWithTarget == null)
                circuits.Add([p, q]);
            else if (circuitWithBox != null && circuitWithTarget == null)
                circuitWithBox.Add(q);
            else if (circuitWithBox == null && circuitWithTarget != null)
                circuitWithTarget.Add(p);
            else if (circuitWithBox != circuitWithTarget) {
                circuitWithBox!.UnionWith(circuitWithTarget!);
                circuits.Remove(circuitWithTarget!);
            }
        }
        // Take top three largest circuits and compute product of their sizes.
        var topThree = circuits.Select(c => c.Count)
            .OrderByDescending(n => n).Take(3).ToList();
        while (topThree.Count < 3)
            topThree.Add(1);
        result = topThree.Aggregate(1, (prod, val) => prod * val);
        Console.WriteLine(result);
    }

    private static void Part2(List<Point3D> junctionBoxes) {
        long result = 0;
        // Test all distances between pairs of points.
        List<(Point3D p, Point3D q, int dist)> connections = [];
        for (int i = 0; i < junctionBoxes.Count; i++) {
            for (int j = i + 1; j < junctionBoxes.Count; j++) {
                Point3D p = junctionBoxes[i];
                Point3D q = junctionBoxes[j];
                double dx = p.X - q.X;
                double dy = p.Y - q.Y;
                double dz = p.Z - q.Z;
                int dist = (int)Math.Sqrt(dx*dx + dy*dy + dz*dz);
                connections.Add((p, q, dist));
            }
        }
        // Combine top [threshold] connections into circuits.
        var sorted = connections.OrderBy(e => e.dist);
        List<HashSet<Point3D>> circuits = [];
        (Point3D a, Point3D b) lastConnection = default;
        foreach (var (p, q, _) in sorted) {
            HashSet<Point3D>? circuitWithBox = null;
            HashSet<Point3D>? circuitWithTarget = null;
            foreach (HashSet<Point3D> circuit in circuits) {
                if (circuit.Contains(p))
                    circuitWithBox = circuit;
                if (circuit.Contains(q))
                    circuitWithTarget = circuit;
            }
            if (circuitWithBox == null && circuitWithTarget == null)
                circuits.Add([p, q]);
            else if (circuitWithBox != null && circuitWithTarget == null)
                circuitWithBox.Add(q);
            else if (circuitWithBox == null && circuitWithTarget != null)
                circuitWithTarget.Add(p);
            else if (circuitWithBox != circuitWithTarget) {
                circuitWithBox!.UnionWith(circuitWithTarget!);
                circuits.Remove(circuitWithTarget!);
            }
            lastConnection = (p, q);
            // Stop when every juction box has formed a single circuit.
            if (circuits.Count == 1 && circuits[0].Count == junctionBoxes.Count)
                break;
        }
        // Multiply X coordinate of last two connections made.
        result = lastConnection.a!.X * lastConnection.b!.X;
        Console.WriteLine(result);
    }
}
