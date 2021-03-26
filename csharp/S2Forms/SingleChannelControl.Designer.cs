namespace S2Forms
{
    partial class SingleChannelControl
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
            this.dutyCycle = new System.Windows.Forms.TextBox();
            this.waveform = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.phase = new System.Windows.Forms.TextBox();
            this.relay = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.amplitude = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.frequency = new System.Windows.Forms.TextBox();
            this.offset = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dutyCycle
            // 
            this.dutyCycle.Location = new System.Drawing.Point(109, 167);
            this.dutyCycle.Name = "dutyCycle";
            this.dutyCycle.Size = new System.Drawing.Size(100, 20);
            this.dutyCycle.TabIndex = 38;
            // 
            // waveform
            // 
            this.waveform.FormattingEnabled = true;
            this.waveform.Items.AddRange(new object[] {
            "Square",
            "Sine",
            "Sawtooth"});
            this.waveform.Location = new System.Drawing.Point(109, 115);
            this.waveform.Name = "waveform";
            this.waveform.Size = new System.Drawing.Size(100, 17);
            this.waveform.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 15);
            this.label6.TabIndex = 37;
            this.label6.Text = "Waveform";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 15);
            this.label12.TabIndex = 27;
            this.label12.Text = "Frequency (Hz)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(0, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 15);
            this.label11.TabIndex = 29;
            this.label11.Text = "Amplitude (V)";
            // 
            // phase
            // 
            this.phase.Location = new System.Drawing.Point(109, 138);
            this.phase.Name = "phase";
            this.phase.Size = new System.Drawing.Size(100, 20);
            this.phase.TabIndex = 35;
            // 
            // relay
            // 
            this.relay.AutoSize = true;
            this.relay.Location = new System.Drawing.Point(3, 3);
            this.relay.Name = "relay";
            this.relay.Size = new System.Drawing.Size(94, 19);
            this.relay.TabIndex = 26;
            this.relay.TabStop = false;
            this.relay.Text = "Output relay";
            this.relay.UseVisualStyleBackColor = true;
            this.relay.CheckedChanged += new System.EventHandler(this.Relay_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 15);
            this.label7.TabIndex = 34;
            this.label7.Text = "Phase (%)";
            // 
            // amplitude
            // 
            this.amplitude.Location = new System.Drawing.Point(109, 61);
            this.amplitude.Name = "amplitude";
            this.amplitude.Size = new System.Drawing.Size(100, 20);
            this.amplitude.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 15);
            this.label9.TabIndex = 33;
            this.label9.Text = "Duty cycle (%)";
            // 
            // frequency
            // 
            this.frequency.Location = new System.Drawing.Point(109, 36);
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(100, 20);
            this.frequency.TabIndex = 28;
            this.frequency.TextChanged += new System.EventHandler(this.Frequency_TextChanged);
            // 
            // offset
            // 
            this.offset.Location = new System.Drawing.Point(109, 88);
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(100, 20);
            this.offset.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 15);
            this.label10.TabIndex = 31;
            this.label10.Text = "Offset (V)";
            // 
            // SingleChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dutyCycle);
            this.Controls.Add(this.waveform);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.phase);
            this.Controls.Add(this.relay);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.amplitude);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.frequency);
            this.Controls.Add(this.offset);
            this.Controls.Add(this.label10);
            this.Name = "SingleChannelControl";
            this.Size = new System.Drawing.Size(213, 194);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dutyCycle;
        private System.Windows.Forms.ListBox waveform;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox phase;
        private System.Windows.Forms.CheckBox relay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox amplitude;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox frequency;
        private System.Windows.Forms.TextBox offset;
        private System.Windows.Forms.Label label10;
    }
}
