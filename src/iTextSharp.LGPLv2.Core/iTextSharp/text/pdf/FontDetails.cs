using System.Collections;

namespace iTextSharp.text.pdf
{
    /// <summary>
    /// Each font in the document will have an instance of this class
    /// where the characters used will be represented.
    /// @author  Paulo Soares (psoares@consiste.pt)
    /// </summary>
    internal class FontDetails
    {
        /// <summary>
        /// Indicates if all the glyphs and widths for that particular
        /// encoding should be included in the document.
        /// </summary>
        protected bool subset = true;
        private readonly CjkFont _cjkFont;

        private readonly IntHashtable _cjkTag;

        /// <summary>
        /// The font type
        /// </summary>
        private readonly int _fontType;

        /// <summary>
        /// The map used with double byte encodings. The key is Int(glyph) and the
        /// value is int[]{glyph, width, Unicode code}
        /// </summary>
        private readonly Hashtable _longTag;

        /// <summary>
        /// The array used with single byte encodings
        /// </summary>
        private readonly byte[] _shortTag;

        /// <summary>
        ///  true  if the font is symbolic
        /// </summary>
        private readonly bool _symbolic;

        /// <summary>
        /// The font if its an instance of  TrueTypeFontUnicode
        /// </summary>
        private readonly TrueTypeFontUnicode _ttu;

        /// <summary>
        /// Each font used in a document has an instance of this class.
        /// This class stores the characters used in the document and other
        /// specifics unique to the current working document.
        /// </summary>
        /// <param name="fontName">the font name</param>
        /// <param name="indirectReference">the indirect reference to the font</param>
        /// <param name="baseFont">the  BaseFont </param>
        internal FontDetails(PdfName fontName, PdfIndirectReference indirectReference, BaseFont baseFont)
        {
            FontName = fontName;
            IndirectReference = indirectReference;
            BaseFont = baseFont;
            _fontType = baseFont.FontType;
            switch (_fontType)
            {
                case BaseFont.FONT_TYPE_T1:
                case BaseFont.FONT_TYPE_TT:
                    _shortTag = new byte[256];
                    break;

                case BaseFont.FONT_TYPE_CJK:
                    _cjkTag = new IntHashtable();
                    _cjkFont = (CjkFont)baseFont;
                    break;

                case BaseFont.FONT_TYPE_TTUNI:
                    _longTag = new Hashtable();
                    _ttu = (TrueTypeFontUnicode)baseFont;
                    _symbolic = baseFont.IsFontSpecific();
                    break;
            }
        }

        /// <summary>
        /// Indicates if all the glyphs and widths for that particular
        /// encoding should be included in the document. Set to  false
        /// to include all.
        /// </summary>
        public bool Subset
        {
            set => subset = value;
            get => subset;
        }

        /// <summary>
        /// Gets the  BaseFont  of this font.
        /// </summary>
        /// <returns>the  BaseFont  of this font</returns>
        internal BaseFont BaseFont { get; private set; }

        /// <summary>
        /// Gets the font name as it appears in the document body.
        /// </summary>
        /// <returns>the font name</returns>
        internal PdfName FontName { get; private set; }

        /// <summary>
        /// Gets the indirect reference to this font.
        /// </summary>
        /// <returns>the indirect reference to this font</returns>
        internal PdfIndirectReference IndirectReference { get; private set; }

