﻿namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept> :
        IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsRecursiveStepCollection(TGraph graph, TVertex startVertex,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(graph != null);
            Assert(startVertex != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactoryConcept = colorMapFactoryConcept;
        }

        public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator()
        {
            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<DfsStepKind, TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumeratorCoroutine()
        {
            yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessVertexCoroutine(StartVertex, colorMap);
                foreach (var step in steps)
                    yield return step;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessVertexCoroutine(TVertex vertex, TColorMap colorMap)
        {
            colorMap[vertex] = Color.Gray;
            yield return Step.Create(DfsStepKind.DiscoverVertex, vertex, default(TEdge));

            TEdges edges;
            if (VertexConcept.TryGetOutEdges(Graph, vertex, out edges) && edges != null)
            {
                foreach (TEdge edge in edges)
                {
                    IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessEdgeCoroutine(edge, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }

            colorMap[vertex] = Color.Black;
            yield return Step.Create(DfsStepKind.FinishVertex, vertex, default(TEdge));
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge, TColorMap colorMap)
        {
            yield return Step.Create(DfsStepKind.ExamineEdge, default(TVertex), edge);

            TVertex target;
            if (EdgeConcept.TryGetTarget(Graph, edge, out target))
            {
                Color neighborColor;
                if (!colorMap.TryGetValue(target, out neighborColor))
                    neighborColor = Color.None;

                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                        IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessVertexCoroutine(target, colorMap);
                        foreach (var step in steps)
                            yield return step;
                        break;
                    case Color.Gray:
                        yield return Step.Create(DfsStepKind.BackEdge, default(TVertex), edge);
                        break;
                    default:
                        yield return Step.Create(DfsStepKind.ForwardOrCrossEdge, default(TVertex), edge);
                        break;
                }
            }

            yield return Step.Create(DfsStepKind.FinishEdge, default(TVertex), edge);
        }
    }
}
