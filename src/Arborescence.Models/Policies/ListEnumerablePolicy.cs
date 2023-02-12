namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct ListEnumerablePolicy<T> : IEnumerablePolicy<List<T>, List<T>.Enumerator>
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private static readonly List<T> s_empty = new();

        public List<T>.Enumerator GetEnumerator(List<T> collection) => collection.GetEnumerator();

        public List<T>.Enumerator GetEmptyEnumerator() => s_empty.GetEnumerator();
    }
}
