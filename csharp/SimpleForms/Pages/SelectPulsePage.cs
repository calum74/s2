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
    public partial class SelectPulsePage : UserControl
    {
        public SelectPulsePage()
        {
            InitializeComponent();
        }

        public void UpdateList(IProvider provider, IHeartRateMonitor dummy)
        {
            hrmList.Items.Clear();

            foreach(var hrm in provider.HeartRateMonitors)
            {
                hrmList.Items.Add(hrm);
            }

            hrmList.Items.Add(dummy);

            hrmList.SetSelected(0, true);
        }

        public IHeartRateMonitor SelectedItem { get; private set; }

        private void HrmList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItem = hrmList.SelectedItem as IHeartRateMonitor;
        }
    }
}
