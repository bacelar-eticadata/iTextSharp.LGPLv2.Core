using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System;
using System.util;

namespace iTextSharp.text
{
    /// <summary>
    /// Contains all the specifications of a font: fontfamily, size, style and color.
    /// </summary>
    /// <example>
    ///
    /// Paragraph p = new Paragraph("This is a paragraph",
    ///                new Font(Font.HELVETICA, 18, Font.BOLDITALIC, new Color(0, 0, 255)) );
    ///
    /// </example>
    public class Font : IComparable
    {
        /// <summary> this is a possible style. </summary>
        public const int BOLD = 1;

        /// <summary> this is a possible style. </summary>
        public const int BOLDITALIC = BOLD | ITALIC;

        /// <summary> a possible value of a font family. </summary>
        public const int COURIER = 0;

        /// <summary> the value of the default size. </summary>
        public const int DEFAULTSIZE = 12;

        /// <summary> a possible value of a font family. </summary>
        public const int HELVETICA = 1;

        /// <summary> this is a possible style. </summary>
        public const int ITALIC = 2;

        /// <summary> this is a possible style. </summary>
        public const int NORMAL = 0;

        /// <summary> this is a possible style. </summary>
        public const int STRIKETHRU = 8;

        /// <summary> a possible value of a font family. </summary>
        public const int SYMBOL = 3;

        /// <summary> a possible value of a font family. </summary>
        public const int TIMES_ROMAN = 2;
        /// <summary> the value of an undefined attribute. </summary>
        public const int UNDEFINED = -1;

        /// <summary>
        /// static membervariables for the different styles
        /// </summary>
        /// <summary> this is a possible style. </summary>
        public const int UNDERLINE = 4;

        /// <summary> a possible value of a font family. </summary>
        public const int ZAPFDINGBATS = 4;

        /// <summary> the value of the color. </summary>
        private BaseColor _color;

        /// <summary> the value of the fontsize. </summary>
        private float _size = UNDEFINED;

        /// <summary>
        /// constructors
        /// </summary>

