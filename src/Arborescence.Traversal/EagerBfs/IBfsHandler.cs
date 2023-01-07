namespace Arborescence.Traversal
{
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/BFSVisitor.html

    /// <summary>
    /// Defines callbacks to be invoked while traversing a graph in a BFS manner.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IBfsHandler<in TGraph, in TVertex, in TEdge>
    {
        /// <summary>
        /// This is invoked when a vertex is encountered for the first time.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="v">The vertex.</param>
        void OnDiscoverVertex(TGraph g, TVertex v);

        /// <summary>
        /// This is invoked on a vertex as it is taken from the frontier.
        /// This happens immediately before <see cref="OnExamineEdge"/> is invoked
        /// on each of the out-edges of vertex <paramref name="v"/>.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="v">The vertex.</param>
        void OnExamineVertex(TGraph g, TVertex v);

        /// <summary>
        /// This is invoked on a vertex after all of its out-edges have been added to the search tree
        /// and all of the adjacent vertices have been discovered
        /// (but before the out-edges of the adjacent vertices have been examined).
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
        /// This is invoked on the subset of non-tree edges whose head is colored gray at the time of examination.
        /// The color gray indicates that the vertex is currently in the queue.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnNonTreeGrayHeadEdge(TGraph g, TEdge e);

        /// <summary>
        /// This is invoked on the subset of non-tree edges whose head is colored black at the time of examination.
        /// The color black indicates that the vertex has been removed from the queue.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="e">The edge.</param>
        void OnNonTreeBlackHeadEdge(TGraph g, TEdge e);
    }
}
