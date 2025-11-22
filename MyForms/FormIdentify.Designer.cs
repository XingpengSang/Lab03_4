
namespace Lab03_4.MyForms
{
    partial class FormIdentify
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
            this.listBoxInfo = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxInfo
            // 
            this.listBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxInfo.FormattingEnabled = true;
            this.listBoxInfo.ItemHeight = 18;
            this.listBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.listBoxInfo.Name = "listBoxInfo";
            this.listBoxInfo.Size = new System.Drawing.Size(800, 450);
            this.listBoxInfo.TabIndex = 0;
            // 
            // FormIdentify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBoxInfo);
            this.Name = "FormIdentify";
            this.Text = "要素信息";
            this.Load += new System.EventHandler(this.FormIdentify_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxInfo;
    }
}