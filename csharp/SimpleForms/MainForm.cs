using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneratorLib;

namespace SimpleForms
{
    public partial class MainForm : Form
    {
        IProvider provider;
        SelectGeneratorPage selectGeneratorPage;
        SelectProgramPage selectProgramPage;
        RunProgramPage runProgramPage;
        RunOptionsPage runOptionsPage;
        BiofeedbackOptionsPage biofeedbackOptionsPage;
        SelectPulsePage selectPulsePage;
        RunBiofeedbackPage runBiofeedbackPage;
        BiofeedbackResultsPage results;

        enum CurrentPage { Main, SelectProgram,
            SelectGenerator, SelectPulse, RunOptions, RunProgram,
            BiofeedbackOptions, RunBiofeedback, Results
            }

        CurrentPage page = CurrentPage.Main;

        public MainForm()
        {
            InitializeComponent();
            provider = new Spooky2.Provider(@"C:\Spooky2");

            selectGeneratorPage = new SelectGeneratorPage(this);
            selectProgramPage = new SelectProgramPage(provider, OnProgramSelected);
            runProgramPage = new RunProgramPage(FinishedProgram);
            runOptionsPage = new RunOptionsPage();
            biofeedbackOptionsPage = new BiofeedbackOptionsPage();
            selectPulsePage = new SelectPulsePage();
            runBiofeedbackPage = new RunBiofeedbackPage();
            results = new BiofeedbackResultsPage();

            selectGeneratorPage.Hide();
            selectProgramPage.Hide();
            runProgramPage.Hide();
            runOptionsPage.Hide();
            biofeedbackOptionsPage.Hide();
            selectPulsePage.Hide();
            runBiofeedbackPage.Hide();
            results.Hide();
            Controls.Add(selectGeneratorPage);
            Controls.Add(selectProgramPage);
            Controls.Add(runProgramPage);
            Controls.Add(runOptionsPage);
            Controls.Add(biofeedbackOptionsPage);
            Controls.Add(selectPulsePage);
            Controls.Add(runBiofeedbackPage);
            Controls.Add(results);

            simulation = new SimulatedFeedback();
            simulation.AddHit(81000, 50, 20);
            simulation.AddHit(820000, 20, 15);
            simulation.AddHit(120000, 10, 10);
            simulation.AddHit(140000, 35, 5);
            simulation.Noise = 5;

            provider.OnHardwareChanged += new HardwareChangedDel(() => BeginInvoke(new HardwareChangedDel(RefreshHardware)));
        }

        void RefreshHardware()
        {
            selectGeneratorPage.RefreshList(provider, simulation);
            selectPulsePage.UpdateList(provider, simulation);
        }

        IChannel selectedChannel;

        public void OnProgramSelected(bool selected)
        {
            nextButton.Enabled = selected;
        }

        public void ChannelSelected(IChannel channel)
        {
            nextButton.Enabled = true;
            selectedChannel = channel;
        }

        private void FinishedProgram()
        {
            nextButton.Enabled = true;
            previousButton.Enabled = false;
            GoToPage(CurrentPage.Main);
        }

        UserControl GetControlForPage(CurrentPage page)
        {
            switch (page)
            {
                case CurrentPage.Main: return mainPage;
                case CurrentPage.RunBiofeedback: return runBiofeedbackPage;
                case CurrentPage.RunProgram: return runProgramPage;
                case CurrentPage.SelectGenerator: return selectGeneratorPage;
                case CurrentPage.SelectProgram: return selectProgramPage;
                case CurrentPage.RunOptions: return runOptionsPage;
                case CurrentPage.BiofeedbackOptions: return biofeedbackOptionsPage;
                case CurrentPage.SelectPulse: return selectPulsePage;
                case CurrentPage.Results: return results;
                default:
                    throw new NotImplementedException();
            }
        }

        SimulatedFeedback simulation;

