namespace Arborescence.Traversal
{
    // https://www.boost.org/doc/libs/1_73_0/libs/graph/doc/DFSVisitor.html

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

        void OnExamineEdge(TGraph g, TEdge e);
        void OnTreeEdge(TGraph g, TEdge e);
        void OnBackEdge(TGraph g, TEdge e);
        void OnForwardOrCrossEdge(TGraph g, TEdge e);
        void OnFinishEdge(TGraph g, TEdge e);
    }
}
