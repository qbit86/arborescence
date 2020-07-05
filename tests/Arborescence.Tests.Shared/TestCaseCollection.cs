namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

#pragma warning disable CA1812 // TestCaseCollection is an internal class that is apparently never instantiated.
    internal sealed class TestCaseCollection : IEnumerable<object[]>
    {
        private const int LowerBound = 1;
        private const int UpperBound = 10;

        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = LowerBound; i < UpperBound; ++i)
                yield return new object[] { i.ToString("D2", CultureInfo.InvariantCulture) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CA1812 // TestCaseCollection is an internal class that is apparently never instantiated.
}
