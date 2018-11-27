﻿namespace Ubiquitous
{
    using System;
    using System.IO;
    using Models;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;

    internal static partial class Program
    {
        private static void SerializeGraphByEdges(JaggedAdjacencyListIncidenceGraph graph,
            ReadOnlySpan<DfsStepKind> vertexKinds, ReadOnlySpan<DfsStepKind> edgeKinds,
            string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\" {{");
            try
            {
                textWriter.WriteLine("  node [shape=circle]");
                if (!vertexKinds.IsEmpty)
                {
                    for (int v = 0; v < graph.VertexUpperBound; ++v)
                    {
                        DfsStepKind vertexKind = vertexKinds[v];
                        if (vertexKind == DfsStepKind.StartVertex)
                            textWriter.WriteLine($"  {v} [style=filled]");
                    }
                }

                if (!edgeKinds.IsEmpty)
                {
                    for (int e = 0; e < graph.EdgeCount; ++e)
                    {
                        if (!graph.TryGetSource(e, out int source))
                            continue;

                        if (!graph.TryGetTarget(e, out int target))
                            continue;

                        textWriter.Write($"  {source} -> {target}");

                        DfsStepKind edgeKind = edgeKinds[e];

                        // http://www.graphviz.org/Documentation/dotguide.pdf
                        switch (edgeKind)
                        {
                            case DfsStepKind.TreeEdge:
                                textWriter.WriteLine($" [label={e} style=bold]");
                                continue;
                            case DfsStepKind.BackEdge:
                                textWriter.WriteLine($" [label={e} style=dotted]");
                                continue;
                            case DfsStepKind.ForwardOrCrossEdge:
                                textWriter.WriteLine($" [label={e} style=dashed]");
                                continue;
                        }

                        textWriter.WriteLine($" [label={e} style=dotted]");
                    }
                }
            }
            finally
            {
                textWriter.WriteLine("}");
            }
        }
    }
}
