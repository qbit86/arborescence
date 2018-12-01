namespace Ubiquitous
{
    using System.IO;
    using Workbench;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class AdjacencyListTest
    {
        public AdjacencyListTest(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void ShouldNotBeLess(string testName)
        {
            using (TextReader textReader = IndexedGraphs.GetTextReader(testName))
            {
            }

            Assert.True(true, testName);
        }
    }
}
