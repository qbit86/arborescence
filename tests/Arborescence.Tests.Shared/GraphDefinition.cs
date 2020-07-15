namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    internal readonly struct GraphDefinition
    {
        private readonly IReadOnlyList<Endpoints<int>> _edges;

        internal GraphDefinition(int vertexCount, IReadOnlyList<Endpoints<int>> edges)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            VertexCount = vertexCount;
            _edges = edges ?? Array.Empty<Endpoints<int>>();
        }

        internal int VertexCount { get; }

        internal IReadOnlyList<Endpoints<int>> Edges => _edges ?? Array.Empty<Endpoints<int>>();
    }
}
