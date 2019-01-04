namespace Ubiquitous.Playground
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Misnomer;

    internal static class Program
    {
        private static Random Prng { get; } = new Random();

        private static TextWriter Out => Console.Out;

        private static void Main()
        {
            const int count = 10;

            var edges = new Rist<SourceTargetPair<int>>(count * (count - 1));
            for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    edges.Add(SourceTargetPair.Create(i, j));
                    edges.Add(SourceTargetPair.Create(j, i));
                }
            }

            Shuffle(edges);

            Out.WriteLine("digraph \"Dense\" {");
            Out.WriteLine("  layout=\"circo\"");
            Out.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");
            Out.Write("  ");
            for (int i = 0; i < count; ++i)
                Out.Write($"{IndexToChar(i)} ");

            Out.WriteLine();
            int edgeCount = edges.Count;
            for (int edgeIndex = 0; edgeIndex < edgeCount; ++edgeIndex)
            {
                SourceTargetPair<int> edge = edges[edgeIndex];
                Out.WriteLine($"  {IndexToChar(edge.Source)} -> {IndexToChar(edge.Target)} // [label={edgeIndex}]");
            }

            Out.WriteLine("}");
            edges.Dispose();
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Debug.Assert(list != null);

            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                int randomIndex = i + Prng.Next(count - i);
                T temp = list[randomIndex];
                list[randomIndex] = list[i];
                list[i] = temp;
            }
        }

        private static string IndexToChar(int i)
        {
            if (i < 0 || i > 26)
                return i.ToString(CultureInfo.InvariantCulture);

            char c = (char)(i + 'a');

            return c.ToString(CultureInfo.InvariantCulture);
        }
    }
}
