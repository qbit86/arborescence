namespace Ubiquitous.Workbench
{
    using System.Collections;
    using System.Collections.Generic;

    public static class OneTimeEnumerable
    {
        public static OneTimeEnumerable<T, TEnumerator> Create<T, TEnumerator>(TEnumerator enumerator)
            where TEnumerator : IEnumerator<T>
        {
            return new OneTimeEnumerable<T, TEnumerator>(enumerator);
        }
    }

    public sealed class OneTimeEnumerable<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        private readonly TEnumerator _enumerator;

        public OneTimeEnumerable(TEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }
    }
}
