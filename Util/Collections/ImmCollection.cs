using System;

namespace Util.Collections;


/// <summary>
/// Immutable collection.
/// Any implementation of this interface MUST guarantee
/// that this collection can never be modified.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface ImmCollection<T> : RCollection<T> {}


/// <summary>
/// Immutable list.
/// Any implementation of this interface MUST guarantee
/// that this collection can never be modified.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface ImmList<T> : ImmCollection<T>, RList<T> {}


/// <summary>
/// Immutable set.
/// Any implementation of this interface MUST guarantee
/// that this collection can never be modified.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface ImmSet<T> : ImmCollection<T>, RSet<T> {}


/// <summary>
/// Immutable set where the elements stay in a stable predicted order.
/// Any implementation of this interface MUST guarantee
/// that this collection can never be modified.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface ImmOrderSet<T> : ImmSet<T>, ImmList<T>, ROrderSet<T> {}


/// <summary>
/// Immutable set of elements, that are sorted using the native (type-default) order.
/// Any implementation of this interface MUST guarantee
/// that this collection can never be modified.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface ImmSortedSet<T> : ImmOrderSet<T>, RSortedSet<T>
    where T : IComparable<T> {}
    