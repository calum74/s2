namespace SimpleForms
{
    partial class MainPage
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
            this.runProgram = new System.Windows.Forms.RadioButton();
            this.biofeedback = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // runProgram
            // 
            this.runProgram.AutoSize = true;
            this.runProgram.Checked = true;
            this.runProgram.Location = new System.Drawing.Point(106, 150);
            this.runProgram.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.runProgram.Name = "runProgram";
            this.runProgram.Size = new System.Drawing.Size(140, 24);
            this.runProgram.TabIndex = 0;
            this.runProgram.TabStop = true;
            this.runProgram.Text = "Run a program";
            this.runProgram.UseVisualStyleBackColor = true;
            this.runProgram.CheckedChanged += new System.EventHandler(this.RunProgram_CheckedChanged);
            // 
            // biofeedback
            // 
            this.biofeedback.AutoSize = true;
            this.biofeedback.Location = new System.Drawing.Point(106, 184);
            this.biofeedback.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.biofeedback.Name = "biofeedback";
            this.biofeedback.Size = new System.Drawing.Size(123, 24);
            this.biofeedback.TabIndex = 1;
            this.biofeedback.Text = "Biofeedback";
            this.biofeedback.UseVisualStyleBackColor = true;
            this.biofeedback.CheckedChanged += new System.EventHandler(this.Biofeedback_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 98);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "What do you want to do?";
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.biofeedback);
            this.Controls.Add(this.runProgram);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainPage";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton runProgram;
        private System.Windows.Forms.RadioButton biofeedback;
        private System.Windows.Forms.Label label1;
    }
}
