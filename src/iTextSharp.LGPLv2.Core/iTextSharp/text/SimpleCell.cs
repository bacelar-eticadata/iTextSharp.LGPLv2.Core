using iTextSharp.text.pdf;
using System;
using System.Collections;

namespace iTextSharp.text
{
    /// <summary>
    /// Summary description for SimpleCell.
    /// </summary>
    public class SimpleCell : Rectangle, IPdfPCellEvent, ITextElementArray
    {

        /// <summary>
        /// the CellAttributes object represents a cell.
        /// </summary>
        public new const bool CELL = false;

        /// <summary>
        /// the CellAttributes object represents a row.
        /// </summary>
        public new const bool ROW = true;
        /// <summary>
        /// Indicates that the largest ascender height should be used to determine the
        /// height of the first line.  Note that this only has an effect when rendered
        /// to PDF.  Setting this to true can help with vertical alignment problems.
        /// </summary>
        protected bool useAscender;

        /// <summary>
        /// Adjusts the cell contents to compensate for border widths.  Note that
        /// this only has an effect when rendered to PDF.
        /// </summary>
        protected bool useBorderPadding;

        /// <summary>
        /// Indicates that the largest descender height should be added to the height of
        /// the last line (so characters like y don't dip into the border).   Note that
        /// this only has an effect when rendered to PDF.
        /// </summary>
        protected bool useDescender;

        /// <summary>
        /// the colspan of a Cell
        /// </summary>
        private int _colspan = 1;

        /// <summary>
        /// an extra spacing variable
        /// </summary>
        private float _spacingTop = float.NaN;

