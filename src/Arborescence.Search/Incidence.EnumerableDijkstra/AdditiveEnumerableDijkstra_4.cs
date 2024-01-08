#if NET7_0_OR_GREATER
namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;
    using System.Numerics;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TWeight : IAdditionOperators<TWeight, TWeight, TWeight>, IAdditiveIdentity<TWeight, TWeight> { }
}
#endif
