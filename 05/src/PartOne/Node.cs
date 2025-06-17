namespace Advent;

public class Node
{
    public int Value { get; }
    public List<int> Before { get; set; }
    public List<int> After { get; set; }

    public Node(int val)
    {
        Value = val;
        Before = new List<int>();
        After = new List<int>();
    }
}