        /// <summary>
        /// A CellAttributes object is always constructed without any dimensions.
        /// Dimensions are defined after creation.
        /// </summary>
        /// <param name="row">only true if the CellAttributes object represents a row.</param>
        public SimpleCell(bool row) : base(0f, 0f, 0f, 0f)
        {
            Cellgroup = row;
            Border = BOX;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns the cellgroup.</returns>
        public bool Cellgroup { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>Returns the colspan.</returns>
        public int Colspan
        {
            get => _colspan;
            set
            {
                if (value > 0)
                {
                    _colspan = value;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns the horizontal alignment.</returns>
        public int HorizontalAlignment { get; set; } = ALIGN_UNDEFINED;

        /// <summary>
        /// Sets the padding parameters if they are undefined.
        /// </summary>
        public float Padding
        {
            set
            {
                if (float.IsNaN(PaddingRight))
                {
                    PaddingRight = value;
                }
                if (float.IsNaN(PaddingLeft))
                {
                    PaddingLeft = value;
                }
                if (float.IsNaN(PaddingBottom))
                {
                    PaddingBottom = value;
                }
                if (float.IsNaN(PaddingTop))
                {
                    PaddingTop = value;
                }
            }
        }

        /// <summary>
        /// </summary>
        public float PaddingBottom { get; set; } = float.NaN;

        public float PaddingLeft { get; set; } = float.NaN;

        public float PaddingRight { get; set; } = float.NaN;

        public float PaddingTop { get; set; } = float.NaN;

        /// <summary>
        /// </summary>
        /// <returns>Returns the spacing.</returns>
        public float Spacing
        {
            set
            {
                SpacingLeft = value;
                SpacingRight = value;
                _spacingTop = value;
                SpacingBottom = value;
            }

        }

        public float SpacingBottom { get; set; } = float.NaN;

        public float SpacingLeft { get; set; } = float.NaN;

        public float SpacingRight { get; set; } = float.NaN;

        public float SpacingTop
        {
            get => _spacingTop;
            set => _spacingTop = value;
        }

        public override int Type => Element.CELL;

        /// <summary>
        /// </summary>
        /// <returns>Returns the useAscender.</returns>
        public bool UseAscender
        {
            get => useAscender;
            set => useAscender = value;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns the useBorderPadding.</returns>
        public bool UseBorderPadding
        {
            get => useBorderPadding;
            set => useBorderPadding = value;
        }

        public bool UseDescender
        {
            get => useDescender;
            set => useDescender = value;
        }

        public int VerticalAlignment { get; set; } = ALIGN_UNDEFINED;

        /// <summary>
        /// </summary>
        /// <returns>Returns the width.</returns>
        public new float Width { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>Returns the widthpercentage.</returns>
        public float Widthpercentage { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>Returns the content.</returns>
        internal ArrayList Content { get; } = new ArrayList();

        /// <summary>
        /// @see com.lowagie.text.TextElementArray#add(java.lang.Object)
        /// </summary>
        public bool Add(object o)
        {
            try
            {
                AddElement((IElement)o);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds content to this object.
        /// @throws BadElementException
        /// </summary>
        /// <param name="element"></param>
        public void AddElement(IElement element)
        {
            if (Cellgroup)
            {
                if (element is SimpleCell)
                {
                    if (((SimpleCell)element).Cellgroup)
                    {
                        throw new BadElementException("You can't add one row to another row.");
                    }
                    Content.Add(element);
                    return;
                }
                else
                {
                    throw new BadElementException("You can only add cells to rows, no objects of type " + element.GetType());
                }
            }
            if (element.Type == PARAGRAPH
                    || element.Type == PHRASE
                    || element.Type == ANCHOR
                    || element.Type == CHUNK
                    || element.Type == LIST
                    || element.Type == MARKED
                    || element.Type == JPEG
                    || element.Type == JPEG2000
                    || element.Type == JBIG2
                    || element.Type == IMGRAW
                    || element.Type == IMGTEMPLATE)
            {
                Content.Add(element);
            }
            else
            {
                throw new BadElementException("You can't add an element of type " + element.GetType() + " to a SimpleCell.");
            }
        }

        /// <summary>
        /// @see com.lowagie.text.pdf.PdfPCellEvent#cellLayout(com.lowagie.text.pdf.PdfPCell, com.lowagie.text.Rectangle, com.lowagie.text.pdf.PdfContentByte[])
        /// </summary>
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            var spLeft = SpacingLeft;
            if (float.IsNaN(spLeft))
            {
                spLeft = 0f;
            }

            var spRight = SpacingRight;
            if (float.IsNaN(spRight))
            {
                spRight = 0f;
            }

            var spTop = _spacingTop;
            if (float.IsNaN(spTop))
            {
                spTop = 0f;
            }

            var spBottom = SpacingBottom;
            if (float.IsNaN(spBottom))
            {
                spBottom = 0f;
            }

            var rect = new Rectangle(position.GetLeft(spLeft), position.GetBottom(spBottom), position.GetRight(spRight), position.GetTop(spTop));
            rect.CloneNonPositionParameters(this);
            canvases[PdfPTable.BACKGROUNDCANVAS].Rectangle(rect);
            rect.BackgroundColor = null;
            canvases[PdfPTable.LINECANVAS].Rectangle(rect);
        }

        /// <summary>
        /// Creates a Cell with these attributes.
        /// @throws BadElementException
        /// </summary>
        /// <param name="rowAttributes"></param>
        /// <returns>a cell based on these attributes.</returns>
        public Cell CreateCell(SimpleCell rowAttributes)
        {
            var cell = new Cell();
            cell.CloneNonPositionParameters(rowAttributes);
            cell.SoftCloneNonPositionParameters(this);
            cell.Colspan = _colspan;
            cell.HorizontalAlignment = HorizontalAlignment;
            cell.VerticalAlignment = VerticalAlignment;
            cell.UseAscender = useAscender;
            cell.UseBorderPadding = useBorderPadding;
            cell.UseDescender = useDescender;
            foreach (IElement element in Content)
            {
                cell.AddElement(element);
            }
            return cell;
        }

        /// <summary>
        /// Creates a PdfPCell with these attributes.
        /// </summary>
        /// <param name="rowAttributes"></param>
        /// <returns>a PdfPCell based on these attributes.</returns>
        public PdfPCell CreatePdfPCell(SimpleCell rowAttributes)
        {
            var cell = new PdfPCell
            {
                Border = NO_BORDER
            };
            var tmp = new SimpleCell(CELL)
            {
                SpacingLeft = SpacingLeft,
                SpacingRight = SpacingRight,
                SpacingTop = _spacingTop,
                SpacingBottom = SpacingBottom
            };
            tmp.CloneNonPositionParameters(rowAttributes);
            tmp.SoftCloneNonPositionParameters(this);
            cell.CellEvent = tmp;
            cell.HorizontalAlignment = rowAttributes.HorizontalAlignment;
            cell.VerticalAlignment = rowAttributes.VerticalAlignment;
            cell.UseAscender = rowAttributes.useAscender;
            cell.UseBorderPadding = rowAttributes.useBorderPadding;
            cell.UseDescender = rowAttributes.useDescender;
            cell.Colspan = _colspan;
            if (HorizontalAlignment != ALIGN_UNDEFINED)
            {
                cell.HorizontalAlignment = HorizontalAlignment;
            }

            if (VerticalAlignment != ALIGN_UNDEFINED)
            {
                cell.VerticalAlignment = VerticalAlignment;
            }

            if (useAscender)
            {
                cell.UseAscender = useAscender;
            }

            if (useBorderPadding)
            {
                cell.UseBorderPadding = useBorderPadding;
            }

            if (useDescender)
            {
                cell.UseDescender = useDescender;
            }

            float p;
            var spLeft = SpacingLeft;
            if (float.IsNaN(spLeft))
            {
                spLeft = 0f;
            }

            var spRight = SpacingRight;
            if (float.IsNaN(spRight))
            {
                spRight = 0f;
            }

            var spTop = _spacingTop;
            if (float.IsNaN(spTop))
            {
                spTop = 0f;
            }

            var spBottom = SpacingBottom;
            if (float.IsNaN(spBottom))
            {
                spBottom = 0f;
            }

            p = PaddingLeft;
            if (float.IsNaN(p))
            {
                p = 0f;
            }

            cell.PaddingLeft = p + spLeft;
            p = PaddingRight;
            if (float.IsNaN(p))
            {
                p = 0f;
            }

            cell.PaddingRight = p + spRight;
            p = PaddingTop;
            if (float.IsNaN(p))
            {
                p = 0f;
            }

            cell.PaddingTop = p + spTop;
            p = PaddingBottom;
            if (float.IsNaN(p))
            {
                p = 0f;
            }

            cell.PaddingBottom = p + spBottom;
            foreach (IElement element in Content)
            {
                cell.AddElement(element);
            }
            return cell;
        }
    }
}
