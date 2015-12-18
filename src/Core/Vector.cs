using System;

namespace Core
{
    public interface IVector<T>
    {
        int Count { get; set; }
        int Capacity { get; set; }
        void EnsureCapacity(int value);
        T this[int index] { get; set; }
        void Push(T value);
        T Pop();
        T[] Data { get; }
    }

    public sealed class Vector<T> : IVector<T>
    {
        private static readonly T[] Empty = new T[0];
        private T[] data = Empty;
        private int size;

        public Vector()
        {}

        public Vector(int capacity)
        { EnsureCapacity(capacity); }
        
        public int Count
        {
            get { return size; }
            set
            {
                EnsureCapacity(value);
                size = value;
            }
        }

        public int Capacity
        {
            get { return data.Length; }
            set
            {
                if (size>value)
                    size = value;
                Array.Resize(ref data, value);
            }
        }

        public void EnsureCapacity(int value)
        {
            if (Capacity<value)
                Capacity = value;
        }

        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public void Push(T value)
        {
            if (Capacity==size)
                Capacity *= 2;
            data[size++] = value;
        }

        public T Pop()
        { return data[size--]; }

        public T[] Data { get { return data; } }
    }
}
