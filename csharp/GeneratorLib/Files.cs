using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratorLib
{
    public interface IEditable : IProgram
    {
        void Add(IProgram r);

        void Remove(IProgram r);
    }

    public class Databases
    {
        IEnumerable<IProgram> Search(string fragment) => throw new NotImplementedException();

        IEnumerable<IProgram> Search(double frequency) => throw new NotImplementedException();

        SortedDictionary<double, List<IProgram>> lookup;
    }


    public class Editable : IEditable
    {
        readonly List<IProgram> steps = new List<IProgram>();
       
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<IProgram> Programs => steps;

        public IEnumerable<IStep> Steps => Enumerable.Empty<IStep>();

        public TimeSpan Duration(RunningOptions options) => steps.Aggregate(default(TimeSpan), (a, b) => a + b.Duration(options));
        
        public void Add(IProgram r)
        {
            steps.Add(r);
        }

        public void Remove(IProgram r)
        {
            steps.Remove(r);
        }

        public IEnumerable<ICommand> Commands => Enumerable.Empty<ICommand>();
    }

    public class EditableChannel : Editable, IChannel
    {
        public IChannel UnderlyingChannel { get; private set; }

        public EditableChannel(IChannel generatorChannel)
        {
            UnderlyingChannel = generatorChannel;
            Name = UnderlyingChannel.Name;
        }

        public double MinFrequency => UnderlyingChannel.MinFrequency;

        public double MaxFrequency => UnderlyingChannel.MaxFrequency;

        public double MaxAmplitude => UnderlyingChannel.MaxAmplitude;

        public bool Available => UnderlyingChannel.Available;

        public IOpenChannel TryOpen()
        {
            return UnderlyingChannel.TryOpen();
        }
    }
}
