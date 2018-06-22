using System;
using System.Windows.Forms;

namespace TaskTrayApplication
{
    public partial class LoginWnd : Form
    {
        private int j = 0;
        private bool lb_test = TaskTrayApplication.Properties.Settings.Default.lb_test;
        public LoginWnd()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.None;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Keys modKey = Control.ModifierKeys;
            int modKey = (int)Control.ModifierKeys;
            if (modKey == 393216) lb_test = true;

            if (Login())
            {
                TaskTrayApplication.Properties.Settings.Default.LoginID = txtBxUserID.Text;
                if (lb_test)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
                //this.Close();
            }
        }

        private bool Login()
        {
            
            bool lb_rtn = false;
            ldapAuthentication ldap = new ldapAuthentication();
            try
            {
                lb_rtn = ldap.AuthenticateUser(txtBxUserID.Text, mTxtBxPassword.Text);
            }
            catch (Exception ex)
            {
                j++;
                if (j < 3)
                {
                    MessageBox.Show("The login failed: " + ex.Message + " Try again.", "Login Failure", MessageBoxButtons.OK);

                    this.Visible = true;
                    this.Focus();
                }
                else
                {
                    MessageBox.Show("The login failed: " + ex.Message + " You have failed 3 login attempts. The application will exit.", "Login Failure", MessageBoxButtons.OK);
                    lb_rtn = false;
                    Application.Exit();
                }

            }

            return lb_rtn;
        }

        private void LoginWnd_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)// || (e.Control == true && e.KeyCode == Keys.Enter))
            {
                if (e.Control == true && e.Alt == true && e.KeyCode == Keys.Enter) lb_test = true;
                if (Login())
                {
                    TaskTrayApplication.Properties.Settings.Default.LoginID = txtBxUserID.Text;
                    if (lb_test)
                    {
                        this.DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                //this.Close();
            }
        }

        private void LoginWnd_MouseClick(object sender, MouseEventArgs e)
        {
            Keys modKey = Control.ModifierKeys; 
            if(modKey == Keys.Control) 
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.X >= 237 && e.Y >= 247)
                    {
                        chkBxTest.Visible = !chkBxTest.Visible;
                    }
                }
            }
        }

        private void chkBxTest_CheckedChanged(object sender, EventArgs e)
        {
            lb_test = chkBxTest.Checked;
        }
    }
}