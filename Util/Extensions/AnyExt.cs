using System.Collections.Generic;
using System.Linq;

namespace Util.Extensions;

public static class AnyExt
{

    public static T? TakeIf<T>(this T? value, bool condition) => condition ? value : default(T?);

    public static bool IsIn<T>(this T item, IEnumerable<T> collection) => collection.Contains(item);

    public static bool IsNotIn<T>(this T item, IEnumerable<T> collection) => !collection.Contains(item);

}