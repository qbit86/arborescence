namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
