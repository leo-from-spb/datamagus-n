using System;
using System.Collections.Generic;

namespace Util.Structures;

/// <summary>
/// Named thing.
/// Just a container for one thing and its name.
/// </summary>
/// <typeparam name="T">type of the thing.</typeparam>
[Serializable]
public readonly struct Named<T> : IComparable<Named<T>>, IEquatable<Named<T>>
{

    /// <summary>
    /// The thing.
    /// </summary>
    public readonly T Thing;

    /// <summary>
    /// The name of the Thing.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Instantiates the container.
    /// </summary>
    public Named(string name, T thing)
    {
        Thing = thing;
        Name  = name;
    }

    public int CompareTo(Named<T> that) => string.Compare(this.Name, that.Name, StringComparison.Ordinal);

    public          bool Equals(Named<T> that) => this.Name == that.Name && EqualityComparer<T>.Default.Equals(this.Thing, that.Thing);
    public override bool Equals(object?  obj)  => obj is Named<T> that && this.Equals(that);
    public override int  GetHashCode()         => Name.GetHashCode();

    public static   bool operator == (Named<T> a, Named<T> b) => a.Name == b.Name && EqualityComparer<T>.Default.Equals(a.Thing, b.Thing);
    public static   bool operator != (Named<T> a, Named<T> b) => a.Name != b.Name || !EqualityComparer<T>.Default.Equals(a.Thing, b.Thing);


    public override string ToString() => $"{Name} = {Thing}";

    public KeyValuePair<string, T> AsKeyValuePair => new KeyValuePair<string, T>(Name, Thing);

    public void Deconstruct(out string name, out T thing)
    {
        name  = Name;
        thing = Thing;
    }
}



public static class NamedAnyExt
{

    public static Named<T> WithName<T>(this T thing, string name) => new Named<T>(name, thing);

    public static void Add<T>(this IDictionary<string, T> dictionary, Named<T> namedThing) =>
        dictionary.Add(namedThing.Name, namedThing.Thing);

}

