using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Text;


/// <summary>
/// Horizontal alignment of a cell value within its column in a <see cref="TextTable{R}"/>.
/// </summary>
public enum TextColumnAlign
{
    /// <summary>The value is padded with spaces on the right.</summary>
    tcaLeft,
    /// <summary>The value is padded with spaces on the left.</summary>
    tcaRight
}


/// <summary>
/// Visual frame of a <see cref="TextTable{R}"/>.
/// <para>
/// Each of the horizontal rule strings (<see cref="TopLine"/>, <see cref="HeadLine"/>,
/// <see cref="BottomLine"/>) is a <em>pattern</em> that is replicated horizontally to
/// match the table width. Setting any of them to <c>null</c> suppresses the corresponding
/// rule entirely; a typical "ASCII art" frame uses <c>"=="</c> for the top rule and
/// <c>"--"</c> for the header/bottom rules.
/// </para>
/// <para>
/// <see cref="CellGap"/>, <see cref="LeftLine"/> and <see cref="RightLine"/> are printed
/// verbatim around and between cells; they are <em>not</em> replicated and they participate
/// in the total table width calculation, so the horizontal rules cover them too.
/// </para>
/// <para>
/// Instances are immutable; tweak settings with a <c>with</c> expression, e.g.
/// <c>look.Grid = look.Grid with { LeftLine = "! ", RightLine = " !" };</c>
/// </para>
/// </summary>
/// <param name="TopLine">pattern of the top rule; <c>null</c> to omit.</param>
/// <param name="HeadLine">pattern of the rule printed between the header and the body; <c>null</c> to omit.</param>
/// <param name="BottomLine">pattern of the bottom rule; <c>null</c> to omit.</param>
/// <param name="CellGap">separator printed between adjacent cells of every row, e.g. <c>"  "</c> or <c>" ! "</c>.</param>
/// <param name="LeftLine">prefix printed before the first cell of every row, e.g. <c>"! "</c>.</param>
/// <param name="RightLine">suffix printed after the last cell of every row, e.g. <c>" !"</c>.</param>
public record TextTableGrid(string? TopLine    = "==",
                            string? HeadLine   = "--",
                            string? BottomLine = "--",
                            string  CellGap    = "  ",
                            string  LeftLine   = "",
                            string  RightLine  = "");


/// <summary>
/// Mutable holder of the rendering options of a <see cref="TextTable{R}"/> instance.
/// </summary>
public class TextTableLook
{
    /// <summary>
    /// The active <see cref="TextTableGrid"/> configuration; replace by assigning a modified copy,
    /// e.g. <c>look.Grid = look.Grid with { CellGap = " | " };</c>
    /// </summary>
    public TextTableGrid Grid { get; set; } = new TextTableGrid();

    /// <summary>
    /// String prepended to every output line of the table, useful to indent the whole table;
    /// <c>null</c> means no indent.
    /// </summary>
    public string? TableIndent { get; set; } = null;
}


