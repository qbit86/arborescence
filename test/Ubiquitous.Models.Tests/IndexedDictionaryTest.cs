namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    public class IndexedDictionaryTest
    {
        private struct StructBackingStore<TValue> : IList<TValue>
        {
            private TValue _item0;
            private TValue _item1;
            private TValue _item2;
            private TValue _item3;

            public IEnumerator<TValue> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(TValue item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(TValue item)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            public bool Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            public int Count => 4;

            public bool IsReadOnly => true;

            public int IndexOf(TValue item)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, TValue item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public TValue this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return _item0;
                        case 1:
                            return _item1;
                        case 2:
                            return _item2;
                        case 3:
                            return _item3;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            _item0 = value;
                            return;
                        case 1:
                            _item1 = value;
                            return;
                        case 2:
                            _item2 = value;
                            return;
                        case 3:
                            _item3 = value;
                            return;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        [Fact]
        public void Struct_backing_store_should_be_modifiable()
        {
            var backingStore = new StructBackingStore<int>();
            var indexedDictionary = new IndexedDictionary<int, StructBackingStore<int>>(backingStore);
            const int expectedValue = 23;
            indexedDictionary[2] = expectedValue;
            int actualValue = indexedDictionary[2];

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
