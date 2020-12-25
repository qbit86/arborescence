namespace Arborescence.Traversal
{
    // https://www.boost.org/doc/libs/1_75_0/libs/graph/doc/DFSVisitor.html

    /// <summary>
    /// Defines callbacks to be invoked while traversing a graph in a DFS manner.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IDfsHandler<in TGraph, in TVertex, in TEdge>
    {
        /// <summary>
        /// This is invoked on the source vertex once before the start of the search.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="v">The vertex.</param>
        void OnStartVertex(TGraph g, TVertex v);

        /// <summary>
        /// This is invoked when a vertex is encountered for the first time.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="v">The vertex.</param>
        void OnDiscoverVertex(TGraph g, TVertex v);

        /// <summary>
        /// This is invoked on vertex <paramref name="v"/> after <see cref="OnFinishVertex"/> has been called
        /// for all the vertices in the DFS-tree rooted at vertex <paramref name="v"/>.
        /// If vertex <paramref name="v"/> is a leaf in the DFS-tree,
        /// then the <see cref="OnFinishVertex"/> function is called on <paramref name="v"/>
        /// after all the out-edges of <paramref name="v"/> have been examined.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="v">The vertex.</param>
        void OnFinishVertex(TGraph g, TVertex v);

        /// <summary>
        /// This is invoked on every out-edge of each vertex after it is discovered.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnExamineEdge(TGraph g, TEdge e);

        /// <summary>
        /// This is invoked on each edge as it becomes a member of the edges that form the search tree.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnTreeEdge(TGraph g, TEdge e);

        /// <summary>
        /// This is invoked on the back edges in the graph.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnBackEdge(TGraph g, TEdge e);

        /// <summary>
        /// This is invoked on forward or cross edges in the graph. In an undirected graph this method is never called.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnForwardOrCrossEdge(TGraph g, TEdge e);

        /// <summary>
        /// This is invoked on each non-tree edge as well as on each tree edge
        /// after <see cref="OnFinishVertex"/> has been called on its head.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnFinishEdge(TGraph g, TEdge e);
    }
}
