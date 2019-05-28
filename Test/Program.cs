using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRCProtocol.Test
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            while (true)
            {
                var raw = Console.ReadLine();
                var message = new IRCMessage();
                message.Parse(raw);

                PrintReflection(message);
            }

        }

        public static void PrintReflection<T>(T value)
        {
            var lines = ToString(value);

            Console.WriteLine();
            Console.WriteLine($"========");

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var prefix = new string(' ', (line.Level + 1) * 4);
                Console.WriteLine($"{prefix}{line.Message}");
            }

        }

        public static List<PrintableLine> ToString(object obj, int level = 0)
        {
            var list = new List<PrintableLine>();

            if (obj == null)
            {
                list.Add(new PrintableLine(level, "{null}"));
            }
            else if (obj is IConvertible convertible)
            {
                list.Add(new PrintableLine(level, $"'{convertible}'"));
            }
            else if (obj is IEnumerable enumerable)
            {
                var array = enumerable.OfType<object>().ToArray();
                list.Add(new PrintableLine(level, $"Enumerable Count = {array.Length}"));

                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(new PrintableLine(level, $"{i}/{array.Length}"));
                    list.AddRange(ToString(array[i], level + 1));
                }

            }
            else
            {
                var type = obj.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                list.Add(new PrintableLine(level, $"Type.FullName = {type.FullName}"));
                list.Add(new PrintableLine(level, $"Properties.Length = {properties.Length}"));

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var propertyLines = ToString(property.GetValue(obj), level + 1);

                    if (propertyLines.Count == 1)
                    {
                        list.Add(new PrintableLine(level, $"{property.Name} = {propertyLines[0].Message}"));
                    }
                    else
                    {
                        list.Add(new PrintableLine(level, $"{property.Name}"));
                        list.AddRange(propertyLines);
                    }

                }

            }

            return list;
        }

    }

}
