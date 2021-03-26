namespace S2Forms
{
    partial class ManualGeneratorControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.invertAndSync = new System.Windows.Forms.CheckBox();
            this.singleChannelControl2 = new S2Forms.SingleChannelControl();
            this.singleChannelControl1 = new S2Forms.SingleChannelControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.singleChannelControl1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 241);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.singleChannelControl2);
            this.groupBox2.Controls.Add(this.invertAndSync);
            this.groupBox2.Location = new System.Drawing.Point(247, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 244);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel 2";
            // 
            // invertAndSync
            // 
            this.invertAndSync.AutoSize = true;
            this.invertAndSync.Location = new System.Drawing.Point(6, 219);
            this.invertAndSync.Name = "invertAndSync";
            this.invertAndSync.Size = new System.Drawing.Size(109, 19);
            this.invertAndSync.TabIndex = 7;
            this.invertAndSync.Text = "Invert and sync";
            this.invertAndSync.UseVisualStyleBackColor = true;
            // 
            // singleChannelControl2
            // 
            this.singleChannelControl2.Channel = null;
            this.singleChannelControl2.Location = new System.Drawing.Point(23, 22);
            this.singleChannelControl2.Name = "singleChannelControl2";
            this.singleChannelControl2.Size = new System.Drawing.Size(213, 191);
            this.singleChannelControl2.TabIndex = 26;
            // 
            // singleChannelControl1
            // 
            this.singleChannelControl1.Channel = null;
            this.singleChannelControl1.Location = new System.Drawing.Point(6, 19);
            this.singleChannelControl1.Name = "singleChannelControl1";
            this.singleChannelControl1.Size = new System.Drawing.Size(213, 194);
            this.singleChannelControl1.TabIndex = 0;
            // 
            // ManualGeneratorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ManualGeneratorControl";
            this.Size = new System.Drawing.Size(503, 247);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox invertAndSync;
        private SingleChannelControl singleChannelControl1;
        private SingleChannelControl singleChannelControl2;
    }
}
