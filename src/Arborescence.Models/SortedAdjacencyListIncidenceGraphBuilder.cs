namespace Arborescence.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <inheritdoc/>
    public struct SortedAdjacencyListIncidenceGraphBuilder : IGraphBuilder<SortedAdjacencyListIncidenceGraph, int, int>
    {
        private ArrayBuilder<int> _orderedTails;
        private ArrayBuilder<int> _heads;
        private ArrayPrefix<int> _edgeUpperBounds;
        private int _lastTail;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedAdjacencyListIncidenceGraphBuilder"/> struct.
        /// </summary>
        /// <param name="initialVertexCount">The initial number of vertices.</param>
        /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
        /// </exception>
        public SortedAdjacencyListIncidenceGraphBuilder(int initialVertexCount, int edgeCapacity = 0)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            _orderedTails = new ArrayBuilder<int>(edgeCapacity);
            _heads = new ArrayBuilder<int>(edgeCapacity);
            _lastTail = 0;
            int[] edgeUpperBounds = Pool.Rent(initialVertexCount);
            Array.Clear(edgeUpperBounds, 0, initialVertexCount);
            _edgeUpperBounds = ArrayPrefix.Create(edgeUpperBounds, initialVertexCount);
        }

        private static ArrayPool<int> Pool => ArrayPool<int>.Shared;

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _edgeUpperBounds.Count;

        /// <summary>
        /// Ensures that the builder can hold the specified number of vertices without growing.
        /// </summary>
        /// <param name="vertexCount">The number of vertices.</param>
        public void EnsureVertexCount(int vertexCount)
        {
            if (vertexCount > VertexCount)
                _edgeUpperBounds = ArrayPrefixBuilder.Resize(_edgeUpperBounds, vertexCount, true);
        }

        /// <inheritdoc/>
        public bool TryAdd(int tail, int head, out int edge)
        {
            if (tail < 0)
            {
                edge = default;
                return false;
            }

            if (head < 0)
            {
                edge = default;
                return false;
            }

            int max = Math.Max(tail, head);
            EnsureVertexCount(max + 1);

            if (tail < _lastTail)
            {
                edge = default;
                return false;
            }

            Assert(_orderedTails.Count == _heads.Count);
            int newEdgeIndex = _heads.Count;
            _orderedTails.Add(tail);
            _heads.Add(head);

            _edgeUpperBounds[tail] = newEdgeIndex + 1;
            _lastTail = tail;

            edge = newEdgeIndex;
            return true;
        }

        // Storage layout:
        // vertexCount      heads
        //         ↓↓↓      ↓↓↓↓↓
        //         [4][^^^^][bbc][aac]
        //            ↑↑↑↑↑↑     ↑↑↑↑↑
        //   edgeUpperBounds     orderedTails

        /// <inheritdoc/>
        public SortedAdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_orderedTails.Count == _heads.Count);
            int vertexCount = VertexCount;
            int headCount = _heads.Count;
            int orderedTailCount = _orderedTails.Count;
            var storage = new int[1 + vertexCount + headCount + orderedTailCount];
            storage[0] = vertexCount;

            ReadOnlySpan<int> headsBuffer = _heads.AsSpan();
            headsBuffer.CopyTo(storage.AsSpan(1 + vertexCount, headCount));
            _heads.Dispose(false);

            ReadOnlySpan<int> orderedTailsBuffer = _orderedTails.AsSpan();
            orderedTailsBuffer.CopyTo(storage.AsSpan(1 + vertexCount + headCount, orderedTailCount));
            _orderedTails.Dispose(false);

            // Make EdgeUpperBounds monotonic in case if we skipped some tails.
            for (int v = 1; v < _edgeUpperBounds.Count; ++v)
            {
                if (_edgeUpperBounds[v] < _edgeUpperBounds[v - 1])
                    _edgeUpperBounds[v] = _edgeUpperBounds[v - 1];
            }

            _edgeUpperBounds.CopyTo(storage, 1);
            if (_edgeUpperBounds.Array != null)
                Pool.Return(_edgeUpperBounds.Array);
            _edgeUpperBounds = ArrayPrefix<int>.Empty;

            _lastTail = 0;

            return new SortedAdjacencyListIncidenceGraph(storage);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
