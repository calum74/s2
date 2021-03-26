using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace GeneratorLib
{
    /// <summary>
    /// A folder or grouping of programs.
    /// It can contain subfolders.
    /// </summary>
    public interface IProgramFolder
    {
        string Name { get; }

        IEnumerable<IProgram> Programs { get; }

        IEnumerable<IProgramFolder> Folders { get; }
    }

    public interface IRunnable
    {
        string Name { get; }

        TimeSpan Duration(RunningOptions options);
    }

    /// <summary>
    /// A single program, consisting of a number of program steps.
    /// Can also be a compound program, that can be run in its entitety
    /// or as individual sub-programs.
    /// </summary>
    public interface IProgram : IRunnable
    {
        string Description { get; }
        IEnumerable<IProgram> Programs { get; }
        IEnumerable<IStep> Steps { get; }
    }



    public delegate void ProgressMonitor(double frequency, string stage, TimeSpan remainingTime);

    /// <summary>
    /// A command to a signal generator.
    /// </summary>
    public interface ICommand
    {
        void Run(IOpenChannel channel);

        TimeSpan Duration { get; }
    }

 

    [Serializable]
    public class Program : IProgram
    {
        public string name, description;
        public Program[] programs;
        public ProgramStep[] steps;

        public static Program Clone(IProgram source)
        {
            var r = new Program();
            r.name = source.Name;
            r.description = source.Description;
            r.programs = source.Programs.Select(Clone).ToArray();
            r.steps = source.Steps.Cast<ProgramStep>().ToArray();
            if (source.Steps.Any())
                ;

            return r;
        }

        public string Name => name;

        public string Description => description;

        public IEnumerable<IProgram> Programs => programs;

        public IEnumerable<IStep> Steps => steps;

        public TimeSpan Duration(RunningOptions options) => Programs.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options));

        public IEnumerable<ICommand> Commands => Enumerable.Empty<ICommand>();
    }

    [Serializable]
    public class ProgramFolder : IProgramFolder
    {
        [XmlElement(ElementName = "Name")]
        public string name;

        [XmlArray("Programs")]
        public Program[] programs;

        // public Runnable[] runnables;
        // public ProgramStep[] steps;

        [XmlArray("Folders")]
        public ProgramFolder[] folders;

        // Creates a deep copy
        public static ProgramFolder Clone(IProgramFolder origin)
        {
            var p = new ProgramFolder();

            p.name = origin.Name;
            p.folders = origin.Folders.Select(Clone).ToArray();
            p.programs = origin.Programs.Select(Program.Clone).ToArray();

            return p;
        }

        public string Name => name;

        public IEnumerable<IProgram> Programs => programs;

        public IEnumerable<IProgramFolder> Folders => folders;

        public void SaveBinary(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this);
            }
        }

        public void SaveXml(string filename)
        {
            var serializer = new XmlSerializer(typeof(ProgramFolder));

            using (var writer = File.CreateText(filename))
            {
                serializer.Serialize(writer, this);
            }
        }

        public void SaveDatabase(string filename)
        {
            using (var stream = File.Create(filename))
            {
                StoredProgramFolder.Create(stream, this);
            }
        }
    }

    public class ProgramFolderData
    {
        public string name;
        public StoredProgramFolder[] folders;
        public StoredProgram[] programs;
    }

    public class StoredProgramFolder : StreamData<ProgramFolderData>, IProgramFolder
    {
        public string Name => Value.name;

        public override string ToString() => Name;

        public IEnumerable<IProgramFolder> Folders => Value.folders;
        public IEnumerable<IProgram> Programs => Value.programs;
        
        public static StoredProgramFolder Create(Stream stream, IProgramFolder source)
        {
            var result = new StoredProgramFolder();
            result.Create(stream);
            stream.WriteString(source.Name);
            var programs = source.Programs.ToArray();
            var folders = source.Folders.ToArray();
            var programData = StreamArray<StoredProgram>.Create(stream, programs.Length);
            var folderData = StreamArray<StoredProgramFolder>.Create(stream, folders.Length);
            programData.Value = programs.Select(p => StoredProgram.Create(stream, p)).ToArray();
            folderData.Value = folders.Select(p => StoredProgramFolder.Create(stream, p)).ToArray();
            return result;
        }

        protected override ProgramFolderData GetValue()
        {
            var data = new ProgramFolderData();
            data.name = Stream.ReadString();
            data.programs = Stream.ReadArray<StoredProgram>();
            data.folders = Stream.ReadArray<StoredProgramFolder>();
            return data;
        }

        protected override void SetValue(ProgramFolderData t)
        {
        }
    }

    public class StoredProgramData
    {
        public string name;
        public StoredProgram[] programs;
        public string commands;

        public static string SetCommands(IEnumerable<IStep> commands)
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var c in commands)
            {
                if (first) first = false;
                else sb.Append(",");
                c.Write(sb);
            }
            return sb.ToString();
        }
    }

    public class StoredProgram : StreamData<StoredProgramData>, IProgram
    {
        public string Description => "";

        public IEnumerable<IProgram> Programs => Value.programs;

        public IEnumerable<IStep> Steps => ProgramParser.Parse(Value.commands);

        public string Name => Value.name;

        public override string ToString() => Name;

        public TimeSpan Duration(RunningOptions options) =>
            Steps.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options)) +
            Programs.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options));

        protected override StoredProgramData GetValue()
        {
            var sd = new StoredProgramData();
            sd.name = Stream.ReadString();
            sd.commands = Stream.ReadString();
            sd.programs = Stream.ReadArray<StoredProgram>();
            return sd;
        }

        protected override void SetValue(StoredProgramData t)
        {
            Stream.WriteString(t.name);
            Stream.WriteString(t.commands);
            var programArray = StreamArray<StoredProgram>.Create(Stream, t.programs.Length);

            programArray.Value = t.programs;
        }

        public static StoredProgram Create(Stream stream, IProgram program)
        {
            var result = new StoredProgram();
            result.Create(stream);
            stream.WriteString(program.Name);
            stream.WriteString(StoredProgramData.SetCommands(program.Steps));
            var programs = program.Programs.ToArray();
            var programArray = StreamArray<StoredProgram>.Create(stream, programs.Length);
            programArray.Value = programs.Select(p => StoredProgram.Create(stream, p)).ToArray();
            return result;
        }
    }

    public class SingleStepProgram : IProgram
    {
        readonly IStep Step;
        public SingleStepProgram(IStep step)
        {
            Step = step;
        }

        public string Description => Step.ToString();

        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        public IEnumerable<IStep> Steps { get { yield return Step; } }

        public string Name => Step.ToString();

        public TimeSpan Duration(RunningOptions options) => Step.Duration(options);
    }
}
