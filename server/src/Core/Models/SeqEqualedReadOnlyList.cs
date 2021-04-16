using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Serialization;

namespace Chad.Models
{
    public class SeqEqualedReadOnlyList<T> :
        ICollection<T>,
        IImmutableList<T>,
        IEquatable<IImmutableList<T>>
    {
        private readonly ImmutableList<T> _list;

        /// <summary>
        ///     基于现有的一些元素创建集合
        /// </summary>
        /// <param name="set">现有元素的序列</param>
        [JsonConstructor]
        public SeqEqualedReadOnlyList(T[] set)
        {
            _list = ImmutableList.Create(set);
        }

        /// <summary>
        ///     基于现有的一些元素创建集合
        /// </summary>
        /// <param name="set">现有元素的序列</param>
        public SeqEqualedReadOnlyList(IEnumerable<T> set)
        {
            _list = ImmutableList.Create(set.ToArray());
        }

        /// <summary>
        ///     基于一个只读集合创建序列。
        /// </summary>
        /// <param name="set">只读集合。</param>
        public SeqEqualedReadOnlyList(ImmutableList<T> set)
        {
            _list = set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count => _list.Count;
        public bool IsReadOnly { get; }

        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException();
        }

        public bool Equals(IImmutableList<T>? other)
        {
            return other is not null && this.SequenceEqual(other);
        }

        public T this[int index] => _list[index];

        public IImmutableList<T> Add(T value)
        {
            return new SeqEqualedReadOnlyList<T>(_list.Add(value));
        }

        public IImmutableList<T> AddRange(IEnumerable<T> items)
        {
            return new SeqEqualedReadOnlyList<T>(_list.AddRange(items));
        }

        public IImmutableList<T> Clear()
        {
            return _list.Clear();
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            return _list.IndexOf(item, index, count, equalityComparer);
        }

        public IImmutableList<T> Insert(int index, T element)
        {
            return new SeqEqualedReadOnlyList<T>(_list.Insert(index, element));
        }

        public IImmutableList<T> InsertRange(int index, IEnumerable<T> items)
        {
            return new SeqEqualedReadOnlyList<T>(_list.InsertRange(index, items));
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            return _list.LastIndexOf(item, index, count, equalityComparer);
        }

        public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            return new SeqEqualedReadOnlyList<T>(_list.Remove(value, equalityComparer));
        }

        public IImmutableList<T> RemoveAll(Predicate<T> match)
        {
            return new SeqEqualedReadOnlyList<T>(_list.RemoveAll(match));
        }

        public IImmutableList<T> RemoveAt(int index)
        {
            return new SeqEqualedReadOnlyList<T>(_list.RemoveAt(index));
        }

        public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            return new SeqEqualedReadOnlyList<T>(_list.RemoveRange(items, equalityComparer));
        }

        public IImmutableList<T> RemoveRange(int index, int count)
        {
            return new SeqEqualedReadOnlyList<T>(_list.RemoveRange(index, count));
        }

        public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            return new SeqEqualedReadOnlyList<T>(_list.Replace(oldValue, newValue, equalityComparer));
        }

        public IImmutableList<T> SetItem(int index, T value)
        {
            return new SeqEqualedReadOnlyList<T>(_list.SetItem(index, value));
        }

        public bool Equals(IEnumerable<T>? other)
        {
            return other is not null && this.SequenceEqual(other);
        }

        public override int GetHashCode()
        {
            if (Count == 0) return 0;
            return HashCode.Combine(_list[0]);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is IImmutableList<T> enu) return Equals(enu);
            return false;
        }

        public static implicit operator SeqEqualedReadOnlyList<T>(T[] source)
        {
            return new(source);
        }
    }
}