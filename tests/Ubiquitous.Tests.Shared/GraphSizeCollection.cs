namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class GraphSizeCollection : IEnumerable<object[]>
    {
        private static readonly double[] s_densityPowers = { 1.0, Math.Sqrt(2.0), 0.5 * (1.0 + Math.Sqrt(5.0)), 2.0 };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 1.0 };

            for (int power = 1; power < 7; ++power)
            {
                double exp = Math.Pow(10.0, 0.5 * power);
                int vertexCount = (int)Math.Ceiling(exp);
                foreach (double densityPower in s_densityPowers)
                    yield return new object[] { vertexCount, densityPower };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
