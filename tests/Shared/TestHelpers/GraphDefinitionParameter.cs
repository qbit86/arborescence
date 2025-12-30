namespace Arborescence;

using System;
using System.Collections.Generic;

internal sealed class GraphDefinitionParameter
{
    private readonly string _description;

    internal GraphDefinitionParameter(int vertexCount, IReadOnlyList<Endpoints<int>> edges, string description)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(vertexCount);

        VertexCount = vertexCount;
        Edges = edges;
        _description = description;
    }

    internal int VertexCount { get; }
    internal IReadOnlyList<Endpoints<int>> Edges { get; }

    public override string ToString() => _description;
}
