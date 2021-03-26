using System;
using Xunit;
using GeneratorLib;
using System.Linq;

namespace GeneratorTests
{
    public class FeedbackTests
    {
        [Fact]
        public void Test1()
        {
        }

        [Fact]
        void ReverseLookupTest()
        {
            var rl = new ReverseLookup();
            rl.Add("a", new double[] { 3, 2, 1 });
            rl.Add("b", new double[] { 2, 3, 4 });

            rl.Sort();

            Assert.Equal(6, rl.Search(0, 5).Count());
            Assert.Equal(0, rl.Search(0, 0).Count());
            Assert.Equal(1, rl.Search(0, 1).Count());
            Assert.Equal(1, rl.Search(1, 1).Count());
            Assert.Equal(3, rl.Search(0.5, 2.5).Count());

            Assert.Equal(0, rl.Search(5, 10).Count());
            Assert.Equal(1, rl.Search(3.5, 10).Count());
            Assert.Equal(1, rl.Search(3.5, 4).Count());
        }
    }
}
