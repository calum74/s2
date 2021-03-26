using System;
using System.Collections.Generic;

namespace GeneratorLib.Next
{
    class GeneratorState
    {
        double frequency1, frequency2;
        double amplitude;
        double phase, offset;
        int waveform;
    }

    enum Status
    {
        Stopped, Running, Paused, Holding
    }

    delegate void StatusChanged(Status status);
    delegate void FrequencyChanged(double frequency);

    class Generator : IScheduledItem
    {
        IChannel channel;
        IOpenChannel openChannel;

        // Events

        public event StatusChanged OnStatusChanged;
        public event FrequencyChanged OnFrequencyChanged;

        List<IProgram> programs;

        public Generator(IChannel channel)
        {

        }

        public void Append(IProgram program)
        {

        }

        public void Remove(int position)
        {
            // What happens if we're running the current position?
            programs.RemoveAt(position);
        }



        //
        public void Clear()
        {
            Stop();
            programs.Clear();
        }

        public void Start()
        {
        }

        public void Stop()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {
        }

        public double Step(double d)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicate whether the program should loop once it has reached the end.
        /// </summary>
        public bool Loop { get; set; } = true;

        /// <summary>
        /// Gets the total time that the program should run.
        /// </summary>
        public TimeSpan TotalTime { get; }
        public TimeSpan RemainingTime { get; }

        /// <summary>
        /// Gets the progress as a percentage between 0 and 100
        /// </summary>
        public double Progress { get; }

        Status State { get; }

        Program current;

        // Events:
        // Frequency updated
        // Generator state updated
        // Program step updated
    }

    struct ScheduledCommand
    {
        public double time;
        public ICommand command;
        public IOpenChannel channel;
    }

    class Step
    {
        double frequency1, frequency2;
        double amplitude;
        double phase, offset;
        int waveform;
    }

    class Program2
    {
        Step[] steps;
    }

    class Runner
    {
        // If previous is null
        void Apply(IOpenChannel channel, Step step, Step previous)
        {

        }
    }

    interface IScheduledItem
    {
        double Step(double d);
    }

    class TestProgram
    {
        void Start(Scheduler sched)
        {

        }

        void Stop()
        {

        }

        double Run(double time)
        {
            return 0;
        }
    }

    class ProgramBeingRun : IScheduledItem
    {
        IOpenChannel output;

        public double Step(double d)
        {
            throw new NotImplementedException();
        }
    }

    class Scheduler
    {
        Scheduler()
        {

        }

        void Add(ScheduledCommand command)
        {
            commands.Add(command);
        }

        public double TimeOfNextCommand() => commands.Any() ? commands.Peek().time : 0.0;

        public bool GetNextCommand(out ScheduledCommand command) => commands.TryPop(out command);

        class ScheduledCommandComparer : IComparer<ScheduledCommand>
        {
            public int Compare(ScheduledCommand x, ScheduledCommand y)
            {
                return x.time < y.time ? -1 : x.time > y.time ? 1 : 0;
            }
        }

        Heap<ScheduledCommand> commands = new Heap<ScheduledCommand>(new ScheduledCommandComparer());
    }
}
