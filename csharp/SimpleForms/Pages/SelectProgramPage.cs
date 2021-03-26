using System.Linq;
using System.Windows.Forms;
using GeneratorLib;
using System;


namespace SimpleForms
{
    public partial class SelectProgramPage : UserControl
    {
        IProvider provider;
        Action<bool> onSelect;

        public SelectProgramPage(IProvider provider, Action<bool> selectAction)
        {
            InitializeComponent();
            this.provider = provider;
            onSelect = selectAction;

            AddNodes(provider, programView.Nodes);

            var list = new ImageList();

            list.Images.Add(Resource1.OxygenFolder);
            list.Images.Add(Resource1.OxygenTaskList);
            list.Images.Add(Resource1.OxygenLightening);
            list.Images.Add(Resource1.OxygenPlus);

            programView.ImageList = list;
        }

        void AddNodes(IProvider provider, TreeNodeCollection nodes)
        {
            var n = nodes.Add(provider.ToString());
            n.ImageIndex = 0;
            n.SelectedImageIndex = 0;
            AddNodes(provider.RootPresetCollection, n.Nodes);
            n.Expand();
        }

        void AddNodes(IProgramFolder folder, TreeNodeCollection nodes)
        {
            foreach(var program in folder.Programs)
            {
                var n = nodes.Add(program.Name);
                n.ImageIndex = 1;
                n.SelectedImageIndex = 1;
                n.Tag = program;
                n.Nodes.Add("Dummy");
            }

            foreach(var f in folder.Folders)
            {
                var n = nodes.Add(f.Name);
                n.ImageIndex = 0;
                n.SelectedImageIndex = 0;
                n.Tag = f;
                n.Nodes.Add("Dummy");
            }
        }

        void AddSteps(IProgram runnable, TreeNodeCollection nodes)
        {
            foreach(var step in runnable.Programs)
            {
                var n = nodes.Add(step.Name);
                n.Tag = step;
                n.ImageIndex = 3;
                n.SelectedImageIndex = 3;
                if (step.Programs.Any() || step.Steps.Any())
                    n.Nodes.Add("Dummy");
            }

            foreach (var step in runnable.Steps)
            {
                var n = nodes.Add(step.ToString());
                n.Tag = step;
                n.ImageIndex = 2;
                n.SelectedImageIndex = 2;
            }
        }

        private void ProgramView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            switch (e.Node.Tag)
            {
                case IProgram runnable:
                    e.Node.Nodes.Clear();
                    AddSteps(runnable, e.Node.Nodes);
                    break;
                case IProgramFolder folder:
                    e.Node.Nodes.Clear();
                    AddNodes(folder, e.Node.Nodes);
                    break;

            }

        }

        public IProgram SelectedProgram { get; private set; }

        private void ProgramView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch(e.Node.Tag)
            {
                case IProgram runnable:
                    SelectedProgram = runnable;
                    onSelect(true);
                    break;
                case IStep step:
                    SelectedProgram = new SingleStepProgram(step);
                    onSelect(true);
                    break;
                default:
                    SelectedProgram = null;
                    onSelect(false);
                    break;
            }
        }
    }
}
