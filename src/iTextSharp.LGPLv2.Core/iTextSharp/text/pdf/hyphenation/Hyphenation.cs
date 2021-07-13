using System.Text;

namespace iTextSharp.text.pdf.hyphenation
{
    /// <summary>
    /// This class represents a hyphenated word.
    /// @author Carlos Villegas
    /// </summary>
    public class Hyphenation
    {
        private readonly string _word;

        /// <summary>
        /// rawWord as made of alternating strings and {@link Hyphen Hyphen}
        /// instances
        /// </summary>
        internal Hyphenation(string word, int[] points)
        {
            _word = word;
            HyphenationPoints = points;
            Length = points.Length;
        }

        /// <summary>
        /// </summary>
        /// <returns>the number of hyphenation points in the word</returns>
        public int Length { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>the pre-break text, not including the hyphen character</returns>
        public string GetPreHyphenText(int index)
        {
            return _word.Substring(0, HyphenationPoints[index]);
        }

        /// <summary>
        /// </summary>
        /// <returns>the post-break text</returns>
        public string GetPostHyphenText(int index)
        {
            return _word.Substring(HyphenationPoints[index]);
        }

        /// <summary>
        /// </summary>
        /// <returns>the hyphenation points</returns>
        public int[] HyphenationPoints { get; }

        public override string ToString()
        {
            var str = new StringBuilder();
            var start = 0;
            for (var i = 0; i < Length; i++)
            {
                str.Append(_word.Substring(start, HyphenationPoints[i]) + "-");
                start = HyphenationPoints[i];
            }
            str.Append(_word.Substring(start));
            return str.ToString();
        }
    }
}