namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
    {
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private enum DisposalStatus
            {
                None = 0,
                Initialized,
                Disposed,
            }

            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            private DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> _collection;
            private TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;

            internal Enumerator(DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> collection)
            {
                Assert(collection.ColorMapFactory != null);
                Assert(collection.StackFactory != null);

                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _collection = collection;
                _colorMap = default(TColorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                        {
                            _colorMap = _collection.ColorMapFactory.Acquire(_collection.Graph);
                            if (_colorMap == null)
                            {
                                DisposeCore();
                                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                                _state = -1;
                                return false;
                            }
                            _colorMapDisposalStatus = DisposalStatus.Initialized;
                            _current = default(Step<DfsStepKind, TVertex, TEdge>);
                            _state = 1;
                            continue;
                        }
                        case 1:
                        {
                            throw new NotImplementedException();
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
                _colorMapDisposalStatus = DisposalStatus.None;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                DisposeCore();
            }

            private void DisposeCore()
            {
                if (_colorMapDisposalStatus == DisposalStatus.Initialized)
                {
                    _collection.ColorMapFactory.Release(_collection.Graph, _colorMap);
                    _colorMap = default(TColorMap);
                    _colorMapDisposalStatus = DisposalStatus.Disposed;
                }
            }
        }
    }
}
