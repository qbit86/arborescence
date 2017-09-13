namespace Ubiquitous
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct RecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        : ITraversal<TVertex, TEdge, TColorMap>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        internal TGraph Graph { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal RecursiveDfsTraversal(TGraph graph,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>> Traverse(TVertex vertex, TColorMap colorMap)
        {
            Assert(colorMap != null);

            return ProcessVertexCoroutine(vertex, colorMap);
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessVertexCoroutine(
            TVertex vertex, TColorMap colorMap)
        {
            Assert(colorMap != null);

            colorMap[vertex] = Color.Gray;
            yield return Step.Create(DfsStepKind.DiscoverVertex, vertex, default(TEdge));

            TEdges edges;
            if (VertexConcept.TryGetOutEdges(Graph, vertex, out edges) && edges != null)
            {
                foreach (TEdge edge in edges)
                {
                    var steps = ProcessEdgeCoroutine(edge, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }

            colorMap[vertex] = Color.Black;
            yield return Step.Create(DfsStepKind.FinishVertex, vertex, default(TEdge));
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge, TColorMap colorMap)
        {
            Assert(colorMap != null);

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
                        var steps = ProcessVertexCoroutine(target, colorMap);
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
