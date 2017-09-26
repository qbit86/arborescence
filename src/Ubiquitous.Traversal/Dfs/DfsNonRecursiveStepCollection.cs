namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsNonRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
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

        public DfsNonRecursiveStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
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
            var stack = new Stack<StackFrame>();

            var initialStackFrame = new StackFrame(
                StackFrameKind.ProcessVertexPrologue, vertex, null,
                default(TEdge));
            stack.Push(initialStackFrame);

            while (stack.Count > 0)
            {
                var steps = ProcessStackFrame(stack, ColorMap);
                foreach (var step in steps)
                    yield return step;
            }
        }

        private IEnumerable<Step<DfsStepKind, TVertex, TEdge>> ProcessStackFrame(
            Stack<StackFrame> stack, TColorMap colorMap)
        {
            Assert(stack != null);
            Assert(colorMap != null);

            // https://www.codeproject.com/Articles/418776/How-to-replace-recursive-functions-using-stack-and

            StackFrame stackFrame = stack.Pop();

            switch (stackFrame.Kind)
            {
                case StackFrameKind.ProcessVertexPrologue:
                    Assert(stackFrame.EdgeEnumerator == null);

                    colorMap[stackFrame.Vertex] = Color.Gray;
                    yield return Step.Create(DfsStepKind.DiscoverVertex, stackFrame.Vertex, default(TEdge));

                    TEdges edges;
                    if (!VertexConcept.TryGetOutEdges(Graph, stackFrame.Vertex, out edges) || edges == null)
                    {
                        colorMap[stackFrame.Vertex] = Color.Black;
                        yield return Step.Create(DfsStepKind.FinishVertex, stackFrame.Vertex, default(TEdge));
                        yield break;
                    }

                    var enumerator = edges;
                    var processVertexEpilogueStackFrame = new StackFrame(
                        StackFrameKind.ProcessVertexEpilogue, stackFrame.Vertex, enumerator,
                        default(TEdge));
                    stack.Push(processVertexEpilogueStackFrame);

                    yield break;

                case StackFrameKind.ProcessVertexEpilogue:
                    Assert(stackFrame.EdgeEnumerator != null);

                    if (!stackFrame.EdgeEnumerator.MoveNext())
                    {
                        colorMap[stackFrame.Vertex] = Color.Black;
                        yield return Step.Create(DfsStepKind.FinishVertex, stackFrame.Vertex, default(TEdge));
                        yield break;
                    }

                    var processVertexEpilogueStackFrame_ = new StackFrame(
                        StackFrameKind.ProcessVertexEpilogue, stackFrame.Vertex, stackFrame.EdgeEnumerator,
                        default(TEdge));
                    stack.Push(processVertexEpilogueStackFrame_);
                    var processEdgePrologueStackFrame = new StackFrame(
                        StackFrameKind.ProcessEdgePrologue, stackFrame.Vertex, stackFrame.EdgeEnumerator,
                        stackFrame.EdgeEnumerator.Current);
                    stack.Push(processEdgePrologueStackFrame);

                    yield break;

                case StackFrameKind.ProcessEdgePrologue:
                    yield return Step.Create(DfsStepKind.ExamineEdge, default(TVertex), stackFrame.Edge);
                    TVertex target;
                    if (!EdgeConcept.TryGetTarget(Graph, stackFrame.Edge, out target))
                    {
                        yield return Step.Create(DfsStepKind.FinishEdge, default(TVertex), stackFrame.Edge);
                        yield break;
                    }

                    Color neighborColor;
                    if (!colorMap.TryGetValue(target, out neighborColor))
                        neighborColor = Color.None;

                    switch (neighborColor)
                    {
                        case Color.None:
                        case Color.White:
                            yield return Step.Create(DfsStepKind.TreeEdge, default(TVertex), stackFrame.Edge);
                            var processEdgeEpilogueStackFrame = new StackFrame(
                                StackFrameKind.ProcessEdgeEpilogue, default(TVertex), null, stackFrame.Edge);
                            stack.Push(processEdgeEpilogueStackFrame);
                            var processVertexPrologueStackFrame = new StackFrame(
                                StackFrameKind.ProcessVertexPrologue, target, null, default(TEdge));
                            stack.Push(processVertexPrologueStackFrame);
                            yield break;
                        case Color.Gray:
                            yield return Step.Create(DfsStepKind.BackEdge, default(TVertex), stackFrame.Edge);
                            break;
                        default:
                            yield return Step.Create(DfsStepKind.ForwardOrCrossEdge, default(TVertex), stackFrame.Edge);
                            break;
                    }
                    yield return Step.Create(DfsStepKind.FinishEdge, default(TVertex), stackFrame.Edge);

                    yield break;

                case StackFrameKind.ProcessEdgeEpilogue:
                    yield return Step.Create(DfsStepKind.FinishEdge, default(TVertex), stackFrame.Edge);

                    yield break;
            }
        }

        private enum StackFrameKind
        {
            None = 0,
            ProcessVertexPrologue,
            ProcessVertexEpilogue,
            ProcessEdgePrologue,
            ProcessEdgeEpilogue,
        }

        private struct StackFrame
        {
            internal StackFrameKind Kind { get; }

            internal TVertex Vertex { get; }

            internal IEnumerator<TEdge> EdgeEnumerator { get; }

            internal TEdge Edge { get; }

            internal StackFrame(StackFrameKind kind, TVertex vertex, IEnumerator<TEdge> edgeEnumerator, TEdge edge)
            {
                Kind = kind;
                Vertex = vertex;
                EdgeEnumerator = edgeEnumerator;
                Edge = edge;
            }
        }
    }
}
