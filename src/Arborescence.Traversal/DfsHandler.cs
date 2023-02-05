namespace Arborescence.Traversal
{
    /// <inheritdoc/>
    public sealed class DfsHandler<TGraph, TVertex, TEdge> : IDfsHandler<TGraph, TVertex, TEdge>
    {
        /// <inheritdoc/>
        public void OnStartVertex(TGraph g, TVertex v) => StartVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnDiscoverVertex(TGraph g, TVertex v) => DiscoverVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnFinishVertex(TGraph g, TVertex v) => FinishVertex?.Invoke(g, v);

        /// <inheritdoc/>
        public void OnExamineEdge(TGraph g, TEdge e) => ExamineEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnTreeEdge(TGraph g, TEdge e) => TreeEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnBackEdge(TGraph g, TEdge e) => BackEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnForwardOrCrossEdge(TGraph g, TEdge e) => ForwardOrCrossEdge?.Invoke(g, e);

        /// <inheritdoc/>
        public void OnFinishEdge(TGraph g, TEdge e) => FinishEdge?.Invoke(g, e);

        /// <summary>
        /// Raised for the source vertex once before the start of the search.
        /// </summary>
        public event VertexHandler<TGraph, TVertex>? StartVertex;

        /// <summary>
        /// Raised when a vertex is encountered for the first time.
        /// </summary>
        public event VertexHandler<TGraph, TVertex>? DiscoverVertex;

        /// <summary>
        /// Raised for a vertex after finished processing all of its descendants.
        /// </summary>
        public event VertexHandler<TGraph, TVertex>? FinishVertex;

        /// <summary>
        /// Raised for every out-edge of each vertex after it is discovered.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge>? ExamineEdge;

        /// <summary>
        /// Raised for each edge as it becomes a member of the search tree.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge>? TreeEdge;

        /// <summary>
        /// Raised for an edge as it is classified as a back edge in the graph.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge>? BackEdge;

        /// <summary>
        /// Raised for an edge as it is classified as a forward or cross edge in the graph.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge>? ForwardOrCrossEdge;

        /// <summary>
        /// Raised for each edge when processing its head is finished.
        /// </summary>
        public event EdgeHandler<TGraph, TEdge>? FinishEdge;
    }
}
