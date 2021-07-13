using iTextSharp.text.pdf.draw;
using iTextSharp.text.xml.simpleparser;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.util;

namespace iTextSharp.text.html.simpleparser
{
    public class HtmlWorker : ISimpleXmlDocHandler, IDocListener
    {
        public const string tagsSupportedString = "ol ul li a pre font span br p div body table td th tr i b u sub sup em strong s strike"
            + " h1 h2 h3 h4 h5 h6 img hr";

        public static Hashtable TagsSupported = new Hashtable();
        protected IDocListener Document;
        protected ArrayList ObjectList;

        private readonly ChainedProperties _cprops = new ChainedProperties();
        private Paragraph _currentParagraph;
        private readonly FactoryProperties _factoryProperties = new FactoryProperties();
        private Hashtable _interfaceProps;
        private bool _isPre;
        private bool _pendingLi;
        private bool _pendingTd;
        private bool _pendingTr;
        private bool _skipText;
        private readonly Stack _stack = new Stack();
        private readonly Stack _tableState = new Stack();

        static HtmlWorker()
        {
            var tok = new StringTokenizer(tagsSupportedString);
            while (tok.HasMoreTokens())
            {
                TagsSupported[tok.NextToken()] = null;
            }
        }

        /// <summary>
        /// Creates a new instance of HTMLWorker
        /// </summary>
        public HtmlWorker(IDocListener document)
        {
            Document = document;
        }

        public HeaderFooter Footer
        {
            set
            {
            }
        }

        public HeaderFooter Header
        {
            set
            {
            }
        }

        public Hashtable InterfaceProps
        {
            set
            {
                _interfaceProps = value;
                FontFactoryImp ff = null;
                if (_interfaceProps != null)
                {
                    ff = _interfaceProps["font_factory"] as FontFactoryImp;
                }

                if (ff != null)
                {
                    _factoryProperties.FontImp = ff;
                }
            }
            get => _interfaceProps;
        }

        public int PageCount
        {
            set
            {
            }
        }

        public StyleSheet Style { set; get; } = new StyleSheet();

        public static ArrayList ParseToList(TextReader reader, StyleSheet style)
        {
            return ParseToList(reader, style, null);
        }

        public static ArrayList ParseToList(TextReader reader, StyleSheet style, Hashtable interfaceProps)
        {
            var worker = new HtmlWorker(null);
            if (style != null)
            {
                worker.Style = style;
            }

            worker.Document = worker;
            worker.InterfaceProps = interfaceProps;
            worker.ObjectList = new ArrayList();
            worker.Parse(reader);
            return worker.ObjectList;
        }

        public bool Add(IElement element)
        {
            ObjectList.Add(element);
            return true;
        }

        public void ClearTextWrap()
        {
        }

        public void Close()
        {
        }

        public virtual void EndDocument()
        {
            foreach (IElement e in _stack)
            {
                Document.Add(e);
            }

            if (_currentParagraph != null)
            {
                Document.Add(_currentParagraph);
            }

            _currentParagraph = null;
        }

