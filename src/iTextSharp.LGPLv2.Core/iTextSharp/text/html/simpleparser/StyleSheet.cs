using System.Collections;

namespace iTextSharp.text.html.simpleparser
{
    public class StyleSheet
    {
        public Hashtable ClassMap = new Hashtable();
        public Hashtable TagMap = new Hashtable();

        public void ApplyStyle(string tag, Hashtable props)
        {
            var map = (Hashtable)TagMap[tag.ToLowerInvariant()];
            Hashtable temp;
            if (map != null)
            {
                temp = new Hashtable(map);
                foreach (DictionaryEntry dc in props)
                {
                    temp[dc.Key] = dc.Value;
                }

                foreach (DictionaryEntry dc in temp)
                {
                    props[dc.Key] = dc.Value;
                }
            }
            var cm = (string)props[Markup.HTML_ATTR_CSS_CLASS];
            if (cm == null)
            {
                return;
            }

            map = (Hashtable)ClassMap[cm.ToLowerInvariant()];
            if (map == null)
            {
                return;
            }

            props.Remove(Markup.HTML_ATTR_CSS_CLASS);
            temp = new Hashtable(map);
            foreach (DictionaryEntry dc in props)
            {
                temp[dc.Key] = dc.Value;
            }

            foreach (DictionaryEntry dc in temp)
            {
                props[dc.Key] = dc.Value;
            }
        }

        public void LoadStyle(string style, Hashtable props)
        {
            ClassMap[style.ToLowerInvariant()] = props;
        }

        public void LoadStyle(string style, string key, string value)
        {
            style = style.ToLowerInvariant();
            var props = (Hashtable)ClassMap[style];
            if (props == null)
            {
                props = new Hashtable();
                ClassMap[style] = props;
            }
            props[key] = value;
        }

        public void LoadTagStyle(string tag, Hashtable props)
        {
            TagMap[tag.ToLowerInvariant()] = props;
        }

        public void LoadTagStyle(string tag, string key, string value)
        {
            tag = tag.ToLowerInvariant();
            var props = (Hashtable)TagMap[tag];
            if (props == null)
            {
                props = new Hashtable();
                TagMap[tag] = props;
            }
            props[key] = value;
        }
    }
}