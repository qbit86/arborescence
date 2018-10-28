namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// Provides the Create factory method for SourceTargetPair&lt;TVertex&gt;.
    /// </summary>
    public static class SourceTargetPair
    {
        public static SourceTargetPair<TVertex> Create<TVertex>(TVertex source, TVertex target)
        {
            return new SourceTargetPair<TVertex>(source, target);
        }

        /// <summary>
        /// Used by SourceTargetPair.ToString to reduce generic code.
        /// </summary>
        internal static string PairToString(object source, object target)
        {
            var s = new StringBuilder();
            s.Append('[');

            if (source != null)
                s.Append(source);

            s.Append(", ");

            if (target != null)
                s.Append(target);

            s.Append(']');

            return s.ToString();
        }
    }

    /// <summary>
    /// A SourceTargetPair holds endpoints of an oriented edge.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex descriptors.</typeparam>
    /// <remarks>
    /// See also: <a href="https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Collections/Generic/KeyValuePair.cs">KeyValuePair.cs</a>
    /// </remarks>
    public readonly struct SourceTargetPair<TVertex> : IEquatable<SourceTargetPair<TVertex>>
    {
        public TVertex Source { get; }
        public TVertex Target { get; }

        public SourceTargetPair(TVertex source, TVertex target)
        {
            Source = source;
            Target = target;
        }

        public override string ToString()
        {
            return SourceTargetPair.PairToString(Source, Target);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TVertex source, out TVertex target)
        {
            source = Source;
            target = Target;
        }

        public bool Equals(SourceTargetPair<TVertex> other)
        {
            EqualityComparer<TVertex> comparer = EqualityComparer<TVertex>.Default;

            if (!comparer.Equals(Source, other.Source))
                return false;

            if (!comparer.Equals(Target, other.Target))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SourceTargetPair<TVertex>))
                return false;

            var other = (SourceTargetPair<TVertex>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Target.GetHashCode();
        }

        public static bool operator ==(SourceTargetPair<TVertex> left, SourceTargetPair<TVertex> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SourceTargetPair<TVertex> left, SourceTargetPair<TVertex> right)
        {
            return !left.Equals(right);
        }
    }
}
