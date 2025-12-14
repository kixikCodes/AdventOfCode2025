namespace AdventOfCode2025.days;

using System;

public class Day09 {
    record Cell(int X, int Y);
    record Rect(Cell A, Cell B);
    static readonly (int dx, int dy)[] Directions = [
        ( 0,-1),
        ( 0, 1),
        (-1, 0),
        ( 1, 0)
    ];

    public static void RunDay(string inputPath, string testPath, bool test) {
        // Parsing
        List<string> lines = test
            ? [.. File.ReadAllLines(testPath)]
            : [.. File.ReadAllLines(inputPath)];
        List<Cell> tiles = [];
        foreach (string line in lines) {
            string[] splitLine = line.Split(',');
            int x = int.Parse(splitLine[0]);
            int y = int.Parse(splitLine[1]);
            tiles.Add(new Cell(x, y));
        }

        // Part 1 (Brute-Force)
        Part1(tiles);

        // Part 2 (Tryhard)
        /*
        - Compress grid coordinates
        - Raycast bound tracing
        - Flood-Fill inner bounds
        - Find largest valid rectangle
        - Decompress for real area
        (test input has printable grid size with PrintGrid for visual representation)
        */
        Part2(tiles);
    }

    private static void Part1(List<Cell> tiles) {
        long result = 0;
        for (int i = 0; i < tiles.Count; i++) {
            for (int j = i + 1; j < tiles.Count; j++) {
                long width = Math.Abs(tiles[i].X - tiles[j].X) + 1;
                long height = Math.Abs(tiles[i].Y - tiles[j].Y) + 1;
                long area = width * height;
                if (area > result)
                    result = area;
            }
        }
        Console.WriteLine(result);
    }

    private static void Part2(List<Cell> tiles) {
        // Coordinate Compression
        var xs = tiles.Select(t => t.X).Distinct().OrderBy(x => x).ToList();
        var ys = tiles.Select(t => t.Y).Distinct().OrderBy(y => y).ToList();
        var xIndex = xs.Select((x, i) => (x, i)).ToDictionary(p => p.x, p => p.i);
        var yIndex = ys.Select((y, i) => (y, i)).ToDictionary(p => p.y, p => p.i);
        List<Cell> cTiles = [.. tiles.Select(t => new Cell(xIndex[t.X], yIndex[t.Y]))];

        // Grid Construction
        int gridHeight = ys.Count + 2;
        int gridWidth  = xs.Count + 2;
        char[,] grid = new char[gridHeight, gridWidth];
        for (int y = 0; y < gridHeight; y++)
            for (int x = 0; x < gridWidth; x++)
                grid[y, x] = '.';
        foreach (var t in cTiles)
            grid[t.Y + 1, t.X + 1] = '#';
        //PrintGrid(grid);

        // Trace Polygon Edge
        Cell startTile = cTiles
            .OrderBy(t => t.Y).ThenBy(t => t.X)
            .Select(t => new Cell(t.X + 1, t.Y + 1))
            .First();
        Cell currentTile = startTile;
        Cell previousTile = new(-1, -1);
        HashSet<(int x, int y)> visited = [(startTile.X, startTile.Y)];
        while (true) {
            if (!FindNextTile(grid, currentTile, previousTile, out Cell nextTile))
                break;
            DrawEdge(grid, currentTile, nextTile);
            if (nextTile.X == startTile.X && nextTile.Y == startTile.Y)
                break;
            if(!visited.Add((nextTile.X, nextTile.Y)))
                break;
            previousTile = currentTile;
            currentTile = nextTile;
        }
        //PrintGrid(grid);

        // Precompute Valid Bounds and Test Rectangles
        bool[,] validBounds = FloodFillBounds(grid);
        int[,] ps = BuildPrefixSum(validBounds);
        long result = 0;
        Rect? biggest = null;
        for (int i = 0; i < cTiles.Count; i++) {
            for (int j = i + 1; j < cTiles.Count; j++) {
                Cell a = cTiles[i];
                Cell b = cTiles[j];
                int minX = Math.Min(a.X, b.X) + 1;
                int maxX = Math.Max(a.X, b.X) + 1;
                int minY = Math.Min(a.Y, b.Y) + 1;
                int maxY = Math.Max(a.Y, b.Y) + 1;
                int w = maxX - minX + 1;
                int h = maxY - minY + 1;
                int insideCount = ps[maxY + 1, maxX + 1]
                    - ps[minY, maxX + 1]
                    - ps[maxY + 1, minX]
                    + ps[minY, minX];
                if (insideCount != w * h)
                    continue;
                // Decompress Coordinates
                long realWidth  = Math.Abs(xs[maxX - 1] - xs[minX - 1]) + 1;
                long realHeight = Math.Abs(ys[maxY - 1] - ys[minY - 1]) + 1;
                long area = realWidth * realHeight;
                if (area > result) {
                    result = area;
                    biggest = new Rect(a, b);
                }
            }
        }
        // Update Grid (test debugging) and Finish
        if (biggest != null)
            DrawRect(grid, biggest);
        //PrintGrid(grid);
        Console.WriteLine(result);
    }

