namespace Util.Collections.Implementation;

/// <summary>
/// Entry of the hash table.
/// This entry contains hash data and chain link, but doesn't contain a data item itself.
/// </summary>
internal struct HashTableEntry
{

    internal const uint BusyBit       = 0x40000000u;  // bit 30
    internal const uint ContinueBit   = 0x20000000u;  // bit 29
    internal const uint HasNextBit    = 0x10000000u;  // bit 28
    internal const uint NextIndexBits = 0x0FFFFFFFu;  // bits 27..0

    /// <summary>
    /// Bit 31: always zero
    /// Bit 30: 0 — the cell is empty (during constructing only),
    ///         1 — busy.
    /// Bit 29: 0 — it is the first link in the chain, and the hash code true relates to the cell index,
    ///         1 — it's a continuation of another hash code.
    /// Bit 28: 0 — this cell is the last link in the chain,
    ///         1 — there are more chains (and bits 27..0 point to the next link)
    /// Bits 27..0: index of the next link cell.
    /// </summary>
    internal uint Link;

    /// <summary>
    /// Index of the item in the data array.
    /// </summary>
    internal int TargetIndex;



    /// <summary>
    /// Index of the next hash entry.
    /// </summary>
    internal uint NextIndex => Link & NextIndexBits;

    /// <summary>
    /// <b>true</b> means this cell is busy (contains a hash entry);
    /// <b>false</b> means this cell is empty.
    /// </summary>
    internal bool IsBusy => (Link & BusyBit) != 0;

    /// <summary>
    /// <b>false</b> means it is the first link in the chain, and the hash code true relates to the cell index;
    /// <b>true</b> means it's a continuation of another hash code.
    /// </summary>
    internal bool IsContinue => (Link & ContinueBit) != 0;

    /// <summary>
    /// <b>false</b> means this cell is the last link in the chain;
    /// <b>true</b> means there are more chains (and bits 27..0 point to the next link).
    /// </summary>
    internal bool HasNext => (Link & HasNextBit) != 0;


    public override string ToString() =>
        $"[{TargetIndex},{NextIndex},{(IsBusy ? "B" : "")}{(IsContinue? "C" : "")}{(HasNext ? "N" : "")}]";

}
