namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TColorMap ColorMap { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public DfsRecursiveStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(colorMap != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            ColorMap = colorMap;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator()
        {
            return ProcessVertexCoroutine(StartVertex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<DfsStepKind, TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> ProcessVertexCoroutine(TVertex vertex)
        {
            ColorMap[vertex] = Color.Gray;
            yield return Step.Create(DfsStepKind.DiscoverVertex, vertex, default(TEdge));

            TEdges edges;
            if (VertexConcept.TryGetOutEdges(Graph, vertex, out edges) && edges != null)
            {
                while (edges.MoveNext())
                {
                    TEdge edge = edges.Current;
                    var steps = ProcessEdgeCoroutine(edge);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }

            ColorMap[vertex] = Color.Black;
            yield return Step.Create(DfsStepKind.FinishVertex, vertex, default(TEdge));
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge)
        {
            yield return Step.Create(DfsStepKind.ExamineEdge, default(TVertex), edge);

            TVertex target;
            if (EdgeConcept.TryGetTarget(Graph, edge, out target))
            {
                Color neighborColor;
                if (!ColorMap.TryGetValue(target, out neighborColor))
                    neighborColor = Color.None;

                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                        var steps = ProcessVertexCoroutine(target);
                        while (steps.MoveNext())
                            yield return steps.Current;
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
