﻿namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    internal sealed class GraphDefinitionParameter
    {
        private readonly string _description;

        public GraphDefinitionParameter(int vertexCount, IReadOnlyList<Endpoints<int>> edges, string description)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            VertexCount = vertexCount;
            Edges = edges ?? Array.Empty<Endpoints<int>>();
            _description = description ?? string.Empty;
        }

        internal int VertexCount { get; }
        internal IReadOnlyList<Endpoints<int>> Edges { get; }

        public override string ToString() => _description;
    }
}