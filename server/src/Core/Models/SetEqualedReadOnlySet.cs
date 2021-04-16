using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Chad.Models
{
    /// <summary>
    ///     集合相等的只读集合。重写了Equals为SetEquals。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    public class SetEqualedReadOnlySet<T> :
        ICollection,
        ICollection<T>,
        IImmutableSet<T>,
        IEquatable<IImmutableSet<T>>
    {
        private readonly ImmutableHashSet<T> _set;

        /// <summary>
        ///     基于现有的一些元素创建集合
        /// </summary>
        /// <param name="set">现有元素的序列</param>
        public SetEqualedReadOnlySet(T[] set)
        {
            _set = ImmutableHashSet.Create(set);
        }

        /// <summary>
        ///     基于现有的一些元素创建集合
        /// </summary>
        /// <param name="set">现有元素的序列</param>
        public SetEqualedReadOnlySet(IEnumerable<T> set)
        {
            _set = ImmutableHashSet.Create(set.ToArray());
        }

        /// <summary>
        ///     基于一个只读集合创建集合。
        /// </summary>
        /// <param name="set">只读集合。</param>
        public SetEqualedReadOnlySet(ImmutableHashSet<T> set)
        {
            _set = set;
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo(array, index);
        }

        /// <inheritdoc />
        public int Count => _set.Count;

        public bool IsSynchronized => false;
        public object SyncRoot => _set;

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException();
        }

        public bool IsReadOnly => true;

        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public bool Contains(T value)
        {
            return _set.Contains(value);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _set.ToArray().CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        /// <summary>
        ///     比较当前和输入的，两个集合的元素是否依次相等
        /// </summary>
        /// <param name="other">另一个集合</param>
        /// <returns>True，如果元素依次相等；反之亦然。</returns>
        public bool Equals(IImmutableSet<T>? other)
        {
            return other?.SetEquals(this) ?? false;
        }

        /// <inheritdoc />
        public IImmutableSet<T> Add(T value)
        {
            return new SetEqualedReadOnlySet<T>(_set.Add(value));
        }

        /// <inheritdoc />
        public IImmutableSet<T> Clear()
        {
            return new SetEqualedReadOnlySet<T>(_set.Clear());
        }

        /// <inheritdoc />
        public IImmutableSet<T> Except(IEnumerable<T> other)
        {
            return new SetEqualedReadOnlySet<T>(_set.Except(other));
        }

        /// <inheritdoc />
        public IImmutableSet<T> Intersect(IEnumerable<T> other)
        {
            return new SetEqualedReadOnlySet<T>(_set.Intersect(other));
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _set.IsSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _set.IsSupersetOf(other);
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            return _set.Overlaps(other);
        }

        /// <inheritdoc />
        public IImmutableSet<T> Remove(T value)
        {
            return new SetEqualedReadOnlySet<T>(_set.Remove(value));
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            return _set.SetEquals(other);
        }

        /// <inheritdoc />
        public IImmutableSet<T> SymmetricExcept(IEnumerable<T> other)
        {
            return new SetEqualedReadOnlySet<T>(_set.SymmetricExcept(other));
        }

        /// <inheritdoc />
        public bool TryGetValue(T equalValue, out T actualValue)
        {
            return _set.TryGetValue(equalValue, out actualValue);
        }

        /// <inheritdoc />
        public IImmutableSet<T> Union(IEnumerable<T> other)
        {
            return new SetEqualedReadOnlySet<T>(_set.Union(other));
        }

        public override int GetHashCode()
        {
            if (Count == 0) return 0;
            return HashCode.Combine(_set.First());
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is IImmutableSet<T> enu) return Equals(enu);
            return false;
        }

        public static implicit operator SetEqualedReadOnlySet<T>(T[] source)
        {
            return new(source);
        }
    }
}