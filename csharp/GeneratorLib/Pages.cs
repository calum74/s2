using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorLib
{
    class Page
    {
        public event EventHandler NextButtonChanged;
        public event EventHandler PrevButtonChanged;

        public bool NextButton { get; protected set; }
        public bool PrevButton { get; protected set; }
    }

    enum Icon
    {
        Folder, Program, Step
    }

    class Node<T>
    {
        public string name;
        public Icon icon;
        public T data;
    }

    class GeneratorPage : Page
    {
        IEnumerable<Node<IChannel>> Channels;
        IChannel SelectedChannel { get; }
    }

    delegate void RunProgress(double outputFrequency, double outputAmplitude, RunState state, TimeSpan remainingTime);

    class RunProgramPage : Page
    {
        // Called every second, or 
        event RunProgress OnProgress;

        void Start(IRunnable runnable, IOpenChannel channel)
        {

        }

        bool Running { get; set; }
    }
}
