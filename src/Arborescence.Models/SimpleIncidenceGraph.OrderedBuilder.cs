﻿namespace Arborescence.Models
{
    using System;

    public readonly partial struct SimpleIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class OrderedBuilder : IGraphBuilder<SimpleIncidenceGraph, int, uint>
        {
            private int _currentTail;
            private int _vertexCount;
            private ArrayBuilder<int> _upperBoundByVertex;
            private ArrayBuilder<uint> _edges;

            /// <summary>
            /// Initializes a new instance of the <see cref="OrderedBuilder"/> class.
            /// </summary>
            /// <param name="initialVertexCount">The initial number of vertices.</param>
            /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
            /// </exception>
            public OrderedBuilder(int initialVertexCount, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                _vertexCount = initialVertexCount;
                _upperBoundByVertex = new ArrayBuilder<int>(initialVertexCount);
                _edges = new ArrayBuilder<uint>(edgeCapacity);
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

                _edges.Add(edge);

                // TODO: Update bound.

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
