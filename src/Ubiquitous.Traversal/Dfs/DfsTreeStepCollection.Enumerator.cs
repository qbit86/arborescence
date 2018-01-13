namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
    {
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;
            private DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> _collection;

            public Enumerator(DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> collection)
            {
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
                _collection = collection;
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                        {
                            throw new NotImplementedException();
                        }
                        case int.MaxValue:
                        {
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
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }
        }
    }
}
