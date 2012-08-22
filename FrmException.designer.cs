namespace ShowLib
{
    partial class FE {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btClose = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tbDetails = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(730, 183);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 0;
            this.btClose.Text = "Fechar";
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(1, -1);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMessage.Size = new System.Drawing.Size(815, 168);
            this.tbMessage.TabIndex = 1;
            // 
            // tbDetails
            // 
            this.tbDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDetails.Location = new System.Drawing.Point(1, 221);
            this.tbDetails.Multiline = true;
            this.tbDetails.Name = "tbDetails";
            this.tbDetails.ReadOnly = true;
            this.tbDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDetails.Size = new System.Drawing.Size(815, 216);
            this.tbDetails.TabIndex = 3;
            // 
            // FE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 438);
            this.ControlBox = false;
            this.Controls.Add(this.tbDetails);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmException";
            this.Load += new System.EventHandler(this.FE_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        public System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbDetails;
    }
}