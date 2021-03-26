using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeneratorLib
{
    public class BiofeedbackSettings
    {
        public double MinFrequency = 76000, MaxFrequency = 145000,
            StepSize = 20, RescanStepSize = 5;
        public bool Stagger = false;
        public Waveform Waveform = Waveform.Sine;

        /// <summary>
        /// Biofeedback threshold. Values less than this will
        /// be discarded and not rescanned.
        /// Low threshold = high sensitivity.
        /// High threshold = low sensitivity.
        /// </summary>
        public double Threshold = 5;
        public int Window = 4;
    }

    public interface IBiofeedbackSource
    {
        double Read();
    }

    public class HrmSource : IBiofeedbackSource
    {
        readonly IHeartRateMonitor hrm;

        double lastValue;

        public double LastBpm => lastValue;
        public double LastResponse { get; private set; }

        public HrmSource(IHeartRateMonitor hrm)
        {
            this.hrm = hrm;
        }

        public double Read()
        {
            var v = hrm.Read().AsBpm();
            var diff = Math.Abs(v - lastValue);
            LastResponse = diff;
            lastValue = v;
            return diff;
        }
    }

    public class Biofeedback : IStep, IProgram
    {
        public class Sample
        {
            public double frequency, response;
            public Sample(double f, double r)
            {
                frequency = f;
                response = r;
            }

            public override string ToString() => $"{frequency} Hz = {response} bpm";
        }

        int nextRead = 0;
        int lastAnalysed = 0;

        List<Sample> samples = new List<Sample>();

        public void Enqueue(double minFreq, double maxFreq, double step)
        {
            for (double f = minFreq; f <= maxFreq; f += step)
                samples.Add(new Sample(f, 0));
        }

        public bool ScanNext(IOpenChannel channel, IBiofeedbackSource source, out Sample sample)
        {
            if (nextRead < samples.Count)
            {
                channel.Frequency = samples[nextRead].frequency;
                samples[nextRead].response = source.Read();
                sample = samples[nextRead];
                nextRead++;
                return true;
            }
            else
            {
                sample = null;
                return false;
            }
        }

        public int TotalSteps => samples.Count;

        public int StepsRemaining => TotalSteps - nextRead;

        public int StepsScanned => nextRead;

        public delegate void Progress(double freq, double response, int stage, TimeSpan eta);
        public delegate void Finished(Sample[] results);

        public IBiofeedbackSource Source { get; set; }

        public event Progress OnProgress;
        public event Finished OnFinished;

        public int Stages { get; set; } = 5;
        public double Threshold { get; set; } = 5.0;
        public int Window { get; set; } = 4;

        public IEnumerable<Command> Commands(RunningOptions options)
        {
            int nextRead = 0;
            var zero = default(TimeSpan);
            TimeSpan eta = TimeSpan.FromSeconds(10);

            var sw = new Stopwatch();  // Count the total running time

            for (int s = 0; s < Stages; ++s)
            {
                for (; nextRead < samples.Count; ++nextRead)
                {
                    var freq = samples[nextRead].frequency;
                    yield return new Command(new SetFrequency(freq, zero), freq, options.defaultWaveform, options.defaultAmplitude, eta);

                    double response = 0;
                    while (response == 0)
                    {
                        Exception ex;
                        try
                        {
                            sw.Start();
                            response = Source.Read();
                            sw.Stop();
                            break;
                        }
                        catch(Exception e)
                        {
                            ex = e;
                            // !! Problem here: If the HRM throws, we similarly re-throw the exception to the 
                            // caller, thereby preseti
                        }
                        yield return new Command(new ThrowCommand(ex), freq, options.defaultWaveform, options.defaultAmplitude, eta);
                    }

                    samples[nextRead].response = response;

                    int remainingSteps = samples.Count - nextRead;
                    int doneSteps = nextRead;
                    if (doneSteps > 0)
                    {
                        var timePerStep = sw.Elapsed.TotalMilliseconds / doneSteps;
                        eta = TimeSpan.FromMilliseconds(timePerStep * remainingSteps);
                    }

                    OnProgress(freq, response, s + 1, eta);
                }

                bool lastStage = s == Stages - 1;
                if (!lastStage)
                    AnalyseAndRescan(Threshold, Window);
            }

            OnFinished(Hits(Threshold, 1).ToArray());
        }

        public string Name => "Biofeedback";

        public TimeSpan Duration(RunningOptions options) => TimeSpan.FromHours(1);

        public string Description => "Pulse biofeedback scan";

        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        public IEnumerable<IStep> Steps { get { yield return this; } }

        public void AnalyseAndRescan(double threshold, int window)
        {
            foreach (var hit in Hits(threshold, window))
            {
                samples.Add(new Sample(hit.frequency, 0));
            }
        }

        public void NextStage()
        {
            lastAnalysed = samples.Count;
        }

        public IEnumerable<Sample> Hits(double threshold, int window)
        {
            // Don't use enumerator -- could become invalid
            int x0 = lastAnalysed, x1 = samples.Count;
            lastAnalysed = x1;

            int lastOut = x0;
            for (int x = x0; x < x1; ++x)
            {
                if (samples[x].response >= threshold)
                {
                    lastOut = Math.Max(lastOut, x - window);
                    for (int y = lastOut + 1; y < x + window && y < x1; ++y)
                    {
                        yield return samples[y];
                        lastOut = y;
                    }
                }
            }
        }

        public void Write(StringBuilder b)
        {
            throw new NotImplementedException();
        }
    }

    internal class ThrowCommand : ICommand
    {
        readonly Exception innerException;

        public ThrowCommand(Exception ex)
        {
            innerException = ex;
        }
        public TimeSpan Duration => default(TimeSpan);

        public void Run(IOpenChannel channel)
        {
            throw innerException;
        }
    }


    public class SimulatedFeedback : IBiofeedbackSource, IChannel, IOpenChannel, IHeartRateMonitor
    {
        public IOpenChannel TryOpen() => this;
        public void Close() { }
        class Hit
        {
            public double frequency, width, size;
            public Hit(double f, double w, double s)
            {
                frequency = f;
                width = w;
                size = s;
            }

            public bool Contains(double f) => f >= frequency - width && f <= frequency + width;
        }

        public double Noise = 5;

        List<Hit> Hits = new List<Hit>();

        public void AddHit(double freq, double w, double size)
        {
            Hits.Add(new Hit(freq, w, size));
        }

        public SimulatedFeedback()
        {

        }

        public string Name => "Test";

        public override string ToString() => "Test";

        public double MinFrequency => throw new NotImplementedException();

        public double MaxFrequency => throw new NotImplementedException();

        public double MaxAmplitude => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public bool Relay { get; set; }
        public double Frequency { get; set; }
        public double Amplitude { get; set; }
        public double VoltageOffset { get; set; }
        public double DutyCycle { get; set; }
        public Waveform Waveform { get; set; }

        public void Dispose()
        {
        }

        public double Read()
        {
            double @base = 0;
            // Is the current Frequency in the list?
            foreach (var h in Hits)
            {
                if (h.Contains(Frequency))
                    @base += h.size;
            }

            return @base + rng.NextDouble() * Noise;
        }

        double previousBpm = 60;

        TimeSpan IHeartRateMonitor.Read()
        {
            double delta = Read(); // bpm;
            double newBpm = previousBpm < 60 ? previousBpm + delta : previousBpm - delta;
            previousBpm = newBpm;
            var ts = TimeSpan.FromSeconds(60.0 / newBpm);

            // Fast-forward to animate the output
            Thread.Sleep((int)(ts.TotalMilliseconds / 100.0));
            return ts;
        }

        Random rng = new Random();
    }
}
