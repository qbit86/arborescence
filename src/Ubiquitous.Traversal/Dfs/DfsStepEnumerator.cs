// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    // https://www.boost.org/doc/libs/1_69_0/boost/graph/depth_first_search.hpp
    internal struct DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TStepPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TStep _current;
        private int _state;

        private readonly TGraph _graph;
        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.

        private TColorMap _colorMap;
        private readonly List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;
        private TStepPolicy _stepPolicy;

        internal DfsStepEnumerator(TGraph graph, TVertex startVertex,
            TColorMap colorMap, List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> stack,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
        {
            Assert(colorMap != null);
            Assert(graphPolicy != null);
            Assert(stepPolicy != null);

            _graph = graph;
            _colorMap = colorMap;
            _stack = stack;
            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
            _stepPolicy = stepPolicy;

            _current = default;
            _state = 0;

            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = startVertex;
        }

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public TStep Current => _current;

        public bool MoveNext()
        {
            // Conventions for state “naming” is taken from here:
            // http://csharpindepth.com/Articles/Chapter6/IteratorBlockImplementation.aspx
            // • -1: "After" - the iterator has finished, either by reaching the end of the method or by hitting yield break
            // • 0: "Before" - MoveNext() hasn't been called yet
            // • Anything positive: indicates where to resume from.

            // With `while (true)` we can avoid `goto label`,
            // simulating the latter with `_state = label; continue;`.
            while (true)
            {
                switch (_state)
                {
                    case 0:
                    {
                        return CreateDiscoverVertexStep(_currentVertex, 1);
                    }
                    case 1:
                    {
                        bool hasOutEdges =
                            _graphPolicy.TryGetOutEdges(_graph, _currentVertex, out TEdgeEnumerator edges);
                        if (!hasOutEdges)
                            return CreateFinishVertexStep(_currentVertex, int.MaxValue);

                        PushVertexStackFrame(_currentVertex, edges);
                        _state = 2;
                        continue;
                    }
                    case 2:
                    {
                        if (!TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> poppedStackFrame))
                            return Terminate();

                        _currentVertex = poppedStackFrame.Vertex;
                        _edgeEnumerator = poppedStackFrame.EdgeEnumerator;
                        if (poppedStackFrame.HasEdge)
                            return CreateEdgeStep(DfsStepKind.FinishEdge, poppedStackFrame.Edge, 3);

                        _state = 3;
                        continue;
                    }
                    case 3:
                    {
                        if (!_edgeEnumerator.MoveNext())
                            return CreateFinishVertexStep(_currentVertex, 2);

                        bool isValid = _graphPolicy.TryGetTarget(_graph, _edgeEnumerator.Current, out _neighborVertex);
                        if (!isValid)
                        {
                            _state = 3;
                            continue;
                        }

                        return CreateEdgeStep(DfsStepKind.ExamineEdge, _edgeEnumerator.Current, 4);
                    }
                    case 4:
                    {
                        Color neighborColor = GetColorOrDefault(_neighborVertex);
                        TEdge edge = _edgeEnumerator.Current;
                        switch (neighborColor)
                        {
                            case Color.None:
                            case Color.White:
                                return CreateEdgeStep(DfsStepKind.TreeEdge, edge, 5);
                            case Color.Gray:
                                return CreateEdgeStep(DfsStepKind.BackEdge, edge, 7);
                            default:
                                return CreateEdgeStep(DfsStepKind.ForwardOrCrossEdge, edge, 7);
                        }
                    }
                    case 5:
                    {
                        PushEdgeStackFrame(_currentVertex, _edgeEnumerator.Current, _edgeEnumerator);
                        _currentVertex = _neighborVertex;
                        return CreateDiscoverVertexStep(_currentVertex, 6);
                    }
                    case 6:
                    {
                        bool hasOutEdges = _graphPolicy.TryGetOutEdges(_graph, _currentVertex, out _edgeEnumerator);
                        if (!hasOutEdges)
                            return CreateFinishVertexStep(_currentVertex, 2);

                        _state = 3;
                        continue;
                    }
                    case 7:
                    {
                        return CreateEdgeStep(DfsStepKind.FinishEdge, _edgeEnumerator.Current, 3);
                    }
                    case int.MaxValue:
                    {
                        return Terminate();
                    }
                }

                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateDiscoverVertexStep(TVertex vertex, int newState)
        {
            _colorMapPolicy.TryPut(_colorMap, _currentVertex, Color.Gray);
            _current = _stepPolicy.CreateVertexStep(DfsStepKind.DiscoverVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateFinishVertexStep(TVertex vertex, int newState)
        {
            _colorMapPolicy.TryPut(_colorMap, _currentVertex, Color.Black);
            _current = _stepPolicy.CreateVertexStep(DfsStepKind.FinishVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateEdgeStep(DfsStepKind kind, TEdge edge, int newState)
        {
            _current = _stepPolicy.CreateEdgeStep(kind, edge);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate()
        {
            _current = default;
            _state = -1;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushVertexStackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator)
        {
            var stackFrame = new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, false, default, edgeEnumerator);
            _stack.Add(stackFrame);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushEdgeStackFrame(TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            var stackFrame = new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, true, edge, edgeEnumerator);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TVertex vertex)
        {
            if (_colorMapPolicy.TryGet(_colorMap, vertex, out Color result))
                return result;

            return Color.None;
        }
    }
}
