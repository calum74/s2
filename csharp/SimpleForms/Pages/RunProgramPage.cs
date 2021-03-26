using System;
using System.Windows.Forms;
using GeneratorLib;

namespace SimpleForms
{
    public partial class RunProgramPage : UserControl
    {
        public RunProgramPage(Action onFinished)
        {
            this.onFinished = onFinished;
            InitializeComponent();
        }

        BackgroundRunner runner;
        Action onFinished;

        IChannel channel;

        bool runInLoop;

        public void Start(IProgram runnable, IChannel channel, RunningOptions options)
        {
            this.options = options;
            this.runnable = runnable;
            this.channel = channel;

            var timeRemaining = runnable.Duration(options);
            etaLabel.Text = timeRemaining.ToString();

            programLabel.Text = runnable.Name;
            generatorLabel.Text = channel.ToString();
            amplitudeLabel.Text = $"10 V";
            waveformLabel.Text = "Sine";
            statusLabel.Text = "Running";
            pauseButton.Text = "Pause";
            pauseButton.Enabled = true;
            stopButton.Enabled = true;

            runner = new BackgroundRunner();
            runner.Progress += new ProgressChanged((a, b, c, d, e) => this.BeginInvoke(new ProgressChanged(ProgressUpdated), a, b, c, d, e));


            runner.Start(runnable, channel, options);
        }

        RunningOptions options;
        IProgram runnable;

        void ProgressUpdated(RunState state, double frequency, Waveform w, double amplitude, TimeSpan timeleft)
        {
            if(state == RunState.Error)
            {
                Pause("Error - check hardware");
            }
            if(!double.IsNaN(frequency))
                frequencyLabel.Text = $"{frequency:0.00} Hz";
            etaLabel.Text = $"{timeleft.Hours:00}:{timeleft.Minutes:00}:{timeleft.Seconds:00}";

            if (!double.IsNaN(amplitude))
                amplitudeLabel.Text = $"{amplitude:0.00} V";

            if (w != Waveform.Default)
                waveformLabel.Text = w.WaveformToString();

            if(state == RunState.Completed)
            {
                if(options.runInLoop)
                {
                    runner.Start(runnable, channel, options);
                }
                else
                {
                    Finished();
                    onFinished();
                }
            }
        }

        void Pause(string status)
        {
            statusLabel.Text = runner.StatusMessage;
            runner.Pause();
            pauseButton.Text = "Resume";
        }

        
        void Finished()
        {
            statusLabel.Text = "Finished";
            etaLabel.Text = "00:00:00";
            TurnOffOutput();
            pauseButton.Enabled = false;
            stopButton.Enabled = false;
        }

        void TurnOffOutput()
        {
            waveformLabel.Text = "";
            amplitudeLabel.Text = "";
            frequencyLabel.Text = "";
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            switch(runner.State)
            {
                case RunState.Running:
                    runner.Pause();
                    pauseButton.Text = "Resume";
                    statusLabel.Text = "Paused";
                    statusLabel.Text = "Paused";
                    break;
                case RunState.Paused:
                case RunState.Error:
                    runner.Resume();
                    pauseButton.Text = "Pause";
                    statusLabel.Text = "Running";
                    break;
            }
        }

        public void Stop()
        {
            if (runner is null) return;
            switch(runner.State)
            {
                case RunState.Running:
                case RunState.Paused:
                    runner.Stop();
                    Finished();
                    break;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to stop the current program?", "Stop program", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Stop();
                onFinished();
            }
        }
    }
}
