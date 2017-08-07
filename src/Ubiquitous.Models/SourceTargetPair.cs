namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides the Create factory method for SourceTargetPair&lt;TVertexKey&gt;.
    /// </summary>
    public static class SourceTargetPair
    {
        public static SourceTargetPair<TVertexKey> Create<TVertexKey>(TVertexKey source, TVertexKey target)
        {
            return new SourceTargetPair<TVertexKey>(source, target);
        }

        /// <summary>
        /// Used by SourceTargetPair.ToString to reduce generic code.
        /// </summary>
        internal static string PairToString(object source, object target)
        {
            var s = new System.Text.StringBuilder();
            s.Append('[');

            if (source != null)
            {
                s.Append(source.ToString());
            }

            s.Append(", ");

            if (target != null)
            {
                s.Append(target.ToString());
            }

            s.Append(']');

            return s.ToString();
        }
    }

    /// <summary>
    /// A SourceTargetPair holds endpoints of an oriented edge.
    /// </summary>
    /// <seealso cref="https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Collections/Generic/KeyValuePair.cs"/>
    /// <typeparam name="TVertexKey">The type of the vertex descriptors.</typeparam>
    public struct SourceTargetPair<TVertexKey> : IEquatable<SourceTargetPair<TVertexKey>>
    {
        public TVertexKey Source { get; }
        public TVertexKey Target { get; }

        public SourceTargetPair(TVertexKey source, TVertexKey target)
        {
            Source = source;
            Target = target;
        }

        public override string ToString()
        {
            return SourceTargetPair.PairToString(Source, Target);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out TVertexKey source, out TVertexKey target)
        {
            source = Source;
            target = Target;
        }

        public bool Equals(SourceTargetPair<TVertexKey> other)
        {
            var comparer = EqualityComparer<TVertexKey>.Default;

            if (!comparer.Equals(Source, other.Source))
                return false;

            if (!comparer.Equals(Target, other.Target))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SourceTargetPair<TVertexKey>))
                return false;

            var other = (SourceTargetPair<TVertexKey>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Target.GetHashCode();
        }

        public static bool operator ==(SourceTargetPair<TVertexKey> left, SourceTargetPair<TVertexKey> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SourceTargetPair<TVertexKey> left, SourceTargetPair<TVertexKey> right)
        {
            return !left.Equals(right);
        }
    }
}
