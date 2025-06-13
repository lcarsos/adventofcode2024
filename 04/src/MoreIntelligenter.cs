using System.Text.RegularExpressions;

namespace Advent;

public class MoreIntelligenterGrid {
    private char[] m_grid;
    private char[] m_transpose;
    private int rows;
    private int cols;

    public MoreIntelligenterGrid(string[] grid) {
        rows = grid.Length;
        cols = grid[0].Length;

        m_grid = new char[rows * cols];
        for (int i = 0; i < rows; i++) {
            char[] row = grid[i].ToCharArray();
            int source_idx = row.GetLowerBound(0);
            int dest_idx = m_grid.GetLowerBound(0) + (i*cols);
            Array.Copy(row, source_idx, m_grid, dest_idx, cols);
        }

        m_transpose = new char[rows * cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                m_transpose[(j * rows) + i] = m_grid[(i * cols) + j];
            }
        }
    }

    public char[] TransposeRow(int rowIdx) {
        char[] row = new char[rows];
        Array.Copy(m_transpose, m_transpose.GetLowerBound(0) + rowIdx * rows, row, row.GetLowerBound(0), rows);
        return row;
    }

    public int LineMatches(string haystack, string needle) {
        if (haystack.Length < needle.Length) {
            return 0;
        }
        Regex forward = new Regex(needle);
        char[] aRev = needle.ToCharArray();
        Array.Reverse(aRev);
        Regex reverse = new Regex(new string(aRev));

        int count = forward.Matches(haystack).Count;
        count += reverse.Matches(haystack).Count;
        return count;
    }
}
