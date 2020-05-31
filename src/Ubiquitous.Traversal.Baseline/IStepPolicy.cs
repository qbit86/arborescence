namespace Ubiquitous.Traversal
{
    public interface IVertexStepPolicy<in TStepKind, in TVertex, out TStep>
    {
        TStep CreateVertexStep(TStepKind kind, TVertex vertex);
    }

    public interface IUndirectedEdgeStepPolicy<in TStepKind, in TEdge, out TStep>
    {
        TStep CreateEdgeStep(TStepKind kind, TEdge edge, bool isReversed);
    }

    public interface IUndirectedStepPolicy<in TStepKind, in TVertex, in TEdge, out TStep> :
        IVertexStepPolicy<TStepKind, TVertex, TStep>, IUndirectedEdgeStepPolicy<TStepKind, TEdge, TStep> { }
}
