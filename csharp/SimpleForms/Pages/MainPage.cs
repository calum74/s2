using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleForms
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public bool RunProgram => runProgram.Checked;
        public bool RunBiofeedback => biofeedback.Checked;

        private void RunProgram_CheckedChanged(object sender, EventArgs e)
        {
            if(runProgram.Checked)
                biofeedback.Checked = false;
        }

        private void Biofeedback_CheckedChanged(object sender, EventArgs e)
        {
            if (biofeedback.Checked)
                runProgram.Checked = false;
        }
    }
}