        public virtual void EndElement(string tag)
        {
            if (!TagsSupported.ContainsKey(tag))
            {
                return;
            }

            var follow = (string)FactoryProperties.FollowTags[tag];
            if (follow != null)
            {
                _cprops.RemoveChain(follow);
                return;
            }
            if (tag.Equals("font") || tag.Equals("span"))
            {
                _cprops.RemoveChain(tag);
                return;
            }
            if (tag.Equals("a"))
            {
                if (_currentParagraph == null)
                {
                    _currentParagraph = new Paragraph();
                }

                IALink i = null;
                var skip = false;
                if (_interfaceProps != null)
                {
                    i = _interfaceProps["alink_interface"] as IALink;
                    if (i != null)
                    {
                        skip = i.Process(_currentParagraph, _cprops);
                    }
                }
                if (!skip)
                {
                    var href = _cprops["href"];
                    if (href != null)
                    {
                        var chunks = _currentParagraph.Chunks;
                        for (var k = 0; k < chunks.Count; ++k)
                        {
                            var ck = (Chunk)chunks[k];
                            ck.SetAnchor(href);
                        }
                    }
                }
                var tmp = (Paragraph)_stack.Pop();
                var tmp2 = new Phrase
                {
                    _currentParagraph
                };
                tmp.Add(tmp2);
                _currentParagraph = tmp;
                _cprops.RemoveChain("a");
                return;
            }
            if (tag.Equals("br"))
            {
                return;
            }
            if (_currentParagraph != null)
            {
                if (_stack.Count == 0)
                {
                    Document.Add(_currentParagraph);
                }
                else
                {
                    var obj = _stack.Pop();
                    if (obj is ITextElementArray)
                    {
                        var current = (ITextElementArray)obj;
                        current.Add(_currentParagraph);
                    }
                    _stack.Push(obj);
                }
            }
            _currentParagraph = null;
            if (tag.Equals(HtmlTags.UNORDEREDLIST) || tag.Equals(HtmlTags.ORDEREDLIST))
            {
                if (_pendingLi)
                {
                    EndElement(HtmlTags.LISTITEM);
                }

                _skipText = false;
                _cprops.RemoveChain(tag);
                if (_stack.Count == 0)
                {
                    return;
                }

                var obj = _stack.Pop();
                if (!(obj is List))
                {
                    _stack.Push(obj);
                    return;
                }
                if (_stack.Count == 0)
                {
                    Document.Add((IElement)obj);
                }
                else
                {
                    ((ITextElementArray)_stack.Peek()).Add(obj);
                }

                return;
            }
            if (tag.Equals(HtmlTags.LISTITEM))
            {
                _pendingLi = false;
                _skipText = true;
                _cprops.RemoveChain(tag);
                if (_stack.Count == 0)
                {
                    return;
                }

                var obj = _stack.Pop();
                if (!(obj is ListItem))
                {
                    _stack.Push(obj);
                    return;
                }
                if (_stack.Count == 0)
                {
                    Document.Add((IElement)obj);
                    return;
                }
                var list = _stack.Pop();
                if (!(list is List))
                {
                    _stack.Push(list);
                    return;
                }
                var item = (ListItem)obj;
                ((List)list).Add(item);
                var cks = item.Chunks;
                if (cks.Count > 0)
                {
                    item.ListSymbol.Font = ((Chunk)cks[0]).Font;
                }

                _stack.Push(list);
                return;
            }
            if (tag.Equals("div") || tag.Equals("body"))
            {
                _cprops.RemoveChain(tag);
                return;
            }
            if (tag.Equals(HtmlTags.PRE))
            {
                _cprops.RemoveChain(tag);
                _isPre = false;
                return;
            }
            if (tag.Equals("p"))
            {
                _cprops.RemoveChain(tag);
                return;
            }
            if (tag.Equals("h1") || tag.Equals("h2") || tag.Equals("h3") || tag.Equals("h4") || tag.Equals("h5") || tag.Equals("h6"))
            {
                _cprops.RemoveChain(tag);
                return;
            }
            if (tag.Equals("table"))
            {
                if (_pendingTr)
                {
                    EndElement("tr");
                }

                _cprops.RemoveChain("table");
                var table = (IncTable)_stack.Pop();
                var tb = table.BuildTable();
                tb.SplitRows = true;
                if (_stack.Count == 0)
                {
                    Document.Add(tb);
                }
                else
                {
                    ((ITextElementArray)_stack.Peek()).Add(tb);
                }

                var state = (bool[])_tableState.Pop();
                _pendingTr = state[0];
                _pendingTd = state[1];
                _skipText = false;
                return;
            }
            if (tag.Equals("tr"))
            {
                if (_pendingTd)
                {
                    EndElement("td");
                }

                _pendingTr = false;
                _cprops.RemoveChain("tr");
                var cells = new ArrayList();
                IncTable table = null;
                while (true)
                {
                    var obj = _stack.Pop();
                    if (obj is IncCell)
                    {
                        cells.Add(((IncCell)obj).Cell);
                    }
                    if (obj is IncTable)
                    {
                        table = (IncTable)obj;
                        break;
                    }
                }
                table.AddCols(cells);
                table.EndRow();
                _stack.Push(table);
                _skipText = true;
                return;
            }
            if (tag.Equals("td") || tag.Equals("th"))
            {
                _pendingTd = false;
                _cprops.RemoveChain("td");
                _skipText = true;
                return;
            }
        }

