using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GeneratorLib;

namespace Spooky2
{
    class Spooky2Preset : IProgram
    {
        // Typically a file
        string Path { get; }

        public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);

        IProgram[] steps;

        public IEnumerable<IProgram> Programs { get { Load(); return steps; } }

        public IEnumerable<IStep> Steps => Enumerable.Empty<IStep>();

        public TimeSpan Duration(RunningOptions options) => Programs.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options));

        public string PresetName { get; private set; }

        string description;
        public string Description { get { Load(); return description; } }

        bool loaded = false;

        void Load()
        {
            if (loaded) return;
            var contents = FileUtils.ParseKeyValues(FileUtils.ReadQuotedLines(Path));
            List<string> programs = new List<string>();
            List<string> frequencies = new List<string>();

            foreach (var c in contents)
            {
                if (c.Key == "PresetName")
                    PresetName = c.Value;
                else if (c.Key == "Preset_Notes")
                    description = c.Value;
                else if (c.Key == "Loaded_Programs")
                    programs.Add(c.Value);
                else if (c.Key == "Loaded_Frequencies")
                    frequencies.Add(c.Value);
            }

            // !! Read the step size from the preset

            steps = programs.Zip(frequencies, (p, f) => new Spooky2Program(p, f, 180)).ToArray();
            loaded = true;
        }

        public Spooky2Preset(string filename)
        {
            Path = filename;
        }
        public IEnumerable<ICommand> Commands => Enumerable.Empty<ICommand>();
    }

    public class Spooky2Program : IProgram
    {
        public string Description => Name;

        public Spooky2Program(string name, string command, double stepSize)
        {
            Name = name;
            commands = ProgramParser.Parse(command).ToArray();

            // Debug - reconstruct the program.
            var debug = StoredProgramData.SetCommands(commands);
            if (debug != command)
            {

            }
        }

        IStep[] commands;

        public string Name { get; private set; }

        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        public IEnumerable<IStep> Steps => commands;

        public TimeSpan Duration(RunningOptions options) => Steps.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options));

        public IEnumerable<ICommand> Commands => Enumerable.Empty<ICommand>();
    }
}
