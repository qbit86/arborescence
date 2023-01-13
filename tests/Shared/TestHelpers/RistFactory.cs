namespace Arborescence;

using System.Collections.Generic;
using Misnomer;

internal static class RistFactory<T>
{
    internal static Rist<T> Create<TEnumerator>(TEnumerator enumerator, int capacity = 0)
        where TEnumerator : IEnumerator<T>
    {
        Rist<T> result = new(capacity);
        while (enumerator.MoveNext())
            result.Add(enumerator.Current);

        return result;
    }
}
