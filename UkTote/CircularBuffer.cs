using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace UkTote
{
    public class CircularBuffer<T> : ICollection<T>, ICollection, IEnumerable
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CircularBuffer<T>));

        private int _capacity;
        private int _size;
        private int _head;
        private int _tail;
        private T[] _buffer;

        [NonSerialized()]
        private object _syncRoot;

        public CircularBuffer(int capacity)
            : this(capacity, false)
        {
        }

        public CircularBuffer(int capacity, bool allowOverflow)
        {
            if (capacity < 0)
                throw new ArgumentException("Zero capacity", "capacity");

            this._capacity = capacity;
            _size = 0;
            _head = 0;
            _tail = 0;
            _buffer = new T[capacity];
            AllowOverflow = allowOverflow;
        }

        public bool AllowOverflow
        {
            get;
            set;
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value == _capacity)
                    return;

                if (value < _size)
                    throw new ArgumentOutOfRangeException("value", "too small");

                var dst = new T[value];
                if (_size > 0)
                    CopyTo(dst);
                _buffer = dst;

                _capacity = value;
            }
        }

        public int Size => _size;

        public bool Contains(T item)
        {
            int bufferIndex = _head;
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _size; i++, bufferIndex++)
            {
                if (bufferIndex == _capacity)
                    bufferIndex = 0;

                if (item == null && _buffer[bufferIndex] == null)
                    return true;
                else if ((_buffer[bufferIndex] != null) &&
                    comparer.Equals(_buffer[bufferIndex], item))
                    return true;
            }

            return false;
        }

        public void Clear()
        {
            _size = 0;
            _head = 0;
            _tail = 0;
        }

        public int Put(T[] src)
        {
            return Put(src, 0, src.Length);
        }

        public int Put(T[] src, int offset, int count)
        {
            if (!AllowOverflow && count > _capacity - _size)
                throw new InvalidOperationException("buffer overflow");

            int srcIndex = offset;
            for (int i = 0; i < count; i++, _tail++, srcIndex++)
            {
                if (_tail == _capacity)
                    _tail = 0;
                _buffer[_tail] = src[srcIndex];
            }
            _size = Math.Min(_size + count, _capacity);
            return count;
        }

        public void Put(T item)
        {
            if (!AllowOverflow && _size == _capacity)
                throw new InvalidOperationException("buffer overflow");

            _buffer[_tail] = item;
            if (++_tail == _capacity)
                _tail = 0;
            _size++;
        }

        public void Skip(int count)
        {
            _head += count;
            if (_head >= _capacity)
                _head -= _capacity;
        }

        public T[] Get(int count)
        {
            var dst = new T[count];
            Get(dst);
            return dst;
        }

        public T[] Peek(int count)
        {
            _logger.DebugFormat("Peek({0}): {1}", count, this.ToString());
            
            var dst = new T[count];
            Peek(dst, 0, dst.Length);
            return dst;
        }

        public int Peek(T[] dst, int offset, int count)
        {
            _logger.DebugFormat("Peek({0}, {1}, {2}): {3}", dst.Length, offset, count, ToString());

            int realCount = Math.Min(count, _size);
            int dstIndex = offset;
            var curr = _head;
            for (int i = 0; i < realCount; i++, curr++, dstIndex++)
            {
                if (curr == _capacity)
                    curr = 0;
                dst[dstIndex] = _buffer[curr];
            }
            
            return realCount;
        }

        public int Get(T[] dst)
        {
            return Get(dst, 0, dst.Length);
        }

        public int Get(T[] dst, int offset, int count)
        {
            int realCount = Math.Min(count, _size);
            int dstIndex = offset;
            for (int i = 0; i < realCount; i++, _head++, dstIndex++)
            {
                if (_head == _capacity)
                    _head = 0;
                dst[dstIndex] = _buffer[_head];
            }
            if (_head == _capacity)
                _head = 0;
            _size -= realCount;
            return realCount;
        }

        public T Get()
        {
            if (_size == 0)
                throw new InvalidOperationException("buffer empty");

            var item = _buffer[_head];
            if (++_head == _capacity)
                _head = 0;
            _size--;
            return item;
        }

        public T Peek()
        {
            _logger.DebugFormat("Peek: {0}", ToString());
            if (_size == 0)
                throw new InvalidOperationException("buffer empty");

            return _buffer[_head];
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, _size);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (count > _size)
                throw new ArgumentOutOfRangeException("count", "MessageReadCountTooLarge");

            int bufferIndex = _head;
            for (int i = 0; i < count; i++, bufferIndex++, arrayIndex++)
            {
                if (bufferIndex == _capacity)
                    bufferIndex = 0;
                array[arrayIndex] = _buffer[bufferIndex];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            int bufferIndex = _head;
            for (int i = 0; i < _size; i++, bufferIndex++)
            {
                if (bufferIndex == _capacity)
                    bufferIndex = 0;

                yield return _buffer[bufferIndex];
            }
        }

        public T[] GetBuffer()
        {
            return _buffer;
        }

        public T[] ToArray()
        {
            var dst = new T[_size];
            CopyTo(dst);
            return dst;
        }

        #region ICollection<T> Members

        int ICollection<T>.Count => Size;

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add(T item)
        {
            Put(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            if (_size == 0)
                return false;

            Get();
            return true;
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection Members

        int ICollection.Count => Size;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyTo((T[])array, arrayIndex);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("capacity({0}) size({1}) head({2}) tail({3})", _capacity, _size, _head, _tail);
        }
    }
}