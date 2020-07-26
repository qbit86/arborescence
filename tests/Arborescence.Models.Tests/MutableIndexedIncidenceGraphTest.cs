namespace Arborescence
{
    using System.Collections.Generic;
    using Graph = Models.MutableIndexedIncidenceGraph;
    using EdgeEnumerator = ArrayPrefixEnumerator<int>;

    public sealed class MutableIndexedIncidenceGraphTest
    {
        private static IEqualityComparer<HashSet<Endpoints>> HashSetEqualityComparer { get; } =
            HashSet<Endpoints>.CreateSetComparer();
    }
}
