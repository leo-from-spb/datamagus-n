using System.Collections.Generic;

namespace Util.Fun;


public static class CollectionFun
{

    public static IReadOnlyList<T> ListOfNonNull<T>(params T?[] items)
        where T: class
    {
        var list = new List<T>(items.Length);
        foreach (var item in items)
        {
            if (item is not null) list.Add(item);
        }

        return list;
    }

}