/// <summary>
/// Renders a small in-memory table of rows as plain monospace text.
///
/// <para>
/// A <see cref="TextTable{R}"/> is a generic builder parameterized by the row type <c>R</c>.
/// Columns are described one by one via the fluent <see cref="Column(string?, Func{R, string?})"/>
/// method, which returns the table itself so calls can be chained. Each column knows how to
/// extract the textual value of a cell from a row via its fetcher function and how to align
/// that value within the cell.
/// </para>
/// <para>
/// The visual frame (top/header/bottom rule lines, the optional left/right borders and
/// the gap between cells) is configured via the <see cref="Look"/> property, see
/// <see cref="TextTableLook"/> and <see cref="TextTableGrid"/>. Column widths are NOT
/// specified by the caller: they are computed automatically from the header titles and
/// from the actual cell content of all rows, so the table is always tight and aligned
/// without any manual measurement.
/// </para>
/// <para>
/// Rendering is performed by <see cref="ProcessContent(IEnumerable{R})"/>, which traverses
/// the supplied content twice — first to measure column widths and then to print the cells.
/// Therefore the content must be safely re-iterable; transient sequences should be
/// materialized into a list before being passed in. The class itself is stateless across
/// calls (the only mutable state is the column list and the <see cref="Look"/> options)
/// but it is not designed for concurrent use: do not share a single instance between
/// threads while a rendering is in progress.
/// </para>
/// <para>
/// Behaviour of edge cases:
/// <list type="bullet">
///   <item>If a fetcher returns <c>null</c> for a row, the cell is rendered as blank spaces
///         of the column's width.</item>
///   <item>If the fetched value is longer than the measured column width, it is truncated
///         to that width. In practice this can happen only when the value exceeds the header
///         title length and is at the same time longer than every value measured before it,
///         i.e. it normally cannot happen after the measurement pass — the truncation is a safety net.</item>
///   <item>If <em>all</em> columns have a <c>null</c> title, the header row and the
///         <see cref="TextTableGrid.HeadLine"/> rule are omitted; otherwise a <c>null</c>
///         title is rendered as a single space.</item>
/// </list>
/// </para>
/// <para>
/// Typical usage:
/// <code>
/// var text =
///     new TextTable&lt;MyRecord&gt;()
///         .Column("Name",  r =&gt; r.Name)
///         .Column("City",  r =&gt; r.Contact.City)
///         .Column("Count", TextColumnAlign.tcaRight, r =&gt; r.Count.ToString())
///         .WithGrid(new TextTableGrid(CellGap: " ! "))
///         .WithIndent("\t")
///         .ProcessContent(rows);
/// </code>
/// </para>
/// </summary>
/// <typeparam name="R">the row type passed to <see cref="ProcessContent(IEnumerable{R})"/>.</typeparam>
public class TextTable<R>
{

    /// <summary>
    /// Per-column metadata: header title, alignment and the fetcher.
    /// Internal because users describe columns via the fluent
    /// <see cref="TextTable{R}.Column(string?, Func{R, string?})"/> method.
    /// </summary>
    private sealed class Col
    {
        public readonly string?          Title;
        public readonly TextColumnAlign            Alignment;
        public readonly Func<R, string?> Fetcher;
        public readonly int              InitialWidth;

        public Col(string? title, TextColumnAlign alignment, Func<R, string?> fetcher)
        {
            Title        = title;
            Alignment    = alignment;
            Fetcher      = fetcher;
            InitialWidth = title?.Length ?? 0;
        }
    }


    private readonly List<Col> columns = [];

    /// <summary>
    /// Rendering options of this table; mutate before calling
    /// <see cref="ProcessContent(IEnumerable{R})"/>.
    /// </summary>
    public TextTableLook Look { get; } = new TextTableLook();


    /// <summary>
    /// Creates an empty <see cref="TextTable{R}"/>; add columns via
    /// <see cref="Column(string?, Func{R, string?})"/> /
    /// <see cref="Column(string?, TextColumnAlign, Func{R, string?})"/>.
    /// </summary>
    public TextTable() { }


    /// <summary>
    /// Appends a left-aligned column to the table.
    /// </summary>
    /// <param name="title">the column header; <c>null</c> means the column has no title.</param>
    /// <param name="fetcher">function that extracts the cell value from a row;
    ///                       may return <c>null</c>, in which case the cell is rendered as blanks.</param>
    /// <returns>this table, for chaining.</returns>
    public TextTable<R> Column(string? title, Func<R, string?> fetcher) =>
        Column(title, TextColumnAlign.tcaLeft, fetcher);

