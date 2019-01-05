namespace Ubiquitous
{
    using System;
    using System.Globalization;
    using System.IO;
    using Models;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;

    internal static partial class Program
    {
        private static void SerializeGraphByEdges(AdjacencyListIncidenceGraph graph,
            ReadOnlySpan<DfsStepKind> vertexKinds, ReadOnlySpan<DfsStepKind> edgeKinds,
            string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\" {{");
            try
            {
                textWriter.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");

                if (!edgeKinds.IsEmpty)
                {
                    for (int e = 0; e < graph.EdgeCount; ++e)
                    {
                        if (!graph.TryGetSource(e, out int source))
                            continue;

                        if (!graph.TryGetTarget(e, out int target))
                            continue;

                        textWriter.Write($"  {IndexToChar(source)} -> {IndexToChar(target)}");

                        DfsStepKind edgeKind = edgeKinds[e];

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

                if (!vertexKinds.IsEmpty)
                {
                    for (int v = 0; v < graph.VertexCount; ++v)
                    {
                        DfsStepKind vertexKind = vertexKinds[v];
                        if (vertexKind == DfsStepKind.StartVertex)
                            textWriter.WriteLine($"  {IndexToChar(v)} [style=filled]");
                    }
                }
            }
            finally
            {
                textWriter.WriteLine("}");
            }

            string IndexToChar(int i)
            {
                if (i < 0 || i > 26)
                    return i.ToString(CultureInfo.InvariantCulture);

                char c = (char)(i + 'a');

                return c.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
