using System;
using System.Runtime.CompilerServices;

namespace Util.Extensions;


public static class ArrayExt
{

    public static T First<T>(this T[] array) =>
        array.Length > 0 ? array[0] : throw new IndexOutOfRangeException("The array is empty");

    public static ArraySegment<T> Segment<T>(this T[] array, int offset, int count) =>
        new ArraySegment<T>(array, offset, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array) => new ReadOnlySpan<T>(array);

}
