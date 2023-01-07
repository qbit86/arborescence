namespace Arborescence
{
    /// <summary>
    /// Represents a notion of quiver (a directed graph permitting loops and parallel edges with own identity)
    /// as a set of vertices, a set of edges, and a pair of incidence functions.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Graph_(discrete_mathematics)#Directed_graph"/>
    /// <seealso href="https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)"/>
    /// <seealso href="https://en.wikipedia.org/wiki/Quiver_(mathematics)"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IGraph<TVertex, in TEdge> :
        IHeadIncidence<TVertex, TEdge>, ITailIncidence<TVertex, TEdge> { }
}
