using System.Text.RegularExpressions;

namespace Advent;

public class MoreIntelligenterGrid {
    private string[] m_grid;
    private string[] m_transpose;
    private int rows;
    private int cols;

    public MoreIntelligenterGrid(string[] grid) {
        rows = grid.Length;
        cols = grid[0].Length;

        m_grid = grid;
        char[] gridArray = new char[rows * cols];
        for (int i = 0; i < rows; i++) {
            char[] row = grid[i].ToCharArray();
            int source_idx = row.GetLowerBound(0);
            int dest_idx = gridArray.GetLowerBound(0) + (i*cols);
            Array.Copy(row, source_idx, gridArray, dest_idx, cols);
        }

        char[] transposed = new char[rows * cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                transposed[(j * rows) + i] = gridArray[(i * cols) + j];
            }
        }
        m_transpose = new string[cols];
        for (int i = 0; i < cols; i++) {
            char[] newRow = new char[rows];
            Array.Copy(transposed, transposed.GetLowerBound(0) + i * rows, newRow, newRow.GetLowerBound(0), rows);
            m_transpose[i] = new string(newRow);
        }
    }

    public string TransposeRow(int rowIdx) {
        return m_transpose[rowIdx];
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

    public int Matches(string needle) {
        int count = 0;
        for (int i = 0; i < rows; i++) {
            count += LineMatches(m_grid[i], needle);
        }
        for (int i = 0; i < cols; i++) {
            count += LineMatches(m_transpose[i], needle);
        }
        return count;
    }
}
