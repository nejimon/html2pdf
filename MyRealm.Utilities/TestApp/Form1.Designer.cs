namespace TestApp
{
    partial class Form1
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
            this.btnListDemo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnListDemo
            // 
            this.btnListDemo.Location = new System.Drawing.Point(21, 12);
            this.btnListDemo.Name = "btnListDemo";
            this.btnListDemo.Size = new System.Drawing.Size(146, 65);
            this.btnListDemo.TabIndex = 1;
            this.btnListDemo.Text = "Demo";
            this.btnListDemo.UseVisualStyleBackColor = true;
            this.btnListDemo.Click += new System.EventHandler(this.btnListDemo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 93);
            this.Controls.Add(this.btnListDemo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnListDemo;
    }
}