    /// <summary>
    /// Appends a column to the table.
    /// </summary>
    /// <param name="title">the column header; <c>null</c> means the column has no title.</param>
    /// <param name="alignment">horizontal alignment of cell values.</param>
    /// <param name="fetcher">function that extracts the cell value from a row;
    ///                       may return <c>null</c>, in which case the cell is rendered as blanks.</param>
    /// <returns>this table, for chaining.</returns>
    public TextTable<R> Column(string? title, TextColumnAlign alignment, Func<R, string?> fetcher)
    {
        columns.Add(new Col(title, alignment, fetcher));
        return this;
    }


    /// <summary>
    /// Replaces the visual frame configuration with the given <paramref name="grid"/>.
    /// </summary>
    /// <param name="grid">the new grid; see <see cref="TextTableGrid"/>.</param>
    /// <returns>this table, for chaining.</returns>
    public TextTable<R> WithGrid(TextTableGrid grid)
    {
        Look.Grid = grid;
        return this;
    }

    /// <summary>
    /// Sets the indent string prepended to every output line of the table.
    /// </summary>
    /// <param name="indent">the indent string; <c>null</c> to disable indentation.</param>
    /// <returns>this table, for chaining.</returns>
    public TextTable<R> WithIndent(string? indent)
    {
        Look.TableIndent = indent;
        return this;
    }


    /// <summary>
    /// Renders the supplied <paramref name="content"/> and returns the resulting text.
    ///
    /// Convenience overload that allocates a fresh <see cref="StringBuilder"/>, delegates to
    /// <see cref="ProcessContent(IEnumerable{R}, StringBuilder)"/>, and returns the result as a string.
    /// See that overload for the detailed rendering contract.
    /// </summary>
    /// <param name="content">the rows to render; iterated twice (for width measurement and printing).</param>
    /// <returns>the rendered table as a string.</returns>
    public string ProcessContent(IEnumerable<R> content)
    {
        var b = new StringBuilder();
        ProcessContent(content, b);
        return b.ToString();
    }

