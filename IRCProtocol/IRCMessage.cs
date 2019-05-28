using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCMessage
    {
        public const char PrefixsPrefix = ':';
        public const char PrefixsSuffix = ' ';

        public IRCPrefix Prefix { get; set; }
        public string Command { get; set; }
        public IRCParams Params { get; set; }

        public IRCMessage()
        {
            this.Prefix = null;
            this.Command = null;
            this.Params = null;
        }

        public IRCMessage Parse(string text)
        {
            var span = new StringSpan(text);
            var next = span.NextChar(IRCMessage.PrefixsPrefix);

            if (next == IRCMessage.PrefixsPrefix)
            {
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
