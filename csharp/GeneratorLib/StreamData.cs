using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneratorLib
{
    public class StreamData
    {
        public Stream Stream { get; set; }
        public long Offset { get; set; }

        public StreamData(StreamData sd)
        {
            Stream = sd.Stream;
            Offset = sd.Offset;
        }

        public StreamData(Stream str, long off)
        {
            Stream = str;
            Offset = off;
        }

        public StreamData(Stream str) : this(str, str.Length)
        {
        }

        public StreamData() { }
        protected void Seek() { Stream.Seek(Offset, SeekOrigin.Begin); }

        public void Create(Stream stream)
        {
            Stream = stream;
            Offset = stream.Length;
            Seek();
        }
    }

    abstract public class StreamData<T> : StreamData
    {
        public StreamData(Stream str, long off) : base(str, off) { }

        public StreamData(Stream str) : base(str)
        {
        }

        public StreamData() { }

        protected abstract void SetValue(T t);
        protected abstract T GetValue();
        public T Value
        {
            get
            {
                Seek();
                return GetValue();
            }

            set
            {
                Seek();
                SetValue(value);
            }
        }

        public void Create(Stream s, T value)
        {
            Create(s);
            Value = value;
        }
    }

    public class StringData : StreamData<string>
    {
        protected override string GetValue() => Stream.ReadString();

        protected override void SetValue(string str) => Stream.WriteString(str);

        public static StringData Create(Stream s, string value)
        {
            var sd = new StringData();
            sd.Create(s);
            sd.Value = value;
            return sd;
        }

        public void ReadFrom(Stream str)
        {
            Stream = str;
            Offset = str.Position;
            GetValue();
        }
    }

    public class IntData : StreamData<int>
    {
        protected override int GetValue() => Stream.ReadInt32();

        protected override void SetValue(int i) => Stream.WriteInt32(i);
    }

    public class StreamPointer<T> : StreamData<T> where T : StreamData, new()
    {
        public StreamPointer(Stream s, long o) : base(s, o) { }

        protected override void SetValue(T value)
        {
            Stream.WriteInt32((int)value.Offset);
        }

        protected override T GetValue()
        {
            var p = Stream.ReadInt32();
            var t = new T();
            t.Stream = Stream;
            t.Offset = Offset;
            return t;
        }
    }

    public class StreamArray<T> : StreamData<T[]> where T : StreamData, new()
    {
        protected override void SetValue(T[] array)
        {
            Stream.WriteInt32(array.Length);
            foreach (var i in array)
                Stream.WriteInt32((int)i.Offset);
        }

        protected override T[] GetValue()
        {
            return Stream.ReadArray<T>();
        }

        public static StreamArray<T> Create(Stream s, int len)
        {
            var sd = new StreamArray<T>();
            sd.Create(s);
            s.WriteArray(len);
            return sd;
        }

    }


    public static class StreamUtils
    {
        public static string ReadString(this Stream str)
        {
            int len = ReadInt32(str);
            var bytes = new byte[len];
            str.Read(bytes, 0, len);
            return Encoding.UTF8.GetString(bytes);
        }

        public static void WriteString(this Stream str, string s)
        {
            str.WriteInt32(s.Length);
            var bytes = Encoding.UTF8.GetBytes(s);
            str.Write(bytes, 0, bytes.Length);
        }

        public static int ReadInt32(this Stream str)
        {
            var bytes = new byte[4];
            str.Read(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void WriteInt32(this Stream str, int i)
        {
            str.Write(BitConverter.GetBytes(i), 0, 4);
        }

        public static double ReadDouble(this Stream str)
        {
            var bytes = new byte[8];
            str.Read(bytes, 0, 8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public static void WriteDouble(this Stream str, double d)
        {
            str.Write(BitConverter.GetBytes(d), 0, 8);
        }

        public static T[] ReadArray<T>(this Stream str) where T : StreamData, new()
        {
            int len = str.ReadInt32();
            var result = new T[len];
            for (int i = 0; i < len; ++i)
            {
                var r = new T();
                r.Stream = str;
                r.Offset = str.ReadInt32();
                result[i] = r;
            }
            return result;
        }

        public static void WriteArray(this Stream str, int length)
        {
            str.WriteInt32(length);
            for (int i = 0; i < length; ++i)
                str.WriteInt32(0);
        }
    }
}
