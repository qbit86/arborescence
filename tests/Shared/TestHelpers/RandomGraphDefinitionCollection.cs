namespace Arborescence;

using System;
using System.Collections;
using System.Collections.Generic;
using static System.FormattableString;

internal sealed class RandomGraphDefinitionCollection : IEnumerable<object[]>
{
    private static readonly double[] s_densityPowers = { 1.0, 1.5, 2.0 };

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 1; i < 6; ++i)
        {
            double power = 0.5 * i;
            int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
            foreach (double densityPower in s_densityPowers)
            {
                List<Endpoints<int>> edges = new();
                GraphHelpers.GenerateEdges(vertexCount, densityPower, edges);
                string description =
                    Invariant($"{{{nameof(vertexCount)}: {vertexCount}, {nameof(densityPower)}: {densityPower}}}");
                GraphDefinitionParameter parameter = new(vertexCount, edges, description);
                yield return new object[] { parameter };
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
