using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace GeneratorLib
{
    public delegate void ProgressChanged(RunState status, double frequency, Waveform w, double amplitude, TimeSpan eta);

    class SynchronousRunner
    {
        private IEnumerable<IStep> Steps(IProgram program)
        {
            foreach (var s in program.Steps)
                yield return s;
            foreach (var step in program.Programs.SelectMany(Steps))
                yield return step;
        }

        public IEnumerable<Command> Run(IProgram program, RunningOptions options)
        {
            TimeSpan duration = program.Duration(options);
            foreach (var step in Steps(program))
            {
                duration -= step.Duration(options);
                foreach(var cmd in step.Commands(options))
                {
                    var cmd2 = cmd;
                    cmd2.Remaining += duration;
                    yield return cmd2;
                }
            }
        }

        IEnumerator<Command> commands;

        public void Start(IProgram program, RunningOptions options)
        {
            commands = Run(program, options).GetEnumerator();
        }

        public bool Next(IOpenChannel channel, out double freq, out Waveform waveform, out double amplitude, out TimeSpan eta)
        {
            if(commands.MoveNext())
            {
                commands.Current.Cmd.Run(channel);
                freq = commands.Current.Frequency;
                eta = commands.Current.Remaining;
                waveform = commands.Current.Waveform;
                amplitude = commands.Current.Amplitude;
                Thread.Sleep(commands.Current.Cmd.Duration);
                return true;
            }
            else
            {
                freq = 0;
                eta = default(TimeSpan);
                waveform = Waveform.Default;
                amplitude = 0;
                return false;
            }
        }
    }

    /// <summary>
    /// Runs a given command in the background.
    /// </summary>
    public class BackgroundRunner : IDisposable
    {
        SynchronousRunner runner;
        public event ProgressChanged Progress;

        IChannel channel;
        IOpenChannel openChannel;
        Thread runningThread;

        public void Start(IProgram runnable, IChannel channel, RunningOptions options)
        {
            this.channel = channel;
            runner = new SynchronousRunner();
            runner.Start(runnable, options);
            source = new CancellationTokenSource();
            eta = runnable.Duration(options);

            state = RunState.Ready;
            runningThread = new Thread(ThreadFn);
            runningThread.Start();
        }

        TimeSpan eta;
        CancellationTokenSource source;

        void ThreadFn()
        {
            try
            {
                NativeMethods.PreventSleep();

                if (openChannel is null)
                {
                    openChannel = channel.TryOpen();
                }
                if (openChannel is null)
                {
                    state = RunState.Error;
                    Progress(RunState.Error, 0, Waveform.Default, 0, eta);
                    return;
                }

                State = RunState.Running;
                openChannel.Relay = true;
                double freq;
                Waveform waveform;
                double amplitude;

                while (runner.Next(openChannel, out freq, out waveform, out amplitude, out eta))
                {
                    Progress(RunState.Running, freq, waveform, amplitude, eta);
                    if (source.Token.IsCancellationRequested)
                    {
                        openChannel.Relay = false;
                        return;
                    }
                }

                openChannel.Relay = false;
                Progress(RunState.Completed, freq, waveform, amplitude, eta);
                State = RunState.Completed;
            }
            catch(HardwareException ex)
            {
                hwex = ex;
                state = RunState.Error;
                Progress(RunState.Error, 0, Waveform.Default, 0, eta);
            }
            catch
            {
                // Shouldn't really get here :-(
                state = RunState.Error;
                Progress(RunState.Error, 0, Waveform.Default, 0, eta);
            }
            finally
            {
                try
                {
                    openChannel?.Close();
                }
                catch
                {
                }
                openChannel = null;
                NativeMethods.AllowSleep();
            }
        }

        public void Pause()
        {
            source.Cancel();
            if (State == RunState.Running)
            {
                runningThread.Join();
                State = RunState.Paused;
            }
        }

        public void Resume()
        {
            // if (State != RunState.Paused) throw new InvalidOperationException("Not paused");
            State = RunState.Running;
            source = new CancellationTokenSource();
            runningThread = new Thread(ThreadFn);
            runningThread.Start();
        }

        public void Stop()
        {
            switch(State)
            {
                case RunState.Running:
                    source.Cancel();
                    State = RunState.Aborted;
                    runningThread.Join();
                    break;
                case RunState.Paused:
                    break;
                default:
                    throw new InvalidOperationException("Invalid stop");
            }
            State = RunState.Aborted;
        }

        public void Dispose()
        {
            openChannel?.Dispose();
        }

        object mutex = new object();
        RunState state;

        HardwareException hwex;

        public RunState State
        {
            get
            {
                lock (mutex) return state;
            }
            private set
            {
                lock (mutex) state = value;
            }
        }

        public string StatusMessage
        {
            get
            {
                lock (mutex)
                {
                    switch(state)
                    {
                        case RunState.Aborted:
                            return "Aborted";
                        case RunState.Completed:
                            return "Finished";
                        case RunState.Error:
                            return $"Error: {hwex.Cause} - {hwex.Remedy}";
                        case RunState.Paused:
                            return "Paused";
                        case RunState.Ready:
                            return "Ready";
                        case RunState.Running:
                            return "Running";
                    }
                    return "";
                }
            }
        }
    }

    public enum DeviceType
    {
        Generator, Hrm
    }

    public class HardwareException : Exception
    {
        public DeviceType Device { get; }
        public ErrorCode Code { get; }
        public string Cause { get; }
        public string Remedy { get; }

        public HardwareException(DeviceType d, ErrorCode c, string probableCause, string remedy)
        {
            Device = d;
            Code = c;
            Cause = probableCause;
            Remedy = remedy;
        }
    }

    public enum RunState
    {
        Ready,
        Running,
        Paused,
        Aborted,
        Completed,
        Error
    }

    public enum ErrorCode
    {
        // Device is working correctly
        NoError,

        // Device disconnected or unplugged. Perhaps switched off.
        Timeout,

        CommunicationError,

        // Could not access the device. Perhaps used in another app.
        DeviceInUse,

        // Unspecified error
        OtherError
    }
}