        void GoToPage(CurrentPage newPage)
        {
            GetControlForPage(page).Hide();

            switch(page)
            {
                case CurrentPage.RunProgram:
                    runProgramPage.Stop();
                    break;
            }

            // Adapt buttons etc
            previousButton.Enabled = true;
            nextButton.Enabled = false;
            switch (newPage)
            {
                case CurrentPage.Main:
                    previousButton.Enabled = false;
                    nextButton.Enabled = true;
                    break;
                case CurrentPage.SelectGenerator:
                    selectGeneratorPage.RefreshList(provider, simulation);
                    break;
                case CurrentPage.RunProgram:
                    previousButton.Enabled = false;
                    runProgramPage.Start(selectProgramPage.SelectedProgram, selectedChannel, runOptionsPage.Options);
                    break;
                case CurrentPage.SelectPulse:
                    selectPulsePage.UpdateList(provider, simulation);
                    nextButton.Enabled = true;
                    break;
                case CurrentPage.BiofeedbackOptions:
                case CurrentPage.RunOptions:
                    nextButton.Enabled = true;
                    break;
                case CurrentPage.RunBiofeedback:
                    previousButton.Enabled = false;                    
                    runBiofeedbackPage.Start(selectGeneratorPage.SelectedItem, selectPulsePage.SelectedItem, biofeedbackOptionsPage.Settings, BiofeedbackFinished);
                    break;
                case CurrentPage.Results:
                    previousButton.Enabled = false;
                    nextButton.Enabled = true;
                    break;
                case CurrentPage.SelectProgram:
                    if (selectProgramPage.SelectedProgram != null)
                        nextButton.Enabled = true;
                    break;
            }

            GetControlForPage(newPage).Show();
            page = newPage;
        }

        void BiofeedbackFinished(Biofeedback.Sample[] r)
        {
            // nextButton.Enabled = true;
            results.SetResults(r, provider.ReverseLookup);
            GoToPage(CurrentPage.Results);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            switch(page)
            {
                case CurrentPage.Main:
                    GoToPage(mainPage.RunBiofeedback ? CurrentPage.SelectGenerator : CurrentPage.SelectProgram);
                    break;
                case CurrentPage.SelectProgram:
                    GoToPage(CurrentPage.SelectGenerator);
                    break;
                case CurrentPage.SelectGenerator:
                    GoToPage(mainPage.RunBiofeedback ? CurrentPage.SelectPulse : CurrentPage.RunOptions);
                    break;
                case CurrentPage.RunOptions:
                    GoToPage(CurrentPage.RunProgram);
                    break;
                case CurrentPage.RunProgram:
                    GoToPage(CurrentPage.Main);
                    break;
                case CurrentPage.SelectPulse:
                    GoToPage(CurrentPage.BiofeedbackOptions);
                    break;
                case CurrentPage.BiofeedbackOptions:
                    GoToPage(CurrentPage.RunBiofeedback);
                    break;
                case CurrentPage.RunBiofeedback:
                    GoToPage(CurrentPage.Results);
                    break;
                case CurrentPage.Results:
                    GoToPage(CurrentPage.Main);
                    break;
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            switch(page)
            {
                case CurrentPage.SelectGenerator:
                    GoToPage(mainPage.RunBiofeedback ? CurrentPage.Main : CurrentPage.SelectProgram);
                    break;
                case CurrentPage.SelectProgram:
                    GoToPage(CurrentPage.Main);
                    break;
                case CurrentPage.RunProgram:
                    GoToPage(CurrentPage.RunOptions);
                    break;
                case CurrentPage.SelectPulse:
                    GoToPage(CurrentPage.SelectGenerator);
                    break;
                case CurrentPage.RunBiofeedback:
                    runBiofeedbackPage.Stop();
                    GoToPage(CurrentPage.BiofeedbackOptions);
                    break;
                case CurrentPage.RunOptions:
                    GoToPage(CurrentPage.SelectGenerator);
                    break;
                case CurrentPage.BiofeedbackOptions:
                    GoToPage(CurrentPage.SelectPulse);
                    break;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            runProgramPage.Stop();
            runBiofeedbackPage.Stop();
        }
    }
}
