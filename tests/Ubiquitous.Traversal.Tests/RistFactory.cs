namespace Ubiquitous
{
    using System.Collections.Generic;
    using Misnomer;

    internal static class RistFactory<T>
    {
        internal static Rist<T> Create<TEnumerator>(TEnumerator enumerator)
        where TEnumerator: IEnumerator<T>
        {
            var result = new Rist<T>();
            while (enumerator.MoveNext())
                result.Add(enumerator.Current);

            return result;
        }
    }
}
