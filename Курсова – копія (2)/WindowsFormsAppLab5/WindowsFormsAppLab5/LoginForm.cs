using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp;
using WindowsFormsAppLab2.Converting;

namespace WindowsFormsAppLab2
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            var username = usernameInp.Text;
            var password = passwordInp.Text;

            var perm = Authenticate(username, password);
            if (perm != null)
            {
                new Form1((Permission)perm).Show();
                this.Hide();
            }
        }

        private Permission? Authenticate(string username, string password)
        {
            var user = DbUtils.GetUser(username, password);
            if (user == null)
            {
                MessageBox.Show("Username or password is incorrect", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                return user.Permission.GetPermission();
            }
            return null;
        }

    }
}
