#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Numerics;

    /// <summary>
    /// Represents Dijkstra's algorithm for solving the single source shortest paths problem
    /// when the weight has the usual additive semantics.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TWeight">The type of edge weight.</typeparam>
    public static partial class AdditiveEnumerableDijkstra<TVertex, TWeight>
        where TVertex : notnull
        where TWeight : IAdditionOperators<TWeight, TWeight, TWeight>, IAdditiveIdentity<TWeight, TWeight> { }
}
#endif
