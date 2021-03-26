using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneratorLib;

namespace SimpleForms
{
    public partial class RunSettingsForm : Form
    {
        public RunSettingsForm()
        {
            InitializeComponent();
            foreach (var form in WaveformExtensions.SelectableWaveforms)
                waveformBox.Items.Add(form);

            Options = new RunningOptions();
        }

        public RunningOptions Options
        { 
            get
            {
                var result = new RunningOptions();

                result.frequencyModulation = fmCheck.Checked;
                result.fmFrequency = double.Parse(fmFrequency.Text);
                result.fmAmplitudeHz = double.Parse(fmAmplitude.Text);

                result.amplitideModulation = amCheck.Checked;
                result.amFrequency = double.Parse(amFrequency.Text);
                result.amAmplitude = double.Parse(amAmplitude.Text);

                result.defaultWaveform = waveformBox.Text.StringToWaveform();
                result.defaultDuration = TimeSpan.FromSeconds(double.Parse(dwellBox.Text));
                result.defaultAmplitude = double.Parse(amplitudeBox.Text);

                result.mw_to_hz_factor = double.Parse(mwConstant.Text);
                result.bp_to_hz_factor = double.Parse(bpConstant.Text);
                result.tissue_factor = double.Parse(tfConstant.Text);
                result.runInLoop = loopCheck.Enabled;

                return result;
            }
            set
            {
                waveformBox.Text = value.defaultWaveform.WaveformToString();
                amplitudeBox.Text = value.defaultAmplitude.ToString();
                dwellBox.Text = value.defaultDuration.TotalSeconds.ToString();

                amCheck.Checked = value.amplitideModulation;
                amAmplitude.Enabled = value.amplitideModulation;
                amAmplitude.Text = value.amAmplitude.ToString();
                amFrequency.Enabled = value.amplitideModulation;
                amFrequency.Text = value.amFrequency.ToString();

                fmFrequency.Enabled = value.frequencyModulation;
                fmFrequency.Text = value.amFrequency.ToString();

                fmCheck.Checked = value.frequencyModulation;
                fmAmplitude.Text = value.fmAmplitudeHz.ToString();
                fmFrequency.Text = value.fmFrequency.ToString();
                fmAmplitude.Enabled = value.frequencyModulation;
                fmFrequency.Enabled = value.frequencyModulation;

                mwConstant.Text = value.mw_to_hz_factor.ToString();
                bpConstant.Text = value.bp_to_hz_factor.ToString();
                tfConstant.Text = value.tissue_factor.ToString();
                loopCheck.Enabled = value.runInLoop;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dummy = Options;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Invalid value", "An item entered was invalid", MessageBoxButtons.OK);
            }
        }

        private void fmCheck_CheckedChanged(object sender, EventArgs e)
        {
            fmAmplitude.Enabled = fmCheck.Checked;
            fmFrequency.Enabled = fmCheck.Checked;
        }

        private void amCheck_CheckedChanged(object sender, EventArgs e)
        {
            amAmplitude.Enabled = amCheck.Checked;
            amFrequency.Enabled = amCheck.Checked;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Options = new RunningOptions();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}
