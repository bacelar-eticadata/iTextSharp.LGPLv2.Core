namespace iTextSharp.text.pdf
{
    /// <summary>
    /// Represents a pattern. Can be used in high-level constructs (Paragraph, Cell, etc.).
    /// </summary>
    public class PatternColor : ExtendedColor
    {

        /// <summary>
        /// Creates a color representing a pattern.
        /// </summary>
        /// <param name="painter">the actual pattern</param>
        public PatternColor(PdfPatternPainter painter) : base(TYPE_PATTERN, .5f, .5f, .5f)
        {
            Painter = painter;
        }

        /// <summary>
        /// Gets the pattern.
        /// </summary>
        /// <returns>the pattern</returns>
        public PdfPatternPainter Painter { get; private set; }

        public override bool Equals(object obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return Painter.GetHashCode();
        }
    }
}