namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;

    internal readonly struct DummyEqualityComparer<TCost, TCostComparer> : IEquatable<TCost>
        where TCostComparer : IComparer<TCost>
    {
        private readonly TCost _dummy;
        private readonly TCostComparer _comparer;

        public DummyEqualityComparer(TCost dummy, TCostComparer comparer)
        {
            if (comparer == null)
                ThrowHelper.ThrowArgumentNullException(nameof(comparer));

            _dummy = dummy;
            _comparer = comparer;
        }

        public bool Equals(TCost other) => _comparer.Compare(_dummy, other) == 0;
    }
}
