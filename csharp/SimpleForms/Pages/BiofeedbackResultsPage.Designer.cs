namespace SimpleForms
{
    partial class BiofeedbackResultsPage
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
            this.resultsList = new System.Windows.Forms.ListView();
            this.frequency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.response = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lookup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Results of Biofeedback scan";
            // 
            // resultsList
            // 
            this.resultsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.frequency,
            this.response,
            this.lookup});
            this.resultsList.HideSelection = false;
            this.resultsList.Location = new System.Drawing.Point(34, 57);
            this.resultsList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.resultsList.Name = "resultsList";
            this.resultsList.Size = new System.Drawing.Size(482, 307);
            this.resultsList.TabIndex = 1;
            this.resultsList.UseCompatibleStateImageBehavior = false;
            this.resultsList.View = System.Windows.Forms.View.Details;
            // 
            // frequency
            // 
            this.frequency.Text = "Frequency (Hz)";
            this.frequency.Width = 89;
            // 
            // response
            // 
            this.response.Text = "Response";
            this.response.Width = 64;
            // 
            // lookup
            // 
            this.lookup.Text = "Matches";
            this.lookup.Width = 150;
            // 
            // saveCheckbox
            // 
            this.saveCheckbox.AutoSize = true;
            this.saveCheckbox.Checked = true;
            this.saveCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveCheckbox.Location = new System.Drawing.Point(445, 372);
            this.saveCheckbox.Name = "saveCheckbox";
            this.saveCheckbox.Size = new System.Drawing.Size(71, 24);
            this.saveCheckbox.TabIndex = 2;
            this.saveCheckbox.Text = "Save";
            this.saveCheckbox.UseVisualStyleBackColor = true;
            // 
            // BiofeedbackResultsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveCheckbox);
            this.Controls.Add(this.resultsList);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BiofeedbackResultsPage";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView resultsList;
        private System.Windows.Forms.ColumnHeader frequency;
        private System.Windows.Forms.ColumnHeader response;
        private System.Windows.Forms.ColumnHeader lookup;
        private System.Windows.Forms.CheckBox saveCheckbox;
    }
}
