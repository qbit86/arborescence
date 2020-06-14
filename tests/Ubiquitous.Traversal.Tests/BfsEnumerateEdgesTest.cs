namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using Models;
    using Traversal;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class BfsEnumerateEdgesTest
    {
        private static IEnumerable<object[]> s_testCases;

        public BfsEnumerateEdgesTest()
        {
            IndexedAdjacencyListGraphPolicy graphPolicy = default;
            IndexedColorMapPolicy colorMapPolicy = default;

            InstantBfs = InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);

            IndexedSetPolicy exploredSetPolicy = default;
            EnumerableBfs = EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray>.Create(
                graphPolicy, exploredSetPolicy);
        }

        private InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; }

        private EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray,
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; }

        public static IEnumerable<object[]> TestCases => s_testCases ??= GraphHelper.CreateTestCases();
    }
}
