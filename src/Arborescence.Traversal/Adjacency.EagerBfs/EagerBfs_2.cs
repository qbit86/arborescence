namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
    public static partial class EagerBfs<TVertex, TVertexEnumerator>
        where TVertex : notnull
        where TVertexEnumerator : IEnumerator<TVertex> { }
}
