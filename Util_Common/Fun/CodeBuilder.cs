using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common.Fun;


public class CodeBuilder
{
    private StringBuilder Buf = new StringBuilder();
    
    
    // settings

    public string Indentation = "\t";

    
    // state

    public string CurrIndentation = "";

    public Stack<string> Indentations = new Stack<string>();

    
    // Methods

    public void Block(string header, string footer, Action action)
    {
        Append(header);
        Indent();
        try
        {
            action();
        }
        finally
        {
            Unindent();
            Append(footer);
        }
    }
    
    public void Indenting(Action action)
    {
        Indent();
        try
        {
            action();
        }
        finally
        {
            Unindent();
        }
    }
    
    public void Indent()
    {
        Indentations.Push(CurrIndentation);
        CurrIndentation += Indentation;
    }

    public void Unindent()
    {
        EnsureEOL();
        CurrIndentation = Indentations.Pop();
    }


    public void Phrase(params string?[] words)
    {
        // check whether at least one word exists
        bool exists = words.OfType<string>().Any();
        if (!exists) return;
        // add all that not null
        Buf.Append(CurrIndentation);
        bool begin = true;
        foreach (string? word in words)
        {
            if (word is null) continue;
            if (begin) begin = false;
            else Buf.Append(' ');
            Buf.Append(word);
        }
        Buf.AppendLine();
    }


    public void Append(string? text)
    {
        if (text is null) return;
        if (text.Contains('\n'))
        {
            var lines = text.Split('\n');
            int n     = lines.Length;
            for (var i = 0; i < n; i++)
            {
                var line = lines[i];
                if (i + 1 == n && line == "") break;
                Buf.Append(CurrIndentation).AppendLine(line);
            }
        }
        else
        {
            Buf.Append(CurrIndentation).AppendLine(text);
        }
    }

    public void EmptyLine()
    {
        EnsureEOL();
        Buf.AppendLine();
    }

    public void EnsureEOL()
    {
        if (Buf.Length == 0) return;
        if (LastChar != '\n') Buf.Append('\n');
    }

    public string Result => Buf.ToString();

    public char LastChar
    {
        get
        {
            int n = Buf.Length;
            return n > 0 ? Buf[n - 1] : '\0';
        }
    }
    
        
}