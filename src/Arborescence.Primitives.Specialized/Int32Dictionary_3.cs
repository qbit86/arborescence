namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    /// <summary>
    /// Provides a dictionary to use when the key is an <see cref="int"/> from a contiguous range
    /// and <typeparamref name="TAbsenceComparer"/> is used to distinguish missing elements.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TValueList">The type of the backing list.</typeparam>
    /// <typeparam name="TAbsenceComparer">The type that provides a method to distinguish missing elements.</typeparam>
    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsenceComparer> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>,
        IEquatable<Int32Dictionary<TValue, TValueList, TAbsenceComparer>>
        where TValueList : IList<TValue>
        where TAbsenceComparer : IEqualityComparer<TValue>
    {
        private readonly TValueList _values;
        private readonly TAbsenceComparer _absenceComparer;
        private readonly TValue? _absenceMarker;

        internal Int32Dictionary(TValueList values, TAbsenceComparer absenceComparer, TValue? absenceMarker)
        {
            _values = values;
            _absenceComparer = absenceComparer;
            _absenceMarker = absenceMarker;
        }

        /// <summary>
        /// Gets the maximum number of elements contained in the
        /// <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>.
        /// </summary>
        public int MaxCount => (_values?.Count).GetValueOrDefault();

        /// <summary>
        /// Computes the number of elements in the dictionary taking into account
        /// the <typeparamref name="TAbsenceComparer"/>.
        /// </summary>
        /// <remarks>Retrieving the number of elements is an O(n) operation.</remarks>
        /// <returns>The number of elements in the dictionary.</returns>
        public int GetCount()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._values is not { } values)
                return 0;
            int count = values.Count;
            int result = 0;
            for (int i = 0; i < count; ++i)
            {
                if (!self.IsAbsence(values[i]))
                    ++result;
            }

            return result;
        }

        /// <inheritdoc/>
        public void Add(int key, TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            TValueList values = self._values;
            int count = values.Count;
            if (unchecked((uint)key > (uint)count))
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(key));
            else if (key == count)
                values.Add(value);
            else if (self.IsAbsence(values[key]))
                values[key] = value;
            else
                ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
        }

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey"/>
        public bool ContainsKey(int key)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._values is not { } values)
                return false;
            return unchecked((uint)key < (uint)values.Count) && !self.IsAbsence(values[key]);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the value is unspecified.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
        /// contains an element that has the specified key;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._values is not { } values)
                return None(out value);
            if (unchecked((uint)key >= (uint)values.Count))
                return None(out value);

            value = values[key];
            return !self.IsAbsence(value);
        }

        private void Put(int key, TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            TValueList values = self._values;
            int count = values.Count;
            if (key < 0)
            {
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(key));
                return;
            }

            if (key < count)
            {
                values[key] = value;
                return;
            }

            int absentCount = key - count;
            for (int i = 0; i < absentCount; ++i)
                values.Add(self._absenceMarker!);
            values.Add(value);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
        public TValue this[int key]
        {
            get => TryGetValueCore(key, out TValue? value)
                ? value
                : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
            set => Put(key, value);
        }

        private bool IsAbsence(TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return self._absenceComparer.Equals(value, self._absenceMarker!);
        }
    }
}
