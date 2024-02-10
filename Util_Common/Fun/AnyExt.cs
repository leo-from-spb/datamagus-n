namespace Util.Common.Fun;

public static class AnyExt
{

    public static T? takeIf<T>(this T? value, bool condition) => condition ? value : default(T?);

    public static bool isIn<T>(this T? item, ICollection<T> set) => item is not null && set.Contains(item);

    public static bool isNotIn<T>(this T item, ICollection<T> set) => !set.Contains(item);

}