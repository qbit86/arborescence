namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
