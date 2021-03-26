using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spooky2;
using GeneratorLib;

namespace S2Forms
{
    public partial class ManualGeneratorControl : UserControl
    {
        ISignalGenerator generator;
        public ISignalGenerator Generator
        {
            get => generator;
            set
            {
                generator = value;
                singleChannelControl1.Channel = generator.Channels.ElementAt(1).TryOpen();
                singleChannelControl2.Channel = generator.Channels.ElementAt(2).TryOpen();
            }
        }

        public ManualGeneratorControl()
        {
            InitializeComponent();
        }
    }
}
