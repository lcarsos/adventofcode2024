using Advent;

namespace test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Grid grid = Grid([
            "abcde",
            "fghij",
            "klmno",
        ]);
        Assert.Equal([], grid.Neighbors(1, 2));
    }
}
