// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TColorMap _colorMap;

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        public BaselineDfsStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            Assert(colorMap != null);
            Assert(graphPolicy != null);
            Assert(colorMapPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            _colorMap = colorMap;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
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
            if (!ColorMapPolicy.TryPut(_colorMap, vertex, Color.Gray))
                yield break;

            yield return Step.Create(DfsStepKind.DiscoverVertex, vertex, default(TEdge));

            if (GraphPolicy.TryGetOutEdges(Graph, vertex, out TEdgeEnumerator edges) && edges != null)
            {
                while (edges.MoveNext())
                {
                    IEnumerator<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessEdgeCoroutine(edges.Current);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }

            if (!ColorMapPolicy.TryPut(_colorMap, vertex, Color.Black))
                yield break;

            yield return Step.Create(DfsStepKind.FinishVertex, vertex, default(TEdge));
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge)
        {
            yield return Step.Create(DfsStepKind.ExamineEdge, default(TVertex), edge);

            if (GraphPolicy.TryGetTarget(Graph, edge, out TVertex target))
            {
                if (!ColorMapPolicy.TryGet(_colorMap, target, out Color neighborColor))
                    neighborColor = Color.None;

                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessVertexCoroutine(target);
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
