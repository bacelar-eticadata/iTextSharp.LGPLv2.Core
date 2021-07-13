namespace iTextSharp.text.pdf
{
    /// <summary>
    ///  PdfRectangle  is the PDF Rectangle object.
    ///
    /// Rectangles are used to describe locations on the page and bounding boxes for several
    /// objects in PDF, such as fonts. A rectangle is represented as an  array  of
    /// four numbers, specifying the lower lef <I>x</I>, lower left <I>y</I>, upper right <I>x</I>,
    /// and upper right <I>y</I> coordinates of the rectangle, in that order.
    /// This object is described in the 'Portable Document Format Reference Manual version 1.3'
    /// section 7.1 (page 183).
    /// @see     iTextSharp.text.Rectangle
    /// @see     PdfArray
    /// </summary>
    public class PdfRectangle : PdfArray
    {

        /// <summary>
        /// constructors
        /// </summary>
        /// <summary>
        /// Constructs a  PdfRectangle -object.
        /// @since       rugPdf0.10
        /// </summary>
        /// <param name="llx">lower left x</param>
        /// <param name="lly">lower left y</param>
        /// <param name="urx">upper right x</param>
        /// <param name="ury">upper right y</param>
        /// <param name="rotation"></param>
        public PdfRectangle(float llx, float lly, float urx, float ury, int rotation)
        {
            if (rotation == 90 || rotation == 270)
            {
                Left = lly;
                Bottom = llx;
                Right = ury;
                Top = urx;
            }
            else
            {
                Left = llx;
                Bottom = lly;
                Right = urx;
                Top = ury;
            }
            base.Add(new PdfNumber(Left));
            base.Add(new PdfNumber(Bottom));
            base.Add(new PdfNumber(Right));
            base.Add(new PdfNumber(Top));
        }

        public PdfRectangle(float llx, float lly, float urx, float ury) : this(llx, lly, urx, ury, 0)
        {
        }

        /// <summary>
        /// Constructs a  PdfRectangle -object starting from the origin (0, 0).
        /// </summary>
        /// <param name="urx">upper right x</param>
        /// <param name="ury">upper right y</param>
        /// <param name="rotation"></param>
        public PdfRectangle(float urx, float ury, int rotation) : this(0, 0, urx, ury, rotation) { }

        public PdfRectangle(float urx, float ury) : this(0, 0, urx, ury, 0)
        {
        }

        /// <summary>
        /// Constructs a  PdfRectangle -object with a  Rectangle -object.
        /// </summary>
        /// <param name="rectangle">a  Rectangle </param>
        /// <param name="rotation"></param>
        public PdfRectangle(Rectangle rectangle, int rotation) : this(rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Top, rotation) { }

        public PdfRectangle(Rectangle rectangle) : this(rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Top, 0)
        {
        }

        /// <summary>
        /// methods
        /// </summary>

        public float Bottom { get; private set; }

        public float Height => Top - Bottom;

        public float Left { get; private set; }

        /// <summary>
        /// Returns the high level version of this PdfRectangle
        /// </summary>
        /// <returns>this PdfRectangle translated to class Rectangle</returns>
        public Rectangle Rectangle => new Rectangle(Left, Bottom, Right, Top);

        public float Right { get; private set; }

        public PdfRectangle Rotate => new PdfRectangle(Bottom, Left, Top, Right, 0);

        public float Top { get; private set; }

        public float Width => Right - Left;

        public override bool Add(PdfObject obj)
        {
            return false;
        }

        /// <summary>
        /// Block changes to the underlying PdfArray
        /// @since 2.1.5
        /// </summary>
        /// <param name="values">stuff we'll ignore. Ha!</param>
        /// <returns>false. You can't add anything to a PdfRectangle</returns>

        public override bool Add(float[] values)
        {
            return false;
        }

        /// <summary>
        /// Block changes to the underlying PdfArray
        /// @since 2.1.5
        /// </summary>
        /// <param name="values">stuff we'll ignore. Ha!</param>
        /// <returns>false. You can't add anything to a PdfRectangle</returns>

        public override bool Add(int[] values)
        {
            return false;
        }

        /// <summary>
        /// Block changes to the underlying PdfArray
        /// @since 2.1.5
        /// </summary>
        /// <param name="obj">Ignored.</param>
        public override void AddFirst(PdfObject obj)
        {
        }

        /// <summary>
        /// Returns the lower left x-coordinate.
        /// </summary>
        /// <returns>the lower left x-coordinaat</returns>
        /// <summary>
        /// Returns the upper right x-coordinate.
        /// </summary>
        /// <returns>the upper right x-coordinate</returns>
        /// <summary>
        /// Returns the upper right y-coordinate.
        /// </summary>
        /// <returns>the upper right y-coordinate</returns>
        /// <summary>
        /// Returns the lower left y-coordinate.
        /// </summary>
        /// <returns>the lower left y-coordinate</returns>
        /// <summary>
        /// Returns the lower left x-coordinate, considering a given margin.
        /// </summary>
        /// <param name="margin">a margin</param>
        /// <returns>the lower left x-coordinate</returns>

        public float GetBottom(int margin)
        {
            return Bottom + margin;
        }

        public float GetLeft(int margin)
        {
            return Left + margin;
        }

        /// <summary>
        /// Returns the upper right x-coordinate, considering a given margin.
        /// </summary>
        /// <param name="margin">a margin</param>
        /// <returns>the upper right x-coordinate</returns>

        public float GetRight(int margin)
        {
            return Right - margin;
        }

        /// <summary>
        /// Returns the upper right y-coordinate, considering a given margin.
        /// </summary>
        /// <param name="margin">a margin</param>
        /// <returns>the upper right y-coordinate</returns>

        public float GetTop(int margin)
        {
            return Top - margin;
        }
    }
}