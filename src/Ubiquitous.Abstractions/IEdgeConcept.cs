namespace Ubiquitous
{
    public interface IEdgeConcept<TGraph, TVertex, TEdge>
    {
        bool TryGetSource(TGraph graph, TEdge edge, out TVertex source);
        bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target);
    }

    /// <summary>Default concept for the type of edge values.</summary>
    /// <typeparam name="TVertex">The type for vertex representative objects.</typeparam>
    /// <typeparam name="TEdgeData">The type of edge associated data.</typeparam>
    public interface IEdgeDataConcept<TVertex, TEdgeData>
    {
        TVertex GetSource(TEdgeData edgeData);
        TVertex GetTarget(TEdgeData edgeData);
    }
}
