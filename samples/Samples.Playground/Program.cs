namespace Arborescence;

using System.Globalization;
using Workbench;

internal static partial class Program
{
    private static CultureInfo P => CultureInfo.InvariantCulture;

    private static void Main() => DemoAdjacencyBfsVertices();

    private static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);

    private static string V(int v) => Base32.ToString(v);

    private static string E<TGraph>(TGraph g, int e)
        where TGraph : IHeadIncidence<int, int>, ITailIncidence<int, int>
    {
        string head = g.TryGetHead(e, out int h) ? V(h) : "?";
        string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
        return string.Concat(tail, " -> ", head);
    }
}
