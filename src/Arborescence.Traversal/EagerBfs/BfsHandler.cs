namespace Arborescence.Traversal
{
    /// <inheritdoc/>
    public sealed class BfsHandler<TGraph, TVertex, TEdge> : IBfsHandler<TGraph, TVertex, TEdge>
    {
        /// <inheritdoc/>
        public void OnDiscoverVertex(TGraph g, TVertex v) => DiscoverVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnExamineVertex(TGraph g, TVertex v) => ExamineVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnFinishVertex(TGraph g, TVertex v) => FinishVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnExamineEdge(TGraph g, TEdge e) => ExamineEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnTreeEdge(TGraph g, TEdge e) => TreeEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnNonTreeGrayHeadEdge(TGraph g, TEdge e) => NonTreeGrayHeadEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnNonTreeBlackHeadEdge(TGraph g, TEdge e) => NonTreeBlackHeadEdge?.Invoke(g, e);

        /// <summary>
        /// Raised when a vertex is encountered for the first time.
        /// </summary>
        public event VertexHandler<TGraph, TVertex> DiscoverVertex;

        /// <summary>
        /// Raised when a vertex is taken from the fringe.
        /// </summary>
        public event VertexHandler<TGraph, TVertex> ExamineVertex;

        /// <summary>
        /// Raised when all of out-edges for a vertex have been added to the search tree
        /// and all of the adjacent vertices have been discovered.
        /// </summary>
        public event VertexHandler<TGraph, TVertex> FinishVertex;

        /// <summary>
        /// Raised for every out-edge of each vertex after it is discovered.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge> ExamineEdge;

        /// <summary>
        /// Raised for each edge as it becomes a member of the search tree.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge> TreeEdge;

        /// <summary>
        /// Raised for a non-tree edge whose head is currently in the queue.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge> NonTreeGrayHeadEdge;

        /// <summary>
        /// Raised for a non-tree edge whose head has been removed from the queue.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge> NonTreeBlackHeadEdge;
    }
}
