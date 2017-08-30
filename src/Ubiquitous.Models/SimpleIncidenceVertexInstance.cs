namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct SimpleIncidenceVertexInstance<TEdgeKey> : IIncidenceVertexConcept<TEdgeKey, IEnumerable<TEdgeKey>>
    {
        public IEnumerable<TEdgeKey> GetOutEdges(IEnumerable<TEdgeKey> vertexData) => vertexData;
    }
}
