using System.Text.RegularExpressions;

namespace Advent;

public class MoreIntelligenterGrid {
    private string[] m_grid;

    /**
     * A matrix flipped along its diagonal from (0,0) to (N,M)
     * \ABC
     * .\DE
     * ..\F
     * ...\
     */
    private string[] m_transpose;

    /**
     * A matrix sliced from the Top Left:
     * .///
     * ////
     * ////
     * ///.
     */
    private string[] m_TLdiagonal;

    /**
     * A matrix sliced from the Top Right:
     * \\\.
     * \\\\
     * \\\\
     * .\\\
     */
    private string[] m_TRdiagonal;
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

        int diagRows = rows + cols - 1;
        int[] diagRowLengths = new int[diagRows];
        int minRowsCols = rows < cols ? rows : cols;
        for (int lenIdx = 0; lenIdx <= diagRows / 2; lenIdx++) {
            int length = lenIdx + 1;
            int clampedLength = length < minRowsCols ? length: minRowsCols;
            diagRowLengths[lenIdx] = clampedLength;
            diagRowLengths[diagRows - lenIdx - 1] = clampedLength;
        }
        int[] startIdxs = new int[diagRows];
        for (int rowIdx = 0; rowIdx < cols; rowIdx++) {
            startIdxs[rowIdx] = rowIdx;
        }
        for (int startIdx = cols; startIdx < diagRows; startIdx++) {
            startIdxs[startIdx] = (startIdx - cols + 2) * cols - 1;
        }
        m_TLdiagonal = new string[diagRows];
        for (int i = 0; i < diagRows; i++) {
            int row = i < cols ? 0 : i / cols;
            int len = diagRowLengths[i];
            int start = startIdxs[i];

            char[] diag = new char[len];
            for (int j = 0; j < len; j++) {
                diag[j] = gridArray[j * (cols-1) + start];
            }
            m_TLdiagonal[i] = new string(diag);
        }

        for (int diagRow = 0; diagRow < cols; diagRow++) {
            startIdxs[diagRow] = cols - diagRow - 1;
        }
        for (int diagRow = cols; diagRow < diagRows; diagRow++) {
            startIdxs[diagRow] = (diagRow - cols + 1) * cols;
        }
        m_TRdiagonal = new string[diagRows];
        for (int i = 0; i < diagRows; i++) {
            int row = i < cols ? 0 : i / cols;
            int len = diagRowLengths[i];
            int start = startIdxs[i];

            char[] diag = new char[len];
            for (int j = 0; j < len; j++) {
                diag[j] = gridArray[j * (cols+1) + start];
            }
            m_TRdiagonal[i] = new string(diag);
        }
    }

    public string TransposeRow(int rowIdx) {
        return m_transpose[rowIdx];
    }

    public string LDiagonal(int rowIdx) {
        return m_TLdiagonal[rowIdx];
    }

    public string RDiagonal(int rowIdx) {
        return m_TRdiagonal[rowIdx];
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
        var lines = m_grid
            .Concat(m_transpose)
            .Concat(m_TLdiagonal)
            .Concat(m_TRdiagonal).ToArray();
        int count = 0;
        for (int i = 0; i < lines.Length; i++) {
            count += LineMatches(lines[i], needle);
        }
        return count;
    }
}
