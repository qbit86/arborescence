namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class IndexedGraphTestCollection : IEnumerable<object[]>
    {
        private const int LowerBound = 1;
        private const int UpperBound = 10;

        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = LowerBound; i < UpperBound; ++i)
                yield return new object[] { i.ToString("D2", CultureInfo.InvariantCulture) };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
