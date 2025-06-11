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

    public class Grid {
        private readonly string[] m_grid;

        public Grid(string[] grid) {
            m_grid = grid;
        }

        
    }
}
