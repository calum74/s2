using GeneratorLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spooky2
{
    class Spooky2PresetFolder : IProgramFolder
    {
        readonly string path;

        public Spooky2PresetFolder(string dir) { path = dir; }

        public string Name => Path.GetFileName(path);

        public IEnumerable<IProgramFolder> Folders =>
            Directory.EnumerateDirectories(path).Select(d => new Spooky2PresetFolder(Path.Combine(path, d)));

        public IEnumerable<IProgram> Programs =>
            Directory.EnumerateFiles(path, "*.txt").Select(f => new Spooky2Preset(Path.Combine(path, f)));
    }

    static class FileUtils
    {
        public static IEnumerable<string> ReadQuotedLines(string filename)
        {
            using (var stream = File.OpenText(filename))
            {
                for (string line = stream.ReadLine(); line != null; line = stream.ReadLine())
                {
                    if (line.StartsWith("\""))
                    {
                        while (!line.EndsWith("\""))
                        {
                            var end = stream.ReadLine();
                            if (end is null)
                                yield break;
                            line += "\n" + end; // !! Handle nulls
                        }
                        yield return line.Substring(1, line.Length - 2);
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> ParseKeyValues(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var i = line.IndexOf('=');
                if (i >= 0)
                    yield return new KeyValuePair<string, string>(line.Substring(0, i), line.Substring(i + 1, line.Length - i - 1));
                else
                    yield return new KeyValuePair<string, string>(line, "");
            }
        }

        public static IEnumerable<string> ParseCsvLine(string line)
        {
            int pos = 0, pos2;
            while (pos < line.Length)
            {
                if (line[pos] == '\"')
                {
                    for (pos2 = pos + 1; pos2 < line.Length && line[pos2] != '\"'; ++pos2)
                        ;
                    yield return line.Substring(pos + 1, pos2 - pos - 1);
                    pos = pos2 + 2;
                }
                else
                {
                    for (pos2 = pos; pos2 < line.Length && line[pos2] != ','; ++pos2)
                        ;
                    yield return line.Substring(pos, pos2 - pos);
                    pos = pos2 + 1;
                }
            }
        }

        public static IEnumerable<IEnumerable<string>> ReadCsv(string filename)
        {
            using (var stream = File.OpenText(filename))
            {
                for (string line = stream.ReadLine(); line != null; line = stream.ReadLine())
                {
                    yield return ParseCsvLine(line);
                }
            }
        }
    }

    class Spooky2DatabaseFile : IProgramFolder
    {
        public string Name => "Custom";

        public string Path { get; }
        public Spooky2DatabaseFile(string file)
        {
            Path = file;
        }

        public IEnumerable<IProgramFolder> Folders => Enumerable.Empty<IProgramFolder>();

        public IEnumerable<IProgram> Programs
        {
            get
            {
                foreach (var line in FileUtils.ReadCsv(Path))
                {
                    var i = line.GetEnumerator();
                    i.MoveNext();
                    var name = i.Current;
                    i.MoveNext();
                    var db = i.Current;
                    i.MoveNext();
                    i.MoveNext();
                    var description = i.Current;
                    i.MoveNext();
                    var commands = i.Current;
                    i.MoveNext();
                    i.MoveNext();
                    i.MoveNext();
                    var stepSize = i.Current;
                    yield return new Spooky2Program(name, commands, int.Parse(stepSize));
                }
            }
        }
    }
}
