using System;
using Xunit;
using GeneratorLib;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GeneratorTests
{
    public class HeapTests
    {

        class IntComparer : IComparer<int>
        {
            public int Compare([AllowNull] int x, [AllowNull] int y)
            {
                return x - y;
            }
        }

        [Fact]
        void Heap()
        {
            var h = new Heap<int>(new IntComparer());
            for (int i = 0; i < 100; ++i)
            {
                h.Add(i);
                Assert.True(h.Valid);
            }

            Assert.Equal(100, h.Count);

            for (int i=0; i<100; ++i)
            {
                Assert.Equal(i, h.Pop());
                Assert.True(h.Valid);
            }
        }

        [Fact]
        void Heap2()
        {
            var h = new Heap<int>(new IntComparer());
            for (int i = 99; i >= 0; --i)
            {
                h.Add(i);
                Assert.True(h.Valid);
            }

            for (int i = 0; i < 10; ++i)
            {
                Assert.Equal(i, h.Pop());
                Assert.True(h.Valid);
            }
        }

    }
}
