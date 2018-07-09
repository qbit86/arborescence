namespace Ubiquitous
{
    using System;
    using System.IO;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;

    internal static partial class Program
    {
        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph,
            ArraySegment<DfsStepKind> vertexKinds, ArraySegment<DfsStepKind> edgeKinds,
            string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\"{Environment.NewLine}{{");
            try
            {
                textWriter.WriteLine("    node [shape=circle]");
                if (vertexKinds.Array != null)
                {
                    for (int v = 0; v < graph.VertexCount; ++v)
                    {
                        DfsStepKind vertexKind = vertexKinds[v];
                        if (vertexKind == DfsStepKind.StartVertex)
                            textWriter.WriteLine($"    {v} [style=filled]");
                    }
                }

                if (edgeKinds.Array != null)
                {
                    for (int e = 0; e < graph.EdgeCount; ++e)
                    {
                        if (!graph.TryGetEndpoints(e, out SourceTargetPair<int> endpoints))
                            continue;

                        textWriter.Write($"    {endpoints.Source} -> {endpoints.Target}");

                        DfsStepKind edgeKind = edgeKinds[e];

                        // http://www.graphviz.org/Documentation/dotguide.pdf
                        switch (edgeKind)
                        {
                            case DfsStepKind.TreeEdge:
                                textWriter.WriteLine(" [style=bold]");
                                continue;
                            case DfsStepKind.BackEdge:
                                textWriter.WriteLine(" [style=dotted]");
                                continue;
                            case DfsStepKind.ForwardOrCrossEdge:
                                textWriter.WriteLine(" [style=dashed]");
                                continue;
                        }

                        textWriter.WriteLine(" [style=dotted]");
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
