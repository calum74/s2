using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GeneratorLib
{
    /// <summary>
    /// Searches for hits in a frequency range.
    /// </summary>
    public interface IReverseLookup
    {
        /// <summary>
        /// Finds all of the hits in a given range.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        IEnumerable<string> Search(double min, double max);
    }

    public static class LookupExtensions
    {
        public static IEnumerable<string> Search(this IReverseLookup lookup, double min, double max, int minOctave, int maxOctave)
        {
            return Enumerable.
                Range(minOctave, maxOctave - minOctave + 1).
                Select(octave => Math.Pow(2.0, octave)).
                SelectMany(m => lookup.Search(m * min, m * max)).
                OrderBy(s => s);
        }
    }

    [Serializable]
    public class ReverseLookup : IReverseLookup
    {
        List<string> Strings = new List<string>();
        List<KeyValuePair<double, int>> Index = new List<KeyValuePair<double, int>>();

        public void SaveBinary(string filename)
        {
            Sort();
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static IReverseLookup Load(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = File.OpenRead(filename))
            {
                return Load(stream);
            }
        }

        public static IReverseLookup Load(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            return (ReverseLookup)formatter.Deserialize(stream);
        }

        public void Add(string result, IEnumerable<double> frequencies)
        {
            if (!frequencies.Any()) return;

            int item = Strings.Count;
            Strings.Add(result);

            foreach(var f in frequencies)
                Index.Add(new KeyValuePair<double, int>(f, item));
        }

        public void Sort()
        {
            Index.Sort((a,b) => Math.Sign(a.Key-b.Key));
        }

        int LowerBound(double v)
        {
            // Invariant: i[a] <= v
            int a = 0, b = Index.Count;
            while (a < b-1)
            {
                int mid = (a+b)/ 2;
                if (Index[mid].Key < v)
                    a = mid;
                else
                    b = mid;
            }
            if (Index[a].Key < v) return b;
            return a;
        }

        int UpperBound(double v)
        {
            // Invariant: v < i[b]
            int a = 0, b = Index.Count;
            while (a < b-1)
            {
                int mid = (a + b) / 2;
                if (v < Index[mid].Key)
                    b = mid;
                else
                    a = mid;
            }

            if (v < Index[a].Key) return a;

            return b;
        }

        public IEnumerable<string> Search(double min, double max)
        {
            int a = LowerBound(min);
            int b = UpperBound(max);
            for (int i = a; i < b; ++i)
                yield return Strings[Index[i].Value];
        }
    }

    public class EmptyLookup : IReverseLookup
    {
        public IEnumerable<string> Search(double min, double max) => Enumerable.Empty<string>();
    }
}
