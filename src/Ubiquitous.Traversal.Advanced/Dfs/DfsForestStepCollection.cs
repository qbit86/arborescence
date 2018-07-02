namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TStack, TGraphConcept, TColorMapFactory, TStackFactory>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
        where TStackFactory : IFactory<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertexEnumerator VertexEnumerator { get; }

        private TGraphConcept GraphConcept { get; }


        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }

        internal DfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TGraphConcept graphConcept, TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapFactory != null);
            Assert(stackFactory != null);

            Graph = graph;
            VertexEnumerator = vertexEnumerator;
            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
            StackFactory = stackFactory;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> IEnumerable<Step<DfsStepKind, TVertex, TEdge>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
