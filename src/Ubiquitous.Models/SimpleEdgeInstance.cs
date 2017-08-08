namespace Ubiquitous
{
    using System;

    public struct SimpleEdgeInstance<TVertexKey> : IEdgeConcept<TVertexKey, SourceTargetPair<TVertexKey>>
    {
        public TVertexKey GetSource(SourceTargetPair<TVertexKey> edgeValue) => edgeValue.Source;

        public TVertexKey GetTarget(SourceTargetPair<TVertexKey> edgeValue) => edgeValue.Target;
    }
}
