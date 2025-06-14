using Advent;

namespace test;

public class AdventGridTests
{
    private readonly ITestOutputHelper logger;

    public AdventGridTests(ITestOutputHelper logger) {
        this.logger = logger;
    }

    [Fact]
    public void TestNeighbors()
    {
        TurtleGrid grid = new TurtleGrid([
            "abcde",
            "fghij",
            "klmno",
        ]);
        Assert.Equal(['c', 'd', 'i', 'n', 'm', 'l', 'g', 'b'], grid.Neighbors(1, 2));
        Assert.Equal(['f', 'g', 'l', null, null, null, null, null], grid.Neighbors(2, 0));
    }

    [Fact]
    public void TestMovingPoints()
    {
        Point pt = new Point { x = 2, y = 2 };
        Point newPt = pt.Move(Direction.N);

        Assert.Equal(new Point { x = 1, y = 2 }, newPt);

        TurtleGrid grid = new TurtleGrid([
            ".Sl..",
            "deA..",
            "...M.",
            "....X",
        ]);
        Point stand = new Point { x = 0, y = 0 };
        stand = stand.Move(Direction.E);
        Assert.Equal(grid.Cell(stand), 'S');
        stand = stand.Move(Direction.E);
        Assert.Equal(grid.Cell(stand), 'l');
        stand = stand.Move(Direction.S);
        Assert.Equal(grid.Cell(stand), 'A');
    }

    [Fact]
    public void CheckWalkForwardTest()
    {
        string toCheck = "XMAS";
        TurtleGrid grid = new TurtleGrid([
            ".XMAS..",
        ]);

        Assert.True(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.N, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 0 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 2 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.W, toCheck));

        grid = new TurtleGrid([
            ".SA..",
            "deAM.",
            "...MX",
            "....X",
        ]);
        Assert.True(grid.DoesMatch(new Point { x = 3, y = 4 }, Direction.NW, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 2, y = 3 }, Direction.NW, toCheck));
    }

    [Fact]
    public void TestMoving()
    {
        TurtleGrid grid = new TurtleGrid([
            "abc",
            "fgh",
            "klm",
        ]);
        Point stand = new Point { x = 1, y = 1 };

        Assert.Equal('b', grid.Cell(stand.Move(Direction.N)));
        Assert.Equal('h', grid.Cell(stand.Move(Direction.E)));
        Assert.Equal('l', grid.Cell(stand.Move(Direction.S)));
        Assert.Equal('f', grid.Cell(stand.Move(Direction.W)));

        Assert.Equal('a', grid.Cell(stand.Move(Direction.NW)));
    }

    [Fact]
    public void CountInputStringsTest()
    {
        string toCheck = "XMAS";
        TurtleGrid grid = new TurtleGrid([
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

        Assert.Equal(18, grid.CountMatches(toCheck));
    }
}
