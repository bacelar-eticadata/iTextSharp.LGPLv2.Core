using System.Collections;
using System.IO;

namespace iTextSharp.text.pdf
{
    /// <summary>
    /// Parses the page or template content.
    /// @author Paulo Soares (psoares@consiste.pt)
    /// </summary>
    public class PdfContentParser
    {
        /// <summary>
        /// Commands have this type.
        /// </summary>
        public const int COMMAND_TYPE = 200;

        /// <summary>
        /// Creates a new instance of PdfContentParser
        /// </summary>
        /// <param name="tokeniser">the tokeniser with the content</param>
        public PdfContentParser(PrTokeniser tokeniser)
        {
            Tokeniser = tokeniser;
        }

        /// <summary>
        /// Sets the tokeniser.
        /// </summary>
        public PrTokeniser Tokeniser { set; get; }

        /// <summary>
        /// Gets the tokeniser.
        /// </summary>
        /// <returns>the tokeniser.</returns>
        public PrTokeniser GetTokeniser()
        {
            return Tokeniser;
        }

        /// <summary>
        /// Reads the next token skipping over the comments.
        /// @throws IOException on error
        /// </summary>
        /// <returns> true  if a token was read,  false  if the end of content was reached</returns>
        public bool NextValidToken()
        {
            while (Tokeniser.NextToken())
            {
                if (Tokeniser.TokenType == PrTokeniser.TK_COMMENT)
                {
                    continue;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Parses a single command from the content. Each command is output as an array of arguments
        /// having the command itself as the last element. The returned array will be empty if the
        /// end of content was reached.
        ///  null  will create a new  ArrayList
        /// @throws IOException on error
        /// </summary>
        /// <param name="ls">an  ArrayList  to use. It will be cleared before using. If it's</param>
        /// <returns>the same  ArrayList  given as argument or a new one</returns>
        public ArrayList Parse(ArrayList ls)
        {
            if (ls == null)
            {
                ls = new ArrayList();
            }
            else
            {
                ls.Clear();
            }

            PdfObject ob = null;
            while ((ob = ReadPrObject()) != null)
            {
                ls.Add(ob);
                if (ob.Type == COMMAND_TYPE)
                {
                    break;
                }
            }
            return ls;
        }

        /// <summary>
        /// Reads an array. The tokeniser must be positioned past the "[" token.
        /// @throws IOException on error
        /// </summary>
        /// <returns>an array</returns>
        public PdfArray ReadArray()
        {
            var array = new PdfArray();
            while (true)
            {
                var obj = ReadPrObject();
                var type = obj.Type;
                if (-type == PrTokeniser.TK_END_ARRAY)
                {
                    break;
                }

                if (-type == PrTokeniser.TK_END_DIC)
                {
                    throw new IOException("Unexpected '>>'");
                }

                array.Add(obj);
            }
            return array;
        }

        /// <summary>
        /// Reads a dictionary. The tokeniser must be positioned past the "&lt;&lt;" token.
        /// @throws IOException on error
        /// </summary>
        /// <returns>the dictionary</returns>
        public PdfDictionary ReadDictionary()
        {
            var dic = new PdfDictionary();
            while (true)
            {
                if (!NextValidToken())
                {
                    throw new IOException("Unexpected end of file.");
                }

                if (Tokeniser.TokenType == PrTokeniser.TK_END_DIC)
                {
                    break;
                }

                if (Tokeniser.TokenType != PrTokeniser.TK_NAME)
                {
                    throw new IOException("Dictionary key is not a name.");
                }

                var name = new PdfName(Tokeniser.StringValue, false);
                var obj = ReadPrObject();
                var type = obj.Type;
                if (-type == PrTokeniser.TK_END_DIC)
                {
                    throw new IOException("Unexpected '>>'");
                }

                if (-type == PrTokeniser.TK_END_ARRAY)
                {
                    throw new IOException("Unexpected ']'");
                }

                dic.Put(name, obj);
            }
            return dic;
        }

        /// <summary>
        /// Reads a pdf object.
        /// @throws IOException on error
        /// </summary>
        /// <returns>the pdf object</returns>
        public PdfObject ReadPrObject()
        {
            if (!NextValidToken())
            {
                return null;
            }

            var type = Tokeniser.TokenType;
            switch (type)
            {
                case PrTokeniser.TK_START_DIC:
                    {
                        var dic = ReadDictionary();
                        return dic;
                    }
                case PrTokeniser.TK_START_ARRAY:
                    return ReadArray();

                case PrTokeniser.TK_STRING:
                    var str = new PdfString(Tokeniser.StringValue, null).SetHexWriting(Tokeniser.IsHexString());
                    return str;

                case PrTokeniser.TK_NAME:
                    return new PdfName(Tokeniser.StringValue, false);

                case PrTokeniser.TK_NUMBER:
                    return new PdfNumber(Tokeniser.StringValue);

                case PrTokeniser.TK_OTHER:
                    return new PdfLiteral(COMMAND_TYPE, Tokeniser.StringValue);

                default:
                    return new PdfLiteral(-type, Tokeniser.StringValue);
            }
        }
    }
}