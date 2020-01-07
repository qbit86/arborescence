namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineUndirectedDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy> : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetSourcePolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
        where TStepPolicy : IUndirectedStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private readonly TVertexColorMap _vertexColorMap;
        private readonly TEdgeColorMap _edgeColorMap;

        internal BaselineUndirectedDfsStepCollection(TGraph graph, TVertex startVertex,
            TVertexColorMap vertexColorMap, TEdgeColorMap edgeColorMap, TGraphPolicy graphPolicy,
            TVertexColorMapPolicy vertexColorMapPolicy, TEdgeColorMapPolicy edgeColorMapPolicy, TStepPolicy stepPolicy)
        {
            Assert(vertexColorMap != null);
            Assert(edgeColorMap != null);
            Assert(graphPolicy != null);
            Assert(vertexColorMapPolicy != null);
            Assert(edgeColorMapPolicy != null);
            Assert(stepPolicy != null);

            _vertexColorMap = vertexColorMap;
            _edgeColorMap = edgeColorMap;

            Graph = graph;
            StartVertex = startVertex;
            GraphPolicy = graphPolicy;
            VertexColorMapPolicy = vertexColorMapPolicy;
            EdgeColorMapPolicy = edgeColorMapPolicy;
            StepPolicy = stepPolicy;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TVertexColorMapPolicy VertexColorMapPolicy { get; }
        private TEdgeColorMapPolicy EdgeColorMapPolicy { get; }
        private TStepPolicy StepPolicy { get; }

        public IEnumerator<TStep> GetEnumerator()
        {
            return ProcessVertexCoroutine(StartVertex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<TStep> ProcessVertexCoroutine(TVertex vertex)
        {
            VertexColorMapPolicy.AddOrUpdate(_vertexColorMap, vertex, Color.Gray);
            yield return StepPolicy.CreateVertexStep(DfsStepKind.DiscoverVertex, vertex);

            TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(Graph, vertex);
            if (outEdges != null)
            {
                while (outEdges.MoveNext())
                {
                    IEnumerator<TStep> steps = ProcessEdgeCoroutine(outEdges.Current, false);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }

            TEdgeEnumerator inEdges = GraphPolicy.EnumerateInEdges(Graph, vertex);
            if (inEdges != null)
            {
                while (inEdges.MoveNext())
                {
                    IEnumerator<TStep> steps = ProcessEdgeCoroutine(inEdges.Current, true);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }

            VertexColorMapPolicy.AddOrUpdate(_vertexColorMap, vertex, Color.Black);
            yield return StepPolicy.CreateVertexStep(DfsStepKind.FinishVertex, vertex);
        }

        private IEnumerator<TStep> ProcessEdgeCoroutine(TEdge edge, bool isReversed)
        {
            yield return StepPolicy.CreateEdgeStep(DfsStepKind.ExamineEdge, edge, isReversed);

            bool hasNeighbour = isReversed
                ? GraphPolicy.TryGetSource(Graph, edge, out TVertex neighbour)
                : GraphPolicy.TryGetTarget(Graph, edge, out neighbour);
            if (hasNeighbour)
            {
                if (!VertexColorMapPolicy.TryGetValue(_vertexColorMap, neighbour, out Color neighborColor))
                    neighborColor = Color.None;

                if (!EdgeColorMapPolicy.TryGetValue(_edgeColorMap, edge, out Color edgeColor))
                    edgeColor = Color.None;

                EdgeColorMapPolicy.AddOrUpdate(_edgeColorMap, edge, Color.Black);
                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return StepPolicy.CreateEdgeStep(DfsStepKind.TreeEdge, edge, isReversed);
                        IEnumerator<TStep> steps = ProcessVertexCoroutine(neighbour);
                        while (steps.MoveNext())
                            yield return steps.Current;
                        break;
                    case Color.Gray:
                        // “Forward and cross edges never occur in a depth-first search of an undirected graph.”
                        // Here is difference with directed version of DFS.
                        if (edgeColor == Color.None || edgeColor == Color.White)
                            yield return StepPolicy.CreateEdgeStep(DfsStepKind.BackEdge, edge, isReversed);
                        break;
                }
            }

            yield return StepPolicy.CreateEdgeStep(DfsStepKind.FinishEdge, edge, isReversed);
        }
    }
}
