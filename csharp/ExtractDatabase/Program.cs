using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GeneratorLib;

namespace ExtractDatabase
{
    static class StringUtils
    {
        static bool IsChar(byte b)
        {
            return b >= 32 && b < 128 || b == '\n' || b == '\r';
        }

        public static IEnumerable<string> VBStrings(this Stream stream, int min = 2)
        {
            byte[] buf = new byte[2];
            var sb = new StringBuilder();
            while (stream.Read(buf, 0, 2) == 2)
            {
                char ch;

                if (buf[0] == 0 && IsChar(buf[1]))
                    ch = (char)buf[1];
                else
                if (buf[1] == 0 && IsChar(buf[0]))
                    ch = (char)buf[0];
                else
                {
                    if (sb.Length >= min)
                        yield return sb.ToString();
                    sb.Clear();
                    continue;
                }
                sb.Append(ch);
            }
            if (sb.Length >= min)
                yield return sb.ToString();
        }

        public static IEnumerable<double> Frequencies(string s)
        {
            yield break;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length !=2)
            {
                Console.WriteLine("Usage: ExtractDatabase Spooky2.DMP output-file.db");
                return;
            }
            int count = 0;

            var programRegex = new Regex("^(.{40}) ([0-9]+) mins +([A-Z]+)\\s+([0-9.,]+)$");
            ReverseLookup database = new ReverseLookup();

            double minValue = 10001, maxValue = 1000000;

            using (var stream = File.OpenRead(args[0]))
            {
                foreach (var s in stream.VBStrings(55).
                    Where(s=>s[40]==' ')
                    )
                {
                    var m = programRegex.Match(s);

                    if (m.Success)
                    {
                        var name = m.Groups[1].Value.Trim();
                        var time = int.Parse(m.Groups[2].Value);
                        var db = m.Groups[3].Value;
                        var program = m.Groups[4].Value.Split(',');
                        ++count;

                        var frequencies = program.
                            Where(m => double.TryParse(m, out _)).
                            Select(m => double.Parse(m)).
                            Where(m => m >= minValue && m <= maxValue);

                        if(db != "CUST")
                            database.Add(name, frequencies);
                    }
                }
            }

            database.SaveBinary(args[1]);

            Console.WriteLine($"Found {count} entries");
        }
    }
}