        public bool NewPage()
        {
            return true;
        }

        public void Open()
        {
        }

        public void Parse(TextReader reader)
        {
            SimpleXmlParser.Parse(this, null, reader, true);
        }

        public void ResetFooter()
        {
        }

        public void ResetHeader()
        {
        }

        public void ResetPageCount()
        {
        }

        public bool SetMarginMirroring(bool marginMirroring)
        {
            return false;
        }

        /// <summary>
        /// @see com.lowagie.text.DocListener#setMarginMirroring(boolean)
        /// @since	2.1.6
        /// </summary>
        public bool SetMarginMirroringTopBottom(bool marginMirroring)
        {
            return false;
        }

        public bool SetMargins(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            return true;
        }

        public bool SetPageSize(Rectangle pageSize)
        {
            return true;
        }

        public virtual void StartDocument()
        {
            var h = new Hashtable();
            Style.ApplyStyle("body", h);
            _cprops.AddToChain("body", h);
        }

        public virtual void StartElement(string tag, Hashtable h)
        {
            if (!TagsSupported.ContainsKey(tag))
            {
                return;
            }

            Style.ApplyStyle(tag, h);
            var follow = (string)FactoryProperties.FollowTags[tag];
            if (follow != null)
            {
                var prop = new Hashtable
                {
                    [follow] = null
                };
                _cprops.AddToChain(follow, prop);
                return;
            }
            FactoryProperties.InsertStyle(h, _cprops);
            if (tag.Equals(HtmlTags.ANCHOR))
            {
                _cprops.AddToChain(tag, h);
                if (_currentParagraph == null)
                {
                    _currentParagraph = new Paragraph();
                }

                _stack.Push(_currentParagraph);
                _currentParagraph = new Paragraph();
                return;
            }
            if (tag.Equals(HtmlTags.NEWLINE))
            {
                if (_currentParagraph == null)
                {
                    _currentParagraph = new Paragraph();
                }

                _currentParagraph.Add(_factoryProperties.CreateChunk("\n", _cprops));
                return;
            }
            if (tag.Equals(HtmlTags.HORIZONTALRULE))
            {
                // Attempting to duplicate the behavior seen on Firefox with
                // http://www.w3schools.com/tags/tryit.asp?filename=tryhtml_hr_test
                // where an initial break is only inserted when the preceding element doesn't
                // end with a break, but a trailing break is always inserted.
                var addLeadingBreak = true;
                if (_currentParagraph == null)
                {
                    _currentParagraph = new Paragraph();
                    addLeadingBreak = false;
                }
                if (addLeadingBreak)
                { // Not a new paragraph
                    var numChunks = _currentParagraph.Chunks.Count;
                    if (numChunks == 0 ||
                        ((Chunk)_currentParagraph.Chunks[numChunks - 1]).Content.EndsWith("\n"))
                    {
                        addLeadingBreak = false;
                    }
                }
                var align = (string)h["align"];
                var hrAlign = Element.ALIGN_CENTER;
                if (align != null)
                {
                    if (Util.EqualsIgnoreCase(align, "left"))
                    {
                        hrAlign = Element.ALIGN_LEFT;
                    }

                    if (Util.EqualsIgnoreCase(align, "right"))
                    {
                        hrAlign = Element.ALIGN_RIGHT;
                    }
                }
                var width = (string)h["width"];
                float hrWidth = 1;
                if (width != null)
                {
                    var tmpWidth = Markup.ParseLength(width, Markup.DEFAULT_FONT_SIZE);
                    if (tmpWidth > 0)
                    {
                        hrWidth = tmpWidth;
                    }

                    if (!width.EndsWith("%"))
                    {
                        hrWidth = 100; // Treat a pixel width as 100% for now.
                    }
                }
                var size = (string)h["size"];
                float hrSize = 1;
                if (size != null)
                {
                    var tmpSize = Markup.ParseLength(size, Markup.DEFAULT_FONT_SIZE);
                    if (tmpSize > 0)
                    {
                        hrSize = tmpSize;
                    }
                }
                if (addLeadingBreak)
                {
                    _currentParagraph.Add(Chunk.Newline);
                }

                _currentParagraph.Add(new LineSeparator(hrSize, hrWidth, null, hrAlign, _currentParagraph.Leading / 2));
                _currentParagraph.Add(Chunk.Newline);
                return;
            }
            if (tag.Equals(HtmlTags.CHUNK) || tag.Equals(HtmlTags.SPAN))
            {
                _cprops.AddToChain(tag, h);
                return;
            }
            if (tag.Equals(HtmlTags.IMAGE))
            {
                var src = (string)h[ElementTags.SRC];
                if (src == null)
                {
                    return;
                }

                _cprops.AddToChain(tag, h);
                Image img = null;
                if (_interfaceProps != null)
                {
                    if (_interfaceProps["img_provider"] is IImageProvider ip)
                    {
                        img = ip.GetImage(src, h, _cprops, Document);
                    }

                    if (img == null)
                    {
                        if (_interfaceProps["img_static"] is Hashtable images)
                        {
                            var tim = (Image)images[src];
                            if (tim != null)
                            {
                                img = Image.GetInstance(tim);
                            }
                        }
                        else
                        {
                            if (!src.StartsWith("http"))
                            { // relative src references only
                                if (_interfaceProps["img_baseurl"] is string baseurl)
                                {
                                    src = baseurl + src;
                                    img = Image.GetInstance(src);
                                }
                            }
                        }
                    }
                }
                if (img == null)
                {
                    if (!src.StartsWith("http"))
                    {
                        var path = _cprops["image_path"];
                        if (path == null)
                        {
                            path = "";
                        }

                        src = Path.Combine(path, src);
                    }
                    img = Image.GetInstance(src);
                }
                var align = (string)h["align"];
                var width = (string)h["width"];
                var height = (string)h["height"];
                var before = _cprops["before"];
                var after = _cprops["after"];
                if (before != null)
                {
                    img.SpacingBefore = float.Parse(before, System.Globalization.NumberFormatInfo.InvariantInfo);
                }

                if (after != null)
                {
                    img.SpacingAfter = float.Parse(after, System.Globalization.NumberFormatInfo.InvariantInfo);
                }

                var actualFontSize = Markup.ParseLength(_cprops[ElementTags.SIZE], Markup.DEFAULT_FONT_SIZE);
                if (actualFontSize <= 0f)
                {
                    actualFontSize = Markup.DEFAULT_FONT_SIZE;
                }

                var widthInPoints = Markup.ParseLength(width, actualFontSize);
                var heightInPoints = Markup.ParseLength(height, actualFontSize);
                if (widthInPoints > 0 && heightInPoints > 0)
                {
                    img.ScaleAbsolute(widthInPoints, heightInPoints);
                }
                else if (widthInPoints > 0)
                {
                    heightInPoints = img.Height * widthInPoints / img.Width;
                    img.ScaleAbsolute(widthInPoints, heightInPoints);
                }
                else if (heightInPoints > 0)
                {
                    widthInPoints = img.Width * heightInPoints / img.Height;
                    img.ScaleAbsolute(widthInPoints, heightInPoints);
                }
                img.WidthPercentage = 0;
                if (align != null)
                {
                    EndElement("p");
                    var ralign = Image.MIDDLE_ALIGN;
                    if (Util.EqualsIgnoreCase(align, "left"))
                    {
                        ralign = Image.LEFT_ALIGN;
                    }
                    else if (Util.EqualsIgnoreCase(align, "right"))
                    {
                        ralign = Image.RIGHT_ALIGN;
                    }

                    img.Alignment = ralign;
                    IImg i = null;
                    var skip = false;
                    if (_interfaceProps != null)
                    {
                        i = _interfaceProps["img_interface"] as IImg;
                        if (i != null)
                        {
                            skip = i.Process(img, h, _cprops, Document);
                        }
                    }
                    if (!skip)
                    {
                        Document.Add(img);
                    }

                    _cprops.RemoveChain(tag);
                }
                else
                {
                    _cprops.RemoveChain(tag);
                    if (_currentParagraph == null)
                    {
                        _currentParagraph = FactoryProperties.CreateParagraph(_cprops);
                    }

                    _currentParagraph.Add(new Chunk(img, 0, 0));
                }
                return;
            }

            EndElement("p");
            if (tag.Equals("h1") || tag.Equals("h2") || tag.Equals("h3") || tag.Equals("h4") || tag.Equals("h5") || tag.Equals("h6"))
            {
                if (!h.ContainsKey(ElementTags.SIZE))
                {
                    var v = 7 - int.Parse(tag.Substring(1));
                    h[ElementTags.SIZE] = v.ToString();
                }
                _cprops.AddToChain(tag, h);
                return;
            }
            if (tag.Equals(HtmlTags.UNORDEREDLIST))
            {
                if (_pendingLi)
                {
                    EndElement(HtmlTags.LISTITEM);
                }

                _skipText = true;
                _cprops.AddToChain(tag, h);
                var list = new List(false);
                try
                {
                    list.IndentationLeft = float.Parse(_cprops["indent"], System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    list.Autoindent = true;
                }
                list.SetListSymbol("\u2022");
                _stack.Push(list);
                return;
            }
            if (tag.Equals(HtmlTags.ORDEREDLIST))
            {
                if (_pendingLi)
                {
                    EndElement(HtmlTags.LISTITEM);
                }

                _skipText = true;
                _cprops.AddToChain(tag, h);
                var list = new List(true);
                try
                {
                    list.IndentationLeft = float.Parse(_cprops["indent"], System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    list.Autoindent = true;
                }
                _stack.Push(list);
                return;
            }
            if (tag.Equals(HtmlTags.LISTITEM))
            {
                if (_pendingLi)
                {
                    EndElement(HtmlTags.LISTITEM);
                }

                _skipText = false;
                _pendingLi = true;
                _cprops.AddToChain(tag, h);
                _stack.Push(FactoryProperties.CreateListItem(_cprops));
                return;
            }
            if (tag.Equals(HtmlTags.DIV) || tag.Equals(HtmlTags.BODY) || tag.Equals("p"))
            {
                _cprops.AddToChain(tag, h);
                return;
            }
            if (tag.Equals(HtmlTags.PRE))
            {
                if (!h.ContainsKey(ElementTags.FACE))
                {
                    h[ElementTags.FACE] = "Courier";
                }
                _cprops.AddToChain(tag, h);
                _isPre = true;
                return;
            }
            if (tag.Equals("tr"))
            {
                if (_pendingTr)
                {
                    EndElement("tr");
                }

                _skipText = true;
                _pendingTr = true;
                _cprops.AddToChain("tr", h);
                return;
            }
            if (tag.Equals("td") || tag.Equals("th"))
            {
                if (_pendingTd)
                {
                    EndElement(tag);
                }

                _skipText = false;
                _pendingTd = true;
                _cprops.AddToChain("td", h);
                _stack.Push(new IncCell(tag, _cprops));
                return;
            }
            if (tag.Equals("table"))
            {
                _cprops.AddToChain("table", h);
                var table = new IncTable(h);
                _stack.Push(table);
                _tableState.Push(new[] { _pendingTr, _pendingTd });
                _pendingTr = _pendingTd = false;
                _skipText = true;
                return;
            }
        }

        public virtual void Text(string str)
        {
            if (_skipText)
            {
                return;
            }

            var content = str;
            if (_isPre)
            {
                if (_currentParagraph == null)
                {
                    _currentParagraph = FactoryProperties.CreateParagraph(_cprops);
                }

                _currentParagraph.Add(_factoryProperties.CreateChunk(content, _cprops));
                return;
            }
            if (content.Trim().Length == 0 && content.IndexOf(" ", StringComparison.Ordinal) < 0)
            {
                return;
            }

            var buf = new StringBuilder();
            var len = content.Length;
            char character;
            var newline = false;
            for (var i = 0; i < len; i++)
            {
                switch (character = content[i])
                {
                    case ' ':
                        if (!newline)
                        {
                            buf.Append(character);
                        }
                        break;

                    case '\n':
                        if (i > 0)
                        {
                            newline = true;
                            buf.Append(' ');
                        }
                        break;

                    case '\r':
                        break;

                    case '\t':
                        break;

                    default:
                        newline = false;
                        buf.Append(character);
                        break;
                }
            }
            if (_currentParagraph == null)
            {
                _currentParagraph = FactoryProperties.CreateParagraph(_cprops);
            }

            _currentParagraph.Add(_factoryProperties.CreateChunk(buf.ToString(), _cprops));
        }
    }
}