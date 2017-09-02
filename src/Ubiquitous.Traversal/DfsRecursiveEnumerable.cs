namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TVertexData, TEdgeData, TEdges, TColorMap, TColorMapFactoryConcept>
        : IEnumerable<Step<TVertex, TEdge>>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        public DfsRecursiveEnumerable(TGraph graph, TVertex startVertex, TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(graph != null);
            Assert(startVertex != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
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
                var enumerator = ProcessVertexCoroutine(StartVertex, colorMap);
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
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

        private IEnumerator<Step<TVertex, TEdge>> ProcessVertexCoroutine(TVertex vertex, TColorMap colorMap)
        {
            colorMap[vertex] = Color.Gray;
            yield return Step.Create(StepKind.DiscoverVertex, vertex, default(TEdge));

            throw new NotImplementedException();

            colorMap[vertex] = Color.Black;
            yield return Step.Create(StepKind.FinishVertex, vertex, default(TEdge));
        }
    }
}
