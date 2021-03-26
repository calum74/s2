using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Spooky2;
using GeneratorLib;


namespace S2Forms
{
    public partial class MainWindow : Form
    {
        Spooky2.Provider spooky2;
        TreeNode generatorNode, pulsesNode, bookmarksNode, biofeedbackNode;

        public MainWindow()
        {
            InitializeComponent();

            spooky2 = new Spooky2.Provider(@"C:\Spooky2");

            var position = new System.Drawing.Point(260, 40);

            generatorControl = new ManualGeneratorControl();
            generatorControl.SuspendLayout();
            generatorControl.Location = position;
            this.Controls.Add(this.generatorControl);
            generatorControl.ResumeLayout(false);
            generatorControl.PerformLayout();
            generatorControl.Hide();

            programControl = new ProgramControl();
            programControl.SuspendLayout();
            programControl.Location = position;
            this.Controls.Add(this.programControl);
            programControl.ResumeLayout(false);
            programControl.PerformLayout();
            programControl.Hide();

            channelControl = new GeneratorControl2();
            channelControl.SuspendLayout();
            channelControl.Location = position;
            this.Controls.Add(channelControl);
            channelControl.ResumeLayout(false);
            channelControl.PerformLayout();
            channelControl.Hide();

            generatorNode = treeView.Nodes.Add("Signal generators");
            pulsesNode = treeView.Nodes.Add("Heart rate monitors");

            biofeedbackNode = treeView.Nodes.Add("Biofeedback");
            biofeedbackNode.ContextMenuStrip = biofeedbackMenu;

            var programsNode = treeView.Nodes.Add("Programs");
            programsNode.Tag = spooky2.RootPresetCollection;
            programsNode.Nodes.Add("Dummy");
            var customNode = treeView.Nodes.Add("Custom programs");
            bookmarksNode = treeView.Nodes.Add("Bookmarks");

            foreach (var pulse in spooky2.HeartRateMonitors)
            {
                this.pulse = pulse;
                pulseMonitor.DoWork += new DoWorkEventHandler(UpdatePulse);
                pulseMonitor.RunWorkerAsync(pulse);
                pulseMonitor.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateCompleted);

                var n = pulsesNode.Nodes.Add(pulse.Name);
                n.Tag = pulse;
            }

            var gl = spooky2.Generators;
            spooky2.GeneratorAdded += OnGeneratorAdded;
            spooky2.GeneratorRemoved += OnGeneratorRemoved;

            foreach (var gen in gl)
            {
                AddGenerator(gen);
                // gen.Reset();
            }

            // Read the user file
            foreach(var prog in spooky2.Custom.Programs)
            {
                AddRunnable(customNode.Nodes, prog);
            }
        }

        void AddRunnable(TreeNodeCollection nodes, IProgram runnable)
        {
            var n = nodes.Add(runnable.Name);
            n.Tag = runnable;
            if(runnable.Programs.Any())
                n.Nodes.Add("Dummy");
            n.ContextMenuStrip = runnableMenu;
        }

        delegate void AddGeneratorDel(ISignalGenerator gen);

        void OnGeneratorAdded(ISignalGenerator gen)
        {
            this.Invoke(new AddGeneratorDel(AddGenerator), gen);
        }

        void AddGenerator(ISignalGenerator gen)
        {
            // runnableMenu.Items[0]
            var b = runnableMenu.Items[0] as ToolStripMenuItem;
            var n = generatorNode.Nodes.Add(gen.Name);
            n.Tag = gen;

            foreach (var channel in gen.Channels)
            {
                var ec = new EditableChannel(channel);

                var n2 = n.Nodes.Add(ec.Name);
                n2.Tag = ec;

                var name = $"{gen.Name} {ec.Name}";
                var item = b.DropDownItems.Add(name);
                item.Tag = n2;
                item.Click += new EventHandler(OnRunProgramOnGenerator);


                // n2.ContextMenuStrip = ...
            }
        }

