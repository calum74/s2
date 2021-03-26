using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneratorLib;

namespace S2Forms
{
    public partial class SingleChannelControl : UserControl
    {
        public IOpenChannel Channel { get; set; }

        public SingleChannelControl()
        {
            InitializeComponent();
        }

        private void Relay_CheckedChanged(object sender, EventArgs e)
        {
            Channel.Relay = relay.Checked;
        }

        private void Frequency_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(frequency.Text, out double value))
            {
                Channel.Frequency = value;
            }
        }
    }
}
