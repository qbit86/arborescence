namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TVertexData, TEdgeData, TEdges, TColorMap,
        TGraphConcept, TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        : IEnumerable<Step<TVertex, TEdge>>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TGraphConcept : IGraphConcept<TGraph, TVertex, TEdge, TVertexData, TEdgeData>
        where TVertexConcept : IIncidenceVertexConcept<TVertexData, TEdges>
        where TEdgeConcept : IEdgeDataConcept<TVertex, TEdgeData>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        internal TGraphConcept GraphConcept { get; }

        internal TVertexConcept VertexConcept { get; }

        internal TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsRecursiveEnumerable(TGraph graph, TVertex startVertex,
            TGraphConcept graphConcept, TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(graph != null);
            Assert(startVertex != null);
            Assert(graphConcept != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            GraphConcept = graphConcept;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactoryConcept = colorMapFactoryConcept;
        }

        public IEnumerator<Step<TVertex, TEdge>> GetEnumerator()
        {
            yield return Step.Create(StepKind.StartVertex, StartVertex, default(TEdge));

            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                IEnumerable<Step<TVertex, TEdge>> steps = ProcessVertexCoroutine(StartVertex, colorMap);
                foreach (var step in steps)
                    yield return step;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerable<Step<TVertex, TEdge>> ProcessVertexCoroutine(TVertex vertex, TColorMap colorMap)
        {
            colorMap[vertex] = Color.Gray;
            yield return Step.Create(StepKind.DiscoverVertex, vertex, default(TEdge));

            TVertexData vertexData;
            if (GraphConcept.TryGetVertexData(Graph, vertex, out vertexData))
            {
                TEdges edges = VertexConcept.GetOutEdges(vertexData);
                foreach (TEdge edge in edges)
                {
                    IEnumerable<Step<TVertex, TEdge>> steps = ProcessEdgeCoroutine(edge, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }

            colorMap[vertex] = Color.Black;
            yield return Step.Create(StepKind.FinishVertex, vertex, default(TEdge));
        }

        private IEnumerable<Step<TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge, TColorMap colorMap)
        {
            yield return Step.Create(StepKind.ExamineEdge, default(TVertex), edge);

            TEdgeData edgeData;
            if (GraphConcept.TryGetEdgeData(Graph, edge, out edgeData))
            {
                TVertex target = EdgeConcept.GetTarget(edgeData);

                Color neighborColor;
                if (!colorMap.TryGetValue(target, out neighborColor))
                    neighborColor = Color.White;

                switch (neighborColor)
                {
                    case Color.White:
                        yield return Step.Create(StepKind.TreeEdge, default(TVertex), edge);
                        IEnumerable<Step<TVertex, TEdge>> steps = ProcessVertexCoroutine(target, colorMap);
                        foreach (var step in steps)
                            yield return step;
                        break;
                    case Color.Gray:
                        yield return Step.Create(StepKind.BackEdge, default(TVertex), edge);
                        break;
                    default:
                        yield return Step.Create(StepKind.ForwardOrCrossEdge, default(TVertex), edge);
                        break;
                }
            }

            yield return Step.Create(StepKind.FinishEdge, default(TVertex), edge);
        }
    }
}
