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

        string newGrid = grid.TransposeRow(0);
        Assert.Equal(expected, newGrid);
        newGrid = grid.TransposeRow(1);
        Assert.Equal("..........", newGrid);
    }

    [Fact]
    public void TestEnsureLDiagonalizationWorks()
    {
        string[] haystack = [
            "0ACFI",
            "BDGJL",
            "EHKMN",
        ];
        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal("AB", grid.LDiagonal(1));
        Assert.Equal("IJK", grid.LDiagonal(4));
        Assert.Equal("LM", grid.LDiagonal(5));
    }

    [Fact]
    public void TestEnsureRDiagonalizationWorks()
    {
        string[] haystack = [
            "0ACFI",
            "BDGJL",
            "EHKMN",
        ];
        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal("FL", grid.RDiagonal(1));
        Assert.Equal("0DK", grid.RDiagonal(4));
        Assert.Equal("BH", grid.RDiagonal(5));
    }

    [Fact]
    public void TestFindingHorizAndVertMatches()
    {
        string[] haystack = [
            "X.X..S",
            ".M.MA.",
            "..AMA.",
            "..XS.S",
        ];
        MoreIntelligenterGrid grid = new MoreIntelligenterGrid(haystack);

        Assert.Equal(3, grid.Matches("XMAS"));
    }

    [Fact]
    public void CountInputStringsTest()
    {
        string toCheck = "XMAS";
        MoreIntelligenterGrid grid = new MoreIntelligenterGrid([
            "MMMSXXMASM",
            "MSAMXMSMSA",
            "AMXSXMAAMM",
            "MSAMASMSMX",
            "XMASAMXAMM",
            "XXAMMXXAMA",
            "SMSMSASXSS",
            "SAXAMASAAA",
            "MAMMMXMMMM",
            "MXMXAXMASX",
        ]);

        Assert.Equal(18, grid.Matches(toCheck));
    }
}
