using Advent;

namespace test;

public class AdventMoreIntelligenterGridTests
{
    private readonly ITestOutputHelper logger;

    public AdventMoreIntelligenterGridTests(ITestOutputHelper logger) {
        this.logger = logger;
    }

    [Fact]
    public void TestFindMatches()
    {
        string toCheck = "XMAS";
        string[] haystack = ["...XMAS..."];

        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal(1, grid.LineMatches(haystack[0], toCheck));
    }

    [Fact]
    public void TestFindsBackwardsMatches()
    {
        string toCheck = "XMAS";
        string[] haystack = ["...SAMX..."];

        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal(1, grid.LineMatches(haystack[0], toCheck));
    }

    [Fact]
    public void TestFindsHorizMatchesInGrad()
    {
        string toCheck = "XMAS";
        string[] haystack = [
            "...SAMXMAS...",
        ];

        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal(2, grid.LineMatches(haystack[0], toCheck));
    }

    [Fact]
    public void TestTurnColumnsIntoRows()
    {
        string[] haystack = [
            "..",
            "..",
            "S.",
            "A.",
            "M.",
            "X.",
            "M.",
            "A.",
            "S.",
            ".."
        ];
        string expected = "..SAMXMAS.";

        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);
        char[] newGrid = grid.TransposeRow(0);

        Assert.Equal(expected, new string(newGrid));
        newGrid = grid.TransposeRow(1);
        Assert.Equal("..........", new string(newGrid));
    }
}
