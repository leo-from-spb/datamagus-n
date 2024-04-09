using System.Collections.Generic;
using System.Linq;

namespace Util.Common.Fun;

public static class AnyExt
{

    public static T? takeIf<T>(this T? value, bool condition) => condition ? value : default(T?);

    public static bool isIn<T>(this T? item, ICollection<T> collection) => item is not null && collection.Contains(item);
    public static bool isIn<T>(this T? item, T[] array) => item is not null && array.Contains(item);

    public static bool isNotIn<T>(this T item, ICollection<T> collection) => !collection.Contains(item);
    public static bool isNotIn<T>(this T item, T[] array) => !array.Contains(item);

}