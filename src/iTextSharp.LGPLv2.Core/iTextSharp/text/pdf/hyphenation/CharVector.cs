using System;

namespace iTextSharp.text.pdf.hyphenation
{
    /// <summary>
    /// This class implements a simple char vector with access to the
    /// underlying array.
    /// @author Carlos Villegas
    /// </summary>
    public class CharVector : ICloneable
    {
        /// <summary>
        /// Capacity increment size
        /// </summary>
        private static readonly int _defaultBlockSize = 2048;

        private readonly int _blockSize;

        public CharVector() : this(_defaultBlockSize)
        {
        }

        public CharVector(int capacity)
        {
            if (capacity > 0)
            {
                _blockSize = capacity;
            }
            else
            {
                _blockSize = _defaultBlockSize;
            }

            Arr = new char[_blockSize];
            Length = 0;
        }

        public CharVector(char[] a)
        {
            _blockSize = _defaultBlockSize;
            Arr = a;
            Length = a.Length;
        }

        public CharVector(char[] a, int capacity)
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
            Length = a.Length;
        }

        public char[] Arr { get; private set; }

        /// <summary>
        /// returns current capacity of array
        /// </summary>
        public int Capacity => Arr.Length;

        /// <summary>
        /// return number of items in array
        /// </summary>
        public int Length { get; private set; }

        public char this[int index]
        {
            get => Arr[index];

            set => Arr[index] = value;
        }

        public int Alloc(int size)
        {
            var index = Length;
            var len = Arr.Length;
            if (Length + size >= len)
            {
                var aux = new char[len + _blockSize];
                Array.Copy(Arr, 0, aux, 0, len);
                Arr = aux;
            }
            Length += size;
            return index;
        }

        /// <summary>
        /// Reset Vector but don't resize or clear elements
        /// </summary>
        public void Clear()
        {
            Length = 0;
        }

        public object Clone()
        {
            var cv = new CharVector((char[])Arr.Clone(), _blockSize)
            {
                Length = Length
            };
            return cv;
        }

        public void TrimToSize()
        {
            if (Length < Arr.Length)
            {
                var aux = new char[Length];
                Array.Copy(Arr, 0, aux, 0, Length);
                Arr = aux;
            }
        }
    }
}