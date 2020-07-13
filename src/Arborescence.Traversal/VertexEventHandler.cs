namespace Arborescence.Traversal
{
    /// <summary>
    /// Represents the method that will handle the vertex event.
    /// </summary>
    /// <param name="g">The graph.</param>
    /// <param name="v">The vertex.</param>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public delegate void VertexEventHandler<in TGraph, in TVertex>(TGraph g, TVertex v);
}
