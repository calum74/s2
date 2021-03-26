using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneratorLib
{
    public enum Waveform
    {
        Default,
        Sine,
        Square,
        AscendingSawtooth,
        DescendingSawtooth,
        Triangle,
        DampedSinusoidal,
        DampedSquare,
        HBombSinusoidal,
        HBombSquare,
        Other,
    }

    public static class WaveformExtensions
    {
        static string[] waveforms = {
            "",
            "Sine",
            "Square",
            "Sawtooth", 
            "Descending sawtooth", 
            "Triangle", 
            "Damped sine",
            "Damped square",
            "H-Bomb sine",
            "H-Bomb square", 
            "" };

        static string[] selectableWaveforms;

        public static IEnumerable<string> SelectableWaveforms => waveforms.Where(w => !string.IsNullOrEmpty(w));

        public static string WaveformToString(this Waveform w) => waveforms[(int)w];

        static Dictionary<string, Waveform> stringToWaveform;

        static WaveformExtensions()
        {
            int i = 0;
            stringToWaveform = new Dictionary<string, Waveform>();
            foreach(var w in waveforms)
            {
                stringToWaveform[w] = (Waveform)i++;
            }
        }

        public static Waveform StringToWaveform(this string str)
        {
            return stringToWaveform[str];
        }
    }
}
