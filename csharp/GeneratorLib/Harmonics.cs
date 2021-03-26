using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneratorLib
{
    interface ICustomWaveform
    {
        /// <summary>
        /// Gets the value of the waveform at a given angle
        /// 0 <= angle < 2pi.
        /// -1 <= result <= 1
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        double ValueAt(double angle);
    }

    public class Harmonic
    {
        public int harmonic;
        public double magnitude;
        public double phase;
    }

    public class Harmonics : ICustomWaveform
    {
        Harmonic[] harmonics;
        double magnitude;

        public Harmonics(Harmonic[] h)
        {
            harmonics = h;
            magnitude = MaxMagnitude(harmonics);
        }

        public static double ValueAt(IEnumerable<Harmonic> harmonics, double x)
        {
            return harmonics.
                    Select(h => h.magnitude * Math.Sin(h.harmonic * (x - h.phase))).
                    Sum();
        }

        static double MaxMagnitude(IEnumerable<Harmonic> harmonics)
            => harmonics.Sum(h => h.magnitude);

        public double ValueAt(double angle) => ValueAt(harmonics, angle)/magnitude;
    }
}
