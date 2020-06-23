namespace Ubiquitous
{
    using System.Globalization;
    using Workbench;

    internal static partial class Program
    {
        private static CultureInfo F => CultureInfo.InvariantCulture;

        private static void Main()
        {
            DemoUndirected();
        }

        private static string V(int v) => Base32.ToString(v);

        private static string E<TGraph>(TGraph g, int e)
            where TGraph : IGraph<int, int>
        {
            string head = g.TryGetHead(e, out int h) ? V(h) : "?";
            string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
            return string.Concat(tail, " -> ", head);
        }
    }
}
