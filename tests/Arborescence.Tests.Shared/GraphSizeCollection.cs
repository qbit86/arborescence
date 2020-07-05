namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CA1812 // GraphSizeCollection is an internal class that is apparently never instantiated.
    internal sealed class GraphSizeCollection : IEnumerable<object[]>
    {
        private static readonly double[] s_densityPowers = { 1.0, 1.5, 2.0 };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 1.0 };

            for (int i = 1; i < 7; ++i)
            {
                double power = 0.5 * i;
                int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
                foreach (double densityPower in s_densityPowers)
                    yield return new object[] { vertexCount, densityPower };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CA1812 // GraphSizeCollection is an internal class that is apparently never instantiated.
}
