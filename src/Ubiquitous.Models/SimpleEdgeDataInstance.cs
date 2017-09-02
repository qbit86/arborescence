namespace Ubiquitous
{
    public struct SimpleEdgeDataInstance<TVertex> : IEdgeDataConcept<TVertex, SourceTargetPair<TVertex>>
    {
        public TVertex GetSource(SourceTargetPair<TVertex> edgeData) => edgeData.Source;

        public TVertex GetTarget(SourceTargetPair<TVertex> edgeData) => edgeData.Target;
    }
}
