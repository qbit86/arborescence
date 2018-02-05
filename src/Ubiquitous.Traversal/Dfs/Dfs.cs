// ReSharper disable FieldCanBeMadeReadOnly.Local
namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
        where TStackFactory : IFactory<TGraph, TStack>
    {
        private TColorMapFactory _colorMapFactory;
        private TStackFactory _stackFactory { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public Dfs(TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            if (stackFactory == null)
                throw new ArgumentNullException(nameof(stackFactory));

            VertexConcept = default(TVertexConcept);
            EdgeConcept = default(TEdgeConcept);
            _colorMapFactory = colorMapFactory;
            _stackFactory = stackFactory;
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>(graph, startVertex,
                VertexConcept, EdgeConcept, _colorMapFactory, _stackFactory);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TStack, TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>(graph,
                vertexEnumerator, VertexConcept, EdgeConcept, _colorMapFactory, _stackFactory);
        }
    }
}
