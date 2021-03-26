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
    public partial class SelectGeneratorPage : UserControl
    {
        readonly MainForm form;

        public SelectGeneratorPage(MainForm form)
        {
            this.form = form;

            InitializeComponent();
        }

        public void RefreshList(IProvider provider, IChannel dummy)
        {
            generatorList.Items.Clear();

            foreach(var i in provider.Generators.SelectMany(g=>g.Channels))
            {
                var item = generatorList.Items.Add(i);
            }

            if (generatorList.Items.Count == 0)
            {
                missingGeneratorLabel.Visible = true;
            }
            else
            {
                missingGeneratorLabel.Visible = false;
            }

            generatorList.Items.Add(dummy);

            generatorList.SelectedIndex = 0;
        }

        public IChannel SelectedItem { get; private set; }

        private void GeneratorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItem = (IChannel)generatorList.SelectedItem;
            form.ChannelSelected(SelectedItem);
        }
    }
}
