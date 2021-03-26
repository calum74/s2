using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GeneratorLib;
using System.Linq;

namespace GeneratorTests
{
    public class ProgramParserTests
    {
        [Fact]
        void SimpleProgram()
        {
            var p = ProgramParser.Tokenize("123");
            Assert.Equal(1, p.Count());

            var steps = ProgramParser.Parse("123, 456, M1, B1, B3").ToArray();
            Assert.Equal(5, steps.Length);
            var options = new RunningOptions();

            var durations = steps.Select(s => ((IStep)s).Duration(options)).ToArray();
            var freqs = steps.Select(s => s.Frequency1.ToHertz(options)).ToArray();
        }

        [Fact]
        void Tokenizer()
        {
        }
    }
}
