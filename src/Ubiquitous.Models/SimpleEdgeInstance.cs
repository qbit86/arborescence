namespace Ubiquitous
{
    using System;

    public struct SimpleEdgeInstance<TVertexKey> : IEdgeConcept<TVertexKey, SourceTargetPair<TVertexKey>>
    {
        public TVertexKey GetSource(SourceTargetPair<TVertexKey> edgeData) => edgeData.Source;

        public TVertexKey GetTarget(SourceTargetPair<TVertexKey> edgeData) => edgeData.Target;
    }
}
