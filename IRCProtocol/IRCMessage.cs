using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCMessage
    {
        public const char TagsPrefix = '@';
        public const char TagsSuffix = ' ';

        public const char PrefixsPrefix = ':';
        public const char PrefixsSuffix = ' ';

        public IRCTags Tags { get; set; }
        public IRCPrefix Prefix { get; set; }
        public string Command { get; set; }
        public IRCParams Params { get; set; }

        public IRCMessage()
        {
            this.Tags = null;
            this.Prefix = null;
            this.Command = null;
            this.Params = null;
        }

        public IRCMessage Parse(string text)
        {
            var span = new StringSpan(text);
            var peek = span.PeekChar();

            if (peek == IRCMessage.TagsPrefix)
            {
                span.NextChar();
                var tagsText = span.TakeToSeparator(IRCMessage.TagsSuffix, false, true);
                this.Tags = new IRCTags().Parse(tagsText);
            }
            else
            {
                this.Tags = null;
            }

            if (peek == IRCMessage.PrefixsPrefix)
            {
                span.NextChar();
                var prefixText = span.TakeToSeparator(IRCMessage.PrefixsSuffix, false, true);
                this.Prefix = new IRCPrefix().Parse(prefixText);
            }
            else
            {
                this.Prefix = null;
            }

            var commandText = span.TakeToSeparator(IRCParams.ParamsPrefix, false);
            this.Command = commandText;

            var paramsText = span.TakeToEnd();
            this.Params = new IRCParams().Parse(paramsText);

            return this;
        }

        public override string ToString()
        {
            var prefix = this.Prefix;
            var prefixToString = prefix != null ? $"{PrefixsPrefix}{prefix}{PrefixsSuffix}" : null;

            var toString = $"{prefixToString}{this.Command}{this.Params}";

            return toString;
        }

    }

}
