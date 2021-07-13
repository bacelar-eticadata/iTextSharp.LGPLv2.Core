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

        /// <summary>
        /// The encapsulated array
        /// </summary>
        private byte[] _arr;

        /// <summary>
        /// Points to next free item
        /// </summary>
        private int _n;

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

            _arr = new byte[_blockSize];
            _n = 0;
        }

        public ByteVector(byte[] a)
        {
            _blockSize = _defaultBlockSize;
            _arr = a;
            _n = 0;
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

            _arr = a;
            _n = 0;
        }

        public byte[] Arr => _arr;

        /// <summary>
        /// returns current capacity of array
        /// </summary>
        public int Capacity => _arr.Length;

        /// <summary>
        /// return number of items in array
        /// </summary>
        public int Length => _n;

        public byte this[int index]
        {
            get => _arr[index];

            set => _arr[index] = value;
        }

        /// <summary>
        /// This is to implement memory allocation in the array. Like Malloc().
        /// </summary>
        public int Alloc(int size)
        {
            var index = _n;
            var len = _arr.Length;
            if (_n + size >= len)
            {
                var aux = new byte[len + _blockSize];
                Array.Copy(_arr, 0, aux, 0, len);
                _arr = aux;
            }
            _n += size;
            return index;
        }

        public void TrimToSize()
        {
            if (_n < _arr.Length)
            {
                var aux = new byte[_n];
                Array.Copy(_arr, 0, aux, 0, _n);
                _arr = aux;
            }
        }
    }
}