        /// <summary>
        /// Converts the text into bytes to be placed in the document.
        /// The conversion is done according to the font and the encoding and the characters
        /// used are stored.
        /// </summary>
        /// <param name="text">the text to convert</param>
        /// <returns>the conversion</returns>
        internal byte[] ConvertToBytes(string text)
        {
            byte[] b = null;
            switch (_fontType)
            {
                case BaseFont.FONT_TYPE_T3:
                    return BaseFont.ConvertToBytes(text);

                case BaseFont.FONT_TYPE_T1:
                case BaseFont.FONT_TYPE_TT:
                    {
                        b = BaseFont.ConvertToBytes(text);
                        var len = b.Length;
                        for (var k = 0; k < len; ++k)
                        {
                            _shortTag[b[k] & 0xff] = 1;
                        }

                        break;
                    }
                case BaseFont.FONT_TYPE_CJK:
                    {
                        var len = text.Length;
                        for (var k = 0; k < len; ++k)
                        {
                            _cjkTag[_cjkFont.GetCidCode(text[k])] = 0;
                        }

                        b = BaseFont.ConvertToBytes(text);
                        break;
                    }
                case BaseFont.FONT_TYPE_DOCUMENT:
                    {
                        b = BaseFont.ConvertToBytes(text);
                        break;
                    }
                case BaseFont.FONT_TYPE_TTUNI:
                    {
                        var len = text.Length;
                        int[] metrics = null;
                        var glyph = new char[len];
                        var i = 0;
                        if (_symbolic)
                        {
                            b = PdfEncodings.ConvertToBytes(text, "symboltt");
                            len = b.Length;
                            for (var k = 0; k < len; ++k)
                            {
                                metrics = _ttu.GetMetricsTt(b[k] & 0xff);
                                if (metrics == null)
                                {
                                    continue;
                                }

                                _longTag[metrics[0]] = new[] { metrics[0], metrics[1], _ttu.GetUnicodeDifferences(b[k] & 0xff) };
                                glyph[i++] = (char)metrics[0];
                            }
                        }
                        else
                        {
                            for (var k = 0; k < len; ++k)
                            {
                                int val;
                                if (Utilities.IsSurrogatePair(text, k))
                                {
                                    val = Utilities.ConvertToUtf32(text, k);
                                    k++;
                                }
                                else
                                {
                                    val = text[k];
                                }
                                metrics = _ttu.GetMetricsTt(val);
                                if (metrics == null)
                                {
                                    continue;
                                }

                                var m0 = metrics[0];
                                var gl = m0;
                                if (!_longTag.ContainsKey(gl))
                                {
                                    _longTag[gl] = new[] { m0, metrics[1], val };
                                }

                                glyph[i++] = (char)m0;
                            }
                        }
                        var s = new string(glyph, 0, i);
                        b = PdfEncodings.ConvertToBytes(s, CjkFont.CJK_ENCODING);
                        break;
                    }
            }
            return b;
        }

        /// <summary>
        /// Writes the font definition to the document.
        /// </summary>
        /// <param name="writer">the  PdfWriter  of this document</param>
        internal void WriteFont(PdfWriter writer)
        {
            switch (_fontType)
            {
                case BaseFont.FONT_TYPE_T3:
                    BaseFont.WriteFont(writer, IndirectReference, null);
                    break;

                case BaseFont.FONT_TYPE_T1:
                case BaseFont.FONT_TYPE_TT:
                    {
                        int firstChar;
                        int lastChar;
                        for (firstChar = 0; firstChar < 256; ++firstChar)
                        {
                            if (_shortTag[firstChar] != 0)
                            {
                                break;
                            }
                        }
                        for (lastChar = 255; lastChar >= firstChar; --lastChar)
                        {
                            if (_shortTag[lastChar] != 0)
                            {
                                break;
                            }
                        }
                        if (firstChar > 255)
                        {
                            firstChar = 255;
                            lastChar = 255;
                        }
                        BaseFont.WriteFont(writer, IndirectReference, new object[] { firstChar, lastChar, _shortTag, subset });
                        break;
                    }
                case BaseFont.FONT_TYPE_CJK:
                    BaseFont.WriteFont(writer, IndirectReference, new object[] { _cjkTag });
                    break;

                case BaseFont.FONT_TYPE_TTUNI:
                    BaseFont.WriteFont(writer, IndirectReference, new object[] { _longTag, subset });
                    break;
            }
        }
    }
}