        void OnRunProgramOnGenerator(object o, EventArgs a)
        {
            var node = o as ToolStripMenuItem;
            var targetNode = node.Tag as TreeNode;
            var channel = targetNode.Tag as EditableChannel;
            var source = clickedNode.Tag as IProgram;
            if (source is null)
                return;
            channel.Add(source);
            AddRunnable(targetNode.Nodes, source);

            if(DialogResult.Yes == MessageBox.Show("Start program now?", "Run program", MessageBoxButtons.YesNo))
            {
                // D
            }
        }

        void AddPulse(IHeartRateMonitor pulse)
        {
            this.pulse = pulse;
            pulseMonitor.DoWork += new DoWorkEventHandler(UpdatePulse);
            pulseMonitor.RunWorkerAsync(pulse);
            pulseMonitor.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateCompleted);

            var n = pulsesNode.Nodes.Add(pulse.Name);
            n.Tag = pulse;

        }

        void OnGeneratorRemoved(ISignalGenerator gen)
        {
        }

        IHeartRateMonitor pulse;

        void UpdatePulse(object o, DoWorkEventArgs args)
        {
            var p = args.Argument as IHeartRateMonitor;

            try
            {
                var t = p.Read();
                args.Result = t;
            }
            catch (TimeoutException)
            {
                args.Result = new TimeSpan();  // 0 = invalid
            }
        }

        void UpdateCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            TimeSpan ts = (TimeSpan)args.Result;
            var bpm = ts.AsBpm();
            toolStripStatusLabel1.Text = bpm.ToString();
            pulseMonitor.RunWorkerAsync(pulse);
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void LaunchGeneratorControl_Click(object sender, EventArgs e)
        {
            var box = new GeneratorControl();
            box.Show();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OPen a file
            // var d = new FileDialog();

            var d = new OpenFileDialog();
            var r = d.ShowDialog();

        }

        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            switch (e.Node.Tag)
            {
                case IProgramFolder folder:
                    e.Node.Nodes.Clear();

                    // Populate the list
                    foreach (var p in folder.Folders)
                    {
                        var n = e.Node.Nodes.Add(p.Name);
                        n.Tag = p;
                        n.Nodes.Add("Dummy");
                    }
                    foreach (var p in folder.Programs)
                    {
                        var n = e.Node.Nodes.Add(p.Name);
                        n.Tag = p;
                        n.Nodes.Add("Dummy");
                        n.ContextMenuStrip = runnableMenu;
                    }
                    break;
                case IProgram runnable:
                    e.Node.Nodes.Clear();
                    foreach (var p in runnable.Programs)
                    {
                        var n = e.Node.Nodes.Add(p.Name);
                        n.Tag = p;
                        n.ContextMenuStrip = runnableMenu;
                        if(p.Programs.Any())  
                            n.Nodes.Add("Dummy");
                    }
                    break;
            }
        }

        ManualGeneratorControl generatorControl;
        ProgramControl programControl;
        GeneratorControl2 channelControl;

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Tag)
            {
                case ISignalGenerator generator:
                    generatorControl.Generator = generator;
                    channelControl.Hide();
                    generatorControl.Show();
                    programControl.Hide();
                    break;
                case IProgram runnable:
                    programControl.Runnable = runnable;
                    channelControl.Hide();
                    programControl.Show();
                    generatorControl.Hide();
                    break;
                case IChannel channel:
                    channelControl.Show();
                    generatorControl.Hide();
                    programControl.Hide();
                    break;
                default:
                    channelControl.Hide();
                    generatorControl.Hide();
                    programControl.Hide();
                    break;
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        TreeNode clickedNode;

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            clickedNode = e.Node;
        }

        private void AddSimulatedPulseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPulse(new DummyPulse());
        }

        private void RunnableMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void AddSimulatedGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGenerator(new DummyGenerator());
        }
    }
}
