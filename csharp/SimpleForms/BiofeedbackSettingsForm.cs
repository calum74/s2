using GeneratorLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleForms
{
    public partial class BiofeedbackSettingsForm : Form
    {
        public BiofeedbackSettingsForm()
        {
            InitializeComponent();
            foreach (var form in WaveformExtensions.SelectableWaveforms)
                waveform.Items.Add(form);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                var _ = Settings;
            }
            catch(FormatException)
            {
                MessageBox.Show("Invalid number format", "Invalid format", MessageBoxButtons.OK);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public BiofeedbackSettings Settings
        {
            get => new BiofeedbackSettings()
            {
                MinFrequency = From,
                MaxFrequency = To,
                StepSize = Step,
                RescanStepSize = Rescan,
                Waveform = Waveform,
                Stagger = Stagger,
                Threshold = Threshold
            };

            set
            {
                From = value.MinFrequency;
                To = value.MaxFrequency;
                Step = value.StepSize;
                Rescan = value.RescanStepSize;
                Waveform = value.Waveform;
                Stagger = value.Stagger;
                Threshold = value.Threshold;
            }
        }

        public double From
        {
            get => double.Parse(startFrequency.Text);
            set { startFrequency.Text = value.ToString(); }
        }

        public double To
        {
            get => double.Parse(endFrequency.Text);
            set { endFrequency.Text = value.ToString(); }
        }

        public double Step
        {
            get => double.Parse(step.Text);
            set { step.Text = value.ToString(); }
        }

        public double Stages
        {
            get => int.Parse(stages.Text);
            set { stages.Text = value.ToString(); }
        }

        public double Rescan
        {
            get => double.Parse(rescanStep.Text);
            set { rescanStep.Text = value.ToString(); }
        }

        public double Threshold
        {
            get => double.Parse(threshold.Text);
            set { threshold.Text = value.ToString(); }
        }

        public GeneratorLib.Waveform Waveform
        {
            get => waveform.Text.StringToWaveform();
            set
            {
                waveform.Text = value.WaveformToString();
            }
        }

        public bool Stagger 
        {
            get => stagger.Checked;
            set { stagger.Checked = value; }
        }
    }
}
