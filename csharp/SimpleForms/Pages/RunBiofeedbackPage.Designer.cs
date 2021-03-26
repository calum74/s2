namespace SimpleForms
{
    partial class RunBiofeedbackPage
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
            this.label2 = new System.Windows.Forms.Label();
            this.eta = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.frequency = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.heartRate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.response = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.stageLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.heatmapControl = new SimpleForms.HeatmapControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 192);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time remaining";
            // 
            // eta
            // 
            this.eta.AutoSize = true;
            this.eta.Location = new System.Drawing.Point(182, 192);
            this.eta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.eta.Name = "eta";
            this.eta.Size = new System.Drawing.Size(49, 20);
            this.eta.TabIndex = 3;
            this.eta.Text = "00:00";
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(245, 356);
            this.pauseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(112, 35);
            this.pauseButton.TabIndex = 4;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(389, 356);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(112, 35);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 233);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Frequency";
            // 
            // frequency
            // 
            this.frequency.AutoSize = true;
            this.frequency.Location = new System.Drawing.Point(182, 233);
            this.frequency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(42, 20);
            this.frequency.TabIndex = 7;
            this.frequency.Text = "0 Hz";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 271);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Heart rate";
            // 
            // heartRate
            // 
            this.heartRate.AutoSize = true;
            this.heartRate.Location = new System.Drawing.Point(182, 271);
            this.heartRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.heartRate.Name = "heartRate";
            this.heartRate.Size = new System.Drawing.Size(53, 20);
            this.heartRate.TabIndex = 9;
            this.heartRate.Text = "0 bpm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 309);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Response";
            // 
            // response
            // 
            this.response.AutoSize = true;
            this.response.Location = new System.Drawing.Point(182, 309);
            this.response.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.response.Name = "response";
            this.response.Size = new System.Drawing.Size(49, 20);
            this.response.TabIndex = 11;
            this.response.Text = "0.000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Stage";
            // 
            // stageLabel
            // 
            this.stageLabel.AutoSize = true;
            this.stageLabel.Location = new System.Drawing.Point(182, 149);
            this.stageLabel.Name = "stageLabel";
            this.stageLabel.Size = new System.Drawing.Size(31, 20);
            this.stageLabel.TabIndex = 13;
            this.stageLabel.Text = "1/5";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(182, 20);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 20);
            this.statusLabel.TabIndex = 14;
            this.statusLabel.Text = "Running";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Progress";
            // 
            // heatmapControl
            // 
            this.heatmapControl.Location = new System.Drawing.Point(38, 84);
            this.heatmapControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.heatmapControl.MaxX = 100D;
            this.heatmapControl.MinX = 0D;
            this.heatmapControl.Name = "heatmapControl";
            this.heatmapControl.NumberOfPoints = 0;
            this.heatmapControl.Size = new System.Drawing.Size(463, 31);
            this.heatmapControl.TabIndex = 0;
            this.heatmapControl.Text = "heatmapControl1";
            this.heatmapControl.Threshold = 10D;
            // 
            // RunBiofeedbackPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.stageLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.response);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.heartRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.frequency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.eta);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.heatmapControl);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RunBiofeedbackPage";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HeatmapControl heatmapControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label eta;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label frequency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label heartRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label response;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label stageLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label7;
    }
}