        /// <summary>
        /// Copy constructor of a Font
        /// </summary>
        /// <param name="other">the font that has to be copied</param>
        public Font(Font other)
        {
            _color = other._color;
            Family = other.Family;
            _size = other._size;
            Style = other.Style;
            BaseFont = other.BaseFont;
        }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="family">the family to which this font belongs</param>
        /// <param name="size">the size of this font</param>
        /// <param name="style">the style of this font</param>
        /// <param name="color">the Color of this font.</param>
        public Font(int family, float size, int style, BaseColor color)
        {
            Family = family;
            _size = size;
            Style = style;
            _color = color;
        }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="bf">the external font</param>
        /// <param name="size">the size of this font</param>
        /// <param name="style">the style of this font</param>
        /// <param name="color">the Color of this font.</param>
        public Font(BaseFont bf, float size, int style, BaseColor color)
        {
            BaseFont = bf;
            _size = size;
            Style = style;
            _color = color;
        }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="bf">the external font</param>
        /// <param name="size">the size of this font</param>
        /// <param name="style">the style of this font</param>
        public Font(BaseFont bf, float size, int style) : this(bf, size, style, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="bf">the external font</param>
        /// <param name="size">the size of this font</param>
        public Font(BaseFont bf, float size) : this(bf, size, UNDEFINED, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="bf">the external font</param>
        public Font(BaseFont bf) : this(bf, UNDEFINED, UNDEFINED, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="family">the family to which this font belongs</param>
        /// <param name="size">the size of this font</param>
        /// <param name="style">the style of this font</param>
        public Font(int family, float size, int style) : this(family, size, style, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="family">the family to which this font belongs</param>
        /// <param name="size">the size of this font</param>
        public Font(int family, float size) : this(family, size, UNDEFINED, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <param name="family">the family to which this font belongs</param>
        public Font(int family) : this(family, UNDEFINED, UNDEFINED, null) { }

        /// <summary>
        /// Constructs a Font.
        /// </summary>
        /// <overloads>
        /// Has nine overloads.
        /// </overloads>
        public Font() : this(UNDEFINED, UNDEFINED, UNDEFINED, null) { }

        /// <summary>
        /// implementation of the Comparable interface
        /// </summary>

        /// <summary>
        /// Gets the BaseFont inside this object.
        /// </summary>
        /// <value>the BaseFont</value>
        public BaseFont BaseFont { get; private set; }

        /// <summary>
        /// Gets the size that can be used with the calculated  BaseFont .
        /// </summary>
        /// <returns>the size that can be used with the calculated  BaseFont </returns>
        public float CalculatedSize
        {
            get
            {
                var s = _size;
                if (s.ApproxEquals(UNDEFINED))
                {
                    s = DEFAULTSIZE;
                }
                return s;
            }
        }

        /// <summary>
        /// Gets the style that can be used with the calculated  BaseFont .
        /// </summary>
        /// <returns>the style that can be used with the calculated  BaseFont </returns>
        public int CalculatedStyle
        {
            get
            {
                var style = Style;
                if (style == UNDEFINED)
                {
                    style = NORMAL;
                }
                if (BaseFont != null)
                {
                    return style;
                }

                if (Family == SYMBOL || Family == ZAPFDINGBATS)
                {
                    return style;
                }
                else
                {
                    return style & (~BOLDITALIC);
                }
            }
        }

        /// <summary>
        /// Get/set the color of this font.
        /// </summary>
        /// <value>the color of this font</value>
        public virtual BaseColor Color
        {
            get => _color;
            set => _color = value;
        }

        /// <summary>
        /// Gets the family of this font.
        /// </summary>
        /// <value>the value of the family</value>
        public int Family { get; private set; } = UNDEFINED;

        /// <summary>
        /// FAMILY
        /// </summary>
        /// <summary>
        /// Gets the familyname as a string.
        /// </summary>
        /// <value>the familyname</value>
        public virtual string Familyname
        {
            get
            {
                var tmp = "unknown";
                switch (Family)
                {
                    case COURIER:
                        return FontFactory.COURIER;
                    case HELVETICA:
                        return FontFactory.HELVETICA;
                    case TIMES_ROMAN:
                        return FontFactory.TIMES_ROMAN;
                    case SYMBOL:
                        return FontFactory.SYMBOL;
                    case ZAPFDINGBATS:
                        return FontFactory.ZAPFDINGBATS;
                    default:
                        if (BaseFont != null)
                        {
                            var names = BaseFont.FamilyFontName;
                            for (var i = 0; i < names.Length; i++)
                            {
                                if ("0".Equals(names[i][2]))
                                {
                                    return names[i][3];
                                }
                                if ("1033".Equals(names[i][2]))
                                {
                                    tmp = names[i][3];
                                }
                                if ("".Equals(names[i][2]))
                                {
                                    tmp = names[i][3];
                                }
                            }
                        }
                        break;
                }
                return tmp;
            }
        }

        /// <summary>
        /// Get/set the size of this font.
        /// </summary>
        /// <value>the size of this font</value>
        public virtual float Size
        {
            get => _size;
            set => _size = value;
        }

        /// <summary>
        /// Gets the style of this font.
        /// </summary>
        /// <value>the style of this font</value>
        public int Style { get; private set; } = UNDEFINED;

        /// <summary>
        /// Translates a string-value of a certain family
        /// into the index that is used for this family in this class.
        /// </summary>
        /// <param name="family">A string representing a certain font-family</param>
        /// <returns>the corresponding index</returns>
        public static int GetFamilyIndex(string family)
        {
            if (Util.EqualsIgnoreCase(family, FontFactory.COURIER))
            {
                return COURIER;
            }
            if (Util.EqualsIgnoreCase(family, FontFactory.HELVETICA))
            {
                return HELVETICA;
            }
            if (Util.EqualsIgnoreCase(family, FontFactory.TIMES_ROMAN))
            {
                return TIMES_ROMAN;
            }
            if (Util.EqualsIgnoreCase(family, FontFactory.SYMBOL))
            {
                return SYMBOL;
            }
            if (Util.EqualsIgnoreCase(family, FontFactory.ZAPFDINGBATS))
            {
                return ZAPFDINGBATS;
            }
            return UNDEFINED;
        }

        /// <summary>
        /// Translates a string-value of a certain style
        /// into the index value is used for this style in this class.
        /// </summary>
        /// <param name="style">a string</param>
        /// <returns>the corresponding value</returns>
        public static int GetStyleValue(string style)
        {
            var s = 0;
            if (style.IndexOf(Markup.CSS_VALUE_NORMAL, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= NORMAL;
            }
            if (style.IndexOf(Markup.CSS_VALUE_BOLD, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= BOLD;
            }
            if (style.IndexOf(Markup.CSS_VALUE_ITALIC, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= ITALIC;
            }
            if (style.IndexOf(Markup.CSS_VALUE_OBLIQUE, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= ITALIC;
            }
            if (style.IndexOf(Markup.CSS_VALUE_UNDERLINE, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= UNDERLINE;
            }
            if (style.IndexOf(Markup.CSS_VALUE_LINETHROUGH, StringComparison.OrdinalIgnoreCase) != -1)
            {
                s |= STRIKETHRU;
            }
            return s;
        }

        /// <summary>
        /// Compares this Font with another
        /// </summary>
        /// <param name="obj">the other Font</param>
        /// <returns>a value</returns>
        public virtual int CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }
            Font font;
            try
            {
                font = (Font)obj;
                if (BaseFont != null && !BaseFont.Equals(font.BaseFont))
                {
                    return -2;
                }
                if (Family != font.Family)
                {
                    return 1;
                }
                if (_size.ApproxNotEqual(font.Size))
                {
                    return 2;
                }
                if (Style != font.Style)
                {
                    return 3;
                }
                if (_color == null)
                {
                    if (font.Color == null)
                    {
                        return 0;
                    }
                    return 4;
                }
                if (font.Color == null)
                {
                    return 4;
                }
                if (_color.Equals(font.Color))
                {
                    return 0;
                }
                return 4;
            }
            catch
            {
                return -3;
            }
        }
        /// <summary>
        /// Replaces the attributes that are equal to null with
        /// the attributes of a given font.
        /// </summary>
        /// <param name="font">the font of a bigger element class</param>
        /// <returns>a Font</returns>
        public virtual Font Difference(Font font)
        {
            if (font == null)
            {
                return this;
            }
            // size
            var dSize = font._size;
            if (dSize.ApproxEquals(UNDEFINED))
            {
                dSize = _size;
            }
            // style
            var dStyle = UNDEFINED;
            var style1 = Style;
            var style2 = font.Style;
            if (style1 != UNDEFINED || style2 != UNDEFINED)
            {
                if (style1 == UNDEFINED)
                {
                    style1 = 0;
                }

                if (style2 == UNDEFINED)
                {
                    style2 = 0;
                }

                dStyle = style1 | style2;
            }
            // color
            object dColor = font.Color;
            if (dColor == null)
            {
                dColor = Color;
            }
            // family
            if (font.BaseFont != null)
            {
                return new Font(font.BaseFont, dSize, dStyle, (BaseColor)dColor);
            }
            if (font.Family != UNDEFINED)
            {
                return new Font(font.Family, dSize, dStyle, (BaseColor)dColor);
            }
            if (BaseFont != null)
            {
                if (dStyle == style1)
                {
                    return new Font(BaseFont, dSize, dStyle, (BaseColor)dColor);
                }
                else
                {
                    return FontFactory.GetFont(Familyname, dSize, dStyle, (BaseColor)dColor);
                }
            }
            return new Font(Family, dSize, dStyle, (BaseColor)dColor);
        }

        /// <summary>
        /// BASEFONT
        /// </summary>
        /// <summary>
        /// Gets the  BaseFont  this class represents.
        /// For the built-in fonts a  BaseFont  is calculated.
        ///  false  to always use  Cp1252
        /// </summary>
        /// <param name="specialEncoding"> true  to use the special encoding for Symbol and ZapfDingbats,</param>
        /// <returns>the  BaseFont  this class represents</returns>
        public BaseFont GetCalculatedBaseFont(bool specialEncoding)
        {
            if (BaseFont != null)
            {
                return BaseFont;
            }

            var style = Style;
            if (style == UNDEFINED)
            {
                style = NORMAL;
            }
            var fontName = BaseFont.HELVETICA;
            var encoding = BaseFont.WINANSI;
            BaseFont cfont = null;
            switch (Family)
            {
                case COURIER:
                    switch (style & BOLDITALIC)
                    {
                        case BOLD:
                            fontName = BaseFont.COURIER_BOLD;
                            break;
                        case ITALIC:
                            fontName = BaseFont.COURIER_OBLIQUE;
                            break;
                        case BOLDITALIC:
                            fontName = BaseFont.COURIER_BOLDOBLIQUE;
                            break;
                        default:
                            //case NORMAL:
                            fontName = BaseFont.COURIER;
                            break;
                    }
                    break;
                case TIMES_ROMAN:
                    switch (style & BOLDITALIC)
                    {
                        case BOLD:
                            fontName = BaseFont.TIMES_BOLD;
                            break;
                        case ITALIC:
                            fontName = BaseFont.TIMES_ITALIC;
                            break;
                        case BOLDITALIC:
                            fontName = BaseFont.TIMES_BOLDITALIC;
                            break;
                        default:
                            //case NORMAL:
                            fontName = BaseFont.TIMES_ROMAN;
                            break;
                    }
                    break;
                case SYMBOL:
                    fontName = BaseFont.SYMBOL;
                    if (specialEncoding)
                    {
                        encoding = BaseFont.SYMBOL;
                    }

                    break;
                case ZAPFDINGBATS:
                    fontName = BaseFont.ZAPFDINGBATS;
                    if (specialEncoding)
                    {
                        encoding = BaseFont.ZAPFDINGBATS;
                    }

                    break;
                default:
                    //case Font.HELVETICA:
                    switch (style & BOLDITALIC)
                    {
                        case BOLD:
                            fontName = BaseFont.HELVETICA_BOLD;
                            break;
                        case ITALIC:
                            fontName = BaseFont.HELVETICA_OBLIQUE;
                            break;
                        case BOLDITALIC:
                            fontName = BaseFont.HELVETICA_BOLDOBLIQUE;
                            break;
                        default:
                            //case NORMAL:
                            fontName = BaseFont.HELVETICA;
                            break;
                    }
                    break;
            }
            cfont = BaseFont.CreateFont(fontName, encoding, false);
            return cfont;
        }

        /// <summary>
        /// SIZE
        /// </summary>
        /// <summary>
        /// Gets the leading that can be used with this font.
        /// a certain linespacing
        /// </summary>
        /// <param name="linespacing"></param>
        /// <returns>the height of a line</returns>
        public float GetCalculatedLeading(float linespacing)
        {
            return linespacing * CalculatedSize;
        }

        /// <summary>
        /// STYLE
        /// </summary>
        /// <summary>
        /// checks if this font is Bold.
        /// </summary>
        /// <returns>a boolean</returns>
        public bool IsBold()
        {
            if (Style == UNDEFINED)
            {
                return false;
            }
            return (Style & BOLD) == BOLD;
        }

        /// <summary>
        /// checks if this font is Bold.
        /// </summary>
        /// <returns>a boolean</returns>
        public bool IsItalic()
        {
            if (Style == UNDEFINED)
            {
                return false;
            }
            return (Style & ITALIC) == ITALIC;
        }

        /// <summary>
        /// Checks if the properties of this font are undefined or null.
        ///
        /// If so, the standard should be used.
        /// </summary>
        /// <returns>a boolean</returns>
        public virtual bool IsStandardFont()
        {
            return (Family == UNDEFINED
                && _size.ApproxEquals(UNDEFINED)
                && Style == UNDEFINED
                && _color == null
                && BaseFont == null);
        }

        /// <summary>
        /// checks if the style of this font is STRIKETHRU.
        /// </summary>
        /// <returns>a boolean</returns>
        public bool IsStrikethru()
        {
            if (Style == UNDEFINED)
            {
                return false;
            }
            return (Style & STRIKETHRU) == STRIKETHRU;
        }

        /// <summary>
        /// checks if this font is underlined.
        /// </summary>
        /// <returns>a boolean</returns>
        public bool IsUnderlined()
        {
            if (Style == UNDEFINED)
            {
                return false;
            }
            return (Style & UNDERLINE) == UNDERLINE;
        }

        /// <summary>
        /// COLOR
        /// </summary>
        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="red">the red-value of the new color</param>
        /// <param name="green">the green-value of the new color</param>
        /// <param name="blue">the blue-value of the new color</param>
        public virtual void SetColor(int red, int green, int blue)
        {
            _color = new BaseColor(red, green, blue);
        }

        /// <summary>
        /// Sets the family using a String ("Courier",
        /// "Helvetica", "Times New Roman", "Symbol" or "ZapfDingbats").
        /// </summary>
        /// <param name="family">A String representing a certain font-family.</param>
        public virtual void SetFamily(string family)
        {
            Family = GetFamilyIndex(family);
        }
        /// <summary>
        /// Sets the style using a String containing one of
        /// more of the following values: normal, bold, italic, underline, strike.
        /// </summary>
        /// <param name="style">A String representing a certain style.</param>
        public virtual void SetStyle(string style)
        {
            if (Style == UNDEFINED)
            {
                Style = NORMAL;
            }

            Style |= GetStyleValue(style);
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="style">the style.</param>
        public virtual void SetStyle(int style)
        {
            Style = style;
        }
    }
}
