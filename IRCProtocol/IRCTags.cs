using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCTags
    {
        public const char TagSeparator = ';';
        public const char TagValueSeparator = '=';

        public Dictionary<IRCTagKey, string> Values { get; }

        public IRCTags()
        {
            this.Values = new Dictionary<IRCTagKey, string>();
        }

        public IRCTags Parse(string text)
        {
            var values = this.Values;
            values.Clear();

            var span = new StringSpan(text);

            while (span.Position < span.Length)
            {
                var tag = span.TakeToSeparator(TagSeparator, false, true);
                string keyString = null;
                string value = null;

                if (tag.Contains(TagValueSeparator) == true)
                {
                    var splits = tag.Split(TagValueSeparator);
                    keyString = splits[0];
                    value = splits[1];
                }
                else
                {
                    keyString = tag;
                }

                var key = new IRCTagKey().Parse(keyString);
                values[key] = value;
            }

            return this;
        }

    }

}
