using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCProtocol
{
    public class IRCPrefix
    {
        public const char UserSeparator = '!';
        public const char HostSeparator = '@';

        public string Nick { get; set; }
        public string User { get; set; }
        public string Host { get; set; }

        public IRCPrefix()
        {

        }

        public IRCPrefix Parse(string text)
        {
            var span = new StringSpan(text);
            var map = span.TakeToEndBySeparator(out string front, IRCPrefix.UserSeparator, IRCPrefix.HostSeparator);

            this.Nick = front;
            this.User = map.TryGetValue(IRCPrefix.UserSeparator, out string user) ? user : null;
            this.Host = map.TryGetValue(IRCPrefix.HostSeparator, out string host) ? host : null;

            return this;
        }

        public override string ToString()
        {
            var nick = this.Nick;
            var user = this.User;
            var host = this.Host;

            var toString = nick;

            if (user != null)
            {
                toString += UserSeparator + user;
            }

            if (host != null)
            {
                toString += HostSeparator + host;
            }

            return toString;
        }

    }

}
