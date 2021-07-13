using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.util;

namespace iTextSharp.text
{
    /// <summary>
    /// Summary description for SimpleTable.
    /// </summary>
    public class SimpleTable : Rectangle, IPdfPTableEvent, ITextElementArray
    {

        /// <summary>
        /// the content of a Table.
        /// </summary>
        private readonly ArrayList _content = new ArrayList();

        /// <summary>
        /// the width of the Table.
        /// </summary>
        private float _width;

        /// <summary>
        /// A RectangleCell is always constructed without any dimensions.
        /// Dimensions are defined after creation.
        /// </summary>
        public SimpleTable() : base(0f, 0f, 0f, 0f)
        {
            Border = BOX;
            BorderWidth = 2f;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns the alignment.</returns>
        public int Alignment { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>Returns the cellpadding.</returns>
        public float Cellpadding { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>Returns the cellspacing.</returns>
        public float Cellspacing { get; set; }

        /// <summary>
        /// @see com.lowagie.text.Element#type()
        /// </summary>
        public override int Type => TABLE;

        /// <summary>
        /// </summary>
        /// <returns>Returns the width.</returns>
        public override float Width
        {
            get => _width;
            set => _width = value;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns the widthpercentage.</returns>
        public float Widthpercentage { get; set; }

        /// <summary>
        /// @see com.lowagie.text.TextElementArray#add(java.lang.Object)
        /// </summary>
        public bool Add(object o)
        {
            try
            {
                AddElement((SimpleCell)o);
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
        public void AddElement(SimpleCell element)
        {
            if (!element.Cellgroup)
            {
                throw new BadElementException("You can't add cells to a table directly, add them to a row first.");
            }
            _content.Add(element);
        }

        /// <summary>
        /// Creates a PdfPTable object based on this TableAttributes object.
        /// @throws DocumentException
        /// </summary>
        /// <returns>a com.lowagie.text.pdf.PdfPTable object</returns>
        public PdfPTable CreatePdfPTable()
        {
            if (_content.Count == 0)
            {
                throw new BadElementException("Trying to create a table without rows.");
            }

            var rowx = (SimpleCell)_content[0];
            var columns = 0;
            foreach (SimpleCell cell in rowx.Content)
            {
                columns += cell.Colspan;
            }
            var widths = new float[columns];
            var widthpercentages = new float[columns];
            var table = new PdfPTable(columns)
            {
                TableEvent = this,
                HorizontalAlignment = Alignment
            };
            int pos;
            foreach (SimpleCell row in _content)
            {
                pos = 0;
                foreach (SimpleCell cell in row.Content)
                {
                    if (float.IsNaN(cell.SpacingLeft))
                    {
                        cell.SpacingLeft = Cellspacing / 2f;
                    }
                    if (float.IsNaN(cell.SpacingRight))
                    {
                        cell.SpacingRight = Cellspacing / 2f;
                    }
                    if (float.IsNaN(cell.SpacingTop))
                    {
                        cell.SpacingTop = Cellspacing / 2f;
                    }
                    if (float.IsNaN(cell.SpacingBottom))
                    {
                        cell.SpacingBottom = Cellspacing / 2f;
                    }
                    cell.Padding = Cellpadding;
                    table.AddCell(cell.CreatePdfPCell(row));
                    if (cell.Colspan == 1)
                    {
                        if (cell.Width > 0)
                        {
                            widths[pos] = cell.Width;
                        }

                        if (cell.Widthpercentage > 0)
                        {
                            widthpercentages[pos] = cell.Widthpercentage;
                        }
                    }
                    pos += cell.Colspan;
                }
            }
            var sumWidths = 0f;
            for (var i = 0; i < columns; i++)
            {
                if (widths[i].ApproxEquals(0))
                {
                    sumWidths = 0;
                    break;
                }
                sumWidths += widths[i];
            }
            if (sumWidths > 0)
            {
                table.TotalWidth = sumWidths;
                table.SetWidths(widths);
            }
            else
            {
                for (var i = 0; i < columns; i++)
                {
                    if (widthpercentages[i].ApproxEquals(0))
                    {
                        sumWidths = 0;
                        break;
                    }
                    sumWidths += widthpercentages[i];
                }
                if (sumWidths > 0)
                {
                    table.SetWidths(widthpercentages);
                }
            }
            if (_width > 0)
            {
                table.TotalWidth = _width;
            }
            if (Widthpercentage > 0)
            {
                table.WidthPercentage = Widthpercentage;
            }
            return table;
        }

        /// <summary>
        /// Creates a Table object based on this TableAttributes object.
        /// @throws BadElementException
        /// </summary>
        /// <returns>a com.lowagie.text.Table object</returns>
        public Table CreateTable()
        {
            if (_content.Count == 0)
            {
                throw new BadElementException("Trying to create a table without rows.");
            }

            var rowx = (SimpleCell)_content[0];
            var columns = 0;
            foreach (SimpleCell cell in rowx.Content)
            {
                columns += cell.Colspan;
            }
            var widths = new float[columns];
            var widthpercentages = new float[columns];
            var table = new Table(columns)
            {
                Alignment = Alignment,
                Spacing = Cellspacing,
                Padding = Cellpadding
            };
            table.CloneNonPositionParameters(this);
            int pos;
            foreach (SimpleCell row in _content)
            {
                pos = 0;
                foreach (SimpleCell cell in row.Content)
                {
                    table.AddCell(cell.CreateCell(row));
                    if (cell.Colspan == 1)
                    {
                        if (cell.Width > 0)
                        {
                            widths[pos] = cell.Width;
                        }

                        if (cell.Widthpercentage > 0)
                        {
                            widthpercentages[pos] = cell.Widthpercentage;
                        }
                    }
                    pos += cell.Colspan;
                }
            }
            var sumWidths = 0f;
            for (var i = 0; i < columns; i++)
            {
                if (widths[i].ApproxEquals(0))
                {
                    sumWidths = 0;
                    break;
                }
                sumWidths += widths[i];
            }
            if (sumWidths > 0)
            {
                table.Width = sumWidths;
                table.Locked = true;
                table.Widths = widths;
            }
            else
            {
                for (var i = 0; i < columns; i++)
                {
                    if (widthpercentages[i].ApproxEquals(0))
                    {
                        sumWidths = 0;
                        break;
                    }
                    sumWidths += widthpercentages[i];
                }
                if (sumWidths > 0)
                {
                    table.Widths = widthpercentages;
                }
            }
            if (_width > 0)
            {
                table.Width = _width;
                table.Locked = true;
            }
            else if (Widthpercentage > 0)
            {
                table.Width = Widthpercentage;
            }
            return table;
        }
        /// <summary>
        /// @see com.lowagie.text.Element#isNestable()
        /// @since   iText 2.0.8
        /// </summary>
        public override bool IsNestable()
        {
            return true;
        }

        /// <summary>
        /// @see com.lowagie.text.pdf.PdfPTableEvent#tableLayout(com.lowagie.text.pdf.PdfPTable, float[][], float[], int, int, com.lowagie.text.pdf.PdfContentByte[])
        /// </summary>
        public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        {
            var width = widths[0];
            var rect = new Rectangle(width[0], heights[heights.Length - 1], width[width.Length - 1], heights[0]);
            rect.CloneNonPositionParameters(this);
            var bd = rect.Border;
            rect.Border = NO_BORDER;
            canvases[PdfPTable.BACKGROUNDCANVAS].Rectangle(rect);
            rect.Border = bd;
            rect.BackgroundColor = null;
            canvases[PdfPTable.LINECANVAS].Rectangle(rect);
        }
    }
}
