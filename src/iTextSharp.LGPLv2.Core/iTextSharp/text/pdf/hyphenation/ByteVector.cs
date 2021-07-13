using System;

namespace iTextSharp.text.pdf.hyphenation
{
    /// <summary>
    /// This class implements a simple byte vector with access to the
    /// underlying array.
    /// @author Carlos Villegas
    /// </summary>
    public class ByteVector
    {
        /// <summary>
        /// Capacity increment size
        /// </summary>
        private static readonly int _defaultBlockSize = 2048;

        private readonly int _blockSize;

        public ByteVector() : this(_defaultBlockSize)
        {
        }

        public ByteVector(int capacity)
        {
            if (capacity > 0)
            {
                _blockSize = capacity;
            }
            else
            {
                _blockSize = _defaultBlockSize;
            }

            Arr = new byte[_blockSize];
            Length = 0;
        }

        public ByteVector(byte[] a)
        {
            _blockSize = _defaultBlockSize;
            Arr = a;
            Length = 0;
        }

        public ByteVector(byte[] a, int capacity)
        {
            if (capacity > 0)
            {
                _blockSize = capacity;
            }
            else
            {
                _blockSize = _defaultBlockSize;
            }

            Arr = a;
            Length = 0;
        }

        public byte[] Arr { get; private set; }

        /// <summary>
        /// returns current capacity of array
        /// </summary>
        public int Capacity => Arr.Length;

        /// <summary>
        /// return number of items in array
        /// </summary>
        public int Length { get; private set; }

        public byte this[int index]
        {
            get => Arr[index];

            set => Arr[index] = value;
        }

        /// <summary>
        /// This is to implement memory allocation in the array. Like Malloc().
        /// </summary>
        public int Alloc(int size)
        {
            var index = Length;
            var len = Arr.Length;
            if (Length + size >= len)
            {
                var aux = new byte[len + _blockSize];
                Array.Copy(Arr, 0, aux, 0, len);
                Arr = aux;
            }
            Length += size;
            return index;
        }

        public void TrimToSize()
        {
            if (Length < Arr.Length)
            {
                var aux = new byte[Length];
                Array.Copy(Arr, 0, aux, 0, Length);
                Arr = aux;
            }
        }
    }
}