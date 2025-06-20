namespace Advent;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Provide a file of rules and changes");
        }
        string fileName = args[0];

        string[] lines = File.ReadAllLines(fileName);

        int splitIdx = Array.IndexOf(lines, "");
        string[] ruleLines = lines.Take(splitIdx).ToArray();
        string[] corrections = lines.Skip(splitIdx + 1).Take(lines.Length - splitIdx).ToArray();

        Rules rules = new Rules(ruleLines);
        int sum = SumMiddleOfSorted(rules, corrections);
        Console.WriteLine(sum);
    }

    public static int SumMiddleOfSorted(Rules rules, string[] corrections)
    {
        return corrections
            .Where(x => rules.IsSorted(x))
            .Aggregate(0, (acc, line) => {
                var pages = line.Split(',').Select(Int32.Parse).ToList();
                var middleIndex = pages.Count / 2;
                return acc + pages[middleIndex];
            }
        );
    }

    public static int SumMiddleOfUnSorted(Rules rules, string[] corrections)
    {
        return corrections
            .Where(x => !rules.IsSorted(x))
            .Select(x => {
                var pages = x.Split(',').Select(Int32.Parse).ToList();
                pages.Sort(rules.ComparePages);
                return pages;
            })
            .Aggregate(0, (acc, pages) => {
                var middleIndex = pages.Count / 2;
                return acc + pages[middleIndex];
            }
        );
    }
}
