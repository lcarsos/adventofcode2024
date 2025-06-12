using System;

namespace Advent {
    class Program {
        static int Main(string[] args) {
            if (args.Length < 1) {
                Console.Error.WriteLine("provide filename as arg 1");
                return 1;
            }

            string[] input = File.ReadAllLines(args[0]);
            Console.WriteLine($"Length: {input[0].Length}");
            Console.WriteLine($"3,4: {input[3][4]}");
            return 0;
        }
    }

    public enum Direction {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    public record Point {
        public int x { get; init; }
        public int y { get; init; }

        public sealed override string ToString() => $"({x}, {y})";

        public Point Move(Direction dir) =>
            dir switch
            {
                Direction.N => new Point { x = x - 1, y = y },
                Direction.NE => new Point { x = x - 1, y = y + 1 },
                Direction.E => new Point { x = x, y = y + 1 },
                Direction.SE => new Point { x = x + 1, y = y + 1 },
                Direction.S => new Point { x = x + 1, y = y },
                Direction.SW => new Point { x = x + 1, y = y - 1 },
                Direction.W => new Point { x = x, y = y - 1 },
                Direction.NW => new Point { x = x - 1, y = y - 1 },
                _ => throw new ArgumentException("Foobar", nameof(dir)),
        };
    }

    public class Grid {
        private readonly string[] m_grid;
        public int columns { get; }
        public int rows { get; }

        public Grid(string[] grid) {
            m_grid = grid;
            rows = m_grid.Length;
            columns = m_grid[0].Length;
        }

        public char? Cell(int x, int y) {
            if (x < 0 || x >= rows) return null;
            if (y < 0 || y >= columns) return null;

            return m_grid[x][y];
        }

        public char? Cell(Point pt) {
            return Cell(pt.x, pt.y);
        }

        public char?[] Neighbors(int x, int y) {
            char?[] neighbors = new char?[8];
            neighbors[(int)Direction.N] ??= Cell(x - 1, y);
            neighbors[(int)Direction.NE] ??= Cell(x - 1, y + 1);
            neighbors[(int)Direction.E] ??= Cell(x, y + 1);
            neighbors[(int)Direction.SE] ??= Cell(x + 1, y + 1);
            neighbors[(int)Direction.S] ??= Cell(x + 1, y);
            neighbors[(int)Direction.SW] ??= Cell(x + 1, y - 1);
            neighbors[(int)Direction.W] ??= Cell(x, y - 1);
            neighbors[(int)Direction.NW] ??= Cell(x - 1, y - 1);
            return neighbors;
        }

        public bool DoesMatch(Point start, Direction heading, string match) {
            Point stand = start;
            for (int i = 0; i < match.Length; i++) {
                if (Cell(stand) == null || Cell(stand) != match[i]) {
                    return false;
                }
                stand = stand.Move(heading);
            }
            return true;
        }

        public int CountMatches(string toCheck) {
            int count = 0;
            for (int i = 0; i < grid.rows; i++) {
                for (int j = 0; j < grid.columns; j++) {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                        Point pt = new Point { x = i, y = j };
                        if (grid.DoesMatch(pt, dir, toCheck)) {
                            count += 1;
                        }
                    }
                }
            }
        }
    }
}
