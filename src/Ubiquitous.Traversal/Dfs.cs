namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges,
        TVertexConcept, TEdgeConcept>

        where TEdges : IEnumerator<TEdge>

        where TVertexConcept : struct, IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }


        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var enumeratorProviderConcept = new DfsRecursiveStepEnumeratorProviderConcept<TColorMap>();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsRecursiveStepEnumeratorProviderConcept<TColorMap>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursively<TVertices, TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var enumeratorProviderConcept = new DfsRecursiveStepEnumeratorProviderConcept<TColorMap>();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsRecursiveStepEnumeratorProviderConcept<TColorMap>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var enumeratorProviderConcept = new DfsNonRecursiveStepEnumeratorProviderConcept<TColorMap>();

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                DfsNonRecursiveStepEnumeratorProviderConcept<TColorMap>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively<TVertices, TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var enumeratorProviderConcept = new DfsNonRecursiveStepEnumeratorProviderConcept<TColorMap>();

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                DfsNonRecursiveStepEnumeratorProviderConcept<TColorMap>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, enumeratorProviderConcept, colorMapFactoryConcept);
        }


        private struct DfsRecursiveStepEnumeratorProviderConcept<TColorMap>
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>

            where TColorMap : IDictionary<TVertex, Color>
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

        private struct DfsNonRecursiveStepEnumeratorProviderConcept<TColorMap>
            : IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap,
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>>,
                TVertexConcept, TEdgeConcept>

            where TColorMap : IDictionary<TVertex, Color>
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
