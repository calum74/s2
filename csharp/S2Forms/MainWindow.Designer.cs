namespace S2Forms
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.frequency1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.frequency2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.pulseMonitor = new System.ComponentModel.BackgroundWorker();
            this.treeView = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanForHardwareChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatorControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSimulatedPulseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSimulatedGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biofeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runnableMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.starToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biofeedbackMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.runnableMenu.SuspendLayout();
            this.biofeedbackMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.frequency1,
            this.frequency2,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 304);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(883, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(52, 20);
            this.toolStripStatusLabel1.Text = "0 bpm";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.ToolStripStatusLabel1_Click);
            // 
            // frequency1
            // 
            this.frequency1.Name = "frequency1";
            this.frequency1.Size = new System.Drawing.Size(30, 20);
            this.frequency1.Text = "Off";
            // 
            // frequency2
            // 
            this.frequency2.Name = "frequency2";
            this.frequency2.Size = new System.Drawing.Size(30, 20);
            this.frequency2.Text = "Off";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 18);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(12, 31);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(224, 270);
            this.treeView.TabIndex = 5;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.hardwareToolStripMenuItem,
            this.biofeedbackToolStripMenuItem,
            this.programsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.newToolStripMenuItem.Text = "New...";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // hardwareToolStripMenuItem
            // 
            this.hardwareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanForHardwareChangesToolStripMenuItem,
            this.generatorControlToolStripMenuItem,
            this.addSimulatedPulseToolStripMenuItem,
            this.addSimulatedGeneratorToolStripMenuItem});
            this.hardwareToolStripMenuItem.Name = "hardwareToolStripMenuItem";
            this.hardwareToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
            this.hardwareToolStripMenuItem.Text = "Hardware";
            // 
            // scanForHardwareChangesToolStripMenuItem
            // 
            this.scanForHardwareChangesToolStripMenuItem.Name = "scanForHardwareChangesToolStripMenuItem";
            this.scanForHardwareChangesToolStripMenuItem.Size = new System.Drawing.Size(270, 26);
            this.scanForHardwareChangesToolStripMenuItem.Text = "Scan for hardware changes";
            // 
            // generatorControlToolStripMenuItem
            // 
            this.generatorControlToolStripMenuItem.Name = "generatorControlToolStripMenuItem";
            this.generatorControlToolStripMenuItem.Size = new System.Drawing.Size(270, 26);
            this.generatorControlToolStripMenuItem.Text = "Generator control...";
            // 
            // addSimulatedPulseToolStripMenuItem
            // 
            this.addSimulatedPulseToolStripMenuItem.Name = "addSimulatedPulseToolStripMenuItem";
            this.addSimulatedPulseToolStripMenuItem.Size = new System.Drawing.Size(270, 26);
            this.addSimulatedPulseToolStripMenuItem.Text = "Add simulated pulse";
            this.addSimulatedPulseToolStripMenuItem.Click += new System.EventHandler(this.AddSimulatedPulseToolStripMenuItem_Click);
            // 
            // addSimulatedGeneratorToolStripMenuItem
            // 
            this.addSimulatedGeneratorToolStripMenuItem.Name = "addSimulatedGeneratorToolStripMenuItem";
            this.addSimulatedGeneratorToolStripMenuItem.Size = new System.Drawing.Size(270, 26);
            this.addSimulatedGeneratorToolStripMenuItem.Text = "Add simulated generator";
            this.addSimulatedGeneratorToolStripMenuItem.Click += new System.EventHandler(this.AddSimulatedGeneratorToolStripMenuItem_Click);
            // 
            // biofeedbackToolStripMenuItem
            // 
            this.biofeedbackToolStripMenuItem.Name = "biofeedbackToolStripMenuItem";
            this.biofeedbackToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.biofeedbackToolStripMenuItem.Text = "Biofeedback";
            // 
            // programsToolStripMenuItem
            // 
            this.programsToolStripMenuItem.Name = "programsToolStripMenuItem";
            this.programsToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.programsToolStripMenuItem.Text = "Programs";
            // 
            // runnableMenu
            // 
            this.runnableMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.runnableMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.starToolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.runnableMenu.Name = "runnableMenu";
            this.runnableMenu.Size = new System.Drawing.Size(164, 100);
            this.runnableMenu.Opening += new System.ComponentModel.CancelEventHandler(this.RunnableMenu_Opening);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.runToolStripMenuItem.Text = "Run on";
            // 
            // starToolStripMenuItem1
            // 
            this.starToolStripMenuItem1.Name = "starToolStripMenuItem1";
            this.starToolStripMenuItem1.Size = new System.Drawing.Size(163, 24);
            this.starToolStripMenuItem1.Text = "Bookmark";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // biofeedbackMenu
            // 
            this.biofeedbackMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.biofeedbackMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1});
            this.biofeedbackMenu.Name = "biofeedbackMenu";
            this.biofeedbackMenu.Size = new System.Drawing.Size(109, 28);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(108, 24);
            this.newToolStripMenuItem1.Text = "New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.NewToolStripMenuItem1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 330);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "OpenSpooky";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.runnableMenu.ResumeLayout(false);
            this.biofeedbackMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker pulseMonitor;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem biofeedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel frequency1;
        private System.Windows.Forms.ToolStripStatusLabel frequency2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripMenuItem hardwareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanForHardwareChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatorControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSimulatedPulseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSimulatedGeneratorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip runnableMenu;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem starToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip biofeedbackMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
    }
}

