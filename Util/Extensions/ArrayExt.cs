using System;

namespace Util.Extensions;


public static class ArrayExt
{

    public static ArraySegment<T> Segment<T>(this T[] array, int offset, int count) =>
        new ArraySegment<T>(array, offset, count);


}
