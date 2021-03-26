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
    public partial class ProgramControl : UserControl
    {
        IProgram runnable;

        public IProgram Runnable
        {
            get => runnable;
            set
            {
                runnable = value;
                description.Text = runnable.Description;
                this.program.Nodes.Clear();
                AddSteps(program.Nodes, runnable);
            }
        }

        void AddSteps(TreeNodeCollection nodes, IProgram runnable)
        {
            foreach (var p in runnable.Programs)
            {
                var node = nodes.Add(p.Name);
                node.Tag = p;
                if(p.Programs.Any())
                    node.Nodes.Add("Dummy");
            }
        }

        public ProgramControl()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RunProgram_Click(object sender, EventArgs e)
        {

        }

        private void QueueProgram_Click(object sender, EventArgs e)
        {

        }

        private void Program_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            switch(e.Node.Tag)
            {
                case IProgram runnable:
                    e.Node.Nodes.Clear();
                    AddSteps(e.Node.Nodes, runnable);
                    break;
            }
        }
    }
}
