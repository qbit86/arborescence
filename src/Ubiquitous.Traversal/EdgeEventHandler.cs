namespace Arborescence.Traversal
{
    public delegate void EdgeEventHandler<in TGraph, in TEdge>(TGraph g, TEdge e);
}
