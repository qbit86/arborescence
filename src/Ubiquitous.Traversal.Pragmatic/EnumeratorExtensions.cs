namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections.Generic;

    internal static class EnumeratorExtensions
    {
        internal static bool TryMoveNext<T, TEnumerator>(this TEnumerator enumerator, out T current)
            where TEnumerator : IEnumerator<T>
        {
            if (enumerator == null)
                throw new ArgumentException(nameof(enumerator));

            bool result = enumerator.MoveNext();

            current = result ? enumerator.Current : default(T);

            return result;
        }
    }
}
