namespace Ubiquitous
{
    using System.Collections.Generic;

    public struct SimpleIncidenceVertexInstance<TEdge> : IIncidenceVertexConcept<TEdge, IEnumerable<TEdge>>
    {
        public IEnumerable<TEdge> GetOutEdges(IEnumerable<TEdge> vertexData) => vertexData;
    }
}
