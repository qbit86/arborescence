namespace Arborescence
{
    /// <summary>
    /// Represents a monoid — an algebraic structure with a single associative binary operation and an identity element.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the monoid.</typeparam>
    public interface IMonoidPolicy<T>
    {
        T Identity { get; }
        T Combine(T left, T right);
    }
}
