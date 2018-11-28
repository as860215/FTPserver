namespace FTP_Server
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Start = new System.Windows.Forms.Button();
            this.lb_status = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.list_log = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(12, 12);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(103, 54);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "Start Server";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.button1_Click);
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Location = new System.Drawing.Point(12, 77);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(91, 15);
            this.lb_status.TabIndex = 1;
            this.lb_status.Text = "Connect Status";
            // 
            // btn_Close
            // 
            this.btn_Close.Enabled = false;
            this.btn_Close.Location = new System.Drawing.Point(135, 12);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(103, 54);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Close Server";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // list_log
            // 
            this.list_log.FormattingEnabled = true;
            this.list_log.ItemHeight = 15;
            this.list_log.Location = new System.Drawing.Point(10, 109);
            this.list_log.Name = "list_log";
            this.list_log.Size = new System.Drawing.Size(424, 154);
            this.list_log.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 293);
            this.Controls.Add(this.list_log);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.btn_Start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.ListBox list_log;
    }
}

