namespace Ubiquitous.Traversal
{
    using IndexedDfsStep = DfsStep<int>;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedDfsStepPolicy
        : IStepPolicy<DfsStepKind, int, int, IndexedDfsStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public IndexedDfsStep CreateVertexStep(DfsStepKind kind, int vertex)
        {
            return new IndexedDfsStep(kind, vertex);
        }

        public IndexedDfsStep CreateEdgeStep(DfsStepKind kind, int edge)
        {
            return new IndexedDfsStep(kind, edge);
        }
    }
}
