namespace SimpleForms
{
    partial class RunSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunSettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.waveformBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.amplitudeBox = new System.Windows.Forms.TextBox();
            this.dwellBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.fmCheck = new System.Windows.Forms.CheckBox();
            this.amCheck = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fmFrequency = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mwConstant = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bpConstant = new System.Windows.Forms.TextBox();
            this.tfConstant = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fmAmplitude = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.amFrequency = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.amAmplitude = new System.Windows.Forms.TextBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.loopCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Waveform";
            // 
            // waveformBox
            // 
            this.waveformBox.FormattingEnabled = true;
            this.waveformBox.Location = new System.Drawing.Point(146, 10);
            this.waveformBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.waveformBox.Name = "waveformBox";
            this.waveformBox.Size = new System.Drawing.Size(168, 24);
            this.waveformBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Amplitude (V)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Dwell (s)";
            // 
            // amplitudeBox
            // 
            this.amplitudeBox.Location = new System.Drawing.Point(146, 43);
            this.amplitudeBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.amplitudeBox.Name = "amplitudeBox";
            this.amplitudeBox.Size = new System.Drawing.Size(168, 22);
            this.amplitudeBox.TabIndex = 4;
            // 
            // dwellBox
            // 
            this.dwellBox.Location = new System.Drawing.Point(146, 75);
            this.dwellBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dwellBox.Name = "dwellBox";
            this.dwellBox.Size = new System.Drawing.Size(168, 22);
            this.dwellBox.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(162, 463);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(101, 28);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(289, 463);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 28);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fmCheck
            // 
            this.fmCheck.AutoSize = true;
            this.fmCheck.Location = new System.Drawing.Point(34, 118);
            this.fmCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fmCheck.Name = "fmCheck";
            this.fmCheck.Size = new System.Drawing.Size(170, 21);
            this.fmCheck.TabIndex = 8;
            this.fmCheck.Text = "Frequency modulation";
            this.fmCheck.UseVisualStyleBackColor = true;
            this.fmCheck.CheckedChanged += new System.EventHandler(this.fmCheck_CheckedChanged);
            // 
            // amCheck
            // 
            this.amCheck.AutoSize = true;
            this.amCheck.Location = new System.Drawing.Point(30, 211);
            this.amCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.amCheck.Name = "amCheck";
            this.amCheck.Size = new System.Drawing.Size(165, 21);
            this.amCheck.TabIndex = 9;
            this.amCheck.Text = "Amplitude modulation";
            this.amCheck.UseVisualStyleBackColor = true;
            this.amCheck.CheckedChanged += new System.EventHandler(this.amCheck_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Frequency (Hz)";
            // 
            // fmFrequency
            // 
            this.fmFrequency.Location = new System.Drawing.Point(288, 144);
            this.fmFrequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fmFrequency.Name = "fmFrequency";
            this.fmFrequency.Size = new System.Drawing.Size(89, 22);
            this.fmFrequency.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "MW to Hz";
            // 
            // mwConstant
            // 
            this.mwConstant.Location = new System.Drawing.Point(146, 307);
            this.mwConstant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mwConstant.Name = "mwConstant";
            this.mwConstant.Size = new System.Drawing.Size(179, 22);
            this.mwConstant.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 347);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "BP to Hz";
            // 
            // bpConstant
            // 
            this.bpConstant.Location = new System.Drawing.Point(146, 343);
            this.bpConstant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bpConstant.Name = "bpConstant";
            this.bpConstant.Size = new System.Drawing.Size(179, 22);
            this.bpConstant.TabIndex = 15;
            // 
            // tfConstant
            // 
            this.tfConstant.Location = new System.Drawing.Point(146, 380);
            this.tfConstant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tfConstant.Name = "tfConstant";
            this.tfConstant.Size = new System.Drawing.Size(179, 22);
            this.tfConstant.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 380);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 17);
            this.label7.TabIndex = 17;
            this.label7.Text = "Tissue factor";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(142, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "Amplitude (Hz)";
            // 
            // fmAmplitude
            // 
            this.fmAmplitude.Location = new System.Drawing.Point(288, 174);
            this.fmAmplitude.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fmAmplitude.Name = "fmAmplitude";
            this.fmAmplitude.Size = new System.Drawing.Size(89, 22);
            this.fmAmplitude.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(142, 242);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Frequency (Hz)";
            // 
            // amFrequency
            // 
            this.amFrequency.Location = new System.Drawing.Point(288, 236);
            this.amFrequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.amFrequency.Name = "amFrequency";
            this.amFrequency.Size = new System.Drawing.Size(89, 22);
            this.amFrequency.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(142, 270);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Amplitude (V)";
            // 
            // amAmplitude
            // 
            this.amAmplitude.Location = new System.Drawing.Point(288, 266);
            this.amAmplitude.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.amAmplitude.Name = "amAmplitude";
            this.amAmplitude.Size = new System.Drawing.Size(89, 22);
            this.amAmplitude.TabIndex = 23;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(35, 463);
            this.resetButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(99, 28);
            this.resetButton.TabIndex = 24;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // loopCheck
            // 
            this.loopCheck.AutoSize = true;
            this.loopCheck.Checked = true;
            this.loopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loopCheck.Location = new System.Drawing.Point(36, 423);
            this.loopCheck.Name = "loopCheck";
            this.loopCheck.Size = new System.Drawing.Size(102, 21);
            this.loopCheck.TabIndex = 25;
            this.loopCheck.Text = "Run in loop";
            this.loopCheck.UseVisualStyleBackColor = true;
            // 
            // RunSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 527);
            this.Controls.Add(this.loopCheck);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.amAmplitude);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.amFrequency);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.fmAmplitude);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tfConstant);
            this.Controls.Add(this.bpConstant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mwConstant);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fmFrequency);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.amCheck);
            this.Controls.Add(this.fmCheck);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.dwellBox);
            this.Controls.Add(this.amplitudeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.waveformBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RunSettingsForm";
            this.Text = "Advanced generator settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox waveformBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox amplitudeBox;
        private System.Windows.Forms.TextBox dwellBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox fmCheck;
        private System.Windows.Forms.CheckBox amCheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fmFrequency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mwConstant;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox bpConstant;
        private System.Windows.Forms.TextBox tfConstant;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox fmAmplitude;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox amFrequency;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox amAmplitude;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.CheckBox loopCheck;
    }
}