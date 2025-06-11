using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Advent
{
    internal class Program
    {
        static int PartOne(string[] args)
        {
            List<int> nums = new List<int>(0);

            string fileName = args[0];
            string input = File.ReadAllText(fileName);

            string pattern = @"mul\((\d+),(\d+)\)";
            foreach (Match match in Regex.Matches(input, pattern)) {
                int res = Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
                nums.Add(res);
            }
            int sum = 0;
            foreach (int i in nums) {
                sum += i;
            }
            Console.WriteLine(sum);
            return 0;
        }

        static int PartTwo(string[] args)
        {
            List<int> nums = new List<int>(0);

            string fileName = args[0];
            string input = File.ReadAllText(fileName);

            string pattern = @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)";
            bool executing = true;
            foreach (Match match in Regex.Matches(input, pattern)) {
                if (match.Groups[0].Value == "don't()")
                {
                    executing = false;
                }
                else if (match.Groups[0].Value == "do()")
                {
                    executing = true;
                }
                else if (executing)
                {
                    int res = Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
                    nums.Add(res);
                }
            }
            int sum = 0;
            foreach (int i in nums) {
                sum += i;
            }
            Console.WriteLine(sum);
            return 0;
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Must pass in name of file to open for input");
                return 1;
            }

            return PartTwo(args);
        }
    }
}

