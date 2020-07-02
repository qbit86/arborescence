namespace Arborescence
{
    using System.Collections.Generic;

    internal static class ListExtensions
    {
        internal static void AddEnumerator<TList, TEnumerator>(this TList list, TEnumerator enumerator)
            where TList : IList<int>
            where TEnumerator : IEnumerator<int>
        {
            if (list is null || enumerator is null)
                return;

            while (enumerator.MoveNext())
                list.Add(enumerator.Current);
        }
    }
}
