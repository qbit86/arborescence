namespace Ubiquitous
{
    using System;
    using System.IO;
    using Models;
    using Traversal;
    using Workbench;
    using static System.Diagnostics.Debug;

    internal static partial class Program
    {
        private static void SerializeGraph(AdjacencyListIncidenceGraph graph,
            ReadOnlySpan<DfsStepKind> vertexKinds, ReadOnlySpan<DfsStepKind> edgeKinds,
            string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\" {{");
            try
            {
                textWriter.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");
                if (!vertexKinds.IsEmpty)
                {
                    for (int v = 0; v < graph.VertexCount; ++v)
                        textWriter.Write(v == 0 ? $"  {IndexToString(v)}" : $" {IndexToString(v)}");

                    textWriter.WriteLine();
                }

                if (!edgeKinds.IsEmpty)
                {
                    for (int e = 0; e < graph.EdgeCount; ++e)
                    {
                        if (!graph.TryGetTail(e, out int tail))
                            continue;

                        if (!graph.TryGetHead(e, out int head))
                            continue;

                        textWriter.Write($"  {IndexToString(tail)} -> {IndexToString(head)}");

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
                            textWriter.WriteLine($"  {IndexToString(v)} [style=filled]");
                    }
                }
            }
            finally
            {
                textWriter.WriteLine("}");
            }

            static string IndexToString(int i) => Base32.ToString(i);
        }
    }
}
