using Advent;

namespace PartOne.Test;

public class UnitTest1
{
    [Fact]
    public void TestEnsureRulesPopulateCorrectly()
    {
        string[] unparsedRules = [
            "47|53",
            "97|13",
            "97|61",
            "97|47",
            "75|29",
        ];
        Rules rules = new Rules(unparsedRules);

        Assert.Equal([97], rules._Rules[47].Before);
        Assert.Equal([53], rules._Rules[47].After);
        Assert.Equal([13, 61, 47], rules._Rules[97].After);
    }

    [Fact]
    public void TestIfSortedCorrectly()
    {
        string[] unparsedRules = [
            "47|53",
            "97|13",
            "97|61",
            "97|47",
            "75|29",
            "61|13",
            "75|53",
            "29|13",
            "97|29",
            "53|29",
            "61|53",
            "97|53",
            "61|29",
            "47|13",
            "75|47",
            "97|75",
            "47|61",
            "75|61",
            "47|29",
            "75|13",
            "53|13",
        ];
        Rules rules = new Rules(unparsedRules);

        Assert.True(rules.IsSorted("75,47,61,53,29"));
        Assert.False(rules.IsSorted("75,97,47,61,53"));
    }

    [Fact]
    public void SumSortedListTest()
    {
        string[] unparsedRules = [
            "47|53",
            "97|13",
            "97|61",
            "97|47",
            "75|29",
            "61|13",
            "75|53",
            "29|13",
            "97|29",
            "53|29",
            "61|53",
            "97|53",
            "61|29",
            "47|13",
            "75|47",
            "97|75",
            "47|61",
            "75|61",
            "47|29",
            "75|13",
            "53|13",
        ];
        Rules rules = new Rules(unparsedRules);
        string[] corrections = [
            "75,47,61,53,29",
            "97,61,53,29,13",
            "75,29,13",
            "75,97,47,61,53",
            "61,13,29",
            "97,13,75,29,47",
        ];

        int sum = Program.SumMiddleOfSorted(rules, corrections);
        Assert.Equal(143, sum);
    }
}
