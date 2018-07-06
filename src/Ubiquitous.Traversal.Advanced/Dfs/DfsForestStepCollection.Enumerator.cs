// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Internal;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
        TColorMap, TGraphConcept, TColorMapConcept>
    {
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            // See explanations why fields are not readonly here:
            // https://codeblog.jonskeet.uk/2014/07/16/micro-optimization-the-surprising-inefficiency-of-readonly-fields/
            private TVertexEnumerator _vertexEnumerator;
            private TColorMapConcept _colorMapConcept;
            private TGraphConcept _graphConcept;

            private readonly TGraph _graph;
            private TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;
            private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
            private DisposalStatus _stackDisposalStatus;

            private DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept,
                TColorMapConcept> _stepEnumerator;

            public Enumerator(DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept> collection)
            {
                Assert(collection.ColorMapConcept != null);

                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _graph = collection.Graph;
                _vertexEnumerator = collection.VertexEnumerator;
                _graphConcept = collection.GraphConcept;
                _colorMapConcept = collection.ColorMapConcept;
                _colorMap = default(TColorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator =
                    default(DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept,
                        TColorMapConcept>);
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                        {
                            _colorMap = _colorMapConcept.Acquire(_graph);
                            if (_colorMap == null)
                            {
                                _state = int.MaxValue;
                                continue;
                            }

                            _colorMapDisposalStatus = DisposalStatus.Initialized;
                            _state = 1;
                            continue;
                        }
                        case 1:
                        {
                            if (!_vertexEnumerator.MoveNext())
                            {
                                _state = int.MaxValue;
                                continue;
                            }

                            _state = 2;
                            continue;
                        }
                        case 2:
                        {
                            Assert(_colorMap != null, nameof(_colorMap) + " != null");
                            if (!_colorMapConcept.TryGet(_colorMap, _vertexEnumerator.Current, out Color vertexColor))
                                vertexColor = Color.None;
                            if (vertexColor != Color.None && vertexColor != Color.White)
                            {
                                _state = 1;
                                continue;
                            }

                            _current = Step.Create(DfsStepKind.StartVertex, _vertexEnumerator.Current,
                                default(TEdge));
                            _state = 3;
                            return true;
                        }
                        case 3:
                        {
                            _stack = ListPool<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Shared.Rent(0);
                            if (_stack == null)
                            {
                                _state = int.MaxValue;
                                continue;
                            }

                            _stackDisposalStatus = DisposalStatus.Initialized;
                            _state = 4;
                            continue;
                        }
                        case 4:
                        {
                            ThrowIfDisposed();
                            _stepEnumerator = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                                TColorMap, TGraphConcept, TColorMapConcept>(
                                _graph, _vertexEnumerator.Current, _colorMap, _stack, _graphConcept, _colorMapConcept);
                            _state = 5;
                            continue;
                        }
                        case 5:
                        {
                            ThrowIfDisposed();
                            if (!_stepEnumerator.MoveNext())
                            {
                                DisposeStack();
                                _state = 1;
                                continue;
                            }

                            _current = _stepEnumerator.Current;
                            _state = 5;
                            return true;
                        }
                        case int.MaxValue:
                        {
                            DisposeCore();
                            _current = default(Step<DfsStepKind, TVertex, TEdge>);
                            _state = -1;
                            return false;
                        }
                    }

                    return false;
                }
            }

            public void Reset()
            {
                DisposeCore();

                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _colorMap = default(TColorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator =
                    default(DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept,
                        TColorMapConcept>);
            }

            // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                DisposeCore();
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = -1;
            }

            private void DisposeStack()
            {
                if (_stackDisposalStatus == DisposalStatus.Initialized)
                {
                    ListPool<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Shared.Return(_stack);
                    _stack = null;
                    _stackDisposalStatus = DisposalStatus.Disposed;
                }
            }

            private void DisposeCore()
            {
                if (_colorMapDisposalStatus == DisposalStatus.Initialized)
                {
                    _colorMapConcept.Release(_graph, _colorMap);
                    _colorMap = default(TColorMap);
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
        }
    }
}
