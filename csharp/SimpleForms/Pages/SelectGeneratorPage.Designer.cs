namespace SimpleForms
{
    partial class SelectGeneratorPage
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
            this.generatorList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.missingGeneratorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // generatorList
            // 
            this.generatorList.FormattingEnabled = true;
            this.generatorList.ItemHeight = 20;
            this.generatorList.Location = new System.Drawing.Point(66, 73);
            this.generatorList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.generatorList.Name = "generatorList";
            this.generatorList.Size = new System.Drawing.Size(348, 224);
            this.generatorList.TabIndex = 0;
            this.generatorList.SelectedIndexChanged += new System.EventHandler(this.GeneratorList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a generator to run on";
            // 
            // missingGeneratorLabel
            // 
            this.missingGeneratorLabel.AutoSize = true;
            this.missingGeneratorLabel.Location = new System.Drawing.Point(62, 325);
            this.missingGeneratorLabel.Name = "missingGeneratorLabel";
            this.missingGeneratorLabel.Size = new System.Drawing.Size(224, 20);
            this.missingGeneratorLabel.TabIndex = 2;
            this.missingGeneratorLabel.Text = "Warning, no generators found!";
            // 
            // SelectGeneratorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.missingGeneratorLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.generatorList);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SelectGeneratorPage";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox generatorList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label missingGeneratorLabel;
    }
}
