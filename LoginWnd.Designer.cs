namespace TaskTrayApplication
{
    partial class LoginWnd
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
            this.txtBxUserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mTxtBxPassword = new System.Windows.Forms.TextBox();
            this.chkBxTest = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtBxUserID
            // 
            this.txtBxUserID.Location = new System.Drawing.Point(81, 69);
            this.txtBxUserID.Name = "txtBxUserID";
            this.txtBxUserID.Size = new System.Drawing.Size(138, 20);
            this.txtBxUserID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "LDAP UserID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(62, 206);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(155, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // mTxtBxPassword
            // 
            this.mTxtBxPassword.Location = new System.Drawing.Point(81, 137);
            this.mTxtBxPassword.Name = "mTxtBxPassword";
            this.mTxtBxPassword.PasswordChar = '*';
            this.mTxtBxPassword.Size = new System.Drawing.Size(138, 20);
            this.mTxtBxPassword.TabIndex = 2;
            this.mTxtBxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginWnd_KeyDown);
            // 
            // chkBxTest
            // 
            this.chkBxTest.AutoSize = true;
            this.chkBxTest.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBxTest.Location = new System.Drawing.Point(237, 247);
            this.chkBxTest.Name = "chkBxTest";
            this.chkBxTest.Size = new System.Drawing.Size(53, 17);
            this.chkBxTest.TabIndex = 5;
            this.chkBxTest.Text = "Test?";
            this.chkBxTest.UseVisualStyleBackColor = true;
            this.chkBxTest.Visible = false;
            this.chkBxTest.CheckedChanged += new System.EventHandler(this.chkBxTest_CheckedChanged);
            // 
            // LoginWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.chkBxTest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBxUserID);
            this.Controls.Add(this.mTxtBxPassword);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginWnd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LDAP Login";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoginWnd_MouseClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginWnd_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBxUserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox mTxtBxPassword;
        private System.Windows.Forms.CheckBox chkBxTest;
    }
}