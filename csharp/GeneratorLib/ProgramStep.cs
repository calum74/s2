using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GeneratorLib
{
    /// <summary>
    /// A single instruction to a generator.
    /// </summary>
    public struct Command
    {
        public ICommand Cmd;        // The command to run.
        public double Frequency;    // The displayed frequency
        public TimeSpan Remaining;  // The time remaining in the current step.
        public Waveform Waveform;
        public double Amplitude;

        public Command(ICommand cmd, double freq, Waveform w, double amplitude, TimeSpan t)
        {
            Cmd = cmd;
            Frequency = freq;
            Remaining = t;
            Waveform = w;
            Amplitude = amplitude;
        }
    }

    /// <summary>
    /// Options for turning program steps into a series of commands.
    /// </summary>
    public class RunningOptions
    {
        public double defaultAmplitude = 10;
        public Waveform defaultWaveform = Waveform.Sine;
        public TimeSpan defaultDuration = TimeSpan.FromSeconds(180);

        // Other constants
        public double mw_to_hz_factor = 2.252342720E+23;
        public double bp_to_hz_factor = 8.63808777288135E+17;
        public double tissue_factor = 1.4158746743606;

        public bool isModulated => amplitideModulation || frequencyModulation;
        public bool amplitideModulation = false;
        public bool frequencyModulation = false;
        public double amAmplitude = 2;
        public double fmAmplitudeHz = 2;

        public double amFrequency = 0.1;
        public double fmFrequency = 0.1;

        public bool runInLoop = true;
    }

    /// <summary>
    /// A step in a program.
    /// </summary>
    public interface IStep
    {
        // Returns the next command, and the estimated time remaining in the current step.
        IEnumerable<Command> Commands(RunningOptions options);

        void Write(StringBuilder b);

        TimeSpan Duration(RunningOptions options);
    }

    public enum FrequencyUnit { Unspecified, Hertz, BP, MW, Wavelength }

    public struct Frequency
    {
        public FrequencyUnit Unit;

        public double Value;

        public bool Specified => Unit != FrequencyUnit.Unspecified && !double.IsNaN(Value);

        static double scale(double f)
        {
            while (f < 1000000) f *= 2.0;
            while (f > 2000000) f /= 2.0;
            return f;
        }

        public double ToHertz(RunningOptions options)
        {
            // This seems to be a fairly suspect formula for conerting a "target"
            // such as a molecule of a particular mass, into a frequency.
            // The ideas is that we can simply scale down the frequency
            // using powers of 2 so that we'll find another harmonic that can 
            // vibrate it.

            const double c = 299792.458;  // Speed of light

            switch (Unit)
            {
                case FrequencyUnit.Hertz: return Value;
                case FrequencyUnit.BP: return scale(Value * options.bp_to_hz_factor);
                case FrequencyUnit.MW: return scale(Value * options.mw_to_hz_factor);
                case FrequencyUnit.Wavelength: return scale(Value * options.tissue_factor * 1e9 / c);
                default: throw new InvalidDataException();
            }
        }

        public  IEnumerable<double> Test(RunningOptions options)
        {
            const double c = 299792.458;  // Speed of light

            for (int C = -1; C<= 1; ++C)
            {
                for (int T = -1; T <= 1; ++T)
                    for (int P = -1; P <= 1; ++P)
                        yield return scale(Math.Pow(options.tissue_factor, T) * Math.Pow(c, C) * Math.Pow(1e9, P));
            }
        }

        public void Write(StringBuilder sb)
        {
            switch (Unit)
            {
                case FrequencyUnit.BP: sb.Append("B"); break;
                case FrequencyUnit.MW: sb.Append("M"); break;
                case FrequencyUnit.Wavelength: sb.Append("L"); break;
            }
            sb.Append(Value);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Write(sb);
            return sb.ToString();
            }
        }

    public static class ProgramParser
    {
        public enum TokenType { Number, Comma, Equals, Dash, Waveform1, Waveform2, Gating, Amplitude1, Amplutude2, Light, Molecular, BasePairs, Offset, Phase, Factor, Constant, Squiggle };

        /// <summary>
        /// Tokenizes a Spooky2 command
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IEnumerable<(TokenType, double)> Tokenize(string str)
        {
            double number = 0;
            bool inNumber = false;
            bool afterDecimalPoint = false;
            double decimalMultiplier = 1;
            bool exponent = false;

            void ResetNumber()
            {
                number = 0;
                inNumber = false;
                afterDecimalPoint = false;
                decimalMultiplier = 1;
                exponent = false;
            }

            TokenType GetToken(char ch)
            {
                switch (ch)
                {
                    case '-': return TokenType.Dash;
                    case '=': return TokenType.Equals;
                    case 'w': return TokenType.Waveform2;
                    case 'W': return TokenType.Waveform1;
                    case 'G': return TokenType.Gating;
                    case 'A': return TokenType.Amplitude1;
                    case 'a': return TokenType.Amplutude2;
                    case 'L': return TokenType.Light;
                    case 'M': return TokenType.Molecular;
                    case 'B': return TokenType.BasePairs;
                    case 'P': return TokenType.Phase;
                    case 'f':
                    case 'F': return TokenType.Factor;
                    case 'c':
                    case 'C': return TokenType.Constant;
                    case ',': return TokenType.Comma;
                    case '~': return TokenType.Squiggle;
                    default: throw new ArgumentException($"Invalid token type '{ch}'");
                }
            }

            int i = 0;
            foreach (var ch in str)
            {
                ++i;
                switch (ch)
                {
                    case '-':
                    case '=':
                    case ',':
                    case 'W':
                    case 'G':
                    case 'A':
                    case 'a':
                    case 'L':
                    case 'M':
                    case 'B':
                    case 'P':
                    case 'F':
                    case 'f':
                    case 'c':
                    case 'w':
                    case 'C':
                    case '~':
                        if (inNumber)
                        {
                            yield return (TokenType.Number, number);
                            ResetNumber();
                        }
                        var tok = GetToken(ch);
                        yield return (tok, 0); ResetNumber();
                        break;
                    case '.':
                        if (afterDecimalPoint)
                            throw new FormatException($"Unexpected token '.' at position {i}");
                        else if (inNumber)
                            afterDecimalPoint = true;
                        else
                            inNumber = true;
                        break;
                    case ' ':
                        if (inNumber)
                        {
                            yield return (TokenType.Number, number);
                            ResetNumber();
                        }
                        break;
                    case 'e':
                    case 'E':
                        exponent = true;
                        break;
                    case '+':
                        if (!exponent) throw new FormatException($"Unexpected token '.' at position {i}");
                        break;
                    default:
                        if (char.IsDigit(ch))
                        {
                            int d = ch - '0';

                            // if(exponent)

                            if (afterDecimalPoint)
                                number = number + d * (decimalMultiplier /= 10.0);
                            else
                                number = number * 10 + d;
                            inNumber = true;
                        }
                        else
                            throw new FormatException($"Unexpected token '{ch}'at position {i}");
                        break;
                }
            }

            if (inNumber) yield return (TokenType.Number, number);
        }

        enum NumberContext
        {
            Frequency1,
            Frequency2,
            Duration,
            Waveform1,
            Waveform2,
            Constant,
            Factor,
            Squiggle
        }

        enum UnitModifier
        {
            Hertz,
            MW,
            BP,
            L
        }

        public static Waveform ParseWaveform(double d)
        {
            switch(d)
            {
                case 1.0: return Waveform.Sine;
                case 2.0: return Waveform.Square;
                case 3.0: return Waveform.AscendingSawtooth;
                case 4.0: return Waveform.DescendingSawtooth;
                case 5.0: return Waveform.Triangle;
                case 6.0: return Waveform.DampedSinusoidal;
                case 7.0: return Waveform.DampedSquare;
                case 8.0: return Waveform.HBombSinusoidal;
                case 9.0: return Waveform.HBombSquare;
                default: return Waveform.Other;
            }
        }

        public static IEnumerable<ProgramStep> Parse(string str)
        {
            // All durations default to 180, or carry on from the previous step.
            double previousDuration = 180;

            var ps = new ProgramStep();  // !! Set all to NaM
            // double f1 = double.NaN, f2 = double.NaN, duration = double.NaN, amplitude0 = double.NaN;

            var nc = NumberContext.Frequency1;
            var um = FrequencyUnit.Hertz;
            Waveform w = Waveform.Default;

            foreach (var token in Tokenize(str))
            {
                switch (token.Item1)
                {
                    case TokenType.Number:
                        switch (nc)
                        {
                            case NumberContext.Frequency1:
                                ps.Frequency1.Value = token.Item2;
                                ps.Frequency1.Unit = um;
                                break;
                            case NumberContext.Frequency2:
                                ps.Frequency2.Value = token.Item2;
                                ps.Frequency2.Unit = um;
                                break;
                            case NumberContext.Duration:
                                if (token.Item2 != previousDuration)
                                {
                                    ps.Duration = token.Item2;
                                    previousDuration = token.Item2;
                                }
                                break;
                            case NumberContext.Waveform1:
                                ps.Waveform1 = ParseWaveform(token.Item2);
                                break;
                            case NumberContext.Waveform2:
                                ps.Waveform2 = ParseWaveform(token.Item2);
                                break;
                        }
                        nc = NumberContext.Frequency1;
                        um = FrequencyUnit.Hertz;
                        break;
                    case TokenType.Equals:
                        nc = NumberContext.Duration;
                        break;
                    case TokenType.Dash:
                        nc = NumberContext.Frequency2;
                        break;
                    case TokenType.Comma:
                        yield return ps;
                        ps = new ProgramStep();
                        break;
                    case TokenType.Waveform1:
                        nc = NumberContext.Waveform1;
                        break;
                    case TokenType.Waveform2:
                        nc = NumberContext.Waveform2;
                        break;
                    case TokenType.Molecular:
                        um = FrequencyUnit.MW;
                        break;
                    case TokenType.Constant:
                        nc = NumberContext.Constant;
                        break;
                    case TokenType.Factor:
                        nc = NumberContext.Factor;
                        break;
                    case TokenType.Squiggle:
                        nc = NumberContext.Squiggle;
                        break;
                    case TokenType.BasePairs:
                        um = FrequencyUnit.BP;
                        break;
                    case TokenType.Light:
                        um = FrequencyUnit.Wavelength;
                        break;
                    default:
                        break;
                }
            }
            // The final step - could be empty maybe
            if (ps.Frequency1.Specified)
                yield return ps;
        }
    }

    [Serializable]
    public class ProgramStep : IStep
    {
        public Frequency Frequency1, Frequency2;
        public double Amplitude1 = double.NaN;
        public double Amplitude2 = double.NaN;
        public Waveform Waveform1 = Waveform.Default, Waveform2 = Waveform.Default;

        [XmlIgnore]
        readonly TimeSpan stepDuration = TimeSpan.FromSeconds(0.1);

        public void Write(StringBuilder builder)
        {
            Frequency1.Write(builder);
            if (IsSweep)
            {
                builder.Append("-");
                Frequency2.Write(builder);
            }
            if (!double.IsNaN(Duration))
            {
                builder.Append("=");
                builder.Append(Duration);
            }
        }

        public ProgramStep() { }

        public bool IsSweep => Frequency2.Specified;

        public override string ToString()
        {
            var sb = new StringBuilder();
            Write(sb);
            return sb.ToString();
        }

        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        public double Duration = double.NaN;

        TimeSpan IStep.Duration(RunningOptions options) => double.IsNaN(Duration) ? options.defaultDuration : TimeSpan.FromSeconds(Duration);

        public IEnumerable<Command> Commands(RunningOptions options)
        {
            double duration = double.IsNaN(Duration) ? options.defaultDuration.TotalSeconds : Duration;

            TimeSpan remaining = TimeSpan.FromSeconds(duration);

            Waveform w = Waveform1;
            if (w == Waveform.Default)
                w = options.defaultWaveform;
            yield return new Command(new SetWaveform(w), double.NaN, w, double.NaN, remaining);

            if (IsSweep || options.isModulated)
            {
                double f1 = Frequency1.ToHertz(options), f2 = IsSweep ? Frequency2.ToHertz(options) : f1;

                double numberOfSteps = duration / stepDuration.TotalSeconds;

                for (int i = 0; i < numberOfSteps; ++i)
                {
                    double f = FrequencyAt(i * stepDuration.TotalSeconds, options);
                    double a = AmplitudeAt(i * stepDuration.TotalSeconds, options);


                    if (options.amplitideModulation)
                        yield return new Command(new SetAmplitude(a), f, w, a, remaining);
                    yield return new Command(new SetFrequency(f, stepDuration), f, w, a, remaining -= stepDuration);
                }
            }
            else
            {
                double f = Frequency1.ToHertz(options);
                double a = AmplitudeAt(0, options);
                yield return new Command(new SetFrequency(f, TimeSpan.FromSeconds(1)), f, w, a, TimeSpan.FromSeconds(duration));
                for (double t = duration - 1; t > 0; t -= 1)
                    yield return new Command(new Noop(TimeSpan.FromSeconds(1)), f, w, a, TimeSpan.FromSeconds(t));
            }
        }

        public double triangleWave(double angle)
        {
            var x = angle / (2.0 * Math.PI);
            x = x - Math.Floor(x);
            return -1 + 2.0 * x;
        }

        public double FrequencyAt(double t, RunningOptions options)
        {
            double f1 = Frequency1.ToHertz(options), f2 = IsSweep ? Frequency2.ToHertz(options) : f1;
            double duration = double.IsNaN(Duration) ? options.defaultDuration.TotalSeconds : Duration;
            var f = f1 + (f2 - f2) * t / duration;

            if(options.frequencyModulation)
            {
                f += options.fmAmplitudeHz * triangleWave(2.0 * Math.PI * t * options.fmFrequency);
            }
            return f;
        }

        double AmplitudeAt(double t, RunningOptions options)
        {
            var v = this.Amplitude1;
            if (double.IsNaN(v)) v = options.defaultAmplitude;

            if(options.amplitideModulation)
            {
                v += options.amAmplitude * triangleWave(2.0 * Math.PI * t * options.amFrequency);
            }
            return v; 
        }
    }

    public class Noop : ICommand
    {
        public TimeSpan Duration { get; private set; }

        public void Run(IOpenChannel channel)
        {
        }

        public Noop(TimeSpan d)
        {
            Duration = d;
        }
    }

    public class SetFrequency : ICommand, IStep
    {
        double Freq;
        public SetFrequency(double freq, TimeSpan duration)
        {
            Freq = freq;
            Duration = duration;
        }

        public string Name => Freq.ToString();

        public string Description => $"Set frequency to {Freq}";
        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        public TimeSpan Duration { get; }

        TimeSpan IStep.Duration(RunningOptions options) => Duration;

        public IEnumerable<Command> Commands(RunningOptions options)
        {
            yield return new Command(this, Freq, Waveform.Default, double.NaN, Duration);
        }

        public void Write(StringBuilder b)
        {
            b.Append(Freq);
            if (Duration.TotalSeconds != 180)
            {
                b.Append("=");
                b.Append(Duration.TotalSeconds);
            }
        }

        public void Run(IOpenChannel channel)
        {
            channel.Frequency = Freq;
        }
    }

    public class SetAmplitude : ICommand
    {
        double Amp;
        public SetAmplitude(double a)
        {
            Amp = a;
        }

        public TimeSpan Duration => default(TimeSpan);

        public void Run(IOpenChannel channel)
        {
            channel.Amplitude = Amp;
        }
    }

    class SetWaveform : ICommand
    {
        Waveform W;
        public SetWaveform(Waveform w)
        {
            W = w;
        }

        public TimeSpan Duration => default(TimeSpan);

        public void Run(IOpenChannel channel)
        {
            channel.Waveform = W;
        }
    }


}
