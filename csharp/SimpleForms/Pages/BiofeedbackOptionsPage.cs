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
    public partial class BiofeedbackOptionsPage : UserControl
    {
        public BiofeedbackOptionsPage()
        {
            InitializeComponent();
            Settings = new BiofeedbackSettings(); 
        }

        public BiofeedbackSettings Settings { get; private set; }

        private void advancedButton_Click(object sender, EventArgs e)
        {
            var options = new BiofeedbackSettingsForm();
            options.Settings = Settings;
            if(options.ShowDialog() == DialogResult.OK)
            {
                Settings = options.Settings;
            }
        }
    }
}
