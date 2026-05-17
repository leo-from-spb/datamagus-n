using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Extensions;

namespace Gena.Code;


/// <summary>
/// Builder for code text — text with indented blocks like in programming languages.
/// </summary>
public class CodeBuilder
{
    private readonly StringBuilder Buf = new StringBuilder();


    // Settings \\

    public string Indentation = "\t";


    public string GluingAtLeft  = "([";
    public string GluingAtRight = ":;.,?!])";


    // State \\

    private string CurrIndentation = "";

    private readonly Stack<string> Indentations = new Stack<string>();

    /// <summary>
    /// Current line number (starting with 1).
    /// </summary>
    public int CurLine { get; private set; } = 1;

    /// <summary>
    /// Current position in the line (starting with 1).
    /// </summary>
    public int CurPos
    {
        get { return field > 1 ? field : CurrIndentation.Length + 1; }
        private set;
    } = 1;

    /// <summary>
    /// Width of the text: the length of the longest line of the text
    /// (without the NL character at the end).
    /// </summary>
    public int TextWidth { get; private set; } = 0;


    // Auxiliary Classes \\

    public class IndentationBlockMarker : IDisposable
    {
        private readonly CodeBuilder codeBuilder;
        private readonly string?     footer;
        private readonly bool        thenSkipLine;

        internal IndentationBlockMarker(CodeBuilder codeBuilder, string? footer = null, bool theSkipLine = false)
        {
            this.codeBuilder  = codeBuilder;
            this.footer       = footer;
            this.thenSkipLine = theSkipLine;
        }

        #pragma warning disable CA1816
        public void Dispose()
        {
            codeBuilder.Unindent();
            if (footer is not null)
                codeBuilder.Text(footer);
            if (thenSkipLine)
                codeBuilder.EmptyLine();
        }
        #pragma warning restore CA1816
    }


    // Methods \\

    /// <summary>
    /// Indent the block wrapping with optional <paramref name="header"/> and <paramref name="footer"/>.
    /// The header and footer are not indented, only the text generated inside the code block.
    /// </summary>
    /// <param name="header">optional header to include before indentation.</param>
    /// <param name="footer">optional footer to include after indentation.</param>
    /// <param name="withIndentation">unusual indentation, or null for the usual one.</param>
    /// <param name="thenSkipLine">add empty line after the block.</param>
    public IndentationBlockMarker Block(string? header = null,
                                        string? footer = null,
                                        string? withIndentation = null,
                                        bool thenSkipLine = false)
    {
        if (header is not null)
            Text(header);
        Indent(withIndentation);
        return new IndentationBlockMarker(this, footer, thenSkipLine);
    }

    public IndentationBlockMarker CurlyBlock(bool thenSkipLine = false) => Block("{", "}", null, thenSkipLine);

    public IndentationBlockMarker Indenting(int spaces)
        => Indenting(new string(' ', spaces));

    public IndentationBlockMarker Indenting(string? withIndentation = null)
    {
        Indent(withIndentation);
        return new IndentationBlockMarker(this);
    }

    public void Indent(string? withIndentation = null)
    {
        Indentations.Push(CurrIndentation);
        string theIndentation = withIndentation ?? Indentation;
        CurrIndentation += theIndentation;
    }

    public void Unindent()
    {
        EnsureEoL();
        CurrIndentation = Indentations.Pop();
    }

    /// <summary>
    /// Adds one line that contains of the given words separated with spaces.
    /// Null words are ignored.
    /// If all the given words are null — the whole line is ignored.
    /// </summary>
    /// <param name="words">words to add; nulls are ignored.</param>
    /// <seealso cref="Text"/>
    public void Phrase(params string?[] words)
    {
        // check whether at least one word exists
        bool exists = words.OfType<string>().Any();
        if (!exists) return;
        // add all that not null
        BufAppend(CurrIndentation);
        bool begin = true;
        foreach (string? word in words)
        {
            if (word is null || word.IsEmpty) continue;
            bool glue = begin || LastChar.IsIn(GluingAtLeft) || word[0].IsIn(GluingAtRight);
            if (!glue) BufAppend(' ');
            BufAppend(word);
            begin = false;
        }
        BufAppendLine();
    }

