namespace Ubiquitous
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal partial struct NonRecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        internal TGraph Graph { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal NonRecursiveDfsImpl(TGraph graph,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        internal IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessVertexCoroutine(TVertex vertex, TColorMap colorMap)
        {
            Assert(colorMap != null);

            var stack = new Stack<StackFrame>();

            var initialStackFrame = new StackFrame(StackFrameKind.ProcessVertexPrologue, vertex);
            stack.Push(initialStackFrame);

            while (stack.Count > 0)
            {
                var steps = ProcessStackFrame(stack, colorMap);
                foreach (var step in steps)
                    yield return step;
            }
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessStackFrame(Stack<StackFrame> stack, TColorMap colorMap)
        {
            Assert(stack != null);
            Assert(colorMap != null);

            // https://www.codeproject.com/Articles/418776/How-to-replace-recursive-functions-using-stack-and

            StackFrame stackFrame = stack.Pop();

            // TODO: Add actual implementation.
            switch (stackFrame.Kind)
            {
                case StackFrameKind.ProcessVertexPrologue:
                    colorMap[stackFrame.Vertex] = Color.Gray;
                    yield return Step.Create(DfsStepKind.DiscoverVertex, stackFrame.Vertex, default(TEdge));

                    TEdges edges;
                    if (VertexConcept.TryGetOutEdges(Graph, stackFrame.Vertex, out edges) && edges != null)
                    {
                        var enumerator = edges.GetEnumerator();
                        while (enumerator.MoveNext())
                        {

                        }
                    }
                    break;
                case StackFrameKind.None:
                    yield break;
            }
        }

        [System.Obsolete]
        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessVertexCoroutine(StackFrame stackFrame, TVertex vertex, TColorMap colorMap)
        {
            colorMap[vertex] = Color.Gray;
            yield return Step.Create(DfsStepKind.DiscoverVertex, vertex, default(TEdge));

            TEdges edges;
            if (VertexConcept.TryGetOutEdges(Graph, vertex, out edges) && edges != null)
            {
                foreach (TEdge edge in edges)
                {
                    IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessEdgeCoroutine(stackFrame, edge, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }

            colorMap[vertex] = Color.Black;
            yield return Step.Create(DfsStepKind.FinishVertex, vertex, default(TEdge));
        }

        [System.Obsolete]
        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(StackFrame stackFrame, TEdge edge, TColorMap colorMap)
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
                        IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = ProcessVertexCoroutine(stackFrame, target, colorMap);
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
