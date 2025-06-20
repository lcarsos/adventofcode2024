namespace Advent;

public class Rules
{
    public Dictionary<int, Node> _Rules { get; }

    public Rules(string[] ruleLines)
    {
        _Rules = new Dictionary<int, Node>();

        foreach (string line in ruleLines)
        {
            var parts = line.Split('|');
            int before = Int32.Parse(parts[0]);
            int after = Int32.Parse(parts[1]);
            if (_Rules.ContainsKey(before))
            {
                if (!_Rules[before].After.Contains(after))
                {
                    _Rules[before].After.Add(after);
                }
            }
            else
            {
                Node node = new Node(before);
                node.After.Add(after);
                _Rules.Add(before, node);
            }
            if (_Rules.ContainsKey(after))
            {
                if (!_Rules[after].Before.Contains(before))
                {
                    _Rules[after].Before.Add(before);
                }
            }
            else
            {
                Node node = new Node(after);
                node.Before.Add(before);
                _Rules.Add(after, node);
            }
        }
    }

    public bool IsSorted(string stringPages)
    {
        var pages = stringPages.Split(',').Select(Int32.Parse).ToList();

        foreach ((int i, int page) in pages.Select((x, i) => (i, x)))
        {
            if (!_Rules.ContainsKey(page))
            {
                continue;
            }

            // If the subset i .. N contains anything in [i].Before list
            var toEnd = pages.Slice(i, pages.Count - i);
            var hits = _Rules[page].Before.Intersect(toEnd);
            if (hits.Count() > 0)
            {
                return false;
            }
        }

        return true;
    }

    public int ComparePages(int lhs, int rhs)
    {
        if (!(_Rules.ContainsKey(lhs) && _Rules.ContainsKey(rhs)))
        {
            return 0;
        }
        if (_Rules[lhs].After.Contains(rhs))
        {
            return -1;
        }
        if (_Rules[lhs].Before.Contains(rhs))
        {
            return 1;
        }
        return 0;
    }
}
