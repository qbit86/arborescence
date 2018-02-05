// ReSharper disable FieldCanBeMadeReadOnly.Local
namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TColorMapFactory _colorMapFactory;

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public BaselineDfs(TColorMapFactory colorMapFactory)
        {
            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            VertexConcept = default(TVertexConcept);
            EdgeConcept = default(TEdgeConcept);
            _colorMapFactory = colorMapFactory;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(graph, startVertex,
                VertexConcept, EdgeConcept, _colorMapFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)

            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(graph, vertexEnumerator,
                VertexConcept, EdgeConcept, _colorMapFactory);
        }
    }
}
