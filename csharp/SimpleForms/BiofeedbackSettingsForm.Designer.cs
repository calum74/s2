namespace SimpleForms
{
    partial class BiofeedbackSettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.waveform = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stagger = new System.Windows.Forms.CheckBox();
            this.startFrequency = new System.Windows.Forms.TextBox();
            this.endFrequency = new System.Windows.Forms.TextBox();
            this.step = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stages = new System.Windows.Forms.TextBox();
            this.rescanStep = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.threshold = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start frequency (Hz)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "End frequency (Hz)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Step size (Hz)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Waveform";
            // 
            // waveform
            // 
            this.waveform.FormattingEnabled = true;
            this.waveform.Location = new System.Drawing.Point(221, 206);
            this.waveform.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.waveform.Name = "waveform";
            this.waveform.Size = new System.Drawing.Size(225, 24);
            this.waveform.TabIndex = 4;
            this.waveform.Text = "Sine";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Number of stages";
            // 
            // stagger
            // 
            this.stagger.AutoSize = true;
            this.stagger.Location = new System.Drawing.Point(221, 247);
            this.stagger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stagger.Name = "stagger";
            this.stagger.Size = new System.Drawing.Size(149, 21);
            this.stagger.TabIndex = 6;
            this.stagger.Text = "Frequency stagger";
            this.stagger.UseVisualStyleBackColor = true;
            // 
            // startFrequency
            // 
            this.startFrequency.Location = new System.Drawing.Point(221, 21);
            this.startFrequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startFrequency.Name = "startFrequency";
            this.startFrequency.Size = new System.Drawing.Size(157, 22);
            this.startFrequency.TabIndex = 7;
            this.startFrequency.Text = "76000";
            // 
            // endFrequency
            // 
            this.endFrequency.Location = new System.Drawing.Point(221, 55);
            this.endFrequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.endFrequency.Name = "endFrequency";
            this.endFrequency.Size = new System.Drawing.Size(157, 22);
            this.endFrequency.TabIndex = 8;
            this.endFrequency.Text = "145000";
            // 
            // step
            // 
            this.step.Location = new System.Drawing.Point(221, 92);
            this.step.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.step.Name = "step";
            this.step.Size = new System.Drawing.Size(157, 22);
            this.step.TabIndex = 9;
            this.step.Text = "20";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Rescan frequency (Hz)";
            // 
            // stages
            // 
            this.stages.Location = new System.Drawing.Point(221, 129);
            this.stages.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stages.Name = "stages";
            this.stages.Size = new System.Drawing.Size(89, 22);
            this.stages.TabIndex = 11;
            this.stages.Text = "4";
            // 
            // rescanStep
            // 
            this.rescanStep.Location = new System.Drawing.Point(221, 167);
            this.rescanStep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rescanStep.Name = "rescanStep";
            this.rescanStep.Size = new System.Drawing.Size(89, 22);
            this.rescanStep.TabIndex = 12;
            this.rescanStep.Text = "5";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(249, 338);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(85, 31);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(363, 338);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 31);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 288);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Rescan threshold";
            // 
            // threshold
            // 
            this.threshold.Location = new System.Drawing.Point(221, 283);
            this.threshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(157, 22);
            this.threshold.TabIndex = 16;
            this.threshold.Text = "5";
            // 
            // BiofeedbackSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 395);
            this.Controls.Add(this.threshold);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.rescanStep);
            this.Controls.Add(this.stages);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.step);
            this.Controls.Add(this.endFrequency);
            this.Controls.Add(this.startFrequency);
            this.Controls.Add(this.stagger);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.waveform);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "BiofeedbackSettingsForm";
            this.Text = "Biofeedback Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox waveform;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox stagger;
        private System.Windows.Forms.TextBox startFrequency;
        private System.Windows.Forms.TextBox endFrequency;
        private System.Windows.Forms.TextBox step;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox stages;
        private System.Windows.Forms.TextBox rescanStep;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox threshold;
    }
}