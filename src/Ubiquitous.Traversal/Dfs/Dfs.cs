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
        where TColorMapFactory : struct, IFactory<TGraph, TColorMap>
        where TStackFactory : struct, IFactory<TGraph, TStack>
    {
        // ReSharper disable UnassignedGetOnlyAutoProperty
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public Dfs(TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            VertexConcept = default(TVertexConcept);
            EdgeConcept = default(TEdgeConcept);
            ColorMapFactory = colorMapFactory;
            StackFactory = stackFactory;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory,
                TStackFactory>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactory, StackFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertices>(TGraph graph, TVertices vertices)
            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory,
                TStackFactory>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactory, StackFactory);
        }
    }
}
