namespace Ubiquitous.Traversal
{
    public interface IVertexStepPolicy<in TStepKind, in TVertex, out TStep>
    {
        TStep CreateVertexStep(TStepKind kind, TVertex vertex);
    }

    public interface IEdgeStepPolicy<in TStepKind, in TEdge, out TStep>
    {
        TStep CreateEdgeStep(TStepKind kind, TEdge edge);
    }

    public interface IStepPolicy<in TStepKind, in TVertex, in TEdge, out TStep> :
        IVertexStepPolicy<TStepKind, TVertex, TStep>, IEdgeStepPolicy<TStepKind, TEdge, TStep> { }
}
