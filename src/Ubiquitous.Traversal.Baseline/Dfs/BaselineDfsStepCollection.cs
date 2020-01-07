// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
            TGraphPolicy, TColorMapPolicy, TStepPolicy>
        : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TColorMap _colorMap;

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        private TStepPolicy StepPolicy { get; }

        internal BaselineDfsStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
        {
            Assert(colorMap != null);
            Assert(graphPolicy != null);
            Assert(colorMapPolicy != null);
            Assert(stepPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            _colorMap = colorMap;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            StepPolicy = stepPolicy;
        }

        public IEnumerator<TStep> GetEnumerator()
        {
            return ProcessVertexCoroutine(StartVertex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<TStep> result = GetEnumerator();
            return result;
        }

        private IEnumerator<TStep> ProcessVertexCoroutine(TVertex vertex)
        {
            ColorMapPolicy.AddOrUpdate(_colorMap, vertex, Color.Gray);
            yield return StepPolicy.CreateVertexStep(DfsStepKind.DiscoverVertex, vertex);

            TEdgeEnumerator edges = GraphPolicy.EnumerateOutEdges(Graph, vertex);
            if (edges != null)
            {
                while (edges.MoveNext())
                {
                    IEnumerator<TStep> steps = ProcessEdgeCoroutine(edges.Current);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }

            ColorMapPolicy.AddOrUpdate(_colorMap, vertex, Color.Black);
            yield return StepPolicy.CreateVertexStep(DfsStepKind.FinishVertex, vertex);
        }

        private IEnumerator<TStep> ProcessEdgeCoroutine(TEdge edge)
        {
            yield return StepPolicy.CreateEdgeStep(DfsStepKind.ExamineEdge, edge);

            if (GraphPolicy.TryGetTarget(Graph, edge, out TVertex target))
            {
                if (!ColorMapPolicy.TryGetValue(_colorMap, target, out Color neighborColor))
                    neighborColor = Color.None;

                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return StepPolicy.CreateEdgeStep(DfsStepKind.TreeEdge, edge);
                        IEnumerator<TStep> steps = ProcessVertexCoroutine(target);
                        while (steps.MoveNext())
                            yield return steps.Current;
                        break;
                    case Color.Gray:
                        yield return StepPolicy.CreateEdgeStep(DfsStepKind.BackEdge, edge);
                        break;
                    default:
                        yield return StepPolicy.CreateEdgeStep(DfsStepKind.ForwardOrCrossEdge, edge);
                        break;
                }
            }

            yield return StepPolicy.CreateEdgeStep(DfsStepKind.FinishEdge, edge);
        }
    }
}
