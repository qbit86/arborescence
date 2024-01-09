#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;
    using System.Numerics;

    /// <summary>
    /// Represents Dijkstra's algorithm for solving the single source shortest paths problem
    /// when the weight has the usual additive semantics.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    /// <typeparam name="TWeight">The type of edge weight.</typeparam>
    public static partial class AdditiveEnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
        where TWeight : IAdditionOperators<TWeight, TWeight, TWeight>, IAdditiveIdentity<TWeight, TWeight> { }
}
#endif
