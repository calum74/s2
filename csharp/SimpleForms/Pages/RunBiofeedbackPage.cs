using System;
using System.Windows.Forms;
using GeneratorLib;

namespace SimpleForms
{
    public partial class RunBiofeedbackPage : UserControl
    {
        public RunBiofeedbackPage()
        {
            InitializeComponent();
        }

        Biofeedback bf;
        HrmSource source;
        Action<Biofeedback.Sample[]> finishedAction;

        public void Start(IChannel channel, IHeartRateMonitor hrm, BiofeedbackSettings settings, Action<Biofeedback.Sample[]> finished)
        {
            if (channel is null) throw new ArgumentException(nameof(channel));
            if (hrm is null) throw new ArgumentException(nameof(hrm));

            finishedAction = finished;
            bf = new Biofeedback();

            if (channel is null) throw new ArgumentException("Couldn't open a channel");

            source = new HrmSource(hrm);

            bf.Enqueue(settings.MinFrequency, settings.MaxFrequency, settings.StepSize);
            int frequencySteps = bf.TotalSteps;
            heatmapControl.NumberOfPoints = frequencySteps;
            heatmapControl.MinX = settings.MinFrequency;
            heatmapControl.MaxX = settings.MaxFrequency;
            statusLabel.Text = "Running";
            pauseButton.Text = "Pause";

            biofeedback = new Biofeedback();
            biofeedback.Enqueue(settings.MinFrequency, settings.MaxFrequency, settings.StepSize);
            runner = new BackgroundRunner();
            biofeedback.Source = source;
            biofeedback.OnProgress += new Biofeedback.Progress((f, r, s, t) => BeginInvoke(new Biofeedback.Progress(OnProgress), f, r, s, t));
            biofeedback.OnFinished += new Biofeedback.Finished(r => BeginInvoke(new Biofeedback.Finished(OnFinished), new object[] { r }));
            runner.Progress += new ProgressChanged((s, t, w, a, e) => BeginInvoke(new ProgressChanged(OnGeneratorProgress), s, t, w, a, e));
            var options = new RunningOptions();
            runner.Start(biofeedback, channel, options);
        }

        void OnProgress(double f, double r, int stage, TimeSpan eta)
        {
            response.Text = $"{r:0.0} bpm";
            heartRate.Text = $"{source.LastBpm:0.0} bpm";
            frequency.Text = $"{f} Hz";
            stageLabel.Text = $"{stage} of {biofeedback.Stages}";
            this.eta.Text = $"{eta.Hours:00}:{eta.Minutes:00}:{eta.Seconds:00}";

            heatmapControl.Update(f, r);
        }

        void OnGeneratorProgress(RunState status, double frequency, Waveform w, double amplitude, TimeSpan eta)
        {
            switch(status)
            {
                case RunState.Error:
                    statusLabel.Text = runner.StatusMessage;
                    pauseButton.Text = "Resume";
                    break;
                default:
                    break;
            }
        }

        void OnFinished(Biofeedback.Sample[] samples)
        {
            Stop();
            finishedAction(samples);
        }

        BackgroundRunner runner;
        Biofeedback biofeedback;

        public void Stop()
        {
            if (runner is null) return;

            if (runner.State == RunState.Paused || runner.State == RunState.Running)
            {
                runner.Stop();
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            switch(runner.State)
            {
                case RunState.Running:
                    runner.Pause();
                    pauseButton.Text = "Resume";
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

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to stop this scan?", "Stop scan", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Stop();
                finishedAction(new Biofeedback.Sample[0]);
            }
        }
    }
}
