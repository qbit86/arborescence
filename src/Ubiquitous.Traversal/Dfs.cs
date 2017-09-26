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
            TraverseRecursively(TGraph graph, TVertex startVertex)
        {
            var enumeratorProviderConcept = new DfsRecursiveStepEnumeratorProviderConcept();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsRecursiveStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursively<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var enumeratorProviderConcept = new DfsRecursiveStepEnumeratorProviderConcept();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsRecursiveStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursivelyOptimized(TGraph graph, TVertex startVertex)
        {
            var enumeratorProviderConcept = new DfsRecursiveManuallyCraftedStepEnumeratorProviderConcept();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsRecursiveManuallyCraftedStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursivelyOptimized<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var enumeratorProviderConcept = new DfsRecursiveManuallyCraftedStepEnumeratorProviderConcept();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsRecursiveManuallyCraftedStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively(TGraph graph, TVertex startVertex)
        {
            var enumeratorProviderConcept = new DfsNonRecursiveStepEnumeratorProviderConcept();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsNonRecursiveStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var enumeratorProviderConcept = new DfsNonRecursiveStepEnumeratorProviderConcept();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsNonRecursiveStepEnumeratorProviderConcept,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, ColorMapFactoryConcept);
        }


        private struct DfsRecursiveStepEnumeratorProviderConcept
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

        private struct DfsRecursiveManuallyCraftedStepEnumeratorProviderConcept
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>
        {
            public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator(
                TGraph graph, TVertex vertex, TColorMap colorMap,
                TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
            {
                Assert(colorMap != null);

                var steps = new DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept>(
                    graph, vertex, colorMap, vertexConcept, edgeConcept);

                return steps.GetEnumerator();
            }
        }

        private struct DfsNonRecursiveStepEnumeratorProviderConcept
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>
        {
            public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator(
                TGraph graph, TVertex vertex, TColorMap colorMap,
                TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
            {
                Assert(colorMap != null);

                var steps = new DfsNonRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept>(
                    graph, vertex, colorMap, vertexConcept, edgeConcept);

                return steps.GetEnumerator();
            }
        }
    }
}