    // -- Part 2 Methods --
    static bool Raycast(char[,] grid, Cell origin, Cell dir, out Cell hit, out int dist) {
        int gridX = grid.GetLength(1);
        int gridY = grid.GetLength(0);
        int x = origin.X + dir.X;
        int y = origin.Y + dir.Y;
        dist = 1;
        
        while (y >= 0 && y < gridY && x >= 0 && x < gridX) {
            if (grid[y, x] == '#') {
                hit = new (x, y);
                return true;
            }
            x += dir.X;
            y += dir.Y;
            dist++;
        }
        hit = new(-1, -1);
        dist = int.MaxValue;
        return false;
    }

    static bool FindNextTile(char[,] grid, Cell current, Cell previous, out Cell next) {
        int closestDist = int.MaxValue;
        next = new(-1, -1);

        foreach (var (dx, dy) in Directions) {
            if (Raycast(grid, current, new Cell(dx, dy), out Cell hit, out int dist)) {
                if (hit.X == previous.X && hit.Y == previous.Y)
                    continue;
                if (dist < closestDist) {
                    closestDist = dist;
                    next = hit;
                }
            }
        }
        return closestDist != int.MaxValue;
    }

    static void DrawEdge(char[,] grid, Cell start, Cell end) {
        int dx = Math.Sign(end.X - start.X);
        int dy = Math.Sign(end.Y - start.Y);
        int x = start.X + dx;
        int y = start.Y + dy;

        while (x != end.X || y != end.Y) {
            if (grid[y, x] == '.')
                grid[y, x] = 'X';
            x += dx;
            y += dy;
        }
    }

    static bool[,] FloodFillBounds(char[,] grid) {
        int h = grid.GetLength(0);
        int w = grid.GetLength(1);
        bool[,] outside = new bool[h, w];

        Queue<(int y, int x)> q = [];
        q.Enqueue((0, 0));
        outside[0, 0] = true;
        while (q.Count > 0) {
            var (y, x) = q.Dequeue();
            foreach (var (dx, dy) in Directions) {
                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || ny < 0 || nx >= w || ny >= h)
                    continue;
                if (outside[ny, nx])
                    continue;
                if (grid[ny, nx] == '#' || grid[ny, nx] == 'X')
                    continue;
                outside[ny, nx] = true;
                q.Enqueue((ny, nx));
            }
        }
        bool[,] inside = new bool[h, w];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                inside[y, x] = !outside[y, x];
        return inside;
    }

    static int[,] BuildPrefixSum(bool[,] inside) {
        int h = inside.GetLength(0);
        int w = inside.GetLength(1);
        int[,] ps = new int[h + 1, w + 1];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                ps[y + 1, x + 1] = ps[y, x + 1] + ps[y + 1, x] - ps[y, x] + (inside[y, x] ? 1 : 0);
        return ps;
    }

    static void DrawRect(char[,] grid, Rect r) {
        int x0 = Math.Min(r.A.X, r.B.X);
        int x1 = Math.Max(r.A.X, r.B.X);
        int y0 = Math.Min(r.A.Y, r.B.Y);
        int y1 = Math.Max(r.A.Y, r.B.Y);

        for (int y = y0; y <= y1; y++)
            for (int x = x0; x <= x1; x++)
                grid[y, x] = 'O';
    }

    // ONLY USABLE WITH TEST INPUT
    static void PrintGrid(char[,] grid) {
        int gridHeight = grid.GetLength(0);
        int gridWidth = grid.GetLength(1);
        for (int i = 0; i < gridHeight; i++) {
            for (int j = 0; j < gridWidth; j++)
                Console.Write(grid[i, j]);
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
