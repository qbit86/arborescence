#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;
    using System.Numerics;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
        where TWeight : IAdditionOperators<TWeight, TWeight, TWeight>, IAdditiveIdentity<TWeight, TWeight> { }
}
#endif
