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
        Grid grid = new Grid([
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

        Grid grid = new Grid([
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
        Grid grid = new Grid([
            ".XMAS..",
        ]);

        Assert.True(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.N, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 0 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 2 }, Direction.E, toCheck));
        Assert.False(grid.DoesMatch(new Point { x = 0, y = 1 }, Direction.W, toCheck));

        grid = new Grid([
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
        Grid grid = new Grid([
            "abc",
            "fgh",
            "klm",
        ]);
        Point stand = new Point { x = 1, y = 1 };

        Assert.Equal(grid.Cell(stand.Move(Direction.N)), 'b');
        Assert.Equal(grid.Cell(stand.Move(Direction.E)), 'h');
        Assert.Equal(grid.Cell(stand.Move(Direction.S)), 'l');
        Assert.Equal(grid.Cell(stand.Move(Direction.W)), 'f');

        Assert.Equal(grid.Cell(stand.Move(Direction.NW)), 'a');
    }

    [Fact]
    public void CountInputStringsTest()
    {
        string toCheck = "XMAS";
        Grid grid = new Grid([
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
        Assert.Equal(18, count);
    }
}
