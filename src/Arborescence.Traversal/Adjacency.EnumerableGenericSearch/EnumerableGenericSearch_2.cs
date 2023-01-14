namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