    /// <summary>
    /// Adds one or several lines of text.
    /// Each argument is one line, or several lines (they can contain the '\n' character).
    /// </summary>
    /// <param name="text">lines of the text.</param>
    /// <seealso cref="Phrase"/>
    public void Text(params string?[] text)
    {
        foreach (string? str in text)
            if (str is not null)
                AppendText(str);
    }

    /// <summary>
    /// Adds one or several lines of text.
    /// Each argument is one line, or several lines (they can contain the '\n' character).
    /// </summary>
    /// <param name="text">lines of the text.</param>
    /// <seealso cref="Phrase"/>
    public void Text(IEnumerable<string?> text)
    {
        foreach (string? str in text)
            if (str is not null)
                AppendText(str);
    }

    /// <summary>
    /// Adjusts the text according to the current indentation,
    /// and then appends it to the buffer.
    /// </summary>
    /// <param name="text">the text to append, can be multiline.</param>
    private void AppendText(string text)
    {
        if (text.Contains('\n'))
        {
            var lines = text.Split('\n');
            int n     = lines.Length;

            for (var i = 0; i < n; i++)
            {
                var line = lines[i].TrimEnd('\r');
                if (i + 1 == n && line == "") break;
                BufAppend(CurrIndentation);
                BufAppendLine(line);
            }
        }
        else
        {
            BufAppend(CurrIndentation);
            BufAppendLine(text);
        }
    }

    /// <summary>
    /// Appends one empty line.
    /// </summary>
    public void EmptyLine()
    {
        EnsureEoL();
        BufAppendLine();
    }

    /// <summary>
    /// Ensures that the last line has the end-of-line character.
    /// Appends one if not appended yet.
    /// </summary>
    public void EnsureEoL()
    {
        if (Buf.Length == 0) return;
        if (LastChar != '\n') BufAppendLine();
    }

    /// <summary>
    /// The generated text.
    /// </summary>
    public string Result => Buf.ToString();

    /// <summary>
    /// The last character in the buffer.
    /// Or '\0' if the buffer is empty.
    /// </summary>
    public char LastChar
    {
        get
        {
            int n = Buf.Length;
            return n > 0 ? Buf[n - 1] : '\0';
        }
    }


    // Buffer Operations \\

    /// <summary>
    /// Appends a single-line string (must not contain '\n' or '\r') to the buffer
    /// and updates <see cref="CurPos"/> and <see cref="TextWidth"/>.
    /// </summary>
    private void BufAppend(string s)
    {
        if (s.Length == 0) return;
        Buf.Append(s);
        CurPos += s.Length;
        if (CurPos - 1 > TextWidth) TextWidth = CurPos - 1;
    }

    /// <summary>
    /// Appends a single character (must not be '\n' or '\r') to the buffer
    /// and updates <see cref="CurPos"/> and <see cref="TextWidth"/>.
    /// </summary>
    private void BufAppend(char c)
    {
        Buf.Append(c);
        CurPos++;
        if (CurPos - 1 > TextWidth) TextWidth = CurPos - 1;
    }

    /// <summary>
    /// Appends a line terminator and finalizes the current line:
    /// updates <see cref="TextWidth"/>, increments <see cref="CurLine"/>, resets <see cref="CurPos"/>.
    /// </summary>
    private void BufAppendLine()
    {
        Buf.AppendLine();
        int lineLen = CurPos - 1;
        if (lineLen > TextWidth) TextWidth = lineLen;
        CurLine++;
        CurPos = 1;
    }

    /// <summary>
    /// Appends a single-line string (must not contain '\n' or '\r') followed by a line terminator,
    /// then finalizes the current line.
    /// </summary>
    private void BufAppendLine(string s)
    {
        Buf.AppendLine(s);
        int lineLen = CurPos - 1 + s.Length;
        if (lineLen > TextWidth) TextWidth = lineLen;
        CurLine++;
        CurPos = 1;
    }


}
