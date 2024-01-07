#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Numerics;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TWeight>
        where TVertex : notnull
        where TWeight : IAdditionOperators<TWeight, TWeight, TWeight>, IAdditiveIdentity<TWeight, TWeight> { }
}
#endif
