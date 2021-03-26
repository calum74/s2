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

namespace SimpleForms
{
    public partial class RunOptionsPage : UserControl
    {
        public RunOptionsPage()
        {
            InitializeComponent();
            Options = new RunningOptions();
        }

        public RunningOptions Options { get; set; }

        private void advancedButton_Click(object sender, EventArgs e)
        {
            var options = new RunSettingsForm();
            options.Options = Options;
            if(options.ShowDialog() == DialogResult.OK)
            {
                Options = options.Options;
            }
        }
    }
}
