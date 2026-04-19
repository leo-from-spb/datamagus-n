using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util.Extensions;

namespace Model.Abstracts;

public class ModFamily<M> : Family<M>
    where M: class, Matter
{
    private readonly Family<M>? OriginFamily;
    private readonly List<M>    Matters;


    public ModFamily(Family<M> originFamily)
    {
        this.OriginFamily = originFamily;
        this.Matters      = new List<M>(originFamily.AsList());
    }

    public ModFamily()
    {
        this.OriginFamily = null;
        this.Matters      = new List<M>();
    }


    public bool Any     => Matters.IsNotEmpty();
    public bool IsEmpty => Matters.IsEmpty();
    public int  Count   => Matters.Count;

    public M?     ById(uint id) => Matters.FirstOrDefault(m => m.Id == id, defaultValue: null);
    public uint[] GetAllIds()   => Matters.Select(m => m.Id).ToArray();

    public M[]              ToArray() => Matters.ToArray();
    public IReadOnlyList<M> AsList()  => Matters;

    public IEnumerator<M>   GetEnumerator() => Matters.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Matters.GetEnumerator();
    
}
