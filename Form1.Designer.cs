namespace TelstratTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtBoxLogging = new System.Windows.Forms.TextBox();
            this.lBx1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtBxPayload = new System.Windows.Forms.TextBox();
            this.txtBxCipher = new System.Windows.Forms.TextBox();
            this.txtBxClientID = new System.Windows.Forms.TextBox();
            this.txtBxServerID = new System.Windows.Forms.TextBox();
            this.txtBxRecieverKeyVer = new System.Windows.Forms.TextBox();
            this.txtBxSenderKeyVer = new System.Windows.Forms.TextBox();
            this.txtBxSignature = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAssemble = new System.Windows.Forms.Button();
            this.btnDisassemble = new System.Windows.Forms.Button();
            this.txtBxAddParms = new System.Windows.Forms.TextBox();
            this.txtBxEmpID = new System.Windows.Forms.TextBox();
            this.txtBxLoginID = new System.Windows.Forms.TextBox();
            this.txtBxTransID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxLogging
            // 
            this.txtBoxLogging.Location = new System.Drawing.Point(429, 84);
            this.txtBoxLogging.Multiline = true;
            this.txtBoxLogging.Name = "txtBoxLogging";
            this.txtBoxLogging.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxLogging.Size = new System.Drawing.Size(276, 316);
            this.txtBoxLogging.TabIndex = 1;
            // 
            // lBx1
            // 
            this.lBx1.FormattingEnabled = true;
            this.lBx1.Location = new System.Drawing.Point(20, 84);
            this.lBx1.Name = "lBx1";
            this.lBx1.Size = new System.Drawing.Size(375, 316);
            this.lBx1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(165, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(297, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtBxPayload
            // 
            this.txtBxPayload.Location = new System.Drawing.Point(20, 432);
            this.txtBxPayload.Multiline = true;
            this.txtBxPayload.Name = "txtBxPayload";
            this.txtBxPayload.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBxPayload.Size = new System.Drawing.Size(375, 56);
            this.txtBxPayload.TabIndex = 5;
            // 
            // txtBxCipher
            // 
            this.txtBxCipher.Location = new System.Drawing.Point(429, 432);
            this.txtBxCipher.Multiline = true;
            this.txtBxCipher.Name = "txtBxCipher";
            this.txtBxCipher.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBxCipher.Size = new System.Drawing.Size(276, 56);
            this.txtBxCipher.TabIndex = 6;
            // 
            // txtBxClientID
            // 
            this.txtBxClientID.Location = new System.Drawing.Point(20, 512);
            this.txtBxClientID.Name = "txtBxClientID";
            this.txtBxClientID.Size = new System.Drawing.Size(182, 20);
            this.txtBxClientID.TabIndex = 7;
            // 
            // txtBxServerID
            // 
            this.txtBxServerID.Location = new System.Drawing.Point(20, 561);
            this.txtBxServerID.Name = "txtBxServerID";
            this.txtBxServerID.Size = new System.Drawing.Size(182, 20);
            this.txtBxServerID.TabIndex = 8;
            // 
            // txtBxRecieverKeyVer
            // 
            this.txtBxRecieverKeyVer.Location = new System.Drawing.Point(20, 608);
            this.txtBxRecieverKeyVer.Name = "txtBxRecieverKeyVer";
            this.txtBxRecieverKeyVer.Size = new System.Drawing.Size(100, 20);
            this.txtBxRecieverKeyVer.TabIndex = 9;
            this.txtBxRecieverKeyVer.Text = "1.0";
            // 
            // txtBxSenderKeyVer
            // 
            this.txtBxSenderKeyVer.Location = new System.Drawing.Point(20, 655);
            this.txtBxSenderKeyVer.Name = "txtBxSenderKeyVer";
            this.txtBxSenderKeyVer.Size = new System.Drawing.Size(100, 20);
            this.txtBxSenderKeyVer.TabIndex = 10;
            this.txtBxSenderKeyVer.Text = "1.0";
            // 
            // txtBxSignature
            // 
            this.txtBxSignature.Location = new System.Drawing.Point(429, 512);
            this.txtBxSignature.Multiline = true;
            this.txtBxSignature.Name = "txtBxSignature";
            this.txtBxSignature.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBxSignature.Size = new System.Drawing.Size(276, 69);
            this.txtBxSignature.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Payload";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 496);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "ClientID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 545);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "ServerID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 592);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "RecieverKeyVersion";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 639);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "SenderKeyVersion";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(426, 416);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Cipher";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(426, 496);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Signature";
            // 
            // btnAssemble
            // 
            this.btnAssemble.Location = new System.Drawing.Point(320, 608);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(75, 23);
            this.btnAssemble.TabIndex = 19;
            this.btnAssemble.Text = "Assemble";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // btnDisassemble
            // 
            this.btnDisassemble.Location = new System.Drawing.Point(429, 608);
            this.btnDisassemble.Name = "btnDisassemble";
            this.btnDisassemble.Size = new System.Drawing.Size(75, 23);
            this.btnDisassemble.TabIndex = 20;
            this.btnDisassemble.Text = "Disassemble";
            this.btnDisassemble.UseVisualStyleBackColor = true;
            this.btnDisassemble.Click += new System.EventHandler(this.btnDisassemble_Click);
            // 
            // txtBxAddParms
            // 
            this.txtBxAddParms.Location = new System.Drawing.Point(757, 512);
            this.txtBxAddParms.Multiline = true;
            this.txtBxAddParms.Name = "txtBxAddParms";
            this.txtBxAddParms.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBxAddParms.Size = new System.Drawing.Size(276, 69);
            this.txtBxAddParms.TabIndex = 21;
            // 
            // txtBxEmpID
            // 
            this.txtBxEmpID.Location = new System.Drawing.Point(757, 432);
            this.txtBxEmpID.Name = "txtBxEmpID";
            this.txtBxEmpID.Size = new System.Drawing.Size(182, 20);
            this.txtBxEmpID.TabIndex = 22;
            // 
            // txtBxLoginID
            // 
            this.txtBxLoginID.Location = new System.Drawing.Point(757, 468);
            this.txtBxLoginID.Name = "txtBxLoginID";
            this.txtBxLoginID.Size = new System.Drawing.Size(182, 20);
            this.txtBxLoginID.TabIndex = 23;
            // 
            // txtBxTransID
            // 
            this.txtBxTransID.Location = new System.Drawing.Point(757, 608);
            this.txtBxTransID.Name = "txtBxTransID";
            this.txtBxTransID.Size = new System.Drawing.Size(182, 20);
            this.txtBxTransID.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(754, 416);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "EmpID";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(754, 496);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Addl Parms";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(754, 452);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "LoginID";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(754, 592);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "TransactionID";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 724);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtBxTransID);
            this.Controls.Add(this.txtBxLoginID);
            this.Controls.Add(this.txtBxEmpID);
            this.Controls.Add(this.txtBxAddParms);
            this.Controls.Add(this.btnDisassemble);
            this.Controls.Add(this.btnAssemble);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBxSignature);
            this.Controls.Add(this.txtBxSenderKeyVer);
            this.Controls.Add(this.txtBxRecieverKeyVer);
            this.Controls.Add(this.txtBxServerID);
            this.Controls.Add(this.txtBxClientID);
            this.Controls.Add(this.txtBxCipher);
            this.Controls.Add(this.txtBxPayload);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lBx1);
            this.Controls.Add(this.txtBoxLogging);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBoxLogging;
        private System.Windows.Forms.ListBox lBx1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtBxPayload;
        private System.Windows.Forms.TextBox txtBxCipher;
        private System.Windows.Forms.TextBox txtBxClientID;
        private System.Windows.Forms.TextBox txtBxServerID;
        private System.Windows.Forms.TextBox txtBxRecieverKeyVer;
        private System.Windows.Forms.TextBox txtBxSenderKeyVer;
        private System.Windows.Forms.TextBox txtBxSignature;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.Button btnDisassemble;
        private System.Windows.Forms.TextBox txtBxAddParms;
        private System.Windows.Forms.TextBox txtBxEmpID;
        private System.Windows.Forms.TextBox txtBxLoginID;
        private System.Windows.Forms.TextBox txtBxTransID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}

