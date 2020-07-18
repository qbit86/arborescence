namespace Arborescence.Models
{
    using System;

    public readonly partial struct SimpleIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<SimpleIncidenceGraph, int, uint>
        {
            private ArrayPrefix<uint> _edges;
            private int _vertexCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            /// <param name="initialVertexCount">The initial number of vertices.</param>
            /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
            /// </exception>
            public Builder(int initialVertexCount, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                _vertexCount = initialVertexCount;
                _edges = ArrayPrefixBuilder.Create<uint>(edgeCapacity);
            }

            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out uint edge)
            {
                if (unchecked((uint)tail) > ushort.MaxValue)
                {
                    edge = default;
                    return false;
                }

                if (unchecked((uint)head) > ushort.MaxValue)
                {
                    edge = default;
                    return false;
                }

                edge = unchecked(((uint)tail << 16) | (uint)head);

                int max = Math.Max(tail, head);
                EnsureVertexCount(max + 1);

                _edges = ArrayPrefixBuilder.Add(_edges, edge, false);
                return true;
            }

            /// <inheritdoc/>
            public SimpleIncidenceGraph ToGraph() => throw new NotImplementedException();

            /// <summary>
            /// Ensures that the builder can hold the specified number of vertices without growing.
            /// </summary>
            /// <param name="vertexCount">The number of vertices.</param>
            public void EnsureVertexCount(int vertexCount) => throw new NotImplementedException();
        }
    }
}
