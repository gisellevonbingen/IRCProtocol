using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class StringSpan
    {
        public string Text { get; }

        public int Position { get; private set; }

        public StringSpan(string text)
        {
            this.Text = text;
            this.Position = 0;
        }

        public int Length => this.Text.Length;

        public Dictionary<char, string> TakeToEndBySeparator(out string front, params char[] separator)
        {
            front = null;

            var builder = new StringBuilder();
            char? cs = null;
            var map = new Dictionary<char, string>();

            while (true)
            {
                if (this.Position == this.Length)
                {
                    break;
                }

                var next = this.NextChar();

                if (Array.IndexOf(separator, next) > -1)
                {
                    this.PutToMap(ref front, builder.ToString(), cs, map);
                    builder.Clear();
                    cs = next;
                }
                else
                {
                    builder.Append(next);
                }

            }

            this.PutToMap(ref front, builder.ToString(), cs, map);

            return map;
        }

        private void PutToMap(ref string front, string builder, char? cs, Dictionary<char, string> map)
        {
            if (cs.HasValue == true)
            {
                map[cs.Value] = builder;
            }
            else
            {
                front = builder;
            }

        }

        public string TakeToEnd()
        {
            var builder = new StringBuilder();

            while (true)
            {
                if (this.Position == this.Length)
                {
                    break;
                }

                var next = this.NextChar();
                builder.Append(next);
            }

            return builder.ToString();
        }

        public string TakeToSeparator(char separator, bool includeSeparator)
        {
            return this.TakeToSeparator(separator, includeSeparator, includeSeparator);
        }

        public string TakeToSeparator(char separator, bool includeSeparator, bool nextOnSeparator)
        {
            var builder = new StringBuilder();

            while (true)
            {
                if (this.Position == this.Length)
                {
                    break;
                }

                var peek = this.PeekChar();

                if (peek == separator)
                {
                    if (includeSeparator == true)
                    {
                        builder.Append(peek);
                    }

                    if (nextOnSeparator == true)
                    {
                        this.NextChar();
                    }

                    break;
                }
                else
                {
                    builder.Append(this.NextChar());
                }

            }

            return builder.ToString();
        }

        public char PeekChar()
        {
            return this.Text[this.Position];
        }

        public char NextChar()
        {
            var peek = this.PeekChar();
            this.Position++;

            return peek;
        }

        public char NextChar(char require)
        {
            var peek = this.PeekChar();

            if (peek == require)
            {
                this.Position++;
            }

            return peek;
        }

    }

}
