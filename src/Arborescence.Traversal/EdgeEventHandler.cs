namespace Arborescence.Traversal
{
    /// <summary>
    /// Represents the method that will handle the edge event.
    /// </summary>
    /// <param name="g">The graph.</param>
    /// <param name="e">The edge.</param>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public delegate void EdgeEventHandler<in TGraph, in TEdge>(TGraph g, TEdge e);
}
