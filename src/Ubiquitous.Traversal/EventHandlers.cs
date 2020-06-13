namespace Ubiquitous.Traversal
{
    public delegate void VertexEventHandler<in TGraph, in TVertex>(TGraph g, TVertex v);

    public delegate void EdgeEventHandler<in TGraph, in TEdge>(TGraph g, TEdge e);
}
