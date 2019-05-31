using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCTagKey : IEquatable<IRCTagKey>
    {
        public const char ClientPrefix = '+';
        public const char VendorSuffix = '/';

        public bool Client { get; set; }
        public string Vendor { get; set; }
        public string Name { get; set; }

        public IRCTagKey()
        {
            this.Client = false;
            this.Vendor = null;
            this.Name = null;
        }

        public IRCTagKey Parse(string text)
        {
            var span = new StringSpan(text);
            var next = span.NextChar(ClientPrefix);
            var read = span.TakeToSeparator(VendorSuffix, false, true);

            this.Client = next == ClientPrefix;

            if (read.EndsWith(VendorSuffix.ToString()) == true)
            {
                this.Vendor = read;
                this.Name = span.TakeToEnd();
            }
            else
            {
                this.Vendor = null;
                this.Name = read;
            }

            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (this.Client == true)
            {
                builder.Append(ClientPrefix);
            }

            var vendor = this.Vendor;

            if (vendor != null)
            {
                builder.Append($"{vendor}{VendorSuffix}");
            }

            builder.Append(this.Name);

            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IRCTagKey);
        }

        public bool Equals(IRCTagKey other)
        {
            if (other == null || other.GetType().Equals(this.GetType()) == false)
            {
                return false;
            }

            if (this.Client != other.Client)
            {
                return false;
            }

            if (string.Equals(this.Vendor, other.Vendor) == false)
            {
                return false;
            }

            if (string.Equals(this.Name, other.Name) == false)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.Client.GetHashCode();
            hash = hash * 31 + this.Vendor?.GetHashCode() ?? 0;
            hash = hash * 31 + this.Name?.GetHashCode() ?? 0;

            return hash;
        }

    }

}
