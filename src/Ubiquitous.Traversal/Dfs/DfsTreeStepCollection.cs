namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
        where TStackFactory : IFactory<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }

        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            Assert(colorMapFactory != null);
            Assert(stackFactory != null);

            Graph = graph;
            StartVertex = startVertex;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
            StackFactory = stackFactory;
        }

        public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
