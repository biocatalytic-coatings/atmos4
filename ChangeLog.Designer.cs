﻿namespace AtMoS3
{
    partial class ChangeLog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.V10 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Version4 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.V10.SuspendLayout();
            this.Version4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Version4);
            this.tabControl1.Controls.Add(this.V10);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(287, 426);
            this.tabControl1.TabIndex = 1;
            // 
            // V10
            // 
            this.V10.Controls.Add(this.label5);
            this.V10.Controls.Add(this.label4);
            this.V10.Controls.Add(this.label3);
            this.V10.Controls.Add(this.label1);
            this.V10.Controls.Add(this.label2);
            this.V10.Location = new System.Drawing.Point(4, 22);
            this.V10.Name = "V10";
            this.V10.Padding = new System.Windows.Forms.Padding(3);
            this.V10.Size = new System.Drawing.Size(279, 400);
            this.V10.TabIndex = 0;
            this.V10.Text = "Version 3.0";
            this.V10.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(251, 39);
            this.label5.TabIndex = 4;
            this.label5.Text = "Version 3.0.3\r\n    - Add entries to app.config to activate/deactivate\r\n       fun" +
    "ctions depending on organisation.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Development versions.";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Version 3.0.2\r\n    - Use backgroundworkers for all functions.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 65);
            this.label1.TabIndex = 1;
            this.label1.Text = "Version 3.0.1\r\n    - Datafile creation now includes writing column\r\n      heading" +
    "s.\r\n    - Display error messages for blank experiment \r\n      description or dat" +
    "afile name.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 26);
            this.label2.TabIndex = 0;
            this.label2.Text = "Version 3.0.0\r\n    - Original version.";
            // 
            // Version4
            // 
            this.Version4.Controls.Add(this.label7);
            this.Version4.Controls.Add(this.label6);
            this.Version4.Location = new System.Drawing.Point(4, 22);
            this.Version4.Name = "Version4";
            this.Version4.Size = new System.Drawing.Size(279, 400);
            this.Version4.TabIndex = 1;
            this.Version4.Text = "Version 4.0";
            this.Version4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Development versions.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 26);
            this.label7.TabIndex = 5;
            this.label7.Text = "Version 4.0.0\r\n    - Clone Version 3.4 and clean up code.";
            // 
            // ChangeLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(311, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "ChangeLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Log";
            this.tabControl1.ResumeLayout(false);
            this.V10.ResumeLayout(false);
            this.V10.PerformLayout();
            this.Version4.ResumeLayout(false);
            this.Version4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage V10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage Version4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}