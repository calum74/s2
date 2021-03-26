namespace SimpleForms
{
    partial class SelectProgramPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.programView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a program to run";
            // 
            // programView
            // 
            this.programView.Location = new System.Drawing.Point(22, 53);
            this.programView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.programView.Name = "programView";
            this.programView.ShowLines = false;
            this.programView.Size = new System.Drawing.Size(506, 344);
            this.programView.TabIndex = 1;
            this.programView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProgramView_BeforeExpand);
            this.programView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ProgramView_AfterSelect);
            // 
            // SelectProgramPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.programView);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SelectProgramPage";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView programView;
    }
}
