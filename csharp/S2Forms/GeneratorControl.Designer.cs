namespace S2Forms
{
    partial class GeneratorControl
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
            this.generatorList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.manualGeneratorControl1 = new S2Forms.ManualGeneratorControl();
            this.SuspendLayout();
            // 
            // generatorList
            // 
            this.generatorList.AllowDrop = true;
            this.generatorList.FormattingEnabled = true;
            this.generatorList.Location = new System.Drawing.Point(125, 12);
            this.generatorList.Name = "generatorList";
            this.generatorList.Size = new System.Drawing.Size(310, 17);
            this.generatorList.TabIndex = 0;
            this.generatorList.SelectedIndexChanged += new System.EventHandler(this.GeneratorList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Generator";
            // 
            // manualGeneratorControl1
            // 
            this.manualGeneratorControl1.Location = new System.Drawing.Point(13, 46);
            this.manualGeneratorControl1.Name = "manualGeneratorControl1";
            this.manualGeneratorControl1.Size = new System.Drawing.Size(503, 257);
            this.manualGeneratorControl1.TabIndex = 2;
            // 
            // GeneratorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 315);
            this.Controls.Add(this.manualGeneratorControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.generatorList);
            this.Name = "GeneratorControl";
            this.Text = "Generator control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox generatorList;
        private System.Windows.Forms.Label label1;
        private ManualGeneratorControl manualGeneratorControl1;
    }
}