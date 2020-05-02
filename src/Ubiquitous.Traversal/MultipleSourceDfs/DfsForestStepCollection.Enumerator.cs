// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Internal;
    using static System.Diagnostics.Debug;

    public readonly partial struct DfsForestStepCollection<TGraph, TVertex, TEdge,
        TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
        TColorMap, TStep, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
    {
        public struct Enumerator : IEnumerator<TStep>
        {
            private TStep _current;
            private int _state;

            private TColorMapPolicy _colorMapPolicy;
            private TGraphPolicy _graphPolicy;
            private TStepPolicy _stepPolicy;

            private readonly TGraph _graph;

            // See explanations why fields are not readonly here:
            // https://codeblog.jonskeet.uk/2014/07/16/micro-optimization-the-surprising-inefficiency-of-readonly-fields/
            private TVertexEnumerator _vertexEnumerator;
            private readonly int _stackCapacity;
            private readonly TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;
            private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
            private DisposalStatus _stackDisposalStatus;

            private DfsStepIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy> _stepIterator;

            internal Enumerator(in DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TStep, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy> collection)
            {
                Assert(collection.GraphPolicy != null);
                Assert(collection.ColorMapPolicy != null);
                Assert(collection.StepPolicy != null);

                _current = default;
                _state = 0;

                _graph = collection.Graph;
                _vertexEnumerator = collection.VertexEnumerablePolicy.GetEnumerator(collection.VertexCollection);
                _stackCapacity = collection.StackCapacity;
                _graphPolicy = collection.GraphPolicy;
                _colorMapPolicy = collection.ColorMapPolicy;
                _stepPolicy = collection.StepPolicy;
                _colorMap = collection.ColorMap;
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepIterator = default;
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                            _colorMapDisposalStatus = DisposalStatus.Initialized;
                            _state = 1;
                            continue;
                        case 1:
                            if (!_vertexEnumerator.MoveNext())
                                return Terminate();

                            _state = 2;
                            continue;
                        case 2:
                            Color vertexColor = GetColorOrDefault(_colorMap, _vertexEnumerator.Current);
                            if (vertexColor != Color.None && vertexColor != Color.White)
                            {
                                _state = 1;
                                continue;
                            }

                            return CreateStartVertexStep();
                        case 3:
                            _stack =
                                ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Acquire(_stackCapacity);
                            if (_stack == null)
                                return Terminate();

                            _stackDisposalStatus = DisposalStatus.Initialized;
                            _state = 4;
                            continue;
                        case 4:
                            ThrowIfDisposed();
                            _stepIterator = new DfsStepIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                                TStep, TGraphPolicy, TColorMapPolicy, TStepPolicy>(_graph, _vertexEnumerator.Current,
                                _colorMap, _stack, _graphPolicy, _colorMapPolicy, _stepPolicy);
                            _state = 5;
                            continue;
                        case 5:
                            ThrowIfDisposed();
                            if (!_stepIterator.TryMoveNext(out TStep current))
                            {
                                DisposeStack();
                                _state = 1;
                                continue;
                            }

                            return PropagateCurrentStep(current);
                    }

                    return false;
                }
            }

            public void Reset()
            {
                DisposeCore();

                _current = default;
                _state = 0;

                _colorMapPolicy.Clear(_colorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepIterator = default;
            }

            // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
            public TStep Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                DisposeCore();
                _current = default;
                _state = -1;
            }

            private void DisposeStack()
            {
                if (_stackDisposalStatus == DisposalStatus.Initialized)
                {
                    ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Release(_stack);
                    _stack = null;
                    _stackDisposalStatus = DisposalStatus.Disposed;
                }
            }

            private void DisposeCore()
            {
                if (_colorMapDisposalStatus == DisposalStatus.Initialized)
                {
                    _colorMapPolicy.Clear(_colorMap);
                    _colorMapDisposalStatus = DisposalStatus.Disposed;
                }

                DisposeStack();
            }

            private void ThrowIfDisposed()
            {
                if (_colorMapDisposalStatus != DisposalStatus.Initialized)
                    throw new ObjectDisposedException(nameof(_colorMap));
                if (_stackDisposalStatus != DisposalStatus.Initialized)
                    throw new ObjectDisposedException(nameof(_stack));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool CreateStartVertexStep()
            {
                _current = _stepPolicy.CreateVertexStep(DfsStepKind.StartVertex, _vertexEnumerator.Current);
                _state = 3;
                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool PropagateCurrentStep(TStep current)
            {
                _current = current;
                _state = 5;
                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool Terminate()
            {
                DisposeCore();
                _current = default;
                _state = -1;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
                _colorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
        }
    }
}
