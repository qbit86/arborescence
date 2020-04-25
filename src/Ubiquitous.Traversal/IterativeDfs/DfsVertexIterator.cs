// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    internal struct DfsVertexIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        internal DfsVertexStep<TVertex> _current;
        private int _state;
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;
        private TGraph _graph;
        private TVertex _currentVertex;
        private TColorMap _colorMap;
        private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;

        internal DfsVertexIterator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _current = new DfsVertexStep<TVertex>(DfsStepKind.None, startVertex);
            _state = 1;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;

            _graph = graph;
            _currentVertex = startVertex;
            _colorMap = colorMap;
            _stack = null;
        }

        internal bool MoveNext()
        {
            if (_state <= 0)
                return false;

            while (true)
            {
                switch (_state)
                {
                    case 1:
                        return CreateNoneVertexStep(_currentVertex, 2, out _current);
                    case 2:
                        return CreateDiscoverVertexStep(_currentVertex, 3, out _current);
                    case 3:
                        TEdgeEnumerator edges = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        if (_stack is null)
                            _stack = new List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();
                        else
                            _stack.Clear();
                        PushVertexStackFrame(_currentVertex, edges);
                        _state = 4;
                        continue;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsVertexStep<TVertex>(DfsStepKind.None, startVertex);
            _stack?.Clear();
            _state = 1;
        }

        internal void Dispose()
        {
            _stack?.Clear();
            _stack = null;
            _current = default;
            _state = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateNoneVertexStep(TVertex vertex, int newState, out DfsVertexStep<TVertex> current)
        {
            current = new DfsVertexStep<TVertex>(DfsStepKind.None, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateDiscoverVertexStep(TVertex vertex, int newState, out DfsVertexStep<TVertex> current)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Gray);
            current = new DfsVertexStep<TVertex>(DfsStepKind.DiscoverVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate(out DfsVertexStep<TVertex> current)
        {
            current = new DfsVertexStep<TVertex>(DfsStepKind.None, _currentVertex);
            _state = -2;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushVertexStackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator)
        {
            var stackFrame = new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, false, default, edgeEnumerator);
            _stack.Add(stackFrame);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame)
        {
            if (_stack.Count == 0)
            {
                stackFrame = default;
                return false;
            }

            stackFrame = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return true;
        }
    }
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
