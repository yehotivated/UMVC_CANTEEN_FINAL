using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class CreateAccount : Form
    {
        private Form _parentForm;

        public CreateAccount(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            cmbRole.Items.AddRange(new string[] { "Admin", "Clerk" , "Cashier" , "User"});
            cmbRole.SelectedIndex = 0;

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string password = SecurityHelper.HashPassword(txtPassword.Text.Trim());
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            UsersDAL.AddUser(
                txtUsername.Text.Trim(),
                txtPassword.Text.Trim(),
                cmbRole.Text
            );

            // REAL-TIME refresh
            if (_parentForm is adminUsers admin)
                admin.LoadUsers();

            MessageBox.Show("Account created successfully!");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
    

