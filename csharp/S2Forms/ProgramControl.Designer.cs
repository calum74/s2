namespace S2Forms
{
    partial class ProgramControl
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
            this.description = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.program = new System.Windows.Forms.TreeView();
            this.runProgram = new System.Windows.Forms.Button();
            this.queueProgram = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.duration = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Description";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(80, 4);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(358, 57);
            this.description.TabIndex = 1;
            this.description.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Program";
            // 
            // program
            // 
            this.program.Location = new System.Drawing.Point(80, 67);
            this.program.Name = "program";
            this.program.Size = new System.Drawing.Size(358, 185);
            this.program.TabIndex = 3;
            this.program.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.Program_BeforeExpand);
            // 
            // runProgram
            // 
            this.runProgram.Location = new System.Drawing.Point(455, 4);
            this.runProgram.Name = "runProgram";
            this.runProgram.Size = new System.Drawing.Size(75, 23);
            this.runProgram.TabIndex = 4;
            this.runProgram.Text = "Run now";
            this.runProgram.UseVisualStyleBackColor = true;
            this.runProgram.Click += new System.EventHandler(this.RunProgram_Click);
            // 
            // queueProgram
            // 
            this.queueProgram.Location = new System.Drawing.Point(455, 37);
            this.queueProgram.Name = "queueProgram";
            this.queueProgram.Size = new System.Drawing.Size(75, 23);
            this.queueProgram.TabIndex = 5;
            this.queueProgram.Text = "Queue";
            this.queueProgram.UseVisualStyleBackColor = true;
            this.queueProgram.Click += new System.EventHandler(this.QueueProgram_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(455, 79);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(79, 19);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Favourite";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(455, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Duration";
            // 
            // duration
            // 
            this.duration.AutoSize = true;
            this.duration.Location = new System.Drawing.Point(455, 227);
            this.duration.Name = "duration";
            this.duration.Size = new System.Drawing.Size(16, 15);
            this.duration.TabIndex = 8;
            this.duration.Text = "   ";
            // 
            // ProgramControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.duration);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.queueProgram);
            this.Controls.Add(this.runProgram);
            this.Controls.Add(this.program);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.description);
            this.Controls.Add(this.label1);
            this.Name = "ProgramControl";
            this.Size = new System.Drawing.Size(582, 256);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView program;
        private System.Windows.Forms.Button runProgram;
        private System.Windows.Forms.Button queueProgram;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label duration;
    }
}
