namespace FileUploadWithResume.AppClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnUpload = new Button();
            btnSelectFile = new Button();
            txtFilePath = new TextBox();
            btnCancel = new Button();
            txtProgress = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnUpload
            // 
            btnUpload.Location = new Point(228, 64);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(106, 31);
            btnUpload.TabIndex = 0;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(671, 18);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(106, 31);
            btnSelectFile.TabIndex = 1;
            btnSelectFile.Text = "SelectFile";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(30, 22);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(622, 27);
            txtFilePath.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(350, 64);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(106, 31);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtProgress
            // 
            txtProgress.Font = new Font("Microsoft JhengHei UI", 9F);
            txtProgress.Location = new Point(30, 139);
            txtProgress.Multiline = true;
            txtProgress.Name = "txtProgress";
            txtProgress.Size = new Size(747, 286);
            txtProgress.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 107);
            label1.Name = "label1";
            label1.Size = new Size(74, 19);
            label1.TabIndex = 5;
            label1.Text = "Progress:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(txtProgress);
            Controls.Add(btnCancel);
            Controls.Add(txtFilePath);
            Controls.Add(btnSelectFile);
            Controls.Add(btnUpload);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnUpload;
        private Button btnSelectFile;
        private TextBox txtFilePath;
        private Button btnCancel;
        private TextBox txtProgress;
        private Label label1;
    }
}
