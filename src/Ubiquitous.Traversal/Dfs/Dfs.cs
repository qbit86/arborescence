namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }


        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBaseline(TGraph graph, TVertex startVertex)
        {
            var enumeratorProviderConcept = new DfsBaselineStepEnumeratorProviderConcept();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsBaselineStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBaseline<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var enumeratorProviderConcept = new DfsBaselineStepEnumeratorProviderConcept();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsBaselineStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }


        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBoost(TGraph graph, TVertex startVertex)
        {
            var enumeratorProviderConcept = new DfsBoostStepEnumeratorProviderConcept();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsBoostStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBoost<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var enumeratorProviderConcept = new DfsBoostStepEnumeratorProviderConcept();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsBoostStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }


        private struct DfsBaselineStepEnumeratorProviderConcept
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>
        {
            public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator(
                TGraph graph, TVertex vertex, TColorMap colorMap,
                TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
            {
                Assert(colorMap != null);

                var steps = new DfsBaselineStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept>(
                    graph, vertex, colorMap, vertexConcept, edgeConcept);

                return steps.GetEnumerator();
            }
        }

        private struct DfsBoostStepEnumeratorProviderConcept
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>
        {
            public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator(TGraph graph, TVertex vertex, TColorMap colorMap, TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
            {
                Assert(colorMap != null);

                var steps = new DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept>(
                    graph, vertex, colorMap, vertexConcept, edgeConcept);

                return steps.GetEnumerator();
            }
        }
    }
}
