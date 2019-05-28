using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCParams
    {
        public const char ParamsPrefix = ' ';
        public const char TrailingPrefix = ':';

        public List<string> Values { get; }

        public IRCParams()
        {
            this.Values = new List<string>();
        }

        public IRCParams Parse(string text)
        {
            var span = new StringSpan(text);

            var values = this.Values;
            values.Clear();

            if (span.Position < span.Length)
            {
                while (true)
                {
                    if (span.NextChar(IRCParams.ParamsPrefix) == IRCParams.ParamsPrefix)
                    {
                        var peek = span.PeekChar();

                        if (peek == IRCParams.TrailingPrefix)
                        {
                            values.Add(span.TakeToEnd());
                        }
                        else
                        {
                            var value = span.TakeToSeparator(IRCParams.ParamsPrefix, false);
                            values.Add(value);
                        }

                    }
                    else
                    {
                        break;
                    }

                    if (span.Position >= span.Length)
                    {
                        break;
                    }

                }

            }

            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var value in this.Values)
            {
                builder.Append(IRCParams.ParamsPrefix);
                builder.Append(value);
            }

            return builder.ToString();
        }

    }

}
