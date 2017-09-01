﻿namespace Ubiquitous
{
    using System.Collections.Generic;

    /// <summary>Incidence concept for the type of vertex values.</summary>
    /// <typeparam name="TEdge">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexData">The type of vertex associated data.</typeparam>
    /// <typeparam name="TEdges">The type of edges sequence.</typeparam>
    public interface IIncidenceVertexConcept<TEdge, TVertexData, TEdges>
    {
        /// <summary>Returns outgoing edges for given vertex associated data.</summary>
        /// <param name="vertexData">Vertex associated data.</param>
        /// <returns>Outgoing edges.</returns>
        TEdges GetOutEdges(TVertexData vertexData);
    }
}