    /// <summary>
    /// Renders the supplied <paramref name="content"/> into the given <paramref name="b"/>.
    ///
    /// <para>The method:</para>
    /// <list type="number">
    ///   <item>iterates <paramref name="content"/> once to measure the maximum width of every column
    ///         (initial widths come from the header titles);</item>
    ///   <item>prints the top rule, if <see cref="TextTableGrid.TopLine"/> is non-<c>null</c>;</item>
    ///   <item>prints the header row and the header rule, unless <em>every</em> column has a
    ///         <c>null</c> title;</item>
    ///   <item>iterates <paramref name="content"/> a second time and prints every row, padding or
    ///         truncating each cell to the measured column width and applying the column's
    ///         <see cref="TextColumnAlign"/>;</item>
    ///   <item>prints the bottom rule, if <see cref="TextTableGrid.BottomLine"/> is non-<c>null</c>.</item>
    /// </list>
    /// <para>
    /// Every printed line is prefixed with <see cref="TextTableLook.TableIndent"/> when it is set.
    /// The <see cref="TextTableGrid.LeftLine"/> / <see cref="TextTableGrid.RightLine"/> borders and
    /// the <see cref="TextTableGrid.CellGap"/> separator are printed as-is and are accounted for
    /// when computing the width to which the horizontal rules are replicated.
    /// </para>
    /// <para>
    /// The output always ends with a line separator, so the result of rendering can be safely
    /// concatenated with subsequent text.
    /// </para>
    /// </summary>
    /// <param name="content">the rows to render; must be safely iterable twice.</param>
    /// <param name="b">the destination buffer to which the rendered text is appended.</param>
    /// <exception cref="InvalidOperationException">if no columns have been added.</exception>
    public void ProcessContent(IEnumerable<R> content, StringBuilder b)
    {
        int columnCnt = columns.Count;
        if (columnCnt == 0)
            throw new InvalidOperationException("A table must have at least one column");

        TextTableGrid grid = Look.Grid;

        // compute column widths
        int   rowCnt       = 0;
        int[] columnWidths = new int[columnCnt];
        for (int i = 0; i < columnCnt; i++) columnWidths[i] = columns[i].InitialWidth;

        foreach (var row in content)
        {
            rowCnt++;
            for (int i = 0; i < columnCnt; i++)
            {
                string? cellStr = columns[i].Fetcher(row);
                if (cellStr != null)
                {
                    int w = CalculateTextWidth(cellStr);
                    if (columnWidths[i] < w) columnWidths[i] = w;
                }
            }
        }

        // compute sizes
        int  columnWidthSum       = columnWidths.Sum();
        int  tableWidth           = columnWidthSum
                                  + (columnCnt - 1) * grid.CellGap.Length
                                  + grid.LeftLine.Length
                                  + grid.RightLine.Length;
        int  tableWidthWithIndent = tableWidth + (Look.TableIndent?.Length ?? 0);
        int  tableSize            = tableWidthWithIndent * (rowCnt + 4);
        bool hasHeader            = columns.Any(c => c.Title != null);

        // rule lines
        string? topLine    = grid.TopLine    is null ? null : ReplicateTillWidth(grid.TopLine,    tableWidth);
        string? headLine   = grid.HeadLine   is null ? null : ReplicateTillWidth(grid.HeadLine,   tableWidth);
        string? bottomLine = grid.BottomLine is null ? null : ReplicateTillWidth(grid.BottomLine, tableWidth);

        b.EnsureCapacity(b.Length + tableSize);

        // print top rule
        if (topLine != null)
        {
            AppendIndent(b);
            b.Append(topLine);
            b.Append('\n');
        }

        // print header
        if (hasHeader)
        {
            AppendIndent(b);
            b.Append(grid.LeftLine);
            for (int i = 0; i < columnCnt; i++)
            {
                int width = columnWidths[i];
                if (i > 0) b.Append(grid.CellGap);
                string h  = columns[i].Title ?? " ";
                string hp = columns[i].Alignment switch
                            {
                                TextColumnAlign.tcaLeft  => h.PadRight(width),
                                TextColumnAlign.tcaRight => h.PadLeft(width),
                                _             => h
                            };
                b.Append(hp);
            }
            b.Append(grid.RightLine);
            b.Append('\n');
            AppendIndent(b);
            if (headLine != null) b.Append(headLine);
            b.Append('\n');
        }

        // print the main content
        foreach (var row in content)
        {
            AppendIndent(b);
            b.Append(grid.LeftLine);
            for (int i = 0; i < columnCnt; i++)
            {
                if (i > 0) b.Append(grid.CellGap);
                int     width = columnWidths[i];
                string? x     = columns[i].Fetcher(row);
                if (x is not null)
                {
                    int w = x.Length;
                    if (w < width)
                    {
                        if (columns[i].Alignment == TextColumnAlign.tcaRight) b.Append(' ', width - w);
                        b.Append(x);
                        if (columns[i].Alignment == TextColumnAlign.tcaLeft)  b.Append(' ', width - w);
                    }
                    else if (w == width)
                    {
                        b.Append(x);
                    }
                    else
                    {
                        b.Append(x.AsSpan(0, width));
                    }
                }
                else
                {
                    b.Append(' ', width);
                }
            }
            b.Append(grid.RightLine);
            b.Append('\n');
        }

        // print bottom rule
        if (bottomLine != null)
        {
            AppendIndent(b);
            b.Append(bottomLine);
            b.Append('\n');
        }
    }


    private static int CalculateTextWidth(string text) => text.Length;

    private void AppendIndent(StringBuilder b)
    {
        if (Look.TableIndent != null) b.Append(Look.TableIndent);
    }

    private static string ReplicateTillWidth(string pattern, int width)
    {
        int pl = pattern.Length;
        if (pl == width) return pattern;
        if (pl >  width) return pattern[..width];

        var b = new StringBuilder(width + pl);
        while (b.Length < width) b.Append(pattern);
        if (b.Length > width) b.Length = width;
        return b.ToString();
    }
}
