namespace TaskTrayApplication
{
    partial class AKcomSettings
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstBxAgents = new System.Windows.Forms.ComboBox();
            this.txtBxServerPath = new System.Windows.Forms.TextBox();
            this.txtBxSearchPath = new System.Windows.Forms.TextBox();
            this.txtBxUserAttr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(74, 172);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(212, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Agent:";
            // 
            // lstBxAgents
            // 
            this.lstBxAgents.AllowDrop = true;
            this.lstBxAgents.FormattingEnabled = true;
            this.lstBxAgents.Location = new System.Drawing.Point(12, 31);
            this.lstBxAgents.Name = "lstBxAgents";
            this.lstBxAgents.Size = new System.Drawing.Size(339, 21);
            this.lstBxAgents.TabIndex = 5;
            this.lstBxAgents.SelectedIndexChanged += new System.EventHandler(this.lstBxAgents_SelectedIndexChanged);
            // 
            // txtBxServerPath
            // 
            this.txtBxServerPath.Location = new System.Drawing.Point(12, 76);
            this.txtBxServerPath.Name = "txtBxServerPath";
            this.txtBxServerPath.Size = new System.Drawing.Size(154, 20);
            this.txtBxServerPath.TabIndex = 6;
            // 
            // txtBxSearchPath
            // 
            this.txtBxSearchPath.Location = new System.Drawing.Point(197, 76);
            this.txtBxSearchPath.Name = "txtBxSearchPath";
            this.txtBxSearchPath.Size = new System.Drawing.Size(154, 20);
            this.txtBxSearchPath.TabIndex = 7;
            // 
            // txtBxUserAttr
            // 
            this.txtBxUserAttr.Location = new System.Drawing.Point(101, 126);
            this.txtBxUserAttr.Name = "txtBxUserAttr";
            this.txtBxUserAttr.Size = new System.Drawing.Size(154, 20);
            this.txtBxUserAttr.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Server Path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(194, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Search Path:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(98, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "User Attr:";
            // 
            // AKcomSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 216);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBxUserAttr);
            this.Controls.Add(this.txtBxSearchPath);
            this.Controls.Add(this.txtBxServerPath);
            this.Controls.Add(this.lstBxAgents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AKcomSettings";
            this.ShowInTaskbar = false;
            this.Text = "AKcomSettings";
            this.Load += new System.EventHandler(this.AKcomSettings_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AKcomSettings_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstBxAgents;
        private System.Windows.Forms.TextBox txtBxServerPath;
        private System.Windows.Forms.TextBox txtBxSearchPath;
        private System.Windows.Forms.TextBox txtBxUserAttr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}